using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SearchResultController : Controller
    {
        List<int> _Ingredients = new List<int>();
        QuickFoodEntities Db = new QuickFoodEntities();
        List<Food> _Food = new List<Food>();

        // GET: SearchResult
        public ActionResult Index()
        {
            ViewBag.Foods = TempData["Foods"];
            return View();
        }
        public ActionResult GetFoods(FormCollection form)
        {
            string foodName, recipe;
            int foodId, categoryId;
            for (int counter = 2; counter < form.Count; counter++)
            {
                var query = "SELECT IngredientId FROM Ingredients where IngredientName =" + "'" + form[counter] + "'";
                if (_Ingredients.Contains(Db.Database.SqlQuery<Int32>(query).FirstOrDefault()))
                {
                    TempData["IngredientFail"] = "Bu malzemeyi zaten seçtiniz";
                }
                else
                {
                    _Ingredients.Add(Db.Database.SqlQuery<Int32>(query).FirstOrDefault());
                }
            }
            var i = 0;
            foreach (int ingredientId in _Ingredients)
            {
                foodId = Db.Database.SqlQuery<Int32>("SELECT FoodId FROM Ingredients where IngredientId=" + "'" + ingredientId + "'").FirstOrDefault();
                categoryId = Db.Database.SqlQuery<Int32>("SELECT CategoryId FROM Foods where FoodId=" + "'" + foodId + "'").FirstOrDefault();
                foodName = Db.Database.SqlQuery<string>("SELECT FoodName FROM Foods where FoodId=" + "'" + foodId + "'").FirstOrDefault();
                recipe = Db.Database.SqlQuery<string>("SELECT Recipe FROM Foods where FoodId=" + "'" + foodId + "'").FirstOrDefault();
                var food = new Food { FoodId = Convert.ToInt32(foodId), CategoryId = Convert.ToInt32(categoryId), FoodName = foodName, Recipe = recipe };

                var match = _Food.FirstOrDefault(stringToCheck => stringToCheck.FoodName.Contains(food.FoodName));
                if (match != null)
                {
                    continue;
                }
                else
                {
                    _Food.Add(food);
                }
            }
            TempData["Foods"] = _Food;
            return RedirectToAction("Index", "SearchResult");
        }
    }
}