using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebAnVat.Models;
using System.Drawing;
using System.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAnVat.Controllers
{
    public class HomeController : Controller
    {
        public string conStr = "workstation id=BanDoAnVatVer2.mssql.somee.com;packet size=4096;user id=HuyKhiemLong123_SQLLogin_1;pwd=t5fpvoy7nc;data source=BanDoAnVatVer2.mssql.somee.com;persist security info=False;initial catalog=BanDoAnVatVer2;TrustServerCertificate=True";
        // GET: Home
        public ActionResult Index()
        {
            List<Mon> ds = new List<Mon>();
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                string query = "select * from Mon";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Mon m = new Mon();
                    m.ID_Mon = int.Parse(rd.GetValue(0).ToString());
                    m.TenMon = rd.GetValue(1).ToString();
                    m.GiaBan = int.Parse(rd.GetValue(2).ToString());
                    m.ID_LoaiMonAn = int.Parse(rd.GetValue(3).ToString());
                    m.HinhAnh = rd.GetValue(4).ToString();
                    m.KhuyenMai = int.Parse(rd.GetValue(5).ToString());
                    m.GiaSauKhiGiam = int.Parse(rd.GetValue(6).ToString());
                    ds.Add(m);
                }
            }
            var foods = ds.Where(t => t.ID_LoaiMonAn == 103).ToList();
            var drinks = ds.Where(t => t.ID_LoaiMonAn == 101 || t.ID_LoaiMonAn == 102 || t.ID_LoaiMonAn == 100).ToList();

            ViewBag.listDrink = drinks;
            ViewBag.listFood = foods;
            return View();
        }

        public ActionResult searchByName(string tenmon)
        {
            List<Mon> ds = new List<Mon>();
            using(SqlConnection conn = new SqlConnection(conStr))
            {
                string query = "select * from Mon where TenMon like '%' +@tenmon+ '%'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tenmon", tenmon);
                SqlDataReader rd = cmd.ExecuteReader();
                while(rd.Read())
                {
                    Mon m = new Mon();
                    m.TenMon = rd.GetValue(1).ToString();
                    m.GiaBan = int.Parse(rd.GetValue(2).ToString());
                    m.ID_LoaiMonAn = int.Parse(rd.GetValue(3).ToString());
                    m.HinhAnh = rd.GetValue(4).ToString();
                    m.KhuyenMai = int.Parse(rd.GetValue(5).ToString());
                    m.GiaSauKhiGiam = int.Parse(rd.GetValue(6).ToString());
                    ds.Add(m);
                }    

            }    
            return View(ds);
        }


        public ActionResult detailProducts(int id)
        {   
            //Trả về sản phẩm vừa click vào bằng id
            Mon monDetail = null;
            string query1 = "select * from Mon m where m.ID_Mon = @id";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query1, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    monDetail = new Mon();
                    monDetail.ID_Mon = int.Parse(rd.GetValue(0).ToString());
                    monDetail.TenMon = rd.GetValue(1).ToString();
                    monDetail.GiaBan = int.Parse(rd.GetValue(2).ToString());
                    monDetail.ID_LoaiMonAn = int.Parse(rd.GetValue(3).ToString());
                    monDetail.HinhAnh = rd.GetValue(4).ToString();
                    monDetail.GiaSauKhiGiam = int.Parse(rd.GetValue(6).ToString());
                }
            }
            //Lấy ra Size và giá tăng theo size 
            List<ChiTietSize> s = new List<ChiTietSize>();
            string query3 = "select * from ChiTietSize";
            using(SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query3, conn);
                SqlDataReader rd = cmd.ExecuteReader();
                while(rd.Read())
                {
                    ChiTietSize cts = new ChiTietSize();
                    cts.ID_Size = int.Parse(rd.GetValue(0).ToString());
                    cts.Loai_Size = rd.GetValue(1).ToString();
                    cts.GiaTang = int.Parse(rd.GetValue(2).ToString());
                    s.Add(cts);
                }    
            }

            var btnSizes = s;
            ViewBag.btnSizes = btnSizes;

            //Lấy ra danh sách topping theo Loai_Topping
            List<ChiTietTopping> ds = new List<ChiTietTopping>();
            string query2 = "select * from ChiTietTopping";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query2, conn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    ChiTietTopping cttp = new ChiTietTopping();
                    cttp.ID_Topping = int.Parse(rd.GetValue(0).ToString());
                    cttp.Loai_Topping = rd.GetValue(1).ToString();
                    cttp.Ten_Topping = rd.GetValue(2).ToString();
                    cttp.GiaTopp = int.Parse(rd.GetValue(3).ToString());
                    ds.Add(cttp);
                }    
            } 

            var toppTraSua = ds.Where(t => t.Loai_Topping == "Trà sữa" || t.Loai_Topping == "Nước").ToList();
            var toppTra = ds.Where(t => t.Loai_Topping == "Trà" || t.Loai_Topping == "Nước").ToList();
            var toppAnVat = ds.Where(t => t.Loai_Topping == "Ăn Vặt").ToList();
            ViewBag.toppTraSua = toppTraSua;
            ViewBag.toppTra = toppTra;
            ViewBag.toppAnVat = toppAnVat;

            return View(monDetail);
        }

        [HttpPost]
        public ActionResult addToCart(string jsonProductData)
        {
            string data = jsonProductData.ToString();
            int idUser = 104;
            int idMon = 108;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                string query = "insert into GioHang values(@idNgMua, @idMon, @productData)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idNgMua", idUser);
                cmd.Parameters.AddWithValue("@idMon", idMon);
                cmd.Parameters.AddWithValue("@productData", data);
                cmd.ExecuteNonQuery();
            }
            return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng" });
        }
    }
}