# 个人资质实现微信扫码登录

#### 在线测试地址：**[微信扫码登录在线测试](https://www.converts.cn/scantest.html)** 

![微信扫码登录在线测试](https://foruda.gitee.com/images/1684464421272543485/19c379c6_5585317.png "屏幕截图")

#### 介绍
微信扫码注册、登录是非常方便的事情。然而当我们的个人网站（博客、论坛）想要接入该功能的时候却发现只对企业开放。不过这难不倒我们，我们依旧可以实现以 -  **个人资质实现博客网站微信扫码登录** 。


#### 软件架构
##### 客户端
客户端基于Furion/.NET 6实现的。代码简洁、易扩展，让开发更简单、更通用、更流行。

##### 小程序端
使用的是微信小程序。


#### 安装教程

无需安装， 解压即用。

#### 前提条件

需要注册微信小程序，并拥有一个域名。

#### 使用说明

需要在 `Sys.Hub.Web.Entry/Sys.Hub.Web.Entry/Configs/SysConfig.json` 配置文件中配置小程序的 'AppID' 、'Secret'

#### 实现思路

具体实现思路请看博客：[https://blog.csdn.net/IT_rookie_newbie/article/details/130510852](https://blog.csdn.net/IT_rookie_newbie/article/details/130510852)

