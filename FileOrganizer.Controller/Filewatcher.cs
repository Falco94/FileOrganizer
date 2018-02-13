using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using System.Windows.Forms;

namespace FileOrganizer.Controller
{
    public class Filewatcher : ContentController<Controller.Filewatcher, View.Filewatcher, Model.Filewatcher>
    {
        public static RoutedCommand AddNewAssignement
            => FileOrganizer.View.Filewatcher.AddNewAssignementCommand;

        public static RoutedCommand DeleteAssignement
            => FileOrganizer.View.Filewatcher.DeleteAssignementCommand;

        public static RoutedCommand ChooseFolder
            => FileOrganizer.View.Filewatcher.ChooseFolderCommand;

        public static RoutedCommand SaveAssignements
            => FileOrganizer.View.Filewatcher.SaveAssignementsCommand;

        private IEnumerable<Models.FileSystemWatcherDto> _fileSystemWatchers;

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.Filewatcher();
            this.Model = new Model.Filewatcher().Init(_fileSystemWatchers);

            this.BindAsync(AddNewAssignement, AddNewAssignementFn, CanAddNewAssignementCommand);
            this.BindAsync(SaveAssignements, SaveMappings, CanSaveMappings);
            this.BindAsync<FileSystemWatcherDto>(ChooseFolder, ChooseFolderDialog, CanChooseFolderDialog);
            this.BindAsync<FileSystemWatcherDto>(DeleteAssignement, DeleteMapping, CanDeleteMapping);

            //führt die Can... Methoden aus
            CommandManager.InvalidateRequerySuggested();
        }

        private async Task<bool> CanDeleteMapping(FileSystemWatcherDto arg)
        {
            return true;
        }

        private async Task DeleteMapping(FileSystemWatcherDto arg)
        {
            var context = ContextManager.Context();

            this.Model.FileSystemWatcherDtos.Remove(arg);

            if (arg.Id != 0)
            {
                context.Entry(arg).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private async Task<bool> CanChooseFolderDialog(FileSystemWatcherDto arg)
        {
            return true;
        }

        private async Task ChooseFolderDialog(FileSystemWatcherDto arg)
        {
            if (arg != null)
            {
                var folderBrowserDialog = new FolderBrowserDialog();

                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.Description = "Wählen Sie einen Zielordner";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    arg.Path = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private async Task<bool> CanSaveMappings()
        {
            return true;
        }

        private async Task SaveMappings()
        {
            var context = ContextManager.Context();

            var mappingItems = context.FileSystemWatchers.ToList();

            foreach (var fileSystemWatcherDto in this.Model.FileSystemWatcherDtos)
            {
                if (fileSystemWatcherDto.Id == 0)
                    context.Entry(fileSystemWatcherDto).State = EntityState.Added;
            }

            context.SaveChanges();
        }

        private async Task<bool> CanAddNewAssignementCommand()
        {
            return true;
        }

        private async Task AddNewAssignementFn()
        {
            this.Model.FileSystemWatcherDtos.Add(new FileSystemWatcherDto());
        }

        public Filewatcher(BITS.UI.WPF.Core.Controllers.Controller parent, IEnumerable<Models.FileSystemWatcherDto> fileSystemWatcher) : base(parent)
        {
            _fileSystemWatchers = fileSystemWatcher;
        }
    }
}
