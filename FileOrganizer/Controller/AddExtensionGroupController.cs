using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FileOrganizer.Data;
using FileOrganizer.Enums;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using FileOrganizer.ViewModels;
using Runtime.Extensions;
using Runtime.Messaging;
using Runtime.MVC;

namespace FileOrganizer.Controller
{
    public class AddExtensionGroupController : ControllerBase
    {
        public static readonly RoutedCommand AddExtensionCommand = new RoutedCommand("AddExtension", typeof(AddExtensionGroupController));
        public static readonly RoutedCommand SaveExtensionGroupCommand = new RoutedCommand("SaveExtensionGroup", typeof(AddExtensionGroupController));
        public static readonly RoutedCommand CloseViewCommand = new RoutedCommand("CloseView", typeof(AddExtensionGroupController));

        private AddExtensionGroupViewModel _viewModel;

        public override void Initialize()
        {
            base.Initialize();

            _viewModel = (AddExtensionGroupViewModel)Model;
        }

        protected void CommandAddExtension(object sender, ExecutedRoutedEventArgs e)
        {
            var extensions = _viewModel.SelectedExtension.Replace(" ", ";").Split(';');

            var dataModel = new FileOrganizerDataModel();

            foreach (var extensionForEach in extensions)
            {
                var extension = extensionForEach;

                //Extension muss mit . beginnen
                if (!extension.StartsWith("."))
                {
                    extension = "." + extension;
                }

                //Die Extension existiert in der Datenbank noch nicht
                if (dataModel.Extensions.FirstOrDefault(x => x.ExtensionName.ToLower().Contains(extension.ToLower())) != null)
                {
                    _viewModel.Extensions.AddSorted(extension);
                    Action saveExtensions = () =>
                    {
                        
                        dataModel.Extensions.Add(new Extension
                        {
                            ExtensionName = extension
                        });
                    };

                    saveExtensions.BeginInvoke(null, null);
                }

                dataModel.SaveChanges();
            }

            _viewModel.SelectedExtension = string.Empty;
            _viewModel.ReloadExtensionSelectionItems();
            _viewModel.ReloadExtensions();
        }

        protected void CheckCommandAddExtension(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandSaveExtensionGroup(object sender, ExecutedRoutedEventArgs e)
        {
            var extensionGroup = new ExtensionGroup();
            var selectedExtensions = _viewModel.SelectedExtensionsSave
                .Where(x => x.IsSelected);

            //extensionGroup.Extensions = new ObservableCollection<string>(selectedExtensions.Select(x=>x.Extension));
            extensionGroup.Name = _viewModel.Name;

            var dataModel = new FileOrganizerDataModel();

            _viewModel.ExtensionGroups.Add(extensionGroup);
            dataModel.ExtensionGroups.Add(extensionGroup);

            dataModel.SaveChanges();

            MessageBus.Default.BeginNotify(this, MessageToken.CloseView, new NotificationEventArgs());
        }

        protected void CheckCommandSaveExtensionGroup(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _viewModel.NameIsValid;
        }

        protected void CommandCloseView(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Default.BeginNotify(this, MessageToken.CloseView, new NotificationEventArgs());
        }

        protected void CheckCommandCloseView(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }

}
