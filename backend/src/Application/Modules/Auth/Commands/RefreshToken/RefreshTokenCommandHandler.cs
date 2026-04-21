using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Modules.Auth.Models;
using MediatR;
using RefreshTokenEntity = Domain.Modules.Auth.Entities.RefreshToken;

namespace Application.Modules.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    ITokenService tokenService,
    IRefreshTokenGenerator refreshTokenGenerator,
    IDateTimeProvider dateTimeProvider,
    IAuthOptions authOptions) : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hashedToken = refreshTokenGenerator.Hash(request.RefreshToken);
        var user = await userRepository.GetByRefreshTokenHashAsync(hashedToken, cancellationToken)
            ?? throw new UnauthorizedException("Refresh token is invalid.");

        var existingToken = user.RefreshTokens.SingleOrDefault(token => token.TokenHash == hashedToken)
            ?? throw new UnauthorizedException("Refresh token is invalid.");

        if (!existingToken.IsActive)
        {
            throw new UnauthorizedException("Refresh token is expired or revoked.");
        }

        var newRawRefreshToken = refreshTokenGenerator.Generate();
        var newHashedRefreshToken = refreshTokenGenerator.Hash(newRawRefreshToken);

        existingToken.Revoke(dateTimeProvider.UtcNow, request.IpAddress, newHashedRefreshToken);

        var replacementToken = RefreshTokenEntity.Create(
            user.Id,
            newHashedRefreshToken,
            dateTimeProvider.UtcNow.AddDays(authOptions.RefreshTokenExpirationDays),
            request.IpAddress);

        user.AddRefreshToken(replacementToken, dateTimeProvider.UtcNow);
        await userRepository.AddRefreshTokenAsync(replacementToken, cancellationToken);
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
            newRawRefreshToken,
            replacementToken.ExpiresAtUtc);
    }
}
