using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DergiOtomasyon.Controllers
{
    public class UserSubscriptionController : Controller
    {

         
        public MagazineDbContext dbContext { get; set; }
        public UserSubscriptionController(MagazineDbContext magazineDbContext)
        {
            dbContext = magazineDbContext;
        }
        public IActionResult Index()
        {
            var Subscriptionplans = dbContext.SubscriptionPlans.ToList();
            return View(Subscriptionplans);
        }


        //Kulanıcı abone olur
        [HttpPost]
        public IActionResult Subscribe(int ıd)
        {
            var userıd = HttpContext.Session.GetInt32("UserId");
            var user = dbContext.Users.FirstOrDefault(x => x.Id == userıd);
            if (user == null)
            {
                RedirectToAction("Index", "Login");
            }

            var Isusersubscribed = dbContext.UserSubscriptions.FirstOrDefault(x => x.UserId == userıd && x.IsActive==true && x.EndDate > DateTime.Now);

            if (Isusersubscribed != null)
            {
                TempData["subscriptionstate"] = "Zaten aktif bir aboneliğiniz var.";
                return RedirectToAction("Index", "UserSubscription");
            }

            if (user.Balance < 100)
            {
                TempData["subscriptionstate"] = "Abonelik Sistemini Satın Almak İçin Yetersiz Para .Lütfen Hesabınıza para yükleniyiz";
                return RedirectToAction("Index", "Profile");
            }

            var subscriptionplan = dbContext.SubscriptionPlans.Find(ıd);
            if (subscriptionplan != null)
            {
                var isusersubscription = dbContext.UserSubscriptions.FirstOrDefault(x => x.UserId == userıd);

                if (isusersubscription!=null)
                {
                    isusersubscription.StartDate = DateTime.Now;
                    isusersubscription.EndDate = DateTime.Now.AddDays(subscriptionplan.DurationInMonths);
                    isusersubscription.User.Balance -= 100;
                    isusersubscription.SubscriptionPlanId = subscriptionplan.Id;
                    isusersubscription.IsActive = true;
                    isusersubscription.AutomaticRenewal = true;
                }
                else
                {
                     var userSubscription = new UserSubscription
                    {
                        UserId = user.Id,
                        SubscriptionPlanId = subscriptionplan.Id,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(subscriptionplan.DurationInMonths),
                        IsActive = true
                    };
                dbContext.UserSubscriptions.Add(userSubscription);
                user.Balance -= 100;
                }

                   
                dbContext.SaveChanges();

                TempData["subscriptionstate"] = "Abonelik başarıyla oluşturuldu.";

            }
            return RedirectToAction("Index", "Profile");
        }

        [HttpGet]
        public IActionResult CancelSubscribe()
        {
            var userıd = HttpContext.Session.GetInt32("UserId");
            var subscription = dbContext.UserSubscriptions.FirstOrDefault(x => x.UserId == userıd && x.IsActive);

            return View(subscription);
        }

        [HttpPost]
        public IActionResult CancelSubscribe(bool cancelnow)
        {
            var userıd = HttpContext.Session.GetInt32("UserId");
            var user = dbContext.Users.FirstOrDefault(x => x.Id == userıd);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var subscription = dbContext.UserSubscriptions.FirstOrDefault(x => x.UserId == userıd && x.IsActive);

            if (subscription != null)
            {
                if (cancelnow)
                {
                    subscription.IsActive = false;
                    subscription.EndDate = DateTime.Now;
                    TempData["subscriptionstate"] = "Aboneliğiniz iptal edilmiştir.";
                }
                else
                {
                    TempData["subscriptionstate"] = "Aboneliğiniz Süre Sonunda iptal edilecektir.";
                }

                dbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Profile");
            }


            else
            {
                TempData["subscriptionstate"] = "Aboneliğiniz  yok";
                return RedirectToAction("Index", "Profile");
            }



        }

        [HttpPost]
        public IActionResult UpdateAutoRenew(int UserSubscriptionsId, bool autoRenew)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var subscription = dbContext.UserSubscriptions
    .FirstOrDefault(x => x.UserId == userId && x.Id == UserSubscriptionsId);



            if (subscription != null)
            {
                subscription.AutomaticRenewal = autoRenew;
                
                dbContext.SaveChanges();
                TempData["subscriptionstate"] = "Otomatik yenileme durumu güncellendi.";
            }
            else
            {
                TempData["subscriptionstate"] = "Aboneliğiniz yok.";
            }
            return RedirectToAction("Index", "Profile");



        }
    }
}
   
