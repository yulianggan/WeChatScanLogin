using Furion.DatabaseAccessor;
using Sys.Hub.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Hub.Core.MiniProgram.Common.Entity
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/19 0:31:25 
    /// 描    述 ：  微信用户表  
    /// </summary>
    [Table("FF_WeChatUser")]
    public class WeChatUserEntity : DefaultEntity, IEntity
    {
        /// <summary> 
        /// OpenId 
        /// </summary> 
        /// <returns></returns> 
        [Column("OPENID")]
        public string? OpenId { get; set; }

        /// <summary> 
        /// AppId 
        /// </summary> 
        /// <returns></returns> 
        [Column("APPID")]
        public string? AppId { get; set; }

        /// <summary> 
        /// AvatarUrl 
        /// </summary> 
        /// <returns></returns> 
        [Column("AVATARURL")]
        public string? AvatarUrl { get; set; }

        /// <summary> 
        /// City 
        /// </summary> 
        /// <returns></returns> 
        [Column("CITY")]
        public string? City { get; set; }

        /// <summary> 
        /// Province 
        /// </summary> 
        /// <returns></returns> 
        [Column("PROVINCE")]
        public string? Province { get; set; }

        /// <summary> 
        /// Country 
        /// </summary> 
        /// <returns></returns> 
        [Column("COUNTRY")]
        public string? Country { get; set; }

        /// <summary> 
        /// Language 
        /// </summary> 
        /// <returns></returns> 
        [Column("LANGUAGE")]
        public string? Language { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Column("F_MOBILE")]
        public string? F_Mobile { get; set; }

        /// <summary> 
        /// Gender 
        /// </summary> 
        /// <returns></returns> 
        [Column("GENDER")]
        public int? Gender { get; set; }

        /// <summary> 
        /// NickName 
        /// </summary> 
        /// <returns></returns> 
        [Column("NICKNAME")]
        public string? NickName { get; set; }

        /// <summary> 
        /// F_UserId (该字段在新版本中可以移除了，)
        /// </summary> 
        /// <returns></returns> 
        [Column("F_USERID")]
        public string? F_UserId { get; set; }
    }
}
