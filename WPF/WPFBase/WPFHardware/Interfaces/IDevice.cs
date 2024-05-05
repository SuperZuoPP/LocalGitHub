using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHardware.Interfaces
{
    public interface IDevice
    {
        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <returns>成功：true，失败：false</returns>
        int Init();

        /// <summary>
        /// 启动设备
        /// </summary>
        /// <returns>成功：true，失败：false</returns>
        int Launch();

        /// <summary>
        /// 重启设备
        /// </summary>
        /// <returns>成功：true，失败：false</returns>
        int Restart();

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns>成功：true，失败：false</returns>
        int Shutdown();

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        int Dispose();
    }
}
