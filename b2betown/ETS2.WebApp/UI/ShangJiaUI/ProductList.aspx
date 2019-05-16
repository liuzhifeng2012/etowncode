<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.ProList" %>
<html><head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">

<title id="js-meta-title">
<%=Com_name%>
</title>
<!-- ▼ Common CSS -->

<link rel="stylesheet" href="/Styles/pc/pc_Bootstrap.css">
<link rel="stylesheet" href="/Styles/pc/pc_man.css"><!-- ▲ Common CSS -->

<!-- ▼ App CSS -->
<link rel="stylesheet" href="/Styles/pc/list_css.css" >
<link rel="stylesheet" href="/Styles/pc/list_css2.css" >
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script> 
<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
<script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
<script src="/Scripts/hoverdelay.js" type="text/javascript"></script>
<script src="/Scripts/MenuButton.js" type="text/javascript"></script>
<script src="/Scripts/common.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        //根据浏览器类型判断跳转页面
        var browsertype = fBrowserRedirect();
        if (browsertype == "mobile"||browsertype=="ipad") {
            window.open("/h5/order/",target="_self");
            return;
        }
        else {
//            window.open("/ui/shangjiaui/ProductList.aspx",target="_self");
//            return;
        }
   

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


    });

         //装载产品列表
            function SearchPorList(pageindex, pageSize,menuid,projectid,viewlist) {

                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=SelectMenupagelist",
                    data: { comid: <%=comid %>, pageindex: pageindex, pagesize: pageSize,menuid:menuid,projectid:projectid },
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
                            $("#"+viewlist).empty();
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#"+viewlist);

                        }
                    }
                })
            }


            //装载产品列表
            function SearchWxList(pageindex, pageSize,menuid,projectid,viewlist) {

                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=shoppagelist",
                    data: { comid: <%=comid %>, pageindex: pageindex, pagesize: pageSize,menuid:menuid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {
                            if (data.totalCount == 0) {
                                //("#tblist").html("<tr><td height=\"26\" colspan=\"7\">查询数据为空</td></tr>");
                            } else {
                                $("#loading").hide();
                                $("#"+viewlist).empty();
                                $("#WxItemp").tmpl(data.msg).appendTo("#"+viewlist);
                            }


                        }
                    }
                })

            }

</script>
</head>
<body class="theme theme--blue" style="">
    <%if (viewtop == 1)
      { %>
<!-- ▼ Main header -->
<header class="ui-header">

    <div class="ui-header-inner clearfix">
        <div class="ui-header-logo">
            <a href="javascript:;" class="js-hover logo" data-target="js-shop-info">
        <%=Com_name%>              <span class="smaller-title hide"><%=Scenic_name %></span>
      </a>
        </div>
        <nav class="ui-header-nav">
            <ul class="clearfix">
                             
    <li><a href="ProductList.aspx">首页</a></li>
	 <li class="divide">|</li>
    <li ><a href="PJList.aspx">全部产品</a></li>
    <%if (RequestUrl != "lvye.etown.cn")
      { %><li class="divide">|</li>
    <li ><a href="Article.aspx">最新文章</a></li>     <%} %>  
      <%if (comid == 1194 || comid == 106)
        {%>
      <li class="divide">|</li>
    <li><a href="http://yd.wlski.com/SellOnline/sellIndex.aspx?UType=3&UCode=etowncn&Upass=24611B10993D318F" target="_blank">万龙滑雪票预订</a>
    <% } %>
        </ul>
        </nav>
    </div>

</header>
<!-- ▲ Main header -->
    <%} %>
