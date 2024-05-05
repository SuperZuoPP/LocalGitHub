using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHardware.Video.Constants
{
    /// <summary>
    /// 通道状态
    /// </summary>
    public enum VideoChannelConnectStatus
    {
        AChan_Disabled = 0,   // 模拟通道禁用
        AChan_Enabled,        // 模拟通道启用
        DChan_Idle,           // IP通道空闲
        DChan_Offline,        // IP通道离线
        DChan_Online          // IP通道在线
    }

    /// <summary>
    /// 通道播放状态
    /// </summary>
    public enum VideoChannelPlayStatus
    {
        None = 0,             // 无
        Preview,              // 预览
        PlayBack,             // 回放
        PlayBack_Pause,       // 回放暂停
        Downloading,          // 下载
        Error                 // 错误
    }
}
