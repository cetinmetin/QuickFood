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
            ViewBag.RegisterFail = TempData["RegisterFail"];
            ViewBag.IngredientError = TempData["IngredientError"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            QuickFoodEntities db = new QuickFoodEntities();
            ViewBag.RandomFoods = db.Database.SqlQuery<GetRandomFood>("SELECT TOP 6 foodId,foodName, Recipe, CategoryId, CategoryName FROM GetRandomFoods ORDER BY NEWID()").ToList();
            return View();
        }


    }
}