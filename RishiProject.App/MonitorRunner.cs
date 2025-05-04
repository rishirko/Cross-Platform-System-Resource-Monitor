using RishiProject.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RishiProject.App
{
    /// <summary>
    /// Responsible for running the monitoring process, invoking plugins, and logging system usage at regular intervals.
    /// </summary>
    public class MonitorRunner
    {
        private readonly IMonitorService monitorService;
        private readonly IEnumerable<IMonitorPlugin> plugins;
        private readonly ILogger<MonitorRunner> logger;
        private readonly int intervalSeconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorRunner"/> class.
        /// </summary>
        /// <param name="monitorService">The service responsible for retrieving system usage data.</param>
        /// <param name="plugins">A collection of plugins to process system usage updates.</param>
        /// <param name="logger">The logger instance for logging information, warnings, and errors.</param>
        /// <param name="intervalSeconds">The interval in seconds between monitoring updates.</param>
        public MonitorRunner(
            IMonitorService monitorService,
            IEnumerable<IMonitorPlugin> plugins,
            ILogger<MonitorRunner> logger,
            int intervalSeconds)
        {
            this.monitorService = monitorService;
            this.plugins = plugins;
            this.logger = logger;
            this.intervalSeconds = intervalSeconds;
        }



        /// <summary>
        /// Starts the monitoring process, periodically retrieving system usage data and invoking plugins.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RunAsync()
        {
            logger.LogInformation("Monitoring started at {Time}", DateTime.Now);

            while (true)
            {
                try
                {
                    // Retrieve system usage data
                    var usage = monitorService.GetSystemUsage();

                    // Log system usage to the console
                    Console.WriteLine(
                        $"CPU: {usage.CpuUsagePercent:0.00}% | RAM: {usage.RamUsedMb:0.00}/{usage.RamTotalMb:0.00} MB | Disk: {usage.DiskUsedMb:0.00}/{usage.DiskTotalMb:0.00} MB");

                    // Invoke each plugin with the system usage data
                    foreach (var plugin in plugins)
                    {
                        try
                        {
                            await plugin.OnSystemUsageUpdateAsync(usage);
                        }
                        catch (Exception ex)
                        {
                            // Log plugin-specific errors
                            logger.LogError(ex, "Plugin '{Plugin}' failed during update.", plugin.GetType().Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log errors occurring in the monitoring loop
                    logger.LogError(ex, "Error occurred in monitoring loop.");
                }

                try
                {
                    // Wait for the specified interval before the next update
                    await Task.Delay(TimeSpan.FromSeconds(intervalSeconds));
                }
                catch (Exception ex)
                {
                    // Log warnings if the delay is interrupted
                    logger.LogWarning(ex, "Task delay was interrupted.");
                }
            }
        }
    }
}
