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
using Mvc.JQuery.DataTables;
using System.Globalization;

namespace LHP.Web.Controllers
{
    public class RoomsController : Controller
    {
        private LHPDbContext lhpDb = new LHPDbContext();

        // GET: Rooms
        public ActionResult Index()
        {

            var rooms = lhpDb.Rooms
                .Include(x => x.Type)
                .Include(x => x.CurrentRoomer).Select(x => new RoomListVm()
                {
                    Id = x.RoomId,
                    RoomNb = x.RoomNb,
                    Active = x.Active,
                    RoomType = x.Type.Description,
                    RoomAmount = x.Type.Amount,
                    RoomerName = (x.CurrentRoomer == null) ? "" : x.CurrentRoomer.Name
                });

            return View(rooms);
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = lhpDb.Rooms.Find(id);
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
                            ModelState.AddModelError("", "Der skal angives et værelsesnummer.");
                        state++;
                        break;
                    }
                    case 2:
                    {
                        roomType = lhpDb.RoomTypes.Find(room.Type.RoomTypeId);
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
                                dbRoom = lhpDb.Rooms
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
                                    lhpDb.RoomProfileRelations.OrderByDescending(x => x.RoomProfileRelationId)
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
                            dbRoom.Active = room.Active;

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
                                lhpDb.Entry(dbRoom).State = EntityState.Modified;
                            }
                            else
                            {
                                lhpDb.Rooms.Add(dbRoom);
                            }

                            if (profile != null)
                                lhpDb.Entry(profile).State = EntityState.Modified;

                            lhpDb.RoomProfileRelations.Add(newProfile);
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
                        lhpDb.SaveChanges();
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
                    RoomTypeId = room.Type.RoomTypeId
                };
            }
            else
            {
                vm.Type = new RoomType() {RoomTypeId = 0, Description = "Vælg en type"};
                vm.RoomNb = "";
            }
            vm.RoomTypes = lhpDb.RoomTypes.Select(x => new SelectListItem()
            {
                Value = x.RoomTypeId.ToString(),
                Text = x.Description + " " + x.Amount.ToString()
            }).ToList();

            vm.ActiveRoomers = lhpDb.Roomers.Select(x => new SelectListItem()
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
            Room room = lhpDb.Rooms
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
            Room room = lhpDb.Rooms.Find(id);
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
            Room room = lhpDb.Rooms.Find(id);
            lhpDb.Rooms.Remove(room);
            lhpDb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lhpDb.Dispose();
            }
            base.Dispose(disposing);
        }

        public DataTablesResult<RoomListVm> GetRoomsDataTable(DataTablesParam dataTablesParam)
        {
            using (var db = new LHPDbContext())
            {
                var rooms = db.Rooms
                    .Include(x=>x.Type)
                    .Include(x=>x.CurrentRoomer)
                    .ToList();

                List<RoomListVm> roomList = new List<RoomListVm>();
                foreach (var room in rooms)
                {
                    var r = new RoomListVm()
                    {
                        Id = room.RoomId,
                        RoomNb = room.RoomNb,
                        Active = room.Active,
                        RoomerName = (room.CurrentRoomer == null)?"" : room.CurrentRoomer.Name,
                        RoomAmount = room.Type.Amount,
                        EditBtn =
                            "<a href='" + Url.Action("Edit", "Rooms", new {id = room.RoomId}) +
                            "' class='btn btn-primary btn-xs btn-fixed'></a>",
                        DetailsBtn =
                            "<a href='" + Url.Action("Details", "Rooms", new {id = room.RoomId}) +
                            "' class='btn btn-primary btn-xs btn-fixed'>Detaljer</a>",
                        BillBtn = ""
                    };

                    roomList.Add(r);
                }

                return DataTablesResult.Create(roomList.AsQueryable(), dataTablesParam,
                    x => new
                    {
                        RoomAmount = string.Format(CultureInfo.CurrentCulture, "{0:N2}", x.RoomAmount),
                        Active = (x.Active)? "Ja":"Nej"
                    });

            }
        }
    }
}
