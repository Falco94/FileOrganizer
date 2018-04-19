using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using BITS.UI.WPF.Core;
using FileOrganizer;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Controller.Helper;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using MahApps.Metro.Controls.Dialogs;
using Runtime.Extensions;

namespace FileOrganizer.Controller
{
    public class ExtensionMapping :
        ContentController<Controller.ExtensionMapping, View.ExtensionMapping, Model.ExtensionMapping>
    {
        private List<ExtensionMappingItem> _extensionMappings;
        private List<Extension> _extensions;
        private List<ExtensionGroup> _extensionGroups;

        public static RoutedCommand AddNewAssignement
            => FileOrganizer.View.ExtensionMapping.AddNewAssignementCommand;

        public static RoutedCommand DeleteAssignement
            => FileOrganizer.View.ExtensionMapping.DeleteAssignementCommand;

        public static RoutedCommand ChooseFolder
            => FileOrganizer.View.ExtensionMapping.ChooseFolderCommand;

        public static RoutedCommand SaveAssignements
            => FileOrganizer.View.ExtensionMapping.SaveAssignementsCommand;

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            Action action = null;

            action = () =>
            {
                this.View = new View.ExtensionMapping();
                this.Model = new Model.ExtensionMapping().Init(_extensionMappings, _extensions, _extensionGroups);
                
                SetupCommandBindings();
            };

            SafeExecutor.ExecuteFn(action);
        }

        //TODO: Combobox Sources Filtern
        private void FilterExtensions(IEnumerable<ExtensionMappingItem> mappingItems, IList<Models.Extension> extensions, IList<Models.ExtensionGroup> extensionGroups)
        {
            //Filtert bereits gemappte Items aus den Listen
            foreach (var mapping in mappingItems)
            {
                var extension = extensions.FirstOrDefault(x => x.ExtensionId == mapping.Extension.ExtensionId);
                if (extension != null)
                {
                    extensions.Remove(extension);
                }

                var extensionGroup =
                    extensionGroups.FirstOrDefault(x => x.Extensions.Contains(y => y.ExtensionId == mapping.Extension.ExtensionId));

                if (extensionGroup != null)
                {
                    extensionGroups.Remove(extensionGroup);
                }
            }
        }

        private void SetupCommandBindings()
        {
            this.BindAsync(AddNewAssignement, AddNewMapping, CanAddNewMapping);
            this.BindAsync(SaveAssignements, SaveMappings, CanSaveMappings);
            this.BindAsync<ExtensionMappingItem>(ChooseFolder, ChooseFolderDialog, CanChooseFolderDialog);
            this.BindAsync<ExtensionMappingItem>(DeleteAssignement, DeleteMapping, CanDeleteMapping);
        }

        private async Task<bool> CanAddNewMapping()
        {
            return this.Model.MappingItems?.FirstOrDefault(
                x => (String.IsNullOrWhiteSpace(x.Extension?.ExtensionName) && String.IsNullOrWhiteSpace(x.ExtensionGroup?.Name))
                     || String.IsNullOrWhiteSpace(x.TargetPath)) == null;
        }

        private async Task AddNewMapping()
        {
            this.Model.MappingItems.Add(new ExtensionMappingItem());
        }

        private async Task<bool> CanSaveMappings()
        {
            //TODO: sinnvolles Mapping (keine doppelten gruppen, etc.)

            if (!this.Model.MappingItems.Any())
                return false;

            var item =
                this.Model.MappingItems.FirstOrDefault(
                    x => (x.Extension == null && x.ExtensionGroup == null) || String.IsNullOrWhiteSpace(x.TargetPath));

            return item == null && IsValid(this.View);
        }

        private async Task SaveMappings()
        {
            var context = ContextManager.Context();

            var mappingItems = context.ExtensionMappingItems.ToList();

            foreach (var mappingItem in this.Model.MappingItems)
            {
                if (mappingItem.ExtensionMappingItemId == 0)
                    context.Entry(mappingItem).State = EntityState.Added;
            }

            int changes = 0;

            SafeExecutor.ExecuteFn(() =>
            {
                changes = context.SaveChanges();
            }, "ExtensionMapping.SaveMappings");


            if (changes > 0)
            {
                await DialogHandler.DialogRoot.ShowMessageAsync("Success", $"Saved {changes} entries",
                    MessageDialogStyle.Affirmative);
            }

            ExtensionMappingManager.ReInit();
        }

        private async Task ChooseFolderDialog(ExtensionMappingItem item)
        {
            if (item != null)
            {
                var folderBrowserDialog = new FolderBrowserDialog();

                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.Description = "Wählen Sie einen Zielordner";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    item.TargetPath = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private async Task<bool> CanChooseFolderDialog(ExtensionMappingItem item)
        {
            return true;
        }

        private async Task DeleteMapping(ExtensionMappingItem arg)
        {
            var context = ContextManager.Context();

            this.Model.MappingItems.Remove(arg);

            if (arg.ExtensionMappingItemId != 0)
            {
                context.Entry(arg).State = EntityState.Deleted;
                context.SaveChanges();
            }

            ExtensionMappingManager.ReInit();
        }

        private async Task<bool> CanDeleteMapping(ExtensionMappingItem item)
        {
            return true;
        }

        private bool IsValid(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all
            // of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&
            LogicalTreeHelper.GetChildren(obj)
            .OfType<DependencyObject>()
            .All(IsValid);
        }

        public ExtensionMapping(BITS.UI.WPF.Core.Controllers.Controller parent,
            IEnumerable<ExtensionMappingItem> extensionMappings, IEnumerable<Extension> extensions,
            IEnumerable<ExtensionGroup> extensionGroups) : base(parent)
        {
            _extensionMappings = new List<ExtensionMappingItem>(extensionMappings);
            _extensions = new List<Extension>(extensions);
            _extensionGroups = new List<ExtensionGroup>(extensionGroups);
        }
    }
}