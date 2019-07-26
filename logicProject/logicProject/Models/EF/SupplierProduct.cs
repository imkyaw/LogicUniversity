using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class SupplierProduct
    {
        [Key]
        public int SupplierProductId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public double Price { get; set; }
    }
}