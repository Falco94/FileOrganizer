using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FileOrganizer.Model.Annotations;
using FileOrganizer.Models;

namespace FileOrganizer.Model
{
    public class Extensions : INotifyPropertyChanged
    {
        private ObservableCollection<Extension> _loadedExtensions;

        public ObservableCollection<Extension> LoadedExtensions
        {
            get
            {
                return _loadedExtensions;
            }

            set
            {
                _loadedExtensions = value;
                OnPropertyChanged();
            }
        }

        public Extensions Init(IEnumerable<Extension> extensions)
        {
            LoadedExtensions = new ObservableCollection<Extension>(extensions);
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
