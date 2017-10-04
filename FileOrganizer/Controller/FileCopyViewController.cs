using FileOrganizer.Helper;
using FileOrganizer.Models;
using FileOrganizer.Test;
using FileOrganizer.ViewModels;
using Runtime.MVC;
using Runtime.Services.DefaultServices;
using Runtime.Services.Plumbing;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace FileOrganizer.Controller
{
    public class FileCopyViewController : ControllerBase
    {
        public static readonly RoutedCommand OverwriteCommand = new RoutedCommand("Overwrite", typeof(FileCopyViewController));
        public static readonly RoutedCommand SkipCommand = new RoutedCommand("Skip", typeof(FileCopyViewController));
        public static readonly RoutedCommand CancelCommand = new RoutedCommand("Cancel", typeof(FileCopyViewController));
        

        private FileCopyViewModel _fileCopyViewModel;

        public override void Initialize()
        {
            base.Initialize();

            _fileCopyViewModel = (FileCopyViewModel)Model;
        }

        protected void CheckCommandOverwrite(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandOverwrite(object sender, ExecutedRoutedEventArgs e)
        {
            if (_fileCopyViewModel.ForAll)
            {
                _fileCopyViewModel.OverwriteForAll = true;
                _fileCopyViewModel.Overwrite = true;
            }
            else
            {
                _fileCopyViewModel.Overwrite = true;
            }

            _fileCopyViewModel.ShowUserRequest = false;
        }

        protected void CheckCommandSkip(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandSkip(object sender, ExecutedRoutedEventArgs e)
        {
            if (_fileCopyViewModel.ForAll)
            {
                _fileCopyViewModel.SkipForAll = true;
                _fileCopyViewModel.Skip = true;
            }
            else
            {
                _fileCopyViewModel.Skip = true;
            }
            _fileCopyViewModel.ShowUserRequest = false;
        }

        protected void CheckCommandCancel(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandCancel(object sender, ExecutedRoutedEventArgs e)
        {
            _fileCopyViewModel.Cancel = true;
            _fileCopyViewModel.ShowUserRequest = false;
        }
    }
}
