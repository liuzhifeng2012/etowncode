<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pro.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.Pro" %>
<html><head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">

<title id="js-meta-title">
<%=pro_name %> - <%=title%>
</title>
<!-- ▼ Common CSS -->

<link rel="stylesheet" href="/Styles/pc/pc_Bootstrap.css">
<link rel="stylesheet" href="/Styles/pc/pc_man.css">
<link rel="stylesheet" href="/Styles/pc/pc_swiper.css">

<!-- ▲ Common CSS -->

<!-- ▼ App CSS -->
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script> 
<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
<script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
<script src="/Scripts/hoverdelay.js" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        var comid = $("#hid_comid").val();
        var proid = $("#hid_proid").val();


        $(".js-hover").hoverDelay({
            hoverEvent: function(){
                $(".shop-info").removeClass("hide");
                $(".shop-info").removeClass("hidestate");
            },
            outEvent: function () {
                if($(".shop-info").hasClass("hidestate")){
                }else{
                 $(".shop-info").addClass("hide");
                }
            }
        });

        $(".shop-info").hover(function () {
            $(".shop-info").addClass("hidestate");
            $(".shop-info").removeClass("hide");
        }, function () {
           $(".shop-info").addClass("hide");
           $(".shop-info").removeClass("hidestate");
        });


        $(".icon-preview").hoverDelay({
             hoverEvent: function(){
                $(".popover-goods").removeClass("hide");
                $(".popover-goods").removeClass("hidestate");
                shake('popover-goods');
            },
            outEvent: function () {
                if($(".popover-goods").hasClass("hidestate")){
                }else{
                 $(".popover-goods").addClass("hide");
                }
               
            }
        });
        <%if ((pro_servertype==1 || pro_servertype==10) && (comid !=112 && comid !=1194))
        
        { %>
            
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
            $("#buy1").removeClass("btn-fx-stopbuy");
            $("#buy1").addClass("btn-fx-buy");
            $("#buy1").html("火热抢购中..");
            $("#hid_dinggou").val("1");
        }



        <%if (Ispanicbuy==1 ){ %>
         <%if (Limitbuytotalnum<=0){%>
            $("#buy1").removeClass("btn-fx-buy");
            $("#buy1").addClass("btn-fx-stopbuy");
            $("#buy1").html("抢购已结束");
            $("#hid_dinggou").val("0");

         <% }else{%>
         
            <%
             if (panic_begintime <= nowtoday && nowtoday<panicbuy_endtime){//抢购在有效期范围内
             %>
                $("#buy1").html("火热抢购中");
                $("#hid_dinggou").val("1");
             <%
             }else if(panic_begintime>nowtoday){         
             %>

                   $("#buy1").removeClass("btn-fx-buy");
                   $("#buy1").addClass("btn-fx-stopbuy");
                   $('#buy1').html('抢购 距开始还剩 <span id="day_show"></span><span class="hour_show"></span><span class="minute_show"></span><span class="second_show"></span>')
                   var jishicount=$('#hid_jishicount').val();

                   var intDiff = parseInt(jishicount); //倒计时
                   timer(intDiff);
                   $("#hid_dinggou").val("0");
             <%
              }else{
             %>
                $("#buy1").removeClass("btn-fx-buy");
                $("#buy1").addClass("btn-fx-stopbuy");
                $("#buy1").html("抢购已结束");
                $("#hid_dinggou").val("0");
              <%} 

              %>

        <%
        }
        }
        else if(Ispanicbuy==2){
        %>
         <%if (Limitbuytotalnum<=0){%>
            $("#buy1").removeClass("btn-fx-buy");
            $("#buy1").addClass("btn-fx-stopbuy");
            $("#buy1").html("抢购已结束");
            $("#hid_dinggou").val("0");

         <%
          }
         } %>


            $("#buy1").click(function () {
              if(<%=Ispanicbuy %>==1||<%=Ispanicbuy %>==2){
                var qianggou=$("#hid_dinggou").val();
                if(qianggou !=0){
                   $("#loading").show();
                   window.location.href="/ui/shangjiaui/createorder.aspx?proid="+proid;
                }else{
                   if(<%=Ispanicbuy %>==1){
                      $("#buy1").html("抢购已结束");
                   }else{
                      $("#buy1").html("产品已售完");
                   }
                }
              }else{
                  $("#loading").show();
                  window.location.href="/ui/shangjiaui/createorder.aspx?proid="+proid;
              }
               

             }); 


        <%}else{ %>
           $(".btn-fx-buy").click(function () {
     
                    $(".popover-goods").removeClass("hide");
                    $(".popover-goods").removeClass("hidestate");
                    shake('popover-goods');
           });

           $(".btn-fx-buy").hoverDelay({
                outEvent: function () {
                    if($(".popover-goods").hasClass("hidestate")){
                    }else{
                     $(".popover-goods").addClass("hide");
                    }
               
                }
            });

        <%} %>




         $(".popover-goods").hoverDelay({
            outEvent: function () {
                if($(".popover-goods").hasClass("hidestate")){
                }else{
                 $(".popover-goods").addClass("hide");
                }
            }
        });



        $(".popover-goods").hover(function () {
             $(".popover-goods").removeClass("hide");
               $(".popover-goods").addClass("hidestate");
        }, function () {
             $(".popover-goods").addClass("hide");
              $(".popover-goods").removeClass("hidestate");
        });

        $(".swiper-pagination-switch").click(function () {
            var index = $(this).attr("data-index");
            var removepx = -(index * 400);
            $(".swiper-wrapper").css({ "left": removepx + "px" });
        });

      
                       
                       
                        var img_tab = "";
                        var img_list = "";
                        var img_tab_html = "";
                        var img_list_html = "";


                        <%if (imgurl !=""){ %>
                            img_list += '<div class="swiper-slide swiper-slide-visible swiper-slide-active" style="width: 400px; height: 400px;">';
                            img_list += '  <div class="swiper-image">';
                            img_list += '    <img src="<%=imgurl %>" alt="商品图">';
                            img_list += '  </div>';
                            img_list += '</div>';

                            img_tab += '  <span class="swiper-pagination-switch swiper-active-switch" data-index="0">';
                            img_tab += '  <img src="<%=imgurl %>" width="44" height="44" alt="商品缩略图">';
                            img_tab += '  <span class="swiper-arrow"></span>';
                            img_tab += '  </span>';
                        <%} %>

        //加载图片 ;
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

                            img_list += '<div class="swiper-slide swiper-slide-visible swiper-slide-active" style="width: 400px; height: 400px;">';
                            img_list += '  <div class="swiper-image">';
                            img_list += '    <img src="' + data.msg[i].imgurl + '" alt="商品图">';
                            img_list += '  </div>';
                            img_list += '</div>';

                            img_tab += '  <span class="swiper-pagination-switch" data-index="'+(i+1) +'">';
                            img_tab += '  <img src="' + data.msg[i].imgurl + '" width="44" height="44" alt="商品缩略图">';
                            img_tab += '  <span class="swiper-arrow"></span>';
                            img_tab += '  </span>';
                        }

                    }
                }
            }
        });

         img_list_html += '<div class="swiper-wrapper" style="width: 2000px; height: 400px; -webkit-transform: translate3d(0px, 0px, 0px); transition: 0.75s; -webkit-transition: 0.75s;">';
         img_list_html += img_list;
         img_list_html += '</div>';


         img_tab_html += ' <div class="swiper-pagination-list">';
         img_tab_html += img_tab;
         img_tab_html += ' </div>';

         $(".swiper-container").html(img_list_html);
         $(".swiper-pagination").html(img_tab_html);
         


         $.post("/JsonFactory/WeiXinHandler.ashx?oper=editwxqrcode", { productid: proid, onlinestatus: 1, channelid: 0, qrcodeid: 0, comid: comid, qrcodename: "系统生成产品id:" + proid, promoteact: 0, promotechannelcompany: 0 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    return;
                }
                if (data.type == 100) {
                    if(data.qrcodeurl==""){
                     $("#proerweima").attr("src", "http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>");
                    }else{
                    $("#proerweima").attr("src", data.qrcodeurl);
                    }

                }else{
                $("#proerweima").attr("src", "http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>");
                }
         })



        $(".swiper-pagination-switch").click(function(){
                $(".swiper-pagination-switch").removeClass("swiper-active-switch");
                $(this).addClass("swiper-active-switch");
				var index= $(this).attr("data-index");
				var removepx =-(index*400);
				$(".swiper-wrapper").css({"left":removepx+"px"});
		});



         //加载qq
         $("#loading").show();
            $.ajax({
                type: "post",
                url: "/JsonFactory/CrmMemberHandler.ashx?oper=channelqqList",
                data: { comid: comid, pageindex: 1, pagesize: 12 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    $("#loading").hide();
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        var qqstr="";
                         for (var i = 0; i < data.msg.length; i++) {
                            qqstr+='<tr><td>QQ：</td><td><a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin='+ data.msg[i].QQ+'&site=qq&menu=yes"><img border="0" src="/images/qq.png" alt="'+ data.msg[i].QQ+'" title="'+ data.msg[i].QQ+'"></a></td></tr>';
                        }
                        $("#qqsevice").append(qqstr);

                    }
                }
            })


            //加载相关产品
             $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=XiangguanPropagelist",
                data: { comid: comid, pageindex: 1, pagesize: 10,proid:proid},
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        var htmlstr=' <h5 class="section-title"><span>相关商品</span></h5><ul class="js-async" >';

                         for (var i = 0; i < data.msg.length; i++) {
                             htmlstr += '<li class="js-log" >';
                             htmlstr += '    <a href="pro.aspx?id='+data.msg[i].Id+'" target="_blank">';
                             htmlstr += '      <div class="image">';
                             htmlstr += '        <img src="'+ data.msg[i].Imgurl +'" alt="'+ data.msg[i].Pro_name +'" height="160" width="160">';
                             htmlstr += '      </div>';
                             htmlstr += '      <div class="title">'+ data.msg[i].Pro_name +'</div>';
                             htmlstr += '      <div class="price">售价：￥'+ data.msg[i].Advise_price +'</div>';
                             //htmlstr += '      <div class="fx-price">成本价：￥'+ data.msg[i].Advise_price +'</div>';
                             htmlstr += '    </a>';
                             htmlstr += '  </li>';
                        }
                        htmlstr += '</ul>';


                        $("#otherpro").html(htmlstr);

                    }
                }
            })


           




    });


    function AutoScroll(){
	    var _scroll = $(".swiper-wrapper");
	    //往左边移动400px
	    _scroll.animate({marginLeft:"-400px"},1000,function(){

	    });
    }


