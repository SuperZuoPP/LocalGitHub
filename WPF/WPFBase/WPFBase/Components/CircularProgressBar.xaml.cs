using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFBase.Components
{
    public partial class CircularProgressBar : UserControl
    {
        //propdp 属性依赖项属性是一种特殊类型的属性，允许界面元素的属性相互影响，并提供了许多有用的特性，如数据绑定、样式、动画和值继承。
        public double ProgressValue
        {
            get { return (double)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.Register("ProgressValue", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(default(double), new PropertyChangedCallback(OnPropertyChanged)));





        public Brush BackColor
        {
            get { return (Brush)GetValue(BackColorProperty); }
            set { SetValue(BackColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackColorProperty =
            DependencyProperty.Register("BackColor", typeof(Brush), typeof(CircularProgressBar), new PropertyMetadata(Brushes.LightCyan));




        public Brush ForeColor
        {
            get { return (Brush)GetValue(ForeColorProperty); }
            set { SetValue(ForeColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForeColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForeColorProperty =
            DependencyProperty.Register("ForeColor", typeof(Brush), typeof(CircularProgressBar), new PropertyMetadata(Brushes.Orange));



        public CircularProgressBar()
        {
            InitializeComponent();
            this.SizeChanged += CircularProgressBar_SizeChanged;
        }

        private void CircularProgressBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateValue();
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CircularProgressBar).UpdateValue();
        }

        private void UpdateValue()
        {
            this.layouot.Width = Math.Min(this.RenderSize.Width, this.RenderSize.Height);
            double radius = Math.Min(this.RenderSize.Width, this.RenderSize.Height) / 2;
            if (radius <= 0) return;
            double newX = 0.0;
            double newY = 0.0;
            double newProgressValue = this.ProgressValue % 100.0;
            newX = radius + (radius - 3) * Math.Cos((newProgressValue % 100.0 * 3.6 - 90) * Math.PI / 180);
            newY = radius + (radius - 3) * Math.Sin((newProgressValue % 100.0 * 3.6 - 90) * Math.PI / 180);

            //M75 3A75 75 0 0 1 147 75
            string pathDataStr = "M{0} 3A{1} {1} 0 {4} 1 {2} {3}";
            pathDataStr = string.Format(pathDataStr,
                radius + 0.01,
                radius - 3,
                newX,
                newY,
                newProgressValue < 50 && newProgressValue > 0 ? 0 : 1
                );
            var convert = TypeDescriptor.GetConverter(typeof(Geometry));
            this.path.Data = (Geometry)convert.ConvertFrom(pathDataStr);
        }
    }
}
