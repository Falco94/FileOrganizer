namespace FileOrganizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogEntriesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        LogEntryId = c.Int(nullable: false, identity: true),
                        File = c.String(),
                        From = c.String(),
                        To = c.String(),
                        Action = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LogEntryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogEntries");
        }
    }
}
