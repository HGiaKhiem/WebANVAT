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
        [HttpPost]
        public ActionResult Pay(DonHang model)
        {
            string query = "insert into DonHang(ID_NguoiMua,NgayDatDon,PTThanhToan,DiaChi,GhiChu,TienGiaoHang,DienThoai)values(@ID_NguoiMua,@NgayDatDon,@PTThanhToan,@DiaChi,@GhiChu,@TienGiaoHang,@DienThoai)";
            int idUser = Convert.ToInt32(Session["ID"]);
            decimal tiengiaohang = 150000;
            DateTime dathang = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_NguoiMua", idUser);
                cmd.Parameters.AddWithValue("@NgayDatDon", dathang);
                cmd.Parameters.AddWithValue("@PTThanhToan", model.PTThanhToan);
                cmd.Parameters.AddWithValue("@DiaChi", model.DiaChi);
                cmd.Parameters.AddWithValue("@GhiChu", model.GhiChu);
                cmd.Parameters.AddWithValue("@TienGiaoHang", tiengiaohang);
                cmd.Parameters.AddWithValue("@DienThoai", model.DienThoai);
                cmd.ExecuteNonQuery();
            }

            string queryfind_id = "select ID_DonHang from DonHang where ID_NguoiMua=@id_user  and ChiTiet=0";
            int id_donhang = 0;
            using (SqlConnection conn1 = new SqlConnection())
            {
                conn1.Open();
                SqlCommand cmd = new SqlCommand(queryfind_id, conn1);
                cmd.Parameters.AddWithValue("@id_user", idUser);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id_donhang = int.Parse(reader.GetValue(0).ToString());
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}