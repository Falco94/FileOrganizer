using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FileOrganizer.Model.Annotations;
using FileOrganizer.Models;

namespace FileOrganizer.Model
{
    public class ExtensionGroups : INotifyPropertyChanged
    {
        private ObservableCollection<ExtensionGroup> _loadedExtensionGroups;
        private IEnumerable<Extension> _availableExtensions;

        public ExtensionGroups(IEnumerable<ExtensionGroup> extensionGroups, IEnumerable<Extension> extensions)
        {
            _loadedExtensionGroups = new ObservableCollection<ExtensionGroup>(extensionGroups);

            _availableExtensions = extensions;
            SetAvailableExtensions();

            LoadedExtensionGroups.CollectionChanged += (sender, args) => SetAvailableExtensions();
        }

        private void SetAvailableExtensions()
        {
            _availableExtensions =
                _availableExtensions?.Where(x => !_loadedExtensionGroups.Any(y => y.Extensions.Any(z => z.Id == x.Id)));
        }

        public ObservableCollection<ExtensionGroup> LoadedExtensionGroups
        {
            get { return _loadedExtensionGroups; }

            set
            {
                _loadedExtensionGroups = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Extension> AvailableExtensions
        {
            get { return _availableExtensions; }

            set
            {
                _availableExtensions = value;
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
