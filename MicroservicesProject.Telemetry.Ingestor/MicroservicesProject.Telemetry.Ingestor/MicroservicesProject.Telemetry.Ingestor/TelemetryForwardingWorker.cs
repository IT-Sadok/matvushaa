using MicroservicesProject.Telemetry.Ingestor.Core;
using MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MicroservicesProject.Telemetry.Ingestor;

public class TelemetryForwardingWorker(
    ITelemetryBuffer buffer,
    ITelemetryPublisher publisher,
    ILogger<TelemetryForwardingWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Telemetry background dispatcher started.");

        try
        {
            await foreach (var payload in buffer.ReadAllAsync(cancellationToken))
            {
                await ProcessPayloadAsync(payload, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Telemetry background dispatcher gracefully stopped.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Fatal error in telemetry background dispatcher. Consumer loop terminated.");
        }
    }

    private async Task ProcessPayloadAsync(TelemetryPayload payload, CancellationToken cancellationToken)
    {
        try
        {
            await publisher.PublishAsync(payload, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to publish telemetry payload for Device: {DeviceId}", payload.DeviceId);
        }
    }
}