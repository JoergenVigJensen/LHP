using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using LHP.DAL;
using LHP.DAL.Models;
using LHP.Web.ViewModels;

namespace LHP.Web.Controllers
{
    public class RoomsController : Controller
    {
        private LHPDbContext db = new LHPDbContext();

        // GET: Rooms
        public ActionResult Index()
        {
            return View(db.Rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {

            var vm = LoadForm(null);

            return View(vm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomVm room)
        {

            if (SaveForm(room))
                return RedirectToAction("Index");

            return View(room);
        }

        private bool SaveForm(RoomVm room)
        {
            int roomtypeId = 0;

            RoomType roomType = null;
            Room dbRoom = null;

            RoomProfileRelation profile = null;
            RoomProfileRelation newProfile = null;

            int state = 1;

            while (ModelState.IsValid && state > 0)
            {
                switch (state)
                {
                    case 1:
                        {
                            if (string.IsNullOrEmpty(room.RoomNb))
                                ModelState.AddModelError("","Der skal angives et værelsesnummer.");
                            state++;
                            break;
                        }
                    case 2:
                        {
                            roomType = db.RoomTypes.Find(room.Type.RoomTypeId);
                            if (roomType == null)
                                ModelState.AddModelError("", "Værelsestype blev ikke fundet.");
                            state++;
                            break;
                        }
                    case 3:
                        {
                            state++;
                            break;
                        }
                    case 4:
                        {
                            try
                            {
                                if (roomType == null)
                                    throw new Exception("Der er ikke valg værelsestype.");

                                if (room.RoomId > 0)
                                {
                                    dbRoom = db.Rooms
                                        .Include(x => x.Type)
                                        .Include(x => x.CurrentRoomer)
                                        .FirstOrDefault(x => x.RoomId == room.RoomId);
                                    if (dbRoom == null)
                                        throw new Exception(string.Format("Værelse {0} blev ikke fundet", room.RoomId));
                                }
                                else
                                {
                                    dbRoom = new Room();
                                }
                                if (room.Type != dbRoom.Type)
                                {
                                    profile =
                                        db.RoomProfileRelations.OrderByDescending(x => x.RoomProfileRelationId)
                                            .FirstOrDefault(x => x.Room.RoomId == dbRoom.RoomId);
                                    if (profile != null)
                                        profile.DeActivated = DateTime.Now;
                                    newProfile = new RoomProfileRelation()
                                    {
                                        Room = dbRoom,
                                        RoomType = room.Type,
                                        Activated = DateTime.Now
                                    };
                                }
                                dbRoom.Type = roomType;
                                dbRoom.RoomNb = room.RoomNb;
                                
                                state++;
                                break;
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("", ex.Message);
                            }
                            state++;
                            break;
                        }
                    case 5:
                        {
                            try
                            {
                                if (dbRoom == null)
                                    throw new Exception("Værelset kan ikke gemmes.");

                                if (room.RoomId > 0)
                                {
                                    db.Entry(dbRoom).State = EntityState.Modified;
                                }
                                else
                                {
                                    db.Rooms.Add(dbRoom);
                                }

                                if (profile != null)
                                    db.Entry(profile).State = EntityState.Modified;

                                db.RoomProfileRelations.Add(newProfile);
                            }

                            catch (Exception ex)
                            {
                                ModelState.AddModelError("", ex.Message);
                            }
                            state++;
                            break;
                        }
                    default:
                        {
                            db.SaveChanges();
                            state = 0;
                            break;
                        }
                }
            }

            if (state == 0)
                return true;

            return false;
        }

        private RoomVm LoadForm(Room room)
        {
            var vm = new RoomVm();
            if (room != null)
            {
                vm = new RoomVm()
                {
                    RoomId = room.RoomId,
                    Type = room.Type,
                    CurretRoomer = (room.CurrentRoomer == null) ? "" : room.CurrentRoomer.RoomerId.ToString(),
                    RoomNb = room.RoomNb,
                    Active = room.Active,
                    RoomTypeId =  room.Type.RoomTypeId
                };
            }
            else
            {
                vm.Type = new RoomType(){RoomTypeId = 0, Description =  "Vælg en type"};
                vm.RoomNb = "";
            }
            vm.RoomTypes = db.RoomTypes.Select(x => new SelectListItem()
            {
                Value = x.RoomTypeId.ToString(),
                Text = x.Description + " " + x.Amount.ToString()
            }).ToList();

            vm.ActiveRoomers = db.Roomers.Select(x => new SelectListItem()
            {
                Value = x.RoomerId.ToString(),
                Text = x.Name
            }).ToList();

            return vm;
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms
                .Include(x => x.Type)
                .Include(x => x.CurrentRoomer)
                .FirstOrDefault(x => x.RoomId == id);

            if (room == null)
            {
                return HttpNotFound();
            }

            var vm = LoadForm(room);


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomVm room)
        {
            if (SaveForm(room))
                return RedirectToAction("Index");

            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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
