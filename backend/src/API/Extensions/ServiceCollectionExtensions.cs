using API.Common.Exceptions;
using API.Common.Options;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    // API katmanina ait framework servisleri burada toplanir.
    // Program.cs kisa kalir, web ile ilgili tum kurulumlar tek yerde okunur.
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        var googleAuthOptions = configuration.GetSection(GoogleAuthOptions.SectionName).Get<GoogleAuthOptions>() ?? new GoogleAuthOptions();
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();

        // Controller tabanli API kullaniyoruz.
        services.AddControllers();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
        services.AddSwaggerGen();
        services.Configure<FrontendOptions>(configuration.GetSection(FrontendOptions.SectionName));
        services.Configure<GoogleAuthOptions>(configuration.GetSection(GoogleAuthOptions.SectionName));
        services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Health check veritabanina ulasilip ulasilamadigini da test eder.
        // Uygulama izleme ve deployment dogrulamalarinda faydalidir.
        services.AddHealthChecks().AddDbContextCheck<AppDbContext>("database");

        // JWT ayarlari configuration'dan okunur.
        // Boylece token mantigi eklendiginde kod degil sadece config degiserek ortamlar ayrisabilir.
        var secretKey = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

        var authenticationBuilder = services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
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
            })
            .AddCookie("External", options =>
            {
                options.Cookie.Name = "kariyeros.external";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            });

        if (googleAuthOptions.IsConfigured)
        {
            authenticationBuilder.AddGoogle("Google", options =>
            {
                options.SignInScheme = "External";
                options.ClientId = googleAuthOptions.ClientId;
                options.ClientSecret = googleAuthOptions.ClientSecret;
            });
        }

        // Policy'ler ileride burada veya ayri bir authorization extension'inda buyutulebilir.
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}
