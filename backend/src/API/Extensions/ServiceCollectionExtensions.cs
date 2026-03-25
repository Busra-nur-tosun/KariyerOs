using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    // API katmanina ait framework servisleri burada toplanir.
    // Program.cs kisa kalir, web ile ilgili tum kurulumlar tek yerde okunur.
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Controller tabanli API kullaniyoruz.
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
        services.AddSwaggerGen();

        // Health check veritabanina ulasilip ulasilamadigini da test eder.
        // Uygulama izleme ve deployment dogrulamalarinda faydalidir.
        services.AddHealthChecks().AddDbContextCheck<AppDbContext>("database");

        // JWT ayarlari configuration'dan okunur.
        // Boylece token mantigi eklendiginde kod degil sadece config degiserek ortamlar ayrisabilir.
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
        var secretKey = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Simdiden dogrulama kurallari tanimli.
                // Auth endpointleri eklendiginde API hazir olacak.
                options.RequireHttpsMetadata = true;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        // Policy'ler ileride burada veya ayri bir authorization extension'inda buyutulebilir.
        services.AddAuthorization();

        return services;
    }
}
