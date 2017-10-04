using System.Windows.Controls;
using System.Windows.Input;

namespace FileOrganizer.View
{
    public partial class ExtensionGroups
    {
        public static readonly RoutedCommand AddNewAssignementCommand = new RoutedCommand(nameof(AddNewAssignementCommand), typeof(ExtensionGroups));
        public static RoutedCommand DeleteAssignementCommand = new RoutedCommand(nameof(DeleteAssignementCommand), typeof(ExtensionGroups));
        public static RoutedCommand ChooseExtensionsCommand = new RoutedCommand(nameof(ChooseExtensionsCommand), typeof(ExtensionGroups));
        public static RoutedCommand SaveGroupsCommand = new RoutedCommand(nameof(SaveGroupsCommand), typeof(ExtensionGroups));

        public ExtensionGroups()
        {
            InitializeComponent();
        }
    }
}
