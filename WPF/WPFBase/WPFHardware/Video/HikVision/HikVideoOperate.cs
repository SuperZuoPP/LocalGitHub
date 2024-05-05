using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WPFHardware.Video.Constants;

namespace WPFHardware.Video.HikVision
{
    public class HikVideoOperate : IVideoOperate
    {
        #region 字段

        /// <summary>
        /// //用户ID值
        /// </summary>
        private Int64 lUserID = -1;
        /// <summary>
        /// 最新错误代码
        /// </summary>
        private uint iLastErr;
        /// <summary>
        /// 预览句柄
        /// </summary>
        private Int64 lRealHandle = -1;
        /// <summary>
        /// 布防句柄
        /// </summary>
        private Int64 lFortifyHandle = -1;
        /// <summary>
        /// 回放句柄
        /// </summary>
        private Int64 lPlayHandle = -1;
        /// <summary>
        /// 设备登陆信息
        /// </summary>
        private VideoLoginInfo loginInfo;
        /// <summary>
        /// 模拟通道总数
        /// </summary>
        private uint dwAChanTotalNum = 0;
        /// <summary>
        /// IP通道总数
        /// </summary>
        private uint dwDChanTotalNum = 0;
        private int[] iChannelNum = new int[96];
        private int[] iIPDevID = new int[96];

        private CHCNetSDK.REALDATACALLBACK fRealData;
        private CHCNetSDK.LOGINRESULTCALLBACK fLoginCallBack;
        private CHCNetSDK.NET_DVR_IPPARACFG_V40 struIpParaCfgV40;
        private CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLogInfo;
        private CHCNetSDK.NET_DVR_IPCHANINFO struChanInfo;
        private CHCNetSDK.NET_DVR_DEVICEINFO_V40 deviceInfo;

        public int GetDeviceLastError()
        {
            return (int)iLastErr;
        }

        public Int64 GetlUserID()
        {
            return lUserID;
        }

        public Int64 GetlRealHandle()
        {
            return lRealHandle;
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

        public HikVideoOperate()
        {
            /* 初始化可以放到公共代码 */
            //try
            //{
            //    isInitOK = CHCNetSDK.NET_DVR_Init(); // 初始化SDK
            //    // 启用日志文件写入接口
            //    CHCNetSDK.NET_DVR_SetLogToFile(3, "c:\\Sdklog\\", true);
            //    //设置连接时间与重连时间
            //    CHCNetSDK.NET_DVR_SetConnectTime(2000, 1);
            //    CHCNetSDK.NET_DVR_SetReconnect(10000, 1);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("摄像机初始化失败 => model: {0}, error: {1}", "hikvision", ex.Message);
            //}
        }

        #endregion

        #region 登录和退出

        /// <summary>
        /// 登陆设备
        /// </summary>
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
            struLogInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();

            //设备IP地址或者域名
            byte[] byIP = System.Text.Encoding.Default.GetBytes(ip);
            struLogInfo.sDeviceAddress = new byte[129];
            byIP.CopyTo(struLogInfo.sDeviceAddress, 0);

            //设备用户名
            byte[] byUserName = System.Text.Encoding.Default.GetBytes(UserNsme);
            struLogInfo.sUserName = new byte[64];
            byUserName.CopyTo(struLogInfo.sUserName, 0);

            //设备密码
            byte[] byPassword = System.Text.Encoding.Default.GetBytes(Password);
            struLogInfo.sPassword = new byte[64];
            byPassword.CopyTo(struLogInfo.sPassword, 0);

            //设备服务端口号
            struLogInfo.wPort = ushort.Parse(Port);

            if (fLoginCallBack == null)
            {
                fLoginCallBack = new CHCNetSDK.LOGINRESULTCALLBACK(cbLoginCallBack); //注册回调函数   
            }
            struLogInfo.cbLoginResult = fLoginCallBack; // 登陆回调
            struLogInfo.bUseAsynLogin = false; //是否异步登录：0- 否，1- 是

            deviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();

            // 登陆设备
            lUserID = CHCNetSDK.NET_DVR_Login_V40(ref struLogInfo, ref deviceInfo);
            if (lUserID < 0)
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                Console.WriteLine("NET_DVR_Login_V40 failed, error code= " + iLastErr);
            }
            else
            {
                Console.WriteLine("NET_DVR_Login_V40 success");

                dwAChanTotalNum = deviceInfo.struDeviceV30.byChanNum;
                dwDChanTotalNum = deviceInfo.struDeviceV30.byIPChanNum + 256 * (uint)deviceInfo.struDeviceV30.byHighDChanNum;

                // 初始化登录信息
                if (loginInfo == null)
                {
                    loginInfo = new VideoLoginInfo();
                    loginInfo.ChannelInfoList = new List<VideoChannelInfo>();
                }
                loginInfo.UserID = lUserID;
                loginInfo.RealHandle = -1;
                loginInfo.dwAChanTotalNum = dwAChanTotalNum;
                loginInfo.dwDChanTotalNum = dwDChanTotalNum;

                if (dwDChanTotalNum > 0)
                {
                    InfoIPChannel(); // 枚举IP通道
                }
                else
                {
                    // 枚举模拟通道
                    for (int i = 0; i < dwAChanTotalNum; i++)
                    {
                        ListAnalogChannel(i + 1, 1, i + (int)deviceInfo.struDeviceV30.byStartChan);
                        iChannelNum[i] = i + (int)deviceInfo.struDeviceV30.byStartChan;
                    }
                }
            }
            return lUserID;
        }

