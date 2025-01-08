using QLBH_ASP.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_ASP.Controllers
{
    public class CategoryController : Controller
    {
        WebsiteBanHangEntities4 objWebsiteBanHangEntities = new WebsiteBanHangEntities4();

        // GET: Category
        public ActionResult AllCategory()
        {
            var lstCategory = objWebsiteBanHangEntities.Categories.ToList();
            return View(lstCategory);
        }



        // GET: Category
        public ActionResult ProductByCategory(int id)
        {
            var listProduct = objWebsiteBanHangEntities.Products.Where(n => n.CategoryId == id).ToList();
            return View(listProduct);
        }
    }
}