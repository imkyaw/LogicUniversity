namespace logicProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFav : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavOrders",
                c => new
                    {
                        FavOrderId = c.Int(nullable: false, identity: true),
                        FavOrderName = c.String(),
                        FavFormId = c.String(),
                        StaffId = c.Int(nullable: false),
                        DeptId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FavOrderId)
                .ForeignKey("dbo.Departments", t => t.DeptId)
                .Index(t => t.DeptId);
            
            CreateTable(
                "dbo.FavOrderDetails",
                c => new
                    {
                        FavOrderDetailsId = c.Int(nullable: false, identity: true),
                        ProductId = c.String(maxLength: 128),
                        FavOrderId = c.Int(nullable: false),
                        FavQty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FavOrderDetailsId)
                .ForeignKey("dbo.FavOrders", t => t.FavOrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.FavOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavOrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.FavOrderDetails", "FavOrderId", "dbo.FavOrders");
            DropForeignKey("dbo.FavOrders", "DeptId", "dbo.Departments");
            DropIndex("dbo.FavOrderDetails", new[] { "FavOrderId" });
            DropIndex("dbo.FavOrderDetails", new[] { "ProductId" });
            DropIndex("dbo.FavOrders", new[] { "DeptId" });
            DropTable("dbo.FavOrderDetails");
            DropTable("dbo.FavOrders");
        }
    }
}
