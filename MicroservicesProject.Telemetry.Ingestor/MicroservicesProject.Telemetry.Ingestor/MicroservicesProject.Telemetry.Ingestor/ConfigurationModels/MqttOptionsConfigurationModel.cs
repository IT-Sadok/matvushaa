using System.ComponentModel.DataAnnotations;

namespace MicroservicesProject.Telemetry.Ingestor.ConfigurationModels;

public class MqttOptionsConfigurationModel
{
    public const string SectionName = "Mqtt";

    [Required]
    public string Host { get; set; } = null!;

    [Range(1, 65535)]
    public int Port { get; set; }

    [Required]
    public string ClientIdPrefix { get; set; } = null!;
}