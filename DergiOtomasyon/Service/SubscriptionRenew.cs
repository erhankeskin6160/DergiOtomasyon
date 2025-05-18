using DergiOtomasyon.Models;

namespace DergiOtomasyon.Service
{
    public class SubscriptionRenew
    {
        private readonly MagazineDbContext magazineDbContext;
        private readonly ILogger<SubscriptionRenew> logger;

        public SubscriptionRenew(MagazineDbContext magazineDbContext, ILogger<SubscriptionRenew> logger)
        {
            this.logger = logger;
            this.magazineDbContext = magazineDbContext;
        }


        public Task SubscriptionRenewal()

        {


            try
            {



                var date = DateTime.Now;
                var endingsubscribers = magazineDbContext.UserSubscriptions
                     .Where(x => x.EndDate <= date && x.IsActive == true && x.AutomaticRenewal==true)
                     .ToList();
                foreach (var item in endingsubscribers)
                {
                    var user = magazineDbContext.Users.FirstOrDefault(x => x.Id == item.UserId);
                    var plan = magazineDbContext.SubscriptionPlans.FirstOrDefault(x => x.Id == item.SubscriptionPlanId);
                    if (user != null && plan != null)
                    {
                        if (user.Balance >= 100)
                        {
                            item.StartDate = DateTime.Now;
                            item.EndDate = DateTime.Now.AddDays(30);
                            user.Balance -= 100;
                            item.IsActive = true;
                            item.SubscriptionPlan = item.SubscriptionPlan;

                            logger.LogInformation($"Abone {item.User.Name +""+ item.User.LastName} için abonelik yenilendi. Yeni bitiş tarihi: {item.EndDate}");

                        }
                        else
                        {
                            item.IsActive = false;

                            item.AutomaticRenewal = false;  // Otomatik yenileme kapatıldı çünkü bakiye yetersiz    
                            logger.LogInformation($"Abone {item.User.Name +""+ item.User.LastName} için abonelik iptal edildi.");
                        }


                         magazineDbContext.SaveChanges();   

                    }



                }
            }
            catch
            {
                logger.LogError("Abonelik yenileme işlemi sırasında bir hata oluştu.");
            }
            return Task.CompletedTask;
        }
    }


}

