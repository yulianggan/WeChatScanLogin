using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Common.Entity
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/17 13:26:17 
    /// 描    述 ：  响应实体
    /// </summary>
    public class ResponseEntity
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public Object? Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string? Message { get; set; }
    }

    /// <summary>
    /// 分页响应实体
    /// </summary>
    public class ResponsePageEntity : ResponseEntity
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        public PagedList? PageData { get; set; }

    }

}
