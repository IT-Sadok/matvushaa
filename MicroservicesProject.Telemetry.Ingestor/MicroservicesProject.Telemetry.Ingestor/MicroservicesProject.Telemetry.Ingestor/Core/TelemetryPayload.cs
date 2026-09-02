namespace MicroservicesProject.Telemetry.Ingestor.Core;

public record TelemetryPayload(string DeviceId, double Voltage, double Current, double Power);