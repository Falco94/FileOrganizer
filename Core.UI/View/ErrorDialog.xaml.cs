using System.Windows.Input;

namespace Core.UI.View
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
