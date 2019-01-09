using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileOrganizer.Enums;
using Runtime.MVC;

namespace FileOrganizer.Models
{
    public class Setting : DataModelBase
    {
        private int _logEntryId;
        private string _name;
        private string _value;

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogEntryId
        {
            get
            {
                return _logEntryId;
            }

            set
            {
                _logEntryId = value;
                OnPropertyChanged();
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }
}
