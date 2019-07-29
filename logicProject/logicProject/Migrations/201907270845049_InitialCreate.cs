namespace logicProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adjustments",
                c => new
                    {
                        AdjustmentId = c.Int(nullable: false, identity: true),
                        StaffId = c.Int(nullable: false),
                        Remark = c.String(),
                        AdjustedDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.AdjustmentId);
            
            CreateTable(
                "dbo.AdjustmentDetails",
                c => new
                    {
                        AdjustmentDetailId = c.Int(nullable: false, identity: true),
                        AdjustmentId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        reason = c.String(),
                    })
                .PrimaryKey(t => t.AdjustmentDetailId)
                .ForeignKey("dbo.Adjustments", t => t.AdjustmentId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.AdjustmentId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Bin = c.String(),
                        Unit = c.String(),
                        Qty = c.Int(nullable: false),
                        ReorderLevel = c.Int(nullable: false),
                        ReorderQty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "dbo.DisbursementDetails",
                c => new
                    {
                        DisDetailId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ReceivedQty = c.Int(nullable: false),
                        DisId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DisDetailId)
                .ForeignKey("dbo.Disbursements", t => t.DisId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.DisId);
            
            CreateTable(
                "dbo.Disbursements",
                c => new
                    {
                        DisId = c.Int(nullable: false, identity: true),
                        DisDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.DisId);
            
            CreateTable(
                "dbo.RequestDisbursementDetails",
                c => new
                    {
                        RequestDisbursementDetailId = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        DisId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestDisbursementDetailId)
                .ForeignKey("dbo.Disbursements", t => t.DisId, cascadeDelete: true)
                .ForeignKey("dbo.Requests", t => t.RequestId, cascadeDelete: true)
                .Index(t => t.RequestId)
                .Index(t => t.DisId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        ReqDate = c.DateTime(nullable: false),
                        StaffId = c.Int(nullable: false),
                        Remark = c.String(),
                        Status = c.String(),
                        DeptId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Departments", t => t.DeptId, cascadeDelete: true)
                .Index(t => t.DeptId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DeptId = c.Int(nullable: false, identity: true),
                        DeptName = c.String(),
                        ContactName = c.String(),
                        PhNo = c.String(),
                        FaxNo = c.String(),
                        EmailAddr = c.String(),
                        HeadId = c.Int(nullable: false),
                        CollectionPt = c.Int(nullable: false),
                        RepId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeptId)
                .ForeignKey("dbo.CollectionPoints", t => t.CollectionPt, cascadeDelete: true)
                .Index(t => t.CollectionPt);
            
            CreateTable(
                "dbo.CollectionPoints",
                c => new
                    {
                        CollectionPtId = c.Int(nullable: false, identity: true),
                        CollectionPt = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CollectionPtId);
            
            CreateTable(
                "dbo.RequestDetails",
                c => new
                    {
                        RequestDetailId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        RequestId = c.Int(nullable: false),
                        ReqQty = c.Int(nullable: false),
                        ReceivedQty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestDetailId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Requests", t => t.RequestId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.RequestId);
            
            CreateTable(
                "dbo.PurchaseOrderDetails",
                c => new
                    {
                        PurchaseOrderDetailId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ReqQty = c.Int(nullable: false),
                        ReceivedQty = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseOrderDetailId)
                .ForeignKey("dbo.PurchaseOrders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        ApprovedDate = c.DateTime(nullable: false),
                        StaffId = c.Int(nullable: false),
                        ReceivedQty = c.Int(nullable: false),
                        Status = c.String(),
                        SuppliertId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Suppliers", t => t.SuppliertId, cascadeDelete: true)
                .Index(t => t.SuppliertId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierId = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(),
                        GSTRegNo = c.String(),
                        ContactName = c.String(),
                        FaxNo = c.String(),
                        PhNo = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.SupplierId);
            
            CreateTable(
                "dbo.SupplierProducts",
                c => new
                    {
                        SupplierProductId = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SupplierProductId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.SupplierId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.StockTransactions",
                c => new
                    {
                        StockTranId = c.Int(nullable: false, identity: true),
                        TranDate = c.DateTime(nullable: false),
                        Qty = c.Int(nullable: false),
                        TotalBalance = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockTranId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Authorizations",
                c => new
                    {
                        AuthNo = c.Int(nullable: false, identity: true),
                        StaffId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AuthNo);
            
            CreateTable(
                "dbo.DepartmentStaffs",
                c => new
                    {
                        StaffId = c.Int(nullable: false, identity: true),
                        StaffName = c.String(),
                        StaffEmail = c.String(),
                        StaffType = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.StaffId);
            
            CreateTable(
                "dbo.StoreStaffs",
                c => new
                    {
                        StaffId = c.Int(nullable: false, identity: true),
                        StaffName = c.String(),
                        StaffEmail = c.String(),
                        StaffType = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.StaffId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockTransactions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PurchaseOrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.SupplierProducts", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PurchaseOrders", "SuppliertId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseOrderDetails", "OrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.DisbursementDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.RequestDisbursementDetails", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.RequestDetails", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.RequestDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Requests", "DeptId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "CollectionPt", "dbo.CollectionPoints");
            DropForeignKey("dbo.RequestDisbursementDetails", "DisId", "dbo.Disbursements");
            DropForeignKey("dbo.DisbursementDetails", "DisId", "dbo.Disbursements");
            DropForeignKey("dbo.AdjustmentDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.AdjustmentDetails", "AdjustmentId", "dbo.Adjustments");
            DropIndex("dbo.StockTransactions", new[] { "ProductId" });
            DropIndex("dbo.SupplierProducts", new[] { "ProductId" });
            DropIndex("dbo.SupplierProducts", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseOrders", new[] { "SuppliertId" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "ProductId" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "OrderId" });
            DropIndex("dbo.RequestDetails", new[] { "RequestId" });
            DropIndex("dbo.RequestDetails", new[] { "ProductId" });
            DropIndex("dbo.Departments", new[] { "CollectionPt" });
            DropIndex("dbo.Requests", new[] { "DeptId" });
            DropIndex("dbo.RequestDisbursementDetails", new[] { "DisId" });
            DropIndex("dbo.RequestDisbursementDetails", new[] { "RequestId" });
            DropIndex("dbo.DisbursementDetails", new[] { "DisId" });
            DropIndex("dbo.DisbursementDetails", new[] { "ProductId" });
            DropIndex("dbo.AdjustmentDetails", new[] { "ProductId" });
            DropIndex("dbo.AdjustmentDetails", new[] { "AdjustmentId" });
            DropTable("dbo.StoreStaffs");
            DropTable("dbo.DepartmentStaffs");
            DropTable("dbo.Authorizations");
            DropTable("dbo.StockTransactions");
            DropTable("dbo.SupplierProducts");
            DropTable("dbo.Suppliers");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.PurchaseOrderDetails");
            DropTable("dbo.RequestDetails");
            DropTable("dbo.CollectionPoints");
            DropTable("dbo.Departments");
            DropTable("dbo.Requests");
            DropTable("dbo.RequestDisbursementDetails");
            DropTable("dbo.Disbursements");
            DropTable("dbo.DisbursementDetails");
            DropTable("dbo.Products");
            DropTable("dbo.AdjustmentDetails");
            DropTable("dbo.Adjustments");
        }
    }
}
