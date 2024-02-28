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

namespace WPFBase.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            menuBar.SelectionChanged += (s,e) => 
            {
                DrawerLeft.IsOpen = false;


            };

            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };
            btnClose.Click += (s, e) =>
            { 
                this.Close();
            };

            mainwindow.MouseMove += (s, e) => 
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };

            mainwindow.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    if (this.WindowState == WindowState.Normal)
                        this.WindowState = WindowState.Maximized;
                    else
                        this.WindowState = WindowState.Normal;
                } 
            };
        }
    }
}
