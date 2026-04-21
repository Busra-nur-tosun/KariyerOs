using Domain.Modules.Users.Entities;
using Domain.Modules.Users.Enums;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Seed;

public static class DevelopmentIdentitySeeder
{
    public static async Task SeedAsync(AppDbContext dbContext, string adminPasswordHash, CancellationToken cancellationToken)
    {
        if (await dbContext.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var adminUser = User.Create("System", "Admin", "admin@kariyeros.local", adminPasswordHash, SystemRoles.Admin);
        await dbContext.Users.AddAsync(adminUser, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
