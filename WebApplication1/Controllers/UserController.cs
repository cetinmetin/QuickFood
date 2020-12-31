using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        QuickFoodEntities db = new QuickFoodEntities();

        public ActionResult Register(string userName, string password, string firstName, string surName, string email)
        {
            var isUser = db.Users.FirstOrDefault(x => x.Username == userName);
            if (isUser != null)
            {
               TempData["RegisterFail"] = "Bu kullanici adina sahip bir kullanici zaten var";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                
                User model = new User();
                model.Username = userName;
                model.Password = password;
                model.FirstName = firstName;
                model.SurName = surName;
                model.Mail = email;
                db.Users.Add(model);
                db.SaveChanges();
                //MailSender(firstName, email);
                FormsAuthentication.SetAuthCookie(userName, false);
                return RedirectToAction("Index", "Home");     
            }
        }
        
        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            var isUser = db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
            if(isUser != null)
            {
                FormsAuthentication.SetAuthCookie(username, false);
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Hatali kullanici adi veya sifre";
                return RedirectToAction("Index", "Home");
            }

           /* var model = GetUsers();
       
            foreach(var user in model)
            {
                if(username.ToLower().Trim() == user.Username && password.Trim() == user.Password)
                {
                    Session["userId"] = user.UserId;
                    Session["FirstName"] = user.FirstName;
                    Session["SurName"] = user.SurName;
                    Session["Username"] = user.Username;
                    Session["Password"] = user.Password;
                    Session["Mail"] = user.Mail;
                    HttpCookie cookie = new HttpCookie("username", user.Username);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");
                }
            }*/
            
        }
        public ActionResult UpdateProfile(string userId, string firstName, string surName, string mail, string newPassword)
        {
            var users = GetAnUser(Convert.ToInt32(userId));
            
            foreach(var user in users)
            {
                if (user.FirstName == firstName && user.SurName == surName && user.Mail == mail && newPassword == "")
                {
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    UpdateUser(Convert.ToInt32(userId), firstName, surName, mail);
                }
            }

            return RedirectToAction("Index", "Profile");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            /*Response.Cookies["username"].Expires = DateTime.Now.AddDays(-1);
            Session.RemoveAll();*/
            return RedirectToAction("Index", "Home");
        }

        public List<User> GetAnUser(int userId)
        {
            var query = "SELECT * FROM Users Where UserId = " + userId.ToString();
            return db.Database.SqlQuery<User>(query).ToList();
        }

        public List<User> GetUsers()
        {
            var query = "SELECT * FROM Users";
            return db.Database.SqlQuery<User>(query).ToList();
        }
        public ActionResult UpdateUser(int userId ,string firstName, string surName, string mail)
        {
            // db.Database.SqlQuery<bool>(@"UpdateUser @userId, @firstName, @surName, @mail", param).Any()
            // db.Database.ExecuteSqlCommand("UpdateUser @userId, @firstName, @surName, @mail", param);
            
            var updateAnUser = db.UpdateUser(userId, firstName.ToString(), surName.ToString(), mail.ToString());
            db.SaveChanges();
            if (updateAnUser == 1) 
            {
                TempData["UpdateUserMessage"] = "Profil Bilgileriniz basariyla guncellendi.";
            }
            else
            {
                TempData["UpdateUserMessage"] = "Profil Bilgileriniz guncellenirken bir hata olustu.";
            }
            return RedirectToAction("Index", "Profile");
           

        }

        public static void MailSender(string name, string mail)
        {
            var fromAddress = new MailAddress("bthncm@gmail.com");
            var toAddress = new MailAddress(mail);
            const string subject = "Kayıt Olundu";
            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "MailSifre")
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = "Hoşgeldiniz" + name + ". QuickFood'a kayıt işleminiz başarıyla tamamlanmıştır. Kayıt Olduğunuz için teşekkür ederiz. <br>QuickFood" })
                {
                    smtp.Send(message);
                }
            }
        }

    }
}