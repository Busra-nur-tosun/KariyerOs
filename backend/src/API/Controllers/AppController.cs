using Application.Common.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
// Bu controller temel "uygulama ayakta mi?" bilgisini donen basit bir ornek endpoint sunar.
// Gercek business endpointleri geldikce feature bazli controller veya minimal API tercih edilebilir.
public sealed class AppController(ISender sender, IWebHostEnvironment environment) : ControllerBase
{
    [HttpGet("info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInfo(CancellationToken cancellationToken)
    {
        // MediatR kullanarak controller'i handler detayindan ayiriyoruz.
        // Bu sayede controller sadece HTTP orchestration isi yapar.
        var response = await sender.Send(new GetApplicationInfoQuery(environment.EnvironmentName), cancellationToken);
        return Ok(response);
    }
}
