﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LHP.DAL.Models;

namespace LHP.Web.ViewModels
{
    public class RoomerVm
    {
        public int RoomerId { get; set; }
        [DisplayName("Navn")]
        public string Name { get; set; }

        [DisplayName("Telefonnummer")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("CO-kontakt")]
        public Employer Employer { get; set; }

        [DisplayName("Oprettet")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Registrated { get; set; }

        [DisplayName("Identifikation")]
        public string Identification { get; set; }

        [DisplayName("Note")]
        public string Comment { get; set; }
        public ICollection<SelectListItem> Employers { get; set; }
    }
}