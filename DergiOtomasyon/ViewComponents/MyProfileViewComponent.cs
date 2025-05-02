using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DergiOtomasyon.ViewComponents
{
    public class MyProfileViewComponent:ViewComponent
    {

        MagazineDbContext dbContext;
        public MyProfileViewComponent(MagazineDbContext dbContext)
        {


            this.dbContext = dbContext;

        }
        [HttpGet]
        public IViewComponentResult Invoke()
        {

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                ViewBag.UserId = userId;
                var UserId = HttpContext.Session.GetInt32("UserId");
                var user = dbContext.Users.FirstOrDefault(x => x.Id == UserId);

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
