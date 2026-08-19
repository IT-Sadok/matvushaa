using MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;

namespace MicroservicesProject.Telemetry.Ingestor.Core.Services;

public class TelemetryIngestionService(ITelemetryBuffer buffer)
{
    public bool Ingest(TelemetryPayload payload)
    {
        if (payload.DeviceId is not { Length: > 0 })
        {
            return false;
        }

        if (payload.Voltage < 0 || payload.Power < 0)
        {
            return false;
        }

        return buffer.TryWrite(payload);
    }
}