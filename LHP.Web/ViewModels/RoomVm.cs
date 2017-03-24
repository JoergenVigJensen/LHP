using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LHP.DAL.Models;

namespace LHP.Web.ViewModels
{
    public class RoomVm
    {
        public int RoomId { get; set; }
        public bool Active { get; set; }
        public RoomType Type { get; set; }

        [Required(ErrorMessage = "Der skal angives et værelsesnummer")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Værelsenummer skal opfylde formatet 023")]

        public string RoomNb { get; set; }
        public string CurretRoomer { get; set; }

        public int RoomTypeId { get; set; }
        public IEnumerable<SelectListItem> RoomTypes { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> ActiveRoomers { get; set; } = new List<SelectListItem>();

    }
}