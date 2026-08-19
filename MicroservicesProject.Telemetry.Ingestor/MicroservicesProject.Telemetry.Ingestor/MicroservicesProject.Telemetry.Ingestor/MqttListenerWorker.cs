using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using HiveMQtt.Client;
using HiveMQtt.Client.Events;
using HiveMQtt.Client.Options;
using MicroservicesProject.Telemetry.Ingestor.ConfigurationModels;
using MicroservicesProject.Telemetry.Ingestor.Core;
using MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicroservicesProject.Telemetry.Ingestor;

public class MqttListenerWorker(
    ITelemetryBuffer buffer, 
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
                        Validator.ValidateObject(_options, new ValidationContext(_options), validateAllProperties: true);

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

            if (payload is { DeviceId.Length: > 0 })
            {
                bool isWritten = buffer.TryWrite(payload);

                if (!isWritten)
                {
                    logger.LogWarning("Buffer is full. Rejected metric for device: {DeviceId}", payload.DeviceId);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to deserialize payload from topic: {Topic}", args.PublishMessage.Topic);
        }
    }
}