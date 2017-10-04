using FileOrganizer.Controller;
using FileOrganizer.Models;
using FileOrganizer.MVC;
using FileOrganizer.ViewModels;
using MahApps.Metro.Controls;
using Runtime.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FileOrganizer.Helper;
using FileOrganizer.Test;
using Path = System.Windows.Shapes.Path;

namespace FileOrganizer.View
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class FileOrganizerMainView : BaseMahappsWindow
    {
        public FileOrganizerMainView()
        {
            InitializeComponent();

            var model = new FileOrganizerMainViewModel();
            model.Initialize();

            var controller = new FileOrganizerMainViewController();
            controller.Model = model;
            controller.Initialize();

            Model = model;
            Controller = controller;
        }

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var folders = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var folder in folders)
                {
                    foreach (var mappingItem in ((FileOrganizerMainViewModel) Model).MappingItems)
                    {
                        //Alle Dateien im Verzeichnis mit der entsprechenden Extension
                        var files = Directory.GetFiles(folder)
                            .Where(x => System.IO.Path.GetExtension(x) == mappingItem.Extension.ExtensionName).ToList();

                        if (files.Count > 0)
                        {
                            var copier = new FileCopier();
                            //copier.Copy();
                            //foreach (var sourceFile in files)
                            //{
                            //    var destinationFile = Path.Combine(TestPaths.Ausgabeordner, Path.GetFileName(sourceFile));



                            //    FileSystem.CopyFile(sourceFile, destinationFile, UIOption.AllDialogs);
                            //}

                            //TODO: Prüfen ob Pfad existiert
                            copier.Copy(files.ToArray(), mappingItem.TargetPath);
                        }
                    }
                }
            }
        }
    }
}
