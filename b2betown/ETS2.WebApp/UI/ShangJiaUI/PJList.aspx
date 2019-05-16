<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PJList.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.PJList" %>

<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title id="js-meta-title">
        <%=Com_name%>
    </title>
    <!-- ▼ Common CSS -->
    <link rel="stylesheet" href="/Styles/pc/pc_Bootstrap.css">
    <link rel="stylesheet" href="/Styles/pc/pc_man.css">
    <!-- ▲ Common CSS -->
    <!-- ▼ App CSS -->
    <link rel="stylesheet" href="/Styles/pc/list_css.css">
    <link rel="stylesheet" href="/Styles/pc/list_css2.css">
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

        $(".btn-fx-buy").click(function () {
            $(".popover-goods").show();
            shake('popover-goods');
        });

        $(".popover-goods").hover(function () {
            $(".popover-goods").show().jshaker();
        }, function () {
            $(".popover-goods").delay(10).hide(0);
        });

        $(".swiper-pagination-switch").click(function () {
            var index = $(this).attr("data-index");
            var removepx = -(index * 400);
            $(".swiper-wrapper").css({ "left": removepx + "px" });
        });



        var stop = true;
        $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                       var pageindex = parseInt($("#pageindex").val()) + 1;
                       var pageSize = parseInt($("#num").val()) + 10;
                       var menuid = "<%=menuid%>";
                       var projectid = "<%=menuid%>";


                       stop = false;
                       
                       SearchPorList(pageindex,12,<%=menuid%>,<%=projectid%>,"list");
                       stop = true;
                    }
                }
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
                url: "/JsonFactory/ProductHandler.ashx?oper=TopPropagelist",
                data: { comid: comid, pageindex: 1, pagesize: 10},
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        var htmlstr=' <h5 class="section-title"><span>热门商品</span></h5><ul class="js-async" >';

                         for (var i = 0; i < data.msg.length; i++) {
                             htmlstr += '<li class="js-log" >';
                             htmlstr += '    <a href="pro.aspx?id='+data.msg[i].Id+'">';
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

            $("#search_botton").click(function(){
              SearchPorList(1,12,<%=menuid%>,<%=projectid%>,"list");
            })

            document.onkeydown = function(e){ 
                var ev = document.all ? window.event : e;
                if(ev.keyCode==13) {
                       SearchPorList(1,12,<%=menuid%>,<%=projectid%>,"list");
                 }
            }


    });

         //装载产品列表
            function SearchPorList(pageindex, pageSize,menuid,projectid,viewlist) {

                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=SelectMenupagelist",
                    data: { comid: <%=comid %>, pageindex: pageindex, pagesize: pageSize,menuid:menuid,projectid:projectid,key:$("#search_name").val() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                           // $("#list").hide();
                            //$("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#"+viewlist);
                            $("#pageindex").val(pageindex);

                        }
                    }
                })
            }
            
            
    </script>
    <style type="text/css">
        .list-search
        {
            height: 32px;
            padding: 5px 10px 7px;
            background: #eee;
        }
        .list-search dl
        {
            height: 32px;
            background: #fff;
            border-radius: 5px;
            border: 1px solid #c9c9c9;
            position: relative;
        }
        .list-search dt
        {
            position: relative;
            overflow: hidden;
            padding-left: 5px;
            margin-right: 40px;
        }
        .list-search dt input
        {
            height: 25px;
            margin-top: 4px;
            border: 0;
            outline: 0;
            background: 0;
            width: 100%;
        }
        .list-search dd
        {
            float: left;
            width: 30px;
            height: 25px;
            margin-top: 4px;
            position: absolute;
            top: 0;
            right: 0;
        }
        .list-search dd s
        {
            width: 17px;
            height: 17px;
            display: block;
            vertical-align: middle;
            margin: 4px auto 0;
            background: url(http://shop.etown.cn/Images/public_com.png) no-repeat -44px 0;
            background-size: 64px 17.5px;
        }
        .fn-clear:after
        {
            visibility: hidden;
            display: block;
            font-size: 0;
            content: " ";
            clear: both;
            height: 0;
        }
    </style>
</head>
<body class="theme theme--blue" style="">
    <%if (viewtop == 1)
      { %>
    <!-- ▼ Main header -->
    <header class="ui-header">
    <div class="ui-header-inner clearfix">
        <div class="ui-header-logo">
            <a href="javascript:;" class="js-hover logo" data-target="js-shop-info">
        <%=Com_name%>              <span class="smaller-title hide"><%=Scenic_name%></span>
      </a>
        </div>
        <nav class="ui-header-nav">
            <ul class="clearfix">
                             
    <li><a href="ProductList.aspx">首页</a></li>
	 <li class="divide">|</li>
    <li ><a href="PJList.aspx">全部产品</a></li>          
    <%if (RequestUrl != "lvye.etown.cn")
      { %><li class="divide">|</li>
    <li ><a href="Article.aspx">最新文章</a><%} %></li>
    <%if (comid == 1194 || comid == 106)
      {%>
    <li class="divide">|</li>
    <li><a href="http://yd.wlski.com/SellOnline/sellIndex.aspx?UType=3&UCode=etowncn&Upass=24611B10993D318F" target="_blank">万龙滑雪票预订</a>
    <% } %>
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
    <div id="Maintop">
    </div>
    <div class="container goods-detail-main clearfix">
        <div class="content">
            <div class="shop_module_floor custom-image-swiper custom-image-swiper-single custom-image-swiper-single2">
                <div class="swiper-container">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide">
                            <!-- 标题    -->

                            <div class="list-search">
                                <dl class="fn-clear">
                                    <dt>
                                        <input type="text" id="search_name">
                                    </dt>
                                    <dd>
                                        <s id="search_botton"></s>
                                    </dd>
                                </dl>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- 商品区域 -->
            <ul id="list" class="sc-goods-list pic clearfix size-1 ">
                <script type="text/javascript">
     $(function () {
        SearchPorList(1,12,<%=menuid%>,<%=projectid%>,"list");

          $("#search_botton").click(function(){
              $("#list").empty();
              SearchPorList(1,12,<%=menuid%>,<%=projectid%>,"list");
          })
     })
                </script>
            </ul>
        </div>
        <%if (viewtop == 1)
          { %>
        <div class="sidebar">
            <!-- 店铺简单信息 -->
            <div class="shop-card clearfix">
                <div class="shop-image pull-left">
                    <img src="<%=comlogo %>" alt="<%=Com_name %>" />
                    <span class="icon-wxv"></span>
                </div>
                <div class="shop-link pull-left">
                    <span class="shop-name">
                        <%=Com_name%>
                    </span><a href="javascript:;" class="btn-add-wx js-hover" data-target="popover-weixin"
                        data-position="bottom" data-align="right" data-shake="true">加微信好友</a>
                </div>
            </div>
            <div class="sidebar-section" style="padding-bottom: 20px;">
                <h5 class="section-title">
                    <span>客服方式</span></h5>
                <table class="sidebar-service">
                    <tbody id="qqsevice">
                        <tr>
                            <td>
                                电话：
                            </td>
                            <td>
                                <%=Tel%>
                            </td>
                        </tr>
                        <%if (weixinname != "")
                          { %>
                        <tr>
                            <td>
                                微信：
                            </td>
                            <td>
                                <%=weixinname%>
                            </td>
                        </tr>
                        <%} %>
                        <%if (Qq != "")
                          { %>
                        <tr>
                            <td>
                                QQ：
                            </td>
                            <td>
                                <a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=<%=Qq %>&site=qq&menu=yes">
                                    <img border="0" src="/images/qq.png" alt="<%=Qq %>" title="<%=Qq %>"></a>
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
        <div class="popover popover-goods js-popover-goods" style="left: 632px; top: 44px;
            display: none;">
            <div class="popover-inner">
                <h4 class="title clearfix">
                    <span class="icon-weixin pull-left"></span>手机启动微信<br>
                    扫一扫购买</h4>
                <div class="ui-goods-qrcode">
                    <img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码"
                        class="qrcode-img">
                </div>
            </div>
        </div>
        <div class="popover popover-weixin js-popover-weixin" style="left: 971.5px; top: 161px;
            display: none;">
            <div class="popover-inner">
                <h4 class="title">
                    打开微信，扫一扫</h4>
                <h5 class="sub-title">
                    或搜索微信号：<%=weixinname %></h5>
                <div class="ui-goods-qrcode">
                    <img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码"
                        class="qrcode-img">
                </div>
            </div>
        </div>
        <div class="shop-info shop-info-fixed js-shop-info hide">
            <div class="container clearfix">
                <div class="js-async shop-qrcode pull-left">
                    <img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码"></div>
                <div class="shop-desc pull-left">
                    <dl>
                        <dt>
                            <%=Com_name %>
                        </dt>
                        <dd>
                        </dd>
                        <dt>微信扫描二维码，访问我们的微信店铺</dt>
                    </dl>
                </div>
                <span class="arrow"></span>
            </div>
        </div>
        <div class="js-notifications notifications">
        </div>
        <div class="back-to-top hide">
            <a href="#" class="js-back-to-top"><i class="icon-chevron-up"></i>返回顶部</a>
        </div>
        <div class="popover-logistics popover" style="display: none; top: 272px; left: 488px;">
            <div class="arrow">
            </div>
            <div class="popover-inner">
                <div class="popover-content">
                    <div class="logistics-content js-logistics-region" style="min-height: 30px;">
                    </div>
                </div>
            </div>
            <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
        <li class="goods-card goods-list small-pic card ">
                    {{if (Server_type==2 || Server_type==8)}}
                        <a href="/h5/linedetail.aspx?lineid=${Id}" class="link js-goods clearfix" data-goods-id="6655060" title="${Pro_name}">
                    {{else}}
                      {{if Server_type==9}}
                          <a href="/h5/hotel/Hotelshow.aspx?proid=${Id}&id=${Comid}" class="link js-goods clearfix" data-goods-id="6655060" title="${Pro_name}">
                        {{else}}
                           {{if  Server_type==10}}
                            <a href="pro.aspx?id=${Id}" class="link js-goods clearfix" data-goods-id="6655060" title="${Pro_name}">
                             {{else}}
                                <a href="pro.aspx?id=${Id}" class="link js-goods clearfix"  data-goods-id="6655060" title="${Pro_name}">
                           {{/if}}
                      {{/if}}
				   
                    {{/if}}
					<div class="photo-block">
                        {{if (Ispanicbuy==1 || Ispanicbuy==2)}}
                            {{if (Limitbuytotalnum<=0)}}
                        <div class="Soldout"><img  src="image/5337034_100415061309_2.gif"  style="display: block;width:80px;padding-top:5px;padding-left:5px;"></div>
                            {{/if}}
                        {{/if}}
						<img class="goods-photo js-goods-lazy" data-width="640" data-height="640" data-src="${Imgurl}" src="${Imgurl}" style="display: block;">
					</div>
					<div class="info clearfix info-title info-price btn4">
						<p class=" goods-title ">${Pro_name}</p>
						<p class="goods-sub-title c-black hide">  ${Pro_explain}</p>
						<p class="goods-price">
                        {{if (Server_type==9)}}
                        <em>￥${HousetypeNowdayprice}</em>
                           {{else}}
                           <em>￥${Advise_price}</em>
                        {{/if}}    
						</p>
						<p class="goods-price-taobao">特价：￥${Advise_price}</p>   
                        {{if (Server_type==11)}}
                          <div class="goods-buy btn1"></div>
                        {{/if}}
					</div></a>
					<div class="goods-buy "> </div>  
						<div onclick="suborder('${Id}','${Pro_name}','${Imgurl}','${Advise_price}','${Server_type}');"  class="js-goods-buy buy-response" data-alias="11sp135rd" data-postage="0" data-buyway="1" data-id="6655060" data-title="${Project_name}" data-price="${Advise_price}"></div>  						
			</li>
            </script>
        </div>
    </div>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
    <input id="hid_action" type="hidden" value="1" />
    <input id="hid_proid" type="hidden" value="0" />
    <input id="pageindex" type="hidden" value="1" />
    <%if (viewtop == 1)
      { %>
    <div class="footer">
        <div class="container">
            <a href="javascript:;">
                <%=Copyright %></a>
        </div>
    </div>
    <%} %>
    <div id="loading" class="loading" style="display: none;">
        正在加载...
    </div>
</body>
</html>
