using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Enums
{
    public static class PathHelper
    {
        public static string ExtensionsSavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "File Organizer", "Extensions.fal");

        public static string ExtensionAssignementsSavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "File Organizer", "ExtensionAssignements.fal");

        public static string ExtensionGroupsSavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "File Organizer", "ExtensionGroups.fal");

        public static string FileWatcherSavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "File Organizer", "Filewatcher.fal");

    }
}
