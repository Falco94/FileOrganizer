using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileOrganizer.Data;
using FileOrganizer.Dto;
using FileOrganizer.Enums;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using Runtime.Messaging;
using Runtime.MVC;

namespace FileOrganizer.ViewModels
{
    public class AddFileWatcherViewModel : ModelBase, INotifyable
    {
        private ObservableCollection<FileSystemWatcher> _fileSystemWatchers = new ObservableCollection<FileSystemWatcher>();
        private ObservableCollection<FileSystemWatcherDto> _fileSystemWatcherDtos;
        private Dictionary<int, FileSystemWatcher> _fileSystemWatcherMapping = new Dictionary<int, FileSystemWatcher>();

        public override void Initialize(params object[] parameter)
        {
            base.Initialize(parameter);

            var dataModel = new FileOrganizerDataModel();

            FileSystemWatcherDtos = new ObservableCollection<FileSystemWatcherDto>(dataModel.FileSystemWatchers.ToList());

            if (FileSystemWatcherDtos.Any())
            {
                foreach (var fileSystemWatcherDto in FileSystemWatcherDtos)
                {
                    var watcher = new FileSystemWatcher
                    {
                        Path = fileSystemWatcherDto.Path,
                        EnableRaisingEvents = fileSystemWatcherDto.Active
                    };

                    FileSystemWatchers.Add(watcher);
                    _fileSystemWatcherMapping.Add(fileSystemWatcherDto.Id, watcher);
                }

                foreach (var fileSystemWatcher in _fileSystemWatchers)
                {
                    fileSystemWatcher.Created += Filewatcher_Created;
                }
            }

            MessageBus.Default.RegisterMessageToken(this, MessageToken.FileSystemWatcherPathChanged);
        }

        public void Filewatcher_Created(object sender, FileSystemEventArgs e)
        {
            var fileCopier = new FileCopier();

            //TODO: Kopieren für alle Zuordnungen, oder nur die Datei!?
            fileCopier.Copy(Path.GetDirectoryName(e.FullPath));
            //fileCopier.Copy(Path.GetDirectoryName(e.FullPath), new List<string> {Path.GetExtension(e.FullPath)});
        }

        public ObservableCollection<FileSystemWatcher> FileSystemWatchers
        {
            get
            {
                return _fileSystemWatchers ?? (_fileSystemWatchers = new ObservableCollection<FileSystemWatcher>());
            }

            set
            {
                _fileSystemWatchers = value;
                OnPropertyChanged(nameof(FileSystemWatchers));
            }
        }

        public ObservableCollection<FileSystemWatcherDto> FileSystemWatcherDtos
        {
            get
            {
                return _fileSystemWatcherDtos;
            }

            set
            {
                _fileSystemWatcherDtos = value;
                OnPropertyChanged(nameof(FileSystemWatcherDtos));
            }
        }

        public Dictionary<int, FileSystemWatcher> FileSystemWatcherMapping
        {
            get
            {
                return _fileSystemWatcherMapping;
            }

            set
            {
                _fileSystemWatcherMapping = value;
            }
        }

        public void Notify(object sender, NotificationEventArgs e)
        {
            switch (e.Token)
            {
                case MessageToken.FileSystemWatcherPathChanged:
                {
                    var changedFilewatcher = e.Data.FirstOrDefault();

                    var filewatcher = _fileSystemWatchers.FirstOrDefault(x => x == changedFilewatcher);
                        _fileSystemWatchers = new ObservableCollection<FileSystemWatcher>(_fileSystemWatchers);
                }
                    break;
            }
        }
    }
}
