using Microsoft.AspNetCore.Mvc;

namespace DergiOtomasyon.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult Profile() 
        {
           return  PartialView();
        }

       public PartialViewResult MyFavorite() 
        {
            return PartialView();

        }

        public PartialViewResult MyBorrow() 
        {
            return PartialView();

        }
    }
}
