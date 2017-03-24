using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHP.DAL.Models
{
    public class RoomType
    {
        public int RoomTypeId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public ICollection<RoomProfileRelation> RoomProfileRelations { get; set; } = new HashSet<RoomProfileRelation>();

    }
}
