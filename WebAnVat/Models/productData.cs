using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAnVat.Models
{
    public class productData
    {
        public int ID_Mon { get; set; }
        public int ID_LoaiMonAn { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string Sweet { get; set; }
        public string Tea { get; set; }
        public string Ice { get; set; }
        public decimal Price { get; set; }
        public List<toppingData> Topping { get; set; }
    }

    public class toppingData
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }


}