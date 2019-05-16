<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="peoplevoiceup.aspx.cs"
    Inherits="ETS2.WebApp.H5.peoplevoiceup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <title>微信录制语音</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=0">
    <link rel="stylesheet" href="http://demo.open.weixin.qq.com/jssdk/css/style.css?ts=1420774989">
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"> </script>
    <script>
        wx.config({
            debug: false,
            appId: '<%=appId %>',
            timestamp: '<%=timestamp %>',
            nonceStr: '<%=nonceStr %>',
            signature: '<%=signature %>',
            jsApiList: [
        'checkJsApi',
        'onMenuShareTimeline',
        'onMenuShareAppMessage',
        'onMenuShareQQ',
        'onMenuShareWeibo',
        'hideMenuItems',
        'showMenuItems',
        'hideAllNonBaseMenuItem',
        'showAllNonBaseMenuItem',
        'translateVoice',
        'startRecord',
        'stopRecord',
        'onRecordEnd',
        'playVoice',
        'pauseVoice',
        'stopVoice',
        'uploadVoice',
        'downloadVoice',
        'chooseImage',
        'previewImage',
        'uploadImage',
        'downloadImage',
        'getNetworkType',
        'openLocation',
        'getLocation',
        'hideOptionMenu',
        'showOptionMenu',
        'closeWindow',
        'scanQRCode',
        'chooseWXPay',
        'openProductSpecificView',
        'addCard',
        'chooseCard',
        'openCard'
      ]
        });
    </script>
    <script src="http://demo.open.weixin.qq.com/jssdk/js/api-6.1.js?ts=1420774989"> </script>
    <script type="text/javascript">
        /*
        * 注意：
        * 1. 所有的JS接口只能在公众号绑定的域名下调用，公众号开发者需要先登录微信公众平台进入“公众号设置”的“功能设置”里填写“JS接口安全域名”。
        * 2. 如果发现在 Android 不能分享自定义内容，请到官网下载最新的包覆盖安装，Android 自定义分享接口需升级至 6.0.2.58 版本及以上。
        * 3. 完整 JS-SDK 文档地址：http://mp.weixin.qq.com/wiki/7/aaa137b55fb2e0456bf8dd9148dd613f.html
        *
        * 如有问题请通过以下渠道反馈：
        * 邮箱地址：weixin-open@qq.com
        * 邮件主题：【微信JS-SDK反馈】具体问题
        * 邮件内容说明：用简明的语言描述问题所在，并交代清楚遇到该问题的场景，可附上截屏图片，微信团队会尽快处理你的反馈。
        */
        wx.ready(function () {
            //            // 1 判断当前版本是否支持指定 JS 接口，支持批量判断
            //            document.querySelector('#checkJsApi').onclick = function () {
            //                wx.checkJsApi({
            //                    jsApiList: [
            //                                'getNetworkType',
            //                                'previewImage'
            //                                ],
            //                    success: function (res) {
            //                        alert(JSON.stringify(res));
            //                    },
            //                    fail: function (res) {
            //                        alert(res.errMsg);
            //                    }
            //                });
            //            };
            // 3 智能接口
            var voice = {
                localId: '',
                serverId: ''
            };
            //            // 3.1 识别音频并返回识别结果
            //            document.querySelector('#translateVoice').onclick = function () {
            //                if (voice.localId == '') {
            //                    alert('请先使用 startRecord 接口录制一段声音');
            //                    return;
            //                }
            //                wx.translateVoice({
            //                    localId: voice.localId,
            //                    complete: function (res) {
            //                        if (res.hasOwnProperty('translateResult')) {
            //                            alert('识别结果：' + res.translateResult);
            //                        } else {
            //                            alert('无法识别');
            //                        }
            //                    }
            //                });
            //            };
            // 4 音频接口
            // 4.2 开始录音
            document.querySelector('#startRecord').onclick = function () {

                wx.startRecord({
                    success: function () {
                        alert("开始录音");
                    },
                    cancel: function () {
                        alert('用户拒绝授权录音');
                    },
                    fail: function (res) {
                        alert(res.errMsg);
                    }
                });
            };
            // 4.3 停止录音
            document.querySelector('#stopRecord').onclick = function () {
                wx.stopRecord({
                    success: function (res) {
                        voice.localId = res.localId;
                        //                        alert("停止录音");
                        //----上传语音----
                        if (voice.localId == '') {
                            alert('请先录制一段声音');
                            return;
                        }
                        wx.uploadVoice({
                            localId: voice.localId,
                            success: function (res) {
                                //                        alert('上传语音成功，serverId 为' + res.serverId);
                                voice.serverId = res.serverId;
                                //微信多媒体接口下载语音到自己的服务器
                                $.post("/JsonFactory/WeiXinHandler.ashx?oper=wxdownvoice", { mediaid: res.serverId, openid: '<%=openid %>', comid: '<%=comid %>', clientuptypemark: '<%=clientuptypemark %>', remarks: "", materialid: '<%=materialid %>' }, function (data) {
                                    data = eval("(" + data + ")");
                                    if (data.type == 1) {
                                        alert(data.msg);
                                    }
                                    if (data.type == 100) {
                                        alert('停止录音');
                                        location.href = "peoplevoicelist.aspx?openid=<%=openid %>&comid=<%=comid %>&clientuptypemark=<%=clientuptypemark %>&materialid=<%=materialid %>";
                                    }
                                })
                            }
                        });
                        //----上传语音----
                    },
                    fail: function (res) {
                        alert(JSON.stringify(res));
                    }
                });
            };
            // 4.4 监听录音自动停止
            wx.onVoiceRecordEnd({
                complete: function (res) {
                    voice.localId = res.localId;
                    //                    alert('录音时间已超过一分钟，自动停止录音');
                    //----上传语音----
                    if (voice.localId == '') {
                        alert('请先录制一段声音');
                        return;
                    }
                    wx.uploadVoice({
                        localId: voice.localId,
                        success: function (res) {
                            //                        alert('上传语音成功，serverId 为' + res.serverId);
                            voice.serverId = res.serverId;
                            //微信多媒体接口下载语音到自己的服务器
                            $.post("/JsonFactory/WeiXinHandler.ashx?oper=wxdownvoice", { mediaid: res.serverId, openid: '<%=openid %>', comid: '<%=comid %>', clientuptypemark: '<%=clientuptypemark %>', remarks: "", materialid: '<%=materialid %>' }, function (data) {
                                data = eval("(" + data + ")");
                                if (data.type == 1) {
                                    alert(data.msg);
                                }
                                if (data.type == 100) {
                                    alert('录音时间已超过一分钟，自动停止录音');
                                    location.href = "peoplevoicelist.aspx?openid=<%=openid %>&comid=<%=comid %>&clientuptypemark=<%=clientuptypemark %>&materialid=<%=materialid %>";
                                }
                            })
                        }
                    });
                    //----上传语音----
                }
            });
            //            // 4.5 播放音频
            //            document.querySelector('#playVoice').onclick = function () {
            //                if (voice.localId == '') {
            //                    alert('请先使用 startRecord 接口录制一段声音');
            //                    return;
            //                }
            //                wx.playVoice({
            //                    localId: voice.localId
            //                });

            //            };
            //            // 4.6 暂停播放音频
            //            document.querySelector('#pauseVoice').onclick = function () {
            //                wx.pauseVoice({
            //                    localId: voice.localId
            //                });
            //            };
            //            // 4.7 停止播放音频
            //            document.querySelector('#stopVoice').onclick = function () {
            //                wx.stopVoice({
            //                    localId: voice.localId
            //                });
            //            };
            //            // 4.8 监听录音播放停止
            //            wx.onVoicePlayEnd({
            //                complete: function (res) {
            //                    alert('录音（' + res.localId + '）播放结束');
            //                }
            //            });
            //            // 4.8 上传语音
            //            document.querySelector('#uploadVoice').onclick = function () {
            //                if (voice.localId == '') {
            //                    alert('请先使用 startRecord 接口录制一段声音');
            //                    return;
            //                }
            //                wx.uploadVoice({
            //                    localId: voice.localId,
            //                    success: function (res) {
            //                        //                        alert('上传语音成功，serverId 为' + res.serverId);
            //                        voice.serverId = res.serverId;
            //                        //微信多媒体接口下载语音到自己的服务器
            //                        $.post("/JsonFactory/WeiXinHandler.ashx?oper=wxdownvoice", { mediaid: res.serverId, openid: '<%=openid %>', comid: '<%=comid %>', clientuptypemark: '<%=clientuptypemark %>', remarks: "" }, function (data) {
            //                            data = eval("(" + data + ")");
            //                            if (data.type == 1) {
            //                                alert(data.msg);
            //                            }
            //                            if (data.type == 100) {
            //                                alert('上传语音成功，serverId 为' + res.serverId);
            //                                location.href = "peoplevoicelist.aspx?openid=<%=openid %>&comid=<%=comid %>&clientuptypemark=<%=clientuptypemark %>";
            //                            }
            //                        })
            //                    }
            //                });
            //            };
            //            // 4.9 下载语音
            //            document.querySelector('#downloadVoice').onclick = function () {
            //                if (voice.serverId == '') {
            //                    alert('请先使用 uploadVoice 上传声音');
            //                    return;
            //                }
            //                wx.downloadVoice({
            //                    serverId: voice.serverId,
            //                    success: function (res) {
            //                        alert('下载语音成功，localId 为' + res.localId);
            //                        voice.localId = res.localId;
            //                    }
            //                });
            //            };
        });
        wx.error(function (res) {
            alert(res.errMsg);
        });

        
    </script>
    <style type="text/css">
        #nav
        {
            height: 40px;
            border-top: #060 2px solid;
            border-bottom: #060 2px solid;
            background-color: #690;
        }
        #nav ul
        {
            list-style: none;
            margin-left: 50px;
        }
        #nav li
        {
            display: inline;
            line-height: 40px;
        }
        #nav a
        {
            color: #fff;
            text-decoration: none;
            padding: 20px 20px;
        }
        #nav a:hover
        {
            background-color: #060;
        }
        #nav li.on
        {
            background-color: #C1D9F3;
        }
    </style>
