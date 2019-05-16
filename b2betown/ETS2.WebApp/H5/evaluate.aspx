<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="evaluate.aspx.cs" Inherits="ETS2.WebApp.H5.evaluate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html class="no-js " lang="zh-CN"><head>
    <meta charset="utf-8">
    <meta name="keywords" content="<%=title %>">
    <meta name="description" content="">
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">

    <title><%=title %></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
     <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var channleid = $("#hid_channleid").val();
            var comid = $("#hid_comid").val();

            getmenubutton(comid, 'js-navmenu');
            SearchList(1);


            //装载产品列表
            function SearchList(pageindex) {
                $("#loading").show();
                var key = $("#key").val();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $("#pageindex").val(pageindex);
                $.post("/JsonFactory/OrderHandler.ashx?oper=evaluatePageList", { pageindex: pageindex, openid: $("#hid_openid").val(), comid: comid, channleid: channleid, evatype: 0 }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $("#loading").hide();
                        $("#content").html("<div class=\"empty-list\" style=\"margin-top:30px;\">哎呀，没有查到评价！</div>");
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();

                        if (data.totalCount == 0) {
                            $("#content").html("<div class=\"empty-list\" style=\"margin-top:30px;\"> <h4>哎呀，没有查到评价！</h4></div>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#content");
                            setpage(data.totalCount, pageSize, pageindex);
                        }
                    }
                })
            }



            var stop = true;


            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#pageindex").val()) + 1;
                        var pageSize = parseInt($("#num").val()) + 10;

                        stop = false;

                        SearchList(pageindex);

                    }
                }
            });


            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchList(page);

                        return false;
                    }
                });
            }


        })



    </script>

    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    
    <!-- CSS -->
    <link rel="stylesheet" href="/Styles/cart.css" onerror="_cdnFallback(this)">  
    <link  href="order/css/bottommenu.css" rel="stylesheet">  
    <link  href="order/css/css.css" rel="stylesheet"> 
  </head>

<body class=" ">
        <!-- container -->
    <div class="container ">
    <div class="header">
        <!-- ▼顶部通栏 -->
                                
            <div class="js-mp-info share-mp-info">
				<a href="Default.aspx" class="page-mp-info">
					<img width="24" height="24" src="<%=logoimg %>" class="mp-image">
					<i class="mp-nickname">
						<%=title %>                </i>
				</a>
            <div class="links">
                
                                                                </div>
        </div>
        <!-- ▲顶部通栏 -->
</div> 
                <div class="content"  style="min-height: 288px;">
					      
      <div id="backs-list-container" style="margin-top: 20px;">
			<li class="block block-order animated">
		        <div id="content"></div>
   
	        </li>
		
		
		<div class="empty-list" style="margin-top:30px; display:none;">
    <!-- 文本 -->
    <div>
        <h4>没有查询到记录？</h4>
        <p class="font-size-12">您也来评价吧</p>
    </div>
    </div>
    </div>        <div class="footer">
        <!-- 商家公众微信号 -->
<div class="copyright">
<div class="ft-copyright">
   <a href="#">易城商户平台技术支持</a>
</div>
</div>    </div>            </div>


<!-- 底部菜单 -->
<div class="js-navmenu js-footer-auto-ele shop-nav nav-menu nav-menu-1 has-menu-3">
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


 <script type="text/x-jquery-tmpl" id="ProductItemEdit">

		<div class="block block-list block-border-top-none block-border-bottom-none" id="prolist${Id}">
    		<div class="block-item name-card name-card-3col clearfix">
                <a href="#" class="thumb">
                            <img src="${Imgurl}" height="60">
                </a>
        		<div class="detail">

                <p class="sku-detail ellipsis js-toggle-more">
                    <span class="" style=" color: #F60; " >${uname}
                    </span>
                </p>
                <p class="sku-detail ellipsis js-toggle-more">
                    <span class="" style=" color: #333333; " >${text}
                    </span>
                </p>
                <p class="sku-detail ellipsis js-toggle-more">
                    <span class="" style=" color: #c6c6c6; " >${jsonDateFormatKaler(subtime)}
                    </span>
                </p>
				</div>

   		 </div>
       
    
	</div>

    </script>

    <div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_AccountId" value="<%=AccountId %>" />
    <input type="hidden" id="hid_channleid" value="<%=channleid %>" />
    
    <input type="hidden" id="pageindex" value="1" />
    
</body></html>
