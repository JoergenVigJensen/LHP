using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHP.DAL.Models
{
    public class Roomer
    {
        public int RoomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Employer Employer { get; set; }
        public DateTime Registrated { get; set; }
        public string Indentification { get; set; }
        public string Comment { get; set; }
    }
}
