namespace FileOrganizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtensionGroups",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.ExtensionMappingItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetPath = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Extension_Id = c.Int(),
                        Extension_ExtensionName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Extensions", t => new { t.Extension_Id, t.Extension_ExtensionName })
                .Index(t => new { t.Extension_Id, t.Extension_ExtensionName });
            
            CreateTable(
                "dbo.Extensions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExtensionName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Id, t.ExtensionName });
            
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
            DropForeignKey("dbo.ExtensionMappingItems", new[] { "Extension_Id", "Extension_ExtensionName" }, "dbo.Extensions");
            DropIndex("dbo.ExtensionMappingItems", new[] { "Extension_Id", "Extension_ExtensionName" });
            DropTable("dbo.FileSystemWatcherDtoes");
            DropTable("dbo.Extensions");
            DropTable("dbo.ExtensionMappingItems");
            DropTable("dbo.ExtensionGroups");
        }
    }
}
