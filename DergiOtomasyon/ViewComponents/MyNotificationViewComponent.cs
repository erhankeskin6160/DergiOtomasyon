using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace DergiOtomasyon.ViewComponents
{
    public class MyNotificationViewComponent: ViewComponent
    {
        MagazineDbContext dbContext;
        public MyNotificationViewComponent(MagazineDbContext dbContext)
        {


            this.dbContext = dbContext;

        }
        [HttpGet]
        public IViewComponentResult Invoke()
        {

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                 var UserId = HttpContext.Session.GetInt32("UserId");
                var user = dbContext.Notifications.ToList();

                return View(user);



            }
            else
            {
                ViewBag.UserId = null;
                return Content("Kullanıcı giriş yapmamış.");
            }


        }
    }
}
