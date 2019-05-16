<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lineBook.aspx.cs" Inherits="ETS2.WebApp.H5.lineBook" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="/Styles/h5/book.css">
    <link rel="stylesheet" type="text/css" href="/Styles/fontcss/font-awesome.min.css">
    <title>线路预订</title>
    <meta name="HandheldFriendly" content="true" />
    <meta name="MobileOptimized" content="width" />
    <meta id="viewport" content="width=device-width, user-scalable=yes,initial-scale=1"
        name="viewport" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="/Scripts/common.js"></script>
    <script type="text/javascript">
    $(function () {

        $("#loginOrder").click(function () {
            var lineName = $.trim($("#lineName").val());
            var budget = $.trim($("#budget").val());
            var budget_child = $.trim($("#budget_child").val());
            var lineid = $.trim($("#lineid").val());
            var userPhone = $.trim($("#userPhone").val());
            var userName = $.trim($("#userName").val());
            var advise_price = $.trim($("#advise_price").val());
            var outdate = $.trim($("#outdate").val());
            var getcode = "";

            $("#submission").show();

            if (outdate == "" || lineid == "0") {
                $("#maskWord").html("请啊返回重新选择出行日期");
                $("#maskWord").show();
                $("#submission").hide();
                return;
            }

            if (userName == "" || userName == "请填写姓名") {
                $("#errorName").show(300).delay(2000).hide(500);
                $("#submission").hide();
                return;
            }
            if (userPhone == '' || userPhone == "联系人手机") {
                $("#errorPhone").show(300).delay(2000).hide(500);
                $("#submission").hide();
                return;
            }

            if (isMobel(userPhone)) {
            } else {
                $("#errorPhoneNum").show(300).delay(2000).hide(500);
                $("#submission").hide();
                return;
            }


            $.post("/JsonFactory/OrderHandler.ashx?oper=lineorder", { proid: lineid, ordertype: 1, u_num: budget, budget_child: budget_child, u_name: userName, u_phone: userPhone, u_traveldate: outdate, getcode: getcode, openid: $("#openid").val() }, function (data) {
                data = eval("(" + data + ")");
                if (parseInt(data.type) == 1) {
                    $("#maskWord").html(data.msg);
                    $("#maskWord").show();
                    $("#submission").hide();
                    alert(data.msg);
                }
                if (parseInt(data.type) == 100) {
                    $("#submission").hide();
//                    alert("订单提交成功，稍后客服会电话与您确认！");
                    if(<%=linepro_booktype %>==1){
                    location.href = "lineBooksuccess.aspx?lineid=" + lineid + "&adultnum=" + budget + "&childnum=" + budget_child + "&outdate=" + outdate;
                    }else{
                    location.href = "pay.aspx?orderid=" + data.msg + " &comid=" + <%=com_id %>;
                  
                    }
                    return;
                }
            })
            function callbackfunc(e, v, m, f) {
                if (v == true)
                    location.reload();
            }
        })

    })

    var viewPortScale;
    var dpr = window.devicePixelRatio;
    viewPortScale = 0.5;
    //
    var detectBrowser = function (name) {
        if (navigator.userAgent.toLowerCase().indexOf(name) > -1) {
            return true;
        } else {
            return false;
        }
    };
    if (detectBrowser('hm')) {
        document.getElementById('viewport').setAttribute(
        'content', 'target-densitydpi=283,user-scalable=no, width=640, minimum-scale=1, initial-scale=1');
    } else if (detectBrowser('ipad')) {
        document.getElementById('viewport').setAttribute(
        'content', 'width=device-width, user-scalable=no,initial-scale=1');
    } else if (detectBrowser('ucbrowser')) {
        document.getElementById('viewport').setAttribute(
        'content', 'user-scalable=no, width=device-width, minimum-scale=0.5, initial-scale=' + viewPortScale);
    } else if (detectBrowser('360browser')) {
        document.getElementById('viewport').setAttribute(
        'content', 'target-densitydpi=320,user-scalable=no, width=640, minimum-scale=1, initial-scale=1');
    } else {
        document.getElementById('viewport').setAttribute(
        'content', 'target-densitydpi=320, user-scalable=no,width=640, minimum-scale=0.5, initial-scale=' + viewPortScale);
    }

    </script>
