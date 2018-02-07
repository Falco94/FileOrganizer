using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Service
{
    public class PropertyMapper
    {
        public TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            var sourceProperties = source.GetType().GetProperties();
            var targetProperties = target.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                //Suche nach gleicher Target Property
                foreach (var targetProperty in targetProperties)
                {
                    if (sourceProperty.Name == targetProperty.Name)
                    {
                        //Wert umschreiben
                        targetProperty.SetValue(target, sourceProperty.GetValue(source));
                        break;
                    }
                }
            }

            return target;
        }
    }
}
