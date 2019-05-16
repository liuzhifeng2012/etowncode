<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="yuding.aspx.cs" Inherits="ETS2.WebApp.H5.order.yuding" %>

<!DOCTYPE html>
<html class="no-js " lang="zh-CN" >

<head>
    <meta charset="utf-8">
    <meta name="keywords" content="<%=title %>" />
    <meta name="description" content="" />
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">

    <title><%=pro_name %>    - <%=title %></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script> 
    <script src="/Scripts/jquery.cookie.2.2.0.min.js" type="text/javascript"></script> 
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/swipe.js"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>

<script>
    $(function () {


        var comid = $("#hid_comid").val();
        var proid = $("#hid_proid").val();

         getmenubutton(comid, 'js-navmenu');

        



        //根据公司id获得关注作者信息
        $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
            dat = eval("(" + data + ")");
            if (dat.type == 1) {

            }
            if (dat.type == 100) {
                $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage  btn btn-follow\">关注我们</a>");
            }
        });

                //查询购物车数量
                $.post("/JsonFactory/OrderHandler.ashx?oper=agentsearchcart", { userid: $("#hid_userid").val(), comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                    }
                    if (data.type == 100) {
                        $("#right-icon").removeClass("hide");
                        $("#Num").html(data.msg);
                    }
                });


         function timer(intDiff) {
            window.setInterval(function () {
                var day = 0,
                                hour = 0,
                                minute = 0,
                                second = 0; //时间默认值

                if (intDiff > 0) {
                    day = Math.floor(intDiff / (60 * 60 * 24));
                    hour = Math.floor(intDiff / (60 * 60)) - (day * 24);
                    minute = Math.floor(intDiff / 60) - (day * 24 * 60) - (hour * 60);
                    second = Math.floor(intDiff) - (day * 24 * 60 * 60) - (hour * 60 * 60) - (minute * 60);
                } else {
                    qinggou();
                }
                if (minute <= 9) minute = '0' + minute;
                if (second <= 9) second = '0' + second;
                $('.day_show').html(day + "天");
                $('.hour_show').html('<s id="h"></s>' + hour + '时');
                $('.minute_show').html('<s></s>' + minute + '分');
                $('.second_show').html('<s></s>' + second + '秒');
                intDiff--;
            }, 1000);
        }
        function qinggou() {
            $("#buy1").removeClass("btn-grey-dark");
            $("#buy2").removeClass("btn-grey-dark");
            $("#buy1").addClass("butn-qrcode");
            $("#buy2").addClass("btn-orange-dark");
            $("#buy1").html("火热抢购中..");
            $("#buy2").html("火热抢购中..");
            $("#hid_dinggou").val("1");
        }

        <%if (Ispanicbuy==1 ){ %>
         <%if (Limitbuytotalnum<=0){%>
            $("#buy1").removeClass("butn-qrcode");
            $("#buy2").removeClass("btn-orange-dark");
            $("#buy1").addClass("btn-grey-dark");
            $("#buy2").addClass("btn-grey-dark");
            $("#buy1").html("抢购已结束");
            $("#buy2").html("抢购已结束");

            $("#hid_dinggou").val("0");

         <% }else{%>
         
            <%
                if (panic_begintime <= nowtoday && nowtoday<panicbuy_endtime){//抢购在有效期范围内
             %>
                $("#buy1").html("火热抢购中");
                $("#buy2").html("火热抢购中");
                $("#hid_dinggou").val("1");
             <%
             }else if(panic_begintime>nowtoday){         
             %>

                   $("#hid_dinggou").val("0");
                   $("#buy1").removeClass("butn-qrcode");
                  $("#buy2").removeClass("btn-orange-dark");
                   $("#buy1").addClass("btn-grey-dark");
                   $("#buy2").addClass("btn-grey-dark");
                   $('#buy2').html('抢购 距开始还剩 <span id="day_show"></span><span class="hour_show"></span><span class="minute_show"></span><span class="second_show"></span>')
                   $('#buy1').html('抢购 距开始还剩 <span id="day_show"></span><span class="hour_show"></span><span class="minute_show"></span><span class="second_show"></span>')
                   var jishicount=$('#hid_jishicount').val();

                   var intDiff = parseInt(jishicount); //倒计时
                   timer(intDiff);
             <%
              }else{
             %>
                $("#buy1").removeClass("butn-qrcode");
                $("#buy2").removeClass("btn-orange-dark");
                $("#buy1").addClass("btn-grey-dark");
                $("#buy2").addClass("btn-grey-dark");
                $("#buy1").html("抢购已结束");
                $("#buy2").html("抢购已结束");
                $("#hid_dinggou").val("0");
              <%} 

              %>


        $(".js-buy-it").click(function () {
            var qianggou= $("#hid_dinggou").val();

            if(qianggou !=0){
                $("#QJwxuFqolZ").show();
                $(".js-confirm-it").html("下一步");
                $("#hid_action").val("1");
                $("#zhegai").show();
            }
        });

        $(".js-qrcode-buy").click(function () {
            var qianggou= $("#hid_dinggou").val();
            if(qianggou !=0){
                $("#QJwxuFqolZ").show();
                $("#zhegai").show();
            }
        });

        <%
        }
        }else{%>
         $(".js-buy-it").click(function () {
       
            $("#QJwxuFqolZ").show();
            $("#zhegai").show();
        });

        $(".js-qrcode-buy").click(function () {
            $("#QJwxuFqolZ").show();
            $("#zhegai").show();
        });
        <%} %>

        $("#zhegai").click(function () {
            $("#QJwxuFqolZ").hide();
            $("#zhegai").hide();
        });




        $(".response-area-minus").click(function () {
            var num = parseInt($(".txt").val());
            if (num <= 1) {
                $(".txt").val(1);
            } else {
                $(".txt").val(num - 1);
            }
        });

        $(".response-area-plus").click(function () {
            var num = parseInt($(".txt").val());
            $(".txt").val(num + 1);
        });

        $(".js-confirm-it").click(function () {

            var num = parseInt($(".txt").val());
            var proid = $("#hid_proid").val();
            var action = $("#hid_action").val();

            //            window.location.href = "pay.aspx?num=" + num + "&id=" + proid;
            //            window.location.href = "/wxpay/micromart_pay_" + num + "_" + proid + ".aspx";
            var issetfinancepaytype = false;
            if ('<%=issetfinancepaytype %>' == 'True' || '<%=issetfinancepaytype %>' == 'true') {
                issetfinancepaytype = true;
            }
            if(action=="1"){//直接订购
                if (isWeiXin() == true && issetfinancepaytype == true) {
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenshiLinkAboutPay", { comid: '<%=comid %>', redirect_uri: "http://shop" + comid + ".etown.cn/wxpay/micromart_pay_" + num + "_" + proid + ".aspx" }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            //                        $.prompt("获取链接出错");
                            window.location.href = "/wxpay/micromart_pay_" + num + "_" + proid + ".aspx";
                            return;
                        }
                        if (data.type == 100) {
                            window.location.href = data.msg;
                        }
                    })
                } else {
                    window.location.href = "/wxpay/micromart_pay_" + num + "_" + proid + ".aspx";
                }
            }else{//加入购物车
                $("#QJwxuFqolZ").hide();
                $("#zhegai").hide();
                $('#cartloading').show();; 
                 $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { userid: $("#hid_userid").val(), comid: comid, proid: proid, u_num:num }, function (data) {
                  data = eval("(" + data + ")");
                  $("#zhegai").hide();
                  if (data.type == 1) {
                  }
                  if (data.type == 100) {
                      $("#right-icon").removeClass("hide");
                      
                       //查询购物车数量
                  $.post("/JsonFactory/OrderHandler.ashx?oper=agentsearchcart", { userid: $("#hid_userid").val(), comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $('#cartloading').html("添加到购物车失败");
                            $('#cartloading').fadeOut(3000);
                        }
                        if (data.type == 100) {
                            $('#cartloading').html("成功添加到购物车");
                            $('#cartloading').fadeOut(3000);
                            $("#Num").html(data.msg);
                        }
                    })
                  }
                 })
          }

        });

        $(".js-add-cart").click(function () {

            $("#QJwxuFqolZ").show();
            $(".js-confirm-it").html("加入购物车");
            $("#hid_action").val("0");
            $("#zhegai").show();

        });
        

        $(".js-cancel").click(function () {
            $("#QJwxuFqolZ").hide();
            $("#zhegai").hide();
        });

        //加载导航 ;
        $.ajax({
            type: "post",
            url: "/JsonFactory/ProductHandler.ashx?oper=getprochildimglist",
            data: { comid: comid, proid: proid },
            async: false,
            success: function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //$.prompt("查询错误");
                    //return;
                }
                if (data.type == 100) {
                    if (data.msg.length > 0) {
                        var daohanglist = "";
                        var tablist = "";
                        for (var i = 0; i < data.msg.length; i++) {
                            daohanglist = daohanglist + "<li><a  onclick=\"return false;\"><div><img class=\"goods-main-photo\" src=\"" + data.msg[i].imgurl + "\" style=\"max-height: 320px;\"/></div></a> </li>";
                            tablist += "<li></li>"
                        }
                        $(".list_font").append(daohanglist);
                        $(".list_tab").append(tablist);

                    }
                }
            }
        })

         <%if (Ispanicbuy==1 || Ispanicbuy==2){ %>
           <%if (Limitbuytotalnum<=0){ %>
                   $("#buy1").removeClass("js-qrcode-buy ");
                   $("#buy1").removeClass("butn-qrcode");
                   $("#buy1").removeClass("btn-orange-dark");
                   $("#buy2").removeClass("js-buy-it");
                   $("#buy2").removeClass("btn-orange-dark");
                   

                   $("#buy1").addClass("btn-orange-dark-shouqing");
                   $("#buy2").addClass("butn-qrcode-shouqing");

                   $("#buy1").html("商品已售罄");
                   $("#buy2").html("商品已售罄");

            <%}%>
         <%}%>



    })
