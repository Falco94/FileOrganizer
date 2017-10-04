using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;
using FileOrganizer;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Data;
using FileOrganizer.Models;

namespace FileOrganizer.Controller
{
    public class Extensions : ContentController<Controller.Extensions, View.Extensions, Model.Extensions>
    {
        private IEnumerable<Extension> _extensions;

        public Extensions(BITS.UI.WPF.Core.Controllers.Controller parent, IEnumerable<Models.Extension> extensions) : base(parent)
        {
            _extensions = extensions;
        }

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            //try
            //{
            //    Task.WaitAll(Task.Delay(2000));
            //    int a = 7;
            //}
            //catch (Exception ex)
            //{
            //}

            this.View = new View.Extensions();

            this.Model = new Model.Extensions();
            this.Model.Init(_extensions);
        }

        public class Busy : IBusy
        {
            public bool IsBusy { get; set; }
        }
    }
}
