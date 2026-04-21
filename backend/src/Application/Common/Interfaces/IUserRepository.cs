using Domain.Modules.Users.Entities;
using RefreshTokenEntity = Domain.Modules.Auth.Entities.RefreshToken;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task AddRefreshTokenAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
