using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortScannerDetectorServer.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PortScannerDetectorServer.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace PortScannerDetectorServer.Services
{
    public class DetectionService : CronJobService
    {

        private readonly ILogger<DetectionService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public DetectionService(IScheduleConfig<DetectionService> config, ILogger<DetectionService> logger, IServiceScopeFactory scopeFactory)
       : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DetectionService starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} DetectionService is working.");
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var _fcmService = scope.ServiceProvider.GetRequiredService<FcmService>();
                var addresses = await _context.Addresses.Where(x => !(x.Blocked)).ToListAsync();
                if (addresses.Count > 0)
                {
                    await _fcmService.SendSuspiciousAddressesToMobileClient(addresses);
                }
            }
            return;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DetectionService is stopping.");
            return base.StopAsync(cancellationToken);
        }


    }
}