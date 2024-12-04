using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAnVat.Models;

namespace WebAnVat.Areas.Admin.Controllers
{
    public class NguoiMuasController : Controller
    {
        private BanDoAnVatVer2Entities db = new BanDoAnVatVer2Entities();

        // GET: Admin/NguoiMuas
        public ActionResult Index()
        {
            return View(db.NguoiMuas.ToList());
        }

        // GET: Admin/NguoiMuas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiMua nguoiMua = db.NguoiMuas.Find(id);
            if (nguoiMua == null)
            {
                return HttpNotFound();
            }
            return View(nguoiMua);
        }

        // GET: Admin/NguoiMuas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NguoiMuas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_NgMua,Ten,Sdt,Email,MatKhau,UserType")] NguoiMua nguoiMua)
        {
            if (ModelState.IsValid)
            {
                db.NguoiMuas.Add(nguoiMua);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nguoiMua);
        }

        // GET: Admin/NguoiMuas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiMua nguoiMua = db.NguoiMuas.Find(id);
            if (nguoiMua == null)
            {
                return HttpNotFound();
            }
            return View(nguoiMua);
        }

        // POST: Admin/NguoiMuas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_NgMua,Ten,Sdt,Email,MatKhau,UserType")] NguoiMua nguoiMua)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nguoiMua).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nguoiMua);
        }

        // GET: Admin/NguoiMuas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiMua nguoiMua = db.NguoiMuas.Find(id);
            if (nguoiMua == null)
            {
                return HttpNotFound();
            }
            return View(nguoiMua);
        }

        // POST: Admin/NguoiMuas/Delete/5
        // POST: Admin/NguoiMuas/Delete/5
        // POST: Admin/NguoiMuas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NguoiMua nguoiMua = db.NguoiMuas.Find(id);

            if (nguoiMua.DonHangs.Count > 0)
            {
                TempData["ErrorMessage"] = "Không thể xóa người mua vì họ đã có đơn hàng.";
                return RedirectToAction("Delete", new { id }); 
            }

            if (nguoiMua.GioHangs.Count > 0)
            {
                db.GioHangs.RemoveRange(nguoiMua.GioHangs);
                db.SaveChanges();

                db.NguoiMuas.Remove(nguoiMua);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Xóa người mua và giỏ hàng thành công!";
            }
            else
            {
                db.NguoiMuas.Remove(nguoiMua);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Xóa người mua thành công!";
            }

            return RedirectToAction("Index");  
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
