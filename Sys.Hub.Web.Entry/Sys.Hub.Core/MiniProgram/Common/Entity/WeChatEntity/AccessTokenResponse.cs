using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Common.WeChatEntity
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/18 22:55:21 
    /// 描    述 ：  AccessToken 相应实体
    /// </summary>
    public class AccessTokenResponse
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒。目前是7200秒之内的值。
        /// </summary>
        public double expires_in { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public double errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
}
