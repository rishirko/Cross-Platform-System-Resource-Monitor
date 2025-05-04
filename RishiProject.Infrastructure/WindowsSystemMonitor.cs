using RishiProject.Core;
using RishiProject.Core.Interface;
using System.Diagnostics;
using Microsoft.VisualBasic;
using NickStrupat;

namespace RishiProject.Infrastructure
{
    /// <summary>
    /// Provides system usage information for Windows systems, including CPU, RAM, and disk usage.
    /// </summary>
    public class WindowsSystemMonitor : IMonitorService
    {
        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        /// <summary>
        /// Retrieves the current system usage statistics, including CPU, RAM, and disk usage.
        /// </summary>
        /// <returns>
        /// A <see cref="SystemUsage"/> object containing the current CPU usage percentage,
        /// RAM usage in megabytes, and disk usage in megabytes.
        /// </returns>
        public SystemUsage GetSystemUsage()
        {
            var computerInfo = new ComputerInfo();

            // Calculate total and used RAM in megabytes
            float ramTotal = computerInfo.TotalPhysicalMemory / (1024 * 1024);
            float ramUsed = ramTotal - (computerInfo.AvailablePhysicalMemory / (1024 * 1024));

            // Calculate total and used disk space in megabytes
            var drives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed);
            float diskUsed = drives.Sum(d => (d.TotalSize - d.TotalFreeSpace) / (1024 * 1024));
            float diskTotal = drives.Sum(d => d.TotalSize / (1024 * 1024));

            // Return system usage statistics
            return new SystemUsage
            {
                CpuUsagePercent = cpuCounter.NextValue(),
                RamUsedMb = ramUsed,
                RamTotalMb = ramTotal,
                DiskUsedMb = diskUsed,
                DiskTotalMb = diskTotal
            };
        }
    }
}
