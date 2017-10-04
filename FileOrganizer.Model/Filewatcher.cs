using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using FileOrganizer.Model.Annotations;
using Runtime.Extensions;


namespace FileOrganizer.Model
{
    public class Filewatcher : INotifyPropertyChanged
    {
        private ObservableCollection<FileSystemWatcher> _fileSystemWatchers = new ObservableCollection<FileSystemWatcher>();
        private ObservableCollection<FileSystemWatcherDto> _fileSystemWatcherDtos;
        private Dictionary<int, FileSystemWatcher> _fileSystemWatcherMapping = new Dictionary<int, FileSystemWatcher>();

        public Filewatcher Init(IEnumerable<FileSystemWatcherDto> fileSystemWatcherDtos)
        {
            FileSystemWatcherDtos = fileSystemWatcherDtos.ToObservableCollection();

            return this;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FileSystemWatcherDto : INotifyPropertyChanged
    {
        private string _path;
        private bool _active;
        private int _id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get { return _id; }

            set { _id = value; }
        }

        public string Path
        {
            get { return _path; }

            set
            {
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        public bool Active
        {
            get { return _active; }

            set
            {
                _active = value;
                OnPropertyChanged(nameof(Active));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
