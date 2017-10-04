using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Enums
{
    public static class MessageToken
    {
        public const string CloseExtensionSelectionView = "CloseExtensionSelectionViewCommand";
        public const string CloseFileCopyView = "CloseFileCopyView";
        public const string CloseView = "CloseView";
        public const string FileSystemWatcherPathChanged = "FileSystemWatcherPathChanged";
    }
}
