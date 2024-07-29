using Prism.Commands;
using Prism.Ioc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WPFHardware.Video;
using WPFBase.ViewModels.SMViewModel;
using WPFHardware.Video.Constants;
using WPFHardware.Base;
using WPFBase.Services;
using HandyControl.Controls;
using Newtonsoft.Json;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using WPFBase.Models;
using WPFBase.Shared.DTO.BM; 
using System.Windows.Forms.Integration;
using Prism.Regions;
using WPFBase.Extensions;
using Prism.Events;

namespace WPFBase.ViewModels.BMViewModel
{
    public class VideoRealPlayViewModel : NavigationViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        public VideoRealPlayViewModel(IContainerProvider containerProvider, IRegionManager regionManager, IEventAggregator eventAggregator) : base(containerProvider)
        {
            this.regionManager= regionManager;
            this.eventAggregator= eventAggregator;
            ExecuteCommand = new DelegateCommand<string>(Execute);
          
        }

        #region 字段

        /// <summary>
        /// 日期字符串格式
        /// </summary>
        private const string FORMAT_DATESTR = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 通道操作成功
        /// </summary>
        private const string FORMAT_MSG_CHANNEL_OPSUCC = "通道{0} {1}";
        /// <summary>
        /// 通道操作失败
        /// </summary>
        private const string FORMAT_MSG_CHANNEL_OPFAIL = "通道{0} {1}: {2}";

        /// <summary>
        /// 磅房编号
        /// </summary>
        private string weighHouseCode;
        /// <summary>
        /// 磅房名称
        /// </summary>
        private string weighHouseName;
        /// <summary>
        /// 计划单号
        /// </summary>
        private string planCode;
        #endregion

        #region 属性

        
        #endregion

        #region 命令

        public DelegateCommand<string> ExecuteCommand { get; set; }


        #endregion

        #region 方法
 
       
        void Execute(string obj)
        {
            switch (obj)
            {
                case "Search": SendDataToView(); break;
                //case "RealPlayPause": GetDataAsync(); break;
                //case "RealPlaySlow": SaveMenu(); break;
                //case "RealPlayNormal": PlayBack(); break;
                //case "RealPlayFast": SaveMenu(); break;
                //case "RealPlayCaptureBMP": SaveMenu(); break;
                //case "RealPlaySingleFrame": SaveMenu(); break;
            }

        }

        public void SendDataToView()
        { 
            DateTime dateTime = DateTime.Now.AddMinutes(-10); //获取毛重或者皮重时间发送
            eventAggregator.GetEvent<ParameterEvent>().Publish(dateTime.ToString());
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);  
            regionManager.Regions[PrismManager.VideoRealPlayViewRegionName].RequestNavigate("CameraManageView"); 
        }
          
        #endregion
    }
}
