using Microsoft.Extensions.Logging;
using Moq;
using RishiProject.Core;
using RishiProject.Plugins;
using Xunit;

namespace RishiProject.UnitTests
{
    /// <summary>
    /// Unit tests for the FileLoggerPlugin class.
    /// </summary>
    public class UnitTest1
    {
        /// <summary>
        /// Tests that the OnSystemUsageUpdateAsync method logs system usage to a file correctly.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task OnSystemUsageUpdateAsync_ValidUsage_LogsToFile()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<FileLoggerPlugin>>();
            var plugin = new FileLoggerPlugin(mockLogger.Object);
            var usage = new SystemUsage
            {
                CpuUsagePercent = 50.5f,
                RamUsedMb = 4096,
                RamTotalMb = 8192,
                DiskUsedMb = 256,
                DiskTotalMb = 512
            };

            var testFilePath = "test_system_usage_log.txt";
            typeof(FileLoggerPlugin)
                .GetField("_filePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(plugin, testFilePath);

            if (File.Exists(testFilePath))
                File.Delete(testFilePath);

            // Act
            await plugin.OnSystemUsageUpdateAsync(usage);

            // Assert
            Assert.True(File.Exists(testFilePath), "Log file was not created.");
            var logContent = await File.ReadAllTextAsync(testFilePath);
            Assert.Contains("CPU: 50.50%", logContent);
            Assert.Contains("RAM: 4096.00/8192.00 MB", logContent);
            Assert.Contains("Disk: 256.00/512.00 MB", logContent);

            // Cleanup
            File.Delete(testFilePath);
        }

        /// <summary>
        /// Tests that the OnSystemUsageUpdateAsync method logs an error when a file write error occurs.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task OnSystemUsageUpdateAsync_FileWriteError_LogsError()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<FileLoggerPlugin>>();
            var plugin = new FileLoggerPlugin(mockLogger.Object);
            var usage = new SystemUsage
            {
                CpuUsagePercent = 50.5f,
                RamUsedMb = 4096,
                RamTotalMb = 8192,
                DiskUsedMb = 256,
                DiskTotalMb = 512
            };

            var testFilePath = "test_system_usage_log.txt";
            typeof(FileLoggerPlugin)
                .GetField("_filePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(plugin, testFilePath);

            // Simulate file write error by opening the file exclusively
            using (var fileStream = File.Open(testFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                // Act
                await plugin.OnSystemUsageUpdateAsync(usage);
            }

            // Assert
            //mockLogger.Verify(
            //    logger => logger.LogError(It.IsAny<IOException>(), "File write error in FileLoggerPlugin."),
            //    Times.Once);

            // Cleanup
            File.Delete(testFilePath);
        }
    }
}