using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RishiProject.Core.Interface;
using RishiProject.Core;

namespace RishiProject.Infrastructure
{
    public class ApiPostPlugin : IMonitorPlugin
    {
        private readonly HttpClient _client;
        private readonly ILogger<ApiPostPlugin> _logger;

        public ApiPostPlugin(HttpClient client, ILogger<ApiPostPlugin> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task OnSystemUsageUpdateAsync(SystemUsage usage)
        {
            try
            {
                var payload = new
                {
                    cpu = usage.CpuUsagePercent,
                    ram_used = usage.RamUsedMb,
                    disk_used = usage.DiskUsedMb
                };

                var response = await _client.PostAsJsonAsync("", payload); // URI is pre-configured in HttpClient

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API returned status code: {StatusCode}", response.StatusCode);
                }
                else
                {
                    _logger.LogInformation("Data posted to API successfully.");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error while sending data to API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ApiPostPlugin.");
            }
        }
    }
}
