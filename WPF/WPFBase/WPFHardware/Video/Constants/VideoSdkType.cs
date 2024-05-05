using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHardware.Video.Constants
{
    /// <summary>
    /// SDK类型
    /// </summary>
    public enum VideoSdkType
    {
        [Description("未知")]
        Unknown = -1,
        [Description("海康威视")]
        HikVision = 0,
        [Description("大华")]
        DaHua = 1
    }
}
