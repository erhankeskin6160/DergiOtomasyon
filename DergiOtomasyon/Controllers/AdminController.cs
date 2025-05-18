using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Security.Claims;

namespace DergiOtomasyon.Controllers
{

    [Authorize(AuthenticationSchemes = "Admin")]
    public class AdminController : Controller
    {
        MagazineDbContext context;

        public AdminController(MagazineDbContext context)
        {
            this.context = context;
        }

        [AllowAnonymous]
        public IActionResult Login(string Email, string Password) 
        {
            var admin = context.Admins.FirstOrDefault(x => x.Email == Email && x.Password == Password);

            if (admin != null)
            {
                HttpContext.Session.SetInt32("UserId", admin.Id);
                HttpContext.Session.SetString("Password", admin.Password);
                HttpContext.Session.SetString("Email", admin.Email);

                var claims = new List<Claim>
                {
            new Claim(ClaimTypes.Email, admin.Email),
            

                };
                var identity = new ClaimsIdentity(claims, "Admin");
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync("Admin", principal);
                TempData["AdminSuccesLogin"] = "Giriş Başarılı";
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
            }
            return View();
        }

        public IActionResult Logout() 
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Admin", "Login");
        }

        public IActionResult Index() {
            return View();
        }
        public IActionResult DergiListele()
        {
            var dergi = context.Magazines.ToList();
            return View(dergi);
        }

        [HttpGet]
        public IActionResult DergiEkle()
        {


            return View();


        }
        [HttpPost]
        public IActionResult DergiEkle(Magazine magazine)
        {

            string görsel = default;
            if (Request.Form.Files.Count > 0)
            {
                var filename = Path.GetFileNameWithoutExtension(Request.Form.Files[0].FileName);
                var extension = Path.GetExtension(Request.Form.Files[0].FileName).ToLower();
                string path = "wwwroot/css/" + filename + extension;
                FileStream stream = new FileStream(path, FileMode.Create);
                Request.Form.Files[0].CopyTo(stream);

                görsel = filename + extension;

            }
            magazine.Img = görsel;
            var addmagazine = magazine;
            context.Magazines.Add(addmagazine);
            context.SaveChanges();

            return View();
        }
        [HttpGet]
        public IActionResult DergiDüzenle(int id)
        {
            var user = context.Users.Find(id);

            return View(user);
        }
        [HttpPost]
        public IActionResult DergiDüzenle(Magazine Magazine)
        {
            var editmagazine = context.Magazines.Where(x => x.Id == Magazine.Id).First();
            string görsel = default;
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var name = Path.GetFileName(file.Name);
                var uzantı = Path.GetExtension(Request.Form.Files[0].Name);
                string path = "wwwroot/css/" + name + uzantı;
                FileStream stream = new FileStream(path, FileMode.Create);
                Request.Form.Files[0].CopyTo(stream);

                görsel = name + uzantı;
            }

            Magazine.Img = görsel;
            editmagazine.MagazineName=Magazine.MagazineName;
            editmagazine.Editor=Magazine.Editor;
            editmagazine.PublisDate=Magazine.PublisDate;
            editmagazine.ısPuslihed = Magazine.ısPuslihed;
            editmagazine.Img = Magazine.Img;
            


            context.SaveChanges();


            return RedirectToAction("Index");
        }

        public IActionResult DergiSil(int id)
        {
            var deletemagazine = context.Magazines.Where(x => x.Id == id).First();

            if (deletemagazine != null)
            {
                context.Magazines.Remove(deletemagazine);
            }
            return RedirectToAction("Index");

        }
         public IActionResult KullanıcıListele() 
        {
            var kullanıcılar = context.Users.ToList();
            return View(kullanıcılar);
        }
        [HttpGet]
        public IActionResult KullanıcıEkle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult KullanıcıEkle(User user)
        {


            context.Users.Add(user);
            context.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult KullanıcıSil(int id)
        {

            var user = context.Users.Find(id);
            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult KullanıcıDüzenle(int id)
        {
            var user = context.Users.Find(id);

            return View(user);
        }
        [HttpPost]
        public IActionResult KullanıcıDüzenle(User user)
        {
            var edituser = context.Users.Where(x => x.Id == user.Id).First();
            string görsel = default;
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0].Name;
                var name = Path.GetFileName(file);
                var uzantı = Path.GetExtension(file);
                string path = "wwwroot/css/"+ name + uzantı;
                FileStream stream = new FileStream(path, FileMode.Create);
                Request.Form.Files[0].CopyTo(stream);

                görsel = name + uzantı;
            }

            user.UserImg = görsel;
            edituser.Email = user.Email;
            edituser.Subscription = user.Subscription;
            edituser.RegistrationDate = user.RegistrationDate;
            edituser.UserName = user.UserName;  
            edituser.LastName   = user.LastName;
            edituser.Balance = user.Balance;
            edituser.UserImg= user.UserImg; 
            edituser.Name = user.Name;
            edituser.Password = user.Password;
           
             
            context.SaveChanges();


            return RedirectToAction("Index");
        }


        public IActionResult Subscription() 
        {
            return View(context.SubscriptionPlans.ToList());
        }

        [HttpGet]
        public IActionResult SubscriptionCreate() 
        {
            return View();
        }



        [HttpPost]
        public IActionResult SubscriptionCreate( SubscriptionPlan subscriptionPlan)
        {
            if (ModelState.IsValid==true)
            {
                context.SubscriptionPlans.Add(subscriptionPlan);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View();
        }
    }
}

