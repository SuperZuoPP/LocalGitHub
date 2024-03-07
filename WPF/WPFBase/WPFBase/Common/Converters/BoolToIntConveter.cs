using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFBase.Common.Converters
{
    public class BoolToIntConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? 1 : 0;
            }
            return -1; // 或者您可以选择其他默认值
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int indexValue)
            {
                return indexValue == 1; // 或者您可以将其他索引映射为 true 或 false
            }
            return false; // 或者您可以选择其他默认值
        }
    }
}