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
    public class LogEntry : DataModelBase
    {
        private int _logEntryId;
        private string _file;
        private string _from;
        private string _to;
        private DateTime _dateTime;
        private FileAction _action;

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

        public string File
        {
            get { return _file; }
            set
            {
                _file = value;
                OnPropertyChanged();
            }
        }

        public string From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged();
            }
        }

        public string To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged();
            }
        }

        public FileAction Action
        {
            get { return _action; }
            set
            {
                _action = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                _dateTime = value; 
                OnPropertyChanged();
            }
        }
    }
}
