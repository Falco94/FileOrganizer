using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Runtime.MVC;

namespace FileOrganizer.Models
{
    public class FileSystemWatcherDto : DataModelBase
    {
        private string _path;
        private bool _active;
        private int _id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get
            {
                return _active;
            }

            set
            {
                _active = value;
                OnPropertyChanged();
            }
        }
    }
}