</script>
  <script type="text/javascript">
      //判断微信版本,微信版本5.0以上
      function isWeiXin() {
          var ua = window.navigator.userAgent.toLowerCase();
          if (ua.match(/MicroMessenger/i) == 'micromessenger' && parseFloat(navigator.appVersion) >= 5) {
              return true;
          } else {
              return false;
          }
      }
    </script>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    
    <!-- CSS -->
    <link onerror="_cdnFallback(this)" href="css/css1.css" rel="stylesheet">    
	<link onerror="_cdnFallback(this)" href="css/css3.css" rel="stylesheet">    
	<link onerror="_cdnFallback(this)" href="css/css.css" rel="stylesheet">    
        <link  href="css/bottommenu.css" rel="stylesheet">  
</head>
<body style=" margin-bottom:40px;" >
        <!-- container -->
    <div class="container wap-goods internal-purchase">
        <div class="header">
        <!-- ▼顶部通栏 -->
                                
            <div class="js-mp-info share-mp-info">
            <a class="page-mp-info" href="default.aspx">
                <img class="mp-image" width="24" height="24" src="<%=logoimg %>"/>
                <i class="mp-nickname">
                    <%=title %>                </i>
            </a>
           <div class="links">
                
                                                                </div>
        </div>
        <!-- ▲顶部通栏 -->
