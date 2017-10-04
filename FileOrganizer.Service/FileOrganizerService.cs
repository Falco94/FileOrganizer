using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileOrganizer.Data;
using FileOrganizer.Dto;
using FileOrganizer.IService;

namespace FileOrganizer.Service
{
    public class FileOrganizerService : IFileOrganizerService
    {
        public IEnumerable<FileSystemWatcherDto> LoadFileSystemWatchers()
        {
            return new FileOrganizerDataModel().FileSystemWatchers;
        }
    }
}
