using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Controller.Helper;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using FileOrganizer.Service;

namespace FileOrganizer.Controller
{
    internal interface IDialogAccessor
    {
        DialogController DialogController { get; }
    }

    public class ExtensionGroups : ContentController<Controller.ExtensionGroups, View.ExtensionGroups, Model.ExtensionGroups>, Helper.IDialogAccessor
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

        public DialogController DialogController { get; set; }

        public ExtensionGroups(BITS.UI.WPF.Core.Controllers.Controller parent,
            IEnumerable<ExtensionGroup> extensionGroups, IEnumerable<Extension> extensions) : base(parent)
        {
            this.DialogController = DialogManager.DialogAccessor;
            _extensionGroups = extensionGroups;
            _extensions = extensions;
        }

        //Init without Extensions
        public ExtensionGroups(BITS.UI.WPF.Core.Controllers.Controller parent,
            IEnumerable<ExtensionGroup> extensionGroups) : base(parent)
        {
            _extensionGroups = extensionGroups;

            var context = ContextManager.Context();
            
            _extensions = context.Extensions.ToList();
        }
        

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.ExtensionGroups();
            this.Model = new Model.ExtensionGroups(_extensionGroups, _extensions);

            this.BindAsync(SaveGroups, SaveExtensionGroups, CanSaveExtensionGroups);
            this.BindAsync(AddNewAssignement, AddNewAssignementFn, CanAddNewAssignement);
            this.BindAsync<ExtensionGroup>(ChooseExtensions, ChooseExtensionsFn, CanChooseExtensions);
            this.BindAsync<ExtensionGroup>(DeleteAssignement, DeleteAssignementFn, CanDeleteAssignement);

            //führt die Can... Methoden aus
            CommandManager.InvalidateRequerySuggested();

        }

        private async Task<bool> CanDeleteAssignement(ExtensionGroup arg)
        {
            return true;
        }

        private async Task DeleteAssignementFn(ExtensionGroup arg)
        {
            var context = ContextManager.Context();

            this.Model.LoadedExtensionGroups.Remove(arg);

            if (arg.ExtensionGroupId != 0)
            {
                context.Entry(arg).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private async Task<bool> CanChooseExtensions(ExtensionGroup arg)
        {
            return true;
        }

        private async Task ChooseExtensionsFn(ExtensionGroup arg)
        {
            //this.Model.IsBusy = true;
            //this.View.Test.Visibility = Visibility.Hidden;
            var canAdd = arg.ExtensionGroupId != 0;

            if (!canAdd)
            {
                //TODO: Message, dass erst gespeichert werden muss
                //await DialogExtension.ErrorDialogAsync(this, "Neu angelegte Gruppen müssen erst gespeichert werden!");
                return;
            }

            IEnumerable<Extension> extensions = null;

            await Task.Run(() =>
            {
                var context = ContextManager.Context();
                    extensions = context.Extensions.ToList();

                    var idList = arg.Extensions.Select(y => y.ExtensionId);

                    //exclude current group??
                    var allUsedExtensions = _extensionGroups.SelectMany(x => x.Extensions.Select(y => y.ExtensionId));
                    

                    //in this group or in no other
                    extensions = extensions.Where(x =>
                        idList.Contains(x.ExtensionId) ||
                        !allUsedExtensions.Contains(x.ExtensionId));

            }).ContinueWith(async antecedent =>
            {
                //this.Model.IsBusy = false;

                await this.SwitchByAsync(region => region.MainContent,
                    new Controller.ExtensionGroupsExtensionAssignement(this, this.Model.LoadedExtensionGroups, arg, extensions).Init(),
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
            var canSave = IsValid(this.View);
            
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

            var context = ContextManager.Context();

            foreach (var extensionGroup in this.Model.LoadedExtensionGroups)
            {
                if (extensionGroup.ExtensionGroupId == 0)
                {
                    context.Entry(extensionGroup).State = EntityState.Added;
                }
                //else
                //{
                //    context.Entry(extensionGroup).State = EntityState.Modified;
                //}

                for (int i = 0; i < extensionGroup.Extensions.Count; i++)
                {
                    var extension = extensionGroup.Extensions[i];
                    
                    try
                    {
                        var test = context.Entry(extension);
                        //test.State = EntityState.Modified;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            context.SaveChanges();


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

//foreach (var extensionGroup in this.Model.LoadedExtensionGroups)
                //{
                //    var changedItem = context.ExtensionGroups.Include(x => x.Extensions).ToList()
                //        .SingleOrDefault(y => y.ExtensionGroupId == extensionGroup.ExtensionGroupId);


                //    //changedItem = propertyMapper.Map(extensionGroup, changedItem);
                //    //    context.Entry(extensionGroup).State = EntityState.Modified;

                //    changedItem.Name = extensionGroup.Name;
                //    context.Entry(changedItem).State = EntityState.Modified;

                //    foreach (var groupExtension in extensionGroup.Extensions)
                //    {
                //        var currentExtension =
                //            changedItem.Extensions
                //                .SingleOrDefault(x => x.ExtensionId == groupExtension.ExtensionId);

                //        if (currentExtension == null)
                //        {
                //            currentExtension = context.Extensions
                //                .SingleOrDefault(x => x.ExtensionId == groupExtension.ExtensionId);

                //            currentExtension.CurrentExtensionGroupId = extensionGroup.ExtensionGroupId;
                //            changedItem.Extensions.Add(currentExtension);

                //            context.Entry(currentExtension).State = EntityState.Modified;
                //        }
                //    }

                //    //falls gelöscht
                //    if (changedItem.Extensions.Count() > extensionGroup.Extensions.Count())
                //    {
                //        //alle extensions, die nicht mehr in der Gruppe sind
                //        var deletedExtensions = changedItem.Extensions
                //            .Where(x => !extensionGroup.Extensions
                //                .Select(y => y.ExtensionId)
                //                .Contains(x.ExtensionId));

                //        foreach (var deletedExtension in deletedExtensions)
                //        {
                //            deletedExtension.ExtensionGroup = null;
                //            deletedExtension.CurrentExtensionGroupId = null;
                //        }

                //        // Extra loop, da sonst Enumerationsfehler wenn:
                //        // context.Entry(deletedExtension).State = EntityState.Modified;
                //        // mit oben drin steht
                //        foreach (var deletedExtensionId in deletedExtensions.Select(x=>x.ExtensionId))
                //        {
                //            var setModified =
                //                changedItem.Extensions.SingleOrDefault(x => x.ExtensionId == deletedExtensionId);

                //            if(setModified == null)
                //                continue;

                //            context.Entry(setModified).State = EntityState.Modified;
                //        }
                //    }
                    
                //    context.SaveChanges();
                //}
