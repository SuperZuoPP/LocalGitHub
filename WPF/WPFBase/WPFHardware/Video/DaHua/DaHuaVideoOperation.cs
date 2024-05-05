using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static WPFHardware.Video.HikVision.CHCNetSDK;
using WPFHardware.Video.Constants;
using NetSDKCS.Control;

namespace WPFHardware.Video.DaHua
{
    /// <summary>
    /// 大华摄像头操作类
    /// </summary>
    public class DaHuaVideoOperation : IVideoOperate
    {
        #region 字段

        /// <summary>
        /// //用户ID值
        /// </summary>
        private Int64 lUserID = -1;
        /// <summary>
        /// 最新错误代码
        /// </summary>
        private int iLastErr;
        /// <summary>
        /// 设备登陆信息
        /// </summary>
        private VideoLoginInfo loginInfo;
        /// <summary>
        /// 预览句柄
        /// </summary>
        private Int64 lRealHandle = -1;
        /// <summary>
        /// 回放句柄
        /// </summary>
        private Int64 lPlayHandle = -1;
        /// <summary>
        /// 布防句柄
        /// </summary>
        private Int64 lFortifyHandle = -1;
        /// <summary>
        /// 断线回调
        /// </summary>
        private static fDisConnectCallBack fdisconnectCallBack;
        /// <summary>
        /// 重连回调
        /// </summary>
        private static fHaveReConnectCallBack freconnectCallBack;
        /// <summary>
        /// 预览回调
        /// </summary>
        //private static fRealDataCallBackEx2 frealDataCallBackEx2;
        /// <summary>
        /// 事件订阅回调
        /// </summary>
        private fAnalyzerDataCallBack fanalyzerDataCallBack;
        /// <summary>
        /// 抓拍回调
        /// </summary>
        private static fSnapRevCallBack fsnapRevCallBack;
        /// <summary>
        /// 设备信息
        /// </summary>
        private NET_DEVICEINFO_Ex deviceInfo;
        /// <summary>
        /// IP通道总数
        /// </summary>
        private int dwDChanTotalNum;
        private const int m_WaitTime = 5000;
        private const int SyncFileSize = 5 * 1024 * 1204;
        private const int MaxSpeed = 8;
        private const int MinSpeed = 1;

        public int GetDeviceLastError()
        {
            return iLastErr;
        }

        public Int64 GetlRealHandle()
        {
            return lRealHandle;
        }

        public Int64 GetlUserID()
        {
            return lUserID;
        }

        public Int64 GetlPlayHandle()
        {
            return lPlayHandle;
        }

        public Int64 GetlFortifyHandle()
        {
            return lFortifyHandle;
        }

        public VideoLoginInfo GetVideoLoginInfo()
        {
            return loginInfo;
        }
        #endregion

        #region 构造器

        public DaHuaVideoOperation()
        {
            try
            {
                fdisconnectCallBack = new fDisConnectCallBack(DisConnectCallBack);
                freconnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack);
                // frealDataCallBackEx2 = new fRealDataCallBackEx2(RealDataCallBackEx);
                fanalyzerDataCallBack = new fAnalyzerDataCallBack(AnalyzerDataCallBack);
                fsnapRevCallBack = new fSnapRevCallBack(SnapRevCallBack);
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像机初始化失败 => model: {0}, error: {1}", "dahua", ex.Message);
            }
        }

        #endregion

        #region 登录和退出

        /// <summary>
        /// 登录设备
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="Port">端口</param>
        /// <param name="UserNsme">用户名</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public Int64 Login(string ip, string Port, string UserNsme, string Password)
        {
            if (lUserID >= 0)
            {
                return lUserID;
            }
            if (ip == "" || Port == "" || UserNsme == "" || Password == "")
            {
                return lUserID;
            }
            deviceInfo = new NET_DEVICEINFO_Ex();
            IntPtr ptrUserID = NETClient.Login(ip, ushort.Parse(Port), UserNsme, Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);
            if (IntPtr.Zero == ptrUserID)
            {
                iLastErr = NETClient.GetDeviceLastError();
                Console.WriteLine("NETClient Login failed, error: " + NETClient.GetLastError());
            }
            else
            {
                lUserID = (Int64)ptrUserID;
                dwDChanTotalNum = deviceInfo.nChanNum;

                // 初始化登录信息
                if (loginInfo == null)
                {
                    loginInfo = new VideoLoginInfo();
                    loginInfo.ChannelInfoList = new List<VideoChannelInfo>();
                }
                loginInfo.UserID = lUserID;
                loginInfo.RealHandle = -1;
                loginInfo.PlayHandle = -1;
                loginInfo.dwDChanTotalNum = checked((uint)dwDChanTotalNum);
                InfoIPChannel(); // 枚举IP通道
            }
            return lUserID;
        }

