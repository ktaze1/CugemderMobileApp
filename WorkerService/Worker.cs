using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        DateTime someDate = DateTime.Now.AddMinutes(1);
        HttpClient http = new HttpClient() { BaseAddress = new Uri( "http://localhost:3000/") };

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                if (DateTime.Now.Minute == someDate.Minute)
                {
                    _logger.LogInformation("Kaan taze -- time is now");
                    await http.GetAsync("api/Notifications");
                }
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
