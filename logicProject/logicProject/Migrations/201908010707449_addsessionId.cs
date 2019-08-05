namespace logicProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsessionId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DepartmentStaffs", "SessionId", c => c.String());
            AddColumn("dbo.StoreStaffs", "SessionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreStaffs", "SessionId");
            DropColumn("dbo.DepartmentStaffs", "SessionId");
        }
    }
}
