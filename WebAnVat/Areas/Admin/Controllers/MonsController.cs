using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAnVat.Models;
using System.IO;  


namespace WebAnVat.Areas.Admin.Controllers
{
    public class MonsController : Controller
    {
        private BanDoAnVatVer2Entities db = new BanDoAnVatVer2Entities();

        // GET: Admin/Mons
        public ActionResult Index()
        {
            var mons = db.Mons.Include(m => m.LoaiMonAn);
            return View(mons.ToList());
        }

        // GET: Admin/Mons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mon mon = db.Mons.Find(id);
            if (mon == null)
            {
                return HttpNotFound();
            }
            return View(mon);
        }

        // GET: Admin/Mons/Create
        // GET: Admin/Mons/Create
        public ActionResult Create()
        {
            ViewBag.ID_LoaiMonAn = new SelectList(db.LoaiMonAns, "ID_LoaiMonAn", "TenLoaiMonAn");
            return View();
        }

        // POST: Admin/Mons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Mon,TenMon,GiaBan,ID_LoaiMonAn,HinhAnh,KhuyenMai,GiaSauKhiGiam")] Mon mon, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(HinhAnh.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/asset/img/Products/"), fileName);
                    HinhAnh.SaveAs(filePath);

                    mon.HinhAnh = "~/asset/img/Products/" + fileName;
                }

                db.Mons.Add(mon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_LoaiMonAn = new SelectList(db.LoaiMonAns, "ID_LoaiMonAn", "TenLoaiMonAn", mon.ID_LoaiMonAn);
            return View(mon);
        }


        // GET: Admin/Mons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mon mon = db.Mons.Find(id);
            if (mon == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_LoaiMonAn = new SelectList(db.LoaiMonAns, "ID_LoaiMonAn", "TenLoaiMonAn", mon.ID_LoaiMonAn);
            return View(mon);
        }

        // POST: Admin/Mons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Mon,TenMon,GiaBan,ID_LoaiMonAn,HinhAnh,KhuyenMai,GiaSauKhiGiam")] Mon mon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_LoaiMonAn = new SelectList(db.LoaiMonAns, "ID_LoaiMonAn", "TenLoaiMonAn", mon.ID_LoaiMonAn);
            return View(mon);
        }

        // GET: Admin/Mons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mon mon = db.Mons.Find(id);
            if (mon == null)
            {
                return HttpNotFound();
            }
            return View(mon);
        }

        // POST: Admin/Mons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mon mon = db.Mons.Find(id);
            db.Mons.Remove(mon);
            db.SaveChanges();
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
