using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class Adjustment
    {
        public int AdjustmentId { get; set; }
        public int StaffId { get; set; }
        public string Remark { get; set; }
        public DateTime AdjustedDate { get; set; }
        public string Status { get; set; }
        public virtual ICollection<AdjustmentDetail> AdjustmentDetails { get; set; }

    }
}