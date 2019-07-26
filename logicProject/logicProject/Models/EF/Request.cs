using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }
        public DateTime ReqDate { get; set; }
        public int StaffId { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public int DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Department Department { get; set; }
        public virtual ICollection<RequestDisbursementDetail> RequestDisbursementDetail{ get; set; }
        public virtual ICollection<RequestDetail> RequestDetails { get; set; }
    }
}