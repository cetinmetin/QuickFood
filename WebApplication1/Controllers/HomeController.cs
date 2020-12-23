using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection form)
        {

            using (QuickFoodEntities db = new QuickFoodEntities())
            {
                User model = new User();
                model.UserName = form["userName"];
                model.Password = form["password"];
                model.FirstName = form["firstName"];
                model.Surname = form["surname"];
                model.Email = form["email"];
                model.FavoriteId = 1;

                db.Users.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", "");

            }
        }
    }
}