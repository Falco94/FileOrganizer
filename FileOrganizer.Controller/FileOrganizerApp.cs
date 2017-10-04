using BITS.UI.WPF.Core.Controllers;

namespace FileOrganizer.Controller
{
    public class FileOrganizerApp : AppController<FileOrganizerApp>
    {
        protected override void OnInit()
        {
            base.OnInit();

            new Controller.MainWindow(this)
                .Start();
        }
    }
}
