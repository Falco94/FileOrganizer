using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileOrganizer.Data;

namespace FileOrganizer.Helper
{
    public class ContextManager
    {
        private static FODataModel _Context;
        private static object _lock = new object();

        public static FODataModel Context()
        {
            lock (_lock)
            {
                if (_Context == null)
                {
                    _Context = new FODataModel();
                }

                return _Context;
            }
        }

        public static void Init()
        {
            Context();
        }
    }
}
