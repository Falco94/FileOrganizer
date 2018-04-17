using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Runtime.Enums;
using Runtime.Services.Plumbing;

namespace FileOrganizer.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                Controller.FileOrganizerApp.Run();
            }
            catch (Exception ex)
            {
                var logger = new LoggingService();
                logger.InitializeService();
                logger.Log(ex.Message, LogType.ERROR);
            }
        }
    }
}
