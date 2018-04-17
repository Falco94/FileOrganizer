using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using System.Windows.Media;
using FileOrganizer.Controller.Helper;
using MahApps.Metro.Controls.Dialogs;
using Binding = System.Windows.Forms.Binding;
using System.Windows.Forms;

namespace FileOrganizer.Controller
{
    public class Filewatcher : ContentController<Controller.Filewatcher, View.Filewatcher, Model.Filewatcher>
    {
        public static RoutedCommand AddNewAssignement
            => FileOrganizer.View.Filewatcher.AddNewAssignementCommand;

        public static RoutedCommand DeleteAssignement
            => FileOrganizer.View.Filewatcher.DeleteAssignementCommand;

        public static RoutedCommand ChooseFolder
            => FileOrganizer.View.Filewatcher.ChooseFolderCommand;

        public static RoutedCommand SaveAssignements
            => FileOrganizer.View.Filewatcher.SaveAssignementsCommand;

        private IEnumerable<Models.FileSystemWatcherDto> _fileSystemWatchers;

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.Filewatcher();
            this.Model = new Model.Filewatcher().Init(_fileSystemWatchers);

            this.BindAsync(AddNewAssignement, AddNewAssignementFn, CanAddNewAssignementCommand);
            this.BindAsync(SaveAssignements, SaveMappings, CanSaveMappings);
            this.BindAsync<FileSystemWatcherDto>(ChooseFolder, ChooseFolderDialog, CanChooseFolderDialog);
            this.BindAsync<FileSystemWatcherDto>(DeleteAssignement, DeleteMapping, CanDeleteMapping);

            //führt die Can... Methoden aus
            CommandManager.InvalidateRequerySuggested();

            IsValid(this.View.ItemsControl);
        }

        private async Task<bool> CanDeleteMapping(FileSystemWatcherDto arg)
        {
            return true;
        }

        private async Task DeleteMapping(FileSystemWatcherDto arg)
        {
            var context = ContextManager.Context();

            this.Model.FileSystemWatcherDtos.Remove(arg);

            if (arg.Id != 0)
            {
                context.Entry(arg).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private async Task<bool> CanChooseFolderDialog(FileSystemWatcherDto arg)
        {
            return true;
        }

        private async Task ChooseFolderDialog(FileSystemWatcherDto arg)
        {
            if (arg != null)
            {
                var folderBrowserDialog = new FolderBrowserDialog();

                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.Description = "Choose a target folder";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    arg.Path = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private async Task<bool> CanSaveMappings()
        {
            return true;
        }

        private async Task SaveMappings()
        {
            var context = ContextManager.Context();

            var mappingItems = context.FileSystemWatchers.ToList();

            foreach (var fileSystemWatcherDto in this.Model.FileSystemWatcherDtos)
            {
                if (fileSystemWatcherDto.Id == 0)
                    context.Entry(fileSystemWatcherDto).State = EntityState.Added;
            }

            int changes = 0;

            SafeExecutor.ExecuteFn(() =>
            {
                changes = context.SaveChanges();
            }, "Filewatcher.SaveMappings");


            if (changes > 0)
            {
                await DialogHandler.DialogRoot.ShowMessageAsync("Success", $"Saved {changes} entries",
                    MessageDialogStyle.Affirmative);
            }
        }

        private async Task<bool> CanAddNewAssignementCommand()
        {
            return true;
        }

        private async Task AddNewAssignementFn()
        {
            this.Model.FileSystemWatcherDtos.Add(new FileSystemWatcherDto());
        }

        public Filewatcher(BITS.UI.WPF.Core.Controllers.Controller parent, IEnumerable<Models.FileSystemWatcherDto> fileSystemWatcher) : base(parent)
        {
            _fileSystemWatchers = fileSystemWatcher;
        }

        private static Dictionary<Type, List<DependencyProperty>> PropertiesReflectionChace = new Dictionary<Type, List<DependencyProperty>>();

        private static List<DependencyProperty> GetDPs(Type t)
        {
            if (PropertiesReflectionChace.ContainsKey(t))
                return PropertiesReflectionChace[t];
            FieldInfo[] properties = t.GetFields(BindingFlags.Public | BindingFlags.GetProperty |
                                                 BindingFlags.Static | BindingFlags.FlattenHierarchy);
            // we cycle and store only the dependency properties
            List<DependencyProperty> dps = new List<DependencyProperty>();

            foreach (FieldInfo field in properties)
                if (field.FieldType == typeof(DependencyProperty))
                    dps.Add((DependencyProperty)field.GetValue(null));
            PropertiesReflectionChace.Add(t, dps);

            return dps;
        }

        public static bool IsValid(DependencyObject parent)
        {
            // Validate all the bindings on the parent
            bool valid = true;
            // get the list of all the dependency properties, we can use a level of caching to avoid to use reflection
            // more than one time for each object
            foreach (DependencyProperty dp in GetDPs(parent.GetType()))
            {
                if (BindingOperations.IsDataBound(parent, dp))
                {
                    System.Windows.Data.Binding binding = BindingOperations.GetBinding(parent, dp);
                    if (binding.ValidationRules.Count > 0)
                    {
                        BindingExpression expression = BindingOperations.GetBindingExpression(parent, dp);
                        switch (binding.Mode)
                        {
                            case BindingMode.OneTime:
                            case BindingMode.OneWay:
                                expression.UpdateTarget();
                                break;
                            default:
                                expression.UpdateSource();
                                break;
                        }
                        if (expression.HasError) valid = false;
                    }
                }
            }

            // Validate all the bindings on the children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (!IsValid(child)) { valid = false; }
            }

            return valid;
        }
    }

}

