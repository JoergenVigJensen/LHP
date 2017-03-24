using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHP.DAL.Models
{
    public class RoomerActivity
    {
        public int RoomerActivityId { get; set; }
        public Roomer Roomer { get; set; }
        public Room Room { get; set; }
        public DateTime DateTime { get; set; }
        public RoomerActivityType Activity { get; set; }
    }
}
