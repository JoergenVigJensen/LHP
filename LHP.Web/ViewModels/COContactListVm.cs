using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LHP.Web.ViewModels
{
    public class COContactListVm
    {
        public int ContactId { get; set; }
        [DisplayName("Navn")]

        public string Name { get; set; }

        [DisplayName("Kontakt")]
        public string Contact { get; set; }

        [DisplayName("Telefonnr")]
        public string Phone { get; set; }

        [DisplayName("Email-adresse")]
        public string Email { get; set; }
    }
}