using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runtime.MVC;

namespace FileOrganizer.Models
{
    public class Extension : DataModelBase
    {
        private int _extensionId;
        private string _extensionName;

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExtensionId
        {
            get
            {
                return _extensionId;
            }

            set
            {
                _extensionId = value;
                OnPropertyChanged(nameof(ExtensionId));
            }
        }

        [Key, Column(Order = 1)]
        public string ExtensionName
        {
            get
            {
                return _extensionName;
            }

            set
            {
                _extensionName = value;
                OnPropertyChanged(nameof(ExtensionName));
            }
        }
    }
}
