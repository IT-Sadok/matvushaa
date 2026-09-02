using System.Diagnostics.Metrics;
using System.Threading.Channels;
using MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;

namespace MicroservicesProject.Telemetry.Ingestor.Core.Services;

public sealed class ChannelTelemetryBuffer : ITelemetryBuffer, IDisposable
{
    public const string MeterName = "MicroservicesProject.Telemetry.Ingestor";

    private const int Capacity = 100_000;

    private readonly Channel<TelemetryPayload> _channel;
    private readonly Meter _meter;
    private readonly Counter<long> _rejectedCounter;

    public ChannelTelemetryBuffer()
    {
        var options = new BoundedChannelOptions(Capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,

            SingleReader = true,
            SingleWriter = false
        };

        _channel = Channel.CreateBounded<TelemetryPayload>(options);

        _meter = new Meter(MeterName);
        _rejectedCounter = _meter.CreateCounter<long>(
            "telemetry.buffer.rejected",
            unit: "{payload}",
            description: "Telemetry payloads rejected because the ingestion buffer was full.");
    }

    public bool TryWrite(TelemetryPayload payload)
    {
        if (_channel.Writer.TryWrite(payload))
        {
            return true;
        }

        _rejectedCounter.Add(1);
        return false;
    }

    public IAsyncEnumerable<TelemetryPayload> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }

    public void Dispose()
    {
        _meter.Dispose();
    }
}