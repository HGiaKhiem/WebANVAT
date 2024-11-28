using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebAnVat.Models;

namespace WebAnVat.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public string conStr = "workstation id=BanDoAnVatVer2.mssql.somee.com;packet size=4096;user id=HuyKhiemLong123_SQLLogin_1;pwd=t5fpvoy7nc;data source=BanDoAnVatVer2.mssql.somee.com;persist security info=False;initial catalog=BanDoAnVatVer2;TrustServerCertificate=True";

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string mk)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(mk))
            {
                ViewBag.Massage = "Tên đăng nhập hoặc mật khẩu không được để trống";
                return View();
            }    
            NguoiMua user = null;
            using (SqlConnection conn = new SqlConnection(conStr))
            {

                conn.Open();
                string sql = "SELECT * FROM NguoiMua WHERE Email = @Email AND MatKhau = @MatKhau ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@MatKhau", mk);

                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    user = new NguoiMua();
                    user.ID_NgMua = int.Parse(rd.GetValue(0).ToString());
                    user.Email = rd.GetValue(3).ToString();
                    user.MatKhau = rd.GetValue(4).ToString();
                }
                if(user != null)
                {
                    Session["ID"] = user.ID_NgMua;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ViewBag.Message = "Đăng nhập không thành công";
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(NguoiMua model, string confirmMatKhau)
        {
            if (model.MatKhau != confirmMatKhau)
            {
                ViewBag.Message = "Mật khẩu và xác nhận mật khẩu không khớp.";
                return View();
            }
            // Kiểm tra các trường không được để trống
            if (string.IsNullOrEmpty(model.Ten) || string.IsNullOrEmpty(model.Sdt) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.MatKhau))
            {
                ViewBag.Message = "Điền đầy đủ thông tin";
                return View();
            }

            // Kiểm tra xem email đã tồn tại chưa
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                string checkSql = "SELECT count(*) FROM NguoiMua WHERE Email = @Email";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@Email", model.Email);
                int emailCount = (int)checkCmd.ExecuteScalar();

                if (emailCount > 0)
                {
                    ViewBag.Message = "Email đã tồn tại";
                    return View();
                }
                // Nếu đăng nhập là hợp lệ thì thêm người dùng mới vào
                string insertSql = "INSERT INTO NguoiMua (Ten, Sdt, MatKhau, Email) VALUES (@Ten, @Sdt, @MatKhau, @Email)";
                SqlCommand insertCmd = new SqlCommand(insertSql, conn);
                insertCmd.Parameters.AddWithValue("@Ten", model.Ten);
                insertCmd.Parameters.AddWithValue("@Sdt", model.Sdt);
                insertCmd.Parameters.AddWithValue("@Email", model.Email);
                insertCmd.Parameters.AddWithValue("@MatKhau", model.MatKhau);
                insertCmd.ExecuteNonQuery();
            }

            ViewBag.Message = "Đăng ký thành công";
            return RedirectToAction("Login");
        }

        public ActionResult ForgetPW()
        {
            return View();  // Hiển thị trang quên mật khẩu
        }

        [HttpPost]
        public ActionResult ForgetPW(NguoiMua model)
        {
            if (ModelState.IsValid)  // Kiểm tra tính hợp lệ của model
            {
                // Kiểm tra xem email có tồn tại trong cơ sở dữ liệu không
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    string checkSql = "SELECT COUNT(*) FROM NguoiMua WHERE Email = @Email";
                    SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@Email", model.Email);
                    int emailCount = (int)checkCmd.ExecuteScalar();

                    if (emailCount == 0)
                    {
                        ViewBag.Message = "Email không tồn tại trong hệ thống.";
                        return View();  // Nếu không tìm thấy email trong cơ sở dữ liệu
                    }

                    // Tạo mã xác thực hoặc liên kết khôi phục mật khẩu
                    string resetToken = Guid.NewGuid().ToString();  // Sử dụng GUID làm mã xác thực
                    string resetLink = Url.Action("ResetPW", "Account", new { token = resetToken }, Request.Url.Scheme);

                    // Gửi email khôi phục mật khẩu
                    SendResetPasswordEmail(model.Email, resetLink);
                    ViewBag.Message = "Liên kết khôi phục mật khẩu đã được gửi đến email của bạn.";
                }
            }

            return View();  // Trả lại View nếu có lỗi hoặc thông báo
        }

        private void SendResetPasswordEmail(string email, string resetLink)
        {
            try
            {
                // Tạo đối tượng MailMessage
                var mailMessage = new MailMessage("your-email@gmail.com", email)
                {
                    Subject = "Khôi phục mật khẩu",
                    Body = $"Nhấn vào liên kết sau để khôi phục mật khẩu của bạn: <a href='{resetLink}'>Khôi phục mật khẩu</a>",
                    IsBodyHtml = true
                };

                // Cấu hình SMTPClient
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587; // Cổng SMTP với TLS
                    smtpClient.EnableSsl = true; // Bật SSL/TLS
                    smtpClient.Credentials = new NetworkCredential("your-email@gmail.com", "your-email-password"); // Thông tin xác thực

                    // Gửi email
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi nếu có
                Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
                ViewBag.Message = $"Đã xảy ra lỗi khi gửi email: {ex.Message}";
            }
        }
    }
}

