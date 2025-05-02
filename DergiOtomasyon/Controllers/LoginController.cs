using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DergiOtomasyon.Controllers
{
    public class LoginController : Controller
    {
        MagazineDbContext dbContext;
        public LoginController(MagazineDbContext magazineDb)
        {
            dbContext=magazineDb;
        }

        [HttpGet]
        public IActionResult Index() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Email,string Password)
        {

            var user = dbContext.Users.FirstOrDefault(x => x.Email == Email && x.Password == Password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Password", user.Password);
                HttpContext.Session.SetString("Email", user.Email);
                ViewBag.SuccesLogin = "Giriş Başarılı";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(); 
            return RedirectToAction("Index", "Home");
        }
    }
}
