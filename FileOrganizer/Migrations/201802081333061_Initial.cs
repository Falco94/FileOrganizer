namespace FileOrganizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtensionGroups",
                c => new
                    {
                        ExtensionGroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ExtensionGroupId);
            
            CreateTable(
                "dbo.Extensions",
                c => new
                    {
                        ExtensionId = c.Int(nullable: false, identity: true),
                        ExtensionName = c.String(nullable: false, maxLength: 128),
                        CurrentExtensionGroupId = c.Int(),
                    })
                .PrimaryKey(t => new { t.ExtensionId, t.ExtensionName })
                .ForeignKey("dbo.ExtensionGroups", t => t.CurrentExtensionGroupId)
                .Index(t => t.CurrentExtensionGroupId);
            
            CreateTable(
                "dbo.ExtensionMappingItems",
                c => new
                    {
                        ExtensionMappingItemId = c.Int(nullable: false, identity: true),
                        TargetPath = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Extension_ExtensionId = c.Int(),
                        Extension_ExtensionName = c.String(maxLength: 128),
                        ExtensionGroup_ExtensionGroupId = c.Int(),
                    })
                .PrimaryKey(t => t.ExtensionMappingItemId)
                .ForeignKey("dbo.Extensions", t => new { t.Extension_ExtensionId, t.Extension_ExtensionName })
                .ForeignKey("dbo.ExtensionGroups", t => t.ExtensionGroup_ExtensionGroupId)
                .Index(t => new { t.Extension_ExtensionId, t.Extension_ExtensionName })
                .Index(t => t.ExtensionGroup_ExtensionGroupId);
            
            CreateTable(
                "dbo.FileSystemWatcherDtoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtensionMappingItems", "ExtensionGroup_ExtensionGroupId", "dbo.ExtensionGroups");
            DropForeignKey("dbo.ExtensionMappingItems", new[] { "Extension_ExtensionId", "Extension_ExtensionName" }, "dbo.Extensions");
            DropForeignKey("dbo.Extensions", "CurrentExtensionGroupId", "dbo.ExtensionGroups");
            DropIndex("dbo.ExtensionMappingItems", new[] { "ExtensionGroup_ExtensionGroupId" });
            DropIndex("dbo.ExtensionMappingItems", new[] { "Extension_ExtensionId", "Extension_ExtensionName" });
            DropIndex("dbo.Extensions", new[] { "CurrentExtensionGroupId" });
            DropTable("dbo.FileSystemWatcherDtoes");
            DropTable("dbo.ExtensionMappingItems");
            DropTable("dbo.Extensions");
            DropTable("dbo.ExtensionGroups");
        }
    }
}
