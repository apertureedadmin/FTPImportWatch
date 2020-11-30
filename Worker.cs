using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FTPImportWatch
{
    public class Worker : BackgroundService
    {
        private bool isLoadingFiles = false;

        private readonly ILogger<Worker> _logger;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            CSVUtil chandler = new CSVUtil();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!isLoadingFiles) {
                    isLoadingFiles = true;
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    _ = chandler.IterateWindowsFolders();

                    await Task.Delay(60000, stoppingToken);
                    isLoadingFiles = false;
                }
            }
        }
    }
}
