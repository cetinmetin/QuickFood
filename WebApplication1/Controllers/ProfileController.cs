using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        // GET: Profile
        QuickFoodEntities db = new QuickFoodEntities();
        public ActionResult Index()
        {
            ViewBag.isUser = db.Users.FirstOrDefault(x => x.Username ==  HttpContext.User.Identity.Name);
            return View();   
        }
        
    }
}