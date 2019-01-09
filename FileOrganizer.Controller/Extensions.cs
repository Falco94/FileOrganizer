using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
using FileOrganizer.IService;
using FileOrganizer.Models;
using FileOrganizer.Views;
using MahApps.Metro.Controls.Dialogs;
using Runtime.Utility;

namespace FileOrganizer.Controller
{
    public class Extensions : ContentController<Controller.Extensions, View.Extensions, Model.Extensions>,
        IDialogAccessor
    {
        private IEnumerable<Extension> _extensions;
        private IDialogCoordinator _dialogCoordinator;
        private IProvideExtensions _provideExtensions;

        public static RoutedCommand DeleteExtension
            => FileOrganizer.View.Extensions.DeleteExtensionCommand;

        public Extensions(BITS.UI.WPF.Core.Controllers.Controller parent, IDialogCoordinator dialogCoordinator,
            IEnumerable<Models.Extension> extensions, IProvideExtensions provideExtensions)
            : base(parent)
        {
            _dialogCoordinator = dialogCoordinator;
            _extensions = extensions;
            _provideExtensions = provideExtensions;
        }

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.Extensions();
            this.Model = new Model.Extensions().Init(_extensions);

            this.BindAsync<Models.Extension>(DeleteExtension, DeleteExtensionFn, CanDeleteExtension);

            this.View.DropDown.Drop += async (s, e) =>
            {
                this.Model.IsBusy = true;

                var newExtensionsList = new List<string>();

                //Ask for subfolder Search
                var settings = new MetroDialogSettings();
                settings.AffirmativeButtonText = "Yes";
                settings.NegativeButtonText = "No";

                this.Model.SearchSubfolders = await DialogHandler.DialogRoot.ShowMessageAsync("Advanced Search",
                                                  "Search in Subfolders?", MessageDialogStyle.AffirmativeAndNegative,
                                                  settings) == MessageDialogResult.Affirmative;

                var result = await DialogHandler.DialogRoot.ShowProgressAsync("Progess", "Searching for extensions...");


                await Task.Run(async () => { newExtensionsList = await ExtensionDrop(e, result); })
                    .ContinueWith(
                        async antecedent =>
                        {
                            this.Model.IsBusy = false;

                            var selectionView = new AddExtensionSelectionView(newExtensionsList);
                            var selectionWindow = new Window
                            {
                                Title = "Auswahl der Extensions",
                                Content = selectionView,
                                Height = 600,
                                Width = 480,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen
                            };

                            selectionView.InitCloseFn(() => selectionWindow.Close());

                            //Still open?
                            if (result.IsOpen)
                            {
                                await result.CloseAsync();
                            }

                            selectionWindow.ShowDialog();
                            this.Model = new Model.Extensions().Init(_provideExtensions.GetExtensions());
                        },
                        TaskScheduler.FromCurrentSynchronizationContext());
            };
        }

        private async Task<List<string>> ExtensionDrop(DragEventArgs dragEventArgs,
            ProgressDialogController dialogController)
        {
            if (dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop))
            {
                IEnumerable<string> extensions = new List<string>();

                var newExtensionsList = new List<string>();

                var folders = (string[]) dragEventArgs.Data.GetData(DataFormats.FileDrop);

                int directoryCount = 0;

                var allFiles = new List<string>();
                var foldersearch = new FolderSearch();

                int progress = 0;

                DirectoryFinished RefreshProgress = delegate(object s, EventArgs e)
                {
                    SafeExecutor.ExecuteFn(() =>
                    {
                        if (progress >= directoryCount)
                        {
                            dialogController.CloseAsync();
                        }
                        else
                        {
                            progress++;
                            dialogController.SetProgress(progress);
                        }

                    }, "RefreshProgressBar", () => dialogController.CloseAsync());
                };

                foldersearch.DirectoryFinished += RefreshProgress;

                if (folders != null)
                {
                    directoryCount = folders.Length;
                }

                List<string> foldersWithError = new List<string>();

                if (this.Model.SearchSubfolders)
                {
                    foreach (var folder in folders)
                    {
                        await SafeExecutor.ExecuteFn(() =>
                            {
                                directoryCount += System.IO.Directory
                                    .GetDirectories(folder, "*", SearchOption.AllDirectories).Length;
                            },
                            "DirectorySearch",
                            "Access denied (did you try to search in a harddrive?) or path too long. Try another directory.",
                            () =>
                            {
                                foldersWithError.Add(folder);
                                //foldersearch.DirectoryFinished -= RefreshProgress;
                                //dialogController.CloseAsync();
                            });
                    }
                }

                //Sets the progressbars maximum
                dialogController.Maximum = directoryCount;

                foreach (var folder in folders)
                {
                    if (foldersWithError.Contains(folder))
                    {
                        continue;
                    }

                    allFiles.AddRange(foldersearch.GetFiles(folder, this.Model.SearchSubfolders, true).ToList());
                }

                extensions = allFiles.Select(x => Path.GetExtension(x)?.ToLower())
                    .Distinct();

                var context = ContextManager.Context();
                var dbExtensions = context.Extensions
                    .Select(x => x.ExtensionName.ToLower());

                foreach (var extension in extensions)
                {
                    if (String.IsNullOrEmpty(extension))
                        continue;

                    if (!dbExtensions
                            .Contains(extension?.ToLower())
                        &&
                        !newExtensionsList
                            .Select(x => x.ToLower())
                            .Contains(extension))
                    {
                        newExtensionsList.Add(extension);
                    }
                }

                return newExtensionsList;

                if (newExtensionsList.Any())
                {
                    var selectionView = new AddExtensionSelectionView(newExtensionsList);
                    var selectionWindow = new Window
                    {
                        Title = "Auswahl der Extensions",
                        Content = selectionView,
                        Height = 600,
                        Width = 480,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };


                    selectionView.InitCloseFn(() => selectionWindow.Close());

                    selectionWindow.ShowDialog();
                }
            }

            return new List<string>();
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

            this.Model.ShownExtensions.Remove(e);

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
