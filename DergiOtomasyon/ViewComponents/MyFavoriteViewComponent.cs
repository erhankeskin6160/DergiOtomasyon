using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DergiOtomasyon.ViewComponents
{
    public class MyFavoriteViewComponent : ViewComponent
    {
        MagazineDbContext dbContext;
        public MyFavoriteViewComponent(MagazineDbContext dbContext)
        {
                
        
            this.dbContext = dbContext;
        
        }
        public IViewComponentResult  Invoke()
        {
            
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                ViewBag.UserId = userId;
                var UserId = HttpContext.Session.GetInt32("UserId");
                var favorite = dbContext.Favorites.Where(x => x.UserId == UserId).ToList();

                return View(favorite);



            }
            else
            {
                ViewBag.UserId = null;
                return Content("Kullanıcı giriş yapmamış.");
            }

        }
    }
}
