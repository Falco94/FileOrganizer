using Runtime.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Models
{
    public class ExtensionGroup : DataModelBase
    {
        private string _name;
        private ObservableCollection<Extension> _extensions = new ObservableCollection<Extension>();

        public ObservableCollection<Extension> Extensions
        {
            get
            {
                return _extensions;
            }

            set
            {
                _extensions = value;
                OnPropertyChanged(nameof(Extensions));
            }
        }
        
        [Key]
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
}
