using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Core.UI.Converters
{
    public class Converter<TFrom, TTo> : IValueConverter
    {
        private Func<object, Type, object, CultureInfo, object> _ConvertFn;

        private Func<object, Type, object, CultureInfo, object> _ConvertBackFn;

        public Converter<TFrom, TTo> Init(Func<object, Type, object, CultureInfo, object> convertFn,
            Func<object, Type, object, CultureInfo, object> convertBackFn = null)
        {
            this._ConvertFn = convertFn;
            this._ConvertBackFn = convertBackFn;

            return this;
        }

        public Converter<TFrom, TTo> Init(Func<object, object, object> convertFn,
            Func<object, object, object> convertBackFn = null)
        {
            this._ConvertFn = (value, targetType, parameter, culture) => convertFn(value, parameter);
            this._ConvertBackFn = (value, targetType, parameter, culture) => convertBackFn(value, parameter);

            return this;
        }

        public Converter<TFrom, TTo> Init(Func<object, object> convertFn,
            Func<object, object> convertBackFn = null)
        {
            this._ConvertFn = (value, targetType, parameter, culture) => convertFn(value);
            this._ConvertBackFn = (value, targetType, parameter, culture) => convertBackFn(value);

            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this._ConvertFn(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (this._ConvertBackFn == null)
                return value;

            return this._ConvertBackFn(value, targetType, parameter, culture);
        }
    }
}
