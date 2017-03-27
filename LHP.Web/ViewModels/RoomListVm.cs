using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LHP.Web.ViewModels
{
    public class RoomListVm
    {
        public int Id { get; set; }
        [DisplayName("Værelsesnr.")]
        public string RoomNb { get; set; }

        [DisplayName("Aktivt")]
        public bool Active { get; set; }

        [DisplayName("Værelsestype")]
        public string RoomType { get; set; }

        [DisplayName("Leje")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double RoomAmount { get; set; }

        [DisplayName("Gæst")]
        public string RoomerName { get; set; }
    }
}