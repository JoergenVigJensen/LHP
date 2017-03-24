using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHP.DAL.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNb { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EnterDate { get; set; }
        public double Amount { get; set; }
        public bool Valid { get; set; }
        public ICollection<InvoiceLine> InvoiceLines { get; set; }
        public InvoiceType InvoiceType { get; set; }



    }
}