        /// <summary>
        /// 枚举IP通道
        /// </summary>
        private void InfoIPChannel()
        {
            NET_MATRIX_CAMERA_INFO[] cameraInfos;
            NETClient.MatrixGetCameras((IntPtr)lUserID, out cameraInfos, dwDChanTotalNum, 1000);

            for (var i = 0; i < cameraInfos.Length; i++)
            {
                VideoChannelInfo ChannelInfo = new VideoChannelInfo();

                NET_MATRIX_CAMERA_INFO stuCameraInfo = cameraInfos[i];
                ChannelInfo.Index = i;
                ChannelInfo.lChannelNo = i;
                ChannelInfo.ChannelName = String.Format("DH_IPC {0}", i);
                ChannelInfo.iChannelNum = stuCameraInfo.nUniqueChannel;
                ChannelInfo.RealHandle = -1;
                ChannelInfo.PlayHandle = -1;
                ChannelInfo.PlayStatus = VideoChannelPlayStatus.None;
                ChannelInfo.ConnectStatus = stuCameraInfo.stuRemoteDevice.bEnable ? VideoChannelConnectStatus.DChan_Online : VideoChannelConnectStatus.DChan_Idle;

                loginInfo.ChannelInfoList.Add(ChannelInfo);
            }
        }

        /// <summary>
        /// 退出设备
        /// </summary>
        /// <returns></returns>
        public void LoginOut()
        {
            try
            {
                if (lUserID >= 0)
                {
                    // 取消事件订阅
                    StopAlarm();

                    if (!NETClient.Logout((IntPtr)lUserID))
                    {
                        iLastErr = NETClient.GetDeviceLastError();
                    }
                    else
                    {
                        lUserID = -1;
                    }

                    // 释放SDK资源
                    NETClient.Cleanup();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("摄像头退出异常:" + ex.Message);
            }
        }

        #endregion

        #region 回调

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

        /// <summary>
        /// 抓拍回调
        /// </summary>
        /// <param name="lLoginID"></param>
        /// <param name="pBuf"></param>
        /// <param name="RevLen"></param>
        /// <param name="EncodeType"></param>
        /// <param name="CmdSerial"></param>
        /// <param name="dwUser"></param>
        private void SnapRevCallBack(IntPtr lLoginID, IntPtr pBuf, uint RevLen, uint EncodeType, uint CmdSerial, IntPtr dwUser)
        {

        }

        /// <summary>
        /// 大华-事件回调
        /// </summary>
        /// <param name="lAnalyzerHandle"></param>
        /// <param name="dwEventType"></param>
        /// <param name="pEventInfo"></param>
        /// <param name="pBuffer"></param>
        /// <param name="dwBufSize"></param>
        /// <param name="dwUser"></param>
        /// <param name="nSequence"></param>
        /// <param name="reserved"></param>
        /// <returns></returns>
        private int AnalyzerDataCallBack(IntPtr lAnalyzerHandle, uint dwEventType, IntPtr pEventInfo, IntPtr pBuffer, uint dwBufSize, IntPtr dwUser, int nSequence, IntPtr reserved)
        {
            EM_EVENT_IVS_TYPE type = (EM_EVENT_IVS_TYPE)dwEventType;
            return 0;
        }

        #endregion

        #region 车牌识别

        /// <summary>
        /// 订阅事件
        /// </summary>
        public void SetAlarm()
        {
            if (this.lFortifyHandle >= 0)
            {
                return;
            }

            int nChannelID = 0; // 通道0
            IntPtr lFortifyHandle = NETClient.RealLoadPicture((IntPtr)lUserID, nChannelID, (uint)EM_EVENT_IVS_TYPE.ALL, true, fanalyzerDataCallBack, IntPtr.Zero, IntPtr.Zero);
            if (lFortifyHandle == IntPtr.Zero)
            {
                iLastErr = NETClient.GetDeviceLastError();
                this.lFortifyHandle = -1;
                LoginOut();
            }
            else
            {
                this.lFortifyHandle = (Int64)lFortifyHandle;
            }
        }

        /// <summary>
        /// 取消事件订阅
        /// </summary>
        public void StopAlarm()
        {
            if (lFortifyHandle >= 0)
            {
                if (!NETClient.StopLoadPic((IntPtr)lFortifyHandle))
                {
                    iLastErr = NETClient.GetDeviceLastError();
                    return;
                }
                lFortifyHandle = -1;
            }
        }

        /// <summary>
        /// 控制道闸
        /// 控制参数：0- 关闭道闸，1- 开启道闸，2- 停止道闸，3- 锁定道闸，4- 解锁道闸  
        /// </summary>
        /// <param name="BarrierGateCtrl"></param>
        /// <returns></returns>
        public bool ControlGate(byte BarrierGateCtrl)
        {
            NET_CTRL_OPEN_STROBE openStrobe = new NET_CTRL_OPEN_STROBE();
            openStrobe.dwSize = (uint)Marshal.SizeOf(typeof(NET_CTRL_OPEN_STROBE));
            openStrobe.nChannelId = 0;
            openStrobe.szPlateNumber = "";
            IntPtr pOpenStrobe = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_CTRL_OPEN_STROBE)));
            Marshal.StructureToPtr(openStrobe, pOpenStrobe, true);

            EM_CtrlType ctrlType;
            switch (BarrierGateCtrl)
            {
                case 0: ctrlType = EM_CtrlType.OPEN_STROBE; break;
                case 1: ctrlType = EM_CtrlType.OPEN_STROBE; break;
                case 3: ctrlType = EM_CtrlType.FORBID_OPEN_STROBE; break;
                default:
                    Marshal.FreeHGlobal(pOpenStrobe);
                    return false;
            }

            bool ret = NETClient.ControlDevice((IntPtr)lUserID, ctrlType, pOpenStrobe, m_WaitTime);
            if (!ret)
            {
                iLastErr = NETClient.GetDeviceLastError();
            }
            Marshal.FreeHGlobal(pOpenStrobe);
            return ret;
        }

        #endregion

        #region 预览

        /// <summary>
        /// 实时预览
        /// </summary>
        /// <param name="lChannel">通道号</param>
        /// <param name="RealPlayWnd">显示控件句柄</param>
        /// <param name="lRealHandle">预览句柄</param>
        /// <returns></returns>
        public bool RealPlay(short lChannel, IntPtr RealPlayWnd, out Int64 lRealHandle)
        {
            if (lUserID < 0)
            {
                lRealHandle = -1;
                return false; // 未登录
            }
            IntPtr ptrRealHandle = NETClient.RealPlay((IntPtr)lUserID, lChannel, RealPlayWnd, EM_RealPlayType.Realplay_1);
            if (ptrRealHandle == IntPtr.Zero)
            {
                iLastErr = NETClient.GetDeviceLastError();
                lRealHandle = -1;
                Console.WriteLine("NETClient RealPlay failed, error: " + NETClient.GetLastError());
                return false;
            }
            lRealHandle = (Int64)ptrRealHandle;
            // 预览实时流回调函数
            // NETClient.SetRealDataCallBack((IntPtr)m_lRealHandle, m_RealDataCallBackEx2, IntPtr.Zero, EM_REALDATA_FLAG.DATA_WITH_FRAME_INFO | EM_REALDATA_FLAG.PCM_AUDIO_DATA | EM_REALDATA_FLAG.RAW_DATA | EM_REALDATA_FLAG.YUV_DATA);
            return true;
        }

        /// <summary>
        /// 停止实时预览
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="realPlayID"></param>
        /// <returns></returns>
        public bool StopRealPlay(Int64 lRealHandle)
        {
            if (lUserID < 0)
            {
                return false; // 未登录
            }
            if (lRealHandle < 0)
            {
                return false; // 未预览
            }
            if (!NETClient.StopRealPlay((IntPtr)lRealHandle))
            {
                iLastErr = NETClient.GetDeviceLastError();
                Console.WriteLine("NETClient StopRealPlay failed, error: " + NETClient.GetLastError());
                return false;
            }
            this.lRealHandle = -1;
            return true;
        }

        /// <summary>
        /// 云台控制
        /// </summary>
        /// <param name="dwPTZCommand"></param>
        /// <param name="Speed"></param>
        /// <param name="dwStop"></param>
        /// <param name="m_lRealHandle"></param>
        /// <param name="m_lUserID"></param>
        /// <param name="m_lChannel"></param>
        /// <returns></returns>
        public bool DeviceControl(int dwPTZCommand, int Speed, int dwStop, Int64 m_lRealHandle = -1, int m_lChannel = -1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 预览回调
        /// </summary>
        /// <param name="lRealHandle"></param>
        /// <param name="dwDataType"></param>
        /// <param name="pBuffer"></param>
        /// <param name="dwBufSize"></param>
        /// <param name="param"></param>
        /// <param name="dwUser"></param>
        private void RealDataCallBackEx(IntPtr lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr param, IntPtr dwUser)
        {
            //do something such as save data,send data,change to YUV. 比如保存数据，发送数据，转成YUV等.
        }

        #endregion

        #region 抓拍

        /// <summary>
        /// 保存路径
        /// </summary>
        private string m_SavePath;

        public bool JPEGCapturePicture(int lChannel, ushort wPicQuality, ushort wPicSize, string savePath)
        {
            m_SavePath = savePath;

            NET_MANUAL_SNAP_PARAMETER par = new NET_MANUAL_SNAP_PARAMETER();
            par.byReserved = new byte[60];
            par.nChannel = lChannel;
            par.bySequence = "";
            IntPtr parPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_MANUAL_SNAP_PARAMETER)));

            Marshal.StructureToPtr(par, parPtr, true);
            bool ret = NETClient.ControlDevice((IntPtr)lUserID, EM_CtrlType.MANUAL_SNAP, parPtr, m_WaitTime);
            if (!ret)
            {
                iLastErr = NETClient.GetDeviceLastError();
            }
            Marshal.FreeHGlobal(parPtr);
            return ret;
        }

        public void JPEGCapturePicture(int lChannel, ushort wPicQuality, ushort wPicSize, out byte[] byJpegPicBuffere, out uint dwSizeReturned)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 保存抓拍图片
        /// 推断buffer 0-Offset-1是整张截图，Offset-fileLength是车牌小图
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fileLenth"></param>
        /// <param name="offSet"></param>
        /// <param name="groupID"></param>
        /// <param name="index"></param>
        /// <param name="id"></param>
        /// <param name="boundingBox"></param>
        private void SavePicture(byte[] buffer, uint fileLenth, uint offSet, string groupID, string index, string id, NET_RECT boundingBox)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return;
            }

            FileStream fileStream = new FileStream(m_SavePath, FileMode.OpenOrCreate);
            // 截掉小图数据
            if (fileLenth > 0 && offSet > 0 && buffer.Length >= offSet + fileLenth)
            {
                fileStream.Write(buffer, 0, (int)offSet - 1);
            }
            else
            {
                fileStream.Write(buffer, 0, buffer.Length);
            }
            fileStream.Flush();
            fileStream.Dispose();
        }

        #endregion

        #region 回放

        /// <summary>
        /// 回放控制
        /// </summary>
        /// <param name="lPlayHandle"></param>
        /// <param name="dwControlCode"></param>
        /// <returns></returns>
        public bool PlayBackControl(int dwControlCode, Int64 lPlayHandle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 按时间回放
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="iChannelNum"></param>
        /// <param name="RealPlayWnd"></param>
        /// <param name="lPlayHandle"></param>
        /// <returns></returns>
        public bool PlayBackByTime(DateTime? startTime, DateTime? endTime, int iChannelNum, IntPtr RealPlayWnd, out Int64 lPlayHandle)
        {
            lPlayHandle = -1;

            // 查询录像文件

            int fileCount = 0;
            NET_RECORDFILE_INFO[] recordFileArray = new NET_RECORDFILE_INFO[5000];
            // set stream type 设置码流类型
            EM_STREAM_TYPE streamType = EM_STREAM_TYPE.MAIN;
            IntPtr pStream = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            Marshal.StructureToPtr((int)streamType, pStream, true);
            NETClient.SetDeviceMode((IntPtr)lUserID, EM_USEDEV_MODE.RECORD_STREAM_TYPE, pStream);
            // query record file 查询录像文件
            bool ret = NETClient.QueryRecordFile((IntPtr)lUserID, iChannelNum, EM_QUERY_RECORD_TYPE.ALL, (DateTime)startTime, (DateTime)endTime, null, ref recordFileArray, ref fileCount, m_WaitTime, false);
            if (!ret)
            {
                iLastErr = NETClient.GetDeviceLastError();
                return false;
            }
            if (fileCount == 0)
            {
                Console.WriteLine("None Record file(没有录像文件)!");
                return false;
            }

            VideoTime[] videoTimeArray = new VideoTime[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                videoTimeArray[i] = new VideoTime();
                videoTimeArray[i].StartTime = recordFileArray[i].starttime.ToDateTime();
                videoTimeArray[i].EndTime = recordFileArray[i].endtime.ToDateTime();
            }
            if (endTime > recordFileArray[fileCount - 1].endtime.ToDateTime())
            {
                endTime = recordFileArray[fileCount - 1].endtime.ToDateTime();
            }

            // 回放
            NET_IN_PLAY_BACK_BY_TIME_INFO stuInfo = new NET_IN_PLAY_BACK_BY_TIME_INFO();
            NET_OUT_PLAY_BACK_BY_TIME_INFO stuOut = new NET_OUT_PLAY_BACK_BY_TIME_INFO();
            stuInfo.stStartTime = NET_TIME.FromDateTime((DateTime)startTime);
            stuInfo.stStopTime = NET_TIME.FromDateTime((DateTime)endTime);
            stuInfo.hWnd = RealPlayWnd;
            stuInfo.cbDownLoadPos = null;
            stuInfo.dwPosUser = IntPtr.Zero;
            stuInfo.fDownLoadDataCallBack = null;
            stuInfo.dwDataUser = IntPtr.Zero;
            stuInfo.nPlayDirection = 0;
            stuInfo.nWaittime = m_WaitTime;

            IntPtr ptrPlayHandle = NETClient.PlayBackByTime((IntPtr)lUserID, iChannelNum, stuInfo, ref stuOut);
            if (ptrPlayHandle == IntPtr.Zero)
            {
                iLastErr = NETClient.GetDeviceLastError();
                return false;
            }
            lPlayHandle = (Int64)ptrPlayHandle;
            return true;
        }

        /// <summary>
        /// 开启/关闭回访音频
        /// </summary>
        /// <param name="bSound"></param>
        /// <param name="lPlayHandle"></param>
        /// <returns></returns>
        public bool PlayBackSoundControl(bool bSound, Int64 lPlayHandle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 暂停回放
        /// </summary>
        /// <param name="bPause"></param>
        /// <param name="lPlayHandle"></param>
        /// <returns></returns>
        public bool PausePlayBack(bool bPause, long lPlayHandle)
        {
            if (lPlayHandle < 0)
            {
                return false;
            }

            PlayBackType type = bPause ? PlayBackType.Pause : PlayBackType.Play;
            if (!NETClient.PlayBackControl((IntPtr)lPlayHandle, type))
            {
                iLastErr = NETClient.GetDeviceLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 停止回放
        /// </summary>
        /// <param name="lPlayHandle"></param>
        /// <returns></returns>
        public bool StopPlayBack(Int64 lPlayHandle)
        {
            if (!NETClient.PlayBackControl((IntPtr)lPlayHandle, PlayBackType.Stop))
            {
                iLastErr = NETClient.GetDeviceLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取回放进度
        /// </summary>
        /// <param name="lPlayHandle"></param>
        /// <returns></returns>
        public int GetPlaybackPos(Int64 lPlayHandle)
        {
            try
            {
                NET_TIME osdTime = new NET_TIME();
                NET_TIME osdStartTime = new NET_TIME();
                NET_TIME osdEndTime = new NET_TIME();
                NETClient.GetPlayBackOsdTime((IntPtr)lPlayHandle, ref osdTime, ref osdStartTime, ref osdEndTime);

                if (osdTime.ToDateTime() >= osdEndTime.ToDateTime())
                {
                    return 100; // 播放结束
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DaHua获取回放进度失败，" + ex.Message);
                return 200; // 网络异常
            }
            return 0;
        }

        /// <summary>
        /// 慢放
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        public bool PlayBackSlow(Int64 lPlayHandle)
        {
            if (!NETClient.PlayBackControl((IntPtr)lPlayHandle, PlayBackType.Slow))
            {
                iLastErr = NETClient.GetDeviceLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 正常
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        public bool PlayBackNormal(Int64 lPlayHandle)
        {
            if (!NETClient.PlayBackControl((IntPtr)lPlayHandle, PlayBackType.Normal))
            {
                iLastErr = NETClient.GetDeviceLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 快放
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        public bool PlayBackFast(Int64 lPlayHandle)
        {
            if (!NETClient.PlayBackControl((IntPtr)lPlayHandle, PlayBackType.Fast))
            {
                iLastErr = NETClient.GetDeviceLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 单帧
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        public bool PlayBackFrame(Int64 lPlayHandle)
        {
            return false;
        }

        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <param name="sBmpPicFileName">图片保存路径和文件名</param>
        /// <returns></returns>
        public bool PlayBackBMP(Int64 lPlayHandle, string sBmpPicFileName = "test.bmp")
        {
            return false;
        }

        #endregion

        #region 硬件通用接口

        #region Useless

        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <returns></returns>
        public int Init()
        {
            return 0;
        }

        /// <summary>
        /// 启动设备
        /// </summary>
        public int Launch()
        {
            return 0;
        }

        /// <summary>
        /// 重启设备
        /// </summary>
        public int Restart()
        {
            return 0;
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        public int Shutdown()
        {
            return 0;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public int Dispose()
        {
            return 0;
        }

        #endregion

        #endregion
    }
}
