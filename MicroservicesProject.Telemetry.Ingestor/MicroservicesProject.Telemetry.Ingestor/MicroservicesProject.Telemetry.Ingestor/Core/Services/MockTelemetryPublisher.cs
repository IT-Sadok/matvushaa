using MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace MicroservicesProject.Telemetry.Ingestor.Core.Services;

public class MockTelemetryPublisher(ILogger<MockTelemetryPublisher> logger) : ITelemetryPublisher
{
    public Task PublishAsync(TelemetryPayload payload, CancellationToken cancellationToken)
    {
        logger.LogInformation("[MOCK RABBITMQ] Відправлено: Пристрій {DeviceId} | Потужність {Power}W", 
            payload.DeviceId, payload.Power);
                
        return Task.CompletedTask;
    }
}