using FileOrganizer.Controller;
using FileOrganizer.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileOrganizer.Views
{
    /// <summary>
    /// Interaktionslogik für AddExtensionSelectionView.xaml
    /// </summary>
    public partial class AddExtensionSelectionView : BaseUserControl
    {
        private AddExtensionSelectionViewController _controller;

        public AddExtensionSelectionView(IEnumerable<string> extensions)
        {
            InitializeComponent();

            var model = new AddExtensionSelectionViewModel(extensions);
            model.Initialize(null);

            _controller = new AddExtensionSelectionViewController();
            _controller.Model = model;
            _controller.Initialize();

            Model = model;
            Controller = _controller;
        }

        public AddExtensionSelectionView InitCloseFn(Action closeFn)
        {
            _controller.Init(closeFn);
            Controller = _controller;
            return this;
        }
    }
}
