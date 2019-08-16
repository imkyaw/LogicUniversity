using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class Disbursement
    {
        [Key]
        public int DisId { get; set; }
        public DateTime DisDate { get; set; }
        public string Status { get; set; }
        public int? StoreStaffId { get; set; }
        public int ReceiveStaffId { get; set; }
        public int CollectionPointId { get; set; }
        public string DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Department Department { get; set; }
        public virtual ICollection<DisbursementDetail> DisbursementDetails { get; set; }
        public virtual ICollection<RequestDisbursementDetail> RequestDisbursementDetails { get; set; }
    }
}