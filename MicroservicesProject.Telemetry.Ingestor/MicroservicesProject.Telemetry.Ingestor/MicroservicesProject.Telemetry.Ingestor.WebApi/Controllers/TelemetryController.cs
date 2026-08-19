using System.Net.Mime;
using MicroservicesProject.Telemetry.Ingestor.Core;
using MicroservicesProject.Telemetry.Ingestor.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesProject.Telemetry.Ingestor.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
public class TelemetryController(TelemetryIngestionService ingestionService) : ControllerBase
{
    [HttpPost("ingest")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Ingest([FromBody] TelemetryPayload payload)
    {
        if (payload is not { DeviceId: { Length: > 0 } })
        {
            return BadRequest(new { Error = "DeviceId is required." });
        }

        bool isAccepted = ingestionService.Ingest(payload);

        if (isAccepted)
        {
            return Accepted();
        }

        return UnprocessableEntity(new { Error = "The data contain abnormal indicators and have been rejected." });
    }
}