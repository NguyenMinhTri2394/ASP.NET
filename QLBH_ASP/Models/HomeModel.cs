using QLBH_ASP.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH_ASP.Models
{
    public class HomeModel
    {
        public List<Product> ListProduct { get; set; }
        public List<Category> ListCategory { get; set; }
    }
}