</script>
</head>
<body class="theme theme--blue" style="">
    <%if (viewtop_pro == 1)
      { %>
<!-- ▼ Main header -->
<header class="ui-header">

    <div class="ui-header-inner clearfix">
        <div class="ui-header-logo">
            <a href="javascript:;" class="js-hover logo" data-target="js-shop-info">
        <%=title%>              <span class="smaller-title hide"><%=Scenic_name%></span>
      </a>
        </div>
        <nav class="ui-header-nav">
            <ul class="clearfix">
                             
    <li><a href="ProductList.aspx">首页</a></li>
	 <li class="divide">|</li>
    <li ><a href="PJList.aspx">全部产品</a></li>  
    <li class="divide">|</li>
    <li ><a href="Article.aspx">最新文章</a></li>                    
    </ul>
        </nav>
                  <!--  <div class="ui-header-user">
                <div class="dropdown hover dropdown-right">
                   <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown">
                        <span class="txt">
                            客服电话  ： ******                        </span>
                        <i class="caret"></i>
                    </a>

                    <ul class="dropdown-menu">
                        <li class="divide"></li>
                                                <li><a href="out.aspx">退出</a></li>
                    </ul>
                </div>
            </div>-->
    </div>
  
</header>
<!-- ▲ Main header -->
  <%} %>