</div>        <div class="content ">
        <div class="content-body">
    <!-- 分享文案 -->
    <span id="wxdesc" class="hide"></span>
    
    <div class="js-image-swiper custom-image-swiper custom-goods-swiper custom-image-swiper-single">
        <div class="swiper-container-img" style="height: 320px;">
        <div class="swiper-wrapper" style="-webkit-transform:translate3d(0,0,0);">
                        <div class="swiper-slide box_swipe" id="banner_box" style="visibility: visible;">
                         <%if (Ispanicbuy==1 || Ispanicbuy==2){ %>
                            <%if (Limitbuytotalnum<=0){ %>
                        <div class="Soldout"><img  src="image/5337034_100415061309_2.gif"  style="display: block;width:120px;padding-top:5px;padding-left:5px;"></div>
                             <%}%>
                        <%}%>

                           <ul class="list_font">
                                <li> <a class="js-no-follow"  onclick="return false;" style="height: 320px;"><img class="goods-main-photo" src="<%=imgurl %>" style="max-height: 320px;" /></a></li>
                           </ul>
                           <ol class="list_tab">
                           <li class="on"></li>
                           </ol>
            </div>
                    </div>
    </div>

    </div>                    <div class="goods-header">
            <h2 class="title"><%=pro_name %></h2>
                        <div class="goods-price ">
                                    <div class="current-price">
                        <span>￥&nbsp;</span><i class="js-goods-price price"><%=price %></i>
                    </div>
                            </div>
        </div>
                                
                                    
        <hr class="with-margin" />
    <div class="sku-detail adv-opts" style="border-top:none;">
        <div class="sku-detail-inner adv-opts-inner">
        <%if (Server_type == 1)
          { %>
            <dl>
                <dt>有效期：</dt>
                <dd><%=pro_youxiaoqi%></dd>
            </dl>
        <%} %>
        </div>
        <div class="qrcode-buy">
            <a  id="buy1" href="javascript:;" class="js-qrcode-buy btn btn-block btn-orange-dark butn-qrcode">我要看房</a>
        </div>
    </div>
        <div class="js-components-container components-container">
        
		<div class="custom-store">
    <a class="custom-store-link clearfix" href="Default.aspx">
        <div class="custom-store-img"> <img class="mp-image" width="24" height="24" src="<%=logoimg %>"/></div>
        <div class="custom-store-name"><%=title %></div>
        <span class="custom-store-enter">进入店铺</span>
    </a>
