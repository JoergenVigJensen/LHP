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
                    Identification = roomer.Identification,
                    CoContact =  roomer.CoContact,
                    Deposit = roomer.Deposit
                };
                vm.Room = dblhp.Rooms.FirstOrDefault(x => x.CurrentRoomer.RoomerId == roomer.RoomerId);
            }
            else
            {
                vm.Registrated = DateTime.Now.Date;
            }

            vm.CoContacts = dblhp.CoContacts.Select(x => new SelectListItem()
            {
                Value = x.COContactId.ToString(),
                Text = x.Name
            }).ToList();

            vm.RoomList = dblhp.Rooms.Where(x=>x.CurrentRoomer == null || x.CurrentRoomer.RoomerId == roomer.RoomerId).Select(x => new SelectListItem()
            {
                Value = x.RoomId.ToString(),
                Text = x.RoomNb + " - " + x.Type.Description
            }).ToList();
            vm.RoomList.Add(new SelectListItem()
            {
                Value = "",
                Text = "Ikke tilknyttet værelse."
            });


            return vm;
        }

        public bool SaveForm(RoomerVm vm)
        {
            using (var db = new LHPDbContext())
            {

                Roomer dbRoomer = null;
                Room dbRoom = null;
                Room newRoom = null;

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

                                    dbRoom = db.Rooms.FirstOrDefault(x => x.CurrentRoomer.RoomerId == vm.RoomerId);
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
                            dbRoomer.Deposit = vm.Deposit;
                            if (vm.CoContact != null)
                            {
                                var emp = db.CoContacts.FirstOrDefault(x => x.COContactId == vm.CoContact.COContactId);
                                if (emp != null)
                                    dbRoomer.CoContact = emp;
                            }
                            state++;
                            break;
                        case 5:
                            if (dbRoom != null)
                            {
                                if (vm.Room.RoomId != dbRoom.RoomId)
                                {
                                    dbRoom.CurrentRoomer = null;
                                    if (vm.Room.RoomId > 0)
                                        newRoom = db.Rooms.FirstOrDefault(x => x.RoomId == vm.Room.RoomId);
                                    if (newRoom != null)
                                        newRoom.CurrentRoomer = dbRoomer;
                                }
                            }
                            state++;
                            break;
                        case 6:                            
                            if (vm.RoomerId > 0)
                            {
                                db.Entry(dbRoomer).State = EntityState.Modified;
                            }
                            else
                            {
                                db.Roomers.Add(dbRoomer);
                            }
                            if (dbRoom != null)
                                db.Entry(dbRoom).State = EntityState.Modified;

                            if (newRoom != null)
                                db.Entry(newRoom).State = EntityState.Modified;

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

                vm.CoContacts = db.CoContacts.Select(x => new SelectListItem()
                {
                    Value = x.COContactId.ToString(),
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
                Roomer roomer = dblhp
                    .Roomers
                    .Include(x=>x.CoContact)
                    .FirstOrDefault(x=>x.RoomerId == id);
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
