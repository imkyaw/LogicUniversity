using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string GSTRegNo { get; set; }
        public string ContactName { get; set; }
        public string FaxNo { get; set; }
        public string PhNo { get; set; }
        public string Address { get; set; }
        
        public virtual ICollection<PurchaseOrder> Purchase { get; set; }

        public virtual ICollection<SupplierProduct> SupplierProduct { get; set; }
    }
}