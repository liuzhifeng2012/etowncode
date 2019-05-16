<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ETS2.WebApp.H5.order.Default" %>

<html lang="zh-CN" class="no-js js-footer-autoheight"><head>

    <meta charset="utf-8">
    <meta content="<%=title %>" name="keywords">
    <meta content="" name="description">
    <meta content="True" name="HandheldFriendly">
    <meta content="320" name="MobileOptimized">
    <meta content="telephone=no" name="format-detection">
    <meta content="on" http-equiv="cleartype">


    <title><%=title %></title>

    <!-- meta viewport -->

    <meta id="viewport" content="width=device-width, user-scalable=yes,initial-scale=1" name="viewport" />
    <!-- CSS -->
    <link onerror="_cdnFallback(this)" href="css/css1.css" rel="stylesheet">    
	<link onerror="_cdnFallback(this)" href="css/css.css" rel="stylesheet"> 
    <link  href="css/bottommenu.css" rel="stylesheet"> 
    <link rel="stylesheet" type="text/css" href="/Styles/fontcss/font-awesome.min.css" />

    <!--[if IE 7]>
		          <link rel="stylesheet" href="/Styles/fontcss/font-awesome-ie7.min.css" />
    <![endif]-->

    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script> 
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>


<script  type="text/javascript">
    $(window).load(function () {
        var browsertype = fBrowserRedirect();
        if (browsertype == "mobile" || browsertype == "ipad") {
            //            window.open("/h5/order/",target="_self");
            //            return;
        }
        else {
            //window.open("/ui/shangjiaui/ProductList.aspx",target="_self");
           // return;
        }

        $("img").each(function () {
            var maxwidth = 540;
            if ($(this).width() > maxwidth) {
                var oldwidth = $(this).width();
                var oldheight = $(this).height();
                var newheight = maxwidth / oldwidth * oldheight;
                $(this).css({ width: maxwidth + "px", height: newheight + "px", cursor: "pointer" });
            }
        });

    });

