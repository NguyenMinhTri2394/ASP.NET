using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_ASP.Context;
using QLBH_ASP.Models;

namespace QLBH_ASP.Controllers
{
    public class HomeController : Controller
    {
        WebsiteBanHangEntities4 objWebsiteBanHangEntities = new WebsiteBanHangEntities4();

        public ActionResult Index()
        {
            // Kiểm tra trạng thái đăng nhập
            if (Session["isUser"] != null && (bool)Session["isUser"])
            {
                ViewBag.Message = "Welcome " + Session["FullName"];
            }
            else
            {
                ViewBag.Message = "You are not logged in.";
            }

            // Chuẩn bị dữ liệu cho HomeModel
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListProduct = objWebsiteBanHangEntities.Products.ToList();
            objHomeModel.ListCategory = objWebsiteBanHangEntities.Categories.ToList();

            return View(objHomeModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}