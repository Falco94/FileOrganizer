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
        private IEnumerable<Extension> _loadedExtensions;
        private ObservableCollection<Extension> _shownExtensions = new ObservableCollection<Extension>();
        private bool _isBusy;
        private bool _searchSubfolders;
        private string _filter;

        public IEnumerable<Extension> LoadedExtensions
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
            if (extensions != null)
            {
                LoadedExtensions = extensions.OrderBy(x => x.ExtensionName);
                ShownExtensions = new ObservableCollection<Extension>(LoadedExtensions);
            }

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

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;

                if (LoadedExtensions != null)
                {
                    if (string.IsNullOrWhiteSpace(_filter))
                    {
                        ShownExtensions = new ObservableCollection<Extension>(LoadedExtensions);
                    }
                    else
                    {
                        ShownExtensions = new ObservableCollection<Extension>(
                            LoadedExtensions.Where(x => x.ExtensionName.ToLower().Contains(_filter.ToLower())));
                    }
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Extension> ShownExtensions
        {
            get { return _shownExtensions; }
            set
            {
                _shownExtensions = value;
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
