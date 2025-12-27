using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Common.WeChatEntity
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/18 22:54:51 
    /// 描    述 ：  微信登录获取 code2Session
    /// </summary>
    public class Code2SessionRequest
    {
        /// <summary>
        /// 小程序 appId
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 小程序 appSecret
        /// </summary>
        public string secret { get; set; }

        /// <summary>
        /// 登录时获取的 code
        /// </summary>
        public string js_code { get; set; }

    }
}
