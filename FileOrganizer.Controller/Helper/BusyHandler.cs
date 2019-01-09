using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Controller.Helper
{
    public static class BusyHandler
    {
        private static bool _isBusy;
        private static object _lock = new object();

        public static bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
            }
        }
    }
}
