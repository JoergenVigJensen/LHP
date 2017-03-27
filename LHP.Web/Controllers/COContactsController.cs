using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LHP.DAL;
using LHP.DAL.Models;
using LHP.Web.ViewModels;

namespace LHP.Web.Controllers
{
    public class COContactsController : Controller
    {
        private LHPDbContext lhpdb = new LHPDbContext();

        // GET: COContacts
        public ActionResult Index()
        {

            using (var db = new LHPDbContext())
            {
                var vm = db.CoContacts.Select(x => new COContactListVm()
                {
                    ContactId = x.COContactId,
                    Name = x.Name,
                    Contact = x.Contact,
                    Phone = x.Phone,
                    Email = x.Email
                }).ToList();

                return View(vm);
            }
        }


        private COContactVm LoadForm(COContact contact)
        {
            var vm = new COContactVm();
            if (contact != null)
            {
                string[] address = contact.Address.Split(';');

                vm = new COContactVm()
                {
                    COContactId = contact.COContactId,
                    Name = contact.Name,
                    Phone = contact.Phone,
                    Contact = contact.Contact,
                    Address1 = address[0],
                    Address2 = (address.Length > 1) ? address[1] : "",
                    ZipCode = contact.ZipCode,
                    City = contact.City,
                    Comment = contact.Comment,
                    Email = contact.Email
                };                
            }

            return vm;
        }


        private bool SaveForm(COContactVm contact)
        {
            COContact dbContact = null;

            int state = 1;
            string address = "";

            using (var db = new LHPDbContext())
            {
                while (state > 0 && ModelState.IsValid)
                {
                    switch (state)
                    {
                        case 1:
                            if (string.IsNullOrEmpty(contact.Name))
                                ModelState.AddModelError("", "Der skal angives et navn");
                            state++;
                            break;
                        case 2:
                            if (contact.Address2.Contains(';') || contact.Address2.Contains(';'))
                                ModelState.AddModelError("", "Adressen må ikke indeholde ';'");
                            state++;
                            break;
                        case 3:
                            address = contact.Address1 + ';' + contact.Address2;
                            state++;
                            break;
                        case 4:
                            try
                            {
                                if (contact.COContactId > 0)
                                    dbContact = db.CoContacts.First(x => x.COContactId == contact.COContactId);
                                else
                                {
                                    dbContact = new COContact();
                                }
                                if (dbContact == null)
                                    throw new Exception("Der blev ikke fundet en kontak.");

                                dbContact.Name = contact.Name;
                                dbContact.Address = address;
                                dbContact.City = contact.City;
                                dbContact.Email = contact.Email;
                                dbContact.Contact = contact.Contact;
                                dbContact.Phone = contact.Phone;
                                dbContact.ZipCode = contact.ZipCode;
                                dbContact.Comment = contact.Comment;

                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("",ex.Message);
                            }
                            state++;
                            break;
                        case 5:
                            try
                            {
                                if (dbContact.COContactId == 0)
                                    db.CoContacts.Add(dbContact);
                                else
                                {
                                    db.Entry(dbContact).State = EntityState.Modified;
                                }
                            }
                            catch (Exception ex)
                            {
                               ModelState.AddModelError("",ex.Message); 
                            }
                            state++;
                            break;
                        default:
                            db.SaveChanges();
                            state = 0;
                            break;
                    }
                }
            }
            if (ModelState.IsValid)
                return true;

            return false;

        }

         
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COContact cOContact = lhpdb.CoContacts.Find(id);
            if (cOContact == null)
            {
                return HttpNotFound();
            }
            return View(cOContact);
        }

        // GET: COContacts/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(COContactVm coContact)
        {

            if (SaveForm(coContact))
                return RedirectToAction("Index");

            return View(coContact);
        }

        // GET: COContacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COContact cOContact = lhpdb.CoContacts.Find(id);
            if (cOContact == null)
            {
                return HttpNotFound();
            }

            var vm = LoadForm(cOContact);

            return View(vm);
        }

        // POST: COContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(COContactVm cOContact)
        {

            if (SaveForm(cOContact))
                return RedirectToAction("Index");

            return View(cOContact);
        }

        // GET: COContacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COContact cOContact = lhpdb.CoContacts.Find(id);
            if (cOContact == null)
            {
                return HttpNotFound();
            }
            return View(cOContact);
        }

        // POST: COContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COContact cOContact = lhpdb.CoContacts.Find(id);
            lhpdb.CoContacts.Remove(cOContact);
            lhpdb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lhpdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
