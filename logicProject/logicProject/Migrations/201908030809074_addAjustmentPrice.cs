namespace logicProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAjustmentPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdjustmentDetails", "UnitPrice", c => c.Double(nullable: false));
            AddColumn("dbo.AdjustmentDetails", "TotalPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdjustmentDetails", "TotalPrice");
            DropColumn("dbo.AdjustmentDetails", "UnitPrice");
        }
    }
}
