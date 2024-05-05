using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHardware.Video
{
    public class VideoLoginInfo
    {
        /// <summary>
        /// 回放句柄
        /// </summary>
        public Int64 PlayHandle { get; set; }
        /// <summary>
        /// 播放句柄
        /// </summary>
        public Int64 RealHandle { get; set; }
        /// <summary>
        /// 登录id
        /// </summary>
        public Int64 UserID { get; set; }
        /// <summary>
        /// 模拟通道总数
        /// </summary>
        public uint dwAChanTotalNum { get; set; }
        /// <summary>
        /// IP通道总数
        /// </summary>
        public uint dwDChanTotalNum { get; set; }
        /// <summary>
        /// 设备通道列表
        /// </summary>
        public List<VideoChannelInfo> ChannelInfoList { get; set; }
    }
}
