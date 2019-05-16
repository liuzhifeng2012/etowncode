<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="evaluate.aspx.cs" Inherits="ETS2.WebApp.H5.order.evaluate" %>

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
            var userid = $("#hid_userid").val();
            var comid = $("#hid_comid").val();

            getmenubutton(comid, 'js-navmenu');
            SearchList(1);


            //装载产品列表
            function SearchList(pageindex) {
                $("#loading").show();
                var key = $("#key").val();
                var uid=$("#hid_AccountId").val();
                var channelid=$("#hid_channelid").val();
                var comid=$("#hid_comid").val();
                
                var evatype=0;
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $("#pageindex").val(pageindex);

                <%if (viewtype == 1){ %>
                    uid=0;
                    evatype=1;
                <%} %>
                <%if (viewtype == 2){ %>
                    channelid=0;
                    evatype=1
                <%} %>
                $.post("/JsonFactory/OrderHandler.ashx?oper=evaluatePageList", { pageindex: pageindex,comid:comid, uid: uid,evatype:evatype }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $("#loading").hide();
                        $("#content").html("<div class=\"empty-list\" style=\"margin-top:30px;\"> 请 <a href=\"Login.aspx?come=/h5/order/order.aspx\">登录</a> 后查看！</div>");
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();

                        if (data.totalCount == 0) {
                            $("#content").html("<div class=\"empty-list\" style=\"margin-top:30px;\"> <h4>哎呀，没有查到？</h4></div>");
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
        <link  href="css/bottommenu.css" rel="stylesheet">  
  </head>

<body class=" ">
        <!-- container -->
    <div class="container ">
                <div class="content"  style="min-height: 288px;">
					<div class="tabber  tabber-n3 tabber-double-11 clearfix">
                        <a class="" href="/m/indexcard.aspx"><< 返回会员中心</a>
                        <%if (viewtype == 0)
                          { %>
						<a class="active" href="evaluate.aspx?type=<%=viewtype %>">评价记录</a>
                        <%}
                          else if (viewtype == 2)
                          { %>
                          <a class="active" href="evaluate.aspx?type=<%=viewtype %>">我的档案</a>
                          <%}
                          else
                          { %>
                          <a class="active" href="evaluate.aspx?type=<%=viewtype %>">我对客户的评价</a>
                          <%} %>
					</div>
                             <div id="backs-list-container" style="margin-top: 20px;">
			<li class="block block-order animated">
		<div id="content"></div>
   
	</li>
		
		
		<div class="empty-list" style="margin-top:30px; display:none;">
    <!-- 文本 -->
    <div>
        <h4>哎呀，购物车？</h4>
        <p class="font-size-12">不落单一起团</p>
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
                <!-- 子菜单 
                            </div>
                            <div class="nav-item">
                <a class="mainmenu js-mainmenu" href="wo ">
                                        <span class="mainmenu-txt">往期回顾</span>
                </a> -->
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
<div class="header">
				<span class="font-size-12">
                {{if evatype==1}}
                  ${proname} : 教练对客户评论
                {{else}}
                  ${proname} : 客户评论
                {{/if}}
                
                
                </span>
		</div>
 <hr class="margin-0 left-10">
		<div class="block block-list block-border-top-none block-border-bottom-none" id="prolist${Id}">
    		<div class="block-item name-card name-card-3col clearfix">
                <a href="#" class="thumb">
                            <img src="${Imgurl}" height="60">
                </a>
        		<div class="detail">
            		<h3>
                         ${uname} 
                    </h3>
                <p class="sku-detail ellipsis js-toggle-more">
                    <span class="c-gray">
                    ${text} 
                    </span>
                </p>
				</div>
				<div class="right-col">
					<div class="num" style="padding-top:10px;"><span>${jsonDateFormatKaler(subtime)}</span></div>
				</div>
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
    <input type="hidden" id="pageindex" value="1" />
    <input id="hid_channelid" type="hidden" value="<%=channelid %>" />
</body></html>
