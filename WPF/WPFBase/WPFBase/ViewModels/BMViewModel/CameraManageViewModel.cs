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
using System.Windows.Forms.Integration;
using System.Windows;
using FastReport.Dialog;
using System.Windows.Controls.Primitives;
using WPFBase.Views.BMView;
using WPFBase.Components;
using HandyControl.Tools.Extension;
using WPFHardware.Video.HikVision;
using ImTools;
using System.Threading.Channels;
using WPFHardware.Video.DaHua;
using static WPFHardware.Video.HikVision.CHCNetSDK;
using DryIoc;
using System.Drawing;
using FastReport;
using System.IO;
using Prism.Regions;
using Prism.Events;
using WPFBase.Extensions;

namespace WPFBase.ViewModels.BMViewModel
{
    public class CameraManageViewModel : NavigationViewModel
    {
        private readonly IVideoService service;
        private readonly IEventAggregator eventAggregator;
        public CameraManageViewModel(IContainerProvider containerProvider, IVideoService service, IEventAggregator eventAggregator) : base(containerProvider)
        {
           
            this.service = service;
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ParameterEvent>().Subscribe(OnDataReceived);
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedGroupCommand = new DelegateCommand<string>(SelectedComboxItem);
           
            CmdLoaded = new DelegateCommand<RoutedEventArgs>(Loaded);
            CmdLayOut = new DelegateCommand<string>(LayOut);
           
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
        //private string weighHouseCode;
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
        /// 是否缩放
        /// </summary>
        private bool isZoom;
        /// <summary>
        /// 视频控制器
        /// </summary>
        private MultiVideoOperate multiVideo;

        //private VideoLoginInfo dvrLoginInfo;

        //private VideoOperateInfo videoinfo;
        /// <summary>
        /// 通道资源列表
        /// </summary>
        private List<VideoMonitorChannelInfo> videoMonitorChannelList;
      
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

        /// <summary>
        /// 默认抓图保存位置
        /// </summary>
        private string defaultJpegRepo = @"C:\HikSdk\JPEGCapture";

        #endregion

        #region 属性
        private UniformGrid MyVideoContainer { get; set; }
        private List<PictureBox> MyPictureBoxes { get; set; } = new List<PictureBox>();

        private string weighHouseCode;

        public string WeighHouseCode
        {
            get { return weighHouseCode; }
            set { weighHouseCode = value; }
        }

        private string qSelectedGroup;

        public string QSelectedGroup
        {
            get { return qSelectedGroup; }
            set { SetProperty<string>(ref qSelectedGroup, value); }
        }

        private ObservableCollection<PoundRoomGroup> groupList = new ObservableCollection<PoundRoomGroup>();

        public ObservableCollection<PoundRoomGroup> GroupList
        {
            get { return groupList; }
            set { SetProperty<ObservableCollection<PoundRoomGroup>>(ref groupList, value); }
        }


        private int intRows = 3;

        public int IntRows
        {
            get { return intRows; }
            set { SetProperty<int>(ref intRows, value); }
        }

        private int intCols = 3;

        public int IntCols
        {
            get { return intCols; }
            set { SetProperty<int>(ref intCols, value); }
        }

       
        private bool isPause;

        /// <summary>
        /// 是否暂停回放
        /// </summary>
        public bool IsPause
        {
            get { return isPause; }
            set { SetProperty<bool>(ref isPause, value); }
        }


        private DateTime selectedTime=DateTime.Now.AddMinutes(-100);

        public DateTime SelectedTime
        {
            get { return selectedTime; }
            set { SetProperty<DateTime>(ref selectedTime, value); }
        }

        private string advanceTime="1";

        public string AdvanceTime
        {
            get { return advanceTime; }
            set { SetProperty<string>(ref advanceTime, value); }
        }

        private string postponeTime = "1";

        public string PostponeTime
        {
            get { return postponeTime; }
            set { SetProperty<string>(ref postponeTime, value); }
        }
        

        private List<TbWeighVideoDto> dvrVideoLists;

        /// <summary>
        /// 获取硬盘录像机列表
        /// </summary>
        public List<TbWeighVideoDto> DvrVideoLists
        {
            get { return dvrVideoLists; }
            set { dvrVideoLists = value; }
        }


        private List<TbWeighVideoDto> dvrMonitorChannelList;

        /// <summary>
        /// 所属硬盘录像机摄像机列表
        /// </summary>
        public List<TbWeighVideoDto> DvrMonitorChannelList
        {
            get { return dvrMonitorChannelList; }
            set { dvrMonitorChannelList = value; }
        }

         
        #endregion

        #region 命令

        public DelegateCommand<string> ExecuteCommand { get; set; }


        public DelegateCommand<string> SelectedGroupCommand { get; set; }
        /// <summary>
        /// 关联控件
        /// </summary>
        public DelegateCommand<RoutedEventArgs> CmdLoaded { get; set; }

        /// <summary>
        /// 布局
        /// </summary>
        public DelegateCommand<string> CmdLayOut { get; set; }
       

        #endregion

        #region 方法

        private void Init() 
        {
            try
            {
                // 初始化多摄像头资源控制类
                if (multiVideo == null)
                {
                    multiVideo = new MultiVideoOperate("", @"C:\HikSdk\SdkLog", defaultJpegRepo, 0, 0xff, "", "", "");
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
            catch
            { 

            }
        }

        private void Loaded(RoutedEventArgs args)
        {
            MyVideoContainer = (args.Source as CameraManageView).videoContainer;
            // 图像显示容器
            for (int i = 0; i < MyVideoContainer.Children.Count; i++)
            {
                MyPictureBoxes.Add((MyVideoContainer.Children[i] as PictureBoxUserControl).CustomWindowsFormsHost.Child as PictureBox);
            }
            Init();
            GetDvrVideoList();

        }
         
        /// <summary>
        /// 页面布局
        /// </summary>
        /// <param name="num"></param>
        private void LayOut(string num)
        {
            int videonum = int.Parse(num);
            IntRows = (int)Math.Sqrt(videonum);
            IntCols = IntRows;
            for (int i = 0; i < MyVideoContainer.Children.Count; i++)
            {
                // PictureBoxUserControl 需要和 CameraManageViewModel 放在一个项目里
                MyVideoContainer.Children[i].Visibility = i < videonum ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 获取硬盘录像机列表
        /// </summary>
        async void GetDvrVideoList() 
        {
            var dvrlist = await service.GetVideoList(new Shared.Parameters.TbWeighVideoDtoParameter()
            { 
                VideoTypeNo = (int)VideoWorkType.DVR,
                Status = (int)EnumDevicestatus.Working,
                WeighHouseCodes= WeighHouseCode
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

                VideoOperateInfo videoinfo = new VideoOperateInfo
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
                if (multiVideo.Login(ref videoinfo))
                {
                    VideoLoginInfo dvrLoginInfo = videoinfo.Operate.GetVideoLoginInfo();

                    if (dvrLoginInfo == null)
                    {
                        Growl.WarningGlobal("设备未登陆，请检查网络！");
                        return;
                    }

                    //// 指定磅房可用摄像头列表 
                    var dvrslaveList = await service.GetDvrMonitorChannelList(new Shared.Parameters.TbWeighVideoDtoParameter()
                    {
                        DeviceNo = deviceNo,
                        WeighHouseCodes = WeighHouseCode,
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
                                UserID = videoinfo.UserID, // 用户ID
                                DeviceNo = videoinfo.DeviceNo, // 设备编号
                                OperateInfo = videoinfo, // 操作类
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
                case "VideoPlay": StarRealtPaly(); break;//实时播放
                case "VideoStop": StopRealPlay(); break; //结束实时播放
                case "jpg": VideoPicture(); break;//保存实时图片 
                case "PlayBack": PlayBack(); break; //回放开始
                case "PlayBackEnd": StopPlayBack(); break; //回放结束
                case "RealPlayPause": PausePlayBack(); break;//回放暂停
                case "RealPlaySlow": PlaySlow(); break; //慢放
                case "RealPlayNormal": PlayNormal(); break; //常速
                case "RealPlayFast": PlayFast(); break; //快放
                case "RealPlayCaptureBMP": CaptureBMP(); break; //抓图
                case "RealPlaySingleFrame": PlayFrame(); break; //单帧 
            } 
        }

        public void Start()
        {
            try
            {
                bool IsInitSDK = CHCNetSDK.NET_DVR_Init();
                CHCNetSDK.REALDATACALLBACK realDataCallBack;
                int UserID  = -1;
                int RealHandle = -1;
                bool IsRecord  = false;
                /// 初始化 
                CHCNetSDK.NET_DVR_USER_LOGIN_INFO dvr_user_login_info = new NET_DVR_USER_LOGIN_INFO();
                // 设备 IP 
                byte[] byIP = Encoding.Default.GetBytes("172.16.76.43");
                dvr_user_login_info.sDeviceAddress = new byte[129];
                byIP.CopyTo(dvr_user_login_info.sDeviceAddress, 0);
                // 设备用户名
                byte[] byUserName = Encoding.Default.GetBytes("admin");
                dvr_user_login_info.sUserName = new byte[64];
                byUserName.CopyTo(dvr_user_login_info.sUserName, 0);
                // 设备密码
                byte[] byPassword = Encoding.Default.GetBytes("tgzn1234");
                dvr_user_login_info.sPassword = new byte[64];
                byPassword.CopyTo(dvr_user_login_info.sPassword, 0);
                // 设备服务端口号
                dvr_user_login_info.wPort = (ushort)8000;
                // 是否异步登录：0- 否，1- 是 
                dvr_user_login_info.bUseAsynLogin = false;

                CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo = new NET_DVR_DEVICEINFO_V40();

                int userid = CHCNetSDK.NET_DVR_Login_V40(ref dvr_user_login_info, ref DeviceInfo);
                

                /// 取流
                if (RealHandle < 0)
                {
                    /// 预览实时流回调函数
                    //OnNoticed(string.Format("预览设备【{0}】实时流回调函数", ListCameraInfos[i].DeviceName), EnumLogType.Debug);
                   
                        // 预览实时流回调函数
                        //realDataCallBack = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);
                    

                    /// 预览参数
                    //OnNoticed(string.Format("预览设备【{0}】参数", ListCameraInfos[i].DeviceName), EnumLogType.Debug);
                    CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO
                    {
                        // 预览窗口
                        hPlayWnd = MyPictureBoxes[0].Handle,
                        // 预览的设备通道
                        lChannel = 1,
                        // 用以下设置不卡顿
                        // 码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                        dwStreamType = 1,  // 默认 0
                                           // 连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP
                        dwLinkMode = 4,  // 默认 0
                                         // 0- 非阻塞取流，1- 阻塞取流
                        bBlocked = true, // 默认 false
                                         // 播放库播放缓冲区最大缓冲帧数
                        dwDisplayBufNum = 5, // 默认 1
                        byProtoType = 0,
                        byPreviewMode = 0
                    };

                    /// 打开预览
                    //OnNoticed(string.Format("预览设备【{0}】", ListCameraInfos[i].DeviceName), EnumLogType.Debug);
                    // 用户数据
                    IntPtr pUser = new IntPtr();
                    // 不抓数据流 RealDataCallBack --> null
                    //ListCameraInfos[i].RealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(ListCameraInfos[i].UserID, ref lpPreviewInfo, ListCameraInfos[i].RealDataCallBack, pUser);
                    RealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(userid, ref lpPreviewInfo, null, pUser);
                    if (RealHandle < 0)
                    {
                        return;
                    }
                }
                //MyPictureBoxes[2].Image = Image.;
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 实时预览
        /// </summary>
        private void StarRealtPaly() 
        { 
            try
            {
               
                if (isPlayBack)
                {
                    Growl.WarningGlobal("请先关闭当前播放！");
                    return;
                }

                if (monitorChannelNum == 0)
                {
                    Growl.WarningGlobal("未找到播放资源！");
                }

                for (var i = 0; i < monitorChannelNum; i++)
                {
                    VideoMonitorChannelInfo monitorChannelInfo = videoMonitorChannelList[i];
                    IVideoOperate operate = monitorChannelInfo.OperateInfo.Operate;

                    // 开启监控
                    if (monitorChannelInfo.ChannelInfo.RealHandle < 0)
                    { 
                        monitorChannelInfo.RealPlayWnd = MyPictureBoxes[i];
                        MyPictureBoxes[i].Tag = monitorChannelInfo.DeviceNo + "#" + monitorChannelInfo.ChannelNum;
                        Int64 realHandle = -1;
                        bool ret = operate.RealPlay((short)monitorChannelInfo.ChannelNum, MyPictureBoxes[i].Handle, out realHandle);
                        if (ret)
                        {
                            monitorChannelInfo.ChannelInfo.RealHandle = realHandle;
                            monitorChannelInfo.ChannelInfo.PlayStatus = VideoChannelPlayStatus.Preview; 
                            isPlayBack = true;
                        }

                    }
                    // 关闭监控
                    else
                    {
                        if (monitorChannelInfo.ChannelInfo.RealHandle >= 0)
                        {
                            bool ret = operate.StopRealPlay(monitorChannelInfo.ChannelInfo.RealHandle);
                            if (ret)
                            {
                                monitorChannelInfo.ChannelInfo.RealHandle = -1;
                                monitorChannelInfo.ChannelInfo.PlayStatus = VideoChannelPlayStatus.None;
                                monitorChannelInfo.RealPlayWnd.Invalidate();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("监控轮询操作异常:" + ex.Message);
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

           
            double advanceTime, postponeTime;
            try
            { 
                advanceTime = double.Parse(AdvanceTime); // 提前时间
                postponeTime = double.Parse(PostponeTime); // 延迟时间
            }
            catch (Exception ex)
            {
                Console.WriteLine("回放时间异常：" + ex.Message);
                return;
            }

            // 打开回放
            for (var i = 0; i < monitorChannelNum; i++)
            {
                PictureBox pictureBox = MyPictureBoxes[i];//picBoxArr[i];
                VideoMonitorChannelInfo monitorChannelInfo = videoMonitorChannelList[i];
                monitorChannelInfo.RealPlayWnd = pictureBox;
                pictureBox.Tag = monitorChannelInfo.DeviceNo + "#" + monitorChannelInfo.ChannelNum;

                try
                {
                    IVideoOperate operate = monitorChannelInfo.OperateInfo.Operate;
                    Int64 playHandle = -1;
                    bool ret = operate.PlayBackByTime(SelectedTime.AddMinutes(-advanceTime), SelectedTime.AddMinutes(postponeTime), (short)monitorChannelInfo.ChannelNum, pictureBox.Handle, out playHandle);
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
            if (isPlayBack)
            {
                if (videoMonitorChannelList.Count > 0)
                {
                    VideoMonitorChannelInfo channelInfo = videoMonitorChannelList[0];
                    Int64 playHandle = channelInfo.ChannelInfo.PlayHandle;
                    if (playHandle >= 0)
                    {
                        int iPos = channelInfo.OperateInfo.Operate.GetPlaybackPos(playHandle);
                        Console.WriteLine("播放进度" + iPos);
                        // 回放结束
                        if (iPos == 100)
                        {
                           StopPlayBack(); 
                        }
                        // 网络异常，回放失败
                        if (iPos == 200)
                        {
                            Console.WriteLine("网络异常，回放失败!");  
                        }
                    }
                }
            }
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

            List<VideoMonitorChannelInfo> playbackList = videoMonitorChannelList;
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
            // 重置状态
            ResetState(); 
        }

        /// <summary>
        /// 暂停回放
        /// </summary>
        private void PausePlayBack()
        {
            if (!isPlayBack)
            {
                return;
            }

            bool selfIsPause = !IsPause;

            List<VideoMonitorChannelInfo> playbackList = videoMonitorChannelList.Where(x => x.ChannelInfo.PlayHandle >= 0).ToList();
            foreach (var monitorChannelInfo in playbackList)
            {
                try
                {
                    bool ret = monitorChannelInfo.OperateInfo.Operate.PausePlayBack(selfIsPause, monitorChannelInfo.ChannelInfo.PlayHandle);
                    if (ret)
                    {
                        monitorChannelInfo.ChannelInfo.PlayStatus = VideoChannelPlayStatus.PlayBack_Pause;
                        monitorChannelInfo.RealPlayWnd.Invalidate();
                        monitorChannelInfo.RealPlayWnd.Tag = null;

                        Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPSUCC, monitorChannelInfo.ChannelNum, IsPause ? "暂停" : "播放"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPFAIL, monitorChannelInfo.ChannelNum, (IsPause ? "暂停" : "播放") + "回放异常", ex.Message));
                    continue;
                }
            }

            IsPause = selfIsPause; 
        }

        /// <summary>
        /// 停止实时回放
        /// </summary>
        private void StopRealPlay()
        {
            if (!isPlayBack)
            {
                return;
            }

            List<VideoMonitorChannelInfo> playbackList = videoMonitorChannelList;
            foreach (var monitorChannelInfo in playbackList)
            {
                try
                {
                    if (monitorChannelInfo.ChannelInfo.RealHandle >= 0) 
                    { 
                        bool ret = monitorChannelInfo.OperateInfo.Operate.StopRealPlay(monitorChannelInfo.ChannelInfo.RealHandle);
                        if (ret)
                        {
                            monitorChannelInfo.ChannelInfo.RealHandle = -1;
                            monitorChannelInfo.ChannelInfo.PlayStatus = VideoChannelPlayStatus.None;
                            monitorChannelInfo.RealPlayWnd.Invalidate();
                            monitorChannelInfo.RealPlayWnd.Tag = null;

                            Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPSUCC, monitorChannelInfo.ChannelNum, "关闭回放"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPFAIL, monitorChannelInfo.ChannelNum, "关闭回放异常", ex.Message));
                    continue;
                }
            }
           
            //// 重置状态
            ResetState(); 
        }

        /// <summary>
        /// 保存图片  注：需要重写通用的方法
        /// </summary>
        private void VideoPicture() 
        {
            try
            {
                // 创建抓拍保存目录
                if (!Directory.Exists(defaultJpegRepo))
                {
                    Directory.CreateDirectory(defaultJpegRepo);
                }
                for (var i = 0; i < monitorChannelNum; i++)
                {
                    VideoMonitorChannelInfo monitorChannelInfo = videoMonitorChannelList[i];
                    IVideoOperate operate = monitorChannelInfo.OperateInfo.Operate;


                    if (monitorChannelInfo.ChannelInfo.RealHandle >= 0)
                    {
                        ushort wPicQuality = 1;
                        ushort wPicSize = 0xff;
                        var picname = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + monitorChannelInfo.ChannelInfo.iChannelNum + ".jpg";
                        var path = defaultJpegRepo + "\\" + picname; 
                        //大华实时抓拍方法需要修改
                        bool ret = monitorChannelInfo.OperateInfo.Operate.JPEGCapturePicture(monitorChannelInfo.ChannelNum, wPicQuality, wPicSize, path);
                        if (ret)
                        {
                            Growl.SuccessGlobal(string.Format("保存图片{0}成功！", picname));
                        }
                        else
                        {
                            var ErrId = (int)CHCNetSDK.NET_DVR_GetLastError();
                            Growl.WarningGlobal("保存图片失败！" + ErrId);
                        }
                         
                    } 
                } 
          }
            catch (Exception ex)
            {
                //Console.WriteLine("摄像头{0}抓拍失败: {1}", deviceNo, ex.Message);
            }
        }

        /// <summary>
        /// 慢放
        /// </summary>
        private void PlaySlow()
        {
            if (!isPlayBack)
            {
                return;
            }

            List<VideoMonitorChannelInfo> playbackList = videoMonitorChannelList;
            foreach (var monitorChannelInfo in playbackList)
            {
                try
                {
                    bool ret = monitorChannelInfo.OperateInfo.Operate.PlayBackSlow(monitorChannelInfo.ChannelInfo.PlayHandle);
                    if (ret)
                    {
                        Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPSUCC, monitorChannelInfo.ChannelNum, "慢放"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPFAIL, monitorChannelInfo.ChannelNum, "慢放异常", ex.Message));
                    continue;
                }
            }
        }

        /// <summary>
        /// 常速
        /// </summary>
        private void PlayNormal()
        {
            if (!isPlayBack)
            {
                return;
            }

            List<VideoMonitorChannelInfo> playbackList = videoMonitorChannelList;
            foreach (var monitorChannelInfo in playbackList)
            {
                try
                {
                    bool ret = monitorChannelInfo.OperateInfo.Operate.PlayBackNormal(monitorChannelInfo.ChannelInfo.PlayHandle);
                    if (ret)
                    {
                        Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPSUCC, monitorChannelInfo.ChannelNum, "常速播放"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPFAIL, monitorChannelInfo.ChannelNum, "常速播放异常", ex.Message));
                    continue;
                }
            }
        }

        /// <summary>
        /// 快放
        /// </summary>
        private void PlayFast()
        {
            if (!isPlayBack)
            {
                return;
            }

            List<VideoMonitorChannelInfo> playbackList = videoMonitorChannelList;
            foreach (var monitorChannelInfo in playbackList)
            {
                try
                {
                    bool ret = monitorChannelInfo.OperateInfo.Operate.PlayBackFast(monitorChannelInfo.ChannelInfo.PlayHandle);
                    if (ret)
                    {
                        Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPSUCC, monitorChannelInfo.ChannelNum, "快放"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPFAIL, monitorChannelInfo.ChannelNum, "快放异常", ex.Message));
                    continue;
                }
            }
        }

        /// <summary>
        /// 单帧
        /// </summary>
        private void PlayFrame()
        {
            if (!isPlayBack)
            {
                return;
            }

            List<VideoMonitorChannelInfo> playbackList = videoMonitorChannelList;
            foreach (var monitorChannelInfo in playbackList)
            {
                try
                {
                    bool ret = monitorChannelInfo.OperateInfo.Operate.PlayBackFrame(monitorChannelInfo.ChannelInfo.PlayHandle);
                    if (ret)
                    {
                        Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPSUCC, monitorChannelInfo.ChannelNum, "单帧播放"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPFAIL, monitorChannelInfo.ChannelNum, "单帧播放异常", ex.Message));
                    continue;
                }
            }
        }

        /// <summary>
        /// 回放抓图
        /// </summary>
        private void CaptureBMP()
        {
            if (!isPlayBack)
            {
                return;
            }

            // 创建抓拍保存目录
            if (!Directory.Exists(defaultJpegRepo))
            {
                Directory.CreateDirectory(defaultJpegRepo);
            }

            List<VideoMonitorChannelInfo> playbackList = videoMonitorChannelList;
            foreach (var monitorChannelInfo in playbackList)
            {
                var picname = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + monitorChannelInfo.ChannelInfo.iChannelNum + ".bmp";
                var path = defaultJpegRepo + "\\" + picname;
                try
                {
                    bool ret = monitorChannelInfo.OperateInfo.Operate.PlayBackBMP(monitorChannelInfo.ChannelInfo.PlayHandle, path);
                    if (ret)
                    {
                        Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPSUCC, monitorChannelInfo.ChannelNum, "抓图"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(FORMAT_MSG_CHANNEL_OPFAIL, monitorChannelInfo.ChannelNum, "抓图异常", ex.Message));
                    continue;
                }
            }
        }
         
        /// <summary>
        /// 释放资源
        /// </summary>
        private void VideoDispose()
        {
            ResetState();
            multiVideo.LoginOut();
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


        private void OnDataReceived(string obj)
        {
            //WeighHouseCode = obj;
            // GetVideoList();
            SelectedTime = Convert.ToDateTime(obj);
        }
       

        private void SelectedComboxItem(string groupid)
        {
            VideoDispose();
            Init();
            WeighHouseCode = groupid;
            GetDvrVideoList();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GroupList.Clear();
            GroupList=AppSession.PoundRoomGroupList;
            WeighHouseCode = GroupList.FirstOrDefault().GroupId; //"4FRGET5yreMRrLN"; 
           
        }
        #endregion
    }
}
