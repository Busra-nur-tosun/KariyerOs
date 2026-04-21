namespace Application.Modules.Auth.Models;

public sealed record AuthResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    string AccessToken,
    DateTime AccessTokenExpiresAtUtc,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc);
