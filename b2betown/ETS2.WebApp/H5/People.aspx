<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="People.aspx.cs" Inherits="ETS2.WebApp.H5.People" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />
    <title>顾问首页</title>
    <link rel="stylesheet" type="text/css" href="/Styles/fontcss/font-awesome.min.css" />
    <!--[if IE 7]>
		          <link rel="stylesheet" href="/Styles/fontcss/font-awesome-ie7.min.css" />
<![endif]-->
    <script type="text/javascript" src="/Scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="/Scripts/swipe.js"></script>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;"
        name="viewport" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <!--播放语音-->
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=0">
    <link rel="stylesheet" href="http://demo.open.weixin.qq.com/jssdk/css/style.css?ts=1420774989">
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"> </script>
    
    <script src="http://demo.open.weixin.qq.com/jssdk/js/api-6.1.js?ts=1420774989"> </script>
    <script type="text/javascript">
        wx.error(function (res) {
            //            aletr(res.errMsg);
        });
    </script>
    <!--播放语音-->
    <link rel="stylesheet" type="text/css" href="/Styles/H5/People.css" />
    <link rel="stylesheet" type="text/css" href="/h5/order/css/css.css" />
    <link rel="stylesheet" type="text/css" href="/h5/order/css/css1.css" />
    <link onerror="_cdnFallback(this)" href="/styles/play.css" rel="stylesheet">
    <style type="text/css">
        .catebox
        {
            margin-bottom: 0px;
        }
    </style>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;

        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=Headimgurl %>",
                desc: "<%=Selfbrief %>",
                title: '<%=MasterName %>'
            });



            var windth = $(window).width();
            if (windth > 620) {
                windth = 620;
            }
            $(".dyname").width((windth - 160) + "px");
            $(window).resize(function () {
                var windth = $(window).width();
                if (windth > 620) {
                    windth = 620;
                }
                $(".dyname").width((windth - 160) + "px");
            });

            var comid = $("#hid_comid").val();
            var pro_class = $("#firstdaohang").val();
            var isoutpro = $("#hid_isoutpro").val();
            var MasterId = $("#hid_MasterId").val();

            SearchList(1, 30);
            getmenubutton(comid, 'js-navmenu');

            //根据公司id获得关注作者信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage btn btn-follow\">关注我们</a>");
                }
            });



            var stop = true;

            var key = $("#search_name").val();
            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageSize = 30;
                        var pageindex = parseInt($("#pageindex").val()) + 1;
                        stop = false;
                        SearchList(pageindex, pageSize);
                        stop = true;
                    }
                }
            });

        });

        //装载产品列表
        function SearchList(pageindex, pageSize) {
            var comid = $("#hid_comid").val();
            var proclass = $("#firstdaohang").val();
            var isoutpro = $("#hid_isoutpro").val();
            var menuid = $("#firstmenuid").val();
            var MasterId = $("#hid_MasterId").val();
            var channelid = $("#hid_channelid").val();
            if (pageindex == '') {
                pageindex = 1;
            }


            $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=Selectpagelist",
                data: { comid: comid, MasterId: MasterId, pageindex: pageindex, pagesize: pageSize, projectid: proclass, proclass:
proclass, isoutpro: isoutpro, openid: $("#hid_guwenopenid").val(), menuid: menuid, channelid: channelid
                },
                async: false,
                success: function (data) {
                    if (data.type == 1) {
                        return;
                    }

                    var menuid_temp = $("#hid_menuid").val();
                    if (menuid_temp != menuid) {
                        $("#line-list").empty(); //清空规则，当切换栏目进行清空，不切换栏目不清空
                        $("#hid_menuid").val(menuid);
                    }

                    stop = true;

                    if (isoutpro == 0) {
                        data = eval("(" + data + ")");

                        $("#loading").hide();

                        if (data.type == 100) {
                            $("#loading").hide();
                            if (data.totalCount == 0) {
                                return;
                            }
                            if (data.msg != null && data.msg != "") {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#line-list");
                                $("#pageindex").val(pageindex);
                            } else {
                                return;
                            }
                        }
                    }
                    if (isoutpro == 1) {
                        data = eval("(" + data + ")");
                        $("#loading").hide();
                        if (data.TourProductList != null) {

                            $("#TmplScript").tmpl(data.TourProductList.TourProduct).appendTo("#line-list");
                            $("#pageindex").val(pageindex);
                        } else {
                            return;
                        }
                    }
                    if (isoutpro == 2) {
                        data = eval("(" + data + ")");
                        $("#loading").hide();
                        if (data.msg != null && data.msg != "") {

                            $("#WenzhangTmplScript").tmpl(data.msg).appendTo("#line-list");
                            $("#pageindex").val(pageindex);
                        } else {
                            return;
                        }
                    }

                    if (isoutpro == 3) {
                        data = eval("(" + data + ")");
                        $("#loading").hide();
                        if (data.msg != null && data.msg != "") {
                            $("#guwen_spaceTmplScript").tmpl(data.msg).appendTo("#line-list");
                            $("#pageindex").val(pageindex);
                        } else {
                            return;
                        }
                    }
                    if (isoutpro == 4) {
                        data = eval("(" + data + ")");
                        $("#loading").hide();
                        if (data.msg != null && data.msg != "") {
                            $("#guwen_consultTmplScript").tmpl(data.msg).appendTo("#line-list");
                            $("#pageindex").val(pageindex);
                        } else {
                            return;
                        }
                    }
                    if (isoutpro == 5) {
                        data = eval("(" + data + ")");
                        $("#loading").hide();
                        if (data.msg != null && data.msg != "") {
                            $("#pingjiaTmplScript").tmpl(data.msg).appendTo("#line-list");
                            $("#pageindex").val(pageindex);
                        } else {
                            return;
                        }
                    }
                }
            })
        }


        function linkjump(id, type, Projectid) {
            //$("#loading").show();
            //根据服务类型不同进入不同页面
            if (type == 1) {//票务
                location.href = "/h5/Order/pro.aspx?id=" + id + "&projectid=" + $("#hid_projectid").val() + "&MasterId=" + MasterId + "&tocomid=0";
                return;
            } else if (type == 2 || type == 8) {//旅游
                location.href = "/h5/linedetail.aspx?lineid=" + id + "&projectid=" + $("#hid_projectid").val() + "&buyuid=0&tocomid=0";
                return;
            } else if (type == 9) {//酒店客房
                location.href = "/h5/hotel/hotelshow.aspx?projectid=" + Projectid + "&uid=0";
                return;
            }
            else if (type == "外部产品") {//外部接口产品(暂时只有易游产品)
                location.href = "http://etschina.com/ui/poui/tourlines/productview.aspx?id=" + id + "&userId=0";
                return;
            } else if (type == "文章") {
                location.href = "/weixin/wxmaterialdetail.aspx?materialid=" + id;
                return;
            }
            else {
                location.href = "/h5/Order/pro.aspx?id=" + id + "&projectid=" + $("#hid_projectid").val() + "&buyuid=0&tocomid=0";
                return;
            }
        }

        function bangdingsub(openid, name, company, masterid, type) {

            if (type == 1) {
                if (confirm("您好，我是 " + name + " ， \n您是否选择我成为你的服务顾问？\n确认后，请关闭本页面后在微信上直接给我留言...")) {
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/CrmMemberHandler.ashx?oper=bangdingchannelid",
                        data: { comid: $("#hid_comid").val(), openid: openid, masterid: masterid },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                return;
                            }
                            if (data.type == 100) {

                                return;
                            }
                        }
                    })

                    //发送消息模板
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/CrmMemberHandler.ashx?oper=kehumessage",
                        data: { comid: $("#hid_comid").val(), openid: openid },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                return;
                            }
                            if (data.type == 100) {

                                return;
                            }
                        }
                    })

                }
            } else {

                alert("我是您的服务顾问 " + name + " ，\n如果希望与我沟通，请关闭本页面后在微信上直接给我留言。\n我会尽快回复...")

                //发送消息模板
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=kehumessage",
                    data: { comid: $("#hid_comid").val(), openid: openid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {

                            return;
                        }
                    }
                })
            }

            //关闭此窗口
            WeixinJSBridge.invoke('closeWindow', {}, function (res) {

                //alert(res.err_msg);

            });

        }
        function Getfirstimg(imgs) {
            if (imgs != null) {
                if (imgs.indexOf(',') == -1) {
                    return imgs;
                } else {

                    return imgs.substring(0, imgs.indexOf(','));
                }
            } else {
                return "/Images/defaultThumb.png";
            }
        }




    </script>
    <script type="text/javascript" src="/Scripts/jquery.cookie.2.2.0.min.js"></script>
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
                var playinglogid = $.cookie("playinglogid");
                wx.stopVoice({
                    localId: $.cookie("voicelocateid" + playinglogid) // 需要停止的音频的本地ID，由stopRecord接口获得
                });

                $("#js-animation" + updownlogid).addClass("hide");
                $.cookie('playinglogid', null, { path: '/', expires: -1 });
            }

            if (voice.localId != "") {
                $.cookie("voicelocateid" + updownlogid, voice.localId, { path: '/', expires: 7 });
                $("#js-animation" + updownlogid).removeClass("hide");


                $.cookie('playinglogid', updownlogid, { path: '/', expires: 7 });
                //                wx.downloadVoice({
                //                    serverId: mediaid,
                //                    success: function (res) {
                //js-sdk 播放接口
                wx.playVoice({
                    localId: voice.localId
                });
                //                    }
                //                });
            }
            else {
                if (mediaid == "") {
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
    <style>
        .btn
        {
            height: 28px;
        }
    </style>
</head>
<body class="sbody">
    <% if (come == "list")
       { %>
    <header style="height: 35px; line-height: 35px;">
    
    <div class="js-mp-info share-mp-info">
            <a class="page-mp-info" href="/h5/order/default.aspx">
                <img class="mp-image" src="<%=comlogo %>"g" height="24" width="24">
                <i class="mp-nickname">
                    <%=title %>                </i>
            </a>
           <div class="links"></div>
        </div>
    

</header>
    <%}
       else
       { %>
    <header class="header">                   
</header>
    <%} %>
    <div class="dytop">
        <div class="dyhead">
            <div class="dytitle">
            </div>
            <div class="avatar">
                <span style="background-image: url(<%=Headimgurl %>)"></span>
            </div>
            <div class="dyname" style="width: 240px;">
                <div style="border-bottom: 1px solid #CCC; padding: 10px 0 2px 0; display: block;">
                    <span class="name" style="text-overflow: ellipsis; overflow: hidden; white-space: nowrap;">
                        <b>
                            <%=MasterName %></b> </span><span class="font12">
                                <%=Job %></span> <span style="float: right;">
                                    <%if (yonghustate == 0)
                                      { %>
                                    <a href="<%=linkguanzhu %>">
                                        <div class="weixinguanzhu">
                                            <img class="weixinkefu" src="/Images/wexin.png">关注</div>
                                    </a>
                                    <%}
                                      else if (yonghustate == 1 && WorkdaysView == 1)//绑定账户并且是工作日内
                                      {%>
                                    <a href="javascript:;" onclick="document.getElementById('mcover').style.display='none';bangdingsub('<%=weixinopenid %>','<%=MasterName %>','<%=ChannelCompanyName %>','<%=channelid %>','0');">
                                        <div class="weixinguanzhu">
                                            <img class="weixinkefu" src="/Images/wexin.png" />咨询
                                        </div>
                                    </a>
                                    <%}
                                      else if (yonghustate == 2 && WorkdaysView == 1)//未绑定（进行绑定），并且工作日内
                                      {%>
                                    <a href="javascript:;" onclick="document.getElementById('mcover').style.display='none';bangdingsub('<%=weixinopenid %>','<%=MasterName %>','<%=ChannelCompanyName %>','<%=channelid %>','1');">
                                        <div class="weixinguanzhu">
                                            <img class="weixinkefu" src="/Images/wexin.png" />咨询
                                        </div>
                                    </a>
                                    <%}
                                      else if (yonghustate == 3 || WorkdaysView == 0)//顾问未关注微信或休息日
                                      {%>
                                    <a href="javascript:;">
                                        <div class="weixinguanzhu">
                                            <img class="weixinkefu imgjiahui" src="/Images/wexin_hui.png" /></div>
                                    </a>
                                    <%}%>
                                </span>
                </div>
                <div style="padding-top: 7px; text-overflow: ellipsis; overflow: hidden; white-space: nowrap;">
                    <span class="at">
                        <%=CompanyName %>
                        <%=ChannelCompanyName %></span>
                </div>
            </div>
            <div id="dy-link">
                <p class="font14" style="height: 45px;">
                    <%=Selfbrief %></p>
                <p style="float: right; margin-top: -20px;">
                    <span>
                        <img alt="" src="/Images/phone1.gif" style="vertical-align: bottom;" /></span>
                    <a href="tel:<%if (Viewtel == 1){ %>
                                                                                                                                                                                                    <%=Tel %>
                                                                                                                                                                                                    <% }else{ %>
                                                                                                                                                                                                    <%=Fixphone%>
                                                                                                                                                                                                    <%} %> "
                        target="_blank" class="tel">
                        <%if (Viewtel == 1)
                          { %>
                        <%=Tel %>
                        <% }
                          else
                          { %>
                        <%=Fixphone%>
                        <%} %>
                    </a>
                    <img style="vertical-align: bottom;" str="" src="/Images/zuobiao.gif" /><a href="/h5/StoreInfo.aspx?menshiid=<%=ChannelCompanyId %>&type=3&wxref=mp.weixin.qq.com">我在这</a>
                </p>
            </div>
        </div>
    </div>
    <div style="padding: 8px;">
        <ul class="indexnav">
            <%=daohang_html %>
        </ul>
    </div>
    <div style="visibility: visible;" id="slider" class="swipe">
        <div style="max-width: 640px;" class="swipe-wrap">
            <ul id="line-list" class=" line-list sc-goods-list pic clearfix size-1 ">
            </ul>
            <%--<div id="line-list" data-index="0" style="max-width: 640px; left: 0px; transition-duration: 0ms; transform: translateX(0px);" class="line-list">
                          

    
              </div>--%>
        </div>
    </div>
    <script src="/Scripts/swipe1.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            window.mySwipe = Swipe(document.getElementById('slider'), {
                callback: function (index) {
                    $(".indexnav li").removeClass('curr').eq(index).addClass("curr");
                }
            });
            $(".indexnav li").bind("click", function () {
                var i = $(this).index();
                //mySwipe.slide(i);
                $(".indexnav li").removeClass('curr').eq(i).addClass("curr");
                var dataid = $(".indexnav li").eq(i).attr("data-id");
                var menuid = $(".indexnav li").eq(i).attr("menu-id");

                var dataisoutpro = $(".indexnav li").eq(i).attr("data-isoutpro");
                $("#firstdaohang").val(dataid);
                $("#firstmenuid").val(menuid);


                $("#hid_isoutpro").val(dataisoutpro);
                SearchList(pageindex, 12);
            });
        });
    </script>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 

        <li class="goods-card goods-list small-pic card ">
                    {{if (Server_type==2 || Server_type==8)}}
                    <a href="/h5/linedetail.aspx?lineid=${Id}&MasterId=<%=MasterId %>" class="link js-goods clearfix" target="_blank" data-goods-id="6655060" title="${Pro_name}">
                    {{else}}
                      {{if Server_type==9}}
                          <a href="/h5/hotel/Hotelshow.aspx?proid=${Id}&id=${Comid}&MasterId=<%=MasterId %>" class="link js-goods clearfix" target="_blank" data-goods-id="6655060" title="${Pro_name}">
                        {{else}}
                           {{if  Server_type==10}}
                            <a href="/h5/OrderEnter.aspx?id=${Id}&MasterId=<%=MasterId %>" class="link js-goods clearfix" target="_blank" data-goods-id="6655060" title="${Pro_name}">
                             {{else}}
                                <a href="/h5/Order/pro.aspx?id=${Id}&MasterId=<%=MasterId %>" class="link js-goods clearfix" target="_blank" data-goods-id="6655060" title="${Pro_name}">
                           {{/if}}
                      {{/if}}
				   
                    {{/if}}
					<div class="photo-block">
                        {{if (Ispanicbuy==1 || Ispanicbuy==2)}}
                            {{if (Limitbuytotalnum<=0)}}
                        <div class="Soldout"><img  src="order/image/5337034_100415061309_2.gif"  style="display: block;width:80px;padding-top:5px;padding-left:5px;"></div>
                            {{/if}}
                        {{/if}}
						<img class="goods-photo js-goods-lazy" data-width="640" data-height="640" data-src="${Imgurl}" src="${Imgurl}" style="display: block;">
					</div>
					<div class="info clearfix info-title info-price btn4">
						<p class=" goods-title ">${Pro_name}</p>
						<p class="goods-sub-title c-black hide">  ${Pro_explain}</p>
						<p class="goods-price">
                        {{if (Server_type==9)}}
                        <em>￥${ViewPrice(HousetypeNowdayprice)}</em>
                           {{else}}
                           <em>￥${ViewPrice(Advise_price)}</em>
                        {{/if}}    
						</p>
						<p class="goods-price-taobao">特价：￥${ ViewPrice(Advise_price)}</p>   
                        {{if (Server_type==11)}}
                          <div class="goods-buy btn1"></div>
                        {{/if}}
					</div></a>
					<div class="goods-buy "> </div>  
						
			</li>


    </script>
    <script type="text/x-jquery-tmpl" id="TmplScript"> 

                        <div class="line-item " style="height: 60px;" onclick="linkjump('${Id}','外部产品','')";>
                                <a href="javascript:;">
                                    <img class="fn-left img" data-img="${Imgs}" src="${Getfirstimg(Imgs)}">
                                    <div class="txt">
                                        <p class="p1">${Name}</p>
                                        <p class="p2">
                                            <label class="fn-right pri">
                                                <span class="n">
                                                 <span class="u">￥</span>${Price}<span>起</span>
                                                </span>
                                            </label>
                                        </p>
                                         <p class="p2">
                                         
                                         </p>
                                    </div>
                                </a>   
                           </div>

    </script>
    <script type="text/x-jquery-tmpl" id="WenzhangTmplScript"> 
             <li class="goods-cardwx  normalwx"> 
                 <div class="custom-messages single">
                    <a href="/weixin/wxmaterialdetail.aspx?materialid= ${MaterialId}" class="clearfix">
                        <div class="custom-messages-image">
                                                <div class="image">
                                <img src="${Imgpath}" style="display: inline;" class="js-lazy" data-src="${Imgpath}">
                            </div>
                        </div>
                        <div class="custom-messages-content">
                            <h4 class="title">
                                ${Title}                    </h4>
                                                <div class="summary">
                                ${cutstr(Summary,80) }                 
                                 {{if Price!=0}}
                                    ¥${Price}<em>起</em>
                                {{/if}}
                        
                                 </div>
                    
                        </div>
                    </a>
                </div>
               </li>


    </script>
    <script type="text/x-jquery-tmpl" id="guwen_spaceTmplScript"> 
                          
       <%if(isrightwxset==1){ %>          
                              {{if clientuptypemark==1}}
                               <div class="line-item" style="height: 60px;" >
                                  <div style="width: 50px; float: left;">
                                     
                                    <img style="border: 0px none;" src="<%=Headimgurl %>" height="65px;" width="65px"> 
                                 </div>
                                 <div class="txt">
                                         <div class="custom-audio-weixin clearfix" style="padding-top: 15px;">
                                            <div>
                                             
                                              <span class="custom-audio-bar" onclick="playVoice('${id}','')" >
                                                <img class="js-animation custom-audio-animation hide js-not-share" src="/images/player.gif" alt="播放器动画" height="17" width="13"  id="js-animation${id}">
                                                <i class="custom-audio-animation-static js-animation-static"></i>

                                                <span class="custom-audio-status js-status"></span>
                                              </span>
          
                                            </div>

                                          </div>
                                  </div>
                               </div>
                              {{/if}}
     <%} %>
                              {{if clientuptypemark==3}}
                               <div class="line-item" style="height: 60px;" >
                                 <div style="width: 50px; float: left;">
                                   
                                    <img style="border: 0px none;" src="<%=Headimgurl %>" height="65px;" width="65px">
                                     
                                </div>
                                <div class="txt">
                                        <p class="p1">上传图片</p>
                                        <p class="p2">
                                            <label class="fn-right pri">
                                                <span class="n">
                                                 <img class="fn-left img" data-img="${relativepath}" src="${relativepath}">
                                                </span>
                                            </label>
                                        </p>
                                         <p class="p2">
                                         </p>
                                    </div>
                               </div>
                              {{/if}}
                         {{if clientuptypemark==4}}
                          <div class="line-item" style="height: 60px;" >
                                 <div style="width: 50px; float: left;">
                                    
                                    <img style="border: 0px none;" src="<%=Headimgurl %>" height="65px;" width="65px">
                                 
                                </div>
                                <div class="txt">
                                        <p class="p1">文字留言</p>
                                        <p class="p2">
                                            <label class="fn-right pri">
                                                <span class="n">
                                                
                                                    ${txtcontent} 
                                                
                                                </span>
                                            </label>
                                        </p>
                                         <p class="p2">
                                         </p>
                                  </div>
                              </div>
                              {{/if}}
                         
    </script>
    <script type="text/x-jquery-tmpl" id="guwen_consultTmplScript"> 

                        <div class="line-item" style="height: 60px;" >
                                 <div style="width: 50px; float: left;">
                        {{if WxHeadimgurl !=""}}
                        <img style="border: 0px none;" src="${WxHeadimgurl}" height="65px;" width="65px">
                        {{/if}}
                        </div>
                                    <div class="txt">
                                        <p class="p1"> 
                                        {{if  MsgType=="text"}}
                                                 ${Content}                  
                                         {{/if}} 
                                          {{if  MsgType=="voice"}}
                                                 ${Recognition}                  
                                         {{/if}} 
                                         </p>
                                        <p class="p2">
                                            <label class="fn-right pri">
                                                <span class="n">
                                               
                                                 ${jsonDateFormatKaler(CreateTimeFormat)}
                                                </span>
                                            </label>
                                        </p>
                                         <p class="p2">
                                         
                                         </p>
                                    </div>
                                
                           </div>

    </script>
     <script type="text/x-jquery-tmpl" id="pingjiaTmplScript"> 

                        <div class="line-item" style="height: 60px;" >
                                 <div style="width: 50px; float: left;">
                        {{if Imgurl !=""}}
                        <img style="border: 0px none;" src="${Imgurl}" height="65px;" width="65px">
                        {{/if}}
                        </div>
                                    <div class="txt">
                                        <p class="p1" style="color: #f60; height: 20px;"> 

                                                 ${uname}                  
                                         </p>
                                          <p class="p1" style=" height: 20px;"> 
                                                 ${text}                  
                                         </p>
                                        <p class="p2" style=" height: 20px;">
                                            <label class="fn-right pri">
                                                <span class="n">
                                               
                                                 ${jsonDateFormatKaler(subtime)}
                                                </span>
                                            </label>
                                        </p>
                                         <p class="p2">
                                         
                                         </p>
                                    </div>
                           </div>

    </script>
    <div class="copyrightbox fn-clear">
        <span id="copydaohang"></span><span class="links"></span>
        <div style="padding-top: 15px; font-size: 12px;">
            易城商户平台技术支<a href="peoplelist.aspx">持</a>
        </div>
    </div>
    <div id="mcover" class="mcover" onclick="document.getElementById('mcover').style.display='';"
        style="display: none;">
        <img src="/images/weixinkefu.png">
    </div>
    <div id="loading" class="loading">
        正在加载..
    </div>
    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=Headimgurl %>",
                desc: "<%=Selfbrief %>",
                title: '<%=MasterName %>'
            });
        });
    </script>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="firstdaohang" type="hidden" value="<%=firstdaohang %>" />
    <input id="firstmenuid" type="hidden" value="<%=firstmenuid %>" />
    <input id="hid_isoutpro" type="hidden" value="<%=isoutpro %>" />
    <input id="hid_MasterId" type="hidden" value="<%=MasterId %>" />
    <input id="hid_channelid" type="hidden" value="<%=channelid %>" />
    
    <input id="pageindex" type="hidden" value="1" />
    <input id="num" type="hidden" value="10" />
    <input id="hid_openid" type="hidden" value="<%=weixinopenid %>" />
    <input id="hid_guwenopenid" type="hidden" value="<%=guwenweixin %>" />
    <input id="hid_menuid" type="hidden" value="" />
    <input id="usern" type="hidden" value="0" />
    <input id="usere" type="hidden" value="0" />

    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {
            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=Headimgurl %>",
                desc: "<%=Selfbrief %>",
                title: ' <%=MasterName %>'
            });
        });
    </script>
</body>
</html>
