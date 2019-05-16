<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="peoplevoicelist.aspx.cs"
    Inherits="ETS2.WebApp.H5.peoplevoicelist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <title>微信语音列表</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=0">
    <link rel="stylesheet" href="http://demo.open.weixin.qq.com/jssdk/css/style.css?ts=1420774989">
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
        wx.error(function (res) {
            alert(res.errMsg);
        });
    </script>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        //获取语音列表
        $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxdownvoicelist", { openid: '<%=openid %>', clientuptypemark: '<%=clientuptypemark %>', materialid: '<%=materialid %>' }, function (data) {
            data = eval("(" + data + ")");
            if (data.type == 1) {
                alert(data.msg);
            }
            if (data.type == 100) {
                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
            }
        })

        function playVoice(updownlogid, mediaid) {
            if (mediaid == "") {
                //                alert("1-1");
                //微信多媒体接口上传语音获得返回的mediaid
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxvoicemediaid", { openid: '<%=openid %>', uplogid: updownlogid, comid: '<%=comid %>' }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                    }
                    if (data.type == 100) {
                        mediaid = data.msg;

                        //                        alert("2-1");
                        //js-sdk下载接口获得音频的本地id
                        if (mediaid == '') {
                            alert('媒体id不存在');
                            return;
                        }
                        wx.downloadVoice({
                            serverId: mediaid,
                            success: function (res) {
                                //                                alert('下载语音成功，localId 为' + res.localId);
                                //                                voice.localId = res.localId;
                                //js-sdk 播放接口
                                wx.playVoice({
                                    localId: res.localId
                                });
                            }
                        });
                    }
                })
            } else {
                //                alert("1-2");
                wx.downloadVoice({
                    serverId: mediaid,
                    success: function (res) {
                        //                        alert("2-2");
                        //                        alert('下载语音成功，localId 为' + res.localId);
                        //                        voice.localId = res.localId;
                        //js-sdk 播放接口
                        wx.playVoice({
                            localId: res.localId
                        });
                    }
                });
            }
        }
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
        <div id="nav">
            <ul>
                <li class="on"><a href="peoplevoicelist.aspx?openid=<%=openid %>&comid=<%=comid %>&clientuptypemark=<%=clientuptypemark %>&materialid=<%=materialid %>">
                    我的语音</a></li>
                <li><a href="peoplevoiceup.aspx?openid=<%=openid %>&comid=<%=comid %>&clientuptypemark=<%=clientuptypemark %>&materialid=<%=materialid %>">
                    录制语音</a></li>
            </ul>
        </div>
        <div class="lbox_close wxapi_form" id="tblist">
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit">  
        <%if(isrightwxset==1){ %>  
            <span class="desc">
            <%if(materialid>0){
            %>
            文章:<%=materialname %>
            <%
            }else{
            %>
            顾问:<%=username %>
            <%
            } %>
            &nbsp;&nbsp;${jsonDateFormatKaler(createtime)}</span>
            <button class="btn btn_primary" id="playVoice" onclick="playVoice('${id}','${mediaid}')">
                播放语音</button>
                <%}else{%>
                <span class="desc"> 商户微信接口设置有误!!</span>
                <%} %>
        </script>
    </div>
</body>
</html>
