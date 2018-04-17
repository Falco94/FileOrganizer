using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BITS.UI.WPF.Core.Models;

namespace UI.Core.Models
{
    public interface IDialog
    {
        Task<DialogResult> AwaitResultAsync();
    }
}
