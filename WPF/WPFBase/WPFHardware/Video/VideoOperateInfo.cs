using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFHardware.Base;
using WPFHardware.Video.Constants;

namespace WPFHardware.Video
{
    /// <summary>
    /// 摄像头操作资源
    /// </summary>
    public class VideoOperateInfo
    {
        /// <summary>
        /// 摄像机IP
        /// </summary>
        public string IP;
        /// <summary>
        /// 摄像机端口
        /// </summary>
        public string Port;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password;
        /// <summary>
        /// 厂家
        /// </summary>
        public string Factory;
        /// <summary>
        /// 摄像头设备编号
        /// </summary>
        public string DeviceNo;
        /// <summary>
        /// 设备位置
        /// </summary>
        public EnumDirection Position;
        /// <summary>
        /// 登录ID
        /// </summary>
        public Int64 UserID = -1;
        /// <summary>
        /// 厂商
        /// </summary>
        public VideoSdkType SDKType;
        /// <summary>
        /// 摄像头功能 1 车牌识别 2 监控 3 监控+抓拍
        /// </summary>
        public VideoWorkType WorkType;
        /// <summary>
        /// 摄像头状态
        /// </summary>
        public EnumDevicestatus DeviceStatus;
        /// <summary>
        /// 操作类
        /// </summary>
        public IVideoOperate Operate;
    }
}
