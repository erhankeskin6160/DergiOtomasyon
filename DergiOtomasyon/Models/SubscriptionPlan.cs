namespace DergiOtomasyon.Models
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        public string PlanName { get; set; }  // Örneğin, "1 Aylık", "3 Aylık" vb. 
        public int DurationInMonths { get; set; }  // Planın süresi (ay olarak)
        public double Price { get; set; }  // Planın fiyatı
    }
}
