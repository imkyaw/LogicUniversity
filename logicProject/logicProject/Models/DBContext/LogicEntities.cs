using logicProject.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace logicProject.Models.DBContext
{
    public class LogicEntities : DbContext
    {
        public LogicEntities() : base("LogicUniversity") { }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Adjustment> Adjustment { get; set; }
        public virtual DbSet<AdjustmentDetail> AdjustmentDetail { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<RequestDetail> RequestDetail { get; set; }
        public virtual DbSet<RequestDisbursementDetail> RequestDisbursementDetail { get; set; }
        public virtual DbSet<Disbursement> Disbursement { get; set; }
        public virtual DbSet<DisbursementDetail> DisbursementDetail { get; set; }
        public virtual DbSet<StoreStaff> StoreStaff { get; set; }
        public virtual DbSet<DepartmentStaff> DepartmentStaff { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<CollectionPoint> CollectionPoint { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<SupplierProduct> SupplierProduct { get; set; }
        public virtual DbSet<StockTransaction> StockTransaction { get; set; }
        public virtual DbSet<Authorization> Authorization { get; set; }
    }
}