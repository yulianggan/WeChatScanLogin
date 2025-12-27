using Furion.DependencyInjection;
using Furion.JsonSerialization;
using StackExchange.Profiling.Internal;
using Sys.Hub.Core.Common.WeChatEntity;

namespace Sys.Hub.Core.Util
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/2/19 0:57:30 
    /// 描    述 ：  小程序相关接口
    /// </summary>
    public class WechatHelper: ITransient
    {
        private readonly IJsonSerializerProvider _jsonSerializer;
        public WechatHelper(IJsonSerializerProvider jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        /// <summary>
        /// 获取接口凭证
        /// </summary>
        /// <param name="appid">小程序唯一凭证，即 AppID，可在「微信公众平台 - 设置 - 开发设置」页中获得。
        /// （需要已经成为开发者，且帐号没有异常状态）</param>
        /// <param name="secret">小程序唯一凭证密钥，即 AppSecret，获取方式同 appid</param>
        /// <returns></returns>
        public AccessTokenResponse GetAccessToken(string appid, string secret)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
            string responseStr = HttpMethods.Get(url);
            return _jsonSerializer.Deserialize<AccessTokenResponse>(responseStr);
        }

        /// <summary>
        /// 获取微信小程序二维码
        /// </summary>
        /// <param name="access_token">access_token</param>
        /// <param name="entity">请求实体参数</param>
        /// <returns></returns>
        public byte[] GetWeChatQrCode(string access_token, QRCodeResquest entity)
        {
            string url = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + access_token;
            return HttpMethods.Post_ReturnByte(url, entity.ToJson());
        }


        /// <summary>
        /// 登录凭证校验
        /// </summary>
        /// <param name="entity">请求参数实体</param>
        /// <returns></returns>
        public jscode2session GetCode2Session(Code2SessionRequest entity)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", entity.appid, entity.secret, entity.js_code);
            string responseStr = HttpMethods.Get(url);
            return _jsonSerializer.Deserialize<jscode2session>(responseStr);

        }
    }
}
