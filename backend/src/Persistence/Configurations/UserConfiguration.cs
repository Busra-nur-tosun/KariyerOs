using Domain.Modules.Users.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);
        builder.Property(user => user.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(user => user.LastName).HasMaxLength(100).IsRequired();
        builder.Property(user => user.Email).HasMaxLength(320).IsRequired();
        builder.Property(user => user.NormalizedEmail).HasMaxLength(320).IsRequired();
        builder.Property(user => user.PasswordHash).HasMaxLength(512).IsRequired();
        builder.Property(user => user.Role).HasMaxLength(50).IsRequired();
        builder.Property(user => user.IsActive).IsRequired();
        builder.Property(user => user.CreatedAtUtc).IsRequired();

        builder.HasIndex(user => user.NormalizedEmail).IsUnique();

        builder.HasMany(user => user.RefreshTokens)
            .WithOne()
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        var refreshTokensNavigation = builder.Metadata.FindNavigation(nameof(User.RefreshTokens));

        if (refreshTokensNavigation is not null)
        {
            refreshTokensNavigation.SetField("_refreshTokens");
            refreshTokensNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
