namespace MicroservicesProject.Telemetry.Ingestor.ConfigurationModels;

public class MqttOptionsConfigurationModel
{
    public const string SectionName = "Mqtt";
    
    public string Host { get; set; }
    
    public int Port { get; set; }
    
    public string ClientIdPrefix { get; set; }
}