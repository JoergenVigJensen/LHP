using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHP.DAL.Models
{
    public class InvoiceLine
    {
        public int InvoiceLineId { get; set; }
        public string Text { get; set; }
        public InvoiceLineType Type { get; set; }
        public double Amount { get; set; }
    }
}
