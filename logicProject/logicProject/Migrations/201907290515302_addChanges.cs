namespace logicProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdjustmentDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.DisbursementDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PurchaseOrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.RequestDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.StockTransactions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Requests", "DeptId", "dbo.Departments");
            DropForeignKey("dbo.DepartmentStaffs", "DeptId", "dbo.Departments");
            DropForeignKey("dbo.PurchaseOrders", "SuppliertId", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierProducts", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.SupplierProducts", "ProductId", "dbo.Products");
            DropIndex("dbo.AdjustmentDetails", new[] { "ProductId" });
            DropIndex("dbo.DisbursementDetails", new[] { "ProductId" });
            DropIndex("dbo.Requests", new[] { "DeptId" });
            DropIndex("dbo.DepartmentStaffs", new[] { "DeptId" });
            DropIndex("dbo.RequestDetails", new[] { "ProductId" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "ProductId" });
            DropIndex("dbo.PurchaseOrders", new[] { "SuppliertId" });
            DropIndex("dbo.SupplierProducts", new[] { "SupplierId" });
            DropIndex("dbo.SupplierProducts", new[] { "ProductId" });
            DropIndex("dbo.StockTransactions", new[] { "ProductId" });
            RenameColumn(table: "dbo.PurchaseOrders", name: "SuppliertId", newName: "SupplierId");
            DropPrimaryKey("dbo.Products");
            DropPrimaryKey("dbo.Departments");
            DropPrimaryKey("dbo.Suppliers");
            DropColumn("dbo.AdjustmentDetails", "ProductId");
            DropColumn("dbo.Products", "ProductId");
            DropColumn("dbo.DisbursementDetails", "ProductId");
            DropColumn("dbo.PurchaseOrderDetails", "ProductId");
            DropColumn("dbo.RequestDetails", "ProductId");
            DropColumn("dbo.SupplierProducts", "ProductId");
            DropColumn("dbo.StockTransactions", "ProductId");
            DropColumn("dbo.Requests", "DeptId");
            DropColumn("dbo.Departments", "DeptId");
            DropColumn("dbo.DepartmentStaffs", "DeptId");
            DropColumn("dbo.PurchaseOrders", "SupplierId");
            DropColumn("dbo.Suppliers", "SupplierId");
            DropColumn("dbo.SupplierProducts", "SupplierId");

            AddColumn("dbo.Products", "Description", c => c.String());
            AddColumn("dbo.Requests", "RequestFormId", c => c.String());
            AddColumn("dbo.AdjustmentDetails", "ProductId", c => c.String(maxLength: 128));
            AddColumn("dbo.Products", "ProductId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.DisbursementDetails", "ProductId", c => c.String(maxLength: 128));
            AddColumn("dbo.Requests", "DeptId", c => c.String(maxLength: 128));
            AddColumn("dbo.Departments", "DeptId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.DepartmentStaffs", "DeptId", c => c.String(maxLength: 128));
            AddColumn("dbo.RequestDetails", "ProductId", c => c.String(maxLength: 128));
            AddColumn("dbo.PurchaseOrderDetails", "ProductId", c => c.String(maxLength: 128));
            AddColumn("dbo.PurchaseOrders", "SupplierId", c => c.String(maxLength: 128));
            AddColumn("dbo.Suppliers", "SupplierId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.SupplierProducts", "SupplierId", c => c.String(maxLength: 128));
            AddColumn("dbo.SupplierProducts", "ProductId", c => c.String(maxLength: 128));
            AddColumn("dbo.StockTransactions", "ProductId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Products", "ProductId");
            AddPrimaryKey("dbo.Departments", "DeptId");
            AddPrimaryKey("dbo.Suppliers", "SupplierId");
            CreateIndex("dbo.AdjustmentDetails", "ProductId");
            CreateIndex("dbo.DisbursementDetails", "ProductId");
            CreateIndex("dbo.Requests", "DeptId");
            CreateIndex("dbo.DepartmentStaffs", "DeptId");
            CreateIndex("dbo.RequestDetails", "ProductId");
            CreateIndex("dbo.PurchaseOrderDetails", "ProductId");
            CreateIndex("dbo.PurchaseOrders", "SupplierId");
            CreateIndex("dbo.SupplierProducts", "SupplierId");
            CreateIndex("dbo.SupplierProducts", "ProductId");
            CreateIndex("dbo.StockTransactions", "ProductId");
            AddForeignKey("dbo.AdjustmentDetails", "ProductId", "dbo.Products", "ProductId");
            AddForeignKey("dbo.DisbursementDetails", "ProductId", "dbo.Products", "ProductId");
            AddForeignKey("dbo.PurchaseOrderDetails", "ProductId", "dbo.Products", "ProductId");
            AddForeignKey("dbo.RequestDetails", "ProductId", "dbo.Products", "ProductId");
            AddForeignKey("dbo.StockTransactions", "ProductId", "dbo.Products", "ProductId");
            AddForeignKey("dbo.Requests", "DeptId", "dbo.Departments", "DeptId");
            AddForeignKey("dbo.DepartmentStaffs", "DeptId", "dbo.Departments", "DeptId");
            AddForeignKey("dbo.PurchaseOrders", "SupplierId", "dbo.Suppliers", "SupplierId");
            AddForeignKey("dbo.SupplierProducts", "SupplierId", "dbo.Suppliers", "SupplierId");
            AddForeignKey("dbo.SupplierProducts", "ProductId", "dbo.Products", "ProductId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.SupplierProducts", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseOrders", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.DepartmentStaffs", "DeptId", "dbo.Departments");
            DropForeignKey("dbo.Requests", "DeptId", "dbo.Departments");
            DropForeignKey("dbo.StockTransactions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.RequestDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PurchaseOrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.DisbursementDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.AdjustmentDetails", "ProductId", "dbo.Products");
            DropIndex("dbo.StockTransactions", new[] { "ProductId" });
            DropIndex("dbo.SupplierProducts", new[] { "ProductId" });
            DropIndex("dbo.SupplierProducts", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseOrders", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "ProductId" });
            DropIndex("dbo.RequestDetails", new[] { "ProductId" });
            DropIndex("dbo.DepartmentStaffs", new[] { "DeptId" });
            DropIndex("dbo.Requests", new[] { "DeptId" });
            DropIndex("dbo.DisbursementDetails", new[] { "ProductId" });
            DropIndex("dbo.AdjustmentDetails", new[] { "ProductId" });
            DropPrimaryKey("dbo.Suppliers");
            DropPrimaryKey("dbo.Departments");
            DropPrimaryKey("dbo.Products");
            DropColumn("dbo.AdjustmentDetails", "ProductId");
            DropColumn("dbo.Products", "ProductId");
            DropColumn("dbo.DisbursementDetails", "ProductId");
            DropColumn("dbo.PurchaseOrderDetails", "ProductId");
            DropColumn("dbo.RequestDetails", "ProductId");
            DropColumn("dbo.SupplierProducts", "ProductId");
            DropColumn("dbo.StockTransactions", "ProductId");
            DropColumn("dbo.Requests", "DeptId");
            DropColumn("dbo.Departments", "DeptId");
            DropColumn("dbo.DepartmentStaffs", "DeptId");
            DropColumn("dbo.PurchaseOrders", "SupplierId");
            DropColumn("dbo.Suppliers", "SupplierId");
            DropColumn("dbo.SupplierProducts", "SupplierId");

            AddColumn("dbo.StockTransactions", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.SupplierProducts", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.SupplierProducts", "SupplierId", c => c.Int(nullable: false));
            AddColumn("dbo.Suppliers", "SupplierId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.PurchaseOrders", "SupplierId", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseOrderDetails", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.RequestDetails", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.DepartmentStaffs", "DeptId", c => c.Int(nullable: false));
            AddColumn("dbo.Departments", "DeptId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Requests", "DeptId", c => c.Int(nullable: false));
            AddColumn("dbo.DisbursementDetails", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ProductId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.AdjustmentDetails", "ProductId", c => c.Int(nullable: false));
            DropColumn("dbo.Requests", "RequestFormId");
            DropColumn("dbo.Products", "Description");
            AddPrimaryKey("dbo.Suppliers", "SupplierId");
            AddPrimaryKey("dbo.Departments", "DeptId");
            AddPrimaryKey("dbo.Products", "ProductId");
            RenameColumn(table: "dbo.PurchaseOrders", name: "SupplierId", newName: "SuppliertId");
            CreateIndex("dbo.StockTransactions", "ProductId");
            CreateIndex("dbo.SupplierProducts", "ProductId");
            CreateIndex("dbo.SupplierProducts", "SupplierId");
            CreateIndex("dbo.PurchaseOrders", "SuppliertId");
            CreateIndex("dbo.PurchaseOrderDetails", "ProductId");
            CreateIndex("dbo.RequestDetails", "ProductId");
            CreateIndex("dbo.DepartmentStaffs", "DeptId");
            CreateIndex("dbo.Requests", "DeptId");
            CreateIndex("dbo.DisbursementDetails", "ProductId");
            CreateIndex("dbo.AdjustmentDetails", "ProductId");
            AddForeignKey("dbo.SupplierProducts", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.SupplierProducts", "SupplierId", "dbo.Suppliers", "SupplierId", cascadeDelete: true);
            AddForeignKey("dbo.PurchaseOrders", "SuppliertId", "dbo.Suppliers", "SupplierId", cascadeDelete: true);
            AddForeignKey("dbo.DepartmentStaffs", "DeptId", "dbo.Departments", "DeptId", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "DeptId", "dbo.Departments", "DeptId", cascadeDelete: true);
            AddForeignKey("dbo.StockTransactions", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.RequestDetails", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.PurchaseOrderDetails", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.DisbursementDetails", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.AdjustmentDetails", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
        }
    }
}
