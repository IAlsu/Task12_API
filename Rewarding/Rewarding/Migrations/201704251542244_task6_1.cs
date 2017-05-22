namespace Rewarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class task6_1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Persons", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Persons", "Name", c => c.String());
        }
    }
}
