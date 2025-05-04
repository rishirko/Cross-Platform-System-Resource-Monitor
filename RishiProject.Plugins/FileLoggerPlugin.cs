using Microsoft.Extensions.Logging;
using RishiProject.Core;
using RishiProject.Core.Interface;

namespace RishiProject.Plugins
{
    /// <summary>
    /// A plugin that logs system usage data to a file.
    /// </summary>
    public class FileLoggerPlugin : IMonitorPlugin
    {
        private readonly string _filePath = "system_usage_log.txt";
        private readonly ILogger<FileLoggerPlugin> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLoggerPlugin"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging errors and information.</param>
        public FileLoggerPlugin(ILogger<FileLoggerPlugin> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Logs the system usage data to a file.
        /// </summary>
        /// <param name="usage">The system usage data to log.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnSystemUsageUpdateAsync(SystemUsage usage)
        {
            try
            {
                var logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | CPU: {usage.CpuUsagePercent:0.00}% | RAM: {usage.RamUsedMb:0.00}/{usage.RamTotalMb:0.00} MB | Disk: {usage.DiskUsedMb:0.00}/{usage.DiskTotalMb:0.00} MB";
                await File.AppendAllTextAsync(_filePath, logLine + Environment.NewLine);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "File write error in FileLoggerPlugin.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in FileLoggerPlugin.");
            }
        }
    }
}
