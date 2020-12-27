using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            ViewBag.isUpdateUser = TempData["isUserUpdate"];
            ViewBag.UpdateUserMessage = TempData["UpdateUserMessage"];
            return View();
        }
        
    }
}