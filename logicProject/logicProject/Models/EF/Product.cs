﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace logicProject.Models.EF
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Bin { get; set; }
        public string Unit { get; set; }
        [Range(0, 1000000, ErrorMessage = "Value cannot be negative")]
        public int Qty { get; set; }
        [Range(0, 1000000, ErrorMessage = "Value cannot be negative")]
        public int ReorderLevel{ get; set; }
        [Range(0, 1000000, ErrorMessage = "Value cannot be negative")]
        public int ReorderQty { get; set; }
        public virtual ICollection<RequestDetail> RequestDetails { get; set; }
        public virtual ICollection<AdjustmentDetail> AdjustmentDetails { get; set; }
        public virtual ICollection<DisbursementDetail> DisbursementDetails { get; set; }
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual ICollection<StockTransaction> Transactions { get; set; }
        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }
    }
}