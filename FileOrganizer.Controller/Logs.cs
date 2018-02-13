using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Helper;
using FileOrganizer.Models;

namespace FileOrganizer.Controller
{
    public class Logs : ContentController<Controller.Logs, View.Logs, Model.Logs>
    {
        private IEnumerable<Extension> _extensions;

        public Logs(BITS.UI.WPF.Core.Controllers.Controller parent) : base(parent)
        {
        }

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            var context = ContextManager.Context();
            var logs = context.LogEntries.OrderByDescending(x => x.DateTime);

            this.View = new View.Logs();
            this.Model = new Model.Logs(logs);
        }

        public class Busy : IBusy
        {
            public bool IsBusy { get; set; }
        }
    }
}
