using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHardware.Video.Constants
{
    /// <summary>
    /// SDK初始化类型
    /// </summary>
    public enum VideoSdkInitType
    {
        HikVisionOnly = 0, // 只海康
        DahuaOnly,         // 只大华
        Mix                // 混合
    }
}
