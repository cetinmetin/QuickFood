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
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            if (UserExist(form["userName"]))
            {
                return RedirectToAction("Index", "");
            }
            else { 
                using (QuickFoodEntities db = new QuickFoodEntities())
                {
                    User model = new User();
                    model.Username = form["userName"];
                
                    model.Password = form["password"];
                    model.FirstName = form["firstName"];
                    model.SurName = form["surname"];
                    model.Mail = form["email"];

                    db.Users.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index", "");
                }
            }
        }

        public bool UserExist(string UserName)
        {


            return true;
        }

    }
}