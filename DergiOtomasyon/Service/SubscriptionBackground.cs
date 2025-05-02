
namespace DergiOtomasyon.Service
{
    public class SubscriptionBackground:BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SubscriptionBackground> _logger;
        public SubscriptionBackground(IServiceScopeFactory scopeFactory, ILogger<SubscriptionBackground> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("⏳ Abonelik kontrol servisi başlatıldı: {time}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<SubscriptionService>();
                        _logger.LogInformation("🔍 Abonelikler kontrol ediliyor: {time}", DateTime.Now);

                        await service.DeactivateExpiredSubscriptions();
                        _logger.LogInformation("✅ Kontrol tamamlandı: {time}", DateTime.Now);

                    }

                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "❌ Arka plan servisinde hata oluştu: {time}", DateTime.Now);
                    ;
                }

                
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
                 
            }
            _logger.LogInformation("🛑 Abonelik servis durduruldu: {time}", DateTime.Now);

        }
    }
}
