<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lineBooksuccess.aspx.cs" Inherits="ETS2.WebApp.H5.lineBooksuccess" %>

<!DOCTYPE HTML>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

<link rel="stylesheet" type="text/css" href="/Styles/h5/book.css">
<link rel="stylesheet" type="text/css" href="/Styles/fontcss/font-awesome.min.css">
<title>预订成功</title>
<meta name="HandheldFriendly" content="true" />
<meta name="MobileOptimized" content="width" />
<meta id="viewport" content="width=device-width, user-scalable=yes,initial-scale=1" name="viewport" />
<meta name="apple-mobile-web-app-capable" content="yes">
<meta content="black" name="apple-mobile-web-app-status-bar-style" />
<meta content="telephone=no" name="format-detection" />
<script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
<script type="text/javascript" src="/Scripts/common.js"></script>

<script type="text/javascript">
   

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
                <span class="icon-arrow-left"></span><a href="javascript:window.history.go(-1);" class="back_icon_info">返回</a>
            </div>
            <span class="title">预订提交成功</span>
    </div>    
</div>
<div class="wrapper" id="wrapper">
	<div id="index">	
	
<div class="main_cont">
    <ul class="mc_lists">
        <li>
            <p class="line_name">
                <a href="linedetail.aspx?lineid=<%=lineid%>"><%=pro_name%></a>
                <span class="line_label">
                    跟团旅游                </span>
            </p>
            <div class="line_price clearfix">
                <p class="lp_top_left">
                    <span>出发日期:</span>
                    <span class="startime"><%=outdate %></span>
                </p>
                <p class="lp_top_right">
                    <span>出游人数:</span>
                    <span class="adult_num"><%=adultnum %></span>
                    <span>成人</span>
                    <span class="child_num"><%=childnum %></span>
                    <span>儿童</span>
                </p>
                <br>
                <p class="lp_btm_left">
                    <span>产品编号:</span>
                    <span class="product_num"><%=travelproductid%></span>
                </p>
                                <p class="lp_btm_right">
                    <span>订单价格:</span>
                    <span class="order_pri">￥<%=advise_price %></span>
                </p>
            </div>
            <p class="price_info" style="margin-top: 30px;color: #666;">
                <span style="font-size: 1.8rem;color: #FF7800;padding-left: 8px;">订单提交成功，客服会电话与您确认!</span>

            </p>
        </li>
    </ul>
</div>
</div>
	</div>
</body>
</html>

