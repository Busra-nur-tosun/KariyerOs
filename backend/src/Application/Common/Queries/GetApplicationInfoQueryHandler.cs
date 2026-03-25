using Application.Common.Models;
using MediatR;

namespace Application.Common.Queries;

// Bu handler business feature degil; MediatR omurgasinin calistigini gosteren minimal bir ornek.
// Sonraki modullerde her use case benzer sekilde Query/Command + Handler olarak eklenebilir.
public sealed class GetApplicationInfoQueryHandler : IRequestHandler<GetApplicationInfoQuery, ApplicationInfoDto>
{
    // Uygulama ilk aya kalktigi an bir kez tutulur.
    // Endpoint cagrilarinda ayni baslangic zamani donulerek servis uptime benzeri bir bilgi saglanir.
    private static readonly DateTime StartedAtUtc = DateTime.UtcNow;

    public Task<ApplicationInfoDto> Handle(GetApplicationInfoQuery request, CancellationToken cancellationToken)
    {
        // Simdilik sabit modul listesi donuyoruz.
        // Ileride bu bilgi config, feature flags veya assembly scanning ile genisletilebilir.
        var response = new ApplicationInfoDto(
            Name: "KariyerOS API",
            Environment: request.Environment,
            StartedAtUtc: StartedAtUtc,
            Modules:
            [
                "Auth",
                "Users",
                "CVs",
                "Jobs",
                "Applications",
                "AI"
            ]);

        return Task.FromResult(response);
    }
}
