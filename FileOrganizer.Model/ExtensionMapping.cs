using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FileOrganizer.Model.Annotations;
using FileOrganizer.Models;
using Runtime.Extensions;

namespace FileOrganizer.Model
{
    public class ExtensionMapping : INotifyPropertyChanged
    {
        private ObservableCollection<ExtensionMappingItem> _mappingItems;
        private ObservableCollection<Models.Extension> _extensions;
        private ObservableCollection<ExtensionGroup> _extensionGroups;

        public ExtensionMapping()
        {
                
        }

        public ExtensionMapping Init(IEnumerable<ExtensionMappingItem> mappingItems, IEnumerable<Models.Extension> extensions, IEnumerable<Models.ExtensionGroup> extensionGroups)
        {
            MappingItems = mappingItems.ToObservableCollection();
            Extensions = extensions.ToObservableCollection();
            ExtensionGroups = extensionGroups.ToObservableCollection();
            return this;
        }

        public ObservableCollection<ExtensionMappingItem> MappingItems
        {
            get
            {
                return _mappingItems ?? new ObservableCollection<ExtensionMappingItem>();
            }

            set
            {
                _mappingItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Models.Extension> Extensions
        {
            get
            {
                return _extensions;
            }

            set
            {
                _extensions = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ExtensionGroup> ExtensionGroups
        {
            get
            {
                return _extensionGroups;
            }

            set
            {
                _extensionGroups = value;
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