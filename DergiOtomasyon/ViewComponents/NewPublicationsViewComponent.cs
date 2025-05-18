using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;

namespace DergiOtomasyon.ViewComponents
{
    public class NewPublicationsViewComponent: ViewComponent
    {
        public MagazineDbContext dbContext;
        public int pagesize = 5;
        public NewPublicationsViewComponent( MagazineDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public   IViewComponentResult Invoke(int page=1)    
        {

            var dergiler = dbContext.MagazinesInfo.OrderByDescending(x => x.Id).Skip((page-1)*pagesize).Take(pagesize).ToList();
           
            ViewBag.Sayfa = page;
            ViewBag.TotalSayfa = (int)Math.Ceiling((double) dbContext.MagazinesInfo.Count() / pagesize);

            return View(dergiler);
        }
    }
}
