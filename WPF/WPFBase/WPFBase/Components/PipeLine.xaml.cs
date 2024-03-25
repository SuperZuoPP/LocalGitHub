using System;
using System.Collections.Generic;
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
    /// <summary>
    /// PipeLine.xaml 的交互逻辑
    /// </summary>
    public partial class PipeLine : UserControl
    {


        public int Direction
        {
            get { return (int)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(int), typeof(PipeLine), new PropertyMetadata(default(int), new PropertyChangedCallback(OnDirectionChanged)));


        private static void OnDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int value = int.Parse(e.NewValue.ToString());
            VisualStateManager.GoToState(d as PipeLine, value == 1 ? "WEFLowState" : "EWFLowState", false);
        }
         
        public Brush LiguidColor
        {
            get { return (Brush)GetValue(LiguidColorProperty); }
            set { SetValue(LiguidColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LiguidColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LiguidColorProperty =
            DependencyProperty.Register("LiguidColor", typeof(Brush), typeof(PipeLine), new PropertyMetadata(Brushes.Orange));
         

        public int CapRadius
        {
            get { return (int)GetValue(CapRadiusProperty); }
            set { SetValue(CapRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CapRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CapRadiusProperty =
            DependencyProperty.Register("CapRadius", typeof(int), typeof(PipeLine), new PropertyMetadata(default(int)));


        public PipeLine()
        {
            InitializeComponent();
        }
    }
}
