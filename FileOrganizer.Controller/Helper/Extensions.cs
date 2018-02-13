using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BITS.UI.WPF.Core;
using BITS.UI.WPF.Core.Controllers;
using BITS.UI.WPF.Core.Models;
using Runtime.View;

namespace FileOrganizer.Controller.Helper
{
    internal interface IDialogAccessor
    {
        DialogController DialogController { get; }
    }


    public static class DialogExtension
    {
        internal static async Task<TResult> LoadDataAsync<TController, TResult>(this TController @this, Func<TResult> loadDataFn)
            where TController : Controller<TController>, IDialogAccessor
        {
            return await @this.LoadDataAsync(loadDataFn, x => true,
                async r =>
                {
                },
                async e =>
                {
                    await @this.ErrorDialogAsync("ERROR: " + e?.Message);

                    return default(TResult);
                });
        }

        internal static async Task<DialogResult> ErrorDialogAsync(this IDialogAccessor @this, string message)
        {
            var view = new ErrorDialog();

            var model = new ConfirmDialog
            {
                Message = message
            };

            var viewResult = new ViewResult<ErrorDialog, ConfirmDialog>(view, model)
                .Bind(x => x.OkCommand, () => model.Result = DialogResult.Ok);

            return await @this.DialogController.ShowAsync(viewResult);
        }
    }
}
