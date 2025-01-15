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
    public class ProductController : Controller
    {
        WebsiteBanHangEntities4 objWebsiteBanHangEntities = new WebsiteBanHangEntities4();
        // GET: Admin/Product
        public ActionResult Index()
        {
            var lstProduct = objWebsiteBanHangEntities.Products.ToList();
            return View(lstProduct);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // Lấy danh sách danh mục và thương hiệu từ cơ sở dữ liệu
            var categories = objWebsiteBanHangEntities.Categories.ToList();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");

            var brands = objWebsiteBanHangEntities.Brands.ToList();
            ViewBag.BrandId = new SelectList(brands, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            try
            {
                if (objProduct.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName + extension;
                    objProduct.Avatar = fileName;
                    objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/product/"), fileName));
                }

                objWebsiteBanHangEntities.Products.Add(objProduct);
                objWebsiteBanHangEntities.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var objProduct = objWebsiteBanHangEntities.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);

        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var objProduct = objWebsiteBanHangEntities.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objWebsiteBanHangEntities.Products.Where(n => n.Id == objPro.Id).FirstOrDefault();

            objWebsiteBanHangEntities.Products.Remove(objProduct);
            objWebsiteBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();

            var product = objWebsiteBanHangEntities.Products.Find(id);
            if (product == null) return HttpNotFound();

            // Truyền danh sách Category và Brand vào ViewBag
            ViewBag.CategoryId = new SelectList(objWebsiteBanHangEntities.Categories, "Id", "Name", product.Id);
            ViewBag.BrandId = new SelectList(objWebsiteBanHangEntities.Brands, "Id", "Name", product.Id);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product objProduct)
        {
            try
            {
                var existingProduct = objWebsiteBanHangEntities.Products.Find(objProduct.Id);
                if (existingProduct == null)
                {
                    return HttpNotFound();
                }

                // Kiểm tra và xử lý tệp tải lên
                if (objProduct.ImageUpload != null && objProduct.ImageUpload.ContentLength > 0)
                {
                    // Xử lý ảnh
                    string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName + extension; // Thêm timestamp để tránh trùng tên
                    string filePath = Path.Combine(Server.MapPath("~/Content/images/items/"), fileName);

                    objProduct.ImageUpload.SaveAs(filePath);

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existingProduct.Avatar))
                    {
                        string oldFilePath = Path.Combine(Server.MapPath("~/Content/images/product/"), existingProduct.Avatar);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Cập nhật đường dẫn ảnh mới
                    existingProduct.Avatar = fileName;
                }

                // Cập nhật các trường khác
                existingProduct.Name = objProduct.Name;
                existingProduct.ShortDes = objProduct.ShortDes;
                existingProduct.FullDescription = objProduct.FullDescription;
                existingProduct.Price = objProduct.Price;
                existingProduct.CategoryId = objProduct.CategoryId;
                existingProduct.BrandId = objProduct.BrandId;

                // Đánh dấu thực thể là đã chỉnh sửa
                objWebsiteBanHangEntities.Entry(existingProduct).State = EntityState.Modified;

                // Lưu thay đổi vào cơ sở dữ liệu
                objWebsiteBanHangEntities.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết (sử dụng thư viện log như NLog hoặc Serilog)
                TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình cập nhật sản phẩm. Vui lòng thử lại.";
                return RedirectToAction("Edit", new { id = objProduct.Id });
            }
        }

    }
}