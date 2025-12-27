var server = ''; //如果开启了https则这里是wss

$(function () {
    var scheme = document.location.protocol == "https:" ? "wss" : "ws";
    var port = document.location.port ? (":" + document.location.port) : "";
    server = scheme + "://" + document.location.hostname + port;

    // 获取小程序二维码
    GetWeChatQrCode();
})

/**
 * 获取微信小程序二维码
 * */
function GetWeChatQrCode() {
    Print(["开始请求小程序二维码"])
    $.ajax({
        url: "/api/WeChatService/GetWeChatQrCode",
        type: "Post",
        dataType: "json",
        async: false,
        success: function (json) {
            if (json.statusCode == 200) {
                // 显示二维码图片
                $(".QRCode").attr("src", json.data.data.imageUrl + "?" + new Date().getMilliseconds());
                Print(["二维码请求成功"])
                Print(["扫码监听中..."])

                // 开始建立webSocket 通讯
                InitWebSocket(json.data.data.webPageKey);
            }
            else {
                Print(["发生错误！请联系管理员！", "邮箱：753109098@qq.com"])
                alert("发生错误！请联系管理员");
            }
        },
        error: function () {
            Print(["发生错误！请联系管理员！", "邮箱：753109098@qq.com"])
            alert("发生错误！请联系管理员");
        }
    });
}

/**
 * 初始化webSocket
 * @param {any} clientKey
 */
function InitWebSocket(WebPageKey) {
    console.log('正在连接 WebSocket, ClientID:', WebPageKey);
    console.log('WebSocket URL:', server + '/ws?ClientID=' + WebPageKey);
    Print(["WebSocket 连接中... ClientID: " + WebPageKey]);
    
    var WEB_SOCKET = new WebSocket(server + '/ws?ClientID=' + WebPageKey);

    WEB_SOCKET.onopen = function (evt) {
        console.log('Connection open ...');
        Print(["WebSocket 连接成功！"]);
    };

    WEB_SOCKET.onmessage = function (evt) {
        console.log('Received Message: ' + evt.data);
        Print(["收到消息: " + evt.data]);
        ChangeState(evt.data);
    };

    WEB_SOCKET.onerror = function (evt) {
        console.log('WebSocket Error:', evt);
        Print(["WebSocket 连接错误！"]);
    };

    WEB_SOCKET.onclose = function (evt) {
        console.log('Connection closed.');
        Print(["通讯已断开，请刷新页面！"])
        $(".QRCode").attr("src", "/Areas/ScanDemo/Content/Image/Lodding.png");
    };
}

/**
 * 显示当前操作状态
 * @param {any} State
 */
function ChangeState(json) {
    var data = JSON.parse(json);
    if (data.status == "Scan") {  //已扫码
        $(".QRCodePanel").addClass("display");
        $(".ScanSuccess").removeClass("display");
        Print(["已扫码"]);
    }
    else if (data.status == "Login") { //登录
        $(".ScanSuccess .title div").text("登录成功");
        Print([
            "登录成功！",
            "获取到用户唯一标识OpenID：" + JSON.parse(data.msg).openId,
            "获取的源数据结果如下：",
            JSON.stringify(data.msg)
        ]);
    }
    else if (data.status == "Calcel") { //取消
        $(".ScanSuccess").addClass("display");
        $(".ScanRefuse").removeClass("display");

        Print(["用户取消登录！"]);
    }
}

function Print(textArray) {
    var html = ` <li><label class="time">[` + GetCurrentTime() + `]</label>`
    $.each(textArray, function (index, item) {
        html += `<div class="monitor">` + item + `</div>`
    });
    $(".print").append(html)

    //让滚动条到底部
    var ele = document.getElementById("print");
    //判断元素是否出现了滚动条
    if (ele.scrollHeight > ele.clientHeight) {
        //设置滚动条到最底部
        ele.scrollTop = ele.scrollHeight + 400;
    }
}
//清空
function Clear() {
    $(".print").html("")
}

// 获取当前日期
function GetCurrentTime() {
    let date = new Date();
    let year = date.getFullYear();
    let month = date.getMonth() + 1
    month = month > 9 ? month : "0" + month;
    let day = date.getDate();
    day = day > 9 ? day : "0" + day;
    let hour = date.getHours(); //获取当前小时数(0-23)
    hour = hour > 9 ? hour : "0" + hour;
    let minute = date.getMinutes(); //获取当前分钟数(0-59)
    minute = minute > 9 ? minute : "0" + minute;
    let second = date.getSeconds(); //获取当前秒数(0-59)
    second = second > 9 ? second : "0" + second;
    let milliseconds = date.getMilliseconds();
    return year + '-' + month + '-' + day + ' ' + hour + ':' + minute + ':' + second + '.' + milliseconds;
}