using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class DisbursementViewModel
    {
        public Disbursement Disbur { get; set; }
        public string StaffName { get; set; }
        public string CollectionPoint { get; set; }
        public List<DisbursementDetail> Detail { get; set; }
        public string RequestedDate
        {
            get { return this.Disbur.DisDate.ToString("dd MMMM yyyy"); }
            set { this.Disbur.DisDate = DateTime.Parse(value); }
        }
    }
}