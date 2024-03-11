using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using WPFBase.Extensions;

namespace WPFBase.Common.Converters
{
    public class ComboBoxItemToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 将ComboBoxItem的值转换为整数类型
            if (value is ComboBoxItem comboBoxItem)
            {
                return System.Convert.ToInt32(comboBoxItem.GetValue(ComboBoxItemHelper.ValueProperty));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                // 创建一个新的 ComboBoxItem，并设置其值
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                ComboBoxItemHelper.SetValue(comboBoxItem, intValue);
                comboBoxItem.Content = intValue.ToString();
                return comboBoxItem;
            }
            return null;
        }
    }
}
