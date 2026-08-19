using System.Threading.Channels;
using MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;

namespace MicroservicesProject.Telemetry.Ingestor.Core.Services;

public class ChannelTelemetryBuffer : ITelemetryBuffer
{
    private readonly Channel<TelemetryPayload> _channel;

    public ChannelTelemetryBuffer()
    {
        var options = new BoundedChannelOptions(100_000)
        {
            FullMode = BoundedChannelFullMode.DropOldest,

            SingleReader = true,
            SingleWriter = false
        };

        _channel = Channel.CreateBounded<TelemetryPayload>(options);
    }

    public bool TryWrite(TelemetryPayload payload)
    {
        return _channel.Writer.TryWrite(payload);
    }

    public IAsyncEnumerable<TelemetryPayload> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}