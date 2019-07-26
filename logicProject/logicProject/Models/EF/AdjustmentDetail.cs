using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class AdjustmentDetail
    {
        [Key]
        public int AdjustmentDetailId { get; set; }
        public int AdjustmentId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("AdjustmentId")]
        public virtual Adjustment Adjustment { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int Qty { get; set; }
        public string reason { get; set; }
    }
}