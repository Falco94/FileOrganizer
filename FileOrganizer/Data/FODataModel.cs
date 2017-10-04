using FileOrganizer.Dto;
using FileOrganizer.Models;

namespace FileOrganizer.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class FODataModel : DbContext
    {
        // Der Kontext wurde für die Verwendung einer FODataModel-Verbindungszeichenfolge aus der 
        // Konfigurationsdatei ('App.config' oder 'Web.config') der Anwendung konfiguriert. Diese Verbindungszeichenfolge hat standardmäßig die 
        // Datenbank 'FileOrganizer.Data.FODataModel' auf der LocalDb-Instanz als Ziel. 
        // 
        // Wenn Sie eine andere Datenbank und/oder einen anderen Anbieter als Ziel verwenden möchten, ändern Sie die FODataModel-Zeichenfolge 
        // in der Anwendungskonfigurationsdatei.
        public FODataModel()
            : base("name=FODataModel")
        {
        }

        // Fügen Sie ein 'DbSet' für jeden Entitätstyp hinzu, den Sie in das Modell einschließen möchten. Weitere Informationen 
        // zum Konfigurieren und Verwenden eines Code First-Modells finden Sie unter 'http://go.microsoft.com/fwlink/?LinkId=390109'.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<ExtensionGroup> ExtensionGroups { get; set; }
        public virtual DbSet<ExtensionMappingItem> ExtensionMappingItems { get; set; }
        public virtual DbSet<Extension> Extensions { get; set; }
        public virtual DbSet<FileSystemWatcherDto> FileSystemWatchers { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}