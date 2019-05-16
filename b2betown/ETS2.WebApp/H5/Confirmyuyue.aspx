<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Confirmyuyue.aspx.cs" Inherits="ETS2.WebApp.H5.Confirmyuyue" %>

<html lang="zh-CN" class="no-js ">
<head>
    <meta charset="utf-8">
    <meta content="<%=title %>" name="keywords">
    <meta content="" name="description">
    <meta content="True" name="HandheldFriendly">
    <meta content="320" name="MobileOptimized">
    <meta content="telephone=no" name="format-detection">
    <meta content="on" http-equiv="cleartype">


    <link href="image/yz_fc.ico" rel="icon">
    <title><%=servertitle%></title>

    <!-- meta viewport -->
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport">
    
    <!-- CSS -->
    <link onerror="_cdnFallback(this)" href="order/css/css1.css" rel="stylesheet">    
	<link onerror="_cdnFallback(this)" href="order/css/css.css" rel="stylesheet">    
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script> 
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;
        var pageSize = 16;
        $(function () {


            var comid = $("#hid_comid").val();

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


        });

        function querenpay(caozuo) {
            var orderid = $("#hid_id").val();
            var mobile = $("#hid_mobile").val();
            var caozuo_str = "";

            if (caozuo == "qr") {
                caozuo_str = "请再次确认 成功预订！";
            } else {
                caozuo_str = "此次操作是取消此预订？请确认！";
            }

            if (confirm(caozuo_str)) {

                //根据公司id获得关注作者信息
                $.post("/JsonFactory/OrderHandler.ashx?oper=querenpay", { orderid: orderid, caozuo: caozuo, mobile: mobile }, function (data) {
                    dat = eval("(" + data + ")");
                    if (dat.type == 1) {
                        alert(dat.msg)
                        return;
                    }
                    if (dat.type == 100) {
                        if (caozuo == "qr") {
                            alert("订单已确认");
                        } else {
                            alert("订单已取消");
                        }
                        location.reload()
                        return;
                    }
                });
            }

        }


    </script>

    <link  href="css/bottommenu.css" rel="stylesheet"> 
    <style>
    
    .avatar {

    background: #F9F9F9 none repeat scroll 0% 0%;
    width: 70px;
    height: 70px;

    /*border-radius: 50%;*/
    margin:0 auto; 
}
.avatar span {
    display: block;
    width: 70px;
    height: 70px;
    border-radius: 50%;
    overflow: hidden;
     
    background-position: center center;
    background-repeat: no-repeat;
    background-size: cover;
    border: 4px solid #C6BFB9;
    box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.2) inset;

}
.btn.btn-block {
    width: 40%;
}
    </style>
	</head>

<body class=" " >
        <!-- container -->
    <div class="container ">
        <div class="header">
        <!-- ▼顶部通栏 -->
                                
            <div class="js-mp-info share-mp-info">
            <a href="default.aspx" class="page-mp-info">
                <img width="24" height="24" src="<%=logoimg%>" class="mp-image">
                <i class="mp-nickname">
                    <%=title %>                </i>
            </a>
            <div class="links">
                
                                                                </div>
        </div>
        <!-- ▲顶部通栏 -->
</div>        <div class="content ">
        <div class="content-body">
    
                    
        <!-- 标题 -->
<div style=" padding-top:50px;">
    <div class="custom-title  text-left">
        <h2 class="title" style="text-align: center;">
           <%=servertitle%>                </h2>
                <p class="sub_title"></p>
            </div>
