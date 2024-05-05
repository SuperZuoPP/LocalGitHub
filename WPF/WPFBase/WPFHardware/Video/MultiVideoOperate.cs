using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WPFHardware.Base;
using WPFHardware.Video.Constants;
using WPFHardware.Video.DaHua;
using WPFHardware.Video.HikVision;

namespace WPFHardware.Video
{
    /// <summary>
    /// 多摄像头操作类
    /// </summary>
    public class MultiVideoOperate
    {
        #region 属性

        /// <summary>
        /// 初始化标志
        /// </summary>
        public bool IsInitOK { get; private set; }

        #endregion

        #region 字段

        /// <summary>
        /// 磅房编码
        /// </summary>
        private string weighHouseCode;
        /// <summary>
        /// 默认日志位置
        /// </summary>
        private string defaultSdkLogRepo;
        /// <summary>
        /// 默认抓图保存位置
        /// </summary>
        private string defaultJpegRepo;
        /// <summary>
        /// 默认抓图质量
        /// </summary>
        private ushort defaultJpegQuality;
        /// <summary>
        /// 默认抓图分辨率
        /// </summary>
        private ushort defaultJpegSize;
        /// <summary>
        /// 默认共享文件夹
        /// </summary>
        private string defaultJpegShareRepo;
        /// <summary>
        /// 默认共享文件夹用户
        /// </summary>
        private string defaultJpegShareUser;
        /// <summary>
        /// 默认共享文件夹密码
        /// </summary>
        private string defaultJpegSharePwd;
        /// <summary>
        /// 摄像头控制类字典
        /// </summary>
        private Dictionary<string, VideoOperateInfo> videoOperateInfoDic;
        /// <summary>
        /// 海康-报警回调
        /// </summary>
        private CHCNetSDK.MSGCallBack falarmData;
        /// <summary>
        /// 海康-异常回调
        /// </summary>
        private CHCNetSDK.EXCEPYIONCALLBACK fexceptionData;
        /// <summary>
        /// 大华-断线回调
        /// </summary>
        private fDisConnectCallBack fdisconnectCallBack;
        /// <summary>
        /// 大华-重连回调
        /// </summary>
        private fHaveReConnectCallBack freconnectCallBack;
        /// <summary>
        /// 大华-抓拍回调
        /// </summary>
        private static fSnapRevCallBack fsnapRevCallBack;

        #endregion

        #region 构造器

