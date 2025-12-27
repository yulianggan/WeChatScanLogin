//app.js
App({
  onLaunch: function () {
  },
  globalData: {
    userInfo: null,  //用户信息

    baseWechatUrl:"https://kornza.com/api/WeChatService",  //访问路径
    webSocketUrl:"wss://kornza.com",  //WebSocket 地址

    openId:'', //用户唯一标识
    loginState:false  , //用户登录状态
  }
})