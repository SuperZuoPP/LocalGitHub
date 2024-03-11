using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFBase.Extensions
{
    public static class ComboBoxItemHelper
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached(
                "Value", typeof(object), typeof(ComboBoxItemHelper));

        public static void SetValue(DependencyObject element, object value)
        {
            element.SetValue(ValueProperty, value);
        }

        public static object GetValue(DependencyObject element)
        {
            return element.GetValue(ValueProperty);
        }
    }
}
