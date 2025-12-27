using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Common.WeChatEntity
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/18 22:55:09 
    /// 描    述 ：  
    /// </summary>
    public class WechatUserProxyEntity
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public string userInfo { get; set; }

        /// <summary>
        /// 临时会话code
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 小程序标识ID
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string? F_DeviceID { get; set; }
    }
}
