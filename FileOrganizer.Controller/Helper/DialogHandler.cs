using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BITS.UI.WPF.Core.Controllers;
using MahApps.Metro.Controls;

namespace FileOrganizer.Controller.Helper
{
    public static class DialogHandler
    {
        private static MetroWindow _dialogRoot;
        private static object _lock = new object();

        public static MetroWindow DialogRoot
        {
            get { return _dialogRoot; }
            set
            {
                _dialogRoot = value;
            }
        }
    }

}
