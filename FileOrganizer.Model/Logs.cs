using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FileOrganizer.Helper;
using FileOrganizer.Model.Annotations;
using FileOrganizer.Models;
using Runtime.Extensions;

namespace FileOrganizer.Model
{
    public class Logs : INotifyPropertyChanged
    {
        private ObservableCollection<LogEntry> _logEntries = new ObservableCollection<LogEntry>();

        public Logs(IEnumerable<LogEntry> entries)
        {
            if(entries != null)
                LogEntries = entries.ToObservableCollection();
        }

        public ObservableCollection<LogEntry> LogEntries
        {
            get { return _logEntries; }
            set
            {
                _logEntries = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}