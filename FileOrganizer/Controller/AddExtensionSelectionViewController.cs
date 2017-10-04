using FileOrganizer.Enums;
using FileOrganizer.Models;
using FileOrganizer.ViewModels;
using Runtime.Messaging;
using Runtime.MVC;
using System;
using System.Linq;
using System.Windows.Input;
using FileOrganizer.Data;

namespace FileOrganizer.Controller
{
    public class AddExtensionSelectionViewController : ControllerBase
    {
        public static readonly RoutedCommand SaveExtensionsCommand = new RoutedCommand("SaveExtensions", typeof(AddExtensionSelectionViewController));
        public static readonly RoutedCommand CloseExtensionSelectionViewCommand = new RoutedCommand("CloseExtensionSelectionView", typeof(AddExtensionSelectionViewController));
        
        AddExtensionSelectionViewModel _addExtensionSelectionViewModel;
        private Action _closeFn;

        public override void Initialize()
        {
            base.Initialize();
            _addExtensionSelectionViewModel = (AddExtensionSelectionViewModel)Model;
        }

        public AddExtensionSelectionViewController Init(Action closeFn)
        {
            _closeFn = closeFn;
            return this;
        }

        protected void CheckCommandSaveExtensions(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandSaveExtensions(object sender, ExecutedRoutedEventArgs e)
        {
            var extensionsToSave = _addExtensionSelectionViewModel.SelectionItems
                .Where(x => x.IsSelected)
                .Select(x=>x.Extension)
                .OrderBy(x=>x)
                .ToList();

            if (extensionsToSave.Any())
            {
                var dataModel = new FODataModel();
                dataModel.Extensions.AddRange(extensionsToSave.Select(x => new Extension
                {
                    ExtensionName = x
                }));
                dataModel.SaveChanges();
            }

            _closeFn?.Invoke();
        }

        protected void CheckCommandCloseExtensionSelectionView(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void CommandCloseExtensionSelectionView(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.Default.Notify(this, MessageToken.CloseExtensionSelectionView, new NotificationEventArgs());
        }
    }
}
