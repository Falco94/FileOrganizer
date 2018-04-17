using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BITS.UI.WPF.Core.Controllers;

namespace FileOrganizer.Controller
{
    public class Donate : ContentController<Controller.Donate, View.Donate, Model.Donate>
    {
        public Donate(BITS.UI.WPF.Core.Controllers.Controller parent)
            : base(parent)
        {
        }

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.Donate();
            this.Model = new Model.Donate();

            //this.View.DonateBtn.Click += DonateBtn_Click;
        }

        private void DonateBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources",
                "donate.html"));
        }
    }
}
