using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    // Infrastructure katmani veritabani, auth ayarlari ve dis bagimliliklari uygular.
    // API katmani sadece bu extension'i cagirir; detaylari bilmek zorunda kalmaz.
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Connection string bulunamazsa uygulama erken fail eder.
        // Bu davranis production'da sessiz yanlis konfigurasyonu engeller.
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

        // AppDbContext uygulama boyunca scoped lifetime ile kullanilir.
        // Her request icin ayri context olusmasi EF Core'un varsayilan ve guvenli kullanimidir.
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services
            // JWT options simdilik sadece konfigurasyon olarak hazir.
            // Token uretimi ileride eklense bile ayarlar merkezi olarak burada okunacak.
            .Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        // IOptions wrapper'ina bagimli kalmamak icin sade JwtOptions erisimi de aciliyor.
        services.AddSingleton(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value);

        return services;
    }
}
