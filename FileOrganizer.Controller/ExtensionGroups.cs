using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Data;
using FileOrganizer.Models;

namespace FileOrganizer.Controller
{
    public class ExtensionGroups : ContentController<Controller.ExtensionGroups, View.ExtensionGroups, Model.ExtensionGroups>
    {
        private readonly IEnumerable<ExtensionGroup> _extensionGroups;
        private readonly IEnumerable<Extension> _extensions;

        public static RoutedCommand AddNewAssignement
            => FileOrganizer.View.ExtensionGroups.AddNewAssignementCommand;

        public static RoutedCommand DeleteAssignement
            => FileOrganizer.View.ExtensionGroups.DeleteAssignementCommand;

        public static RoutedCommand ChooseExtensions
            => FileOrganizer.View.ExtensionGroups.ChooseExtensionsCommand;

        public static RoutedCommand SaveGroups
            => FileOrganizer.View.ExtensionGroups.SaveGroupsCommand;


        public ExtensionGroups(BITS.UI.WPF.Core.Controllers.Controller parent,
            IEnumerable<ExtensionGroup> extensionGroups, IEnumerable<Extension> extensions) : base(parent)
        {
            _extensionGroups = extensionGroups;
            _extensions = extensions;
        }

        //Init without Extensions
        public ExtensionGroups(BITS.UI.WPF.Core.Controllers.Controller parent,
            IEnumerable<ExtensionGroup> extensionGroups) : base(parent)
        {
            parent.Exit();

            _extensionGroups = extensionGroups;

            using (var dataModel = new FODataModel())
            {
                _extensions = dataModel.Extensions.ToList();
            }
        }
        

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.ExtensionGroups();
            this.Model = new Model.ExtensionGroups(_extensionGroups, _extensions);

            this.Bind(SaveGroups, SaveExtensionGroups, CanSaveExtensionGroups);
            this.Bind(AddNewAssignement, AddNewAssignementFn, CanAddNewAssignement);
            this.Bind<ExtensionGroup>(ChooseExtensions, ChooseExtensionsFn, CanChooseExtensions);
            
        }

        private async Task<bool> CanChooseExtensions(ExtensionGroup group)
        {
            return true;
        }

        private async Task ChooseExtensionsFn(ExtensionGroup group)
        {
            this.Model.IsBusy = true;
            this.View.Test.Visibility = Visibility.Hidden;

            IEnumerable<Extension> extensions = null;

            await Task.Run(() =>
            {
                using (var dataModel = new FODataModel())
                {
                    extensions = dataModel.Extensions.ToList();

                    var idList = group.Extensions.Select(y => y.Id);

                    //exclude current group??
                    var allUsedExtensions = _extensionGroups.SelectMany(x => x.Extensions.Select(y => y.Id));
                    

                    //in this group or in no other
                    extensions = extensions.Where(x =>
                        idList.Contains(x.Id) ||
                        !allUsedExtensions.Contains(x.Id));
                }

            }).ContinueWith(async antecedent =>
            {
                this.Model.IsBusy = false;

                await this.SwitchByAsync(region => region.MainContent,
                    new Controller.ExtensionGroupsExtensionAssignement(this, group, extensions).Init());
            }, TaskScheduler.FromCurrentSynchronizationContext());

            this.Model.IsBusy = false;
        }

        private async Task<bool> CanAddNewAssignement()
        {
            var canAdd = IsValid(this.View);

            return canAdd;
        }

        private async Task AddNewAssignementFn()
        {

            this.Model.LoadedExtensionGroups.Add(new ExtensionGroup());
        }

        private async Task<bool> CanSaveExtensionGroups()
        {
            
            //Alle Name sind unique
            var canSave = IsValid(this.View);
            //this.Model.LoadedExtensionGroups?.Select(x => x.Name).Distinct().Count() ==
            //this.Model.LoadedExtensionGroups?.Count;

            return canSave;
        }

        private async Task SaveExtensionGroups()
        {

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
    }
}
