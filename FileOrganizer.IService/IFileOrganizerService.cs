using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileOrganizer.Models;

namespace FileOrganizer.IService
{
    public interface IFileOrganizerService
    {
        IEnumerable<FileSystemWatcherDto> LoadFileSystemWatchers();
    }
}
