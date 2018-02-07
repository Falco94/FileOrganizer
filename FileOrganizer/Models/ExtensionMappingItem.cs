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

        public ExtensionMappingItem()
        {
            IsActive = true;
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
        
        public Extension Extension { get; set; }

        public ExtensionGroup ExtensionGroup { get; set; }

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
    }
}
