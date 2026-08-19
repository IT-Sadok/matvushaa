namespace MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;

public interface ITelemetryPublisher
{
    Task PublishAsync(TelemetryPayload payload, CancellationToken cancellationToken);
}