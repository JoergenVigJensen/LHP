using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHP.DAL;
using LHP.DAL.Models;
using LOES.DAL.Models;


namespace LHPAdapter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starter....");
            ConvertRoomers();
            ConvertRooms();
            ConvertInvoices();
            ConvertInvoiceLines();

            Console.ReadLine();
        }

        private static void ConvertRoomTypes()
        {
            throw new NotImplementedException();
        }


        private static void ConvertInvoiceLines()
        {
            throw new NotImplementedException();
        }

        private static void ConvertInvoices()
        {
            throw new NotImplementedException();
        }

        private static void ConvertRoomers()
        {
           var lhpdb = new LHPDbContext();

            using (var db = new LOESEntities())
            {
                var roomers = db.RoomerTbl.ToList();

                foreach (var roomer in roomers)
                {
                    var lhpRoomer = new LHP.DAL.Models.Roomer();
                    lhpRoomer.Comment = roomer.Note;
                    lhpRoomer.Name = roomer.FirstName + " " + roomer.LastName;
                    lhpRoomer.Identification = roomer.Identification;
                    lhpRoomer.Registrated = roomer.StartDate.Value;
                    lhpRoomer.Phone = roomer.Phone;
                    
                }
            }
        }

        private static void ConvertRooms()
        {

            var lhpDb = new LHPDbContext();

            using (var db = new LOESEntities())
            {
                var invoiceList = db.InvoiceTbl.Where(x=>x.PayCode == 1).OrderByDescending(x=>x.InvoiceID).ToList();

                foreach (var invoice in invoiceList)
                {
                    var room = lhpDb.Rooms.FirstOrDefault(x => x.RoomNb == invoice.RoomID);

                    if (room != null)
                        continue;

                    Console.Write("Værelse: "+ invoice.RoomID);

                    if (invoice.Amount != null)
                    {
                        var roomtype = lhpDb.RoomTypes.FirstOrDefault(x => x.Amount == invoice.Amount.Value);
                        if (roomtype == null)
                        {
                            roomtype = new RoomType()
                            {
                                Amount = (double)invoice.Amount.Value,
                                Description = invoice.AmountDescr
                            };
                            roomtype = lhpDb.RoomTypes.Add(roomtype);
                            lhpDb.SaveChanges();
                        }
                        room = new Room()
                        {
                            RoomNb = invoice.RoomID,
                            Type = roomtype
                        };
                        room = lhpDb.Rooms.Add(room);
                        lhpDb.SaveChanges();

                        var relation = new LHP.DAL.Models.RoomProfileRelation()
                        {
                            Activated = DateTime.Today,
                            Room = room,
                            RoomType = roomtype
                        };
                        lhpDb.RoomProfileRelations.Add(relation);

                        lhpDb.SaveChanges();
                    }
                    
                }
            }
        }
    }
}
