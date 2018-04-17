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
    public class ExtensionGroups : INotifyPropertyChanged, IDataErrorInfo
    {
        private ObservableCollection<ExtensionGroup> _loadedExtensionGroups = new ObservableCollection<ExtensionGroup>();
        private IEnumerable<Extension> _availableExtensions;
        private bool _isBusy;

        public ExtensionGroups(IEnumerable<ExtensionGroup> extensionGroups, IEnumerable<Extension> extensions)
        {
            if(extensionGroups != null)
                _loadedExtensionGroups = new ObservableCollection<ExtensionGroup>(extensionGroups);

            _availableExtensions = extensions;
            SetAvailableExtensions();

            LoadedExtensionGroups.CollectionChanged += (sender, args) => SetAvailableExtensions();
        }

        private void SetAvailableExtensions()
        {
            _availableExtensions =
                _availableExtensions?.Where(x => !_loadedExtensionGroups.Any(y => y.Extensions.Any(z => z.ExtensionId == x.ExtensionId)));
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

        public bool IsValid
        {
            get
            {
                foreach (var property in PropertyList)
                {
                    if (GetValidationError(property) != null)
                        return false;
                }

                return true;
            }
        }

        static readonly string[] PropertyList =
        {
            nameof(LoadedExtensionGroups)
        };

        string IDataErrorInfo.this[string propertyName]
        {
            get { return GetValidationError(propertyName); }
        }

        string GetValidationError(string propertyName)
        {
            string error = null;

            switch (propertyName)
            {
                case nameof(LoadedExtensionGroups):
                    error = ValidateLoadedExtensionGroups();
                    break;
            }

            return error;
        }

        private string ValidateLoadedExtensionGroups()
        {
            if (this.LoadedExtensionGroups.FirstOrDefault(x => x.ExtensionGroupId == 0) != null)
            {
                return "Neu angelegte Gruppen müssen erst gespeichert werden!";
            }
            else
                return null;
        }

        string IDataErrorInfo.Error { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
