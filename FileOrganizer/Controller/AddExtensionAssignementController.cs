using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using FileOrganizer.Data;
using FileOrganizer.Enums;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using FileOrganizer.ViewModels;
using Runtime.MVC;
using Binding = System.Windows.Forms.Binding;

namespace FileOrganizer.Controller
{
    public class AddExtensionAssignementController : ControllerBase
    {
        public static readonly RoutedCommand AddNewAssignementCommand = new RoutedCommand("AddNewAssignement", typeof(AddExtensionAssignementController));
        public static readonly RoutedCommand ChooseFolderCommand = new RoutedCommand("ChooseFolder", typeof(AddExtensionAssignementController));
        public static readonly RoutedCommand SaveAssignementsCommand = new RoutedCommand("SaveAssignements", typeof(AddExtensionAssignementController));
        public static readonly RoutedCommand DeleteAssignementCommand = new RoutedCommand("DeleteAssignement", typeof(AddExtensionAssignementController));

        private AddExtensionAssignementViewModel _viewModel;

        public override void Initialize()
        {
            base.Initialize();

            _viewModel = (AddExtensionAssignementViewModel)Model;
        }

        public static bool IsValid(DependencyObject parent)
        {
            if (Validation.GetHasError(parent))
                return false;

            // Validate all the bindings on the children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (!IsValid(child)) { return false; }
            }

            return true;
        }

        protected void CommandAddNewAssignement(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModel.MappingItems.Add(new ExtensionMappingItem());
        }

        protected void CheckCommandAddNewAssignement(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _viewModel.MappingItems?.FirstOrDefault(x => String.IsNullOrWhiteSpace(x.Extension.ExtensionName)
                                                                       || String.IsNullOrWhiteSpace(x.TargetPath)) == null;
        }

        protected void CommandChooseFolder(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedMappingItem = e.Parameter as ExtensionMappingItem;

            if (selectedMappingItem != null)
            {
                var folderBrowserDialog = new FolderBrowserDialog();

                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.Description = "Wählen Sie einen Zielordner";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {

                    selectedMappingItem.TargetPath = folderBrowserDialog.SelectedPath;

                }
            }
        }

        protected void CheckCommandChooseFolder(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandSaveAssignements(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModel.PersistenceService.Save(_viewModel.MappingItems, PathHelper.ExtensionAssignementsSavePath);
        }

        protected void CheckCommandSaveAssignements(object sender, CanExecuteRoutedEventArgs e)
        {
            var isValid = IsValid(sender as DependencyObject);

            e.CanExecute = _viewModel.MappingItems.FirstOrDefault(x => String.IsNullOrWhiteSpace(x.Extension.ExtensionName)
                                                           || String.IsNullOrWhiteSpace(x.TargetPath)) == null
                                                           && isValid;
        }

        protected void CommandDeleteAssignement(object sender, ExecutedRoutedEventArgs e)
        {
            var dataModel = new FileOrganizerDataModel();

            var selectedMappingItem = e.Parameter as ExtensionMappingItem;

            if (selectedMappingItem != null)
            {
                _viewModel.MappingItems.Remove(selectedMappingItem);
                var validItemsToSave = _viewModel.MappingItems.Where(x => x.IsActive);

                var extensionMappingItems = validItemsToSave as ExtensionMappingItem[] ?? validItemsToSave.ToArray();

                if (extensionMappingItems.Any())
                {
                    //TODO:
                    //dataModel.ExtensionMappingItems .SaveExtensionMappingItems(extensionMappingItems);
                }
            }
        }

        protected void CheckCommandDeleteAssignement(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }

}
