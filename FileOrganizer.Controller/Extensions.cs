using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using BITS.UI.WPF.Core;
using FileOrganizer;
using BITS.UI.WPF.Core.Controllers;
using Core.UI;
using Core.UI.View;
using FileOrganizer.Controller.Helper;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using MahApps.Metro.Controls.Dialogs;

namespace FileOrganizer.Controller
{
    public class Extensions : ContentController<Controller.Extensions, View.Extensions, Model.Extensions>, IDialogAccessor
    {
        private IEnumerable<Extension> _extensions;
        private IDialogCoordinator _dialogCoordinator;

        public static RoutedCommand DeleteExtension
            => FileOrganizer.View.Extensions.DeleteExtensionCommand;

        public Extensions(BITS.UI.WPF.Core.Controllers.Controller parent, IDialogCoordinator dialogCoordinator, IEnumerable<Models.Extension> extensions) 
            : base(parent)
        {
            _dialogCoordinator = dialogCoordinator;
            _extensions = extensions;
        }

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            //try
            //{
            //    Task.WaitAll(Task.Delay(2000));
            //    int a = 7;
            //}
            //catch (Exception ex)
            //{
            //}
            this.View = new View.Extensions();
            this.Model = new Model.Extensions();
            this.Model.Init(_extensions);

            this.BindAsync<Models.Extension>(DeleteExtension, DeleteExtensionFn, CanDeleteExtension);
        }

        private async Task<bool> CanDeleteExtension(Extension e)
        {
            return true;
        }

        private async Task DeleteExtensionFn(Extension e)
        {
            SafeExecutor.ExecuteFn(() => DeleteExtensionExecute(e), "Extensions.DeleteExtensionFn");
        }

        private void DeleteExtensionExecute(Extension e)
        {
            var context = ContextManager.Context();

            this.Model.LoadedExtensions.Remove(e);

            if (e.ExtensionId != 0)
            {
                context.Entry(e).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public class Busy : IBusy
        {
            public bool IsBusy { get; set; }
        }

        public DialogController DialogController { get; private set; }
    }
}
