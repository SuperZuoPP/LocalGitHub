using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFHardware.Video.Constants;

namespace WPFHardware.Video
{
    public class VideoChannelInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 通道连接状态
        /// </summary>
        public VideoChannelConnectStatus ConnectStatus { get; set; }
        /// <summary>
        /// 通道播放状态
        /// </summary>
        public VideoChannelPlayStatus PlayStatus { get; set; }
        /// <summary>
        /// 预览句柄
        /// </summary>
        public Int64 RealHandle { get; set; }
        /// <summary>
        /// 回访句柄
        /// </summary>
        public Int64 PlayHandle { get; set; }
        /// <summary>
        /// 通道索引
        /// </summary>
        public int lChannelNo { get; set; }
        /// <summary>
        /// 通道
        /// </summary>
        public int iChannelNum { get; set; }

        /// <summary>
        /// 是否打开声音
        /// </summary>
        public bool IsOpenSound { get; set; }
        /// <summary>
        /// 获取通道状态描述
        /// </summary>
        /// <returns></returns>
        public static string GetChannelStatusDesc(VideoChannelConnectStatus channelStatus)
        {
            switch (channelStatus)
            {
                case VideoChannelConnectStatus.AChan_Disabled: return "禁用";
                case VideoChannelConnectStatus.AChan_Enabled: return "启用";
                case VideoChannelConnectStatus.DChan_Idle: return "空闲";
                case VideoChannelConnectStatus.DChan_Offline: return "离线";
                case VideoChannelConnectStatus.DChan_Online: return "在线";
                default: return "空闲";
            }
        }
    }
}
