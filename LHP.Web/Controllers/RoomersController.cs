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
using LHP.Web.ViewModels;

namespace LHP.Web.Controllers
{
    public class RoomersController : Controller
    {
        private LHPDbContext dblhp = new LHPDbContext();

        // GET: Roomers
        public ActionResult Index()
        {
            return View(dblhp.Roomers.ToList());
        }


        public RoomerVm LoadForm(Roomer roomer)
        {
            RoomerVm vm = new RoomerVm();
            if (roomer != null)
            {
                vm = new RoomerVm()
                {
                    RoomerId = roomer.RoomerId,
                    Name = roomer.Name,
                    Email = roomer.Email,
                    Comment = roomer.Comment,
                    Phone = roomer.Phone,
                    Registrated = roomer.Registrated,
                    Identification = roomer.Identification
                };
            }
            else
            {
                vm.Registrated = DateTime.Now.Date;
            }

            vm.Employers = dblhp.Employers.Select(x => new SelectListItem()
            {
                Value = x.EmployerId.ToString(),
                Text = x.Name
            }).ToList();

            return vm;
        }

        public bool SaveForm(RoomerVm vm)
        {
            using (var db = new LHPDbContext())
            {

                Roomer dbRoomer = null;
                int state = 1;

                while (ModelState.IsValid && state > 0)
                {
                    switch (state)
                    {
                        case 1:
                            if (string.IsNullOrEmpty(vm.Name))
                                ModelState.AddModelError("","Gæsten skal have et navn");
                            state++;
                            break;
                        case 2:
                            state++;
                            break;
                        case 3:
                            try
                            {
                                if (vm.RoomerId > 0)
                                {
                                    dbRoomer = db.Roomers.FirstOrDefault(x => x.RoomerId == vm.RoomerId);
                                    if (dbRoomer == null)
                                          throw new Exception(string.Format("Hmmm... gæsten med nr {0} blev ikke fundet.",vm.RoomerId));
                                }
                                else
                                {
                                    dbRoomer = new Roomer();
                                }
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("","Fejl: " + ex.Message);
                            }
                            state++;
                            break;
                        case 4:
                            dbRoomer.Name = vm.Name;
                            dbRoomer.Comment = vm.Comment;
                            dbRoomer.Email = vm.Email;
                            dbRoomer.Identification = vm.Identification;
                            dbRoomer.Registrated = vm.Registrated;
                            if (vm.Employer != null)
                            {
                                var emp = db.Employers.FirstOrDefault(x => x.EmployerId == vm.Employer.EmployerId);
                                if (emp != null)
                                    dbRoomer.Employer = emp;
                            }
                            state++;
                            break;

                        case 5:
                            if (vm.RoomerId > 0)
                            {
                                db.Entry(dbRoomer).State = EntityState.Modified;
                            }
                            else
                            {
                                db.Roomers.Add(dbRoomer);
                            }
                            state++;
                            break;
                        default:                            
                            db.SaveChanges();
                            state = 0;
                            break;
                    }
                }

                if (state == 0)
                    return true;

                vm.Employers = db.Employers.Select(x => new SelectListItem()
                {
                    Value = x.EmployerId.ToString(),
                    Text = x.Name
                }).ToList();

                return false;

            }
        }
        // GET: Roomers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roomer roomer = dblhp.Roomers.Find(id);
            if (roomer == null)
            {
                return HttpNotFound();
            }
            return View(roomer);
        }

        // GET: Roomers/Create
        public ActionResult Create()
        {
            var vm = LoadForm(null);
            return View(vm);
        }

        // POST: Roomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomerVm roomer)
        {
            if (SaveForm(roomer))
                return RedirectToAction("Index");

            return View(roomer);
        }

        // GET: Roomers/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            using (var db = new LHPDbContext())
            {
                Roomer roomer = dblhp.Roomers.Find(id);
                if (roomer == null)
                {
                    return HttpNotFound();
                }

                var vm = LoadForm(roomer);
                return View(vm);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomerVm roomer)
        {

            if (SaveForm(roomer))
               return RedirectToAction("Index");

            return View(roomer);
        }

        // GET: Roomers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roomer roomer = dblhp.Roomers.Find(id);
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
            Roomer roomer = dblhp.Roomers.Find(id);
            dblhp.Roomers.Remove(roomer);
            dblhp.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dblhp.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
