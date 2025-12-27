using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Common.Entity
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/17 14:06:40 
    /// 描    述 ：  价值对实体
    /// </summary>
    public class KeyValueEntity
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; } = "";
    }
}
