using Sys.Hub.Core.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Util.Extensions
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/17 14:45:37 
    /// 描    述 ：  获取键值对的Value
    /// </summary>
    public static class GetEnumerableParamsValue
    {
        /// <summary>
        /// 键值对集合拓展函数
        /// </summary>
        /// <param name="List">键值对集合</param>
        /// <param name="Key">键</param>
        /// <returns></returns>
        public static string? GetValue(this List<KeyValueEntity> List, string Key)
        {
            return List.FirstOrDefault(x => x.Key == Key)?.Value;
        }
    }
}
