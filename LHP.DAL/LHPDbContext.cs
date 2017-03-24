using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHP.DAL.Models;

namespace LHP.DAL
{
    public class LHPDbContext : DbContext
    {

        public LHPDbContext() : base ("name=LHPDbConnection")
        {            
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Roomer> Roomers { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<RoomProfileRelation> RoomProfileRelations { get; set; }
        public DbSet<RoomActivity> RoomActivities { get; set; }
        public DbSet<RoomerActivity> RoomerActivities { get; set; }

    }
}
