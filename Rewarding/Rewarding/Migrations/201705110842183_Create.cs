namespace Rewarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Birthdate = c.DateTime(nullable: false),
                        Age = c.Int(nullable: false),
                        PhotoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PhotoId)
                .Index(t => t.PhotoId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                    })
                .PrimaryKey(t => t.ImageId);
            
            CreateTable(
                "dbo.Rewards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 250),
                        ImageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.ImageId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.PersonRewards",
                c => new
                    {
                        RewardId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RewardId, t.PersonId })
                .ForeignKey("dbo.Rewards", t => t.RewardId, cascadeDelete: true)
                .ForeignKey("dbo.Persons", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.RewardId)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.PersonRewards", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonRewards", "RewardId", "dbo.Rewards");
            DropForeignKey("dbo.Rewards", "ImageId", "dbo.Pictures");
            DropForeignKey("dbo.Persons", "PhotoId", "dbo.Pictures");
            DropIndex("dbo.PersonRewards", new[] { "PersonId" });
            DropIndex("dbo.PersonRewards", new[] { "RewardId" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.Rewards", new[] { "ImageId" });
            DropIndex("dbo.Persons", new[] { "PhotoId" });
            DropTable("dbo.PersonRewards");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.Rewards");
            DropTable("dbo.Pictures");
            DropTable("dbo.Persons");
        }
    }
}
