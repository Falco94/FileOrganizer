namespace FileOrganizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileOrganizerDataFODataModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExtensionMappingItems", "ExtensionGroup_Name", c => c.String(maxLength: 128));
            AddColumn("dbo.Extensions", "ExtensionGroup_Name", c => c.String(maxLength: 128));
            CreateIndex("dbo.Extensions", "ExtensionGroup_Name");
            CreateIndex("dbo.ExtensionMappingItems", "ExtensionGroup_Name");
            AddForeignKey("dbo.Extensions", "ExtensionGroup_Name", "dbo.ExtensionGroups", "Name");
            AddForeignKey("dbo.ExtensionMappingItems", "ExtensionGroup_Name", "dbo.ExtensionGroups", "Name");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtensionMappingItems", "ExtensionGroup_Name", "dbo.ExtensionGroups");
            DropForeignKey("dbo.Extensions", "ExtensionGroup_Name", "dbo.ExtensionGroups");
            DropIndex("dbo.ExtensionMappingItems", new[] { "ExtensionGroup_Name" });
            DropIndex("dbo.Extensions", new[] { "ExtensionGroup_Name" });
            DropColumn("dbo.Extensions", "ExtensionGroup_Name");
            DropColumn("dbo.ExtensionMappingItems", "ExtensionGroup_Name");
        }
    }
}
