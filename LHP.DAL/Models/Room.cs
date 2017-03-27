using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHP.DAL.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Description("Værelsesnummer")]
        public string RoomNb { get; set; }

        [Description("Værelsestype")]
        public RoomType Type { get; set; }
        [Description("Aktivt")]
        public  bool Active { get; set; }

        [Description("Aktiv Gælst")]
        public virtual Roomer CurrentRoomer { get; set; }

        public ICollection<RoomProfileRelation> RoomProfileRelations { get; set; } = new HashSet<RoomProfileRelation>();
    }
}
