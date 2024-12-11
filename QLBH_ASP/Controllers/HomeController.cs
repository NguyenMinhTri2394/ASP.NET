using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_ASP.Context;

namespace QLBH_ASP.Controllers
{
    public class HomeController : Controller
    {
        QLBH_ASPEntities objQLBH_ASPEntities = new QLBH_ASPEntities();

        public ActionResult Index()
        {
            var lstProduct = objQLBH_ASPEntities.Products.ToList();
            return View(lstProduct);
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