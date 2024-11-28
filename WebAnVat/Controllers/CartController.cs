using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAnVat.Models;

namespace WebAnVat.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public string conStr = "workstation id=BanDoAnVatVer2.mssql.somee.com;packet size=4096;user id=HuyKhiemLong123_SQLLogin_1;pwd=t5fpvoy7nc;data source=BanDoAnVatVer2.mssql.somee.com;persist security info=False;initial catalog=BanDoAnVatVer2;TrustServerCertificate=True";
        public ActionResult deleteItem(int ID_GioHang)
        {
            string query = "delete from GioHang where ID_GioHang = @ID_GioHang";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_GioHang", ID_GioHang);
                cmd.ExecuteNonQuery();
            }    
            return RedirectToAction("loadCart","Home");
        }

        public ActionResult editItem(int ID_GioHang, int ID_Mon)
        {
            string query = "delete from GioHang where ID_GioHang = @ID_GioHang";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_GioHang", ID_GioHang);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("detailProducts", "Home", new {id = ID_Mon});
        }

    }
}