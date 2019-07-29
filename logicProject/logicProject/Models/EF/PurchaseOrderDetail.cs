using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class PurchaseOrderDetail
    {
        [Key]
        public int PurchaseOrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        [ForeignKey("OrderId")]
        public virtual PurchaseOrder Order { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int ReqQty { get; set; }
        public string ReceivedQty { get; set; }
    }
}