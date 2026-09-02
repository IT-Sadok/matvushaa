namespace MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;

public interface ITelemetryBuffer
{
    bool TryWrite(TelemetryPayload payload);

    IAsyncEnumerable<TelemetryPayload> ReadAllAsync(CancellationToken cancellationToken);
}