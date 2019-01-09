namespace FileOrganizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Settings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        LogEntryId = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.LogEntryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
