using FileOrganizer.Controller;
using FileOrganizer.ViewModels;
using Runtime.Messaging;
using Runtime.View;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System;
using System.Linq;
using FileOrganizer.Helper;
using FileOrganizer.Enums;

namespace FileOrganizer.Views
{
    /// <summary>
    /// Interaktionslogik für AddNewExtensionsView.xaml
    /// </summary>
    public partial class AddNewExtensionsView : BaseUserControl, INotifyable
    {
        Window _selectionView;

        public AddNewExtensionsView()
        {
            InitializeComponent();

            var model = new AddNewExtensionsViewModel();
            model.Initialize(null);

            var controller = new AddNewExtensionsViewController();
            controller.Model = model;
            controller.Initialize();

            Model = model;
            Controller = controller;

            MessageBus.Default.RegisterMessageToken(this, MessageToken.CloseExtensionSelectionView);
        }

        public void Notify(object sender, NotificationEventArgs e)
        {
            switch (e.Token)
            {
                case MessageToken.CloseExtensionSelectionView:
                    if(_selectionView != null)
                    {
                        Dispatcher.Invoke(() =>_selectionView.Close());
                    }
                    break;
                default:
                    break;
            }
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var newExtensionsList = new List<string>();

                var folders = (string[])e.Data.GetData(DataFormats.FileDrop);

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

                        foreach (var extension in extensions)
                        {
                            if(String.IsNullOrEmpty(extension))
                                continue;

                            var model = (AddNewExtensionsViewModel)Model;

                            if (!model.Extensions
                                .Select(x=>x.ToLower())
                                .Contains(extension?.ToLower()) 
                                && 
                                !newExtensionsList
                                .Select(x=>x.ToLower())
                                .Contains(extension))
                            {
                                newExtensionsList.Add(extension);
                            }
                        }
                    }
                }

                if (newExtensionsList.Any())
                {
                    var selectionView = new AddExtensionSelectionView(newExtensionsList);
                    
                    _selectionView = new Window
                    {
                        Title = "Auswahl der Extensions",
                        Content = selectionView,
                        Height = 600,
                        Width = 480,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    selectionView.InitCloseFn(() => _selectionView.Close());

                    _selectionView.ShowDialog();

                    var model = (AddNewExtensionsViewModel)Model;
                    model.ReloadExtensions();
                }
            }
        }
    }
}
