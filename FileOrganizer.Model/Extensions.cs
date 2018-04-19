using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FileOrganizer.Model.Annotations;
using FileOrganizer.Models;

namespace FileOrganizer.Model
{
    public class Extensions : INotifyPropertyChanged
    {
        private ObservableCollection<Extension> _loadedExtensions = new ObservableCollection<Extension>();
        private bool _isBusy;
        private bool _searchSubfolders;

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
            if(extensions != null)
                LoadedExtensions = new ObservableCollection<Extension>(extensions.OrderBy(x => x.ExtensionName));

            return this;
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public bool SearchSubfolders
        {
            get { return _searchSubfolders; }
            set
            {
                _searchSubfolders = value;
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
