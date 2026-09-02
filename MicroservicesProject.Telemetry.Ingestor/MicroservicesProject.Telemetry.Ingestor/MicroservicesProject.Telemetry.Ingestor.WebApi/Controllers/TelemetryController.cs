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
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult Ingest([FromBody] TelemetryPayload payload)
    {
        if (payload is not { DeviceId: { Length: > 0 } })
        {
            return BadRequest(new { Error = "DeviceId is required." });
        }

        return ingestionService.Ingest(payload) switch
        {
            TelemetryIngestResult.Accepted => Accepted(),
            TelemetryIngestResult.BufferFull => BufferFull(),
            _ => UnprocessableEntity(new { Error = "The data contain abnormal indicators and have been rejected." })
        };
    }

    private IActionResult BufferFull()
    {
        Response.Headers.RetryAfter = "1";
        return StatusCode(
            StatusCodes.Status503ServiceUnavailable,
            new { Error = "Ingestion buffer is full. Retry later." });
    }
}