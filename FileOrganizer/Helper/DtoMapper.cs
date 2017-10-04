using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Helper
{
    public static class ValueTransferer
    {
        public static void WriteToTarget(object source, object target)
        {
            if (source != null && target != null)
            {
                var targetType = target.GetType();

                foreach (var property in source.GetType().GetProperties())
                {
                    targetType.GetProperties().FirstOrDefault(x=>x.Name == property.Name)?.SetValue(target, property.GetValue(source));
                }
            }
        }
    }
}
