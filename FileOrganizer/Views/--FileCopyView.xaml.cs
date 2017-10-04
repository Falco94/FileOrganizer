using FileOrganizer.Controller;
using FileOrganizer.Enums;
using FileOrganizer.ViewModels;
using Runtime.Messaging;
using Runtime.View;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FileOrganizer.Views
{
    /// <summary>
    /// Interaktionslogik für FileCopyView.xaml
    /// </summary>
    public partial class FileCopyView : BaseWindow, INotifyable
    {
        public FileCopyView(IEnumerable<string> files, string destinationPath)
        {
            InitializeComponent();

            var model = new FileCopyViewModel(files, destinationPath);
            model.Dispatcher = Dispatcher;

            var controller = new FileCopyViewController();
            controller.Model = model;
            controller.Initialize();

            Model = model;
            Controller = controller;

            MessageBus.Default.RegisterMessageToken(this, MessageToken.CloseFileCopyView);

            this.Loaded += FileCopyView_Loaded;
        }

        private void FileCopyView_Loaded(object sender, RoutedEventArgs e)
        {
            Model.Initialize(null);
        }

        public void Notify(object sender, NotificationEventArgs e)
        {
            Dispatcher.Invoke(() => this.Close());
        }


    }
}
