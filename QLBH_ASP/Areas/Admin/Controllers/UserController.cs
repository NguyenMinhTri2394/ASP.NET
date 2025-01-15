using QLBH_ASP.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_ASP.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        WebsiteBanHangEntities4 objWebsiteBanHangEntities = new WebsiteBanHangEntities4();
        // GET: Admin/User
        public ActionResult Index(string searchTerm, int? page)
        {
            // Get all products as IQueryable
            var lstUser = objWebsiteBanHangEntities.Users.AsQueryable();
            return View(lstUser);  // Return IPagedList<Product> to the view
        }


        public ActionResult Details(int Id)
        {
            var objUser = objWebsiteBanHangEntities.Users.Where(n => n.Id == Id).FirstOrDefault();
            return View(objUser);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var objUser = objWebsiteBanHangEntities.Users.Where(n => n.Id == Id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Delete(User objUse)
        {
            var objUser = objWebsiteBanHangEntities.Users.Where(n => n.Id == objUse.Id).FirstOrDefault();

            objWebsiteBanHangEntities.Users.Remove(objUser);
            objWebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User objUser)
        {
            try
            {
                // Mã hóa mật khẩu
                if (!string.IsNullOrEmpty(objUser.Password))
                {
                    objUser.Password = HashPassword(objUser.Password);
                }

                // Thêm người dùng vào cơ sở dữ liệu
                objWebsiteBanHangEntities.Users.Add(objUser);
                objWebsiteBanHangEntities.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // Hàm mã hóa mật khẩu
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // GET: Admin/Product
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();

            var user = objWebsiteBanHangEntities.Users.Find(id);
            if (user == null) return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User objUser)
        {
            try
            {
                var existingUser = objWebsiteBanHangEntities.Users.Find(objUser.Id);
                if (existingUser == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật các trường khác
                existingUser.FirstName = objUser.FirstName;
                existingUser.LastName = objUser.LastName;
                existingUser.Email = objUser.Email;
                existingUser.Password = objUser.Password;

                // Đánh dấu thực thể là đã chỉnh sửa
                objWebsiteBanHangEntities.Entry(existingUser).State = EntityState.Modified;

                // Lưu thay đổi vào cơ sở dữ liệu
                objWebsiteBanHangEntities.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết (sử dụng thư viện log như NLog hoặc Serilog)
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình cập nhật sản phẩm. Vui lòng thử lại.";
                return RedirectToAction("Edit", new { id = objUser.Id });
            }
        }
    }
}