<!-- ▼ Main container -->
<div id="Maintop"></div>
  <div class="container goods-detail-main clearfix">
    <div class="content">
      <!-- 商品简介 -->
      <div class="goods-summary clearfix">
        <!-- 商品信息 -->
        <div class="goods-info pull-right">
          <div id="js-goods-info" class="goods-info-section"><div data-reactid=".0">
		  <h3 class="goods-title" style=" padding-bottom:10px;"><%=pro_name %></h3>
		  <table >
   <tbody>
    <tr >
     <td colspan="2" class="goods-meta-name"  height="80">
      	<div class="goods-price clearfix">
      		<strong class="goods-current-price pull-left" ><em class="goods-rmb">￥</em><span ><%=price %></span></strong>      			</div></td>
     </tr>
     <%if (Face_price != 0)
       { %>
    <tr >
     <td class="goods-meta-name"  height="35">门市价</td>
     <td><span> ￥</span><span ><del><%=Face_price%></del></span></td>
    </tr>
    <%} %>
    <%if (Ispanicbuy != 0)
      { %>
    <tr >
     <td class="goods-meta-name"  height="35">库存数量</td>
     <td ><span class="sku-last-row-stock" ><span> </span><span><%=Limitbuytotalnum %></span><span>件可售</span></span></td>
    </tr>
    <%} %>
	<tr >
     <td class="goods-meta-name"  height="35">有效期</td>
     <td ><span class="sku-last-row-stock"><span><%=pro_youxiaoqi %></span></span></td>
    </tr>
   </tbody>
  </table>

  <div class="goods-action clearfix">
   <a id="buy1" href="javascript:;" class="btn-fx-buy pull-left">立即购买</a>
   <i class="icon-preview" data-position="top" data-target="js-popover-goods"></i>
  </div>
  
  <div class="goods-payment">
            <dl class="clearfix">
              <dt>支付：</dt>
              <dd class="pull-left clearfix">
                <a href="javascript:;" class="clearfix pull-left"><span class="icon-alipay"></span>支付宝</a>
              	  <a href="javascript:;" class="clearfix pull-left"><span class="icon-wxpay"></span>微信支付</a>
                  <a href="javascript:;" class="clearfix pull-left"><span class="icon-bankpay"></span>银行卡</a>
              </dd>
            </dl>
  </div>
  
  </div></div>
        </div>
        <!-- 商品图片 -->
        <div class="goods-image pull-left">
          <div class="swiper-container">
            

          </div>
          <div class="swiper-pagination">

          </div>


        </div>
      </div>
      <!-- 商品详情 -->
            <div class="goods-detail js-goods-detail">
        <ul class="title nav js-autofixed">
            <li class="active">
                <a href="#goods-detail">商品详情</a>
            </li>
        </ul>


        <div class="goods-desc" id="goods-detail">
          <h5 class="goods-desc-title">商品详情</h5>
          <div class="rich-text">
          
          
          <%=sumaryend%>
          
          </div>
        </div>
      </div>
          </div>
              <%if (viewtop_pro == 1)
                { %>
    <div class="sidebar">
      <!-- 店铺简单信息 -->
      <div class="shop-card clearfix">
        <div class="shop-image pull-left">
          <img src="<%=comlogo %>" alt="<%=title %>" />
                       <span class="icon-wxv"></span>
                  </div>
        <div class="shop-link pull-left">
          <span class="shop-name"><%=title%></span>
                       <a href="javascript:;" class="btn-add-wx js-hover" data-target="popover-weixin" data-position="bottom" data-align="right" data-shake="true">加微信好友</a>
                  </div>
      </div>
            <div class="sidebar-section" style=" padding-bottom:20px;">
        <h5 class="section-title"><span>客服方式</span></h5>
        <table class="sidebar-service">
          <tbody id="qqsevice">
                  <tr>
              <td>电话：</td>
              <td><%=Tel%></td>
            </tr>
            <%if (weixinname != "")
              { %>
             <tr>
              <td>
                微信：
              </td>
              <td>
                <%=weixinname%>  </td>
            </tr>
             <%} %>
            <%if (Qq != "")
              { %>
            <tr>
              <td>
                QQ：
              </td>
              <td>
                  <a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=<%=Qq %>&site=qq&menu=yes"><img border="0" src="/images/qq.png" alt="<%=Qq %>" title="<%=Qq %>"></a>
              </td>
            </tr>
            <%} %>
                      </tbody>
        </table>
      </div>

      <div class="sidebar-section" id="otherpro">

        </div>
                    
          </div>
 		  <%} %>
    <div class="hide popover popover-goods js-popover-goods " id="popover-goods" style="left: 760px; top: 30px;">
      <div class="popover-inner">
        <h4 class="title clearfix"><span class="icon-weixin pull-left"></span>手机启动微信<br>扫一扫购买</h4>
        <div class="ui-goods-qrcode">
          <img id="proerweima" src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码" class="qrcode-img">
        </div>
      </div>
    </div>
	
    <div class="popover popover-weixin js-popover-weixin" style="left: 971.5px; top: 161px; display: none;">
      <div class="popover-inner">
        <h4 class="title">打开微信，扫一扫</h4>
        <h5 class="sub-title">或搜索微信号：<%=weixinname %></h5>
        <div class="ui-goods-qrcode">
            <img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码" class="qrcode-img">
        </div>
      </div>
    </div>

