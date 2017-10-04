using System.Windows.Threading;
using FileOrganizer.Controller;
using FileOrganizer.Enums;
using FileOrganizer.Models;
using FileOrganizer.ViewModels;
using Runtime.Messaging;
using Runtime.View;

namespace FileOrganizer.Views
{
    /// <summary>
    /// Interaktionslogik für AddExtensionGroupView.xaml
    /// </summary>
    public partial class AddExtensionGroupView : BaseWindow, INotifyable
    {
        public AddExtensionGroupView(ExtensionGroup extensiongroup = null)
        {
            InitializeComponent();

            var model = new AddExtensionGroupViewModel();
            model.Initialize(new object[] {extensiongroup});
            model.Dispatcher = Dispatcher;

            var controller = new AddExtensionGroupController();
            controller.Model = model;
            controller.Initialize();

            Model = model;
            Controller = controller;

            MessageBus.Default.RegisterMessageToken(this, MessageToken.CloseView);
        }

        public void Notify(object sender, NotificationEventArgs e)
        {
            Dispatcher.Invoke(Close);
        }
    }
}
