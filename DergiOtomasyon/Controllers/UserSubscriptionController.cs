using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DergiOtomasyon.Controllers
{
    public class UserSubscriptionController : Controller
    {
        public MagazineDbContext dbContext { get; set; }    
        public UserSubscriptionController( MagazineDbContext magazineDbContext)
        {
            dbContext = magazineDbContext;
        }
        public IActionResult Index()
        {
            var Subscriptionplans=dbContext.SubscriptionPlans.ToList(); 
            return View(Subscriptionplans);
        }


        //Kulanıcı abone olur
        [HttpPost]
        public IActionResult Subscribe(int ıd) 
        {
            var userıd =HttpContext.Session.GetInt32("UserId");
            var user = dbContext.Users.FirstOrDefault(x => x.Id == userıd);
            if (user==null)
            {
                RedirectToAction("Index", "Login");
            }

            var Isusersubscribed = dbContext.UserSubscriptions.FirstOrDefault(x => x.UserId == userıd && x.IsActive && x.EndDate > DateTime.Now);

            if (Isusersubscribed!=null)
            {
                TempData["subscriptionstate"] = "Zaten aktif bir aboneliğiniz var.";
                return RedirectToAction("Index", "UserSubscription");
            }

            var subscriptionplan = dbContext.SubscriptionPlans.Find(ıd);
            if (subscriptionplan != null)
            {
                var userSubscription = new UserSubscription
                {
                    UserId = user.Id,
                    SubscriptionPlanId = subscriptionplan.Id,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(subscriptionplan.DurationInMonths),
                    IsActive = true
                };
                dbContext.UserSubscriptions.Add(userSubscription);
                dbContext.SaveChanges();

                TempData["subscriptionstate"] = "Abonelik başarıyla oluşturuldu.";

            }
            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public IActionResult CancelSubscribe()
        {
            var userıd = HttpContext.Session.GetInt32("UserId");
            var user = dbContext.Users.FirstOrDefault(x => x.Id == userıd);
            if (user == null)
            {
                RedirectToAction("Index", "Login");
            }

            var subscription = dbContext.UserSubscriptions.FirstOrDefault(x => x.UserId == userıd && x.IsActive );

            if (subscription!=null)
            {
                subscription.IsActive = false;
                subscription.EndDate = DateTime.Now; 
              dbContext.SaveChangesAsync();
            }
            TempData["subscriptionstate"] = "Aboneliğiniz iptal edilmiştir.";

            return RedirectToAction("Index", "Profile");

            return View();
        }


    }
}
   
