using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace logicProject.Models.ViewModel
{
    public class ItemModel
    {
        public List<SelectListItem> ListItems { get; set; }
        public int SelectedItem { get; set; }

        public string SelectedCategory { get; set; }

        public int SelectedMonth { get; set; }
    }
}