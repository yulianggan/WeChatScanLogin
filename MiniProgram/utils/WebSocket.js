const app = getApp();
// 域名地址(项目实地址)
const Host = app.globalData.webSocketUrl;
 
// Socket连接成功
var socketOpen = false;
// Socket关闭
var socketClose = false;
// 消息队列
var socketMsgQueue = [];
 
// 判断心跳变量
var heart = null;
// 心跳失败次数
var heartBeatFailCount = 0;
// 终止心跳
var heartBeatTimeout = null;
// 终止重连
var connectSocketTimeout = null;
 
var webSocket = {
    // 连接Socket
    connectSocket:function(options) {
      var that = this;
      console.log(options);
        wx.showLoading({
            title: '正在请求中',
            mask: true,
        });
        socketOpen = false;
        socketClose = false;
        socketMsgQueue = [];
        
        // 添加连接超时处理（10秒）
        var connectTimeout = setTimeout(function() {
            if (!socketOpen) {
                wx.hideLoading();
                wx.showToast({
                    title: '连接超时，请重试',
                    icon: 'none',
                    duration: 2000
                });
            }
        }, 10000);
        
        wx.connectSocket({
            url:Host + '/ws?ClientID='+options.ClientID,
            success:function(res) {
              console.log("连接成功");
              clearTimeout(connectTimeout);
                if (options) {
                    // options.success && options.success(res);
                }
            },
            fail:function(res) {
              console.log("连接失败", res);
              clearTimeout(connectTimeout);
              wx.hideLoading();
              wx.showToast({
                  title: '连接失败：' + (res.errMsg || '未知错误'),
                  icon: 'none',
                  duration: 2000
              });
                if (options) {
                    options.fail && options.fail(res);
                }
            }
        })
    },
    // 发送消息
    sendSocketMessage:function(options) {
        if (socketOpen) {
            wx.sendSocketMessage({
                data: JSON.stringify(options),
                success: function(res) {
                    if (options) {
                        options.success && options.success(res);
                    }
                },
                fail: function(res) {
                    if (options) {
                        options.fail && options.fail(res);
                    }
                }
            })
        } else {
            socketMsgQueue.push(options)
        }
    },
    // 关闭Socket
    closeSocket: function(options) {
        if (connectSocketTimeout) {
            clearTimeout(connectSocketTimeout);
            connectSocketTimeout = null;
        };
        socketClose = true;
        this.stopHeartBeat();
        wx.closeSocket({
            success: function(res) {
                if (options) {
                    options.success && options.success(res);
                }
            },
            fail: function(res) {
                if (options) {
                    options.fail && options.fail(res);
                }
            }
        })
    },
    // 收到消息
    onSocketMessageCallback: function(msg) {},
 
    // 开始心跳
    startHeartBeat: function() {
        heart = true;
        this.heartBeat();
    },
 
    // 正在心跳
    heartBeat: function() {
        var that = this;
        if (!heart) {
            return;
        };
        that.sendSocketMessage({
            msg: JSON.stringify({
                // 与后端约定，传点消息，保持链接
                "msg_type": "heart"
            }),
            success: function(res) {
                if (heart) {
                    heartBeatTimeout = setTimeout(() => {
                        that.heartBeat();
                    }, 7000);
                }
            },
            fail: function(res) {
                if (heartBeatFailCount > 2) {
                    that.connectSocket();
                };
                if (heart) {
                    heartBeatTimeout = setTimeout(() => {
                        that.heartBeat();
                    }, 7000);
                };
                heartBeatFailCount++;
            },
        });
    },
 
    // 结束心跳
    stopHeartBeat: function() {
        heart = false;
        if (heartBeatTimeout) {
            clearTimeout(heartBeatTimeout);
            heartBeatTimeout = null;
        };
        if (connectSocketTimeout) {
            clearTimeout(connectSocketTimeout);
            connectSocketTimeout = null;
        }
    }
};
 
// 监听WebSocket打开连接
wx.onSocketOpen(function(res) {
    wx.hideLoading();
    // 如果已经关闭socket
    if (socketClose) {
        webSocket.closeSocket();
    } else {
        socketOpen = true
        console.log("发送数据。。。");
        console.log(socketMsgQueue);
        for (var i = 0; i < socketMsgQueue.length; i++) {
            webSocket.sendSocketMessage(socketMsgQueue[i])
        };
        socketMsgQueue = []
        webSocket.startHeartBeat();
    }
});
 
// 监听WebSocket错误
wx.onSocketError(function(res) {
    console.log('WebSocket连接打开失败，请检查！', res);
    wx.hideLoading();
    wx.showToast({
        title: '连接服务器失败',
        icon: 'none',
        duration: 2000
    });
});
// 监听WebSocket接受到服务器的消息
wx.onSocketMessage(function(res) {
    webSocket.onSocketMessageCallback(res.data);
});
 
// 监听WebSocket关闭连接后马上重连
wx.onSocketClose(function(res) {
    if (!socketClose) {
        clearTimeout(connectSocketTimeout);
        connectSocketTimeout = setTimeout(() => {
            webSocket.connectSocket();
        }, 3000);
    }
});
 
module.exports = webSocket;