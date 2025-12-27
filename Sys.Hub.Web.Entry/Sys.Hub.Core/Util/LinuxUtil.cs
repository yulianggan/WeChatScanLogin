using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Util
{
    /// <summary>
    /// 类 名 称 ：  LinuxUtil
    /// 创 建 人 ：  taojian
    /// 创建时间 ：  2022/8/18 11:35:36 
    /// 描     述 ： 主要提供一些Liunx 和 Windows 下表现差异转换， 例如：路径， 串口等等
    /// </summary>
    public static class LinuxUtil
    {
        #region 路径处理

        /// <summary>
        /// 路径转换
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetRuntimeDirectory(string path)
        {
            //ForLinux
            if (IsLinuxRunTime())
                return GetLinuxDirectory(path);
            //ForWindows
            if (IsWindowRunTime())
                return GetWindowDirectory(path);
            return path;
        }

        /// <summary>
        /// OSPlatform.Windows监测运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowRunTime()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        /// <summary>
        /// OSPlatform.Linux运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsLinuxRunTime()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        /// <summary>
        /// 转换为Linux路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetLinuxDirectory(string path)
        {
            string pathTemp = Path.Combine(path);
            return pathTemp.Replace(@"\", @"/");
        }

        /// <summary>
        /// 转换为Window路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetWindowDirectory(string path)
        {
            string pathTemp = Path.Combine(path);
            return pathTemp.Replace(@"/", @"\");
        }

        #endregion

        #region 串口/Com 口处理

        /// <summary>
        /// 根据操作系统自动转换串口
        /// Linux: 串口1 是 /dev/ttyS0
        /// windows: 串口1 是 COM1
        /// </summary>
        /// <param name="Com">COM 标识</param>
        /// <returns></returns>
        public static string ConvertCOMBySys(string Com)
        {
            //获取当前操作系统是否是Linux 系统
            var linux = IsLinuxRunTime();
            if (linux)
            {
                //linux 系统（只转换COM的写法， /dev/ttyS0 不管）
                if (Com.Contains("COM"))
                {
                    return "/dev/ttyS" + (Convert.ToInt16(Com.Substring(3)) - 1).ToString();
                }
            }
            else
            {
                //windows 系统 （只转换/dev/ttyS0的写法，COM 不管）
                if (Com.Contains("/dev/ttyS"))
                {
                    return "COM" + (Convert.ToInt16(Com.Substring(9)) + 1).ToString();
                }
            }
            return Com;
        }


        #endregion
    }
}
