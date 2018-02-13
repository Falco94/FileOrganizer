using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using FileOrganizer.Model;
using FileOrganizer.View;
using FileOrganizer.ViewModels;
using FileOrganizer.Views;
using Runtime.Utility;

namespace FileOrganizer.Controller
{
    public class DropExtension : ContentController<Controller.DropExtension, View.DropExtension, Model.DropExtension>
    {
        public DropExtension(BITS.UI.WPF.Core.Controllers.Controller parent)
            : base(parent)
        {
        }

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.DropExtension();
            this.Model = new Model.DropExtension();

            this.View.DropDown.Drop += (s, e) =>
            {
                this.Model.IsBusy = true;

                var newExtensionsList = new List<string>();

                Task.Run(() =>
                    {
                        newExtensionsList = ExtensionDrop(e); 
                    })
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

                            selectionWindow.ShowDialog();

                        },
                    TaskScheduler.FromCurrentSynchronizationContext());
            };
        }

        private List<string> ExtensionDrop(DragEventArgs dragEventArgs)
        {
            if (dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop))
            {
                IEnumerable<string> extensions = new List<string>();

                var newExtensionsList = new List<string>();

                var folders = (string[]) dragEventArgs.Data.GetData(DataFormats.FileDrop);
                var allFiles = new List<string>();



                foreach (var folder in folders)
                {
                    allFiles.AddRange(FolderSearch.GetFiles(folder, this.Model.SearchSubfolders).ToList());

                }

                extensions = allFiles.Select(x => Path.GetExtension(x).ToLower())
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
    }
}
