using Runtime.MVC;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FileOrganizer.Models
{
    public class ExtensionGroup : DataModelBase
    {
        private string _name;
        private ObservableCollection<Extension> _extensions = new ObservableCollection<Extension>();
        
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

        public ObservableCollection<Extension> Extensions
        {
            get
            {
                if (_extensions == null)
                {
                    _extensions = new ObservableCollection<Extension>();
                }
                return _extensions;
            }

            set
            {
                _extensions = value;
                OnPropertyChanged(nameof(Extensions));
            }
        }
    }
}
