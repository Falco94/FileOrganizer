using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace FileOrganizer.Controller.Helper
{
    public static class SafeExecutor
    {
        public static void ExecuteFn(Action action)
        {
            ExecuteFn(action, string.Empty);
        }

        internal static async void ExecuteFn(Action action, string name)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                var result = await DialogHandler.DialogRoot.ShowMessageAsync("Copy to Clipboard? Error at " + name, e.Message, MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)
                {
                    Clipboard.SetText(e.Message);
                }
            }
        }
    }
}
