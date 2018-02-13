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
    public partial class Filewatcher : UserControl
    {
        public static readonly RoutedCommand AddNewAssignementCommand = new RoutedCommand(nameof(AddNewAssignementCommand), typeof(ExtensionMapping));
        public static readonly RoutedCommand ChooseFolderCommand = new RoutedCommand(nameof(ChooseFolderCommand), typeof(ExtensionMapping));
        public static readonly RoutedCommand SaveAssignementsCommand = new RoutedCommand(nameof(SaveAssignementsCommand), typeof(ExtensionMapping));
        public static readonly RoutedCommand DeleteAssignementCommand = new RoutedCommand(nameof(DeleteAssignementCommand), typeof(ExtensionMapping));

        public Filewatcher()
        {
            InitializeComponent();
        }
    }
}
