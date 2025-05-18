using Microsoft.EntityFrameworkCore;

namespace DergiOtomasyon.Models
{
    public class MagazineDbContext:DbContext
    {
        public MagazineDbContext()
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>().HasIndex(r => new { r.UserId, r.MagazineInfoId }).IsUnique();
            modelBuilder.Entity<UserSubscription>().HasIndex(r => new { r.UserId }).IsUnique();
        }

        public MagazineDbContext(DbContextOptions<MagazineDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
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

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Like> Likes { get; set; }
        public DbSet<PenaltyLog>  PenaltyLogs { get; set; }


    }
}