        public MultiVideoOperate(string weighHouseCode, string defaultSdkLogRepo, string defaultJpegRepo, ushort defaultJpegQuality, ushort defaultJpegSize, string defaultJpegShareRepo, string defaultJpegShareUser, string defaultJpegSharePwd)
        {
            IsInitOK = false;
            this.weighHouseCode = weighHouseCode;
            this.defaultSdkLogRepo = defaultSdkLogRepo;
            this.defaultJpegRepo = defaultJpegRepo;
            this.defaultJpegQuality = defaultJpegQuality;
            this.defaultJpegSize = defaultJpegSize;
            this.defaultJpegShareRepo = defaultJpegShareRepo;
            this.defaultJpegShareUser = defaultJpegShareUser;
            this.defaultJpegSharePwd = defaultJpegSharePwd;
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化SDK
        /// </summary>
        /// <returns></returns>
        public bool Init(VideoSdkInitType sdkInitType)
        {
            try
            {
                // 初始化摄像头操作类字典
                videoOperateInfoDic = new Dictionary<string, VideoOperateInfo>();
                switch (sdkInitType)
                {
                    case VideoSdkInitType.HikVisionOnly:
                        IsInitOK = InitDaHuaSDK();
                        break;
                    case VideoSdkInitType.DahuaOnly:
                        IsInitOK = InitHikSDK();
                        break;
                    case VideoSdkInitType.Mix:
                        IsInitOK = InitHikSDK() && InitDaHuaSDK();
                        break;
                    default:
                        IsInitOK = false;
                        break;
                }

                IsInitOK = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像头SDK初始化失败: {0}", ex.Message);
                IsInitOK = false;
            }

            return IsInitOK;
        }

        /// <summary>
        /// 初始化海康SDK
        /// </summary>
        private bool InitHikSDK()
        {
            try
            {
                bool bInitOk = CHCNetSDK.NET_DVR_Init();
                CHCNetSDK.NET_DVR_SetLogToFile(3, defaultSdkLogRepo, true); // 启用日志文件写入接口
                CHCNetSDK.NET_DVR_SetConnectTime(2000, 1); // 设置连接超时
                CHCNetSDK.NET_DVR_SetReconnect(10000, 1); // 设置重连时间

                falarmData = new CHCNetSDK.MSGCallBack(MSgCallback);
                fexceptionData = new CHCNetSDK.EXCEPYIONCALLBACK(ExceptionCallBack);

                // 报警
                if (!CHCNetSDK.NET_DVR_SetDVRMessageCallBack_V30(falarmData, IntPtr.Zero))
                {
                    Console.WriteLine("海康SDK回调设置失败");
                }
                // 异常
                if (!CHCNetSDK.NET_DVR_SetExceptionCallBack_V30(0, IntPtr.Zero, fexceptionData, IntPtr.Zero))
                {
                    Console.WriteLine("大华SDK回调设置失败");
                }

                return bInitOk;
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像机初始化失败 => model: {0}, error: {1}", "hikvision", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 初始化大华SDK
        /// </summary>
        private bool InitDaHuaSDK()
        {
            try
            {
                fdisconnectCallBack = new fDisConnectCallBack(DisConnectCallBack);
                freconnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack);

                // 初始化，断线重连
                bool bInitOk = NETClient.Init(fdisconnectCallBack, IntPtr.Zero, null);
                // 自动重连
                NETClient.SetAutoReconnect(freconnectCallBack, IntPtr.Zero);
                return bInitOk;
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像机初始化失败 => model: {0}, error: {1}", "dahua", ex.Message);
            }
            return false;
        }

        #endregion

        #region 登录和退出

        /// <summary>
        /// 登录摄像机
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Login(ref VideoOperateInfo info)
        {
            if (info == null)
            {
                return false; // 值非空
            }

            string deviceNo = info.DeviceNo;
            try
            {
                if (videoOperateInfoDic.ContainsKey(deviceNo))
                {
                    return false; // 已注册
                }
                videoOperateInfoDic.Add(deviceNo, info); // 注册

                // 设备操作类
                IVideoOperate videoOperate = info.Operate;
                if (videoOperate == null)
                {
                    switch (info.SDKType)
                    {
                        case VideoSdkType.DaHua:
                            videoOperate = new DaHuaVideoOperation();
                            break;
                        case VideoSdkType.HikVision:
                            videoOperate = new HikVideoOperate();
                            break;
                        default: // 未知SDK
                            info.DeviceStatus = EnumDevicestatus.Video_Error;
                            return false;
                    }
                }

                // 登陆设备
                if (info.UserID < 0)
                {
                    info.Operate = videoOperate;
                    Int64 lUserID = videoOperate.Login(info.IP, info.Port, info.UserName, info.Password);
                    info.UserID = lUserID;
                    info.DeviceStatus = lUserID >= 0 ? EnumDevicestatus.Video_Ready : EnumDevicestatus.Video_Error;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像头{0}登陆失败, 错误: {1}", deviceNo, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// 退出全部设备
        /// </summary>
        public void LoginOut()
        {
            foreach (var x in videoOperateInfoDic)
            {
                VideoOperateInfo info = x.Value;
                try
                {
                    info.Operate.LoginOut();
                    info.DeviceStatus = EnumDevicestatus.Video_Logout;
                    info.UserID = -1;
                }
                catch (Exception ex)
                {
                    string errMsg = string.Format("坐席摄像头{0}退出失败, 错误: {1}", info.DeviceNo, ex.Message);
                    Console.WriteLine(errMsg);
                    //202405tb_weigh_log.Add(errMsg, null, null, null, DateTime.Now);
                    continue;
                }
            }
        }

        /// <summary>
        /// 退出设备
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        public void LoginOut(string deviceNo)
        {
            var kp = videoOperateInfoDic.Where(x => { return x.Value.DeviceNo == deviceNo; }).FirstOrDefault();
            VideoOperateInfo info = kp.Value;
            if (info != null)
            {
                try
                {
                    info.Operate.LoginOut();
                    info.DeviceStatus = EnumDevicestatus.Video_Logout;
                    info.UserID = -1;
                }
                catch (Exception ex)
                {
                    string errMsg = string.Format("摄像头{0}退出失败, 错误: {1}", info.DeviceNo, ex.Message);
                    Console.WriteLine(errMsg);
                    //202405tb_weigh_log.Add(errMsg, null, null, null, DateTime.Now);
                }
            }
        }

        #endregion

        #region 车牌识别

        /// <summary>
        /// 布防
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        public bool SetAlarm(string deviceNo)
        {
            try
            {
                VideoOperateInfo info = videoOperateInfoDic[deviceNo];
                if (info != null)
                {
                    info.Operate.SetAlarm();
                    info.DeviceStatus = EnumDevicestatus.Video_Monitoring;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// 海康-报警回调
        /// </summary>
        /// <param name="lCommand"></param>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        private void MSgCallback(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            switch (lCommand)
            {
                case CHCNetSDK.COMM_ITS_PLATE_RESULT:
                    ProcessCommAlarm_ITSPlate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 海康-异常回调
        /// </summary>
        /// <param name="dwType"></param>
        /// <param name="lUserID"></param>
        /// <param name="lHandle"></param>
        /// <param name="pUser"></param>
        private void ExceptionCallBack(uint dwType, int lUserID, int lHandle, IntPtr pUser)
        {
            switch (dwType)
            {
                case CHCNetSDK.EXCEPTION_EXCHANGE: break; // 用户交互时异常
                case CHCNetSDK.EXCEPTION_PREVIEW: break; // 网络预览时异常
                case 0x8015: break; // 预览时重连成功
                case 0x8017: break; // 用户交互恢复
                default: break; // 异常
            }
        }

        /// <summary>
        /// 海康-报警处理函数
        /// </summary>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        private void ProcessCommAlarm_ITSPlate(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            try
            {
                CHCNetSDK.NET_ITS_PLATE_RESULT struAlarmInfoV30 = new CHCNetSDK.NET_ITS_PLATE_RESULT();
                uint dwSize = (uint)Marshal.SizeOf(struAlarmInfoV30);
                struAlarmInfoV30 = (CHCNetSDK.NET_ITS_PLATE_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_ITS_PLATE_RESULT));
                string AlarmCarNo = Encoding.GetEncoding("GBK").GetString(struAlarmInfoV30.struPlateInfo.sLicense).TrimEnd('\0').Substring(1);

               //202405 Publisher.Fire(CustomEventType.HardwareEvt_PlateRecognition, AlarmCarNo);
            }
            catch (Exception ex)
            {
                //202405 tb_weigh_log.Add("海康-报警处理函数:" + ex.ToString(), null, null, null, DateTime.Now);
            }
        }

        #endregion

        #region 控制道闸

        /// <summary>
        /// 控制全部道闸
        /// </summary>
        /// <param name="gateStatus"></param>
        /// <returns></returns>
        public void controlGate(EnumDevicestatus gateStatus)
        {
            foreach (var kp in videoOperateInfoDic)
            {
                if (kp.Value.WorkType == VideoWorkType.PlateCognition)
                {
                    continue; // 非车牌识别摄像机无法控制道闸
                }
                byte BarrierGateCtrl;
                switch (gateStatus)
                {
                    case EnumDevicestatus.Gate_Closed: BarrierGateCtrl = 0; break;
                    case EnumDevicestatus.Gate_Opening: BarrierGateCtrl = 1; break;
                    case EnumDevicestatus.Gate_Lock: BarrierGateCtrl = 3; break;
                    case EnumDevicestatus.Gate_UnLock: BarrierGateCtrl = 4; break;
                    default: return;
                }
                kp.Value.Operate.ControlGate(BarrierGateCtrl);
            }
        }

        /// <summary>
        /// 控制道闸
        /// </summary>
        /// <param name="lUserID">用户ID</param>
        /// <param name="gateStatus">道闸状态</param>
        /// <returns></returns>
        public bool controlGate(Int64 lUserID, EnumDevicestatus gateStatus)
        {
            try
            {
                KeyValuePair<string, VideoOperateInfo> kp = videoOperateInfoDic.Where(x =>
                {
                    return x.Value.UserID == lUserID && x.Value.WorkType == VideoWorkType.PlateCognition;
                }).FirstOrDefault();

                byte BarrierGateCtrl;
                switch (gateStatus)
                {
                    case EnumDevicestatus.Gate_Closed: BarrierGateCtrl = 0; break;
                    case EnumDevicestatus.Gate_Opening: BarrierGateCtrl = 1; break;
                    case EnumDevicestatus.Gate_Lock: BarrierGateCtrl = 3; break;
                    case EnumDevicestatus.Gate_UnLock: BarrierGateCtrl = 4; break;
                    default: return false;
                }

                return kp.Value.Operate.ControlGate(BarrierGateCtrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 控制道闸
        /// </summary>
        /// <param name="deviceNo">设备编码</param>
        /// <param name="gateStatus">道闸状态</param>
        /// <returns></returns>
        public bool controlGate(string deviceNo, EnumDevicestatus gateStatus)
        {
            try
            {
                VideoOperateInfo info = videoOperateInfoDic[deviceNo];
                if (info.WorkType != VideoWorkType.PlateCognition)
                {
                    return false; // 非车牌识别摄像机无法控制道闸
                }
                return controlGate(info.UserID, gateStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine("控制道闸失败: {0}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 控制对侧道闸
        /// </summary>
        /// <param name="lUserID">用户ID</param>
        /// <param name="gateStatus">道闸状态</param>
        /// <returns></returns>
        public bool controlOppositeGate(Int64 lUserID, EnumDevicestatus gateStatus)
        {
            try
            {
                KeyValuePair<string, VideoOperateInfo> kp = videoOperateInfoDic.Where(x =>
                {
                    return x.Value.WorkType == VideoWorkType.PlateCognition && x.Value.UserID != lUserID;
                }).FirstOrDefault();

                return controlGate(kp.Value.UserID, gateStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 控制对侧道闸
        /// </summary>
        /// <param name="deviceNo">设备编码</param>
        /// <param name="gateStatus">道闸状态</param>
        /// <returns></returns>
        public bool controlOppositeGate(string deviceNo, EnumDevicestatus gateStatus)
        {
            try
            {
                KeyValuePair<string, VideoOperateInfo> kp = videoOperateInfoDic.Where(x =>
                {
                    return x.Value.WorkType == VideoWorkType.PlateCognition && x.Key != deviceNo;
                }).FirstOrDefault();

                return controlGate(kp.Value.UserID, gateStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine("控制道闸失败: {0}", ex.Message);
            }
            return false;
        }

        #endregion

        #region 抓拍

        /// <summary>
        /// 指定摄像头抓拍
        /// </summary>
        /// <param name="deviceNo">摄像头设备编号</param>
        /// <param name="jpgName">图片名称</param>
        /// <returns></returns>
        public bool JPEGCapturePicture(string deviceNo, string jpgName)
        {
            try
            {
                VideoOperateInfo info = videoOperateInfoDic[deviceNo];
                if (info != null)
                {
                    // 单通道大华从0开始，海康从1开始
                    int lChannel;
                    switch (info.SDKType)
                    {
                        case VideoSdkType.DaHua: lChannel = 0; break;
                        case VideoSdkType.HikVision: lChannel = 1; break;
                        default: return false;
                    }
                    // 创建抓拍保存目录
                    if (!Directory.Exists(defaultJpegRepo))
                    {
                        Directory.CreateDirectory(defaultJpegRepo);
                    }
                    return info.Operate.JPEGCapturePicture(lChannel, defaultJpegQuality, defaultJpegSize, Path.Combine(defaultJpegRepo, jpgName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像头{0}抓拍失败: {1}", deviceNo, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 指定摄像头抓拍
        /// </summary>
        /// <param name="userID">摄像头登录用户ID</param>
        /// <param name="jpgName">图片名称</param>
        /// <returns></returns>
        public bool JPEGCapturePicture(Int64 userID, string jpgName)
        {
            string deviceNo = "用户" + userID.ToString();
            try
            {
                KeyValuePair<string, VideoOperateInfo> kp = videoOperateInfoDic.Where(x =>
                {
                    return x.Value.UserID.Equals(userID);
                }).FirstOrDefault();

                if (kp.Value != null)
                {
                    deviceNo = kp.Value.DeviceNo;

                    // 单通道大华从0开始，海康从1开始
                    int lChannel;
                    switch (kp.Value.SDKType)
                    {
                        case VideoSdkType.DaHua: lChannel = 0; break;
                        case VideoSdkType.HikVision: lChannel = 1; break;
                        default: return false;
                    }
                    // 创建抓拍保存目录
                    if (!Directory.Exists(defaultJpegRepo))
                    {
                        Directory.CreateDirectory(defaultJpegRepo);
                    }
                    return kp.Value.Operate.JPEGCapturePicture(lChannel, defaultJpegQuality, defaultJpegSize, Path.Combine(defaultJpegRepo, jpgName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像头{0}抓拍失败: {1}", deviceNo, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 全部抓拍摄像头抓拍
        /// </summary>
        /// <param name="jpgName">图片名称</param>
        /// <returns></returns>
        public bool JPEGCapturePicture(string jpgName)
        {
            List<KeyValuePair<string, VideoOperateInfo>> captureKeyPairs = videoOperateInfoDic.Where(x =>
            {
                return x.Value.WorkType == VideoWorkType.MonitorCapture && x.Value.DeviceStatus == EnumDevicestatus.Video_Monitoring;
            }).ToList();

            try
            {
                // 创建抓拍保存目录
                if (!Directory.Exists(defaultJpegRepo))
                {
                    Directory.CreateDirectory(defaultJpegRepo);
                }

                foreach (var x in captureKeyPairs)
                {
                    // 单通道大华从0开始，海康从1开始
                    int lChannel;
                    switch (x.Value.SDKType)
                    {
                        case VideoSdkType.DaHua: lChannel = 0; break;
                        case VideoSdkType.HikVision: lChannel = 1; break;
                        default: return false;
                    }
                    string newJpgName = Path.GetFileNameWithoutExtension(jpgName) + "_" + x.Value.Position + Path.GetExtension(jpgName);
                    x.Value.Operate.JPEGCapturePicture(lChannel, defaultJpegQuality, defaultJpegSize, Path.Combine(defaultJpegRepo, jpgName));
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像头抓拍失败: {0}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 指定摄像头抓拍并同步到共享文件夹
        /// </summary>
        /// <param name="deviceNo">摄像头设备编号</param>
        /// <param name="jpgName">图片名称</param>
        /// <returns></returns>
        public bool JPEGCapturePictureRemote(string deviceNo, string jpgName)
        {
            try
            {
                VideoOperateInfo info = videoOperateInfoDic[deviceNo];
                if (info != null)
                {
                    byte[] byJpegPicBuffere;
                    uint dwSizeReturned;
                    info.Operate.JPEGCapturePicture(1, defaultJpegQuality, defaultJpegSize, out byJpegPicBuffere, out dwSizeReturned);
                    bool bSuccess = JPEGSaveToShareRepo(byJpegPicBuffere, (int)dwSizeReturned, jpgName, defaultJpegShareRepo, defaultJpegShareUser, defaultJpegSharePwd);
                    if (!bSuccess)
                    {
                        JPEGSaveToLocalRepo(byJpegPicBuffere, (int)dwSizeReturned, jpgName); // 上传失败保存到本地
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像头{0}抓拍失败: {1}", deviceNo, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 指定摄像头抓拍并同步到共享文件夹
        /// </summary>
        /// <param name="userID">摄像头登录用户ID</param>
        /// <param name="jpgName">图片名称</param>
        /// <returns></returns>
        public bool JPEGCapturePictureRemote(Int64 userID, string jpgName)
        {
            string deviceNo = "用户" + userID.ToString();

            try
            {
                KeyValuePair<string, VideoOperateInfo> kp = videoOperateInfoDic.Where(x =>
                {
                    return x.Value.UserID.Equals(userID);
                }).FirstOrDefault();
                if (kp.Value != null)
                {
                    byte[] byJpegPicBuffere;
                    uint dwSizeReturned;
                    kp.Value.Operate.JPEGCapturePicture(1, defaultJpegQuality, defaultJpegSize, out byJpegPicBuffere, out dwSizeReturned);
                    bool bSuccess = JPEGSaveToShareRepo(byJpegPicBuffere, (int)dwSizeReturned, jpgName, defaultJpegShareRepo, defaultJpegShareUser, defaultJpegSharePwd);
                    if (!bSuccess)
                    {
                        JPEGSaveToLocalRepo(byJpegPicBuffere, (int)dwSizeReturned, jpgName); // 上传失败保存到本地
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像头{0}抓拍失败: {1}", deviceNo, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 全部抓拍摄像头抓拍并同步到共享文件夹
        /// </summary>
        /// <param name="jpgName">图片名称</param>
        /// <returns></returns>
        public bool JPEGCapturePictureRemote(string jpgName)
        {
            try
            {
                List<KeyValuePair<string, VideoOperateInfo>> captureKeyPairs = videoOperateInfoDic.Where(x =>
                {
                    return x.Value.WorkType == VideoWorkType.MonitorCapture && x.Value.DeviceStatus == EnumDevicestatus.Video_Monitoring;
                }).ToList();

                foreach (var x in captureKeyPairs)
                {
                    byte[] byJpegPicBuffere;
                    uint dwSizeReturned;
                    string newJpgName = Path.GetFileNameWithoutExtension(jpgName) + "_" + captureKeyPairs.IndexOf(x) + Path.GetExtension(jpgName);
                    x.Value.Operate.JPEGCapturePicture(1, defaultJpegQuality, defaultJpegSize, out byJpegPicBuffere, out dwSizeReturned);
                    bool bSuccess = JPEGSaveToShareRepo(byJpegPicBuffere, (int)dwSizeReturned, newJpgName, defaultJpegShareRepo, defaultJpegShareUser, defaultJpegSharePwd);
                    if (!bSuccess)
                    {
                        JPEGSaveToLocalRepo(byJpegPicBuffere, (int)dwSizeReturned, jpgName); // 上传失败保存到本地
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像头抓拍失败: {0}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 上传到共享文件夹
        /// </summary>
        /// <param name="buffer">缓冲</param>
        /// <param name="bufferLen">缓冲长度</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="remoteUrl">共享文件夹路径</param>
        /// <param name="username">共享用户名</param>
        /// <param name="password">共享密码</param>
        /// <returns></returns>
        private bool JPEGSaveToShareRepo(byte[] buffer, int bufferLen, string fileName, string remoteUrl, string username, string password)
        {
            bool bSuccess = false;
            WebClient myWebClient = new WebClient();
            NetworkCredential cread = new NetworkCredential(username, password, "Domain");
            myWebClient.Credentials = cread;
            Stream postStream = null;
            try
            {
                postStream = myWebClient.OpenWrite(Path.Combine(remoteUrl, weighHouseCode, fileName));
                if (postStream.CanWrite)
                {
                    postStream.Write(buffer, 0, bufferLen);
                    bSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("抓拍图片共享失败: {0}", ex.Message);
            }
            finally
            {
                if (postStream != null)
                {
                    postStream.Close();
                }
            }
            return bSuccess;
        }

        /// <summary>
        /// 将缓冲区里的JPEG图片数据写入本地文件
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bufferLen"></param>
        /// <param name="fileName"></param>
        private bool JPEGSaveToLocalRepo(byte[] buffer, int bufferLen, string fileName)
        {
            try
            {
                FileStream fs = new FileStream(Path.Combine(defaultJpegRepo, fileName), FileMode.Create);
                fs.Write(buffer, 0, bufferLen);
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("抓拍图片保存到本地失败: {0}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 恢复上传
        /// </summary>
        public void JPEGTryUpload()
        {
            if (Directory.Exists(defaultJpegRepo))
            {
                string[] capturePicturesPath = Directory.GetFiles(defaultJpegRepo);
                foreach (var x in capturePicturesPath)
                {
                    string filePath = Path.Combine(defaultJpegRepo, x);
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    int bufferLen = (int)fs.Length;
                    byte[] buffer = new byte[bufferLen];
                    fs.Read(buffer, 0, bufferLen);
                    fs.Close();
                    bool bSuccess = JPEGSaveToShareRepo(buffer, bufferLen, Path.GetFileName(x), defaultJpegShareRepo, defaultJpegShareUser, defaultJpegSharePwd);
                    if (bSuccess)
                    {
                        File.Delete(filePath); // 上传成功，删除本地图片
                    }
                }
            }
        }

        #endregion

        #region 设备状态

        /// <summary>
        /// 获取设备错误码
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <returns></returns>
        public int GetDeviceLastError(string deviceNo)
        {
            try
            {
                VideoOperateInfo info = videoOperateInfoDic[deviceNo];
                if (info != null)
                {
                    return info.Operate.GetDeviceLastError();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }

        /// <summary>
        /// 设置设备状态
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <param name="status">设备状态</param>
        /// <returns>成功：true，失败：false</returns>
        public bool SetDeviceStatus(string deviceNo, EnumDevicestatus status)
        {
            VideoOperateInfo info = videoOperateInfoDic[deviceNo];
            if (info == null)
            {
                return false; // 设备信息不存在
            }
            switch (status)
            {
                case EnumDevicestatus.Disable: // 禁用
                case EnumDevicestatus.Working: // 启用
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取设备状态
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <returns></returns>
        public EnumDevicestatus GetDeviceStatus(string deviceNo)
        {
            try
            {
                return videoOperateInfoDic[deviceNo].DeviceStatus;
            }
            catch (Exception)
            {
                return EnumDevicestatus.Disconnected;
            }
        }

        /// <summary>
        /// 丢失连接回调
        /// </summary>
        /// <param name="lLoginID"></param>
        /// <param name="pchDVRIP"></param>
        /// <param name="nDVRPort"></param>
        /// <param name="dwUser"></param>
        private void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {

        }

        /// <summary>
        /// 重连回调
        /// </summary>
        /// <param name="lLoginID"></param>
        /// <param name="pchDVRIP"></param>
        /// <param name="nDVRPort"></param>
        /// <param name="dwUser"></param>
        private void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {

        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 获取设备方位
        /// </summary>
        /// <returns></returns>
        public static EnumDirection GetDirection(string deviceName)
        {
            if (deviceName.Contains("左"))
            {
                return EnumDirection.LEFT;
            }
            else if (deviceName.Contains("右"))
            {
                return EnumDirection.RIGHT;
            }
            else if (deviceName.Contains("前"))
            {
                return EnumDirection.FRONT;
            }
            else if (deviceName.Contains("后"))
            {
                return EnumDirection.BACK;
            }
            return EnumDirection.UnKnown;
        }

        /// <summary>
        /// 根据厂商获取SDK类型
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static VideoSdkType GetSdkTypeByFactory(string factory)
        {
            VideoSdkType sdkType;
            if (VideoSdkType.DaHua.GetDescription() == factory)
            {
                sdkType = VideoSdkType.DaHua; // 大华
            }
            else if (VideoSdkType.HikVision.GetDescription() == factory)
            {
                sdkType = VideoSdkType.HikVision; // 海康
            }
            else
            {
                sdkType = VideoSdkType.Unknown; // 未知
            }
            return sdkType;
        }

        #endregion
    }
}
