using FileOrganizer.Enums;
using FileOrganizer.Test;
using Runtime.Messaging;
using Runtime.MVC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FileOrganizer.ViewModels
{
    public class FileCopyViewModel : ModelBase
    {
        private IEnumerable<string> _files;
        private string _destinationPath;

        private int _currentFile;
        private int _fileCount;
        private double _progress;
        private bool _overwrite;
        private bool _overwriteForAll;
        private bool _showUserRequest;
        private bool _forAll;
        private bool _skip;
        private bool _skipForAll;
        private bool _cancel;

        public FileCopyViewModel(IEnumerable<string> files, string destinationPath)
        {
            Files = files;
            DestinationPath = destinationPath;
        }

        public override void Initialize(object[] args)
        {
            base.Initialize(args);

            FileCount = Files.Count();
            CurrentFile = 0;

            Copy();
        }

        private void Copy()
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            bool cancelFlag = false;

            var maxBytes = _files.Sum(x => new FileInfo(x).Length);

            foreach (var sourceFilePath in _files)
            {
                var filename = Path.GetFileName(sourceFilePath);
                var destFilePath = Path.Combine(_destinationPath, filename);

                using (FileStream source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream dest = new FileStream(destFilePath, FileMode.CreateNew, FileAccess.Write))
                    {
                        long totalBytes = 0;
                        int currentBlockSize = 0;

                        while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            totalBytes += currentBlockSize;
                            Progress += (double)totalBytes * 100.0 / maxBytes;

                            dest.Write(buffer, 0, currentBlockSize);

                            cancelFlag = false;

                            if (cancelFlag == true)
                            {
                                // Delete dest file here
                                break;
                            }
                        }
                    }
                }

            }
            
        }

        //private void CopyFiles()
        //{
        //    foreach (var file in _files)
        //    {
        //        CurrentFile++;

        //        if(_skipForAll)
        //        {
        //            continue;
        //        }

        //        if(_skip)
        //        {
        //            _skip = false;
        //            continue;
        //        }

        //        var filename = Path.GetFileName(file);
        //        var newFilepath = Path.Combine(_destinationPath, filename);

        //        if (_overwriteForAll)
        //        {
        //            File.Copy(file, newFilepath, true);
        //        }
        //        else if(_overwrite)
        //        {
        //            File.Copy(file, newFilepath, true);
        //            _overwrite = false;
        //        }
        //        else
        //        {
        //            //Datei existiert bereits --> Benutzerabfrage
        //            if(File.Exists(newFilepath))
        //            {
        //                ShowUserRequest = true;

        //                while (ShowUserRequest)
        //                {
        //                    if (this.Dispatcher.HasShutdownStarted || this.Dispatcher.HasShutdownFinished)
        //                    {
        //                        break;
        //                    }

        //                    //Simuliert DoEvents()
        //                    this.Dispatcher.Invoke(
        //                        DispatcherPriority.Background,
        //                        new ThreadStart(delegate { }));
        //                    Thread.Sleep(20);
        //                }

        //                //Nicht abgebrochen
        //                if(!_cancel)
        //                {
        //                    //Überschreiben
        //                    if(_overwrite)
        //                    {
        //                        File.Copy(file, newFilepath, true);
        //                        _overwrite = false;
        //                    }
        //                    //Überspringen
        //                    else
        //                    {
        //                        continue;
        //                    }
        //                }
        //                //Abgebrochen
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    MessageBus.Default.BeginNotify(this, MessageToken.CloseFileCopyView, null);
        //}

        public IEnumerable<string> Files
        {
            get
            {
                return _files;
            }

            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }

        public string DestinationPath
        {
            get
            {
                return _destinationPath;
            }

            set
            {
                _destinationPath = value;
                OnPropertyChanged(nameof(DestinationPath));
            }
        }

        public bool OverwriteForAll
        {
            get
            {
                return _overwriteForAll;
            }

            set
            {
                _overwriteForAll = value;
                OnPropertyChanged(nameof(OverwriteForAll));
            }
        }

        public bool Overwrite
        {
            get
            {
                return _overwrite;
            }

            set
            {
                _overwrite = value;
                OnPropertyChanged(nameof(Overwrite));
            }
        }

        public int CurrentFile
        {
            get
            {
                return _currentFile;
            }

            set
            {
                _currentFile = value;
                OnPropertyChanged(nameof(CurrentFile));
            }
        }

        public int FileCount
        {
            get
            {
                return _fileCount;
            }

            set
            {
                _fileCount = value;
                OnPropertyChanged(nameof(FileCount));
            }
        }

        public bool ShowUserRequest
        {
            get
            {
                return _showUserRequest;
            }

            set
            {
                _showUserRequest = value;
                OnPropertyChanged(nameof(ShowUserRequest));
            }
        }

        public bool Cancel
        {
            get
            {
                return _cancel;
            }

            set
            {
                _cancel = value;
                OnPropertyChanged(nameof(Cancel));
            }
        }

        public bool ForAll
        {
            get
            {
                return _forAll;
            }

            set
            {
                _forAll = value;
                OnPropertyChanged(nameof(ForAll));
            }
        }

        public bool Skip
        {
            get
            {
                return _skip;
            }

            set
            {
                _skip = value;
                OnPropertyChanged(nameof(Skip));
            }
        }

        public bool SkipForAll
        {
            get
            {
                return _skipForAll;
            }

            set
            {
                _skipForAll = value;
                OnPropertyChanged(nameof(SkipForAll));
            }
        }

        public double Progress
        {
            get
            {
                return _progress;
            }

            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
    }
}
