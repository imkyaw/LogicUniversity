namespace logicProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfav : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "FavRequest", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "FavRequest");
        }
    }
}
