namespace logicProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDbString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Authorizations", "DeptId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Authorizations", "DeptId", c => c.Int(nullable: false));
        }
    }
}
