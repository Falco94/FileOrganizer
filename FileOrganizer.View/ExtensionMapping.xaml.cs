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
    /// <summary>
    /// Interaktionslogik für ExtensionMapping.xaml
    /// </summary>
    public partial class ExtensionMapping : UserControl
    {
        public static readonly RoutedCommand AddNewAssignementCommand = new RoutedCommand("AddNewAssignement", typeof(ExtensionMapping));
        public static readonly RoutedCommand ChooseFolderCommand = new RoutedCommand("ChooseFolder", typeof(ExtensionMapping));
        public static readonly RoutedCommand SaveAssignementsCommand = new RoutedCommand("SaveAssignements", typeof(ExtensionMapping));
        public static readonly RoutedCommand DeleteAssignementCommand = new RoutedCommand("DeleteAssignement", typeof(ExtensionMapping));

        public ExtensionMapping()
        {
            InitializeComponent();
        }
    }
}
