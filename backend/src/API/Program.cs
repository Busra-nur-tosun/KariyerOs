using API.Endpoints;
using API.Extensions;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Katman bagimliliklari tek bir composition root uzerinden baglaniyor.
// Bu dosya uygulamanin nereden neyi yukledigini net gosteren ana giris noktasi.
builder.Services.AddApplication();
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
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Health endpoint altyapi durumunu izlemek icin,
// controllers ise business/API endpointleri icin kullanilir.
app.MapHealthEndpoints();
app.MapControllers();

app.Run();

public partial class Program;
