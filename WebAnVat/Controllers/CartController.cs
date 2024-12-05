using Newtonsoft.Json;
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
        //public ActionResult deleteItem(int ID_GioHang)
        //{
        //    string query = "delete from GioHang where ID_GioHang = @ID_GioHang";
        //    using (SqlConnection conn = new SqlConnection(conStr))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@ID_GioHang", ID_GioHang);
        //        cmd.ExecuteNonQuery();
        //    }    
        //    return RedirectToAction("loadCart","Home");
        //}

        
        public ActionResult deleteItem(int  id_mon)
        {
            List<productData> cart = Session["Cart"] as List<productData>;
            if (cart == null || cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng hiện đang trống.";
                return RedirectToAction("loadCart", "Home");
            }
            for (int i = cart.Count - 1; i >= 0; i--)
            {
                if (cart[i].ID_Mon == id_mon)
                {
                    cart.RemoveAt(i);
                    break;
                }
            }
            // Cập nhật lại session
            Session["Cart"] = cart;
            return RedirectToAction("loadCart", "Home");
        }
        [HttpPost]
        public ActionResult Pay(DonHang model)
        {
            // thêm thông tin vào DonHang
            string query = "insert into DonHang(ID_NguoiMua,NgayDatDon,PTThanhToan,DiaChi,GhiChu,TienGiaoHang,DienThoai)values(@ID_NguoiMua,@NgayDatDon,@PTThanhToan,@DiaChi,@GhiChu,@TienGiaoHang,@DienThoai)";
            int idUser = Convert.ToInt32(Session["ID"]);
            decimal tiengiaohang = 150000;
            DateTime ngaydat = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_NguoiMua", idUser);
                cmd.Parameters.AddWithValue("@NgayDatDon", ngaydat);
                cmd.Parameters.AddWithValue("@PTThanhToan", model.PTThanhToan);
                cmd.Parameters.AddWithValue("@DiaChi", model.DiaChi);
                cmd.Parameters.AddWithValue("@GhiChu", model.GhiChu);
                cmd.Parameters.AddWithValue("@TienGiaoHang", tiengiaohang);
                cmd.Parameters.AddWithValue("@DienThoai", model.DienThoai);
                cmd.ExecuteNonQuery();
            }
            // lấy ra ID_DonHang
            string queryfind_id = "select ID_DonHang from DonHang where ID_NguoiMua=@id_user  and ChiTiet=0";
            int id_donhang = 0;
            using (SqlConnection conn1 = new SqlConnection(conStr))
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
            // lấy ra session lưu trữ các món đã đặt
            List<productData> cart = Session["Cart"] as List<productData>;
            if (cart == null || cart.Count == 0)
            {
                return View(new List<productData>()); // Trả về giỏ hàng rỗng nếu không có sản phẩm
            }
            // thực hiện thêm vào chi tiết đơn hàng 
            for (int i = cart.Count - 1; i >= 0; i--)
            {
                 string queryctdh = "insert into ChiTietDonHang (ID_Don,ID_Mon,SoLuong,GiaThucTe,ChiTietMonAn_JSON,ThanhTien) " +
                        "values (@ID_Don,@ID_Mon,@SoLuong,@GiaThucTe,@ChiTietMonAn_JSON,@ThanhTien)";
                using (SqlConnection conn3 = new SqlConnection(conStr))
                {
                    conn3.Open();
                    SqlCommand cmd = new SqlCommand(queryctdh, conn3);
                    cmd.Parameters.AddWithValue("@ID_Don", id_donhang);
                    cmd.Parameters.AddWithValue("@ID_Mon", cart[i].ID_Mon);
                    cmd.Parameters.AddWithValue("@SoLuong", cart[i].Quantity);
                    cmd.Parameters.AddWithValue("@GiaThucTe", cart[i].Price);
                    cmd.Parameters.AddWithValue("@ThanhTien", cart[i].Price);
                    string toppingJson = JsonConvert.SerializeObject(cart[i].Topping);
                    cmd.Parameters.AddWithValue("@ChiTietMonAn_JSON", toppingJson);
                    cmd.ExecuteNonQuery();
                }
                // sau khi thêm vào chi tiết đơn hàng sẽ xóa món đã đặt khỏi session
                cart.Remove(cart[i]);
                Session["Cart"] = cart;
            }
            string queryupdate = "update  DonHang set ChiTiet=1 where ID_NguoiMua=@iduser and ChiTiet=0";
            using (SqlConnection conn4 = new SqlConnection(conStr))
            {
                conn4.Open();
                SqlCommand cmd=new SqlCommand(queryupdate, conn4);
                cmd.Parameters.AddWithValue("@iduser", idUser);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}