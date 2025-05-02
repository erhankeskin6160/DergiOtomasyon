using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace DergiOtomasyon.ViewComponents
{
    public class MySubscriptionViewComponent : ViewComponent
    {
        private MagazineDbContext dbContext;
        public MySubscriptionViewComponent(MagazineDbContext magazineDbContext)
        {
                dbContext= magazineDbContext;   
        }
        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var subscription = dbContext.UserSubscriptions.FirstOrDefault(x => x.UserId == userId && x.IsActive == true);
                return View(subscription);


            }

            return View();


        }
    }
}
