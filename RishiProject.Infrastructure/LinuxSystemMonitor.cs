using RishiProject.Core.Interface;
using RishiProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RishiProject.Infrastructure
{
    public class LinuxSystemMonitor : IMonitorService
    {
        public SystemUsage GetSystemUsage()
        {
            // Just return dummy data to be implemented
            return new SystemUsage
            {
                CpuUsagePercent = 42.0f,
                RamUsedMb = 2048,
                RamTotalMb = 4096,
                DiskUsedMb = 50000,
                DiskTotalMb = 100000
            };
        }
    }
}
