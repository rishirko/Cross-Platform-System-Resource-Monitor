using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RishiProject.App;
using RishiProject.Core.Interface;
using RishiProject.Infrastructure;
using RishiProject.Plugins;
using System.Runtime.InteropServices;
using Polly;

var config = new ConfigurationBuilder()
    .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
    .Build();

var settings = config.Get<AppSettings>();

var services = new ServiceCollection();

/// <summary>
/// Configures logging for the application.
/// </summary>
services.AddLogging(builder =>
{
    builder.AddConsole(); 
    builder.SetMinimumLevel(LogLevel.Information); 
});

/// <summary>
/// Registers application settings as a singleton service.
/// </summary>
services.AddSingleton(settings);

/// <summary>
/// Registers the appropriate system monitor service based on the operating system.
/// </summary>
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    services.AddSingleton<IMonitorService, WindowsSystemMonitor>();
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    services.AddSingleton<IMonitorService, LinuxSystemMonitor>();
else
    throw new PlatformNotSupportedException("Unsupported OS platform.");

/// <summary>
/// Registers the FileLoggerPlugin as a singleton service.
/// </summary>
services.AddSingleton<IMonitorPlugin, FileLoggerPlugin>();

/// <summary>
/// Configures the MonitorRunner service with its dependencies.
/// </summary>
services.AddSingleton<MonitorRunner>(provider =>
{
    var monitorService = provider.GetRequiredService<IMonitorService>();
    var plugins = provider.GetServices<IMonitorPlugin>();
    var logger = provider.GetRequiredService<ILogger<MonitorRunner>>();
    var appSettings = provider.GetRequiredService<AppSettings>(); // Assuming it's already registered

    return new MonitorRunner(monitorService, plugins, logger, appSettings.IntervalSeconds);
});

/// <summary>
/// Configures an HTTP client for the ApiPostPlugin with retry policies.
/// </summary>
services.AddHttpClient<ApiPostPlugin>(client =>
{
    client.BaseAddress = new Uri(settings.ApiUrl);
})
.AddTransientHttpErrorPolicy(policyBuilder =>
    policyBuilder.WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))); // Exponential backoff

/// <summary>
/// Registers the ApiPostPlugin as a singleton service.
/// </summary>
services.AddSingleton<IMonitorPlugin>(provider =>
    provider.GetRequiredService<ApiPostPlugin>());

var serviceProvider = services.BuildServiceProvider();

try
{
    /// <summary>
    /// Resolves and starts the MonitorRunner service.
    /// </summary>
    var runner = serviceProvider.GetRequiredService<MonitorRunner>();
    await runner.RunAsync();
}
catch (Exception ex)
{
    /// <summary>
    /// Logs any unhandled exceptions that occur in the application.
    /// </summary>
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "Unhandled exception occurred in application.");
}
