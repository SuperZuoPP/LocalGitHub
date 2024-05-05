using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHardware.Video.Constants
{
    /// <summary>
    /// 摄像头功能 
    /// 1 车牌识别 
    /// 2 监控 
    /// 3 监控和抓拍 
    /// 4 硬盘录像机
    /// </summary>
    public enum VideoWorkType
    {
        PlateCognition = 1,     // 车牌识别
        Monitor = 2,            // 监控
        MonitorCapture = 3,     // 监控和抓拍
        DVR = 4                 // 硬盘录像机
    }
}
