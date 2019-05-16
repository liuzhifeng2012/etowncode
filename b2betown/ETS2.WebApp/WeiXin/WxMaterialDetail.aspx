<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WxMaterialDetail.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.WxMaterialDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;" />
    <meta name="format-detection" content="telephone=no" />
    <!-- Mobile Devices Support @begin -->
    <meta content="application/xhtml+xml;charset=UTF-8" http-equiv="Content-Type">
    <meta content="no-cache,must-revalidate" http-equiv="Cache-Control">
    <meta content="no-cache" http-equiv="pragma">
    <meta content="0" http-equiv="expires">
    <meta content="telephone=no, address=no" name="format-detection">
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <!-- apple devices fullscreen -->
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
    <!-- Mobile Devices Support @end -->
    <title>
        <%=title %></title>
    <!--播放语音-->
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
            //            alert(res.errMsg);
        });
    </script>
    <link onerror="_cdnFallback(this)" href="/h5/order/css/css1.css" rel="stylesheet">
    <link onerror="_cdnFallback(this)" href="/styles/play.css" rel="stylesheet">
    <!--播放语音-->
    <style>
         table {
    margin-bottom: 10px;
    border-collapse: collapse;
    display: table;
    width: 100% !important;
}
td, th {
    word-wrap: break-word;
    word-break: break-all;
    padding: 5px 10px;
    border: 1px solid #DDD;
}
        body
        {
            padding: 0px;
            font-family: "宋体";
            background: none repeat scroll 0% 0% #FFF;
            color: #222;
            height: 100%;
            position: relative;
            margin: 0 auto !important;
        }
        body, article, section, h1, h2, hgroup, p, a, ul, li, em, div, small, span, footer, canvas, figure, figcaption, input
        {
            margin: 0;
            padding: 0;
        }
        a
        {
            text-decoration: none;
            cursor: pointer;
        }
        a.autotel
        {
            text-decoration: none;
            color: inherit;
        }
        
        .inner
        {
            padding: 10px 10px;
            margin: 0 auto;
            max-width: 640px;
        }
        h1
        {
            font-size: 22px;
            font-weight: normal;
            line-height: 26px;
            margin-bottom: 18px;
        }
        img
        {
            max-width: 100%;
            border: none;
            margin-bottom: 8px;
            clear: both;
            margin: auto;
        }
        .old_message
        {
            line-height: 20px;
            text-indent: 2em;
            font-size: 14px;
            color: #565752;
            text-align: left;
            margin-bottom: 10px;
            word-wrap: break-word;
        }
        p
        {
            color: #565752;
            text-align: left;
            margin-bottom: 10px;
            word-wrap: break-word;
            line-height: 25px;
        }
        .phone
        {
        }
        .phone a
        {
            font-size: 16px;
            font-weight: bold;
            color: #fff;
            background: #fe932b;
            padding: 5px 10px;
            text-align: center;
            vertical-align: middle;
        }
        .tel
        {
            width: 134px;
            height: 33px;
            line-height: 33px;
            display: inline-block;
            text-align: center;
            font-size: 14px;
            border: 1px solid #4a4a4a;
            margin: 0 7px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            text-shadow: 0 1px #2daf35;
            color: #fff;
            box-shadow: inset 0 0 5px #8ee392;
            background-color: #29a832;
            background-image: -webkit-gradient(linear,0% 0,0% 100%,from(#4cbd51),to(#079414));
        }
        .yuding
        {
            width: 134px;
            height: 33px;
            line-height: 33px;
            display: inline-block;
            text-align: center;
            font-size: 14px;
            border: 1px solid #4a4a4a;
            margin: 0 7px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            text-shadow: 0 1px #2daf35;
            color: #fff;
            box-shadow: inset 0 0 5px #8ee392;
            background-color: #29a832;
            background-image: -webkit-gradient(linear,0% 0,0% 100%,from(#4cbd51),to(#079414));
        }
        #mcover
        {
            position: fixed;
            top: 0px;
            left: 0px;
            width: 100%;
            height: 100%;
            background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.7);
            display: none;
            z-index: 20000;
        }
        #mcover img
        {
            position: fixed;
            right: 18px;
            top: 5px;
            width: 260px !important;
            height: 180px !important;
            z-index: 20001;
        }
        .text
        {
            margin: 15px 0px;
            font-size: 14px;
            word-wrap: break-word;
            color: #727272;
        }
        #mess_share
        {
            margin: 15px 0px;
            display: block;
        }
        #share_1
        {
            float: left;
            width: 49%;
            display: block;
        }
        .button2
        {
            font-size: 16px;
            padding: 8px 0px 0px 0px;
            border: 1px solid #ADADAB;
            color: #000;
            background-color: #E8E8E8;
            background-image: linear-gradient(to top, #DBDBDB, #F4F4F4);
            box-shadow: 0px 1px 1px rgba(0, 0, 0, 0.45), 0px 1px 1px #EFEFEF inset;
            text-shadow: 0.5px 0.5px 1px #FFF;
            text-align: center;
            border-radius: 3px;
            width: 100%;
            vertical-align: middle;
        }
        #share_2
        {
            float: right;
            width: 49%;
            display: block;
        }
        #mess_share img
        {
            width: 22px !important;
            height: 22px !important;
            vertical-align: top;
            border: 0px none;
        }
        
        
        
        
        .share-mp-info
        {
            position: relative;
            background: none repeat scroll 0% 0% #EEE;
            color: #FFF;
            font-size: 0px;
            line-height: 0;
            padding: 1px 105px 1px 1px;
        }
        .page-mp-info
        {
            display: block;
            padding: 4px 10px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        .share-mp-info .links
        {
            font-size: 14px;
            line-height: 24px;
            color: #888;
        }
        
        .share-mp-info .links
        {
            font-size: 14px;
            line-height: 24px;
            color: #888;
        }
        .share-mp-info i
        {
            color: #888;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        .share-mp-info em, .share-mp-info i
        {
            vertical-align: middle;
            font-style: normal;
        }
        .share-mp-info .links
        {
            position: absolute;
            top: 6px;
            right: 10px;
            display: inline-block;
        }
        .share-mp-info .page-mp-info, .share-mp-info .links
        {
            font-size: 14px;
            line-height: 24px;
            color: #888;
        }
        .share-mp-info .links a
        {
            margin-left: 8px;
        }
        .share-mp-info a
        {
            color: #888;
        }
        .btn-follow
        {
            padding: 5px;
            color: #0C3;
            border: 1px solid #0C3;
            margin-right: 5px;
            font-size: 12px;
            line-height: 12px;
        }
        .share-mp-info img.mp-image
        {
            vertical-align: middle;
            margin-right: 3px;
            width: 24px;
            height: 24px;
            border-radius: 100%;
            box-shadow: 0px 0px 3px rgba(0, 0, 0, 0.25);
        }
        .btn
        {
            display: inline-block;
            background-color: #FFF;
            border: 1px solid #E5E5E5;
            border-radius: 2px;
            padding: 4px;
            text-align: center;
            margin-right: 2px;
            color: #999;
            font-size: 12px;
            cursor: pointer;
            line-height: 18px;
            height: 28px;
        }
        .btn-follow
        {
            padding: 5px;
            color: #0C3;
            border: 1px solid #0C3;
            margin-right: 5px;
            font-size: 12px;
            line-height: 12px;
        }
    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/jquery.cookie.2.2.0.min.js"></script>
    <script type="text/javascript">
        var pageindex = 1;
        var pageSize = 16;
        $(function () {


            var comid = $("#hid_comid").val();
            getmenubutton(comid, 'js-navmenu');
            var projectid = $("#hid_projectid").val();
            //根据公司id获得关注作者信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage btn btn-follow \" style=\"height: 25px;\">关注我们</a>");
                }
            });

        });

    </script>
    <script type="text/javascript">
        $(function () {
            $("#divinner").click(function () {
                var articleurl = $("#hid_articleurl").val();
                if (articleurl != "") {
                    window.open(articleurl, target = "_self");
                }
            })
        })
    </script>
    <!--播放语音-->
    <script type="text/javascript">
        wx.ready(function () {
            wx.onVoicePlayEnd({
                success: function (res) {
                    var localId = res.localId; // 返回音频的本地ID
                    $(".js-animation").addClass("hide");
                }
            });
        })

        //获取语音列表
        $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxdownvoicelist", { openid: '<%=openid %>', clientuptypemark: '<%=clientuptypemark %>', materialid: '<%=materialid %>' }, function (data) {
            data = eval("(" + data + ")");
            if (data.type == 1) {
                $("#tblist").hide();
                //                alert(data.msg);

            }
            if (data.type == 100) {
                $("#tblist").show();
                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
            }
        })

        function playVoice(updownlogid, mediaid) {
            $(".js-animation").addClass("hide");
            //从本地cookie中读取localId
            var voice = { localId: "" };
            //比较mediaid 和 cookie中存储的是否相同：相同不在处理；不相同则是最新重新上传了语音，需要重新上传微信服务器获得localId

            if ($.cookie("voicemediaid" + updownlogid) != mediaid) {

                if (mediaid != "") {
                    wx.downloadVoice({
                        serverId: mediaid,
                        success: function (res) {

                            //                        alert('下载语音成功，localId 为' + res.localId);
                            //                        voice.localId = res.localId;

                            $.cookie("voicemediaid" + updownlogid, mediaid, { path: '/', expires: 7 });
                            $.cookie("voicelocateid" + updownlogid, res.localId, { path: '/', expires: 7 });
                            $("#js-animation" + updownlogid).removeClass("hide");
                            $.cookie('playinglogid', updownlogid, { path: '/', expires: 7 });
                            //js-sdk 播放接口
                            wx.playVoice({
                                localId: res.localId
                            });
                        }
                    });
                }
                else {
                    //微信多媒体接口上传语音获得返回的mediaid
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxvoicemediaid", { openid: '<%=openid %>', uplogid: updownlogid, comid: '<%=comid %>' }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            //                        alert(data.msg);
                        }
                        if (data.type == 100) {

                            mediaid = data.msg;


                            //js-sdk下载接口获得音频的本地id
                            if (mediaid == '') {
                                //                                alert('媒体id不存在');
                                return;
                            }
                            wx.downloadVoice({
                                serverId: mediaid,
                                success: function (res) {
                                    //                                alert('下载语音成功，localId 为' + res.localId);
                                    //                                voice.localId = res.localId;
                                    $.cookie("voicemediaid" + updownlogid, mediaid, { path: '/', expires: 7 });
                                    $.cookie("voicelocateid" + updownlogid, res.localId, { path: '/', expires: 7 });
                                    $("#js-animation" + updownlogid).removeClass("hide");

                                    $.cookie('playinglogid', updownlogid, { path: '/', expires: 7 });
                                    //js-sdk 播放接口
                                    wx.playVoice({
                                        localId: res.localId
                                    });
                                }
                            });
                        }
                    })
                }
                return;
            }


            //获得语音的本地id
            if ($.cookie("voicelocateid" + updownlogid)) {

                voice.localId = $.cookie("voicelocateid" + updownlogid);


                //播放中的媒体再次点击，则停止播放中的语音；并不在向下执行
                if ($.cookie("playinglogid")) {
                    if ($.cookie("playinglogid") == updownlogid) {

                        var playinglogid = $.cookie("playinglogid");
                        wx.stopVoice({
                            localId: $.cookie("voicelocateid" + playinglogid) // 需要停止的音频的本地ID，由stopRecord接口获得
                        });
                        $("#js-animation" + updownlogid).addClass("hide");
                        $.cookie('playinglogid', null, { path: '/', expires: -1 });
                        return;
                    }
                }
            }


            //停止播放中的语音；并继续播放新的语音
            if ($.cookie("playinglogid")) {
                //                        alert("1");
                var playinglogid = $.cookie("playinglogid");
                wx.stopVoice({
                    localId: $.cookie("voicelocateid" + playinglogid) // 需要停止的音频的本地ID，由stopRecord接口获得
                });

                $("#js-animation" + updownlogid).addClass("hide");
                $.cookie('playinglogid', null, { path: '/', expires: -1 });
            }

            if (voice.localId != "") {
                //                    alert("2=" + voice.localId);
                $.cookie("voicelocateid" + updownlogid, voice.localId, { path: '/', expires: 7 });
                $("#js-animation" + updownlogid).removeClass("hide");

                $.cookie('playinglogid', updownlogid, { path: '/', expires: 7 });

                //                    wx.downloadVoice({
                //                                serverId: mediaid,
                //                                success: function (res) {
                //js-sdk 播放接口
                wx.playVoice({
                    localId: res.localId
                });
                //                          }
                //                    });
            }
            else {
                if (mediaid == "") {
                    //                        alert("3");
                    //微信多媒体接口上传语音获得返回的mediaid
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxvoicemediaid", { openid: '<%=openid %>', uplogid: updownlogid, comid: '<%=comid %>' }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            //                        alert(data.msg);
                        }
                        if (data.type == 100) {

                            mediaid = data.msg;


                            //js-sdk下载接口获得音频的本地id
                            if (mediaid == '') {
                                //                                alert('媒体id不存在');
                                return;
                            }
                            wx.downloadVoice({
                                serverId: mediaid,
                                success: function (res) {
                                    //                                alert('下载语音成功，localId 为' + res.localId);
                                    //                                voice.localId = res.localId;
                                    $.cookie("voicelocateid" + updownlogid, res.localId, { path: '/', expires: 7 });
                                    $("#js-animation" + updownlogid).removeClass("hide");

                                    $.cookie('playinglogid', updownlogid, { path: '/', expires: 7 });
                                    //js-sdk 播放接口
                                    wx.playVoice({
                                        localId: res.localId
                                    });
                                }
                            });
                        }
                    })
                }
                else {
                    wx.downloadVoice({
                        serverId: mediaid,
                        success: function (res) {

                            //                        alert('下载语音成功，localId 为' + res.localId);
                            //                        voice.localId = res.localId;
                            $.cookie("voicelocateid" + updownlogid, res.localId, { path: '/', expires: 7 });
                            $("#js-animation" + updownlogid).removeClass("hide");
                            $.cookie('playinglogid', updownlogid, { path: '/', expires: 7 });
                            //js-sdk 播放接口
                            wx.playVoice({
                                localId: res.localId
                            });
                        }
                    });
                }
            }
        }
    </script>
    <!--播放语音-->
