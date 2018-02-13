using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileOrganizer.Helper
{
    public class ExtensionMappingManager
    {
        private static Object _lock = new Object();

        private static Dictionary<int, string> _extensionIdPathMappings = new Dictionary<int, string>();
        private static Dictionary<string, int> _extensionMappings = new Dictionary<string, int>();

        private static bool isBusy = false;

        private static bool IsBusy
        {
            get
            {
                return isBusy; 
            }
            set
            {
                lock (_lock)
                {
                    isBusy = value; 
                }
            }
        }

        public static void ReInit()
        {
            while (IsBusy)
            {
                Thread.Sleep(200);
            }

            _extensionIdPathMappings = new Dictionary<int, string>();
            _extensionMappings = new Dictionary<string, int>();
        }

        public static string LookUpExtensionMapping(string extension)
        {
            IsBusy = true;

            if (String.IsNullOrWhiteSpace(extension))
                return String.Empty;

            var context = ContextManager.Context();
            var mappingPath = string.Empty;

            var extensionId = 0;

            if (_extensionMappings.ContainsKey(extension))
            {
                extensionId = _extensionMappings[extension];
            }
            else
            {
                extensionId = context.Extensions.SingleOrDefault(x => x.ExtensionName == extension)?.ExtensionId ?? 0;

                if (extensionId == 0)
                    return String.Empty;

                _extensionMappings.Add(extension, extensionId);
            }

            if (_extensionIdPathMappings.ContainsKey(extensionId))
            {
                mappingPath = _extensionIdPathMappings[extensionId];
            }
            else
            {
                mappingPath = context.ExtensionMappingItems.SingleOrDefault(x =>
                                  x.Extension.ExtensionId == extensionId)
                                  ?.TargetPath ??
                              string.Empty;

                if (String.IsNullOrWhiteSpace(mappingPath))
                {
                    //Get Groups with the specified Extension
                    var group = context.ExtensionGroups.FirstOrDefault(x =>
                        x.Extensions.Select(y => y.ExtensionId).Contains(extensionId));

                    if (group != null)
                    {
                        mappingPath = context.ExtensionMappingItems.SingleOrDefault(x =>
                                              x.ExtensionGroup.ExtensionGroupId == group.ExtensionGroupId)
                                          ?.TargetPath ??
                                      string.Empty;
                    }
                    else
                    {
                        mappingPath = string.Empty;
                    }
                }

            }

            IsBusy = false;

            return mappingPath;
        }

    }
}
