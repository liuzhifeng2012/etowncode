<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Archives.aspx.cs" Inherits="ETS2.WebApp.H5.Archives" %>

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
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $("#pageindex").val(pageindex);
                $.post("/JsonFactory/OrderHandler.ashx?oper=ConsumerOrderPageList", { pageindex: pageindex, openid: $("#hid_openid").val(), AccountId: $("#hid_AccountId").val() }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $("#loading").hide();
                        $("#content").html("<div class=\"empty-list\" style=\"margin-top:30px;\"> 请 <a href=\"Login.aspx?come=/h5/order/order.aspx\">登录</a> 后查看我的订单！</div>");
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
                        <a class="" href="Default.aspx"><< 继续购物</a>
						<a class="" class="" href="Cart.aspx">购物车</a>
						<a class="active" href="Order.aspx">订单记录</a>
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
				<span class="font-size-12">订单号:${Id}</span>
		</div>
 <hr class="margin-0 left-10">
		<div class="block block-list block-border-top-none block-border-bottom-none" id="prolist${Id}">
    		<div class="block-item name-card name-card-3col clearfix">
                <a href="#" class="thumb">
                            <img src="${Imgurl}" height="60">
                </a>
        		<div class="detail">
            		<h3>
                    
                    {{if Server_type==1 || Server_type==3  || Server_type==12}}
                        <a href="pro.aspx?id=${ProId}">
                        ${Proname}
                        </a>
                    {{else}}
                        ${Proname}
                    {{/if}}
                    
                    </h3>
                <p class="sku-detail ellipsis js-toggle-more">
                    <span class="c-gray">
                    </span>
                </p>
				</div>
				<div class="right-col">
					<div class="price"><span>￥${Pay_price}</span></div>
					<div class="num">
						<span>
                       ×${U_num}
                        </span>
					</div>
				</div>
   		 </div>
       
    <div class="bottom">
    总价：<span class="c-orange">￥${sumprice}</span>
    <div class="opt-btn">
                        {{if Order_state==1}}
                        <a href="/wxpay/micromart_orderpay.aspx?id=${Id}&comid=${Comid}" class="btn btn-orange btn-in-order-list" >付款</a>
                        {{/if}}
                        {{if Order_state==2}}
                        {{if Server_type==1}}
                            <a href="/m/EticketDetail.aspx?orderid=${Id}" class="btn btn-in-order-list" >已支付</a>
                        {{else}}
                            已支付
                        {{/if}}
                        {{/if}}
                        {{if Order_state==4}}
                        {{if Server_type==1 || Server_type==3  }}
                            <a href="/m/EticketDetail.aspx?orderid=${Id}" class="btn btn-in-order-list" >已发码,点击查看</a>
                        {{else}}
                            {{if Server_type==11}}
                            已发货 ${Expresscom}${Expresscode}
                            {{else}}
                              {{if Server_type==12}}
                                    预订提交成功
                              {{else}}
                                    已发货
                              {{/if}}
                             {{/if}}
                        {{/if}}
                        {{/if}}
                        {{if Order_state==8}}
                        {{if Server_type==1|| Server_type==3  }}
                            <a href="/m/EticketDetail.aspx?id=${Id}" class="btn btn-in-order-list" >已验证</a>
                        {{else}}
                            {{if Server_type==11}}
                                已发货 ${Expresscom}${Expresscode}
                            {{else}}
                                已使用
                            {{/if}}
                        {{/if}}
                        {{/if}}
                        {{if Order_state==22}}
                            已处理
                            {{/if}}
                        {{if Order_state==16}}
                        已退款
                        {{/if}}
                        {{if Order_state==23}}
                        超时已取消
                        {{/if}}
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
    
</body></html>
