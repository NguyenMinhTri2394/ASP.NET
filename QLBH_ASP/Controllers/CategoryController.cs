using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_ASP.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult AllCategory()
        {
            return View();
        }

        // GET: Category
        public ActionResult ProductByCategory()
        {
            return View();
        }
    }
}