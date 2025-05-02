using Castle.Core.Smtp;
using DergiOtomasyon.Models;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using System;

namespace DergiOtomasyon.Service
{
    public class SubscriptionReminder
    {
        private readonly MagazineDbContext _context;
        private readonly IEmailService _emailSender;
        private readonly ILogger<SubscriptionReminder> _logger;

        public SubscriptionReminder(MagazineDbContext context, IEmailService emailSender, ILogger<SubscriptionReminder> logger)
        {
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task SendSubscriptionReminders()
        {
            var subscriptions = await _context.UserSubscriptions
                .Where(x => x.EndDate < DateTime.Now.AddDays(8) && x.IsActive)
                .ToListAsync();
            foreach (var subscription in subscriptions)
            {
                var user = await _context.Users.FindAsync(subscription.UserId);
                if (user != null)
                {
                    var message = $"Merhaba {user.Name}, aboneliğinizin süresi dolmak üzere. Lütfen yenileyin.";

                    try
                    {
                        await _emailSender.SendAsync(user.Email, "Abonelik Hatırlatması",, message);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, $"E-posta gönderilemedi: {user.Email}");

                        
                    }
                    
                }
            }
        }
    }
}
