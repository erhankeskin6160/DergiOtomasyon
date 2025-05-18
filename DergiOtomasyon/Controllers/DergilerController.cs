using AutoMapper;
using DergiOtomasyon.DTO;
using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Linq;

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
        public IActionResult Index(string? search,string? sıralama,string?alfabesıralama,string kategori,int? yıldızsıralama,string?yayınevi,string?yazarfiltreleme, string? tarih, string? yayınyılı,string keyword,bool?populersıralamaaktifmi=false,bool encokbegenilen=false,int? pagesize=20)
        
        
        
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

                ViewBag.PopulerlikAktifMi = populersıralamaaktifmi;
            }
            //En çok beğenilene göre

            if (encokbegenilen == true)
            {
                var likemagizeinfolist = context.Likes  
                    .GroupBy(x => x.MagazineInfoId)
                    .Select(like => new
                    {
                        MagazineInfoId = like.Key,
                        NumberLike = like.Count()
                    }).OrderByDescending(x=>x.NumberLike)
                    .ToList();

                magazineinfolist = magazineinfolist
                    .OrderByDescending(m =>
                        likemagizeinfolist.FirstOrDefault(p => p.MagazineInfoId == m.MagazineId)?.NumberLike ?? 0)
                    .ToList();

                ViewBag.EnCokBegilenAktifmi=encokbegenilen;
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
            

            ViewBag.InfoCount = magazineInfoCounts;



            // yıldız sayısana göre sıralama

            if (yıldızsıralama.HasValue)
            {
                magazineinfolist = magazineinfolist.Where(x=>x.Ratings.Any(r => r.Point == yıldızsıralama)).ToList();
            }
            // Her dergideki yazarları al, virgülle ayır ve tekrarsız olarak listele
            var authorlist = magazineinfolist
                .SelectMany(m => m.Authors.Split(','))    
                .Select(y => y.Trim().ToLowerInvariant())                   
                 
                .Distinct()                              
                .ToList();

            //Yazar Filtreleme

            if (yazarfiltreleme!=null)
            {
                magazineinfolist = magazineinfolist
    .Where(m => m.Authors.Split(',')
        .Select(a => a.Trim())
        .Contains(yazarfiltreleme))
    .ToList();
            }
            ViewBag.authorlist = authorlist;

            //yayınevine göre sıralama
            if (!string.IsNullOrEmpty(yayınevi))
            {
                magazineinfolist = magazineinfolist
                    .Where(m => m.Magazine.Publisher != null && m.Magazine.Publisher.IndexOf(yayınevi, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            ViewBag.Publisher = context.Magazines.Select(x=>x.Publisher).ToList();
      context.Magazines.Select(x => x.Publisher).ToList();



            // Keyword göre Sıralama
            if (!string.IsNullOrEmpty(keyword))
            {
                var keywords = keyword.Split(',').Select(x => x.Trim().ToLower()).ToList();

                 magazineinfolist = magazineinfolist.Where(x =>
                    x.Keyword != null &&
                    x.Keyword.Split(',')
                        .Select(k => k.Trim().ToLower())
                        .Any(k => keywords.Contains(k))
                ).ToList();

            }
            ViewBag.AllMagazine = magazineinfolist.Count().ToString();
            return View(magazineinfolist);
        

        }

         


        [HttpGet]
        public IActionResult MagazineDetail(int id) 
        {
            var magazinedetail = context.MagazinesInfo.Where(x => x.Id == id).FirstOrDefault();
            var magazinelist=context.MagazinesInfo.Where(x => x.MagazineId == magazinedetail.MagazineId).ToList();

            ViewBag.MagazineList = magazinelist;
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

            var subscription = context.UserSubscriptions.FirstOrDefault(usersubscription => usersubscription.UserId == UserId && usersubscription.IsActive);
            if (subscription==null || subscription.EndDate<DateTime.Now)
            {
                TempData["subscriptionstate"] = "Aboneliğiniz yok veya aboneliğinizin süresi dolmuş lütfen abonelik alınız";
                return RedirectToAction("Index", "UserSubscription");
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
        [HttpGet]
        public IActionResult FavoriteMagazine(int MagazineInfoId)
        {
            var book = context.MagazinesInfo.FirstOrDefault(b => b.Id == MagazineInfoId);
            if (book == null)
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

             bool isAlreadyFavorite = context.Favorites.Any(f => f.MagazineInfoId == MagazineInfoId && f.UserId == UserId.Value);
            if (isAlreadyFavorite) 
            {
                TempData["Error"] = "Bu dergi zaten favorilerinizde mevcut.";
                return RedirectToAction("Index", "Home");
            }
                var magazinefavorite = new Favorite
            {
                MagazineInfoId = MagazineInfoId,
                UserId = UserId.Value,
                Favorite_date = DateTime.Now.ToLocalTime(),
            };

            context.Favorites.Add(magazinefavorite);
            TempData["SuccesLike"] = "Dergi Favori  Listeye eklendi";
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult LikeMagazine(int MagazineInfoId)
        {
            var book = context.MagazinesInfo.FirstOrDefault(b => b.Id == MagazineInfoId);
            if (book == null)
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

            bool isAlreadyFavorite = context.Favorites.Any(f => f.MagazineInfoId == MagazineInfoId && f.UserId == UserId.Value);
            if (isAlreadyFavorite)
            {
                TempData["Error"] = "Bu dergi zaten beğendiniz  .";
                return RedirectToAction("Index", "Home");
            }
            var magazinelike = new Like
            {
                MagazineInfoId = MagazineInfoId,
                UserId = UserId.Value,
                Lıke_date = DateTime.Now.ToLocalTime(),
            };
            context.Likes.Add(magazinelike);
            TempData["SuccesLike"] = "Dergi Beğenilen Listeye eklendi";
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult MagizineCopy() 
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            if (!UserId.HasValue)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(context.MagazinesInfo.ToList());

        }

        [HttpPost]
        public IActionResult Rate(int MagazineId, int point) 
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            if (!UserId.HasValue)
            {
                return RedirectToAction("Index", "Login");
            }
            var rating = context.Ratings.FirstOrDefault(x => x.UserId == UserId && x.MagazineInfoId == MagazineId);
            if (rating != null)
            {
                rating.Point = point;
               rating.RatedDate = DateTime.Now;
                return RedirectToAction("MagazineDetail", new { id = MagazineId });

            }
            else
            {
                var ratings = new Rating
                {
                    UserId = UserId.Value,
                    MagazineInfoId = MagazineId,
                    Point = point,
                    RatedDate = DateTime.Now
                };
                context.Ratings.Add(ratings);
                context.SaveChanges();
                return RedirectToAction("Details", new { id = MagazineId });

            }

            return View();  
        }

    }
}