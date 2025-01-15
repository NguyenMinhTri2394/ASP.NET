using QLBH_ASP.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_ASP.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        WebsiteBanHangEntities4 objWebsiteBanHangEntities = new WebsiteBanHangEntities4();
        // GET: Admin/Category
        public ActionResult Index(string searchTerm, int? page)
        {
            var lstCategory = objWebsiteBanHangEntities.Categories.AsQueryable();
            return View(lstCategory);  // Return IPagedList<Product> to the view
        }

        public ActionResult Details(int Id)
        {
            var objCategory = objWebsiteBanHangEntities.Categories.Where(n => n.Id == Id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var objCategory = objWebsiteBanHangEntities.Categories.Where(n => n.Id == Id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Delete(Category objCate)
        {
            var objCategory = objWebsiteBanHangEntities.Categories.Where(n => n.Id == objCate.Id).FirstOrDefault();

            objWebsiteBanHangEntities.Categories.Remove(objCategory);
            objWebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category objCategory)
        {
            try
            {
                if (objCategory.Avatar != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + extension;
                    objCategory.Avatar = fileName;
                    objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/category/"), fileName));
                }

                objWebsiteBanHangEntities.Categories.Add(objCategory);
                objWebsiteBanHangEntities.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
        // GET: Admin/Product
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();

            var category = objWebsiteBanHangEntities.Categories.Find(id);
            if (category == null) return HttpNotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category objCategory)
        {
            try
            {
                var existingCategory = objWebsiteBanHangEntities.Categories.Find(objCategory.Id);
                if (existingCategory == null)
                {
                    return HttpNotFound();
                }

                // Kiểm tra và xử lý tệp tải lên
                if (objCategory.ImageUpload != null && objCategory.ImageUpload.ContentLength > 0)
                {
                    // Xử lý ảnh
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + extension; // Thêm timestamp để tránh trùng tên
                    string filePath = Path.Combine(Server.MapPath("~/Content/images/category/"), fileName);

                    objCategory.ImageUpload.SaveAs(filePath);

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existingCategory.Avatar))
                    {
                        string oldFilePath = Path.Combine(Server.MapPath("~/Content/images/items/"), existingCategory.Avatar);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Cập nhật đường dẫn ảnh mới
                    existingCategory.Avatar = fileName;
                }

                // Cập nhật các trường khác
                existingCategory.Name = objCategory.Name;
                //existingCategory.Description = objCategory.Description;

                // Đánh dấu thực thể là đã chỉnh sửa
                objWebsiteBanHangEntities.Entry(existingCategory).State = EntityState.Modified;

                // Lưu thay đổi vào cơ sở dữ liệu
                objWebsiteBanHangEntities.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết (sử dụng thư viện log như NLog hoặc Serilog)
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình cập nhật sản phẩm. Vui lòng thử lại.";
                return RedirectToAction("Edit", new { id = objCategory.Id });
            }
        }
    }
}