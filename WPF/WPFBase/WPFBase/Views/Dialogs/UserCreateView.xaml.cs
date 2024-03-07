﻿using Prism.Events;
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

namespace WPFBase.Views.Dialogs
{
    /// <summary>
    /// UserCreateView.xaml 的交互逻辑
    /// </summary>
    public partial class UserCreateView : UserControl
    {
        public UserCreateView(IEventAggregator aggregator)
        {
            InitializeComponent();

            //注册提示消息
            aggregator.ResgiterMessage(arg =>
            {
                LoginSnakeBar.MessageQueue.Enqueue(arg.Message);
            }, "Login");
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
