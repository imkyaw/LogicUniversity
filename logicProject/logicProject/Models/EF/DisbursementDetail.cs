using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class DisbursementDetail
    {
        [Key]
        public int DisDetailId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int ReceivedQty { get; set; }

        public int DisId { get; set; }
        [ForeignKey("DisId")]
        public virtual Disbursement Disbursement { get; set; }
    }
}