using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LHP.Web.ViewModels
{
    public class COContactVm
    {
        public int COContactId { get; set; }

        [DisplayName("Navn")]
        public string Name { get; set; }

        [DisplayName("Kontaktperson")]
        public string Contact { get; set; }

        [DisplayName("Telefonnr")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Adresse 1")]
        public string Address1 { get; set; }

        [DisplayName("Adresse 2")]
        public string Address2 { get; set; }

        [DisplayName("Postnr.")]
        public string ZipCode { get; set; }

        [DisplayName("By")]
        public string City { get; set; }

        [DisplayName("Note")]
        public string Comment { get; set; }
    }
}