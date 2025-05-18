using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DergiOtomasyon.Controllers
{
    public class HomeController : Controller
    { 
        MagazineDbContext context=new MagazineDbContext();
        public HomeController(MagazineDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int page = 1)
        {
            var magazine = context.Magazines.Include(x=>x.MagazineInfo).ToList();
            var magazine1 = context.MagazinesInfo;
            ViewBag.magazine = magazine1;
            var dergiler1 = context.MagazinesInfo.OrderByDescending(x => x.Id).ToList();
            var populerdergi = context.Borrowings.Take(5).OrderByDescending(x => x.MagazineInfoId).ToList();
            ViewBag.populerdergi = populerdergi;

            ViewBag.dergilerbaskı = dergiler1;


            //Random random = new Random();
            //var dayoffer =random.Next(dergiler1.Count);
            //ViewBag.DayRandom = dergiler1[dayoffer];

            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            string suggestiondate = HttpContext.Session.GetString("ÖneriTarihi");
            int ? suggestiondateındex = HttpContext.Session.GetInt32("SonOneriIndex");
            if (suggestiondate != nowdate || !suggestiondateındex.HasValue)
            {
                 
                Random random = new Random();
                var dayoffer = random.Next(dergiler1.Count);

               
                ViewBag.DayRandom = dergiler1[dayoffer];

              
                HttpContext.Session.SetString("ÖneriTarihi", nowdate);
                HttpContext.Session.SetInt32("SonOneriIndex", dayoffer);
            }
            else
            {
                // Bugün zaten bir öneri yapılmışsa, aynı dergiyi göster
                ViewBag.DayRandom = dergiler1[suggestiondateındex.Value];
            }



            ViewBag.MagazineCount = magazine.Count.ToString();
                ViewBag.CategoryCount = context.Categories.Count().ToString();
            ViewBag.MagazineInfoCount = context.MagazinesInfo.Count().ToString();
            ViewBag.UserCount = context.Users.Count().ToString();
           


            return View(dergiler1);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About() 
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact() 
        {
            return View();
        }
        [HttpPost]

        public IActionResult Contact(Contanct contanct)
        {
            
            if (ModelState.IsValid==true)
            {
                TempData["SuccesContant"] = "Mesajınız Başarıyla Gönderildi";
                context.Contants.Add(contanct);
                context.SaveChanges();
            }
            else 
            {
                TempData["ErrrorContant"] = "Mesajınız Gönderilmedi";
            }
            
            return View(contanct);
        }

        
    }
}