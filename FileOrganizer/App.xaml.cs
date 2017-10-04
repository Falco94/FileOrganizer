using System.Windows;
using FileOrganizer.Controller;

namespace FileOrganizer
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Controller.FileOrganizerApp.Run();
        }
    }
}
