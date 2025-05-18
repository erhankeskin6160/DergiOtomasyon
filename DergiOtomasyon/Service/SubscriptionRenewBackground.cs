
namespace DergiOtomasyon.Service
{
    public class SubscriptionRenewBackground : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SubscriptionRenewBackground> _logger;

        public SubscriptionRenewBackground(IServiceScopeFactory serviceScope, ILogger<SubscriptionRenewBackground> logger)
        {
            _scopeFactory = serviceScope;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
         while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<SubscriptionRenew>();
                         await service.SubscriptionRenewal();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Arka plan servisinde hata oluştu: {time}", DateTime.Now);
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
            
        }
    }
}
