using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.ViewModel
{
    public class DisbursementDetailViewModel
    {
        public string ProductId { get; set; }
        public string DepartmentId { get; set; }
        public int Quantity { get; set; }
    }
}