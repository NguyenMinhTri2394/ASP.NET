using QLBH_ASP.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace QLBH_ASP.Controllers
{
    public class UserController : Controller
    {
        WebsiteBanHangEntities4 objWebsiteBanHangEntities = new WebsiteBanHangEntities4();


        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = objWebsiteBanHangEntities.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    objWebsiteBanHangEntities.Configuration.ValidateOnSaveEnabled = false;
                    objWebsiteBanHangEntities.Users.Add(_user);
                    objWebsiteBanHangEntities.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();


        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = objWebsiteBanHangEntities.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    // Add session
                    Session["FullName"] = data.FirstOrDefault().LastName + " " + data.FirstOrDefault().FirstName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().Id;
                    //Session["isUser"] = true;

                    // Log session values for debugging
                    System.Diagnostics.Debug.WriteLine("Session FullName: " + Session["FullName"]);
                    System.Diagnostics.Debug.WriteLine("Session Email: " + Session["Email"]);
                    System.Diagnostics.Debug.WriteLine("Session idUser: " + Session["idUser"]);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }



        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            // Hủy Session hiện tại
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}