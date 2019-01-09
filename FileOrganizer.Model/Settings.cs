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
    public class Settings : INotifyPropertyChanged
    {
        private ObservableCollection<Setting> _settings = new ObservableCollection<Setting>();
        private Setting _autostart;
        private Setting _subfolders;

        public Settings(IEnumerable<Setting> entries)
        {
            if(entries != null)
                SettingList = entries.ToObservableCollection();

            Autostart = _settings.FirstOrDefault(x => x.Name == "Autostart");
            Subfolders = _settings.FirstOrDefault(x => x.Name == "Subfolders");
        }

        public ObservableCollection<Setting> SettingList
        {
            get { return _settings; }
            set
            {
                _settings = value;
                OnPropertyChanged();
            }
        }

        public Setting Autostart
        {
            get { return _autostart; }
            set
            {
                _autostart = value; 
                OnPropertyChanged();
            }
        }

        public Setting Subfolders
        {
            get { return _subfolders; }
            set
            {
                _subfolders = value;
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