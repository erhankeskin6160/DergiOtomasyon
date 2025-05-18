namespace DergiOtomasyon.Models
{
    public class UserSubscription
    {
        public int Id { get; set; }

       
        public int UserId { get; set; }
        public int SubscriptionPlanId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now; // Abonelik başlangıç tarihi
        public DateTime EndDate { get; set; }// Abonelik bitiş tarihi

        public bool IsActive { get; set; } = true; //Abonelik Sistemi Aktifmi 

        public bool AutomaticRenewal { get; set; } = true; //Otomatik yenileme durumu
        public virtual User User { get; set; }
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
