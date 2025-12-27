using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Common.WeChatEntity
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/18 22:54:59 
    /// 描    述 ：  微信二维码请求实体
    /// </summary>
    public class QRCodeResquest
    {
        /// <summary>
        /// 【必填】场景
        /// 最大32个可见字符，只支持数字，大小写英文以及部分特殊字符：
        /// !#$&'()*+,/:;=?@-._~，其它字符请自行编码为合法字符（因不支持%，中文无法使用 urlencode 处理，请使用其他编码方式）
        /// </summary>
        public string scene { get; set; } = "Login";

        /// <summary>
        /// 必须是已经发布的小程序存在的页面（否则报错），例如 pages/index/index, 根路径前不要填加 /,不能携带参数
        /// （参数请放在scene字段里），如果不填写这个字段，默认跳主页面
        /// </summary>
        public string page { get; set; }

        /// <summary>
        /// 二维码的宽度，单位 px。最小 280px，最大 1280px
        /// </summary>
        public int width { get; set; } = 100;

        /// <summary>
        /// 是否需要透明底色，为 true 时，生成透明底色的小程序码
        /// </summary>
        public bool is_hyaline { get; set; }

    }
}
