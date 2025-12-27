using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Util.Extensions
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/17 13:46:53 
    /// 描    述 ：  异常拓展
    /// </summary>
    public static class AppFriendlyExceptionExtensions
    {
        /// <summary>
        /// 设置异常状态码
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static AppFriendlyException StatusCode(this AppFriendlyException exception, int statusCode = StatusCodes.Status500InternalServerError)
        {
            exception.StatusCode = statusCode;
            return exception;
        }
        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ToExceptionMessage(this Exception exception)
        {
            StringBuilder sbLogMessage = new StringBuilder();
            sbLogMessage.AppendLine($"【异常消息】：{exception.Message}");
            sbLogMessage.AppendLine($"【内部异常】：{exception.InnerException?.Message}");
            sbLogMessage.AppendLine($"【堆栈消息】：{exception.StackTrace?.ToString()}");
            return sbLogMessage.ToString();
        }
        /// <summary>
        /// 设置额外数据
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static AppFriendlyException WithData(this AppFriendlyException exception, object data)
        {
            exception.Data = data;
            return exception;
        }
    }
}
