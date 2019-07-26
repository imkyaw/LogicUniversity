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
        public int ProductId { get; set; }
        public string Bin { get; set; }
        public string Unit { get; set; }
        public int Qty { get; set; }
        public int ReorderLevel{ get; set; }
        public int ReorderQty { get; set; }
        public virtual ICollection<RequestDetail> RequestDetails { get; set; }
        public virtual ICollection<AdjustmentDetail> AjustmentDetails { get; set; }
        public virtual ICollection<DisbursementDetail> DisbursementDetails { get; set; }
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual ICollection<StockTransaction> Transactions { get; set; }
    }
}