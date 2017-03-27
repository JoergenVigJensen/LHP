using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LHP.DAL.Models;

namespace LHP.Web.ViewModels
{
    public class InvoiceVm
    {
        public int InvoiceId { get; set; }
        public string InvoiceNb { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EnterDate { get; set; }
        public double Amount { get; set; }
        public bool Valid { get; set; }
        public Room Room { get; set; }
        public Roomer Roomer { get; set; }
        public ICollection<InvoiceLine> InvoiceLines { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public InvoiceState InvoiceState { get; set; }
    }
}