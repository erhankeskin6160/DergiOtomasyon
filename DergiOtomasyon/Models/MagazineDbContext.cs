using Microsoft.EntityFrameworkCore;

namespace DergiOtomasyon.Models
{
    public class MagazineDbContext:DbContext
    {
        public MagazineDbContext()
        {
                
        }
        public MagazineDbContext(DbContextOptions<MagazineDbContext> options) : base(options) { }

        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }

        public DbSet<MagazineInfo> MagazinesInfo { get; set;}
        public DbSet<Category> Categories { get; set;}
     
        public DbSet<Contanct> Contants { get; set; }

        public DbSet<Favorite> Favorites { get; set; }
        //public DbSet<MagazineInfo> magazineInfos { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

    }
}
