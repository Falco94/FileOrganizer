using System.Windows.Input;

namespace UI.Core.View
{
    public partial class ErrorDialog
    {
        public static RoutedCommand Ok = new RoutedCommand(nameof(Ok), typeof(ErrorDialog));

        public RoutedCommand OkCommand => Ok;

        public ErrorDialog()
        {
            InitializeComponent();
        }
    }
}
