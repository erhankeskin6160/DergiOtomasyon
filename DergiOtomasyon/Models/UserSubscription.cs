namespace DergiOtomasyon.Models
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int SubscriptionPlanId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual User User { get; set; }
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
