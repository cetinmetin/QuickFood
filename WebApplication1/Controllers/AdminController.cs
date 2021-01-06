using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        QuickFoodEntities db = new QuickFoodEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if(Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login","Admin");
            }
            ViewBag.foodCount = db.Database.SqlQuery<int>("SELECT Count(FoodId) FROM Foods").FirstOrDefault();
            ViewBag.userCount = db.Database.SqlQuery<int>("SELECT Count(UserId) FROM Users").FirstOrDefault();
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            if(Convert.ToInt32(Session["state"]) == 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            var isUser = db.Users.FirstOrDefault(x => x.Username == userName && x.Password == password && x.State == 1);

            if (isUser != null)
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                Session["state"] = 1;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                TempData["ErrorMessage"] = "Lütfen bilgilerinizi kontrol ediniz!";
                return RedirectToAction("Login", "Admin");
            }

        }
        public ActionResult ListFood()
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Foods = db.Database.SqlQuery<Food>("SELECT * FROM Foods").ToList();
            ViewBag.Categories = db.Database.SqlQuery<Category>("SELECT * FROM Categories").ToList();
            return View();
        }

        public ActionResult AddFood()
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Categories = db.Database.SqlQuery<Category>("SELECT * FROM Categories").ToList();
            ViewBag.Ingredients = db.Database.SqlQuery<string>("SELECT DISTINCT IngredientName FROM Ingredients").ToList();
            ViewBag.Nutritions = db.Database.SqlQuery<string>("SELECT DISTINCT NutritionName FROM Nutritions").ToList();
            return View();
        }
        [HttpPost][ValidateInput(false)]
        public ActionResult AddFood(FormCollection form, string foodName, string recipe)
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }
            var ingredientName = db.Database.SqlQuery<string>("SELECT DISTINCT IngredientName FROM Ingredients").ToList();
            var ingredientCount2 = ingredientName.Count();
            var nutritionName = db.Database.SqlQuery<string>("SELECT DISTINCT NutritionName FROM Nutritions").ToList();

            Food model = new Food();
            model.FoodName = foodName;
            model.Recipe = recipe;
            model.CategoryId = Convert.ToInt32(form["category"]);
            db.Foods.Add(model);
            db.SaveChanges();
            int lastFoodId = db.Foods.Max(item => item.FoodId);


            var nutritions = form["nutrition"].Split(',');
            var ingredients = form["ingredient"].Split(',');
            Nutrition n = new Nutrition();
            var i = 0;
            foreach (var nutrition in nutritions)
            {
                n.FoodId = lastFoodId;
                n.NutritionName = nutritionName[i];
                n.NutritionValue = Convert.ToDouble(nutrition);
                db.Nutritions.Add(n);
                db.SaveChanges();
                i++;
            }

            Ingredient ing = new Ingredient();
            i = 0;
            foreach(var ingredient in ingredients)
            {
                ing.IngredientName = ingredient;
                ing.FoodId = lastFoodId;
                db.Ingredients.Add(ing);
                db.SaveChanges();
                i++;
            }

            return RedirectToAction("AddFood","Admin");
        }

        public ActionResult CategoryList()
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewBag.Foods = db.Database.SqlQuery<Food>("SELECT * FROM Foods").ToList();
            ViewBag.Categories = db.Database.SqlQuery<Category>("SELECT * FROM Categories").ToList();
            if (TempData["Deleted"] != null)
                ViewBag.Deleted = "Category" + TempData["Deleted"] + "deleted";
            if (TempData["CategoryDeleteError"] != null)
                ViewBag.Deleted = TempData["CategoryDeleteError"];
            if (TempData["CategoryAdd"] != null)
                ViewBag.CategoryAdd = TempData["CategoryAdd"];
            return View();
        }
        public ActionResult AddCategory(string categoryName)
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }
            var isCategory = db.Categories.FirstOrDefault(x => x.CategoryName == categoryName);
            if(isCategory == null) { 
                Category c = new Category();
                c.CategoryName = categoryName;
                db.Categories.Add(c);
                db.SaveChanges();
                TempData["CategoryAdd"] = "Category has added.";
            }
            else
            {
                TempData["CategoryAdd"] = "Error occurred while adding category. Please try again.";
            }
            return RedirectToAction("CategoryList", "Admin");
        }
        public ActionResult DeleteCategory(int id)
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }
            var isFood = db.Foods.FirstOrDefault(x => x.CategoryId == id);
            if (isFood != null)
            {
                TempData["CategoryDeleteError"] = "Error occurred while deleting category";
                return RedirectToAction("CategoryList", "Admin");
            }
            var deletedName = db.Database.SqlQuery<string>("SELECT CategoryName FROM Categories WHERE CategoryId = " + id).FirstOrDefault();
            Category c = new Category();
            c.CategoryId = id;
            c.CategoryName = deletedName;
            if(deletedName != null)
            {
                var a = db.Database.SqlQuery<List<string>>("DELETE FROM Categories WHERE CategoryId =" + id).Any();

                TempData["Deleted"] = deletedName;
            }
            else
            {
                TempData["CategoryDeleteError"] = "Error occurred while deleting category";
            }
            return RedirectToAction("CategoryList", "Admin");

        }
        public ActionResult AddIngredients()
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewBag.Ingredients = db.Database.SqlQuery<string>("SELECT DISTINCT IngredientName FROM Ingredients").ToList();
            
            return View();
        }
        [HttpPost]
        public ActionResult AddIngredients(string ingredientName)
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }
            var isIngredient = db.Ingredients.FirstOrDefault(x => x.IngredientName == ingredientName);

            if(isIngredient != null)
            {
                TempData["ErrorAddIngredients"] = "Error occurred while adding ingredients";
                return RedirectToAction("AddIngredients", "Admin");
            }
            Ingredient i = new Ingredient();
            i.IngredientName = ingredientName;
            db.Ingredients.Add(i);
            db.SaveChanges();
            return RedirectToAction("AddIngredients", "Admin");
        }

        public ActionResult AdminProfile()
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.UpdateUserMessage = TempData["UpdateUserMessage"];
            ViewBag.isUser = db.Users.FirstOrDefault(x => x.Username == HttpContext.User.Identity.Name);
            return View();
        }
        [HttpPost]
        public ActionResult AdminProfile(string userId, string firstName, string surName, string mail)
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }
            var query = "SELECT * FROM Users Where UserId = " + userId.ToString();
            var user = db.Database.SqlQuery<User>(query).FirstOrDefault();

            
            if (user.FirstName == firstName && user.SurName == surName && user.Mail == mail)
            {
                return RedirectToAction("AdminProfile", "Admin");
            }
            else
            {
                var updateAnUser = db.UpdateUser(Convert.ToInt32(userId), firstName.ToString(), surName.ToString(), mail.ToString());
                db.SaveChanges();
                if (updateAnUser == 1)
                {
                    TempData["UpdateUserMessage"] = "Profil Bilgileriniz basariyla guncellendi.";
                }
                else
                {
                    TempData["UpdateUserMessage"] = "Profil Bilgileriniz guncellenirken bir hata olustu.";
                }
            }

            return RedirectToAction("AdminProfile", "Admin");
        }

        public ActionResult CreateAdminUser()
        {
            if (Convert.ToInt32(Session["state"]) != 1)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.RegisterFail = TempData["RegisterFail"];
            return View();
        }
        [HttpPost]
        public ActionResult CreateAdminUser(string userName, string password, string firstName, string surName, string email, byte state = 1)
        {
            var isUser = db.Users.FirstOrDefault(x => x.Username == userName);
            if (isUser != null)
            {
                TempData["RegisterFail"] = "Bu kullanici adina sahip bir kullanici zaten var";
                return RedirectToAction("CreateAdminUser", "Admin");
            }
            else
            {
                User model = new User();
                model.Username = userName;
                model.Password = password;
                model.FirstName = firstName;
                model.SurName = surName;
                model.Mail = email;
                model.State = state;
                db.Users.Add(model);
                db.SaveChanges();
                //MailSender(firstName, email);
                TempData["RegisterFail"] = "Kullanici olusturma basarili bir sekilde tamamlandi.";
                return RedirectToAction("CreateAdminUser", "Admin");
            }
        }

            public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            /*Response.Cookies["username"].Expires = DateTime.Now.AddDays(-1);
            Session.RemoveAll();*/
            return RedirectToAction("Login", "Admin");
        }
    }
}