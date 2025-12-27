using Furion;
using Furion.DynamicApiController;
using Furion.JsonSerialization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using Sys.Hub.Core.Common.Entity;
using Sys.Hub.Core.Common.WeChatEntity;
using Sys.Hub.Core.MiniProgram.Common.Entity;
using Sys.Hub.Core.Util;

namespace Sys.Hub.Application.MiniProgram
{
    /// <summary>
    /// 创 建 人 ：  胖太乙
    /// 创建时间 ：  2023/5/1 0:43:55 
    /// 描    述 ：  微信小程序公共服务
    /// </summary>
    [ApiDescriptionSettings("微信小程序公共服务", Name = "微信小程序公共服务", Description = "微信小程序公共服务", Order = 2)]
    [Route("api/WeChatService")]
    public class WeChatApiService : IDynamicApiController
    {
        WechatHelper _wechatHelper;

        /// <summary>
        /// 序列化
        /// </summary>
        private readonly IJsonSerializerProvider _jsonSerializer;

        public WeChatApiService(IJsonSerializerProvider jsonSerializer)
        {
            _wechatHelper = App.GetService<WechatHelper>();
            _jsonSerializer = jsonSerializer;
        }

        /// <summary>
        /// 微信授权登录
        /// </summary>
        /// <param name="wechatUserEntity">请求实体</param>
        /// <returns></returns>
        [HttpPost("WechatUserProxy")]
        public async Task<ResponseEntity> WechatUserProxy(WechatUserProxyEntity wechatUserEntity)
        {
            Code2SessionRequest entity = new Code2SessionRequest();
            entity.appid = App.GetConfig<string>("AppID");
            entity.secret = App.GetConfig<string>("Secret");
            entity.js_code = wechatUserEntity.code;
            var jscode2session = _wechatHelper.GetCode2Session(entity);
            if (string.IsNullOrEmpty(jscode2session.openid))
            {
                return new ResponseEntity()
                {
                    Code = 500,
                    Message = "登录失败"
                };
            }
            else
            {
                WeChatUserEntity wechat = _jsonSerializer.Deserialize<WeChatUserEntity>(wechatUserEntity.userInfo);
                wechat.OpenId = jscode2session.openid;
                wechat.AppId = entity.appid;

                //此处需要保存到数据库
                //_weChatService.SaveWechatUserEntity(wechat, wechatUserEntity.F_DeviceID);

                return new ResponseEntity()
                {
                    Data = new
                    {
                        openId = jscode2session.openid,
                        sessionKey = jscode2session.session_key,
                        code = wechatUserEntity.code,
                        appId = entity.appid,
                        nickName = wechat.NickName,
                        avatarUrl = wechat.AvatarUrl,
                        gender = wechat.Gender
                    },
                    Code = 200,
                    Message = "登录成功"
                };
            }
        }


        /// <summary>
        /// 微信小程序二维码
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetWeChatQrCode")]
        public async Task<ResponseEntity> GetWeChatQrCode()
        {
            var appid = App.GetConfig<string>("AppID");
            var secret = App.GetConfig<string>("Secret");

            //获取 AccessToken
            AccessTokenResponse entity = _wechatHelper.GetAccessToken(appid, secret);

            string miniProgramKey = Guid.NewGuid().ToString().Substring(0, 32);

            // 初始化二维码信息
            QRCodeResquest QRCodeEntity = new QRCodeResquest();
            QRCodeEntity.page = "pages/Login/Index";  // 注： 这里的路径必须是已经发布成功的页面路径， 否则会报错。
            QRCodeEntity.scene = miniProgramKey;

            // 获取小程序二维码
            byte[] byteArray = _wechatHelper.GetWeChatQrCode(entity.access_token, QRCodeEntity);

            var imageUrl = string.Empty;
            var code = 200;
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                //如果没有填写appid 和 Secret， 此处会报错
                try
                {
                    var outputImg = SixLabors.ImageSharp.Image.Load(byteArray, new SixLabors.ImageSharp.Formats.Jpeg.JpegDecoder());
                    var path = Directory.GetCurrentDirectory();
                    outputImg.Save(LinuxUtil.GetRuntimeDirectory(path + "/wwwroot/TemporaryFile/QrCode.jpg"));
                    imageUrl = "/TemporaryFile/QrCode.jpg";
                }
                catch (Exception)
                {
                    imageUrl = "/Areas/ScanDemo/Content/Image/Lodding.png";
                    code = 500;
                }
            }

            return new ResponseEntity()
            {
                Data = new
                {
                    ImageUrl = imageUrl,
                    MiniProgramKey = miniProgramKey.Substring(0,25),  // 通过此ID ,使小程序和后端建立通讯
                    WebPageKey = miniProgramKey.Substring(7)  // 通过此ID ,使页面和后端建立通讯
                },
                Code = code,
                Message = "二维码获取成功"
            };


        }
    }
}
