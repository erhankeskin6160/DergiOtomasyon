using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DergiOtomasyon.ViewComponents
{
    public class MyBorrowViewComponent : ViewComponent
    {
        MagazineDbContext dbContext;
        public MyBorrowViewComponent(MagazineDbContext dbContext)
        {
                
        
            this.dbContext = dbContext;
        
        }
        public IViewComponentResult  Invoke()
        {
            
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                ViewBag.UserId = userId;
                
                var borrow = dbContext.Borrowings.Where(x => x.UserId == userId).ToList();

                return View(borrow);



            }
            else
            {
                ViewBag.UserId = null;
                return Content("Kullanıcı giriş yapmamış.");
            }

        }
    }
}
