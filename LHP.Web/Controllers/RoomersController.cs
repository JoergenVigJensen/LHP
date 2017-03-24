using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LHP.DAL;
using LHP.DAL.Models;

namespace LHP.Web.Controllers
{
    public class RoomersController : Controller
    {
        private LHPDbContext db = new LHPDbContext();

        // GET: Roomers
        public ActionResult Index()
        {
            return View(db.Roomers.ToList());
        }

        // GET: Roomers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roomer roomer = db.Roomers.Find(id);
            if (roomer == null)
            {
                return HttpNotFound();
            }
            return View(roomer);
        }

        // GET: Roomers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomerId,Name,Phone,Email,Registrated,Indentification,Comment")] Roomer roomer)
        {
            if (ModelState.IsValid)
            {
                db.Roomers.Add(roomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomer);
        }

        // GET: Roomers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roomer roomer = db.Roomers.Find(id);
            if (roomer == null)
            {
                return HttpNotFound();
            }
            return View(roomer);
        }

        // POST: Roomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomerId,Name,Phone,Email,Registrated,Indentification,Comment")] Roomer roomer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomer);
        }

        // GET: Roomers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roomer roomer = db.Roomers.Find(id);
            if (roomer == null)
            {
                return HttpNotFound();
            }
            return View(roomer);
        }

        // POST: Roomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Roomer roomer = db.Roomers.Find(id);
            db.Roomers.Remove(roomer);
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