</head>
<body>
    <div class="header">
        <!-- ▼顶部通栏 -->
        <div class="js-mp-info share-mp-info">
            <a href="/h5/order/default.aspx" class="page-mp-info">
                <img width="24" height="24" src="<%=comlogo%>" class="mp-image">
                <i class="mp-nickname">
                    <%=Com_name%>
                </i></a>
            <div class="links ">
            </div>
        </div>
        <!-- ▲顶部通栏 -->
    </div>
    <!-- ▼播放语音 -->
    <div class="lbox_close wxapi_form" id="tblist" style="display: none;">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">  
    <%if(isrightwxset==1){ %>  
        <div class="custom-audio-weixin clearfix custom-audio-weixin-right">
        <div>
          <img class="custom-audio-logo js-not-share" src="<%=comlogo%>" alt="音频播放" height="40" width="40">
          <span class="custom-audio-bar" onclick="playVoice('${id}','${mediaid}')">
            <img class="js-animation custom-audio-animation hide js-not-share" src="/images/green_player.gif" alt="播放器动画" height="17" width="13" id="js-animation${id}">
            <i class="custom-audio-animation-static js-animation-static"></i>

            <span class="custom-audio-status js-status"></span>
          </span>
          
        </div>

      </div>
    <%} %>

    </script>
    <!-- ▲播放语音 -->
    <div class="inner" id="divinner">
        <h1>
            <%=price %>
            <%=title %></h1>
        <div style="height: 20px; line-height: 15px; font-size: 12px; text-align: left; margin-top: -7px;">
            <%=datetime%>&nbsp;
            <a href="<%=authorpayurl %>" ><%=Author%></a>
        </div>
        <%
            if (headPortraitImgSrc != "")
            {
        %>
        <p>
            <center>
                <strong><span style="color: #ff0000;">
                    <img title="" src="<%=headPortraitImgSrc %>" /></span></strong></center>
        </p>
        <%
            }
        %>
        <p>
            <%=summary %>
        </p>
        <p>
            <%=article %>
        </p>
        <input id="hid_comid" type="hidden" value="<%=comid %>" />
        <input type="hidden" id="hid_articleurl" value="<%=Articleurl %>" />
    </div>
    <style>
        .copyrightbox
        {
            padding: 30px 0px;
            font-size: 14px;
            background: #f0f0f0;
            color: #666666;
            text-align: center;
        }
        .copyrightbox .copyrightbtn
        {
            color: #0066cc;
            padding-right: 15px;
        }
    </style>
    <div class="copyrightbox fn-clear">
        <span id="copydaohang"></span><span class="links"></span>
        <div style="padding-top: 15px; font-size: 12px; display: none;">
            易城商户平台技术支持
        </div>
    </div>
    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=headPortraitImgSrc %>",
                desc: "<%=summary %>",
                title: ' <%=title %>'
            });
        });
    </script>
</body>
</html>