</script>
    
    <script type="text/javascript">
        var pageindex = 1;
        var pageSize = 16;
        $(function () {
             var comid = $("#hid_comid").val();
            getmenubutton(comid,'js-navmenu');

            //根据公司id获得关注作者信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage btn btn-follow\">关注我们</a>");
                }
            });

            //查询购物车数量
                $.post("/JsonFactory/OrderHandler.ashx?oper=agentsearchcart", { userid: $("#hid_userid").val(), comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                    }
                    if (data.type == 100) {
                       $("#right-icon").removeClass("hide");
                    }
                })


         

            //立即购买，或加入购物车
           $(".js-confirm-it").click(function () {

            var num = parseInt($(".txt").val());
            var proid = $("#hid_proid").val();
            var action = $("#hid_action").val();

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
                                   $("#right-icon").removeClass("hide");
                                    $('#cartloading').html("成功添加到购物车");
                                    $('#cartloading').fadeOut(3000);
                                }
                            })
                  }
                 })
          }

        });

       $(".js-cancel").click(function () {
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

        

        
       });
            //装载产品列表
            function SearchPorList(pageindex, pageSize,menuid,projectid,viewlist,menuviewtype) {

                $("#loading").show();

                if(menuviewtype==0 || menuviewtype==3){
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

                if(menuviewtype==1){
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/ProductHandler.ashx?oper=SelectMenuHotelpagelist",
                        data: { comid: <%=comid %>, pageindex: pageindex, pagesize: pageSize,projectid:projectid },
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
                                $("#ProductItemHotel").tmpl(data.msg).appendTo("#"+viewlist);

                            }
                        }
                    })
                }

                if(menuviewtype==2){
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
                                $("#ProductItemShuttle").tmpl(data.msg).appendTo("#"+viewlist);

                            }
                        }
                    })
                }

            }


            //装载产品列表
            function SearchWxList(pageindex, pageSize,menuid,projectid,viewlist,menuviewtype) {

                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=shoppagelist",
                    data: { comid: <%=comid %>, pageindex: pageindex, pagesize: pageSize,menuid:menuid,applystate:1 },
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

            //产品id，名称，图片，action=1为直接购买，0为购物车
            function suborder(proid,title,img,price,action) {

               $("#hid_proid").val(proid);
               
               $("#hid_proid").val(proid);
               $("#buytitle").html(title); 
               $("#buyprice").html(price);
               $("#buyimg").html('<img src="'+img+'" alt="">'); 
               
               if(action==11){
                $("#hid_action").val(0);
                $(".js-confirm-it").html("加入购物车");
               }
               else if (action == 9) {
                location.href = "/h5/hotel/Hotelshow.aspx?proid="+proid+"&id="+$("#hid_comid");
                return;
               }
               else{
                  location.href = "/h5/order/pro.aspx?id=" + proid;
                   return;
//                $("#hid_action").val(1);
//                $(".js-confirm-it").html("下一步");
               }
                $("#QJwxuFqolZ").show();
                $("#zhegai").show();
                $(".txt").val(1);
            }
       
       

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
	<div class="content " style="min-height: 607px;">
        <div class="content-body">
            
                            
                                                
	<table class="custom-cube2-table">
	    <tbody>
	        <tr><td class="not-empty cols-4 rows-2 " colspan="4" rowspan="2" data-index="0">
            <a href="<%=bannerlink%>">
            <img src="<%=bannerimg%>" alt="<%=bannertitle %>" width="320" height="150"">
            </a>
        </td>
        </tr> 
        </tbody>
	</table>
           
  <div class="custom-search <%if (setsearch==0) {%> hide <%} %>">
    <form action="list.aspx" method="GET">
        <input class="custom-search-input" placeholder="商品搜索：请输入商品关键字" name="q" value="" type="search">
        <button type="submit" class="custom-search-button">搜索</button>
    </form>
    </div>
                    
         <!--           
        <div class="custom-notice">
    <div class="custom-notice-inner">
        <div class="custom-notice-scroll">
            <span class="js-scroll-notice" style="position: relative; left: 455px;">公告:</span>        </div>
    </div>
</div>-->
                    
           
    

    <%
     if (menutotalcount>0){
        
            for (int i = 0; i < menutotalcount;i++ ){ 
                
                
                
                %>

     
    <!-- 图片广告 -->
      <%if ((menulist[i].menuviewtype == 1 && menulist[i].Usestyle == 1) || menulist[i].menuviewtype == 0 || menulist[i].menuviewtype == 2)//如果是 酒店，没有设定图片的 则不现实上部广告标题，酒店的需要使用图片
      {%>

    <div class="shop_module_floor custom-image-swiper custom-image-swiper-single custom-image-swiper-single2">
        <div class="swiper-container" >
            <div class="swiper-wrapper">
               <div class="swiper-slide">
                 
                    <%if (menulist[i].Menutype == 0)//产品
                      {%>

                        <%if (menulist[i].menuviewtype == 0 )// 标准
                          { %>
                       <a href="<%=menulist[i].Linkurl%>"> 
                       <%} %>
                        <%if (menulist[i].menuviewtype == 1 )//酒店
                          { %>
                       <a href="/h5/order/hotel.aspx"> 
                       <%} %>
                        <%if (menulist[i].menuviewtype == 2)//大巴
                          { %>
                       <a href="/h5/order/ShuttleList.aspx"> 
                       <%} %>

                   <%}
                      else
                      { %>
                        <a href="javascript:;">
                    <%} %>

                        <%if (menulist[i].Imgurl_address != "" && menulist[i].Imgurl_address != "/Images/defaultThumb.png" && menulist[i].Usestyle == 1)
                          { %>
                            <img src="<%=menulist[i].Imgurl_address %>"  style=" max-width: 100%;" >
                        <%}
                          else
                          { %>
                          <span style="padding-left: 5px;"><%=menulist[i].Name%></span>
                        <%} %>

                    </a>

                 
               </div>
            </div>
        </div>
    </div>
    <%} %>
   

    <!-- 商品区域 -->

    <%if (menulist[i].menuviewtype != 2)//如果是 默认，酒店格式 按此格式加载
      {%>
    <ul id="list<%=menulist[i].Id%>" class="sc-goods-list pic clearfix size-1 ">
    <script>
     $(function () {
        <%if(menulist[i].Menutype==0){ %>
        SearchPorList(1,12,<%=menulist[i].Id%>,<%=menulist[i].Projectlist%>,"list<%=menulist[i].Id%>",<%=menulist[i].menuviewtype%>);
        <%} %>

        <%if(menulist[i].Menutype==1){ %>
        SearchWxList(1,12,<%=menulist[i].Id%>,<%=menulist[i].Projectlist%>,"list<%=menulist[i].Id%>",<%=menulist[i].menuviewtype%>);
        <%} %>

     })
    </script>
    </ul>
    <%}
      else//班车列表 按此样式加载
      { %>

      <ul id="list<%=menulist[i].Id%>" class="recommend_box">
    <script>
     $(function () {
        <%if(menulist[i].Menutype==0 ){ %>
        SearchPorList(1,12,<%=menulist[i].Id%>,<%=menulist[i].Projectlist%>,"list<%=menulist[i].Id%>",<%=menulist[i].menuviewtype%>);
        <%} %>

        <%if(menulist[i].Menutype==1){ %>
        SearchWxList(1,12,<%=menulist[i].Id%>,<%=menulist[i].Projectlist%>,"list<%=menulist[i].Id%>",<%=menulist[i].menuviewtype%>);
        <%} %>

     })
    </script>
    </ul>



    <%} %>

    <%      }
     }else{ %>

     <%  for (int m = 0; m < projectlist.Count; m++)
         { 
              %>

      <!-- 图片广告 -->
    <div class="shop_module_floor custom-image-swiper custom-image-swiper-single custom-image-swiper-single2">
        <div class="swiper-container" >
            <div class="swiper-wrapper">
               <div class="swiper-slide">
                    <!-- 标题    --> 
                   <div class="shop_module_hd"><h4><a href="/h5/order/list.aspx?projectid=<%=projectlist[m].Id %>"> 
                  <span style="padding-left: 5px;"><%=projectlist[m].Projectname%></span><i class="icon_arrow"></i></a></h4></div>
               </div>
            </div>
        </div>
    </div>
        <!-- 商品区域 -->
    <ul id="list<%=projectlist[m].Id%>" class="sc-goods-list pic clearfix size-1 ">
    <script>
     $(function () {
        SearchPorList(1,12,0,<%=projectlist[m].Id%>,"list<%=projectlist[m].Id%>",0);
     })
    </script>
    </ul>


    <%} %>
   <%} %>
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
   	
    
                    </div>        
			
			
		<div style="min-height: 1px;" class="js-footer">
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
        </div>     <!-- fuck taobao -->
    <div class="fullscreen-guide fuck-taobao hide" id="js-fuck-taobao">
        <span class="js-close-taobao guide-close">×</span>
        <span class="guide-arrow"></span>
        <div class="guide-inner">
            <div class="step step-1"></div>
            <div class="js-step-2 step"></div>
        </div>
    </div>    
	</div>

    <!-- JS -->

	<div class="motify"><div class="motify-inner"></div></div>

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

         {{if (isbig==0)}}
         <li class="goods-card goods-list small-pic card ">
         {{else}}
          <li class="js-goods-card goods-card small-pic card " style="width: 100%;">
         {{/if}}
                    {{if (Server_type==2 || Server_type==8)}}
                    <a href="/h5/linedetail.aspx?lineid=${Id}" class="link js-goods clearfix" target="_blank" data-goods-id="6655060" title="${Pro_name}">
                    {{else}}
                        {{if Server_type==9}}
                          <a href="/h5/hotel/Hotelshow.aspx?proid=${Id}&id=${Comid}" class="link js-goods clearfix" target="_blank" data-goods-id="6655060" title="${Pro_name}">
                        {{else}}
                        <a href="/h5/Order/pro.aspx?id=${Id}" class="link js-goods clearfix" target="_blank" data-goods-id="6655060" title="${Pro_name}">
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
                        <em>￥${ViewPrice(HousetypeNowdayprice)}</em>
                           {{else}}
                           <em>￥${ViewPrice(Advise_price)}</em>
                        {{/if}}    
						</p>
						<p class="goods-price-taobao">特价：￥${ ViewPrice(Advise_price)}</p>   
                        {{if (Server_type==11)}}
                          <div class="goods-buy btn1"></div>
                        {{/if}}
					</div></a>
					<div class="goods-buy "> </div>  
						<div onclick="suborder('${Id}','${Pro_name}','${Imgurl}','${Advise_price}','${Server_type}');"  class="js-goods-buy buy-response" data-alias="" data-postage="0" data-buyway="1" data-id="" data-title="${Project_name}" data-price="${Advise_price}"></div>  						
			</li>

            </script>

 <script type="text/x-jquery-tmpl" id="ProductItemHotel"> 
                

        <li class="cnt_box clearfix" url-id="">
        <a href="/h5/hotel/Hotelshow.aspx?projectid=${Id}" class="link js-goods clearfix" target="_blank" data-goods-id="" title=""> 
          <div class="cnt_img"><img src="${Imgaddress}" title="${Projectname}"></div>
          <div class="cnt_right">
            <p class="hotel_tit">${Projectname}</p>
            <div class="hotel_cnt1_box"><span class="grade">{{if grade !=0}}${grade}{{/if}}</span><span class="stop"></span>
              <p class="info"><span>¥</span><span class="info_c">${minprice}</span>起</p>
            </div>
            <div class="hotel_cnt2_box"><span class="star">${star}</span>
              <p class="tb"><span class="cu">${cu}</span><span class="li"></span></p>
            </div>
            <div class="hotel_cnt3_box"><span class="address">${Province} ${City}</span><span class="addnum"></span></div>
          </div>
          </a>
        </li>

    </script>
       
    <script type="text/x-jquery-tmpl" id="ProductItemShuttle"> 
                
        <li class="recommend_item " data-lineid="25">
		  <div class="item_left">
            <a title="${Project_name}" data-goods-id="" target="_blank" class="link js-goods clearfix" href="pro.aspx?id=${Id}">
			 <p class="recommend_station"> ${Pro_name}</p>
			 <p class="recommend_time">发车时间:${firststationtime}</p>
            </a>
		  </div>
		  <div class="item_right">
			  <div class="recommend_price"><a title="${Project_name}" data-goods-id="6427318" target="_blank" class="link js-goods clearfix" href="/h5/OrderEnter.aspx?id=${Id}&num=1&speciid=0"><span line-id="25" data-strategytag=""><small><i>￥</i>${Advise_price} 购票</small></span></a></div>
		  </div>
		</li> 

    </script>
            
            <script type="text/x-jquery-tmpl" id="WxItemp"> 
               <li class="goods-cardwx  normalwx"> 
                 <div class="custom-messages single">
                    <a href="/weixin/wxmaterialdetail.aspx?materialid= ${MaterialId}" class="clearfix">
                        <div class="custom-messages-image">
                                                <div class="image">
                                <img src="${Imgpath}" style="display: inline;" class="js-lazy" data-src="${Imgpath}">
                            </div>
                        </div>
                        <div class="custom-messages-content">
                            <h4 class="title">
                                ${Title}                    </h4>
                                                <div class="summary">
                                ${cutstr(Summary,80) }                 
                                 {{if Price!=0}}
                                    ¥${Price}<em>起</em>
                                {{/if}}
                        
                                 </div>
                    
                        </div>
                    </a>
                </div>
               </li>
            </script>


            <div id="zhegai" style="z-index: 1009;display: none;  position: absolute;  top: 0%;  left: 0%;  width: 100%;  height: 1000%;  background-color: black;  -moz-opacity: 0.9;  opacity:.90;  filter: alpha(opacity=90);"></div>      
    
   <div id="QJwxuFqolZ" class="sku-layout sku-box-shadow" style="overflow: hidden; visibility: visible; opacity: 1; bottom: 0px; left: 0px; right: 0px; transform: translate3d(0px, 0px, 0px); position: fixed; z-index: 1100; transition: all 300ms ease 0s; display:none;">
        <div class="layout-title sku-box-shadow name-card sku-name-card">
            <div class="thumb" id="buyimg"></div>
            <div class="detail goods-base-info clearfix">
                <p class="title c-black ellipsis" id="buytitle"></p>
                <div class="goods-price clearfix">
                    <div class="current-price c-black pull-left">
                        <span class="price-name pull-left font-size-14 c-orangef60">￥</span>
                        <i class="js-goods-price price font-size-18 c-orangef60 vertical-middle" id="buyprice">0</i>
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
				<input id="number" class="txt" value="1" type="number">            
				<button class="plus" type="button"></button>            
				<div class="response-area response-area-plus"></div>            
				<div class="txtCover"></div>        </div>
				<div class="stock pull-right font-size-12">
			
			</div>
		</dl>
		</dd></dl>
		
		<div style="display: none;" class="block-item block-item-messages"></div></div>
        <div class="confirm-action content-foot">
    <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark">下一步</a>
	</div>
    </div>
</div>

<div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>
        <div id="cartloading" class="loading" style="display:;bottom: 220px;">
            已成功添加到购物车
        </div>


<input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
    <input id="hid_action" type="hidden" value="1" />
    <input id="hid_proid" type="hidden" value="0" />
 <input id="pageindex" type="hidden" value="1" />


     <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=comlogo %>",
                desc: "<%=Scenic_intro %>",
                title: ' <%=title %>'
            });
        });
    </script>

</body></html>