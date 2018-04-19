using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using FileOrganizer.IService;
using FileOrganizer.Models;

namespace FileOrganizer.Service
{
    public class FileOrganizerService : IFileOrganizerService
    {
        public IEnumerable<FileSystemWatcherDto> LoadFileSystemWatchers()
        {
            return ContextManager.Context().FileSystemWatchers;
        }
    }
}
