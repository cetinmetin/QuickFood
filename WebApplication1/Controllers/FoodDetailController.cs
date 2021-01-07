using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class FoodDetailController : Controller
    {
        QuickFoodEntities db = new QuickFoodEntities();
        // GET: FoodDetail
        public ActionResult Index(int id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var Food = db.Database.SqlQuery<Food>("SELECT * FROM Foods WHERE FoodId = " + "'" + id + "'").ToList();
            var categories = db.Database.SqlQuery<Category>("SELECT * FROM Categories WHERE CategoryId = " + "'" + Food[0].CategoryId + "'").ToList();
            var nutritions = db.Database.SqlQuery<Nutrition>("SELECT * FROM Nutritions WHERE FoodId = " + "'" + id + "'").ToList();
            var ingredients = db.Database.SqlQuery<Ingredient>("SELECT * FROM Ingredients WHERE FoodId = " + "'" + id + "'").ToList();
            ViewBag.Foods = Food;
            ViewBag.Category = categories;
            ViewBag.Nutritions = nutritions;
            ViewBag.Ingredients = ingredients;
            ViewData["foodId"] = id;
            return View();
        }

    }
}