<div class="shop-info shop-info-fixed js-shop-info hide">
      <div class="container clearfix">
        <div class="js-async shop-qrcode pull-left" >
		<img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码"></div>
        <div class="shop-desc pull-left">
          <dl>
            <dt>
              <%=title %>                          </dt>
            <dd></dd>
            <dt>微信扫描二维码，访问我们的微信店铺</dt>
                      </dl>
        </div>
        <span class="arrow"></span>      </div>
    </div>


<div class="js-notifications notifications"></div>
<div class="back-to-top hide">
    <a href="#" class="js-back-to-top"><i class="icon-chevron-up"></i>返回顶部</a>
</div>



<div class="popover-logistics popover" style="display: none; top: 272px; left: 488px;"><div class="arrow"></div>
<div class="popover-inner">
    <div class="popover-content">
        <div class="logistics-content js-logistics-region" style="min-height: 30px;"></div>
    </div>
</div>

</div>

  </div>

<input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
    <input id="hid_action" type="hidden" value="1" />
    <input id="hid_proid" type="hidden" value="<%=id %>" />
    <input id="pageindex" type="hidden" value="1" />
    <input id="hid_jishicount" type="hidden" value="<%=shijiacha %>" />
    <input id="hid_dinggou" value="0" type="hidden"  />
    
     <%if (viewtop_pro == 1)
      { %>
    <div class="footer">
    <div class="container">
      <a href="javascript:;"><%=Copyright %></a>
    </div>
  </div>
  <%} %>
   <div id="loading" class="loading" style="display: none;">
            正在加载...
   </div>
</body></html>