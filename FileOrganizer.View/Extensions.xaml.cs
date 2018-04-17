using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaktionslogik für Extensions.xaml
    /// </summary>
    public partial class Extensions : UserControl
    {
        public static readonly RoutedCommand DeleteExtensionCommand = new RoutedCommand("DeleteExtension", typeof(Extensions));

        public Extensions()
        {
            InitializeComponent();
        }
    }
}
