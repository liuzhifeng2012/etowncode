<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quitorder.aspx.cs" Inherits="ETS2.WebApp.H5.order.quitorder" %>


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
            var orderid = $("#hid_orderid").val();

            SearchList();


            //装载订单列表
            function SearchList() {
                $("#loading").show();
                $.post("/JsonFactory/OrderHandler.ashx?oper=ConsumerOrderbyorderid", { openid: $("#hid_openid").val(), AccountId: $("#hid_AccountId").val(), orderid: $("#hid_orderid").val(), comid: $("#hid_comid").val() }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $("#loading").hide();
                        $("#content").html("<div class=\"empty-list\" style=\"margin-top:30px;\"> 请 <a href=\"Login.aspx?come=/h5/order/order.aspx\">登录</a> 后查看我的订单！</div>");
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();

                        $("#Id").html("订单号："+data.msg.Id);
                        $("#Imgurl").html(" <img src=" + data.msg.Imgurl + " height=\"60\">");
                        $("#Pro_name").html(data.msg.Pro_name);
                        $("#Pay_price").html("￥" + data.msg.Pay_price);
                        $("#U_num").html("×" + data.msg.U_num);
                        $("#sumprice").html("￥" + data.msg.sumprice);
                        $("#U_name").html(data.msg.U_name);
                        $("#U_phone").html(data.msg.U_phone);
                        $("#Pno").html(data.msg.Pno);

                        $("#number").val(data.msg.U_num);
                        $("#hid_proid").val(data.msg.Pro_id);
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#orderstate");

                    }
                })
            }

            $("#quitorder").click(function () {
                $("#quit").show();
            });

            $(".js-confirm-it").click(function () {

                $("#loading").show();
                $.post("/JsonFactory/OrderHandler.ashx?oper=userquitorder", { openid: $("#hid_openid").val(), orderid: $("#hid_orderid").val(), comid: $("#hid_comid").val(), quit_num: $("#number").val(), proid: $("#hid_proid").val() }, function (data) {

                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#loading").hide();
                        alert("退票失败" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        alert("退票成功");
                       
                        window.location.href = 'order.aspx';
                    }
                })
            });


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
		<div id="content">
 <div class="header">
				<span class="font-size-12" id="Id"></span>
		</div>
        <hr class="margin-0 left-10">
		<div class="block block-list block-border-top-none block-border-bottom-none" id="prolist${Id}">
    		<div class="block-item name-card name-card-3col clearfix">
                <a href="#" class="thumb" id="Imgurl">
                            
                </a>
        		<div class="detail">
            		<h3 id="Pro_name">
                       
                    </h3>
                <p class="sku-detail ellipsis js-toggle-more">
                    <span class="c-gray">
                    </span>
                </p>
				</div>
				<div class="right-col">
					<div class="price"><span id="Pay_price"></span></div>
					<div class="num">
						<span id="U_num">
                       
                        </span>
					</div>
				</div>
   		 </div>
       
        <div class="bottom">
        总价：<span class="c-orange" id="sumprice"></span>
        </div>
        <div class="bottom">
            订购人姓名：<span class="c-orange" id="U_name"></span>
        </div>
        <div class="bottom">
            订购人手机：<span class="c-orange" id="U_phone"></span>
        </div>
        <div class="bottom" style=" display:none;">
            电子码：<span class="c-orange" id="Pno"></span>
        </div>
        <div class="bottom" id="orderstate">
               订单状态： 
               
            <script type="text/x-jquery-tmpl" id="ProductItemEdit">
                        {{if Order_state==1}}
                            <a href="/wxpay/micromart_orderpay.aspx?id=${Id}&comid=${Comid}" class="btn btn-orange btn-in-order-list" >付款</a>
                        {{/if}}
                        {{if Order_state==2}}
                            已支付 <span class="c-orange"> 
                        {{/if}}

                        {{if Order_state==4}}
                            {{if Server_type==1 || Server_type==3  }}
                                已发码 
                            {{else}}
                                 {{if Server_type==11}}
                                        <a href="#" class="btn btn-in-order-list" >已发货 ${Expresscom}${Expresscode}</a>
                                 {{else}}
                                      {{if Server_type==12}}
                                            <a href="yuyuesuc.aspx?id=${Id}&md5=${md5}" class="btn btn-in-order-list" >预订提交成功</a>
                                      {{else}}
                                            {{if Server_type==13}}
                                                <a href="Coachyuyuesuc.aspx?id=${Id}&md5=${md5}" class="btn btn-in-order-list" >预约成功</a>
                                            {{else}}
                                                    <a href="#" class="btn btn-in-order-list" >已发货</a>
                                            {{/if}}
                                      {{/if}}
                                 {{/if}}
                             {{/if}}
                        {{/if}}

                        {{if Order_state==8}}
                            {{if Server_type==1|| Server_type==3  }}
                               已验证
                            {{else}}
                                {{if Server_type==11}}
                                    <a href="#" class="btn btn-in-order-list" >已发货 ${Expresscom}${Expresscode}</a>
                                {{else}}
                                    <a href="#" class="btn btn-in-order-list" >已使用</a>
                                {{/if}}
                            {{/if}}
                        {{/if}}
                        {{if Order_state==22}}
                            <a href="#" class="btn btn-in-order-list" >已处理</a>
                        {{/if}}
                        {{if Order_state==16}}
                            <a href="#" class="btn btn-in-order-list" >已退款</a>
                        {{/if}}
                        {{if Order_state==23}}
                            <a href="#" class="btn btn-in-order-list" >超时已取消</a>
                        {{/if}}
                           </script>
        </div>

        <div class="bottom" id="quit" style=" display:;">
               退票数量： <span class="c-orange"> <input id="number" style="width:80px;" class="txt" value="" type="number" /> </span>
               <input id="hid_proid" class="hide" value="" type="text" /> 
               <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark"> 确认我要退票 </a>
        </div>
	</div>
        </div>
   
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

                            </div>
                            <div class="nav-item">
                <a class="mainmenu js-mainmenu" href="/h5/order/Order.aspx">
                                        <span class="mainmenu-txt">我的订单</span>
                </a>
                            </div>
            </div>

        


    <div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_orderid" value="<%=orderid %>" />
</body></html>
