<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="indexcard.aspx.cs" Inherits="ETS2.WebApp.M.indexcard" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=comname %></title>
    <meta charset="utf-8"></meta>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"
        name="viewport"></meta>
    <meta content="telephone=no" name="format-detection"></meta>
    <style type="text/css">
        ul, li, span, a, div, em, b, abbr, article, aside, audio, canvas, datalist, details, dialog, eventsource, figure, figcaption, footer, header, hgroup, mark, menu, meter, nav, output, progress, section, small, time, video, legend
        {
            display: block;
        }
        ul li
        {
            text-decoration: none;
            list-style: none;
        }
    </style>
    <link href="../Styles/weixin/StyleBinding.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link  href="/h5/order/css/bottommenu.css" rel="stylesheet">  
    
<script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").val();
            getmenubutton(comid, 'js-navmenu');


            document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
                WeixinJSBridge.call('hideToolbar');
            });

            var card = $("#hid_Card").val();
            if ($("#hid_f").val() != 2) {
                $("#binding").css("display", "none");
            }
            //            if (card == null || card == "") {
            //                $("#mappContainer em").html("登录失效，请关注【微旅行】");
            //                $("#show").css("display", "none");
            //                return;
            //            }
            $("#Integral_down").click(function () {
                var hidin = $("#hid_in").val();
                if (hidin == 0) {
                    $("#integralshow").show();
                    $("#Integral_down").removeClass("power");
                    $("#Integral_down").addClass("power open");
                    $("#Integral_down a").removeClass("close");
                    $("#hid_in").val(1);
                }
                else {
                    $("#integralshow").css("display", "none");
                    $("#Integral_down").removeClass("power open");
                    $("#Integral_down").addClass("power");
                    $("#Integral_down a").addClass("close");
                    $("#hid_in").val(0);
                }

            })
            $("#imprest_down").click(function () {
                var hidim = $("#hid_im").val();
                if (hidim == 0) {
                    $("#imprestshow").show();
                    $("#imprest_down").removeClass("power");
                    $("#imprest_down").addClass("power open");
                    $("#imprest_down a").removeClass("close");
                    $("#hid_im").val(1);
                }
                else {
                    $("#imprestshow").css("display", "none");
                    $("#imprest_down").removeClass("power open");
                    $("#imprest_down").addClass("power");
                    $("#imprest_down a").addClass("close");
                    $("#hid_im").val(0);
                }

            })
            $("#order_down").click(function () {
                var hidim = $("#hid_io").val();

                if (hidim == 0) {
                    $("#ordershow").show();
                    $("#order_down").removeClass("power");
                    $("#order_down").addClass("power open");
                    $("#order_down a").removeClass("close");
                    $("#hid_io").val(1);
                }
                else {
                    $("#ordershow").css("display", "none");
                    $("#order_down").removeClass("power open");
                    $("#order_down").addClass("power");
                    $("#order_down a").addClass("close");
                    $("#hid_io").val(0);
                }

            })




            //加载微信端会员卡素材
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Membershipcardpagelist", { comid: $("#hid_comid").val(), applystate: true, pageindex: 1, pagesize: 100 }, function (data2) {
                data2 = eval("(" + data2 + ")");
                if (data2.type == 1) {
                    $.prompt("素材列表获取出现问题");
                    return;
                }
                if (data2.type == 100) {
                    $("#Ul1").empty();
                    var str = "";
                    for (var i = 0; i < data2.totalCount; i++) {
                        //                        str += '<li id="' + data2.msg[i].MaterialId + '"><a href="/MemberShipCard/MemberShipCardDetail.aspx?materialid=' + data2.msg[i].MaterialId + '">MaterialId:' + data2.msg[i].MaterialId + '--Title:' + data2.msg[i].Title + '</a></li>';
                        str += "<a class='intro' href='h5info.aspx?materialid=" + data2.msg[i].MaterialId + "'>" + data2.msg[i].Title + "</a>";
                    }
                    if (str == "") {
                        $("#Ul1").parent().hide();
                    } else {
                        $("#Ul1").append(str);
                    }
                }
            })
        })
    </script>
