namespace logicProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authorizations", "DeptId", c => c.Int(nullable: false));
            AddColumn("dbo.DepartmentStaffs", "DeptId", c => c.Int(nullable: false));
            CreateIndex("dbo.DepartmentStaffs", "DeptId");
            AddForeignKey("dbo.DepartmentStaffs", "DeptId", "dbo.Departments", "DeptId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DepartmentStaffs", "DeptId", "dbo.Departments");
            DropIndex("dbo.DepartmentStaffs", new[] { "DeptId" });
            DropColumn("dbo.DepartmentStaffs", "DeptId");
            DropColumn("dbo.Authorizations", "DeptId");
        }
    }
}
