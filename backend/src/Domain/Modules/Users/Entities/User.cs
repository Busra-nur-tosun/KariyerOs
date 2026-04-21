using Domain.Common;
using Domain.Modules.Auth.Entities;
using Domain.Modules.Users.Enums;

namespace Domain.Modules.Users.Entities;

public sealed class User : AuditableEntity
{
    private readonly List<RefreshToken> _refreshTokens = [];

    private User()
    {
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string NormalizedEmail { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = SystemRoles.Candidate;
    public bool IsActive { get; private set; } = true;

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        string role)
    {
        if (!SystemRoles.All.Contains(role))
        {
            throw new ArgumentException("Invalid user role.", nameof(role));
        }

        return new User
        {
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.Trim(),
            NormalizedEmail = email.Trim().ToUpperInvariant(),
            PasswordHash = passwordHash,
            Role = role
        };
    }

    public void AddRefreshToken(RefreshToken refreshToken, DateTime updatedAtUtc)
    {
        _refreshTokens.Add(refreshToken);
        MarkAsUpdated(updatedAtUtc);
    }

    public void RemoveExpiredRefreshTokens(DateTime currentUtc)
    {
        _refreshTokens.RemoveAll(token => token.ExpiresAtUtc <= currentUtc && token.RevokedAtUtc is not null);
    }
}
