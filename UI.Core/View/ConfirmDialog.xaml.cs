using System.Windows.Controls;
using System.Windows.Input;

namespace UI.Core.View
{
    public partial class ConfirmDialog
    {
        public static RoutedCommand No = new RoutedCommand(nameof(No), typeof(ConfirmDialog));

        public static RoutedCommand Yes = new RoutedCommand(nameof(Yes), typeof(ConfirmDialog));

        public RoutedCommand NoCommand => No;

        public RoutedCommand YesCommand => Yes;

        public ConfirmDialog()
        {
            InitializeComponent();
        }
    }
}
