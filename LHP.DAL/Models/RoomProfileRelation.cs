using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHP.DAL.Models
{
    public class RoomProfileRelation
    {
        public int RoomProfileRelationId { get; set; }
        public Room Room { get; set; }
        public RoomType RoomType { get; set; }

        public DateTime Activated { get; set; }

        public DateTime? DeActivated { get; set; }

        public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
    }
}
