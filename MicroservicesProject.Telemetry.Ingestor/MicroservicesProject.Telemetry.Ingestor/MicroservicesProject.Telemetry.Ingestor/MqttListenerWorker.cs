using System.Text.Json;
using HiveMQtt.Client;
using HiveMQtt.Client.Events;
using HiveMQtt.Client.Options;
using MicroservicesProject.Telemetry.Ingestor.ConfigurationModels;
using MicroservicesProject.Telemetry.Ingestor.Core;
using MicroservicesProject.Telemetry.Ingestor.Core.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicroservicesProject.Telemetry.Ingestor;

public class MqttListenerWorker(
    TelemetryIngestionService ingestionService,
    IOptions<MqttOptionsConfigurationModel> mqttOptions,
    ILogger<MqttListenerWorker> logger) : BackgroundService
{
    private readonly MqttOptionsConfigurationModel _options = mqttOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        HiveMQClient? client = null;

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (client is null)
                    {
                        var clientOptions = new HiveMQClientOptions
                        {
                            Host = _options.Host,
                            Port = _options.Port,
                            ClientId = $"{_options.ClientIdPrefix}_{Guid.NewGuid()}"
                        };

                        client = new HiveMQClient(clientOptions);
                        client.OnMessageReceived += (_, args) => OnMessageReceived(args);
                    }

                    if (!client.IsConnected())
                    {
                        logger.LogInformation("Connecting to MQTT broker at {Host}:{Port}...", _options.Host, _options.Port);
                        await client.ConnectAsync().ConfigureAwait(false);

                        await client.SubscribeAsync("telemetry/#").ConfigureAwait(false);
                        logger.LogInformation("Successfully subscribed to topic: telemetry/#");
                    }
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    logger.LogError(ex, "Failed to connect to MQTT broker. Retrying in 5 seconds...");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
        finally
        {
            if (client is { } connectedClient && connectedClient.IsConnected())
            {
                logger.LogInformation("Disconnecting from MQTT broker...");
                await connectedClient.DisconnectAsync().ConfigureAwait(false);
            }

            client?.Dispose();
        }
    }

    private void OnMessageReceived(OnMessageReceivedEventArgs args)
    {
        try
        {
            var payloadText = args.PublishMessage.PayloadAsString?.Trim('\uFEFF');

            if (string.IsNullOrEmpty(payloadText)) 
                return;

            var payload = JsonSerializer.Deserialize<TelemetryPayload>(
                payloadText,
                JsonSerializerOptions.Web
            );

            if (payload is null)
                return;

            switch (ingestionService.Ingest(payload))
            {
                case TelemetryIngestResult.Rejected:
                    logger.LogWarning("Telemetry payload rejected for device: {DeviceId}", payload.DeviceId);
                    break;
                case TelemetryIngestResult.BufferFull:
                    logger.LogWarning(
                        "Ingestion buffer full; dropped MQTT telemetry for device: {DeviceId}", payload.DeviceId);
                    break;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to deserialize payload from topic: {Topic}", args.PublishMessage.Topic);
        }
    }
}