﻿using FastReport;
using ImTools;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
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
        private PerformanceCounter cpuCounter, ramCounter, diskCounter; 

        public HomeViewModel(IContainerProvider containerProvider, IRegionManager regionManager) : base(containerProvider)
        {
            this.regionManager = regionManager; 
            ComponentCommand = new DelegateCommand<object>(ComponentCmd);
            OPCCommand = new DelegateCommand(OPCConnect); 
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


        private double cpuUsage;

        public double CpuUsage
        {
            get { return cpuUsage; }
            set { SetProperty<double>(ref cpuUsage, value); }
        }

        private double memoryUsage;

        public double MemoryUsage
        {
            get { return memoryUsage; }
            set { SetProperty<double>(ref memoryUsage, value); }
        }

        private double diskUsage;

        public double DiskUsage
        {
            get { return diskUsage; }
            set { SetProperty<double>(ref diskUsage, value); }
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
                StartOpcTimer();
            }
            else
            { 
                StopOpcTimer();
            } 
        }

        public void StartOpcTimer()
        {
            communication.ConnectToOpcServer();
            opcTimer = new Timer(3000); // 3秒（3000毫秒）
            opcTimer.Elapsed += OpcTimer_Elapsed;
            opcTimer.AutoReset = true; // 设置为true，计时器到时间后会自动重新开始
            opcTimer.Enabled = true; // 启动计时器
        }

        public void StopOpcTimer()
        {
            communication.DisConnectOpcServer();
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


        /// <summary>
        /// 获取电脑使用性能
        /// </summary>
        private void ComputerPerformance()
        { 
            // 每隔一段时间更新计数器值
            System.Timers.Timer timer = new System.Timers.Timer(3000);
            try
            { 
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

                // 开始计算CPU和内存、磁盘使用情况  
                timer.Elapsed += (s, e) =>
                {
                    CpuUsage = Math.Round(cpuCounter.NextValue(), 0);
                    DiskUsage = Math.Round(diskCounter.NextValue(), 0);
                    long totalMemory = 1;  
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem"))
                    {
                        foreach (ManagementObject share in searcher.Get())
                        {
                            totalMemory = Convert.ToInt64(share["TotalVisibleMemorySize"])/1024;
                        }
                    } 
                    double ramUsage = (100 - (ramCounter.NextValue() / totalMemory)*100); 
                    MemoryUsage = Math.Round(ramUsage, 0);
                };
                timer.Start();
            }
            catch 
            {
                timer.Stop();
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            IsChenked = true;
            OPCConnect();
            ComputerPerformance();
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            IsChenked=false;
            OPCConnect();
        }


        #endregion

    }
}
