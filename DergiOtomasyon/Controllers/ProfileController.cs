using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DergiOtomasyon.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        MagazineDbContext dbContext = new MagazineDbContext();
        public ProfileController(MagazineDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId==null)
            {
                return RedirectToAction("Index", "Login");
            }

            var issubscription = dbContext.UserSubscriptions.FirstOrDefault(x => x.UserId == UserId && x.IsActive == true);
            ViewBag.issubscription = issubscription;

           
            return View();
        }

       


        [HttpPost]
        public IActionResult EditProfile(User updateuser)
            
        {
            var user= dbContext.Users.Find(updateuser.Id);
            user.Name = updateuser.Name;
            user.LastName = updateuser.LastName;
            user.Email = updateuser.Email;
            user.UserImg = updateuser.UserImg;
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Profile");
        }

       public PartialViewResult MyFavorite() 
        {
            return PartialView();

        }

        public PartialViewResult MyBorrow() 
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            var borrow =dbContext.Borrowings.Where(x=>x.UserId==UserId).ToList();
            ViewBag.borrow = borrow;
            return PartialView(borrow);

        }


        public PartialViewResult MySubscription()
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            var subscription = dbContext.UserSubscriptions.FirstOrDefault(x => x.UserId == UserId && x.IsActive == true);

            return PartialView(subscription);
        }

        public IActionResult MagazineReturn(int MagazineInfoId) 
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            var borrow = dbContext.Borrowings.FirstOrDefault(x => x.UserId == UserId && x.MagazineInfoId == MagazineInfoId);
            if (borrow!=null)
            {
                borrow.ReturnDate = DateTime.Now;
                borrow.ısReturned = true;

                dbContext.SaveChanges();
            }
           
            return RedirectToAction("Index", "Profile");
        }
    }
}
