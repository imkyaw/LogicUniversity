namespace logicProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lastUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisbursementDetails", "RequiredQty", c => c.Int(nullable: false));
            AddColumn("dbo.DisbursementDetails", "Remarks", c => c.String());
            AddColumn("dbo.Disbursements", "StoreStaffId", c => c.Int());
            AddColumn("dbo.Disbursements", "ReceiveStaffId", c => c.Int(nullable: false));
            AddColumn("dbo.Disbursements", "CollectionPointId", c => c.Int(nullable: false));
            AddColumn("dbo.Disbursements", "DeptId", c => c.String(maxLength: 128));
            AddColumn("dbo.PurchaseOrders", "Remarks", c => c.String());
            AddColumn("dbo.StockTransactions", "Remarks", c => c.String());
            AlterColumn("dbo.PurchaseOrders", "ApprovedDate", c => c.DateTime());
            CreateIndex("dbo.Disbursements", "DeptId");
            AddForeignKey("dbo.Disbursements", "DeptId", "dbo.Departments", "DeptId");
            DropColumn("dbo.PurchaseOrderDetails", "ReceivedQty");
            DropColumn("dbo.PurchaseOrders", "ReceivedQty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PurchaseOrders", "ReceivedQty", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseOrderDetails", "ReceivedQty", c => c.String());
            DropForeignKey("dbo.Disbursements", "DeptId", "dbo.Departments");
            DropIndex("dbo.Disbursements", new[] { "DeptId" });
            AlterColumn("dbo.PurchaseOrders", "ApprovedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.StockTransactions", "Remarks");
            DropColumn("dbo.PurchaseOrders", "Remarks");
            DropColumn("dbo.Disbursements", "DeptId");
            DropColumn("dbo.Disbursements", "CollectionPointId");
            DropColumn("dbo.Disbursements", "ReceiveStaffId");
            DropColumn("dbo.Disbursements", "StoreStaffId");
            DropColumn("dbo.DisbursementDetails", "Remarks");
            DropColumn("dbo.DisbursementDetails", "RequiredQty");
        }
    }
}
