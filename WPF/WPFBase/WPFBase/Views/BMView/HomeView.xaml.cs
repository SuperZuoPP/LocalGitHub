﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFBase.Components;

namespace WPFBase.Views.BMView
{
    /// <summary>
    /// HomeView.xaml 的交互逻辑
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent(); 
        }
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double newWidth = this.mainView.ActualWidth + e.Delta;
            double newHeight = this.mainView.ActualHeight + e.Delta;

            if (newWidth < 500) newWidth = 500;
            if (newHeight < 100) newHeight = 100;


            this.mainView.Width = newWidth;
            this.mainView.Height = newHeight;
            this.mainView.SetValue(Canvas.LeftProperty, (this.RenderSize.Width - this.mainView.Width) / 2);
        }

        bool _isMoving = false;
        Point _downPonit = new Point(0, 0);
        double left = 0;
        double top = 0;
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMoving = true;
            _downPonit = e.GetPosition(sender as Canvas);
            left = double.Parse(this.mainView.GetValue(Canvas.LeftProperty).ToString());
            top = double.Parse(this.mainView.GetValue(Canvas.TopProperty).ToString());
            (sender as Canvas).CaptureMouse();
            e.Handled = true;
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMoving = false;
            (sender as Canvas).ReleaseMouseCapture();
            e.Handled = true;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMoving)
            {
                Point currentPoint = e.GetPosition(sender as Canvas);
                this.mainView.SetValue(Canvas.LeftProperty, left + (currentPoint.X - _downPonit.X));
                this.mainView.SetValue(Canvas.TopProperty, top + (currentPoint.Y - _downPonit.Y));
                e.Handled = true;
            }
        }

 
    }
}
