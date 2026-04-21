using Application.Common.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Common;
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
        services
            // JWT options simdilik sadece konfigurasyon olarak hazir.
            // Token uretimi ileride eklense bile ayarlar merkezi olarak burada okunacak.
            .Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        // IOptions wrapper'ina bagimli kalmamak icin sade JwtOptions erisimi de aciliyor.
        services.AddSingleton(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value);

        services.AddSingleton<IAuthOptions, AuthOptionsAccessor>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasherAdapter>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}
