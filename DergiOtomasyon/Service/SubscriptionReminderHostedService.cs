
namespace DergiOtomasyon.Service
{
    public class SubscriptionReminderHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubscriptionReminderHostedService> _logger;

        public SubscriptionReminderHostedService(IServiceProvider serviceProvider, ILogger<SubscriptionReminderHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Abonelik hatırlatıcı servisi başlatılıyor...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var reminder = scope.ServiceProvider.GetRequiredService<SubscriptionReminder>();

                try
                {
                    await reminder.SendSubscriptionReminders();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Abonelik hatırlatıcı çalıştırılırken hata oluştu.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