</head>
<body ontouchstart="">
    <div class="wxapi_container">
        <div class="wxapi_index_container">
            <div id="nav">
                <ul>
                    <li><a href="peoplevoicelist.aspx?openid=<%=openid %>&comid=<%=comid %>&clientuptypemark=<%=clientuptypemark %>&materialid=<%=materialid %>">
                        我的语音</a></li>
                    <li class="on"><a href="peoplevoiceup.aspx?openid=<%=openid %>&comid=<%=comid %>&clientuptypemark=<%=clientuptypemark %>&materialid=<%=materialid %>">
                        录制语音</a></li>
                </ul>
            </div>
            <div class="lbox_close wxapi_form">
                <%-- <h3 id="menu-basic">
                    基础接口</h3>
                <span class="desc">判断当前客户端是否支持指定JS接口</span>
                <button class="btn btn_primary" id="checkJsApi">
                    checkJsApi</button>--%>
                <h3 id="menu-voice">
                    <%if (materialid > 0)
                      {
                    %>
                    文章:<%=materialname %>
                    <%
                        }
                      else
                      {
                    %>
                    顾问:<%=username %>
                    <%
                        } %>
                    &nbsp;&nbsp;录制语音</h3>
                <%if (isrightwxset == 1)
                  { %>
                <span class="desc"></span>
                <button class="btn btn_primary" id="startRecord">
                    开始录音</button>
                <span class="desc"></span>
                <button class="btn btn_primary" id="stopRecord">
                    停止录音</button>
                <%}
                  else
                  { %>
                <h3>
                    商户微信接口设置有误!!</h3>
                <%} %>
                <%--<span class="desc">播放语音接口</span>
                <button class="btn btn_primary" id="playVoice">
                    playVoice</button>
                <span class="desc">暂停播放接口</span>
                <button class="btn btn_primary" id="pauseVoice">
                    pauseVoice</button>
                <span class="desc">停止播放接口</span>
                <button class="btn btn_primary" id="stopVoice">
                    stopVoice</button>--%>
                <%--  <span class="desc">上传语音接口</span>
                <button class="btn btn_primary" id="uploadVoice">
                    uploadVoice</button>--%>
                <%-- <span class="desc">下载语音接口</span>
                <button class="btn btn_primary" id="downloadVoice">
                    downloadVoice</button>
                <h3 id="menu-smart">
                    智能接口</h3>
                <span class="desc">识别音频并返回识别结果接口</span>
                <button class="btn btn_primary" id="translateVoice">
                    translateVoice</button>--%>
            </div>
        </div>
</body>
</html>
