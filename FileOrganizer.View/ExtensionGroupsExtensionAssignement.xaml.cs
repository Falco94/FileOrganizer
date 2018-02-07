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

namespace FileOrganizer.View
{
    public partial class ExtensionGroupsExtensionAssignement : UserControl
    {
        public static readonly RoutedCommand CloseViewCommand = new RoutedCommand(nameof(CloseViewCommand), typeof(ExtensionGroupsExtensionAssignement));
        public static RoutedCommand DeleteAssignementCommand = new RoutedCommand(nameof(DeleteAssignementCommand), typeof(ExtensionGroupsExtensionAssignement));
        public static RoutedCommand ChooseExtensionsCommand = new RoutedCommand(nameof(ChooseExtensionsCommand), typeof(ExtensionGroupsExtensionAssignement));
        public static RoutedCommand SaveGroupCommand = new RoutedCommand(nameof(SaveGroupCommand), typeof(ExtensionGroupsExtensionAssignement));

        public ExtensionGroupsExtensionAssignement()
        {
            InitializeComponent();
        }
    }
}
