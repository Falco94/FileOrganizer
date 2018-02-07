using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Runtime.MVC;

namespace FileOrganizer.Dto
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
                OnPropertyChanged(nameof(Path));
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
                OnPropertyChanged(nameof(Active));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
