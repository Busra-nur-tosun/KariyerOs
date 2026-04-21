using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Modules.Auth.Models;
using MediatR;
using RefreshTokenEntity = Domain.Modules.Auth.Entities.RefreshToken;

namespace Application.Modules.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IRefreshTokenGenerator refreshTokenGenerator,
    IDateTimeProvider dateTimeProvider,
    IAuthOptions authOptions) : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken)
            ?? throw new UnauthorizedException("Invalid credentials.");

        if (!user.IsActive || !passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        var rawRefreshToken = refreshTokenGenerator.Generate();
        var refreshToken = RefreshTokenEntity.Create(
            user.Id,
            refreshTokenGenerator.Hash(rawRefreshToken),
            dateTimeProvider.UtcNow.AddDays(authOptions.RefreshTokenExpirationDays),
            request.IpAddress);

        user.AddRefreshToken(refreshToken, dateTimeProvider.UtcNow);
        await userRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
        user.RemoveExpiredRefreshTokens(dateTimeProvider.UtcNow);

        await userRepository.SaveChangesAsync(cancellationToken);

        var accessToken = tokenService.GenerateAccessToken(user);

        return new AuthResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role,
            accessToken.Token,
            accessToken.ExpiresAtUtc,
            rawRefreshToken,
            refreshToken.ExpiresAtUtc);
    }
}
