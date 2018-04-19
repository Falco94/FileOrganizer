using System;
using System.Collections.Generic;
using System.Windows.Input;
using Runtime.View;

namespace FileOrganizer.View
{
    /// <summary>
    /// Interaktionslogik für AddExtensionSelectionView.xaml
    /// </summary>
    public partial class AddExtensionSelection
    {
        public static readonly RoutedCommand SaveExtensionsCommand = new RoutedCommand("SaveExtensions", typeof(AddExtensionSelection));
        public static readonly RoutedCommand CloseExtensionSelectionViewCommand = new RoutedCommand("CloseExtensionSelectionView", typeof(AddExtensionSelection));

        public AddExtensionSelection()
        {
            InitializeComponent();
        }
    }
}