</div>

 <div class="custom-recommend-goods js-custom-recommend-goods hide clearfix">
    <div class="custom-recommend-goods-title">
        <a class="js-custom-recommend-goods-link" href="">推荐商品</a>
    </div>
     <ul class="custom-recommend-goods-list js-custom-recommend-goods-list clearfix">
    </ul>
</div>


<!-- 富文本内容区域 -->
<div class="custom-richtext">
<%=sumaryend%></div>

    
                                
    </div>
        <div class="js-bottom-opts bottom-opts">
                                    
<!-- 购买链接 -->

            <a href="javascript:;" id="buy2" class="js-buy-it btn btn-orange-dark btn-2-1">我要看房</a>
           </div>
											

</div>
<div class="modal modal-login js-modal-login">
    <div class="account-form login-form">
    <form class="js-login-form " method="GET" action="login.aspx">
                <h2 class="js-login-title form-title big">
            <span>
                            请先登录账号
            </span>
        </h2>
        <!-- 表单主体 -->
        <ul class="block block-form margin-bottom-normal">
            <li class="block-item">
                <label for="phone">手机号码</label>
                <input id="phone" type="tel" name="phone" maxlength="11" class="js-login-phone" autocomplete="off" placeholder="请输入您的手机号" value=""/>
            </li>
            <li class="block-item">
                <label for="password">登录密码</label>
                <input id="password" type="password" name="password" autocomplete="off" maxlength="20" class="js-login-pwd" placeholder="请填写您的密码" />
            </li>
            <li class="relative block-item js-auth-code-li auth-hide">
                <label for="code">验证码</label>
                <input id="code" type="tel" name="code" class="js-auth-code" placeholder="请输入短信验证码" maxlength="6"/>
                <button type="button" class="btn btn-green btn-auth-code font-size-12 js-auth-code-btn" data-text="获取验证码">
                    获取验证码
                </button>
            </li>
        </ul>
        <div class="action-container">
            <button type="submit" class="js-submit btn btn-green btn-block" disabled>
                            登录
                        </button>
                        <button type="button" class="js-login-cancel btn btn-block btn-white" >返回</button>
                    </div>
        <div class="action-links">
            <p class="center">
                                    <a class="js-login-mode c-blue" data-login-mode="signup" href="javascript:;">注册账号</a>
                                <span class="division-span"></span>
                <a href="#changepassword" class="js-forget-password forget-password">忘记密码</a>
            </p>
        </div>
    </form>
