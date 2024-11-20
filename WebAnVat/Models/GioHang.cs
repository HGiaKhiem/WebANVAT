

namespace WebAnVat.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GioHang
    {
        public int ID_GioHang { get; set; }
        public int ID_NgMua { get; set; }
        public int ID_Mon { get; set; }
        public string jsonProductData { get; set; }
    
        public virtual Mon Mon { get; set; }
        public virtual NguoiMua NguoiMua { get; set; }
    }
}
