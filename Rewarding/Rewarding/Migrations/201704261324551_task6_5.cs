namespace Rewarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class task6_5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rewards", "ImageId", "dbo.Pictures");
            DropIndex("dbo.Rewards", new[] { "ImageId" });
            AlterColumn("dbo.Rewards", "ImageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Rewards", "ImageId");
            AddForeignKey("dbo.Rewards", "ImageId", "dbo.Pictures", "ImageId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rewards", "ImageId", "dbo.Pictures");
            DropIndex("dbo.Rewards", new[] { "ImageId" });
            AlterColumn("dbo.Rewards", "ImageId", c => c.Int());
            CreateIndex("dbo.Rewards", "ImageId");
            AddForeignKey("dbo.Rewards", "ImageId", "dbo.Pictures", "ImageId");
        }
    }
}
