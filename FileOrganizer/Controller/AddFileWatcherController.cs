using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using FileOrganizer.Dto;
using FileOrganizer.Enums;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using FileOrganizer.ViewModels;
using Runtime.Messaging;
using Runtime.MVC;
using Runtime.Services.DefaultServices;
using Runtime.Services.Plumbing;

namespace FileOrganizer.Controller
{
    public class AddFileWatcherController : ControllerBase
    {
        public static readonly RoutedCommand AddFileWatcherCommand = new RoutedCommand("AddFileWatcher", typeof(AddFileWatcherController));
        public static readonly RoutedCommand ChooseFolderCommand = new RoutedCommand("ChooseFolder", typeof(AddFileWatcherController));
        public static readonly RoutedCommand SaveFileWatcherCommand = new RoutedCommand("SaveFileWatcher", typeof(AddFileWatcherController));

        private AddFileWatcherViewModel _viewModel;
        private IDataPersistenceService _persistenceService;

        public IDataPersistenceService PersistenceService
        {
            get
            {
                return _persistenceService ??
                       (_persistenceService = ServiceLocator.Default.GetService<IDataPersistenceService>());
            }

            set
            {
                _persistenceService = value;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            _viewModel = (AddFileWatcherViewModel)Model;
        }

        protected void CommandAddFileWatcher(object sender, ExecutedRoutedEventArgs e)
        {
            var filewatcher = new FileSystemWatcher();
            filewatcher.Created += _viewModel.Filewatcher_Created;

            _viewModel.FileSystemWatchers.Add(filewatcher);
        }

        protected void CheckCommandAddFileWatcher(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _viewModel.FileSystemWatchers.FirstOrDefault(x=>String.IsNullOrWhiteSpace(x.Path)) == null;
        }

        protected void CommandChooseFolder(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedFilesystemWatcher = e.Parameter as FileSystemWatcher;

            if (selectedFilesystemWatcher != null)
            {
                var folderBrowserDialog = new FolderBrowserDialog();

                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.Description = "Wählen Sie einen Zielordner";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilesystemWatcher.Path = folderBrowserDialog.SelectedPath;
                    _viewModel.OnPropertyChanged(nameof(selectedFilesystemWatcher.Path));
                }

                var notificationargs = new NotificationEventArgs();
                notificationargs.Data = new List<object>();
                notificationargs.Data.Add(selectedFilesystemWatcher);

                MessageBus.Default.BeginNotify(this, MessageToken.FileSystemWatcherPathChanged, notificationargs);
            }
        }

        protected void CheckCommandChooseFolder(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandSaveFileWatcher(object sender, ExecutedRoutedEventArgs e)
        {
            var watcherDtos = _viewModel.FileSystemWatchers.Select(x => new FileSystemWatcherDto
            {
                Active = x.EnableRaisingEvents,
                Path = x.Path
            }).ToList();

            PersistenceService.Save(watcherDtos, PathHelper.FileWatcherSavePath);
        }

        protected void CheckCommandSaveFileWatcher(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _viewModel.FileSystemWatchers.FirstOrDefault(x => String.IsNullOrWhiteSpace(x.Path)) == null;
        }
    }

}