</head>
<body class="" id="page_card">
    <div id="mappContainer">
        <%--url('../Images/card_bk_12.png') no-repeat 0 0;--%>
        <div style="height: auto;" class="inner root">
            <div data-card-type="0" class="card" style="background: #fff; -webkit-background-size: 267px 159px;
                background-size: 267px 159px;">
                <img src="<%=comlogo %>" class="logo"><h1 style="color: #bab1b1; text-shadow: 0 1px #e0dada;">
                    <%--<%=comname %>--%></h1>
                <!--<h2></h2>-->
                <strong class="pdo verify"><span style="color: #a5afaf; text-shadow: 0 1px #f9fbfb;">
                    <em style="color: #a5afaf; text-shadow: 0 1px #f9fbfb;">会员卡号</em>
                    <%=AccountCard %></span></strong>
            </div>
            <div style="display: block" id="show">
                <p>
                    <span>使用时请出示此卡号</span>
                </p>
              
                <!--<ul class="cardround" id="privates">
                    <li><span style="display: block; float: left; font-size: 16px; line-height: 27px;
                        position: relative; z-index: 1;">我的优惠券</span> <a style="display: block; padding-right: 17px;
                            font-size: 10px; color: #656565; text-align: right; position: relative; z-index: 2;"
                            href="/M/Default.aspx">领取优惠劵</a> </li>
                </ul>-->
                <ul class="cardround" id="Ul4">
                    <li><span style="display: block; float: left; font-size: 16px; line-height: 27px;
                        position: relative; z-index: 1;">我的电子凭证</span> <a style="display: block; padding-right: 17px;
                            font-size: 10px; color: #656565; text-align: right; position: relative; z-index: 2;"
                            href="/M/EticketList.aspx">查看我的电子凭证</a> </li>
                </ul>
                <ul class="cardround" id="Ul2">
                    <li><span style="display: block; float: left; font-size: 16px; line-height: 27px;
                        position: relative; z-index: 1;">我的订单</span> <a style="display: block; padding-right: 17px;
                            font-size: 10px; color: #656565; text-align: right; position: relative; z-index: 2;"
                            href="/h5/order/order.aspx">查看我的订单</a> </li>
                </ul>

                <%if (unchannel == 1)
                  { %>
                <ul class="cardround" id="Ul5">
                    <li><span style="display: block; float: left; font-size: 16px; line-height: 27px;
                        position: relative; z-index: 1;">我的评价</span> <a style="display: block; padding-right: 17px;
                            font-size: 10px; color: #656565; text-align: right; position: relative; z-index: 2;"
                            href="/h5/order/evaluate.aspx?type=1">查看我的评价</a> </li>
                </ul>
                <ul class="cardround" id="Ul6">
                    <li><span style="display: block; float: left; font-size: 16px; line-height: 27px;
                        position: relative; z-index: 1;">我的预约</span> <a style="display: block; padding-right: 17px;
                            font-size: 10px; color: #656565; text-align: right; position: relative; z-index: 2;"
                            href="/h5/order/Coachappointment.aspx">查看我的预约</a> </li>
                </ul>
                <%}
                  else
                  { %>
                  <ul class="cardround" id="Ul8">
                    <li><span style="display: block; float: left; font-size: 16px; line-height: 27px;
                        position: relative; z-index: 1;">我的评价</span> <a style="display: block; padding-right: 17px;
                            font-size: 10px; color: #656565; text-align: right; position: relative; z-index: 2;"
                            href="/h5/order/evaluate.aspx">查看我的评价</a> </li>
                </ul>
                <ul class="cardround" id="Ul7">
                    <li><span style="display: block; float: left; font-size: 16px; line-height: 27px;
                        position: relative; z-index: 1;">我的档案</span> <a style="display: block; padding-right: 17px;
                            font-size: 10px; color: #656565; text-align: right; position: relative; z-index: 2;"
                            href="/h5/order/evaluate.aspx?type=2">查看我的档案</a> </li>
                </ul>
                <%} %>
                <ul style="display: none;" class="cardround" id="customs">
                </ul>
                <ul class="cardround" id="publics">
                    <li id="Integral_down" class="power" style="background: url('../Images/icon_score.png') no-repeat 9px 14px;
                        -webkit-background-size: 24px 21px; background-size: 24px 21px;"><a style="padding: 8px 0 0 30px;"
                            class="close" href="javascript:void(0)">积分: <span id="Integral_span"></span>
                            <%=Integral%></a>
                        <div id="integralshow" style="display: none;">
                            <%=Integrallist %>
                            <ul>
                                <li>获取积分途径：根据商家活动不同，一般可在微信加关注时获得奖励积分、在 线订购成功时根据产品设置获得一定金额奖励积分。积分不可充值，
                                    可等值抵扣消费。具体咨询商家相关说明。</li>
                            </ul>
                        </div>
                    </li>
                    <li class="power" id="imprest_down" style="background: url('../Images/icon_balance.png') no-repeat 9px 14px;
                        -webkit-background-size: 24px 21px; background-size: 24px 21px;"><a style="padding: 8px 0 0 30px;"
                            class="close" href="javascript:void(0)">预存余额: <span id="imprest_span"></span>
                            <%=Imprest%>元</a>
                        <div id="imprestshow" style="display: none;">
                             <%=Imprestlist %>
                            <ul>
                                <li>根据商家会员服务政策，提供不同方式的消费预存充值途径，如商家官网在线充值、线下实 体门店充值、礼品储值卡充值、微信充值等。会员预存余额消费更方便，具体请咨询商家客
                                    服。 </li>
                            </ul>
                        </div>
                    </li>
                    <!--  <li class="power" id="order_down" style="background: url('../Images/icon_msg.png') no-repeat 9px 14px;
                        -webkit-background-size: 24px 21px; background-size: 24px 21px;"><a style="padding: 8px 0 0 30px;"
                            class="close" href="javascript:void(0)">消费记录
                            </a>
                        <div id="ordershow" style="display: none;">
                             <%=Orderlist%>
                            <ul>
                                <li>只显示最后10条消费订单 </li>
                            </ul>
                        </div>
                    </li>-->
                </ul>
                <ul class="cardround" id="additional">
                    <li><a class="bind" href="userinfo.aspx">完善用户个人资料</a></li>
                    <li id="binding"><a class="bind" href="Binding.aspx">绑定实体会员卡</a></li>
                </ul>
                <ul class="cardround" >
                  <li id="Ul1"></li> 
                  <%if(iscreate_b2bcrmlevel==1&&levelname!=""){ %>
                    <li  id="Ul3"><span style="display: block; float: left; font-size: 16px; line-height: 27px;
                        position: relative; z-index: 1;"><%=levelname %></span> <a style="display: block; padding-right: 17px;
                            font-size: 10px; color: #656565; text-align: right; position: relative; z-index: 2;"
                            href="/M/h5_b2bcrmtequan.aspx?comid=<%=comid %>&crmlevel=<%=crmlevel %>&openid=<%=openid %>">查看会员特权</a> </li>
                    <%} %>
                </ul>
                <ul class="cardround">
                    <li><a class="null" href="/h5/storelist.aspx?comid=<%=comid %>">适用门店电话及地址</a> </li>
                </ul>
            </div>
        </div>
    </div>

    
<!-- 底部菜单 -->
<div class="js-navmenu js-footer-auto-ele shop-nav nav-menu nav-menu-11 has-menu-3">
         <div class="nav-special-item nav-item">
            <a href="/h5/order/Default.aspx" class="home">主页</a>
        </div>
                            <div class="nav-item">
                <a class="mainmenu js-mainmenu" href="/m/indexcard.aspx">
                                        <span class="mainmenu-txt">会员中心</span>
                </a>
                <!-- 子菜单 -->
                            </div>
                            <div class="nav-item">
                <a class="mainmenu js-mainmenu" href="/h5/order/Order.aspx">
                                        <span class="mainmenu-txt">我的订单</span>
                </a>
                <!-- 子菜单 -->
                            </div>
            </div>


    <input type="hidden" id="hid_id" value="<%=AccountId %>" />
    <input type="hidden" id="hid_Card" value="<%=AccountCard %>" />
    <input type="hidden" id="hid_comid" value="<%=comid %>" />
    <input type="hidden" id="hid_in" value="0" />
    <input type="hidden" id="hid_im" value="0" />
    <input type="hidden" id="hid_io" value="0" />
    <input id="hid_f" type="hidden" value="<%=fcard %>" />
</body>
</html>
