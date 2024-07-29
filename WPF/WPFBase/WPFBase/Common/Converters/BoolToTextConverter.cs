using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFBase.Common.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value)
            {
                return "继续"; // 当值为true时返回“继续”
            }
            else
            {
                return "暂停"; // 当值为false或不是bool时返回“暂停”
            }
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