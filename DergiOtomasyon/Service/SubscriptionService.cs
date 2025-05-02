using DergiOtomasyon.Models;
using Microsoft.EntityFrameworkCore;

namespace DergiOtomasyon.Service
{
    public class SubscriptionService
    {
        private readonly MagazineDbContext _context;

        public SubscriptionService(MagazineDbContext context)
        {
            _context = context;
        }

        public async Task DeactivateExpiredSubscriptions()
        {
            var expired = await _context.UserSubscriptions
                .Where(x => x.EndDate < DateTime.Now && x.IsActive)
                .ToListAsync();

            foreach (var sub in expired)
            {
                sub.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}
