using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Common.Entity
{

    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/17 13:19:22 
    /// 描    述 ：  无分页请求参数实体
    /// </summary>
    public class DtoEntity<T> where T : new()
    {
        public DtoEntity()
        {
            this.Param = new T();
            this.UserID = "";
            this.ExtensionsParams = new List<KeyValueEntity>();
        }

        /// <summary>
        /// 参数实体（一版情况下，都是使用实体字段进行查询）
        /// </summary>
        public T Param { get; set; }

        /// <summary>
        /// 扩展参数， 实体之外的参数
        /// </summary>
        public List<KeyValueEntity> ExtensionsParams { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

    }

    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/17 13:19:22 
    /// 描    述 ：  有分页请求参数实体
    /// </summary>
    public class DtoPageEntity<T> : DtoEntity<T> where T : new()
    {
        public DtoPageEntity() : base()
        {
            this.PageParams = new PagedList();

        }
        /// <summary>
        /// 需要分页的请求参数
        /// </summary>
        public PagedList PageParams { get; set; }
    }


}
