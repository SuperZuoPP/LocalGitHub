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
using System.Windows.Documents;
using FastReport.DataVisualization.Charting;
using FastReport.RichTextParser;
using System.Windows.Forms.Integration;

namespace WPFBase.ViewModels.BMViewModel
{
    public class VideoRealPlayViewModel : NavigationViewModel
    {
        private readonly IVideoService service;
         
        public VideoRealPlayViewModel(IContainerProvider containerProvider, IVideoService service) : base(containerProvider)
        {
           
            this.service = service;
            ExecuteCommand = new DelegateCommand<string>(Execute);
            GetDvrVideoList(); 

            try
            {
                // 初始化多摄像头资源控制类
                if (multiVideo == null)
                {
                    multiVideo = new MultiVideoOperate("", @"C:\HikSdk\SdkLog", @"C:\HikSdk\JPEGCapture", 0, 0xff, "", "", "");
                }
                else
                {
                    multiVideo.LoginOut();
                }

                // 初始化sdk
                if (!multiVideo.Init(VideoSdkInitType.Mix))
                {
                    Console.WriteLine("摄像机SDK初始化失败");
                    return;
                }

                // 初始化通道列表
                if (videoMonitorChannelList == null)
                {
                    videoMonitorChannelList = new List<VideoMonitorChannelInfo>();
                }
                else
                {
                    videoMonitorChannelList.Clear();
                }
                 
            }
            finally
            {
                // 重置变量
                ResetState();
            }

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
        /// <summary>
        /// 用户句柄
        /// </summary>
        private Int64 lUserID;
        /// <summary>
        /// 是否正在回放
        /// </summary>
        private bool isPlayBack;
        /// <summary>
        /// 是否打开声音
        /// </summary>
        private bool isSoundOpen;
        /// <summary>
        /// 是否暂停回放
        /// </summary>
        private bool isPause;
        /// <summary>
        /// 是否缩放
        /// </summary>
        private bool isZoom;
        /// <summary>
        /// 视频控制器
        /// </summary>
        private MultiVideoOperate multiVideo;
        /// <summary>
        /// 通道资源列表
        /// </summary>
        private List<VideoMonitorChannelInfo> videoMonitorChannelList;
        /// <summary>
        /// 图片控件列表
        /// </summary>
        private PictureBox[] picBoxArr;
        /// <summary>
        /// 监控面板数量
        /// </summary>
        private int monitorChannelNum = 0;
        /// <summary>
        /// 监控通道信息
        /// </summary>
        internal class VideoMonitorChannelInfo
        {
            public Int64 UserID;                 // 登录用户
            public string DeviceNo;              // 硬盘录像机设备编号
            public VideoOperateInfo OperateInfo; // 硬盘录像机操作类
            public int ChannelNum;               // 通道
            public PictureBox RealPlayWnd;       // 控件句柄
            public bool IsActive;                // 是否聚焦
            public VideoChannelInfo ChannelInfo; // 通道信息
        }

        #endregion

        #region 属性

        private List<TbWeighVideoDto> dvrVideoLists;

        public List<TbWeighVideoDto> DvrVideoLists
        {
            get { return dvrVideoLists; }
            set { dvrVideoLists = value; }
        }


        private List<TbWeighVideoDto> dvrMonitorChannelList;

        public List<TbWeighVideoDto> DvrMonitorChannelList
        {
            get { return dvrMonitorChannelList; }
            set { dvrMonitorChannelList = value; }
        }

        private PictureBox picture1;

        public PictureBox Picture1
        {
            get { return picture1; }
            set { SetProperty<PictureBox>(ref picture1, value); }
        }

        #endregion

        #region 命令

        public DelegateCommand<string> ExecuteCommand { get; set; }


        #endregion

        #region 方法

        //获取硬盘录像机，指定启用状态的摄像机列表
        async void GetDvrVideoList() 
        {
            var dvrlist = await service.GetVideoList(new Shared.Parameters.TbWeighVideoDtoParameter()
            { 
                VideoTypeNo = (int)VideoWorkType.DVR,
                Status = (int)EnumDevicestatus.Working
            });
            if (dvrlist != null && dvrlist.Status)
            {  
                var resultjson = dvrlist.Result.ToString();
                DvrVideoLists = JsonConvert.DeserializeObject<List<TbWeighVideoDto>>(resultjson); 
            } 
            else
            {
                Growl.WarningGlobal("获取摄像机数据失败！");
            }

            foreach (var dvr in DvrVideoLists)
            {
                string deviceNo = dvr.Attribute1;
                EnumDirection position = MultiVideoOperate.GetDirection(dvr.Position);
                VideoSdkType sdkType = MultiVideoOperate.GetSdkTypeByFactory(dvr.Factory);

                // 获取启用状态
                EnumDevicestatus deviceStatus = EnumDevicestatus.Disable;
                try
                {
                    deviceStatus = EnumDevicestatus.Working;//(EnumDevicestatus)tb_weigh_devicestatus.GetModel(deviceNo).Status;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("获取录像机{0}初始状态失败: {1}", deviceNo, ex.Message);
                }

                VideoOperateInfo info = new VideoOperateInfo
                {
                    DeviceNo = deviceNo,
                    Position = position,
                    WorkType = VideoWorkType.DVR,
                    SDKType = sdkType,
                    DeviceStatus = deviceStatus,
                    IP = dvr.IP,
                    Port = dvr.Port,
                    UserName = dvr.UserName,
                    Password = dvr.PassWord
                };

                // 登录硬盘录像机
                if (multiVideo.Login(ref info))
                {
                    VideoLoginInfo dvrLoginInfo = info.Operate.GetVideoLoginInfo();
                    // 磅房可用摄像头列表

                    var dvrslaveList = await service.GetDvrMonitorChannelList(new Shared.Parameters.TbWeighVideoDtoParameter()
                    {
                        DeviceNo = dvr.Attribute1,
                        WeighHouseCodes = "U8VMH-XGYCY-IB5O7-4SR58",
                        Status = (int)EnumDevicestatus.Working
                    });

                    if (dvrslaveList != null && dvrslaveList.Status)
                    {
                        var resultslavejson = dvrslaveList.Result.ToString();
                        DvrMonitorChannelList = JsonConvert.DeserializeObject<List<TbWeighVideoDto>>(resultslavejson);
                    }
                    else
                    {
                        Growl.WarningGlobal("获取摄像机数据失败！");
                    }
                    //List<tb_weigh_video> videoList = tb_weigh_video.GetDvrMonitorChannelList(dvr.Attribute1, weighHouseCode);

                    foreach (var channelInfo in dvrLoginInfo.ChannelInfoList)
                    {
                        try
                        {
                            if (channelInfo.ConnectStatus == VideoChannelConnectStatus.AChan_Disabled || channelInfo.ConnectStatus == VideoChannelConnectStatus.DChan_Idle)
                            {
                                continue; // 通道禁用或空闲
                            }
                            int iChannelNum = channelInfo.iChannelNum;
                            int index = DvrMonitorChannelList.FindIndex(x =>
                            {
                                int ch;
                                if (int.TryParse(x.Attribute4, out ch))
                                {
                                    return ch == iChannelNum;
                                }
                                return false;
                            });
                            if (index < 0)
                            {
                                continue; // 通道不属于磅房
                            }

                            VideoMonitorChannelInfo monitorChannelInfo = new VideoMonitorChannelInfo
                            {
                                UserID = info.UserID, // 用户ID
                                DeviceNo = info.DeviceNo, // 设备编号
                                OperateInfo = info, // 操作类
                                ChannelInfo = channelInfo, // 通道信息
                                ChannelNum = iChannelNum, // 通道号
                                IsActive = false
                            };
                            videoMonitorChannelList.Add(monitorChannelInfo);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                    }

                    // 更新监控窗口数量
                    monitorChannelNum = Math.Min(videoMonitorChannelList.Count(), 6);
                }
            }
         
        }

        void Execute(string obj)
        {
            switch (obj)
            {
                case "RealPlayEnd": StopPlayBack(); break;
                //case "RealPlayPause": GetDataAsync(); break;
                //case "RealPlaySlow": SaveMenu(); break;
                case "RealPlayNormal": PlayBack(); break;
                //case "RealPlayFast": SaveMenu(); break;
                //case "RealPlayCaptureBMP": SaveMenu(); break;
                //case "RealPlaySingleFrame": SaveMenu(); break;
            }

        }


        /// <summary>
        /// 打开回放
        /// </summary>
        private void PlayBack()
        {
            if (isPlayBack)
            {
                Growl.WarningGlobal("请关闭当前回放！"); 
                return;
            }

            // 查询时间
            DateTime playBackTime;
            double advanceTime, postponeTime;
            try
            {
                playBackTime = DateTime.Now.AddMinutes(-100);
                advanceTime = double.Parse("1"); // 提前时间
                postponeTime = double.Parse("1"); // 延迟时间
            }
            catch (Exception ex)
            {
                Console.WriteLine("回放时间异常：" + ex.Message);
                return;
            }

            // 打开回放
            for (var i = 0; i < monitorChannelNum; i++)
            {
                PictureBox pictureBox = picBoxArr[i];
                VideoMonitorChannelInfo monitorChannelInfo = videoMonitorChannelList[i];
                monitorChannelInfo.RealPlayWnd = pictureBox;
                pictureBox.Tag = monitorChannelInfo.DeviceNo + "#" + monitorChannelInfo.ChannelNum;

                try
                {
                    IVideoOperate operate = monitorChannelInfo.OperateInfo.Operate;
                    Int64 playHandle = -1;
                    bool ret = operate.PlayBackByTime(playBackTime.AddMinutes(-advanceTime), playBackTime.AddMinutes(postponeTime), (short)monitorChannelInfo.ChannelNum, pictureBox.Handle, out playHandle);
                    if (ret)
                    {
                        monitorChannelInfo.ChannelInfo.PlayHandle = playHandle;
                        monitorChannelInfo.ChannelInfo.PlayStatus = VideoChannelPlayStatus.PlayBack;
                        isPlayBack = true;

                        Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPSUCC, monitorChannelInfo.ChannelNum, "打开回放"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPFAIL, monitorChannelInfo.ChannelNum, "打开回放异常", ex.Message));
                    continue;
                }
            }
            // 至少有一个通道成功开启回放
            //if (isPlayBack)
            //{
            //    SetControlButtons(true);
            //    timerPlayBack.Start();
            //}
        }

        /// <summary>
        /// 停止回放
        /// </summary>
        private void StopPlayBack()
        {
            if (!isPlayBack)
            {
                return;
            }

            List<VideoMonitorChannelInfo> playbackList = videoMonitorChannelList.Where(x => x.ChannelInfo.PlayHandle >= 0).ToList();
            foreach (var monitorChannelInfo in playbackList)
            {
                try
                {
                    bool ret = monitorChannelInfo.OperateInfo.Operate.StopPlayBack(monitorChannelInfo.ChannelInfo.PlayHandle);
                    if (ret)
                    {
                        monitorChannelInfo.ChannelInfo.PlayHandle = -1;
                        monitorChannelInfo.ChannelInfo.PlayStatus = VideoChannelPlayStatus.None;
                        monitorChannelInfo.RealPlayWnd.Invalidate();
                        monitorChannelInfo.RealPlayWnd.Tag = null;

                        Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPSUCC, monitorChannelInfo.ChannelNum, "关闭回放"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPFAIL, monitorChannelInfo.ChannelNum, "关闭回放异常", ex.Message));
                    continue;
                }
            }
            //// 结束计时
            //timerPlayBack.Stop();
            //// 重置状态
            ResetState();
            //// 重置UI
            //RefreshPlayBackUI();
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        private void ResetState()
        {
            planCode = "";
            lUserID = -1;
            isSoundOpen = true;
            isPause = false;
            isPlayBack = false;
        }
        #endregion
    }
}
