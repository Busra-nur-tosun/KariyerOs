using API.Endpoints;
using API.Extensions;
using Application;
using Infrastructure;
using Persistence;
using Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Katman bagimliliklari tek bir composition root uzerinden baglaniyor.
// Bu dosya uygulamanin nereden neyi yukledigini net gosteren ana giris noktasi.
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Development ortaminda Swagger ve OpenAPI dokumani acik.
    // Production'da bu alanlar ihtiyaca gore ayrica sinirlandirilabilir.
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS redirection ve auth middleware sirasiyla calisir.
// Once request guvenli kanala yonlenir, sonra kimlik dogrulama/authorization devreye girer.
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

// Health endpoint altyapi durumunu izlemek icin,
// controllers ise business/API endpointleri icin kullanilir.
app.MapHealthEndpoints();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<Application.Common.Interfaces.IPasswordHasher>();

    await DevelopmentIdentitySeeder.SeedAsync(
        dbContext,
        passwordHasher.HashPassword("Admin123!"),
        CancellationToken.None);
}

app.Run();

public partial class Program;
