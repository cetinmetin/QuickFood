using Newtonsoft.Json;
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
        List<int> _IDsOfIngredients = new List<int>();
        List<string> _SearchedIngredients = new List<string>();
        QuickFoodEntities _Db = new QuickFoodEntities();
        List<Food> _Food = new List<Food>();
        List<string> _CategoryNames = new List<string>();
        List<string> _IngredientNames = new List<string>();
        List<Food> UnFilteredFoods = new List<Food>();
        List<Food> FilteredFoods = new List<Food>();
        List<Food> SortedFoods = new List<Food>();
        
        bool control = true;
        //Dictionary<int, int> foodCount = new Dictionary<int, int>();
        // GET: SearchResult
        public ActionResult Index()
        {
            if (TempData["Foods"] != null)
            {
                ViewBag.Foods = TempData["Foods"];
                ViewBag.SearchedIngredients = TempData["SearchedIngredients"];
                return View();
            }

            TempData["IngredientError"] = TempData["IngredientError"];
            return RedirectToAction("Index", "Home");
        }
        public ActionResult GetFoods(FormCollection form)
        {
            
            string foodName, recipe, categoryName;
                var countOfRepeat = 0;
            int foodId, categoryId;
            for (int i = 0; i < form.Count; i++)
            {
                var name = form[i].Split(',');
                var query = _Db.Database.SqlQuery<Food>("SELECT * FROM Foods WHERE FoodId IN (SELECT DISTINCT Foods.FoodId FROM Ingredients INNER JOIN Foods ON Foods.FoodId = Ingredients.FoodId WHERE Ingredients.IngredientName =  '"+ name[0] +"')").ToList();
                
                foreach (var food in query)
                {
                    UnFilteredFoods.Add(food);
                }
                var query2 = "SELECT IngredientId FROM Ingredients where IngredientName =" + "'" + name[0] + "'";
                var tempIngredientId = _Db.Database.SqlQuery<Int32>(query2).FirstOrDefault();
                bool isInList = _IDsOfIngredients.IndexOf(tempIngredientId) != -1;
                if (isInList != false || tempIngredientId == 0)
                {
                    continue;
                }
                else
                {
                    _SearchedIngredients.Add(name[0]);
                    _IDsOfIngredients.Add(tempIngredientId);
                }
            }


            foreach (var food in UnFilteredFoods)
            {

                foodId = food.FoodId;

                for (int i = 0; i < FilteredFoods.Count; i++)
                {
                    if(FilteredFoods[i].FoodId == foodId)
                    {
                        control = false;
                        break;
                    }
                    else
                    {
                        control = true;
                    }
                }

                if (control)
                {
                    FilteredFoods.Add(food);
                }
            }
            int[] foodCount = new int[FilteredFoods.Count];

            for (int i = 0; i < FilteredFoods.Count; i++)
            {
                int id = FilteredFoods[i].FoodId;
                for (int j = 0; j < UnFilteredFoods.Count; j++)
                {
                    if (id == UnFilteredFoods[j].FoodId)
                    {
                        countOfRepeat++;
                    }
                }
                foodCount[i] = countOfRepeat;
                countOfRepeat = 0;
                //foodCount.Add(Convert.ToInt32(abc));
            }

            for(int i = 0; i < FilteredFoods.Count; i++)
            {
                for(int j = 0; j < FilteredFoods.Count; j++)
                {
                    if (foodCount[i] >= foodCount[j])
                    {

                        var temp = foodCount[i];

                        foodCount[i] = foodCount[j];

                        foodCount[j] = temp;

                        var temp2 = FilteredFoods[i];

                        FilteredFoods[i] = FilteredFoods[j];

                        FilteredFoods[j] = temp2;

                    }
                }
            }
            
            if (FilteredFoods.Any())
            {
                TempData["Foods"] = FilteredFoods;
                TempData["SearchedIngredients"] = _SearchedIngredients;
                return RedirectToAction("Index", "SearchResult");
            }
            else
            {
                TempData["IngredientError"] = "Lütfen Malzeme Seçimi Yapın";
                return RedirectToAction("Index", "Home");
            }

            /*for (int counter = 2; counter < form.Count; counter++)
            {

                var name = form[counter].Split(',');
                var query = "SELECT IngredientId FROM Ingredients where IngredientName =" + "'" + name[0] + "'";
                var tempIngredientId = _Db.Database.SqlQuery<Int32>(query).FirstOrDefault();
                bool isInList = _IDsOfIngredients.IndexOf(tempIngredientId) != -1;
                if (isInList != false || tempIngredientId == 0)
                {
                    continue;
                }
                else
                {
                    _SearchedIngredients.Add(name[0]);
                    _IDsOfIngredients.Add(tempIngredientId);
                }
                if (_Ingredients.Contains(Db.Database.SqlQuery<Int32>(query).FirstOrDefault()))
                {
                    continue;
                }
                else
                {
                    _Ingredients.Add(Db.Database.SqlQuery<Int32>(query).FirstOrDefault());
                }
            }*/
            /*foreach (int ingredientId in _IDsOfIngredients)
            {
                foodId = _Db.Database.SqlQuery<Int32>("SELECT FoodId FROM Ingredients where IngredientId=" + "'" + ingredientId + "'").FirstOrDefault();
                categoryId = _Db.Database.SqlQuery<Int32>("SELECT CategoryId FROM Foods where FoodId=" + "'" + foodId + "'").FirstOrDefault();
                categoryName = _Db.Database.SqlQuery<string>("SELECT CategoryName from Categories where CategoryId =" + "'" + categoryId + "'").FirstOrDefault();
                foodName = _Db.Database.SqlQuery<string>("SELECT FoodName FROM Foods where FoodId=" + "'" + foodId + "'").FirstOrDefault();
                recipe = _Db.Database.SqlQuery<string>("SELECT Recipe FROM Foods where FoodId=" + "'" + foodId + "'").FirstOrDefault();
                var food = new Food { FoodId = Convert.ToInt32(foodId), CategoryName = categoryName, CategoryId = Convert.ToInt32(categoryId), FoodName = foodName, Recipe = recipe };
                //var ingredients = _Db.Database.SqlQuery<Ingredient>("SELECT * from Ingredients where FoodId=" + foodId).ToList();

                var match = _Food.FirstOrDefault(stringToCheck => stringToCheck.FoodName.Contains(food.FoodName));
                if (match != null)
                {
                    continue;
                }
                else
                {
                    _Food.Add(food);
                }
            }*/
            //TempData["Foods"] = _Food;
            //TempData["SearchedIngredients"] = _SearchedIngredients;

            //if (_Food.Any())
            //   return RedirectToAction("Index", "SearchResult");
            //else
            //{
            //   TempData["IngredientError"] = "Lütfen Malzeme Seçimi Yapın";
            //  return RedirectToAction("Index", "Home");
            //}
        }

        public ActionResult GetFoodsAsCategory(int id)
        {
            var CategoryFoods = _Db.Database.SqlQuery<Food>("SELECT * FROM Foods F INNER JOIN Categories C ON C.CategoryId = F.CategoryId WHERE F.CategoryId =" + id).ToList();
            TempData["Foods"] = CategoryFoods;
            return RedirectToAction("Index", "SearchResult");
        }

    }
}