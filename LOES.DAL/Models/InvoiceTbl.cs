//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LOES.DAL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvoiceTbl
    {
        public int InvoiceID { get; set; }
        public Nullable<int> RoomerID { get; set; }
        public string RoomerName { get; set; }
        public string RoomID { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<System.DateTime> PeriodStart { get; set; }
        public Nullable<System.DateTime> PeriodEnd { get; set; }
        public string AmountDescr { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<int> PayCode { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
        public bool Entered { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceNote { get; set; }
        public Nullable<int> FeatureID { get; set; }
    }
}