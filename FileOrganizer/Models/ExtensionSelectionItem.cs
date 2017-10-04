using Runtime.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Models
{
    public class ExtensionSelectionItem : DataModelBase
    {
        private string _extension;
        private bool _isSelected;

        public string Extension
        {
            get
            {
                return _extension;
            }

            set
            {
                _extension = value;
                OnPropertyChanged(nameof(this.Extension));
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(this.IsSelected));
            }
        }
    }
}
