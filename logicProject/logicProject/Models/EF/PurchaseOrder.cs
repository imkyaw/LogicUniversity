using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class PurchaseOrder
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int StaffId { get; set; }
        public int ReceivedQty { get; set; }
        public string Status { get; set; }
        public string SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseOrderDetail> RequestDetails { get; set; }
    }
}