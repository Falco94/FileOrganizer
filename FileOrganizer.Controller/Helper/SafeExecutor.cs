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
        public static async Task<bool> ExecuteFn(Action action, string name, Action failureFn)
        {
            if(!await ExecuteFn(action, name))
                return await ExecuteFn(failureFn);
            else
            {
                return true;
            }
        }

        public static async Task<bool> ExecuteFn(Action action)
        {
            return await ExecuteFn(action, string.Empty);
        }

        internal static async Task<bool> ExecuteFn(Action action, string name, string message, Action failureFn)
        {
            if (!await ExecuteFn(action, name, message))
                return await ExecuteFn(failureFn);
            else
            {
                return true;
            }
        }

        internal static async Task<bool> ExecuteFn(Action action, string name, string message)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(action);
                return true;
            }
            catch (Exception e)
            {
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    var result = await DialogHandler.DialogRoot.ShowMessageAsync("Copy to Clipboard? Error at " + name, message, MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Clipboard.SetText(e.Message);
                    }
                });

                return false;
            }
        }

        internal static async Task<bool> ExecuteFn(Action action, string name)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(action);
                return true;
            }
            catch (Exception e)
            {
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    var result = await DialogHandler.DialogRoot.ShowMessageAsync("Copy to Clipboard? Error at " + name, e.Message, MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Clipboard.SetText(e.Message);
                    }
                });

                return false;
            }
        }
    }
}
