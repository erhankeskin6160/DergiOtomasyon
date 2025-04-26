using DergiOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DergiOtomasyon.Controllers
{
    public class UserController : Controller
    {
        MagazineDbContext context;   
        public UserController(MagazineDbContext dbContext)
        {
              context=dbContext;
        }
        
        public IActionResult Index()
        {   
            var users=context.Users.ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(User user) 
        {
            context.Users.Add(user);
            context.SaveChanges();
            return RedirectToAction("Index");  
        }
        public IActionResult DeleteUser(int id)
        {
          
            var user=context.Users.Find(id);
            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("Index");   
        }

        [HttpGet]
        public IActionResult EditUser(int id) 
        {
            var user = context.Users.Find(id);
            
            return View(user);
        }
        [HttpPost]
        public IActionResult EditUser(User user) 
        {

            string görsel = default;
            if (Request.Form.Files.Count>0)
            {
                var file = Request.Form.Files[0];
                var name = Path.GetFileName(file.Name);
                var uzantı=Path.GetExtension(file.Name);
                string path = "/wwwroot/User" + file + name;
                
            }
             
            context.Users.Update(user);
            context.SaveChanges();
          

            return RedirectToAction("Index");

        }
    }
}
