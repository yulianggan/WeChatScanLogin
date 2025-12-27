using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Common
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/16 21:44:40 
    /// 描    述 ：  默认实体，存储表的默认字段
    /// </summary>
    public class DefaultEntity
    {
        #region 默认字段

        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("F_ID")]
        public string F_ID { get; set; }

        /// <summary>
        /// 有效标志0否1是
        /// </summary>
        [Column("F_ENABLEDMARK")]
        public int? F_EnabledMark { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [Column("F_CREATEUSERID")]
        public string? F_CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("F_CREATEDATE")]
        public DateTime F_CreateDate { get; set; }

        /// <summary>
        /// 编辑人ID
        /// </summary>
        [Column("F_MODIFYUSERID")]
        public string? F_ModifyUserId { get; set; }

        /// <summary>
        /// 编辑日期
        /// </summary>
        [Column("F_MODIFYDATE")]
        public DateTime? F_ModifyDate { get; set; }

        /// <summary>
        /// 删除标记0否1是
        /// </summary>
        [Column("F_DELETEMARK")]
        public int F_DeleteMark { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        [Column("F_DESCRIPTION")]
        public string? F_Description { get; set; }

        #endregion


        #region 扩展操作

        /// <summary>
        /// 新增调用
        /// </summary>
        /// <param name="CreateUserId">创建人ID</param>
        public void CreateInit(string? CreateUserId = "")
        {
            this.F_ID = Guid.NewGuid().ToString();
            this.F_CreateDate = DateTime.Now;
            this.F_CreateUserId = CreateUserId;
            this.F_EnabledMark = 1;
        }


        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <param name="ModifyUserId">修改人ID</param>
        public void ModifyInit(string? ModifyUserId = "")
        {
            this.F_ModifyDate = DateTime.Now;
            this.F_ModifyUserId = ModifyUserId;
        }
        #endregion

    }

}
