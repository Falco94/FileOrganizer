﻿using System.Threading.Tasks;
using BITS.UI.WPF.Core;
using BITS.UI.WPF.Core.Controllers;
using BITS.UI.WPF.Core.Models;

namespace Core.UI
{
    public interface IDialogAccessor
    {
        DialogController DialogController { get; }
    }

    public static class DialogExtension
    {
        //internal static async Task<TResult> LoadDataAsync<TController, TResult>(this TController @this, Func<TResult> loadDataFn)
        //    where TController : Controller<TController>, IDialogAccessor
        //{
        //    return await @this.LoadDataAsync(loadDataFn, x => true,
        //        async r =>
        //        {
        //            var dialogResult = await @this.ConfirmDialogAsync("SUCCESS");
        //        },
        //        async e =>
        //        {
        //            await @this.ErrorDialogAsync("ERROR: " + e?.Message);

        //            return default(TResult);
        //        });
        //}

        public static async Task<DialogResult> ConfirmDialogAsync(this IDialogAccessor @this, string message)
        {
            var view = new View.ConfirmDialog();

            var model = new Models.ConfirmDialog
            {
                Message = message
            };

            var viewResult = new ViewResult<View.ConfirmDialog, Models.ConfirmDialog>(view, model)
                .Bind(x => x.NoCommand, () => model.Result = DialogResult.No)
                .Bind(x => x.YesCommand, () => model.Result = DialogResult.Yes);

            return await @this.DialogController.ShowAsync(viewResult);
        }

        //internal static async Task<DialogResult> ErrorDialogAsync(this IDialogAccessor @this, string message)
        //{
        //    var view = new View.ErrorDialog();

        //    var model = new ConfirmDialog
        //    {
        //        Message = message
        //    };

        //    var viewResult = new ViewResult<View.ErrorDialog, ConfirmDialog>(view, model)
        //        .Bind(x => x.OkCommand, () => model.Result = DialogResult.Ok);

        //    return await @this.DialogController.ShowAsync(viewResult);
        //}
    }
}
