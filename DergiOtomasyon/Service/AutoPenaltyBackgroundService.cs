namespace DergiOtomasyon.Service
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DergiOtomasyon.Service;

    public class AutoPenaltyBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AutoPenaltyBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var penaltyService = scope.ServiceProvider.GetRequiredService<AutoPenaltyService>();
                    penaltyService.AutoPenalty();
                }

             
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }

}
