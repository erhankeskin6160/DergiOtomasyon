using AutoMapper;
using DergiOtomasyon.DTO;
using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace DergiOtomasyon.Controllers
{
    public class DergilerController : Controller
    {
        public MagazineDbContext context;
        public readonly IMapper mapper;
        public DergilerController(MagazineDbContext magazineDbContext ,IMapper _mapper)
        {
            context = magazineDbContext;
            mapper = _mapper;   
        }

        [HttpGet]
        public IActionResult Index(string? search,string? sıralama,string?alfabesıralama,string kategori, string? tarih, string? yayınyılı,bool?populersıralamaaktifmi=false,int? pagesize=20)
        
        
        
       {


            var allcategory = context.Categories.ToList();
            TempData["Kategoriler"] = allcategory;

            //Dergilerin   Kategorisini filteler filtresi
            var magazinelist = string.IsNullOrEmpty(kategori)
                ? context.Magazines.ToList()
                : context.Magazines.Where(x => x.Category.CategoryName == kategori).ToList();

            //Dergilerin bütün sayıları birlikte listeler
            var magazineinfolist = magazinelist.SelectMany(x => x.MagazineInfo).ToList();

            // Tarih filtresi
            DateTime dateLimit = DateTime.MinValue;
            if (!string.IsNullOrEmpty(tarih))
            {
                if (tarih == "1ay")
                    dateLimit = DateTime.Now.AddMonths(-1);
                else if (tarih == "3ay")
                    dateLimit = DateTime.Now.AddMonths(-3);
                else if (tarih == "1yil")
                    dateLimit = DateTime.Now.AddYears(-1);

                 magazineinfolist = magazinelist
               .SelectMany(x => x.MagazineInfo)
               .Where(mi => mi.PublicationDate >= dateLimit)
               .ToList();
            }
            //pöpürlerliğe göre sıralama

            if (populersıralamaaktifmi==true)
            {
                //Ödünç Alınan dergiler arasında en pöpüler dergileri listeler hangi dergi sayısı en çok alındıysa o  en pöpüler dergi olur
                var populermagizineınfolist = context.Borrowings.GroupBy(x => x.MagazineInfoId).Select(g => new
                {
                    MagazineInfoId = g.Key,
                    NumberReads = g.Count()//Okuma Sayısını Alır
                }).OrderByDescending(g => g.NumberReads).ToList();


                magazineinfolist = magazineinfolist
                 .OrderByDescending(mi => populermagizineınfolist
                     .FirstOrDefault(p => p.MagazineInfoId == mi.Id)?.NumberReads ?? 0)
                 .ToList();

            }

            // Yayın Yılına Göre Filtreleme

            if (!string.IsNullOrEmpty(yayınyılı))
            {
                magazineinfolist = magazineinfolist.Where(magazine => magazine.Magazine.PublisDate.Year == int.Parse(yayınyılı)).ToList();
            }
        
           

             // Magazinınfodaki aynı kategori isimli magazininfo veri sayılarını alır
            var magazineInfoCounts = context.Categories
                .Select(category => new
                {
                    CategoryName = category.CategoryName,
                    MagazineInfoCount = category.Magazine
                        .SelectMany(m => m.MagazineInfo)
                        .Count()
                })
                .ToList();

            // arama göre sıralama
            if (!string.IsNullOrEmpty(search))
            {
                magazineinfolist = magazineinfolist
                    .Where(m =>
                        (m.TopicName != null && m.TopicName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (m.Magazine != null && m.Magazine.MagazineName != null && m.Magazine.MagazineName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    )
                    .ToList();
            }

            //eklenme sırasına göre sıralama
            if (!string.IsNullOrEmpty(sıralama))
            {
                if (sıralama.Equals("Yeni"))
                {
                    magazineinfolist = magazineinfolist.OrderByDescending(m=>m.Id).ToList();
                }
                else
                {
                    magazineinfolist = magazineinfolist.OrderBy(m => m.Id).ToList();

                }
            }

            //Alfabe göre sıralama
            if (!string.IsNullOrEmpty(alfabesıralama))
            {
                if(alfabesıralama == "A-Z")
                {
                    magazineinfolist = magazineinfolist.OrderBy(x => x.TopicName).ToList();
                }
                else
                {
                    magazineinfolist = magazineinfolist.OrderByDescending(x => x.TopicName).ToList();
                }
            }

            //pagesize göre sıralama
             magazineinfolist=magazineinfolist.Take((int)pagesize).ToList(); 
            var populerlikAktifMi = Request.Query["populerlikaktifmi"];
            ViewBag.PopulerlikAktifMi = populerlikAktifMi;

            ViewBag.InfoCount = magazineInfoCounts;
          
            return View(magazineinfolist);
        

        }

         


        [HttpGet]
        public IActionResult MagazineDetail(int id) 
        {
            var magazinedetail = context.MagazinesInfo.Where(x => x.Id == id).FirstOrDefault();
            var magazinelist=context.MagazinesInfo.Where(x => x.MagazineId == magazinedetail.MagazineId).ToList();

            TempData["magazine"] = magazinelist;
            return View(magazinedetail);
        }

        [HttpGet]
        public IActionResult Borrow (int MagazineInfoId) 
        {

            var book = context.MagazinesInfo.FirstOrDefault(b => b.Id == MagazineInfoId);
            if (book==null)
            {
                return NotFound("");
            }

            var UserId = HttpContext.Session.GetInt32("UserId");
            if (!UserId.HasValue)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = context.Users.FirstOrDefault(user => user.Id == UserId);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }


            

            var magazineloan = new Borrowing
            {
                MagazineInfoId = MagazineInfoId,
                UserId = UserId.Value,
                BorrowingDate = DateTime.Now.ToLocalTime(),

                
            };
            context.Borrowings.Add(magazineloan);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}