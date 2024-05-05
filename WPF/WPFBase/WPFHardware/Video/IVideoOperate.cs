using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFHardware.Interfaces;

namespace WPFHardware.Video
{
    public interface IVideoOperate : IDevice
    {
        #region 登录和退出

        /// <summary>
        /// 登陆设备
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Int64 Login(string ip, string port, string userName, string password);
        /// <summary>
        /// 推出设备
        /// </summary>
        /// <returns></returns>
        void LoginOut();

        #endregion

        #region 事件订阅

        /// <summary>
        /// 布防
        /// </summary>
        void SetAlarm();
        /// <summary>
        /// 取消事件订阅
        /// </summary>
        void StopAlarm();

        #endregion

        #region 预览和设备控制

        /// <summary>
        /// 实时预览
        /// </summary>
        /// <param name="lChannel">通道号</param>
        /// <param name="RealPlayWnd">显示控件句柄</param>
        /// <param name="lRealHandle">预览句柄</param>
        /// <returns></returns>
        bool RealPlay(short lChannel, IntPtr RealPlayWnd, out Int64 lRealHandle);
        /// <summary>
        /// 停止实时预览
        /// </summary>
        /// <param name="lRealHandle">预览句柄</param>
        /// <returns></returns>
        bool StopRealPlay(Int64 lRealHandle);
        /// <summary>
        /// 道闸控制  //控制参数：0- 关闭道闸，1- 开启道闸，2- 停止道闸，3- 锁定道闸，4- 解锁道闸 
        /// </summary>
        /// <returns></returns>
        bool ControlGate(byte BarrierGateCtrl);
        /// <summary>
        /// 云台控制
        /// </summary>
        /// <param name="dwPTZCommand"></param>
        /// <param name="Speed"></param>
        /// <param name="dwStop"></param>
        /// <param name="lRealHandle"></param>
        /// <param name="lUserID"></param>
        /// <param name="lChannel"></param>
        /// <returns></returns>
        bool DeviceControl(int dwPTZCommand, int Speed, int dwStop, Int64 lRealHandle = -1, int lChannel = -1);

        #endregion

        #region 获取状态
        /// <summary>
        /// 获取设备错误码
        /// </summary>
        /// <returns></returns>
        int GetDeviceLastError();
        /// <summary>
        /// 获取登录ID
        /// </summary>
        /// <returns></returns>
        Int64 GetlUserID();
        /// <summary>
        /// 获取预览句柄
        /// </summary>
        /// <returns></returns>
        Int64 GetlRealHandle();
        /// <summary>
        /// 获取回放句柄
        /// </summary>
        /// <returns></returns>
        Int64 GetlPlayHandle();
        /// <summary>
        /// 获取事件订阅句柄
        /// </summary>
        /// <returns></returns>
        Int64 GetlFortifyHandle();
        /// <summary>
        /// 获取设备登陆信息
        /// </summary>
        /// <returns></returns>
        VideoLoginInfo GetVideoLoginInfo();

        #endregion

        #region 抓图

        /// <summary>
        /// 抓拍
        /// </summary>
        /// <param name="lChannel">通道</param>
        /// <param name="wPicQuality">图片质量</param>
        /// <param name="wPicSize">图片分辨率</param>
        /// <param name="savePath">保存位置</param>
        /// <returns></returns>
        bool JPEGCapturePicture(int lChannel, ushort wPicQuality, ushort wPicSize, string savePath);
        /// <summary>
        /// 抓拍
        /// </summary>
        /// <param name="lChannel"></param>
        /// <param name="wPicQuality"></param>
        /// <param name="wPicSize"></param>
        /// <param name="byJpegPicBuffere"></param>
        /// <param name="dwSizeReturned"></param>
        void JPEGCapturePicture(int lChannel, ushort wPicQuality, ushort wPicSize, out byte[] byJpegPicBuffere, out uint dwSizeReturned);

        #endregion

        #region 回放

        /// <summary>
        /// 回放控制
        /// </summary>
        /// <param name="lPlayHandle"></param>
        /// <param name="dwControlCode"></param>
        /// <returns></returns>
        bool PlayBackControl(int dwControlCode, Int64 lPlayHandle);
        /// <summary>
        /// 按时间回放
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="iChannelNum">通道号</param>
        /// <param name="RealPlayWnd">显示控件句柄</param>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        bool PlayBackByTime(DateTime? startTime, DateTime? endTime, int iChannelNum, IntPtr RealPlayWnd, out Int64 lPlayHandle);
        /// <summary>
        /// 声音控制
        /// </summary>
        /// <param name="bSound">是否打开声音</param>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        bool PlayBackSoundControl(bool bSound, Int64 lPlayHandle);
        /// <summary>
        /// 结束回放
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        bool StopPlayBack(Int64 lPlayHandle);
        /// <summary>
        /// 暂停回放
        /// </summary>
        /// <param name="bPause">是否暂停</param>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        bool PausePlayBack(bool bPause, Int64 lPlayHandle);
        /// <summary>
        /// 获取回放进度条
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        int GetPlaybackPos(Int64 lPlayHandle);
        /// <summary>
        /// 慢放
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        bool PlayBackSlow(Int64 lPlayHandle);
        /// <summary>
        /// 正常
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        bool PlayBackNormal(Int64 lPlayHandle);
        /// <summary>
        /// 快放
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        bool PlayBackFast(Int64 lPlayHandle);
        /// <summary>
        /// 单帧
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <returns></returns>
        bool PlayBackFrame(Int64 lPlayHandle);
        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="lPlayHandle">回放句柄</param>
        /// <param name="sBmpPicFileName">图片保存路径和文件名</param>
        /// <returns></returns>
        bool PlayBackBMP(Int64 lPlayHandle, string sBmpPicFileName = "test.bmp");

        #endregion
    }
}
