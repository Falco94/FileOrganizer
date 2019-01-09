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
using BITS.UI.WPF.Core;
using FileOrganizer.Controller.Helper;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using FileOrganizer.Model;
using Runtime.Extensions;

namespace FileOrganizer.Controller
{
    public class ExtensionGroupsExtensionAssignement : ContentController<Controller.ExtensionGroupsExtensionAssignement, View.ExtensionGroupsExtensionAssignement, Model.ExtensionGroupsExtensionAssignement>
    {
        public static RoutedCommand CancelView => FileOrganizer.View.ExtensionGroupsExtensionAssignement.CancelViewCommand;
        public static RoutedCommand SaveGroup => FileOrganizer.View.ExtensionGroupsExtensionAssignement.SaveGroupCommand;

        private ExtensionGroup _extensionGroup;
        private IEnumerable<Extension> _extensions;
        private Controller.ExtensionGroups _parent;
        private IEnumerable<ExtensionGroup> _currentGroups;

        private bool isOrderedAscending = false;

        public ExtensionGroupsExtensionAssignement(Controller.ExtensionGroups parent,IEnumerable<ExtensionGroup> currentGroups, ExtensionGroup extensionGroup, IEnumerable<Models.Extension> extensions) : base(parent)
        {
            _extensionGroup = extensionGroup;
            _extensions = extensions;
            _parent = parent;
            _currentGroups = currentGroups;
        }

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.ExtensionGroupsExtensionAssignement();
            this.Model = new Model.ExtensionGroupsExtensionAssignement(_extensionGroup, _extensions);

            //ListView Header Functions
            this.View.ExtensionsNameHeader.MouseLeftButtonDown += OrderGroupsOnClick;
            this.View.OrderGroupsAscending.Click += async (s, e) =>
            {
                this.Model.IsBusy = true;
                await testAsync();
            };

            this.View.OrderGroupsDescending.Click += OrderGroupsDescendingOnClick;
            this.View.SelectAllCheckbox.Click += SelectAllCheckboxOnClick;


            this.BindAsync(CancelView, CancelViewFn, CanCancelView);
            this.BindAsync(SaveGroup, SaveGroupFn, CanSaveGroup);
        }

        private async Task testAsync()
        {
            var listToOrder = new List<GroupExtensionItem>();
            listToOrder = this.Model.Extensions.ToList();

            await Task.Run(() =>
            {
                listToOrder = listToOrder.OrderBy(x => x.ExtensionName).ToList();
                isOrderedAscending = true;
                this.Model.Extensions = listToOrder.ToAsyncObservableCollection();
            });
            
        }

        private void SelectAllCheckboxOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (this.View.SelectAllCheckbox.IsChecked == null)
                return;

            foreach (var groupExtensionItem in this.Model.Extensions)
            {
                groupExtensionItem.IsSelected = this.View.SelectAllCheckbox.IsChecked.Value;
            }
        }

        private async void OrderGroupsOnClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            this.Model.IsBusy = true;
            await OrderGroupsOnClickAsync();
            this.Model.IsBusy = false;
        }

        private async Task OrderGroupsOnClickAsync()
        {
            if (isOrderedAscending)
            {
                this.Model.Extensions = this.Model.Extensions.OrderByDescending(x => x.ExtensionName)
                    .ToAsyncObservableCollection();
                isOrderedAscending = false;
            }
            else
            {
                this.Model.Extensions = this.Model.Extensions.OrderBy(x => x.ExtensionName).ToAsyncObservableCollection();
                isOrderedAscending = true;
            }
        }
        
        private async Task OrderGroupsAscendingOnClickAsync()
        {
            this.Model.Extensions = this.Model.Extensions.OrderBy(x => x.ExtensionName).ToAsyncObservableCollection();
            isOrderedAscending = true;
        }


        private void OrderGroupsDescendingOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Model.Extensions = this.Model.Extensions.OrderByDescending(x => x.ExtensionName).ToAsyncObservableCollection();
            isOrderedAscending = false;

            this.Model.IsBusy = false;
        }

        private async Task<bool> CanSaveGroup()
        {
            return true;
        }

        private async Task SaveGroupFn()
        {
            //Hinzufügen der Selektierten Extensions
            var extensionKeys = this.Model.Extensions.Where(x => x.IsSelected).Select(x => x.ExtensionId);

            var context = ContextManager.Context();

            foreach (var extensionId in extensionKeys)
            {
                var existingExtension = _extensionGroup.Extensions.SingleOrDefault(x => x.ExtensionId == extensionId);

                //extension already exists
                if(existingExtension != null)
                    continue;

                var extension = context.Extensions.SingleOrDefault(x => x.ExtensionId == extensionId);

                extension.CurrentExtensionGroupId = _extensionGroup.ExtensionGroupId;
                _extensionGroup.Extensions.Add(extension);
            }

            //remove deleted extensions
            var deletedExtensions = _extensionGroup.Extensions
                .Where(x => !extensionKeys.Contains(x.ExtensionId))
                .ToList();

            foreach (var deletedExtension in deletedExtensions)
            {
                deletedExtension.CurrentExtensionGroupId = null;
                deletedExtension.ExtensionGroup = null;
                _extensionGroup.Extensions.Remove(deletedExtension);
            }

            //List<ExtensionGroup> groups = null;

            //using (var model = new FODataModel())
            //{
            //    groups = model.ExtensionGroups.Where(x => x.ExtensionGroupId != _extensionGroup.ExtensionGroupId).ToList();
            //    groups.Add(this.Model.Group);
            //}
            
            await this.SwitchByAsync(region => _parent.View.MainContent, new Controller.ExtensionGroups(_parent, _currentGroups).Init(),
                (region, view) =>
                {
                    region.Children.Clear();
                    region.Children.Add(view); }
                , (region, view) => region.Children.Clear());

        }

        private async Task<bool> CanCancelView()
        {
            return true;
        }

        //Alle Gruppen übergeben, aktuellen Bearbeitungsfortschritt ignorieren
        private async Task CancelViewFn()
        {
            List<ExtensionGroup> groups = null;

            var context = ContextManager.Context();

            groups = context.ExtensionGroups.ToList();

            await this.SwitchByAsync(region => _parent.View.MainContent, new Controller.ExtensionGroups(_parent, groups).Init(),
                (region, view) =>
                {
                    region.Children.Clear();
                    region.Children.Add(view);
                }
                , (region, view) => region.Children.Clear());
        }

        private void RunAsync(Task asyncFn, Action continueFn)
        {
            this.Model.IsBusy = true;
            Task.Run(async () =>
            {
                await asyncFn;
            }).ContinueWith(async antecedent =>
            {
                this.Model.IsBusy = false;
                continueFn();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }



        public class Busy : IBusy
        {
            public bool IsBusy { get; set; }
        }
    }

}