        /// <summary>
        /// 枚举IP通道
        /// </summary>
        /// <returns></returns>
        private void InfoIPChannel()
        {
            uint dwSize = (uint)Marshal.SizeOf(struIpParaCfgV40);
            IntPtr ptrIpParaCfgV40 = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(struIpParaCfgV40, ptrIpParaCfgV40, false);

            uint dwReturn = 0;
            int iGroupNo = 0;  //该Demo仅获取第一组64个通道，如果设备IP通道大于64路，需要按组号0~i多次调用NET_DVR_GET_IPPARACFG_V40获取

            if (!CHCNetSDK.NET_DVR_GetDVRConfig((int)lUserID, CHCNetSDK.NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, dwSize, ref dwReturn))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                Console.WriteLine("NET_DVR_GET_IPPARACFG_V40 failed, error code= " + iLastErr);
            }
            else
            {
                Console.WriteLine("NET_DVR_GET_IPPARACFG_V40 success!");

                struIpParaCfgV40 = (CHCNetSDK.NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(CHCNetSDK.NET_DVR_IPPARACFG_V40));

                for (int i = 0; i < dwAChanTotalNum; i++)
                {
                    int iChannelNum1 = i + (int)deviceInfo.struDeviceV30.byStartChan;
                    ListAnalogChannel(i + 1, struIpParaCfgV40.byAnalogChanEnable[i], iChannelNum1);
                    iChannelNum[i] = iChannelNum1;
                }

                byte byStreamType = 0;
                uint iDChanNum = 64;

                if (dwDChanTotalNum < 64)
                {
                    iDChanNum = dwDChanTotalNum; //如果设备IP通道小于64路，按实际路数获取
                }

                for (int i = 0; i < iDChanNum; i++)
                {
                    int channelNum = i + (int)struIpParaCfgV40.dwStartDChan;
                    iChannelNum[i + dwAChanTotalNum] = channelNum;
                    byStreamType = struIpParaCfgV40.struStreamMode[i].byGetStreamType;

                    dwSize = (uint)Marshal.SizeOf(struIpParaCfgV40.struStreamMode[i].uGetStream);

                    switch (byStreamType)
                    {
                        //目前NVR仅支持直接从设备取流 NVR supports only the mode: get stream from device directly
                        case 0:
                            IntPtr ptrChanInfo = Marshal.AllocHGlobal((Int32)dwSize);
                            Marshal.StructureToPtr(struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfo, false);
                            struChanInfo = (CHCNetSDK.NET_DVR_IPCHANINFO)Marshal.PtrToStructure(ptrChanInfo, typeof(CHCNetSDK.NET_DVR_IPCHANINFO));

                            //列出IP通道 List the IP channel
                            ListIPChannel(i + 1, struChanInfo.byEnable, struChanInfo.byIPID, channelNum);
                            iIPDevID[i] = struChanInfo.byIPID + struChanInfo.byIPIDHigh * 256 - iGroupNo * 64 - 1;

                            Marshal.FreeHGlobal(ptrChanInfo);
                            break;
                        default:
                            break;
                    }
                }
            }
            Marshal.FreeHGlobal(ptrIpParaCfgV40);
        }

        /// <summary>
        /// 添加IP通道
        /// </summary>
        private void ListIPChannel(Int32 iChanNo, byte byOnline, int byIPID, int iChannelNum)
        {
            VideoChannelInfo ChannelInfo = new VideoChannelInfo();
            ChannelInfo.lChannelNo = iChanNo;
            ChannelInfo.ChannelName = String.Format("HIK_IP {0}", iChanNo);
            ChannelInfo.Index = loginInfo.ChannelInfoList.Count;
            ChannelInfo.iChannelNum = iChannelNum;
            ChannelInfo.RealHandle = -1;
            ChannelInfo.PlayHandle = -1;
            ChannelInfo.PlayStatus = VideoChannelPlayStatus.None;
            if (byIPID == 0)
            {
                ChannelInfo.ConnectStatus = VideoChannelConnectStatus.DChan_Idle; //通道空闲，没有添加前端设备 the channel is idle                  
            }
            else
            {
                if (byOnline == 0)
                {
                    ChannelInfo.ConnectStatus = VideoChannelConnectStatus.DChan_Offline; //通道不在线 the channel is off-line
                }
                else
                {
                    ChannelInfo.ConnectStatus = VideoChannelConnectStatus.DChan_Online; //通道在线 The channel is on-line
                }
            }

            loginInfo.ChannelInfoList.Add(ChannelInfo);
        }

        /// <summary>
        /// 添加模拟通道
        /// </summary>
        private void ListAnalogChannel(Int32 iChanNo, byte byEnable, int iChannelNum)
        {
            VideoChannelInfo ChannelInfo = new VideoChannelInfo();
            ChannelInfo.ChannelName = String.Format("Camera {0}", iChanNo);
            ChannelInfo.lChannelNo = iChanNo;
            ChannelInfo.Index = loginInfo.ChannelInfoList.Count;
            ChannelInfo.iChannelNum = iChannelNum;
            ChannelInfo.RealHandle = -1;
            if (byEnable == 0)
            {
                ChannelInfo.ConnectStatus = VideoChannelConnectStatus.AChan_Disabled; //通道已被禁用 This channel has been disabled               
            }
            else
            {
                ChannelInfo.ConnectStatus = VideoChannelConnectStatus.AChan_Enabled; //通道处于启用状态 This channel has been enabled
            }
            loginInfo.ChannelInfoList.Add(ChannelInfo);
        }

        /// <summary>
        /// 登陆回调
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="dwResult"></param>
        /// <param name="lpDeviceInfo"></param>
        /// <param name="pUser"></param>
        public void cbLoginCallBack(int lUserID, int dwResult, IntPtr lpDeviceInfo, IntPtr pUser)
        {

        }

        /// <summary>
        /// 登出设备
        /// </summary>
        public void LoginOut()
        {
            if (lUserID >= 0)
            {
                // 撤销布防上传通道
                if (lFortifyHandle >= 0 && !CHCNetSDK.NET_DVR_CloseAlarmChan_V30((int)lFortifyHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                }
                // 暂停预览
                if (lRealHandle >= 0 && !CHCNetSDK.NET_DVR_StopRealPlay((int)lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                }
                // 暂停回放
                if (lPlayHandle >= 0 && !CHCNetSDK.NET_DVR_StopPlayBack((int)lPlayHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                }
                // 暂停通道预览和回放
                if (loginInfo.ChannelInfoList.Count > 0)
                {
                    foreach (var x in loginInfo.ChannelInfoList)
                    {
                        if (x.RealHandle >= 0 && !CHCNetSDK.NET_DVR_StopRealPlay((int)x.RealHandle))
                        {
                            iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        }
                        if (x.PlayHandle >= 0 && !CHCNetSDK.NET_DVR_StopPlayBack((int)x.PlayHandle))
                        {
                            iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        }
                    }
                }
                // 注销用户
                if (!CHCNetSDK.NET_DVR_Logout((int)lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                }
                // 释放SDK资源
                if (!CHCNetSDK.NET_DVR_Cleanup())
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                }
            }
        }

        #endregion

        #region 车牌识别

        /// <summary>
        /// 布防
        /// </summary>
        public void SetAlarm()
        {
            CHCNetSDK.NET_DVR_SETUPALARM_PARAM m_struSetupParam = new CHCNetSDK.NET_DVR_SETUPALARM_PARAM();
            m_struSetupParam.dwSize = (uint)Marshal.SizeOf(m_struSetupParam); // 结构体大小
            m_struSetupParam.byLevel = 1; // 布防优先级：0 - 一等级（高），1 - 二等级（中），2 - 三等级（低） 
            m_struSetupParam.byAlarmInfoType = 1; // 智能交通报警信息上传类型：0 - 老报警信息（NET_DVR_PLATE_RESULT），1 - 新报警信息(NET_ITS_PLATE_RESULT)
            lFortifyHandle = CHCNetSDK.NET_DVR_SetupAlarmChan_V41((int)lUserID, ref m_struSetupParam);
            if (lFortifyHandle < 0)
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                LoginOut(); // 登出
            }
        }

        /// <summary>
        /// 取消事件订阅
        /// </summary>
        public void StopAlarm()
        {
            // 撤销布防上传通道
            if (lFortifyHandle >= 0 && !CHCNetSDK.NET_DVR_CloseAlarmChan_V30((int)lFortifyHandle))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
            }
        }

        /// <summary>
        /// 控制道闸
        /// 控制参数：0- 关闭道闸，1- 开启道闸，2- 停止道闸，3- 锁定道闸，4- 解锁道闸 
        /// </summary>
        /// <param name="BarrierGateCtrl"></param>
        public bool ControlGate(byte BarrierGateCtrl)
        {
            bool openState = false;
            CHCNetSDK.NET_DVR_BARRIERGATE_CFG BarrierGateInfo = new CHCNetSDK.NET_DVR_BARRIERGATE_CFG();
            BarrierGateInfo.dwSize = 24;
            BarrierGateInfo.byBarrierGateCtrl = BarrierGateCtrl;
            BarrierGateInfo.byLaneNo = 1; //道闸号：0- 表示无效值(设备需要做有效值判断)，1- 道闸1 
            BarrierGateInfo.dwChannel = 1;
            BarrierGateInfo.byEntranceNo = 1; //出入口编号，取值范围：[1,8] 
            //启用解锁使能：0 - 不启用，1 - 启用
            //如果设备不支持byUnlock字段，该参数赋值为0，byBarrierGateCtrl控制参数取值“0 - 关闭道闸、1 - 开启道闸、2 - 停止道闸”中任何一种操作皆可进行解锁；
            //如果设备支持byUnlock字段，解锁时需要将参数赋值为1，并结合byBarrierGateCtrl赋值为“4 - 解锁道闸”来进行解锁，byUnlock为1时其他操作均不能解锁
            if (BarrierGateCtrl == 4)
            {
                BarrierGateInfo.byUnlock = 1;
            }
            else
            {
                BarrierGateInfo.byUnlock = 0;
            }
            BarrierGateInfo.byRes = new byte[12]; //保留，置为0
            uint dwSize1 = (uint)Marshal.SizeOf(BarrierGateInfo);
            IntPtr ptrAcsParam = Marshal.AllocHGlobal((int)dwSize1);
            Marshal.StructureToPtr(BarrierGateInfo, ptrAcsParam, false);
            openState = CHCNetSDK.NET_DVR_RemoteControl((int)lUserID, (uint)3128, ptrAcsParam, dwSize1);
            iLastErr = CHCNetSDK.NET_DVR_GetLastError();
            return openState;
        }

        #endregion

        #region 预览

        /// <summary>
        /// 实时预览
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="lChannel"></param>
        /// <param name="RealPlayWnd"></param>
        /// <param name="lRealHandle"></param>
        /// <returns></returns>
        public bool RealPlay(Int16 lChannel, IntPtr RealPlayWnd, out Int64 lRealHandle)
        {
            if (lUserID < 0)
            {
                lRealHandle = -1;
                return false;
            }
            CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            lpPreviewInfo.hPlayWnd = RealPlayWnd;//预览窗口
            lpPreviewInfo.lChannel = lChannel;//预te览的设备通道
            lpPreviewInfo.dwStreamType = 1;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
            lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
            lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
            lpPreviewInfo.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数
            lpPreviewInfo.byProtoType = 0;
            lpPreviewInfo.byPreviewMode = 0;
            //if (RealData == null)
            //{
            //    RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
            //}
            IntPtr pUser = new IntPtr();//用户数据 //打开预览 Start live view 
            lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40((int)lUserID, ref lpPreviewInfo, null, pUser);
            if (lRealHandle < 0)
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                Console.WriteLine("NET_DVR_RealPlay_V40 failed, error code= " + iLastErr);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 停止实时预览
        /// </summary>
        /// <param name="m_lUserID"></param>
        /// <param name="lRealHandle"></param>
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
            if (!CHCNetSDK.NET_DVR_StopRealPlay((int)lRealHandle))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                Console.WriteLine("NET_DVR_StopRealPlay failed, error code= " + iLastErr);
                return false;
            }
            lRealHandle = -1;
            return true;
        }

        /// <summary>
        /// 云台控制
        /// </summary>
        /// <param name="dwPTZCommand"></param>
        /// <param name="Speed"></param>
        /// <param name="dwStop"></param>
        /// <param name="lRealHandle"></param>
        /// <param name="m_lUserID"></param>
        /// <param name="lChannel"></param>
        /// <returns></returns>
        public bool DeviceControl(int dwPTZCommand, int Speed, int dwStop, Int64 lRealHandle = -1, int lChannel = -1)
        {
            bool result = false;
            if (lRealHandle != -1) //已开始预览
            {
                if (Speed > 7)
                {
                    Speed = 7;
                }
                else if (Speed < 0)
                {
                    Speed = 0;
                }
                if (CHCNetSDK.NET_DVR_PTZControlWithSpeed((int)lRealHandle, (uint)dwPTZCommand, (uint)dwStop, (uint)Speed))
                {
                    result = true;
                }
                else
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                }
            }
            else
            {
                if (CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other((int)lUserID, lChannel, dwPTZCommand, dwStop, Speed))
                {
                    result = true;
                }
                else
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                }
            }
            return result;
        }

        #endregion

        #region 抓拍

        /// <summary>
        /// 缓存照片
        /// </summary>
        /// <param name="iChannelNum"></param>
        /// <param name="ipSavePath"></param>
        /// <returns></returns>
        public bool JPEGCapturePicture(int iChannelNum, ushort wPicQuality, ushort wPicSize, string ipSavePath)
        {
            uint dwSizeReturned = 0;
            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = wPicQuality; //图像质量 Image quality
            lpJpegPara.wPicSize = wPicSize; //抓图分辨率 Picture size: 0xff-Auto(使用当前码流分辨率) 
                                            //抓图分辨率需要设备支持，更多取值请参考SDK文档
                                            //JEPG抓图，数据保存在缓冲区中 Capture a JPEG picture and save in the buffer
            uint iBuffSize = 1024 * 1024 * 10; //缓冲区大小需要不小于一张图片数据的大小 The buffer size should not be less than the picture size
            byte[] byJpegPicBuffere = new byte[iBuffSize];
            //JPEG抓图保存成文件 Capture a JPEG picture
            if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture_NEW((int)lUserID, iChannelNum, ref lpJpegPara, byJpegPicBuffere, iBuffSize, ref dwSizeReturned))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            else
            {
                //将缓冲区里的JPEG图片数据写入文件 save the data into a file
                FileStream fs = new FileStream(ipSavePath, FileMode.Create);
                int iLen = (int)dwSizeReturned;
                // byte[] bytes = Convert.FromBase64String(byJpegPicBuffer);
                //给全局变量赋图片缓存，后续保存成功清除缓存
                fs.Write(byJpegPicBuffere, 0, iLen);
                fs.Close();

                return true;
            }
        }

        public void JPEGCapturePicture(int iChannelNum, ushort wPicQuality, ushort wPicSize, out byte[] byJpegPicBuffere, out uint dwSizeReturned)
        {
            dwSizeReturned = 0;
            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = wPicQuality; //图像质量 Image quality
            lpJpegPara.wPicSize = wPicSize; //抓图分辨率 Picture size: 0xff-Auto(使用当前码流分辨率) 
                                            //抓图分辨率需要设备支持，更多取值请参考SDK文档
                                            //JEPG抓图，数据保存在缓冲区中 Capture a JPEG picture and save in the buffer
            uint iBuffSize = 1024 * 1024 * 10; //缓冲区大小需要不小于一张图片数据的大小 The buffer size should not be less than the picture size
            byJpegPicBuffere = new byte[iBuffSize];
            //JPEG抓图保存成文件 Capture a JPEG picture
            CHCNetSDK.NET_DVR_CaptureJPEGPicture_NEW((int)lUserID, iChannelNum, ref lpJpegPara, byJpegPicBuffere, iBuffSize, ref dwSizeReturned);
        }

        #endregion

        #region 回放

        /// <summary>
        /// 回放控制
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <param name="dwControlCode">控制码</param>
        /// <returns></returns>
        public bool PlayBackControl(int dwControlCode, Int64 lPlayHandle)
        {
            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40((int)lPlayHandle, (uint)dwControlCode, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 按照时间回放
        /// </summary>
        /// <param name="startTime">回放开始时间</param>
        /// <param name="endTime">回放结束时间</param>
        /// <param name="iChannelNum">通道号</param>
        /// <param name="RealPlayWnd">显示控件句柄</param>
        /// <param name="PlayHandle">回放句柄</param>
        /// <returns></returns>
        public bool PlayBackByTime(DateTime? startTime, DateTime? endTime, int iChannelNum, IntPtr RealPlayWnd, out Int64 lPlayHandle)
        {
            CHCNetSDK.NET_DVR_VOD_PARA struVodPara = new CHCNetSDK.NET_DVR_VOD_PARA();
            struVodPara.dwSize = (uint)Marshal.SizeOf(struVodPara);
            struVodPara.struIDInfo.dwChannel = (uint)iChannelNum; //通道号 Channel number  
            struVodPara.hWnd = RealPlayWnd;//回放窗口句柄

            //设置回放的开始时间 Set the starting time to search video files
            struVodPara.struBeginTime.dwYear = (uint)startTime.Value.Year;
            struVodPara.struBeginTime.dwMonth = (uint)startTime.Value.Month;
            struVodPara.struBeginTime.dwDay = (uint)startTime.Value.Day;
            struVodPara.struBeginTime.dwHour = (uint)startTime.Value.Hour;
            struVodPara.struBeginTime.dwMinute = (uint)startTime.Value.Minute;
            struVodPara.struBeginTime.dwSecond = (uint)startTime.Value.Second;

            //设置回放的结束时间 Set the stopping time to search video files
            struVodPara.struEndTime.dwYear = (uint)endTime.Value.Year;
            struVodPara.struEndTime.dwMonth = (uint)endTime.Value.Month;
            struVodPara.struEndTime.dwDay = (uint)endTime.Value.Day;
            struVodPara.struEndTime.dwHour = (uint)endTime.Value.Hour;
            struVodPara.struEndTime.dwMinute = (uint)endTime.Value.Minute;
            struVodPara.struEndTime.dwSecond = (uint)endTime.Value.Second;

            //按时间回放 Playback by time
            lPlayHandle = CHCNetSDK.NET_DVR_PlayBackByTime_V40((int)lUserID, ref struVodPara);
            if (lPlayHandle < 0)
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            if (!PlayBackControl(CHCNetSDK.NET_DVR_PLAYSTART, lPlayHandle))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 声音控制
        /// </summary>
        /// <param name="bSound">true 声音开, false 声音关</param>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        public bool PlayBackSoundControl(bool bSound, Int64 lPlayHandle)
        {
            if (lPlayHandle < 0)
            {
                return false;
            }

            int dwControlCode = bSound ? CHCNetSDK.NET_DVR_PLAYSTARTAUDIO : CHCNetSDK.NET_DVR_PLAYSTOPAUDIO;
            if (!PlayBackControl(dwControlCode, lPlayHandle))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 暂停回放
        /// </summary>
        /// <param name="bPause">true 暂停, false 继续</param>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        public bool PausePlayBack(bool bPause, Int64 lPlayHandle)
        {
            if (lPlayHandle < 0)
            {
                return false;
            }

            int dwControlCode = bPause ? CHCNetSDK.NET_DVR_PLAYPAUSE : CHCNetSDK.NET_DVR_PLAYRESTART;
            if (!PlayBackControl(dwControlCode, lPlayHandle))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 结束回放
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        public bool StopPlayBack(Int64 lPlayHandle)
        {
            if (!CHCNetSDK.NET_DVR_StopPlayBack((int)lPlayHandle))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取回放进度
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        public int GetPlaybackPos(Int64 lPlayHandle)
        {
            int PosValue = 0;
            uint iOutValue = 0;
            IntPtr lpOutBuffer = Marshal.AllocHGlobal(4);
            //获取回放进度
            CHCNetSDK.NET_DVR_PlayBackControl_V40((int)lPlayHandle, CHCNetSDK.NET_DVR_PLAYGETPOS, IntPtr.Zero, 0, lpOutBuffer, ref iOutValue);
            PosValue = (int)Marshal.PtrToStructure(lpOutBuffer, typeof(int));
            if (PosValue == 100)  //回放结束
            {
                // StopPlayBack(lPlayHandle);
            }

            if (PosValue == 200) //网络异常，回放失败
            {
                // PosValue = -1;
            }
            Marshal.FreeHGlobal(lpOutBuffer);
            return PosValue;
        }

        /// <summary>
        /// 慢放
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        public bool PlayBackSlow(Int64 lPlayHandle)
        {
            uint iOutValue = 0;

            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40((int)lPlayHandle, CHCNetSDK.NET_DVR_PLAYSLOW, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
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
            uint iOutValue = 0;

            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40((int)lPlayHandle, CHCNetSDK.NET_DVR_PLAYNORMAL, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
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
            uint iOutValue = 0;

            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40((int)lPlayHandle, CHCNetSDK.NET_DVR_PLAYFAST, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
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
            uint iOutValue = 0;

            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40((int)lPlayHandle, CHCNetSDK.NET_DVR_PLAYFRAME, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <param name="sBmpPicFileName">图片保存路径和文件名</param>
        /// <returns></returns>
        public bool PlayBackBMP(Int64 lPlayHandle, string sBmpPicFileName = "test.bmp")
        {
            if (lPlayHandle < 0)
            {
                return false;
            }

            if (!CHCNetSDK.NET_DVR_PlayBackCaptureFile((int)lPlayHandle, sBmpPicFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return false;
            }
            return true;
        }

        #endregion

        #region 硬件通用接口

        #region Useless

        /// <summary>
        /// 初始化色设备
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
