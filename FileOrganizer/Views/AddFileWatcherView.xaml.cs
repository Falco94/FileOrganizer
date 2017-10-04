using FileOrganizer.Controller;
using FileOrganizer.ViewModels;
using Runtime.View;

namespace FileOrganizer.Views
{
    /// <summary>
    /// Interaktionslogik für AddFileWatcherView.xaml
    /// </summary>
    public partial class AddFileWatcherView : BaseUserControl
    {
        public AddFileWatcherView()
        {
            InitializeComponent();

            var model = new AddFileWatcherViewModel();
            model.Initialize(null);
            model.Dispatcher = Dispatcher;

            var controller = new AddFileWatcherController();
            controller.Model = model;
            controller.Initialize();

            Model = model;
            Controller = controller;
        }
    }
}
