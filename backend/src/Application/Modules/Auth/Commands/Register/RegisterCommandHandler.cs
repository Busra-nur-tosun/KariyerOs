using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Modules.Auth.Models;
using Domain.Modules.Users.Entities;
using MediatR;
using RefreshTokenEntity = Domain.Modules.Auth.Entities.RefreshToken;

namespace Application.Modules.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IRefreshTokenGenerator refreshTokenGenerator,
    IDateTimeProvider dateTimeProvider,
    IAuthOptions authOptions) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();

        if (await userRepository.ExistsByEmailAsync(normalizedEmail, cancellationToken))
        {
            throw new ConflictException("A user with this email already exists.");
        }

        var user = User.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            passwordHasher.HashPassword(request.Password),
            request.Role);

        var rawRefreshToken = refreshTokenGenerator.Generate();
        var refreshToken = RefreshTokenEntity.Create(
            user.Id,
            refreshTokenGenerator.Hash(rawRefreshToken),
            dateTimeProvider.UtcNow.AddDays(authOptions.RefreshTokenExpirationDays),
            request.IpAddress);

        user.AddRefreshToken(refreshToken, dateTimeProvider.UtcNow);

        await userRepository.AddAsync(user, cancellationToken);
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
