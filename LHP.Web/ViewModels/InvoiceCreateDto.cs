using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LHP.DAL.Models;

namespace LHP.Web.ViewModels
{
    public class InvoiceCreateDto
    {
        public int RoomerId { get; set; }
        public int RoomId { get; set; }
        public InvoiceType InvoiceType { get; set; }
    }
}