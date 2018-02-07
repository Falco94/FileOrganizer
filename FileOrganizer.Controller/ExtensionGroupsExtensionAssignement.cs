using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using FileOrganizer.Data;
using Runtime.Extensions;

namespace FileOrganizer.Controller
{
    public class ExtensionGroupsExtensionAssignement : ContentController<Controller.ExtensionGroupsExtensionAssignement, View.ExtensionGroupsExtensionAssignement, Model.ExtensionGroupsExtensionAssignement>
    {
        public static RoutedCommand CloseView => FileOrganizer.View.ExtensionGroupsExtensionAssignement.CloseViewCommand;
        public static RoutedCommand SaveGroup => FileOrganizer.View.ExtensionGroupsExtensionAssignement.SaveGroupCommand;

        private ExtensionGroup _extensionGroup;
        private IEnumerable<Extension> _extensions;
        private Controller.ExtensionGroups _parent;

        public ExtensionGroupsExtensionAssignement(Controller.ExtensionGroups parent, ExtensionGroup extensionGroup, IEnumerable<Models.Extension> extensions) : base(parent)
        {
            _extensionGroup = extensionGroup;
            _extensions = extensions;
            _parent = parent;
        }

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.ExtensionGroupsExtensionAssignement();
            this.Model = new Model.ExtensionGroupsExtensionAssignement(_extensionGroup, _extensions);

            this.Bind(CloseView, CloseViewFn, CanCloseView);
            this.Bind(SaveGroup, SaveGroupFn, CanSaveGroup);
        }

        private async Task<bool> CanSaveGroup()
        {
            return true;
        }

        private async Task SaveGroupFn()
        {
            //Hinzufügen der Selektierten Extensions
            _extensionGroup.Extensions = this.Model.Extensions.Where(x => x.IsSelected).Select(x => new Extension()
            {
                ExtensionName = x.ExtensionName,
                Id = x.Id
            }).ToObservableCollection();

            List<ExtensionGroup> groups = null;

            using (var model = new FODataModel())
            {
                groups = model.ExtensionGroups.Where(x => x.Name != _extensionGroup.Name).ToList();
                groups.Add(this.Model.Group);
            }

            await this.SwitchByAsync(region => _parent.View.MainContent, new Controller.ExtensionGroups(_parent, groups).Init());
            
        }

        private async Task<bool> CanCloseView()
        {
            return true;
        }

        private async Task CloseViewFn()
        {
            _parent.View.Test.Visibility = Visibility.Visible;
        }

        public class Busy : IBusy
        {
            public bool IsBusy { get; set; }
        }
    }

}
