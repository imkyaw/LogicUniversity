using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class RequestDisbursementDetail
    {
        [Key]
        public int RequestDisbursementDetailId { get; set; }
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
        public int DisId { get; set; }
        [ForeignKey("DisId")]
        public virtual Disbursement Disbursement { get; set; }
    }
}