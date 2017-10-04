using FileOrganizer.Helper;
using FileOrganizer.Models;
using FileOrganizer.Test;
using FileOrganizer.ViewModels;
using FileOrganizer.Views;
using Runtime.MVC;
using Runtime.Services.DefaultServices;
using Runtime.Services.Plumbing;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.VisualBasic.FileIO;

namespace FileOrganizer.Controller
{
    public class FileOrganizerMainViewController : ControllerBase
    {
        public static readonly RoutedCommand CopyFilesToDestinationCommand = new RoutedCommand("CopyFilesToDestination", typeof(FileOrganizerMainViewController));
        public static readonly RoutedCommand SaveExtensionsCommand = new RoutedCommand("SaveExtensions", typeof(FileOrganizerMainViewController));
        public static readonly RoutedCommand AddExtensionCommand = new RoutedCommand("AddExtension", typeof(FileOrganizerMainViewController));
        

        private FileOrganizerMainViewModel _fileOrganizerMainViewModel;

        public override void Initialize()
        {
            base.Initialize();

            _fileOrganizerMainViewModel = (FileOrganizerMainViewModel)Model;
        }

        protected void CheckCommandCopyFilesToDestination(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandCopyFilesToDestination(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var mappingItem in _fileOrganizerMainViewModel.MappingItems)
            {
                //Alle Dateien im Verzeichnis mit der entsprechenden Extension
                var files = Directory.GetFiles(TestPaths.Eingabeordner)
                    .Where(x => Path.GetExtension(x) == mappingItem.Extension.ExtensionName).ToList();

                if (files.Count > 0)
                {
                    var copier = new FileCopier();
                    //copier.Copy();
                    //foreach (var sourceFile in files)
                    //{
                    //    var destinationFile = Path.Combine(TestPaths.Ausgabeordner, Path.GetFileName(sourceFile));



                    //    FileSystem.CopyFile(sourceFile, destinationFile, UIOption.AllDialogs);
                    //}

                    copier.Copy(files.ToArray(), TestPaths.Ausgabeordner);
                }
            }
        }
    }
}
