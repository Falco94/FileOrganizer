using System.Data.Entity;
using FileOrganizer.Models;

namespace FileOrganizer.Data
{
    public class FileOrganizerDataModel : DbContext
    {
        // Der Kontext wurde für die Verwendung einer FileOrganizerModel-Verbindungszeichenfolge aus der 
        // Konfigurationsdatei ('App.config' oder 'Web.config') der Anwendung konfiguriert. Diese Verbindungszeichenfolge hat standardmäßig die 
        // Datenbank 'FileOrganizer.FileOrganizerModel' auf der LocalDb-Instanz als Ziel. 
        // 
        // Wenn Sie eine andere Datenbank und/oder einen anderen Anbieter als Ziel verwenden möchten, ändern Sie die FileOrganizerModel-Zeichenfolge 
        // in der Anwendungskonfigurationsdatei.
        public FileOrganizerDataModel()
            : base("name=FileOrganizer")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<FileOrganizerDataModel>(null);
            base.OnModelCreating(modelBuilder);
        }

        // Fügen Sie ein 'DbSet' für jeden Entitätstyp hinzu, den Sie in das Modell einschließen möchten. Weitere Informationen 
        // zum Konfigurieren und Verwenden eines Code First-Modells finden Sie unter 'http://go.microsoft.com/fwlink/?LinkId=390109'.

        public virtual DbSet<ExtensionGroup> ExtensionGroups { get; set; }
        public virtual DbSet<ExtensionMappingItem> ExtensionMappingItems { get; set; }
        public virtual DbSet<Extension> Extensions { get; set; }
        public virtual DbSet<FileSystemWatcherDto> FileSystemWatchers { get; set; }
}
}