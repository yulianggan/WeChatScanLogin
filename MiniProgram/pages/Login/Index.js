const app = getApp()
const WebSocket = require('../../utils/WebSocket.js');
Page({
  /**
   * 页面的初始数据
   */
  data: {
    QRCode: '',
    SendClientId: '',  // 小程序和后端的WebSocket的ID
    ReceiveID: '',     // 需要件将消息发送给谁
  },
  /**
   * 页面加载
   */
  onLoad: function (option) {
    var that = this;
    if (!!option.scene) {
      this.setData({
        //获取二维码中的参数
        QRCode: option.scene,
        SendClientId: option.scene.substring(0, 25),
        ReceiveID: option.scene.substring(7)
      });

      // 创建连接
      WebSocket.connectSocket({ ClientID: option.scene.substring(0, 25), ReceiveID: option.scene.substring(7) });
      // 设置接收消息回调
      WebSocket.onSocketMessageCallback = this.onSocketMessageCallback;
      setTimeout(() => {
        that.sendSocketMessage("Scan", 0);
      }, 100);

    }
  },
  /**
   * 发送消息
   */
  sendSocketMessage(Action, Message) {
    console.log("调用发送消息");
    var ReceiveID = this.data.ReceiveID;
    WebSocket.sendSocketMessage({ "SendClientId": "", "Action": Action, "Msg": Message, "ReceiveID": ReceiveID });

  },
  // Socket收到的信息
  onSocketMessageCallback: function (res) {
    console.log(res);
  },
  // 页面销毁时关闭连接
  onUnload: function (options) {
    WebSocket.closeSocket();
  },
  /**
   * 确认登录（wx.getUserProfile 已废弃，改用 wx.login 获取 code）
   */
  getUserProfile(info) {
    var that = this;
    wx.showLoading({
      title: '正在登录...',
      mask: true,
    })
    // 执行登录操作
    wx.login({
      success: (res) => {
        if (res.code) {
          // 使用默认用户信息（因 getUserProfile 已废弃）
          var userInfo = JSON.stringify({
            nickName: '微信用户',
            avatarUrl: '',
            gender: 0
          });
          that.getSessionKey(userInfo, res.code);
        } else {
          wx.hideLoading();
          wx.showToast({
            title: '登录失败',
            icon: 'none'
          });
        }
      },
      fail: () => {
        wx.hideLoading();
        wx.showToast({
          title: '登录失败',
          icon: 'none'
        });
      }
    });
  },
  /**
   * 获取SessionKey
   */
  getSessionKey: function (userInfo, code) {
    var that = this
    that.GetOpenId(userInfo, code);
  },
  /**
   * 获取openId，并保存用户
   */
  GetOpenId: function (userInfo, code) {
    //获取openId
    var that = this
    //开始请求用户Sessionkey
    wx.request({
      url: app.globalData.baseWechatUrl + '/WechatUserProxy',
      data: {
        userInfo: userInfo,
        code: code,
        F_DeviceID: wx.getStorageSync("RegisterID")
      },
      dataType: "json",
      method: "POST",
      success: function (res) {
        wx.hideLoading(); //隐藏加载中提示
        console.log("WechatUserProxy 返回:", res.data);
        var data = res.data;
        // 修复：检查 data.data.code 而不是 data.statusCode
        if (data.statusCode == 200 && data.data && data.data.code == 200) {
          // 发送完整的用户信息（包括 code、sessionKey 等）
          var loginData = data.data.data;
          that.sendSocketMessage("Login", JSON.stringify(loginData));
          wx.showToast({
            title: "登录成功",
            duration: 1500,
            icon: 'success',
            mask: true
          });
        } else {
          var errMsg = (data.data && data.data.message) ? data.data.message : "登录失败";
          wx.showToast({
            title: errMsg,
            duration: 2000,
            icon: 'none',
            mask: true
          })
        }
      },
      fail: function(err) {
        wx.hideLoading();
        console.log("请求失败:", err);
        wx.showToast({
          title: "网络请求失败",
          duration: 2000,
          icon: 'none',
          mask: true
        })
      }
    })
  },
  //取消登录
  CancelLoginBtn: function () {
    this.sendSocketMessage("Calcel", "2");
    wx.showToast({
      title: "取消登录",
      duration: 1000,
      icon: 'none',
      mask: true
    })
  },
})

