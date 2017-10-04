using FileOrganizer.Controller;
using FileOrganizer.ViewModels;
using Runtime.View;

namespace FileOrganizer.Views
{
    /// <summary>
    /// Interaktionslogik für AddExtensionAssignementView.xaml
    /// </summary>
    public partial class AddExtensionAssignementView : BaseUserControl
    {
        public AddExtensionAssignementView()
        {
            InitializeComponent();

            var model = new AddExtensionAssignementViewModel();
            model.Initialize(null);

            var controller = new AddExtensionAssignementController();
            controller.Model = model;
            controller.Initialize();

            Model = model;
            Controller = controller;
        }
    }
}
