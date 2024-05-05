using ImTools;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Timers;
using WPFBase.Base;
using WPFBase.Extensions;
using WPFBase.ViewModels.SMViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WPFBase.ViewModels.BMViewModel
{
    public class HomeViewModel : NavigationViewModel
    {
        private readonly IRegionManager regionManager;
        private Timer opcTimer;
        private Dictionary<string, object> opcProperties = new Dictionary<string, object>();
        OPCCommunication communication = new OPCCommunication();
        public HomeViewModel(IContainerProvider containerProvider, IRegionManager regionManager) : base(containerProvider)
        {
            this.regionManager = regionManager;
            IsChenked = true;
            ComponentCommand = new DelegateCommand<object>(ComponentCmd);
            OPCCommand = new DelegateCommand(OPCConnect);
            OPCConnect();
        }


        #region 属性
        private bool isChenked;

        public bool IsChenked
        {
            get { return isChenked; }
            set { SetProperty<bool>(ref isChenked, value); }
        }
        private string tag;

        public string Tag
        {
            get 
            {
                return tag;
            }
            set { tag = value; }
        }

      
        public Dictionary<string, object> OPCProperties
        {
            get { return opcProperties;  }
            set { SetProperty<Dictionary<string, object>>(ref opcProperties, value); }
        }

      

        #endregion


        #region 命令


        public DelegateCommand<object> ComponentCommand { get; set; }

        public DelegateCommand OPCCommand { get; set; }
        #endregion

        #region 方法
        private void ComponentCmd(object obj)
        {
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("WeightSiteView");
        }

        private void OPCConnect()
        {
            if (IsChenked) 
            {
                communication.ConnectToOpcServer();
                StartOpcTimer();
            }else
            {
                communication.DisConnectOpcServer();
                StopOpcTimer();
            } 
        }

        public void StartOpcTimer()
        { 
            opcTimer = new Timer(3000); // 3秒（3000毫秒）
            opcTimer.Elapsed += OpcTimer_Elapsed;
            opcTimer.AutoReset = true; // 设置为true，计时器到时间后会自动重新开始
            opcTimer.Enabled = true; // 启动计时器
        }

        public void StopOpcTimer()
        {
            if (opcTimer != null)
            {
                opcTimer.Stop(); // 停止计时器
                opcTimer.Dispose(); // 释放资源
                opcTimer = null; // 将引用设置为null
            }
        }

        private void OpcTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Read();
        }

        
        private async void Read()
        {
            //OpcPropertyKey = "ns=2;s=数据类型示例.16 位设备.R 寄存器.Double1";
            var newProperties = await communication.ReadMultiple(new[] { "Double1", "Word1", "Float1" });
            OPCProperties = newProperties;
        }
        #endregion

    }
}
