using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mvc.JQuery.DataTables;

namespace LHP.Web.ViewModels
{
    public class RoomListVm
    {
        [DataTables(Visible = false)]
        public int Id { get; set; }
        [DataTables(DisplayName = "Værelse", Searchable = true)]
        public string RoomNb { get; set; }

        [DataTables(DisplayName = "Aktivt", Searchable = true)]
        public bool Active { get; set; }

        [DataTables(DisplayName = "Værelsestype", Searchable = true)]
        public string RoomType { get; set; }

        [DataTables(DisplayName = "Beløb",CssClass="text-right")]
        public double RoomAmount { get; set; }

        [DataTables(DisplayName = "Beboer")]
        public string RoomerName { get; set; }

        [DataTables(DisplayName = "", Sortable = false, Width = "78px")]
        public string DetailsBtn { get; set; }

        [DataTables(DisplayName = "", Sortable = false, Width = "78px")]
        public string EditBtn { get; set; }

        [DataTables(DisplayName = "", Sortable = false, Width = "78px")]
        public string BillBtn { get; set; }

    }
}