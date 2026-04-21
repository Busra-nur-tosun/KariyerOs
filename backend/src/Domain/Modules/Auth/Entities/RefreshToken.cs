using Domain.Common;

namespace Domain.Modules.Auth.Entities;

public sealed class RefreshToken : AuditableEntity
{
    private RefreshToken()
    {
    }

    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }
    public string CreatedByIpAddress { get; private set; } = string.Empty;
    public string? RevokedByIpAddress { get; private set; }

    public bool IsActive => RevokedAtUtc is null && ExpiresAtUtc > DateTime.UtcNow;

    public static RefreshToken Create(
        Guid userId,
        string tokenHash,
        DateTime expiresAtUtc,
        string createdByIpAddress)
    {
        return new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAtUtc = expiresAtUtc,
            CreatedByIpAddress = createdByIpAddress
        };
    }

    public void Revoke(DateTime revokedAtUtc, string revokedByIpAddress, string? replacedByTokenHash = null)
    {
        RevokedAtUtc = revokedAtUtc;
        RevokedByIpAddress = revokedByIpAddress;
        ReplacedByTokenHash = replacedByTokenHash;
        MarkAsUpdated(revokedAtUtc);
    }
}
