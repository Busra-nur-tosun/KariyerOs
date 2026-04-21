using Domain.Modules.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(token => token.Id);
        builder.Property(token => token.TokenHash).HasMaxLength(256).IsRequired();
        builder.Property(token => token.CreatedByIpAddress).HasMaxLength(64).IsRequired();
        builder.Property(token => token.RevokedByIpAddress).HasMaxLength(64);
        builder.Property(token => token.ReplacedByTokenHash).HasMaxLength(256);
        builder.Property(token => token.CreatedAtUtc).IsRequired();
        builder.Property(token => token.ExpiresAtUtc).IsRequired();

        builder.HasIndex(token => token.TokenHash).IsUnique();
    }
}