</head>
<body>
    <div id="header">
        <div class="header clearfix" id="topHeader">
            <div class="back_icon_control">
                <span class="icon-arrow-left"></span><a href="javascript:self.location=document.referrer;"
                    class="back_icon_info">返回</a>
            </div>
            <span class="title">填写订单</span>
        </div>
    </div>
    <div class="wrapper" id="wrapper">
        <div id="index">
            <div class="main_cont">
                <ul class="mc_lists">
                    <li>
                        <p class="line_name">
                            <a href="linedetail.aspx?lineid=<%=lineid%>">
                                <%=pro_name%></a> <span class="line_label">跟团旅游 </span>
                        </p>
                        <div class="line_price clearfix">
                            <p class="lp_top_left" style="width: 500px;">
                                <span>出发日期:</span> <span class="startime">
                                    <%=outdate %></span>
                            </p>
                            <p class="lp_top_left" style="width: 500px;">
                                <span>出游人数:</span> <span class="adult_num">
                                    <%=adultnum %></span> <span>成人</span> <span class="child_num">
                                        <%=childnum %></span> <span>儿童</span>
                            </p>
                            <p class="lp_top_left" style="width: 500px;">
                                <span>产品编号:</span> <span class="product_num">
                                    <%=travelproductid%></span>
                            </p>
                            <p class="lp_top_left" style="width: 500px;">
                                <span>订单价格:</span> <span class="order_pri">
                                    <%=pricedetail%></span>
                            </p>
                        </div>
                        <p class="price_info" style="margin-top: 30px; color: #666;">
                            <span style="font-size: 140%">价格说明:</span> <span style="font-size: 132%; padding-left: 8px;">
                                订单提交后，客服会电话与您确认最终</span>
                            <p style="padding-left: 133px; font-size: 132%; color: #666;">
                                优惠价格</p>
                        </p>
                    </li>
                </ul>
            </div>
            <div class="user_info">
                <div id="submission">
                    <div class="submission">
                        <div class="loading">
                            <img src="/Images/h5/loading.gif" alt="">
                        </div>
                        <div class="submit_word">
                            订单提交中...
                        </div>
                    </div>
                </div>
                <div id="mask">
                    <p id="maskWord">
                    </p>
                </div>
                <p class="user_info_name">
                    <span>预订人姓名：</span>
                    <input type="text" value="" id="userName" class="user_input default" placeholder="请填写预订人姓名">
                    <p class="alert_name" id="errorName" style="display: none">
                        <span class="man_arrow"></span>未填写预订人姓名</p>
                </p>
                <p class="bottom_input">
                    <span>预订人手机：</span>
                    <input type="tel" value="" id="userPhone" class="user_input default" placeholder="请填写预订人手机号">
                    <p class="alert_phone" id="errorPhone" style="display: none">
                        <span class="man_arrow"></span>请填写预订人手机号</p>
                    <p class="alert_phone" id="errorPhoneNum" style="display: none">
                        <span class="man_arrow"></span>号码格式错误，请输入11位的手机号码</p>
                </p>
                <div class="info_frame" id="confirmInfoFrame">
                    <div class="login_confirm">
                        <p>
                            手机号码用于接收订单确认信息，请保证手机号码真实有效。提交订单后，客服会电话与您联系，请保持手机畅通。</p>
                    </div>
                    <div class="member_info" id="confirmCheck">
                        <span id="checkBtn" class="icon icon-checkbox-checked"></span><span>我同意并确认以上信息</span>
                    </div>
                </div>
                <div class="order_confirm" style="text-align:center;">
                    <%if (IsYouXiao == 1)
                      {
                    %>
                    <a href="javascript:void(0)" class="login_order" id="loginOrder">提交订单</a>
                    <%
                        }
                      else
                      { 
                    %>
                   <span class="title" style="color:Red;">线路团期已满，请选择其他出行路线</span>
                         
                    <%
                        } %>
                    <input type="hidden" id="lineName" value="<%=pro_name%>">
                    <input type="hidden" id="budget" value="<%=adultnum %>">
                    <input type="hidden" id="budget_child" value="<%=childnum %>">
                    <input type="hidden" id="lineid" value="<%=lineid %>">
                    <input type="hidden" id="advise_price" value="<%=advise_price %>">
                    <input type="hidden" id="outdate" value="<%=outdate %>">
                    <input type="hidden" id="openid" value="<%=openid %>">
                </div>
            </div>
        </div>
    </div>
</body>
</html>
