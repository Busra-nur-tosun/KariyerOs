using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace API.Endpoints;

public static class HealthEndpointExtensions
{
    public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true
        });

        return endpoints;
    }
}
