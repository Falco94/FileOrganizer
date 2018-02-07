using Runtime.MVC;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileOrganizer.Models
{
    public class ExtensionGroup : DataModelBase
    {
        private string _name;
        private ObservableCollection<Extension> _extensions = new ObservableCollection<Extension>();
        private int _extensionGroupId;

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExtensionGroupId
        {
            get { return _extensionGroupId; }
            set
            {
                _extensionGroupId = value; 
                OnPropertyChanged();
            }
        }
        
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
