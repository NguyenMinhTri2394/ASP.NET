using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_ASP.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AccountSetting()
        {
            return View();
        }
        public ActionResult Address()
        {
            return View();
        }
        public ActionResult Wishlist()
        {
            return View();
        }
        public ActionResult SellingItems()
        {
            return View();
        }
        public ActionResult Orders()
        {
            return View();
        }
    }
}