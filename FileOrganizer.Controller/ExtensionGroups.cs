using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BITS.UI.WPF.Core.Controllers;
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

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.ExtensionGroups();
            this.Model = new Model.ExtensionGroups(_extensionGroups, _extensions);

            this.Bind(SaveGroups, SaveExtensionGroups, CanSaveExtensionGroups);
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
