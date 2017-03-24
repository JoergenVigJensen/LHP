using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHP.DAL.Models
{
    public class RoomActivity
    {
        public int RoomActivityId { get; set; }
        public Room Room { get; set; }
        public Roomer Roomer { get; set; }
        public DateTime DateTime { get; set; }
        public RoomActivityType Type { get; set; }
    }
}
