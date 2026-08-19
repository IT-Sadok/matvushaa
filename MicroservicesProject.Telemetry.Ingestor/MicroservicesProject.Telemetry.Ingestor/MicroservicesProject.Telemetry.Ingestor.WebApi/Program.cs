using MicroservicesProject.Telemetry.Ingestor;
using MicroservicesProject.Telemetry.Ingestor.ConfigurationModels;
using MicroservicesProject.Telemetry.Ingestor.Core.Interfaces;
using MicroservicesProject.Telemetry.Ingestor.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.Configure<MqttOptionsConfigurationModel>(
    builder.Configuration.GetSection(MqttOptionsConfigurationModel.SectionName));

builder.Services.Configure<HostOptions>(options =>
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);

builder.Services.AddControllers();
builder.Services.AddSingleton<ITelemetryBuffer, ChannelTelemetryBuffer>();
builder.Services.AddTransient<ITelemetryPublisher, MockTelemetryPublisher>();
builder.Services.AddTransient<TelemetryIngestionService>();
builder.Services.AddHostedService<TelemetryForwardingWorker>();
builder.Services.AddHostedService<MqttListenerWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapControllers();

app.Run();