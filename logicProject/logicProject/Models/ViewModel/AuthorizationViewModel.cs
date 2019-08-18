using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class AuthorizationViewModel
    {
        public string staffName { get; set; }
        public Authorization auth { get; set; }
        public string startDate
        {
            get { return this.auth.StartDate.ToString("dd MMMM yyyy"); }
            set { this.auth.StartDate = DateTime.Parse(value); }
        }
        public string endDate
        {
            get { return this.auth.EndDate.ToString("dd MMMM yyyy"); }
            set { this.auth.EndDate = DateTime.Parse(value); }
        }
    }
}