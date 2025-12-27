using Furion.FriendlyException;
using Sys.Hub.Core.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Hub.Core.Common.Entity
{
    public class OperateResult
    {

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public OperateResult() { 
        
        }

        /// <summary>
        /// 使用指定的消息实例化一个默认的结果对象
        /// </summary>
        /// <param name="msg">错误消息</param>
        public OperateResult(string msg)
        {
            this.Message = msg;
        }

        /// <summary>
        /// 使用错误代码，消息文本来实例化对象
        /// </summary>
        /// <param name="err">错误代码</param>
        /// <param name="msg">错误消息</param>
        public OperateResult(int err, string msg)
        {
            this.ErrorCode = err;
            this.Message = msg;
        }

        /// <summary>
        /// 实例化一个默认的结果对象
        /// </summary>
        public OperateResult(OperateResult result)
        {
            Assignment(result);
        }

        #endregion

        #region 字段

        /// <summary>
        /// 指示本次访问是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 具体的错误描述
        /// </summary>
        public string Message { get; set; } = "UnknownError";

        /// <summary>
        /// 具体的错误代码
        /// </summary>
        public int ErrorCode { get; set; } = 10000;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime InitialTime { get; protected set; } = DateTime.Now;
        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public double? TimeConsuming { get; private set; }

        #endregion

        /// <summary>
        /// 获取错误代号及文本描述
        /// </summary>
        /// <returns>包含错误码及错误消息</returns>
        public string ToMessageShowString()
        {
            return $"错误代号:{ErrorCode}{Environment.NewLine}错误描述:{Message}";
        }

        /// <summary>
        /// 获取错误代号及文本描述
        /// </summary>
        /// <returns>包含错误码及错误消息</returns>
        public string ToMessageString()
        {
            return $"是否成功:{IsSuccess}{Environment.NewLine}共计耗时{TimeConsuming}错误代号:{ErrorCode}{Environment.NewLine}错误描述:{Message}";
        }

        /// <summary>
        /// 从另一个结果类中拷贝错误信息
        /// </summary>
        /// <typeparam name="TResult">支持结果类及派生类</typeparam>
        /// <param name="result">结果类及派生类的对象</param>
        public void CopyErrorFromOther<TResult>(TResult result) where TResult : OperateResult
        {
            if (result != null)
            {
                Assignment(result);
            }
        }

        /// <summary>
        /// 赋值
        /// </summary>
        public void Assignment(OperateResult result)
        {
            IsSuccess = result.IsSuccess;
            InitialTime = result.InitialTime;
            Message = result.Message;
            ErrorCode = result.ErrorCode;
            EndTime();
        }

        /// <summary>
        /// 结束时间统计
        /// </summary>
        public virtual OperateResult EndTime()
        {
            this.TimeConsuming = (DateTime.Now - this.InitialTime).TotalMilliseconds;
            return this;
        }

        #region Static Method

        /// <summary>
        /// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="result">之前的结果对象</param>
        /// <returns>带默认泛型对象的失败结果类</returns>
        public static OperateResult<T> CreateFailedResult<T>(OperateResult result)
        {
            return new OperateResult<T>()
            {
                InitialTime = result.InitialTime,
                ErrorCode = result.ErrorCode,
                Message = result.Message,
                TimeConsuming = result.TimeConsuming
            };
        }

        /// <summary>
        /// 创建并返回一个失败的结果对象，该对象复制另一个结果对象的错误信息
        /// </summary>
        /// <typeparam name="T1">目标数据类型一</typeparam>
        /// <typeparam name="T2">目标数据类型二</typeparam>
        /// <param name="result">之前的结果对象</param>
        /// <returns>带默认泛型对象的失败结果类</returns>
        public static OperateResult<T1, T2> CreateFailedResult<T1, T2>(OperateResult result)
        {
            return new OperateResult<T1, T2>()
            {
                InitialTime = result.InitialTime,
                ErrorCode = result.ErrorCode,
                Message = result.Message,
                TimeConsuming = result.TimeConsuming
            };
        }

        /// <summary>
        /// 创建并返回一个成功的结果对象
        /// </summary>
        /// <returns>成功的结果对象</returns>
        public static OperateResult CreateSuccessResult()
        {
            var result = new OperateResult()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "操作成功",
            };
            return result.EndTime();
        }

        /// <summary>
        /// 创建并返回一个成功的结果对象，并带有一个参数对象
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="value">类型的值对象</param>
        /// <returns>成功的结果对象</returns>
        public static OperateResult<T> CreateSuccessResult<T>(T value)
        {
            return new OperateResult<T>()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "操作成功",
                Content = value,
            };
        }

        /// <summary>
        /// 创建并返回一个成功的结果对象，并带有两个参数对象
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <param name="value1">类型一对象</param>
        /// <param name="value2">类型二对象</param>
        /// <returns>成的结果对象</returns>
        public static OperateResult<T1, T2> CreateSuccessResult<T1, T2>(T1 value1, T2 value2)
        {
            return new OperateResult<T1, T2>()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "操作成功",
                Content1 = value1,
                Content2 = value2,
            };
        }


        #endregion
    }

    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/17 13:32:02 
    /// 描    述 ：  操作结果的类，只带有成功标志和错误信息
    /// </summary>
    public class OperateResult<T> : OperateResult
    {
        #region Constructor

        /// <summary>
        /// 实例化一个默认的结果对象
        /// </summary>
        public OperateResult() : base()
        {
        }

        /// <summary>
        /// 使用指定的消息实例化一个默认的结果对象
        /// </summary>
        /// <param name="msg">错误消息</param>
        public OperateResult(string msg) : base(msg)
        {

        }

        /// <summary>
        /// 使用错误代码，消息文本来实例化对象
        /// </summary>
        /// <param name="err">错误代码</param>
        /// <param name="msg">错误消息</param>
        public OperateResult(int err, string msg) : base(err, msg)
        {

        }
        /// <summary>
        /// 使用OperateResult 构建一个新的OperateResult
        /// </summary>
        /// <param name="result"></param>
        public OperateResult(OperateResult result) : base(result)
        {

        }
        #endregion

        /// <summary>
        /// 用户自定义的泛型数据
        /// </summary>
        public T Content { get; set; }
        /// <summary>
        /// 统计结束时间
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public override OperateResult<T> EndTime()
        {
            base.EndTime();
            return this;
        }
    }

    /// <summary>
    /// 操作结果的泛型类，允许带两个用户自定义的泛型对象，推荐使用这个类
    /// </summary>
    /// <typeparam name="T1">泛型类</typeparam>
    /// <typeparam name="T2">泛型类</typeparam>
    public class OperateResult<T1, T2> : OperateResult
    {
        #region Constructor

        /// <summary>
        /// 实例化一个默认的结果对象
        /// </summary>
        public OperateResult() : base()
        {
        }

        /// <summary>
        /// 使用指定的消息实例化一个默认的结果对象
        /// </summary>
        /// <param name="msg">错误消息</param>
        public OperateResult(string msg) : base(msg)
        {

        }

        /// <summary>
        /// 使用错误代码，消息文本来实例化对象
        /// </summary>
        /// <param name="err">错误代码</param>
        /// <param name="msg">错误消息</param>
        public OperateResult(int err, string msg) : base(err, msg)
        {

        }
        /// <summary>
        /// 使用OperateResult 构建一个新的OperateResult
        /// </summary>
        /// <param name="result"></param>
        public OperateResult(OperateResult result) : base(result)
        {

        }
        #endregion

        /// <summary>
        /// 用户自定义的泛型数据1
        /// </summary>
        public T1 Content1 { get; set; }

        /// <summary>
        /// 用户自定义的泛型数据2
        /// </summary>
        public T2 Content2 { get; set; }

        /// <summary>
        /// 统计结束时间
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public override OperateResult<T1, T2> EndTime()
        {
            base.EndTime();
            return this;
        }
    }

    public static class OperateResultBuilder
    {
        public static OperateResult CreateOperateResult(Action action, Action action1)
        {
            return new OperateResultAction(action).TryCatch(action1);
        }
        public static OperateResult CreateOperateResult(Action action)
        {
            return new OperateResultAction(action).TryCatch();
        }
        public static OperateResult<T> CreateOperateResult<T>(Func<T> func)
        {
            return new OperateResultFunc<T>(func).TryCatch();
        }
    }

    public class OperateResultAction
    {
        private Action _action;
        public OperateResultAction(Action action)
        {
            _action = action;
        }
        public OperateResult TryCatch(Action action = null)
        {
            var operateResult = new OperateResult();
            try
            {
                _action();
                operateResult.IsSuccess = true;
                operateResult.Message = "";
            }
            catch (Exception ex)
            {
                operateResult.IsSuccess = false;
                operateResult.Message = ex.ToExceptionMessage();
            }
            finally
            {
                if (action != null)
                {
                    action();
                }
            }
            return operateResult.EndTime();
        }
    }

    public class OperateResultFunc<T>
    {
        private Func<T> _func;
        public OperateResultFunc(Func<T> func)
        {
            _func = func;
        }
        public OperateResult<T> TryCatch()
        {
            var operateResult = new OperateResult<T>();
            try
            {
                var result = _func();
                if (result == null)
                {
                    throw Oops.Oh("数据为空");
                }
                operateResult.IsSuccess = true;
                operateResult.Content = result;
                operateResult.Message = "Successed";
            }
            catch (Exception ex)
            {
                operateResult.IsSuccess = false;
                operateResult.Message = ex.ToExceptionMessage();
            }
            return operateResult.EndTime();
        }
    }
}
