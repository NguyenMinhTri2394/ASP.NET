using QLBH_ASP.Context;
using QLBH_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_ASP.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        WebsiteBanHangEntities4 objWebsiteBanHangEntities = new WebsiteBanHangEntities4();
        // GET: Payment
        public ActionResult Index()
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                // lấy thông tin từ giỏ hàng trong session
                var istCart = (List<CartModel>)Session["cart"];
                if (istCart == null || !istCart.Any())
                {
                    // Giỏ hàng trống, bạn có thể thông báo hoặc làm gì đó ở đây
                    TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm vào giỏ hàng.";
                    return RedirectToAction("Index", "Home"); // Hoặc chuyển hướng khác
                }

                // tạo dữ liệu cho Order
                Order objOrder = new Order();
                objOrder.Name = "DonHang-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                objOrder.Id = int.Parse(Session["idUser"].ToString());
                objOrder.CreatedOnUtc = DateTime.Now;
                objOrder.Status = 17;

                objWebsiteBanHangEntities.Orders.Add(objOrder);

                // lưu thông tin vào bảng Order
                objWebsiteBanHangEntities.SaveChanges();

                // Lấy OrderId vừa tạo để lưu vào bảng OrderDetail
                int orderId = objOrder.Id;
                List<Order_Detail> lstOrderDetail = new List<Order_Detail>();

                foreach (var item in istCart)
                {
                    var product = objWebsiteBanHangEntities.Products.FirstOrDefault(p => p.Id == item.Product.Id);

                    if (product == null)
                    {
                        // Nếu sản phẩm không tồn tại trong cơ sở dữ liệu, bạn có thể thông báo lỗi
                        TempData["ErrorMessage"] = "Sản phẩm trong giỏ hàng không tồn tại. Vui lòng thử lại.";
                        return RedirectToAction("Index", "Home");
                    }
                    Order_Detail obj = new Order_Detail();
                    obj.Quantity = item.Quantity;
                    obj.OrderId = orderId;
                    obj.ProductId = item.Product.Id;
                    lstOrderDetail.Add(obj);
                }

                objWebsiteBanHangEntities.Order_Detail.AddRange(lstOrderDetail);
                objWebsiteBanHangEntities.SaveChanges();
                ViewBag.OrderDetails = lstOrderDetail;
                ViewBag.TotalAmount = lstOrderDetail.Sum(m => m.Quantity * m.Product.Price);


                // Xóa giỏ hàng sau khi thanh toán thành công
                Session["cart"] = null;
                Session["count"] = 0;
            }

            return View();
        }
    }
}