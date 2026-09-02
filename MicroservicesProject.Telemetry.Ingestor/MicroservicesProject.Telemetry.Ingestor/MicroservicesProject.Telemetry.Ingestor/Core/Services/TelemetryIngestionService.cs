using MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;

namespace MicroservicesProject.Telemetry.Ingestor.Core.Services;

public class TelemetryIngestionService(ITelemetryBuffer buffer)
{
    public TelemetryIngestResult Ingest(TelemetryPayload payload)
    {
        if (payload.DeviceId is not { Length: > 0 })
        {
            return TelemetryIngestResult.Rejected;
        }

        if (payload.Voltage < 0 || payload.Power < 0)
        {
            return TelemetryIngestResult.Rejected;
        }

        return buffer.TryWrite(payload)
            ? TelemetryIngestResult.Accepted
            : TelemetryIngestResult.BufferFull;
    }
}