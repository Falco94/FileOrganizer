using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        private bool _extensionGroupsComboBoxIsEnabled;
        private bool _extensionsComboBoxIsEnabled;

        public ExtensionMapping()
        {

        }

        public ExtensionMapping Init(IEnumerable<ExtensionMappingItem> mappingItems,
            IEnumerable<Models.Extension> extensions, IEnumerable<Models.ExtensionGroup> extensionGroups)
        {
            MappingItems = mappingItems.ToObservableCollection();
            Extensions = extensions.ToObservableCollection();
            ExtensionGroups = extensionGroups.ToObservableCollection();

            //add null values
            Extensions.Add(new Extension());
            ExtensionGroups.Add(new ExtensionGroup());

            Extensions = Extensions.OrderBy(x => x.ExtensionId).ToObservableCollection();
            ExtensionGroups = ExtensionGroups.OrderBy(x => x.ExtensionGroupId).ToObservableCollection();

            return this;
        }

        public ObservableCollection<ExtensionMappingItem> MappingItems
        {
            get { return _mappingItems ?? new ObservableCollection<ExtensionMappingItem>(); }

            set
            {
                _mappingItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Models.Extension> Extensions
        {
            get { return _extensions; }

            set
            {
                _extensions = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ExtensionGroup> ExtensionGroups
        {
            get { return _extensionGroups; }

            set
            {
                _extensionGroups = value;
                OnPropertyChanged();
            }
        }

        public bool ExtensionGroupsComboBoxIsEnabled
        {
            get { return _extensionGroupsComboBoxIsEnabled; }
            set
            {
                _extensionGroupsComboBoxIsEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool ExtensionsComboBoxIsEnabled
        {
            get { return _extensionsComboBoxIsEnabled; }
            set
            {
                _extensionsComboBoxIsEnabled = value;
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