using FileOrganizer.Enums;
using FileOrganizer.Models;
using FileOrganizer.ViewModels;
using Runtime.Messaging;
using Runtime.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Data;
using FileOrganizer.Helper;

namespace FileOrganizer.Controller
{
    public class AddExtensionSelection : ContentController<Controller.AddExtensionSelection, View.AddExtensionSelection, Model.AddExtensionSelection>
    {
        public static RoutedCommand SaveExtensions =>
            FileOrganizer.View.AddExtensionSelection.SaveExtensionsCommand;

        public static RoutedCommand CloseExtensionSelectionView =>
            FileOrganizer.View.AddExtensionSelection.CloseExtensionSelectionViewCommand;
        
        private Action _closeFn;

        private IEnumerable<string> _extensions;

        public AddExtensionSelection(BITS.UI.WPF.Core.Controllers.Controller parent, IEnumerable<string> extensions) : base(parent)
        {
            _extensions = extensions;
        }

        protected override Task OnSetupAsync()
        {
            this.View = new View.AddExtensionSelection();
            this.Model = new Model.AddExtensionSelection(_extensions);
            return base.OnSetupAsync();
        }

        public AddExtensionSelection Init(Action closeFn)
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
            var extensionsToSave = this.Model.SelectionItems
                .Where(x => x.IsSelected)
                .Select(x=>x.Extension)
                .OrderBy(x=>x)
                .ToList();

            if (extensionsToSave.Any())
            {
                var context = ContextManager.Context();
                context.Extensions.AddRange(extensionsToSave.Select(x => new Extension
                {
                    ExtensionName = x
                }));
                context.SaveChanges();
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
