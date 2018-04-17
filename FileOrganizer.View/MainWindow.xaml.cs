using System;
using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FileOrganizer.View
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool _isClosingConfirmed;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            if (this._isClosingConfirmed)
                return;

            ShowConfirmationDialog();

            e.Cancel = true;
            base.OnClosing(e);
        }

        private async void ShowConfirmationDialog()
        {
            var userChoice = await this.ShowMessageAsync("Exit", "Do you really want to close this app?",
                MessageDialogStyle.AffirmativeAndNegative);

            if (userChoice != MessageDialogResult.Affirmative)
            {
                return;
            }

            this._isClosingConfirmed = true;
            this.Close();
        }
    }
}
