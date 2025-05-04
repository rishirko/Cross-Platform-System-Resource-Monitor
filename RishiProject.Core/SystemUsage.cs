using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RishiProject.Core
{
    public class SystemUsage
    {
        public float CpuUsagePercent { get; set; }
        public float RamUsedMb { get; set; }
        public float RamTotalMb { get; set; }
        public float DiskUsedMb { get; set; }
        public float DiskTotalMb { get; set; }
    }
}
