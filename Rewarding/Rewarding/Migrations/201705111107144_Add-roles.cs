namespace Rewarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addroles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IdentityRoles", "Description", c => c.String());
            AddColumn("dbo.IdentityRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IdentityRoles", "Discriminator");
            DropColumn("dbo.IdentityRoles", "Description");
        }
    }
}
