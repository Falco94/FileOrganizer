using FileOrganizer.Enums;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using FileOrganizer.Test;
using FileOrganizer.ViewModels;
using Runtime.Extensions;
using Runtime.MVC;
using Runtime.Services.DefaultServices;
using Runtime.Services.Plumbing;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using FileOrganizer.Data;
using FileOrganizer.Views;

namespace FileOrganizer.Controller
{
    public class AddNewExtensionsViewController : ControllerBase
    {
        public static readonly RoutedCommand SaveExtensionsCommand = new RoutedCommand("SaveExtensions", typeof(AddNewExtensionsViewController));
        public static readonly RoutedCommand AddExtensionCommand = new RoutedCommand("AddExtension", typeof(AddNewExtensionsViewController));
        public static readonly RoutedCommand RemoveExtensionCommand = new RoutedCommand("RemoveExtension", typeof(AddNewExtensionsViewController));
        public static readonly RoutedCommand AddExtensionGroupCommand = new RoutedCommand("AddExtensionGroup", typeof(AddNewExtensionsViewController));
        public static readonly RoutedCommand EditExtensionGroupCommand = new RoutedCommand("EditExtensionGroup", typeof(AddNewExtensionsViewController));
        
        private AddNewExtensionsViewModel _addNewExtensionsViewModel;

        public override void Initialize()
        {
            base.Initialize();

            _addNewExtensionsViewModel = (AddNewExtensionsViewModel)Model;
        }

        protected void CheckCommandSaveExtensions(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandSaveExtensions(object sender, ExecutedRoutedEventArgs e)
        {
            using (var dataModel = new FileOrganizerDataModel())
            {
                foreach (var extension in _addNewExtensionsViewModel.Extensions)
                {
                    var exists = dataModel.Extensions.FirstOrDefault(x => x.ExtensionName == extension) != null;

                    if (!exists)
                    {
                        dataModel.Extensions.Add(new Extension
                        {
                            ExtensionName = extension
                        });
                    }
                }
            }
        }

        protected void CheckCommandAddExtension(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandAddExtension(object sender, ExecutedRoutedEventArgs e)
        {
            var extensions = _addNewExtensionsViewModel.SelectedExtension.Replace(" ", ";").Split(';');

            foreach (var extensionForEach in extensions)
            {
                var extension = extensionForEach;

                //Extension muss mit . beginnen
                if (!extension.StartsWith("."))
                {
                    extension = "." + extension;
                }

                if (!_addNewExtensionsViewModel.Extensions.Select(x => x.ToLower()).Contains(extension.ToLower()))
                {
                    _addNewExtensionsViewModel.Extensions.AddSorted(extension);

                    using (var dataModel = new FileOrganizerDataModel())
                    {
                        var exists = dataModel.Extensions.FirstOrDefault(x => x.ExtensionName == extension) != null;

                        if (!exists)
                        {
                            dataModel.Extensions.Add(new Extension
                            {
                                ExtensionName = extension
                            });
                        }

                        dataModel.SaveChanges();
                    }
                }
            }

            _addNewExtensionsViewModel.SelectedExtension = string.Empty;
        }

        protected void CheckCommandRemoveExtension(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandRemoveExtension(object sender, ExecutedRoutedEventArgs e)
        {
            var value = (string)e.Parameter;

            var dataModel = new FileOrganizerDataModel();

            if(_addNewExtensionsViewModel.Extensions.Contains(value))
            {
                _addNewExtensionsViewModel.Extensions.Remove(value);

                Action saveExtensions = () =>
                {
                    var extentionToDelete =
                        dataModel.Extensions.FirstOrDefault(x => x.ExtensionName.ToLower() == value.ToLower());

                    dataModel.Extensions.Remove(extentionToDelete);
                    dataModel.SaveChanges();
                };

                saveExtensions.BeginInvoke(null, null);
            }
        }

        protected void CheckCommandAddExtensionGroup(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandAddExtensionGroup(object sender, ExecutedRoutedEventArgs e)
        {
            var addExtensionGroupView = new AddExtensionGroupView();
            addExtensionGroupView.ShowDialog();

            
            var dataModel = new FileOrganizerDataModel();
            _addNewExtensionsViewModel.ExtensionGroups = dataModel.ExtensionGroups
                .ToList()
                .ToObservableCollection();
        }

        protected void CheckCommandEditExtensionGroup(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandEditExtensionGroup(object sender, ExecutedRoutedEventArgs e)
        {
            var group = e.Parameter as ExtensionGroup;

            if (group != null)
            {
                var addExtensionGroupView = new AddExtensionGroupView(group);
                addExtensionGroupView.ShowDialog();

                var dataModel = new FileOrganizerDataModel();
                _addNewExtensionsViewModel.ExtensionGroups = new ObservableCollection<ExtensionGroup>(dataModel.ExtensionGroups.ToList());
            }
        }
    }
}
