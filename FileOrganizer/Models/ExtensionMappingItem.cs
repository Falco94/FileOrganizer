using Runtime.MVC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Models
{
    public class ExtensionMappingItem : DataModelBase
    {
        private int _extensionMappingItemId;
        private string _extensionId;
        private string _targetPath;
        private bool _isActive;
        private bool _extensionGroupsComboBoxIsEnabled;
        private bool _extensionsComboBoxIsEnabled;
        private Extension _extension;
        private ExtensionGroup _extensionGroup;

        public ExtensionMappingItem()
        {
            IsActive = true;
            ExtensionGroupsComboBoxIsEnabled = true;
            ExtensionsComboBoxIsEnabled = true;
        }

        [Key]
        public int ExtensionMappingItemId
        {
            get
            {
                return _extensionMappingItemId;
            }

            set
            {
                _extensionMappingItemId = value;
                OnPropertyChanged(nameof(ExtensionMappingItemId));
            }
        }

        public Extension Extension
        {
            get { return _extension;}
            set
            {
                _extension = value;
                
                ExtensionGroupsComboBoxIsEnabled = _extension?.ExtensionId == 0;
            }
        }

        public ExtensionGroup ExtensionGroup
        {
            get => _extensionGroup;
            set
            {
                _extensionGroup = value;
                ExtensionsComboBoxIsEnabled = _extensionGroup?.ExtensionGroupId == 0;
            }
        }

        public string TargetPath
        {
            get
            {
                return _targetPath;
            }

            set
            {
                _targetPath = value;
                OnPropertyChanged(nameof(TargetPath));
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        [NotMapped]
        public bool ExtensionGroupsComboBoxIsEnabled
        {
            get { return _extensionGroupsComboBoxIsEnabled; }
            set
            {
                _extensionGroupsComboBoxIsEnabled = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        public bool ExtensionsComboBoxIsEnabled
        {
            get { return _extensionsComboBoxIsEnabled; }
            set
            {
                _extensionsComboBoxIsEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}