<!-- ▼ Main container -->
<div id="Maintop"></div>
  <div class="container goods-detail-main clearfix">
    <div class="content">
	 <%
     if (menutotalcount>0){

         for (int i = 0; i < menulist.Count; i++)
         {
                                
                %>
    <!-- 图片广告 -->
    <div class="shop_module_floor custom-image-swiper custom-image-swiper-single custom-image-swiper-single2">
        <div class="swiper-container" >
            <div class="swiper-wrapper">
               <div class="swiper-slide">
                 
                  <% if (menulist[i].Usestyle == 1)
                     { %>
                   <a href="<%=menulist[i].Linkurl%>" target="_blank">
                    <img src="<%=menulist[i].Imgurl_address %>"  style="max-height: 84px; max-width: 100%;" height="50" width="320">
                    </a>
                  <%}
                     else
                     { %>
                  <%if (menulist[i].Fonticon != "")
                    { %>
                  <!-- 标题    --> 
                          
                   <!--<span class="<%=menulist[i].Fonticon %>" id="tab" style="padding-left:10px; font-size:40px;color:#F60; line-height:40px;vertical-align: bottom;"></span> --> 
                   <%} %>


                    <!-- 标题    --> 
                   <div class="shop_module_hd"><h4>
                  <%if (menulist[i].Menutype == 0)
                    { %>
                   
                   <a href="PJList.aspx?projectid=<%=menulist[i].Projectlist%>"> 
                   <%}
                    else
                    { %>
                    <a href="Article.aspx?promotetypeid=<%=menulist[i].Projectlist%>"> 

                   <%} %>
                   <%if (menulist[i].Fonticon != "")
                     { %>
                  <!-- 标题    --> 
                   <span class="<%=menulist[i].Fonticon %>" id="tab" style="color:#F60;font-size: 20px;"></span>
                   <%} %> <%=menulist[i].Name%><i class="icon_arrow"></i></a></h4></div>


                    <%}%>
                 
               </div>
            </div>
        </div>
    </div>
    <!-- 商品区域 -->
    <ul id="list<%=menulist[i].Id%>" class="sc-goods-list pic clearfix size-1 ">
    <script>
     $(function () {
      <%if(menulist[i].Menutype==0){ %>
        SearchPorList(1,20,<%=menulist[i].Id%>,<%=menulist[i].Projectlist%>,"list<%=menulist[i].Id%>");
         <%} %>

        <%if(menulist[i].Menutype==1){ %>
        SearchWxList(1,20,<%=menulist[i].Id%>,<%=menulist[i].Projectlist%>,"list<%=menulist[i].Id%>");
        <%} %>
     })
    </script>
    </ul>
    <%     
            }
     }else{ %>

     <% 
         if (projectlist != null)
         {
             for (int i = 0; i < projectlist.Count; i++)
             {  %>

      <!-- 图片广告 -->
    <div class="shop_module_floor custom-image-swiper custom-image-swiper-single custom-image-swiper-single2">
        <div class="swiper-container" >
            <div class="swiper-wrapper">
               <div class="swiper-slide">
                    <!-- 标题    --> 
                   <div class="shop_module_hd"><h4><a href="PJList.aspx?projectid=<%=projectlist[i].Id %>"> 
                  <%=projectlist[i].Projectname%><i class="icon_arrow"></i></a></h4></div>
               </div>
            </div>
        </div>
    </div>
        <!-- 商品区域 -->
    <ul id="list<%=projectlist[i].Id%>" class="sc-goods-list pic clearfix size-1 ">
    <script>
     $(function () {
        SearchPorList(1,20,0,<%=projectlist[i].Id%>,"list<%=projectlist[i].Id%>");
     })
    </script>
    </ul>


    <%}
         } %>
   <%} %>


	  
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
          <span class="shop-name"><%=Com_name %>  </span>
                       <a href="javascript:;" class="btn-add-wx js-hover" data-target="popover-weixin" data-position="bottom" data-align="right" data-shake="true">加微信好友</a>
                  </div>
      </div>
            <div class="sidebar-section" style=" padding-bottom:20px;">
        <h5 class="section-title"><span>客服方式</span></h5>
        <table class="sidebar-service">
          <tbody id="qqsevice">
            <tr>
              <td>电话：</td>
              <td><%=Tel %></td>
            </tr>
            <%if (weixinname != "")
              { %>
             <tr>
              <td>
                微信：
              </td>
              <td>
                <%=weixinname %>  </td>
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
    <div class="popover popover-goods js-popover-goods" style="left: 632px; top: 44px; display: none;">
      <div class="popover-inner">
        <h4 class="title clearfix"><span class="icon-weixin pull-left"></span>手机启动微信<br>扫一扫购买</h4>
        <div class="ui-goods-qrcode">
          <img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码" class="qrcode-img">
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

<div  class="shop-info shop-info-fixed js-shop-info hide">
      <div class="container clearfix">
        <div class="js-async shop-qrcode pull-left" >
		<img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码"></div>
        <div class="shop-desc pull-left">
          <dl>
            <dt>
              <%=Com_name %>                          </dt>
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

<script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
        <li class="goods-card goods-list small-pic card ">
                    {{if (Server_type==2 || Server_type==8)}}
                    <a href="/h5/linedetail.aspx?lineid=${Id}" class="link js-goods clearfix"  data-goods-id="6655060" title="${Pro_name}">
                    {{else}}
                      {{if Server_type==9}}
                          <a href="/h5/hotel/Hotelshow.aspx?proid=${Id}" class="link js-goods clearfix"  data-goods-id="6655060" title="${Pro_name}">
                        {{else}}
                           {{if  Server_type==10}}
                                <a href="pro.aspx?id=${Id}" class="link js-goods clearfix" data-goods-id="6655060" title="${Pro_name}">
                             {{else}}
                                <a href="pro.aspx?id=${Id}" class="link js-goods clearfix" data-goods-id="6655060" title="${Pro_name}">
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

                        <script type="text/x-jquery-tmpl" id="WxItemp"> 
               <li class="goods-cardwx  normalwx"> 
                 <div class="custom-messages single">
                    <a href="Article.aspx?id=${MaterialId}" class="clearfix">
                        <div class="custom-messages-image">
                                                <div class="image">
                                <img src="${Imgpath}" style="display: inline;" class="js-lazy" data-src="${Imgpath}">
                            </div>
                        </div>
                        <div class="custom-messages-content">
                            <h4 class="title">
                                ${Title}                    </h4>
                                                <div class="summary">
                                ${cutstr(Summary,240) }                 
                                 {{if Price!=0}}
                                    ¥${Price}<em>起</em>
                                {{/if}}
                        
                                 </div>
                    
                        </div>
                    </a>
                </div>
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
      <a href="javascript:;"> <%=Copyright%></a>
    </div>
  </div>
  <%} %>
            <div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>
</body></html>