</div>
                    
            
    <!-- 商品区域 -->
    <!-- 展现类型判断 -->
            <ul id="list" class="sc-goods-list clearfix list size-3" style=" padding-bottom:50px;">
            <%if (Server_type == 13)
              { %>
                     <li style="text-align: center; padding:5px;font-size:14px;">预约产品名称：<%=Pro_name%></li> 
                     <li style="text-align: center; padding:5px;font-size:14px;">预约日期和时间：<%=yuyuetime%></li> 

                     <li style="text-align: center; padding:5px;font-size:14px;">预约人姓名：<%=u_name%></li> 
                     <li style="text-align: center; padding:5px;font-size:14px;">预约教练：<%=bindiname%> </li>
                       <%if (Order_state == 23)
                         { %>

                         <li style="text-align: center; padding:5px;font-size:14px;">订单状态：订单已取消 </li>  
                       <%}
                         else
                         { %>
                             <%if (Pay_state == 0)
                               { %>
                             <li style="text-align: center; padding:5px;font-size:14px;">预约状态：<span style=" color:#ff0000">未确认</span> </li>      
                                
                             <li style="text-align: center; padding:15px;font-size:14px;"><a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" onclick="querenpay('qr')">确认预约</a>    <a href="javascript:;" class="js-cannel-it btn btn-block"  onclick="querenpay('qx')">取消预约</a></li>
                             <%}
                               else if (Pay_state == 1)
                               { %>
                                <li style="text-align: center; padding:5px;font-size:14px;">预约状态：已确认 </li>  

                             <%}
                               else
                               { %>
                               <li style="text-align: center; padding:5px;font-size:14px;">预约状态：已支付 </li>  
                             <%} %>  
                      <%} %>    
                      <%}
              else if (Server_type == 9)
              { %>

              <li style="text-align: center; padding:5px;font-size:14px;">酒店名称：<%=Pro_name%></li> 
                     <li style="text-align: center; padding:5px;font-size:14px;">入住日期：<%=yuyuetime%></li> 
                     <li style="text-align: center; padding:5px;font-size:14px;">间数：<%=Num%></li> 
                     <li style="text-align: center; padding:5px;font-size:14px;">预约人姓名：<%=u_name%></li> 
                     <li style="text-align: center; padding:5px;font-size:14px;">预约人电话：<%=phone%></li> 
                     
                       <%if (Order_state == 23)
                         { %>

                         <li style="text-align: center; padding:5px;font-size:14px;">订单状态：订单已取消 </li>  
                       <%}
                         else
                         { %>
                             <%if (Order_state == 4 || Order_state == 2)
                               { %>
                             <li style="text-align: center; padding:5px;font-size:14px;">订房状态：<span style=" color:#ff0000">未确认</span> </li>      
                                
                             <li style="text-align: center; padding:15px;font-size:14px;"><a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark" onclick="querenpay('qr')">确认订房</a>    <a href="javascript:;" class="js-cannel-it btn btn-block"  onclick="querenpay('qx')">满房取消</a></li>
                             <%}
                               else if (Order_state == 22)
                               { %>
                                <li style="text-align: center; padding:5px;font-size:14px;">订房状态：已确认 </li>  
                             <%}
                               else if (Order_state == 8)
                               { %>
                                <li style="text-align: center; padding:5px;font-size:14px;">订房状态：已验证 </li>  
                            <%}
                               else if (Pay_state == 2)
                               { %>
                                <li style="text-align: center; padding:5px;font-size:14px;">订房状态：已支付 </li>  
                             <%}
                               else
                               { %>
                               <li style="text-align: center; padding:5px;font-size:14px;"> </li>  
                             <%} %>  
                      <%} %>   



                      <%} %>
             </ul>
        </div>                       		
	    <div class="content-sidebar">
		            <a href="Default.aspx" class="link">
		                <div class="sidebar-section shop-card">
		                    <div class="table-cell">
		                       <img src="<%=logoimg %>" class="shop-img" alt="公众号头像" height="60" width="60">
		                    </div>
		                    <div class="table-cell">
		                        <p class="shop-name">
		                        	<%=title %>		                        			                       
                                </p>
		                    </div>
		                </div>
		            </a>
		               <div class="sidebar-section shop-info">
		                    <div class="section-detail">
		                         <p class="shop-detail"><%=Scenic_intro %></p>
		                        	<p class="text-center weixin-title">微信“扫一扫”立即关注</p>
			                        <div class="js-follow-qrcode text-center qr-code">
			                        <img width="158" height="158" src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>">
                                    </div>
			                        <p class="text-center weixin-no">微信号：<%=weixinname %></p>
			                </div>
		               </div>		    		        		    	    
          </div>
    
               
			   </div>        <div style="min-height: 1px;" class="js-footer">
            
    <div class="footer">
        <div class="copyright">
                            <div class="ft-links">
                    <span id="copydaohang"></span>
                    <span class="links"></span>
                                                        </div>
                        <div class="ft-copyright">
<a href="#">易城商户平台技术支持</a>
</div>
        </div>
    </div>
        </div>        <!-- fuck taobao -->
    <div class="fullscreen-guide fuck-taobao hide" id="js-fuck-taobao">
        <span class="js-close-taobao guide-close">×</span>
        <span class="guide-arrow"></span>
        <div class="guide-inner">
            <div class="step step-1"></div>
            <div class="js-step-2 step"></div>
        </div>
    </div>    </div>

    <!-- JS -->
   
   <div class="motify"><div class="motify-inner"></div></div>
   




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



<div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>

    <input id="hid_comid" type="hidden" value="<%=comid %>" />
   
    <input id="hid_id" type="hidden" value="<%=id %>" />
    <input id="hid_mobile" type="hidden" value="<%=qrmoble %>" />
    
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
     


</body></html>
