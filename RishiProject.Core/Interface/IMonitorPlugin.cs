using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RishiProject.Core.Interface
{
    public interface IMonitorPlugin
    {
        Task OnSystemUsageUpdateAsync(SystemUsage usage);
    }
}