</div>
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
   	
    
                    </div>        <div class="js-footer" style="min-height: 1px;">
            
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
        </div> 
     </div>

               
    <div id="zhegai" style="z-index: 1009;position:absolute;left:0;top:0; bottom:0;  width:100%; height:1000%; filter:alpha(Opacity=80);-moz-opacity:0.9;opacity: 0.9; display:none; background:#000000;" ></div>      
    <div id="QJwxuFqolZ" class="sku-layout sku-box-shadow" style="overflow: hidden; visibility: visible; opacity: 1; bottom: 0px; left: 0px; right: 0px; transform: translate3d(0px, 0px, 0px); position: fixed; z-index: 1100; transition: all 300ms ease 0s;display:none;">
        <div class="layout-title sku-box-shadow name-card sku-name-card">
            <div class="thumb"><img src="<%=imgurl %>" alt=""></div>
            <div class="detail goods-base-info clearfix">
                <p class="title c-black ellipsis"><%=pro_name %></p>
                <div class="goods-price clearfix">
                    <div class="current-price c-black pull-left">
                        <span class="price-name pull-left font-size-14 c-orangef60">￥</span>
                        <i class="js-goods-price price font-size-18 c-orangef60 vertical-middle"><%=price %></i>
                    </div>
                </div>
            </div> 
            <div class="js-cancel sku-cancel">
                <div class="cancel-img"></div>
            </div>
    </div>
    
    <div style="height: 123px; " class="adv-opts layout-content">
        <div class="goods-models js-sku-views block block-list block-border-top-none">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>数量</label>
		</dt>
		<dd>
		<dl class="clearfix">
				<div class="quantity">
				<div class="response-area response-area-minus"></div>            
				<button disabled="disabled" class="minus " type="button"></button>            
				<input id="number" class="txt" value="1" type="number" />            
				<button class="plus" type="button"></button>            
				<div class="response-area response-area-plus"></div>            
				<div class="txtCover"></div>        </div>
				<div class="stock pull-right font-size-12" />
				<!--<dt class="model-title stock-label pull-left">
			        <label>剩余: </label>
			        </dt>
			        <dd class="stock-num">
				        10
			     </dd>-->
			
			</div>
		</dl>
		</dd></dl>
		
		<div style="display: none;" class="block-item block-item-messages"></div></div>
        <div class="confirm-action content-foot">
    <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark">下一步</a>
	</div>
    </div>
</div>  
                
                


   <div id="right-icon" class="icon-hide   no-border hide" data-count = "1">
	<div class="right-icon-container clearfix" style="width: 50px">
		
		<a id="global-cart" href="Cart.aspx" class="no-text" style="background-image: url(image/s0.png);
							background-size: 50px 50px;
							background-position: center;">
			<p class="right-icon-img"></p>
			<p class="right-icon-txt">购物车</p>
		</a>
	</div>
</div>
<div id='cart' style=" display:none;position: absolute; bottom: 6em; right: 2em; width: 55px; height:55px; background-color: #FFFAFA; margin:10px; border-radius:60px; border: solid rgb(55,55,55)  #FF0000   1px;cursor:pointer;"><a href="Cart.aspx"><img src="/images/cart.gif" width="39" style="padding:8px 0 0 9px;"/></a></div>
   

<div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>
<div id="cartloading" class="loading" style="display:;bottom: 220px;">
            已成功添加到购物车
        </div>

        



        <!-- js在最后 -->
<script>
    $(function () {
        new Swipe(document.getElementById('banner_box'), {
            speed: 500,
            auto: 3000,
            callback: function () {
                var lis = $(this.element).next("ol").children();
                lis.removeClass("on").eq(this.index).addClass("on");
            }
        });
    });
	</script>
    <script type="text/javascript" src="/Scripts/hilove.js"></script>
<input id="hid_proid" value="<%=id %>" type="hidden"  />
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
    
    <input id="hid_price" value="<%=price %>" type="hidden"  />
    <input id="hid_action" value="1" type="hidden"  />

    <input id="hid_jishicount" value="<%=shijiacha %>" type="hidden"  />
    <input id="hid_dinggou" value="0" type="hidden"  />


    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=imgurl %>",
                desc: "<%=remark %>",
                title: ' <%=pro_name %>'
            });
        });
    </script>

    </body>
</html>