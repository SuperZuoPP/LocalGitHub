using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHardware.Base
{
    /// <summary>
    /// 设备状态
    /// </summary>
    public enum EnumDevicestatus
    {
        Disable,                //停用
        Working,                //启用 
        Disconnected,           //未连接
        Gate_Opening,           //道闸_抬竿中
        Gate_Closed,            //道闸_已关闭
        Gate_Lock,              //道闸_锁定
        Gate_UnLock,            //道闸_解除锁定
        Weigh_NoZero,           //衡器_仪表不能自动校零
        Video_Ready,            //摄像头_就绪
        Video_Logout,           //摄像头_退出
        Video_Monitoring,       //摄像头_布防
        Video_Error             //摄像头_错误
    }


    /// <summary>
    /// 相对磅房方位
    /// </summary>
    public enum EnumDirection
    {
        UnKnown = 0,
        LEFT,
        RIGHT,
        FRONT,
        BACK
    }

}
