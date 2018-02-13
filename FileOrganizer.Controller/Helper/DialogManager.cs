using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BITS.UI.WPF.Core.Controllers;

namespace FileOrganizer.Controller.Helper
{
    public class DialogManager
    {
        private static DialogController _dialogAccessor;
        private static object _lock = new object();


        public static DialogController DialogAccessor
        {
            get { return _dialogAccessor; }
            set
            {
                _dialogAccessor = value;
            }
        }
    }

}
