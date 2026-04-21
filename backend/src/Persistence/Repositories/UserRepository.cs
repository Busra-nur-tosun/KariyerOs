using Application.Common.Interfaces;
using Domain.Modules.Users.Entities;
using Microsoft.EntityFrameworkCore;
using RefreshTokenEntity = Domain.Modules.Auth.Entities.RefreshToken;

namespace Persistence.Repositories;

public sealed class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public Task<bool> ExistsByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) =>
        dbContext.Users.AnyAsync(user => user.NormalizedEmail == normalizedEmail, cancellationToken);

    public Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) =>
        dbContext.Users.Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.NormalizedEmail == normalizedEmail, cancellationToken);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Users.SingleOrDefaultAsync(user => user.Id == id, cancellationToken);

    public Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken) =>
        dbContext.Users.Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.RefreshTokens.Any(token => token.TokenHash == refreshTokenHash), cancellationToken);

    public Task AddAsync(User user, CancellationToken cancellationToken) =>
        dbContext.Users.AddAsync(user, cancellationToken).AsTask();

    public Task AddRefreshTokenAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken) =>
        dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken).AsTask();

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
