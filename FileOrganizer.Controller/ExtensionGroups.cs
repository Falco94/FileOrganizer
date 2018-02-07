using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Data;
using FileOrganizer.Models;
using FileOrganizer.Service;
using RefactorThis.GraphDiff;

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

            this.BindAsync(SaveGroups, SaveExtensionGroups, CanSaveExtensionGroups);
            this.BindAsync(AddNewAssignement, AddNewAssignementFn, CanAddNewAssignement);
            this.BindAsync<ExtensionGroup>(ChooseExtensions, ChooseExtensionsFn, CanChooseExtensions);
            
        }

        private async Task<bool> CanChooseExtensions(ExtensionGroup group)
        {
            return true;
        }

        private async Task ChooseExtensionsFn(ExtensionGroup group)
        {
            //this.Model.IsBusy = true;
            //this.View.Test.Visibility = Visibility.Hidden;

            IEnumerable<Extension> extensions = null;

            await Task.Run(() =>
            {
                using (var dataModel = new FODataModel())
                {
                    extensions = dataModel.Extensions.ToList();

                    var idList = group.Extensions.Select(y => y.ExtensionId);

                    //exclude current group??
                    var allUsedExtensions = _extensionGroups.SelectMany(x => x.Extensions.Select(y => y.ExtensionId));
                    

                    //in this group or in no other
                    extensions = extensions.Where(x =>
                        idList.Contains(x.ExtensionId) ||
                        !allUsedExtensions.Contains(x.ExtensionId));
                }

            }).ContinueWith(async antecedent =>
            {
                //this.Model.IsBusy = false;

                await this.SwitchByAsync(region => region.MainContent,
                    new Controller.ExtensionGroupsExtensionAssignement(this, group, extensions).Init(),
                    (region, view) =>
                    {
                        region.Children.Clear();
                        region.Children.Add(view);
                    }, 
                    (region, view) => region.Children.Clear());
            }, TaskScheduler.FromCurrentSynchronizationContext());
            
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

        /// <summary>
        /// //see http://www.entityframeworktutorial.net/entityframework6/save-entity-graph.aspx
        /// </summary>
        /// <returns></returns>
        private async Task SaveExtensionGroups()
        {
            this.Model.IsBusy = true;

            var propertyMapper = new PropertyMapper();

            using (var context = new FODataModel())
            {
                foreach (var extensionGroup in this.Model.LoadedExtensionGroups)
                {
                    //    var changedItem = context.ExtensionGroups.Include(x => x.Extensions).ToList()
                    //        .SingleOrDefault(y => y.ExtensionGroupId == extensionGroup.ExtensionGroupId);

                    //    if (changedItem == null)
                    //    {
                    //        changedItem = extensionGroup;
                    //        context.Entry(changedItem).State = EntityState.Added;
                    //    }
                    //    else
                    //    {
                    //        changedItem = propertyMapper.Map(extensionGroup, changedItem);

                    //        context.Entry(changedItem).State = EntityState.Modified;
                    //    }

                    //    context.SaveChanges();

                    context.UpdateGraph(extensionGroup, map => map.OwnedCollection(x => x.Extensions));

                }

                context.SaveChanges();
            }

            this.Model.IsBusy = false;
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

        public void UpdateCollection<TCollection, TKey>(
            DbContext context, IList<TCollection> databaseCollection,
            IList<TCollection> detachedCollection,
            Func<TCollection, TKey> keySelector) where TCollection : class where TKey : IEquatable<TKey>
        {
            var databaseCollectionClone = databaseCollection.ToArray();
            foreach (var databaseItem in databaseCollectionClone)
            {
                var detachedItem = detachedCollection.SingleOrDefault(item => keySelector(item).Equals(keySelector(databaseItem)));
                if (detachedItem != null)
                {
                    context.Entry(databaseItem).CurrentValues.SetValues(detachedItem);
                }
                else
                {
                    context.Set<TCollection>().Remove(databaseItem);
                }
            }

            foreach (var detachedItem in detachedCollection)
            {
                if (databaseCollectionClone.All(item => keySelector(item).Equals(keySelector(detachedItem)) == false))
                {
                    databaseCollection.Add(detachedItem);
                }
            }
        }
    }
}
