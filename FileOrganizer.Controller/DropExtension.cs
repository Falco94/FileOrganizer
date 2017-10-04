using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Data;
using FileOrganizer.Model;
using FileOrganizer.View;
using FileOrganizer.ViewModels;
using FileOrganizer.Views;

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
                ExtensionDrop(e);
            };
        }

        private void ExtensionDrop(DragEventArgs dragEventArgs)
        {
            if (dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var newExtensionsList = new List<string>();

                var folders = (string[])dragEventArgs.Data.GetData(DataFormats.FileDrop);

                foreach (var folder in folders)
                {
                    if (Directory.Exists(folder))
                    {
                        IEnumerable<string> extensions = new List<string>();

                        try
                        {
                            extensions = Directory.GetFiles(folder).Select(x => Path.GetExtension(x).ToLower()).Distinct();
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        using (var dataModel = new FODataModel())
                        {

                            foreach (var extension in extensions)
                            {
                                if (String.IsNullOrEmpty(extension))
                                    continue;

                                if (!dataModel.Extensions
                                    .Select(x => x.ExtensionName.ToLower())
                                    .Contains(extension?.ToLower())
                                    &&
                                    !newExtensionsList
                                        .Select(x => x.ToLower())
                                        .Contains(extension))
                                {
                                    newExtensionsList.Add(extension);
                                }
                            }
                        }
                    }
                }

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
        }
    }
}
