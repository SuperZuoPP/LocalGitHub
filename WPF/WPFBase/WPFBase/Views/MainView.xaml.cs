using Prism.Events;
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
using WPFBase.Common;
using WPFBase.Views.SMView;

namespace WPFBase.Views
{
    public partial class MainView : Window
    {
        private readonly IDialogHostService dialogHostService;

        public MainView(IEventAggregator aggregator, IDialogHostService dialogHostService)
        {
            InitializeComponent();
            this.dialogHostService = dialogHostService;
            //注册提示消息
            aggregator.ResgiterMessage(arg =>
            {
                Snackbar.MessageQueue.Enqueue(arg.Message);
            });

            //注册等待消息窗口
            aggregator.Resgiter(arg =>
            {
                DialogHost.IsOpen = arg.IsOpen;

                if (DialogHost.IsOpen)
                    DialogHost.DialogContent = new ProgressView();
            });

            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };
            btnClose.Click +=   (s, e) =>
            { 
                this.Close();
            };
            ColorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };

            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };

            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
            
        }
    }
 
}
