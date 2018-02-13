namespace FileOrganizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogEntriesAddedDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogEntries", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogEntries", "DateTime");
        }
    }
}
