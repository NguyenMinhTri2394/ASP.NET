using QLBH_ASP.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH_ASP.Models
{
    public class CartModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => (decimal)(Product.Price * Quantity); // Tính tổng tiền của sản phẩm
    }


}