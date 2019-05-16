<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Article.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.Article" %>
<html><head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">

<title id="js-meta-title">
<%=pro_name %> - <%=title%>
</title>
<!-- ▼ Common CSS -->

<link rel="stylesheet" href="/Styles/pc/pc_Bootstrap.css"/>
<link rel="stylesheet" href="/Styles/pc/pc_man.css"/>
<link rel="stylesheet" href="/Styles/pc/pc_swiper.css"/>
<link rel="stylesheet" href="/Styles/pc/Article.css"/>
<!-- ▲ Common CSS -->

<!-- ▼ App CSS -->
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script> 
<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
<script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
<script src="/Scripts/hoverdelay.js" type="text/javascript"></script>
<script src="/Scripts/common.js" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        var comid = $("#hid_comid").val();
        var id = $("#hid_id").val();
        var promotetypeid = $("#hid_promotetypeid").val();


        $(".js-hover").hoverDelay({
            hoverEvent: function () {
                $(".shop-info").removeClass("hide");
                $(".shop-info").removeClass("hidestate");
            },
            outEvent: function () {
                if ($(".shop-info").hasClass("hidestate")) {
                } else {
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
                    var qqstr = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        qqstr += '<tr><td>QQ：</td><td><a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=' + data.msg[i].QQ + '&site=qq&menu=yes"><img border="0" src="/images/qq.png" alt="' + data.msg[i].QQ + '" title="' + data.msg[i].QQ + '"></a></td></tr>';
                    }
                    $("#qqsevice").append(qqstr);

                }
            }
        })

        //加载右侧各分类第一篇文章
        $.ajax({
            type: "post",
            url: "/JsonFactory/WeiXinHandler.ashx?oper=TopArticleWxMaterialPageList",
            data: { comid: comid, pageindex: 1, pagesize: 10 },
            async: false,
            success: function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    return;
                }
                if (data.type == 100) {
                    $("#loading").hide();
                    var htmlstr = ' <h5 class="section-title"><span>相关文章</span></h5><ul class="js-async" >';

                    for (var i = 0; i < data.msg.length; i++) {
                        htmlstr += '<li class="js-log" >';
                        htmlstr += '    <a href="Article.aspx?id=' + data.msg[i].MaterialId + '" target="_blank">';
                        htmlstr += '      <div class="image" style="height: 110px;">';
                        htmlstr += '        <img src="' + data.msg[i].Imgpath + '" alt="' + data.msg[i].Title + '" height="110" width="160">';
                        htmlstr += '      </div>';
                        htmlstr += '      <div class="title">' + data.msg[i].Title + '</div>';
                        htmlstr += '      <div class="">' + ChangeDateFormat(data.msg[i].Operatime) + '</div>';
                        htmlstr += '    </a>';
                        htmlstr += '  </li>';
                    }
                    htmlstr += '</ul>';
                    $("#otherpro").html(htmlstr);
                }
            }
        })



        function readerarticle(pageindex) {
            $("#loading").show();
            //加载相关产品
            $.ajax({
                type: "post",
                url: "/JsonFactory/WeiXinHandler.ashx?oper=articlepagelist",
                data: { comid: comid, pageindex: pageindex, pagesize: 5, id: id, promotetypeid: promotetypeid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        if (id != 0) {
                            if (data.artmsg != null) {
                                $("#ProductItemEdit").tmpl(data.artmsg).appendTo(".content-body");
                            }
                        }
                        if (data.totalCount == 0) {
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo(".content-body");
                            $("#pageindex").val(pageindex)
                        }
                    }
                }
            })
        }


        readerarticle(1);


        var stop = true;
        $(window).scroll(function () {

            totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
            if ($(document).height() <= totalheight) {
                if (stop == true) {
                    var pageindex = parseInt($("#pageindex").val()) + 1;
                    stop = false;
                    readerarticle(pageindex);
                    stop = true;
                }
            }
        });
    });

</script>
</head>
<body class="theme theme--blue" style="">

<!-- ▼ Main header -->
<header class="ui-header">
    <div class="ui-header-inner clearfix">
        <div class="ui-header-logo">
            <a href="javascript:;" class="js-hover logo" data-target="js-shop-info">
        <%=title%>              <span class="smaller-title hide"><%=Scenic_name %></span>
      </a>
        </div>
        <nav class="ui-header-nav">
            <ul class="clearfix">
                             
    <li><a href="ProductList.aspx">首页</a></li>
	 <li class="divide">|</li>
    <li ><a href="PJList.aspx">全部产品</a></li>
     <li class="divide">|</li>
    <li ><a href="Article.aspx">全部文章</a></li>
      <%if (comid==1194||comid==106) {%>
      <li class="divide">|</li>
    <li><a href="http://yd.wlski.com/SellOnline/sellIndex.aspx?UType=3&UCode=etowncn&Upass=24611B10993D318F" target="_blank">万龙滑雪票预订</a>
    <% } %>
               </ul>
        </nav>
    </div>
</header>
<!-- ▲ Main header -->

<!-- ▼ Main container -->
<div id="Maintop"></div>
  <div class="container goods-detail-main clearfix">
    <div class="content ">

    <div class=" content-body ">



    </div>

    </div>

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
              <td><%=Tel %></td>
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

  <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
        <!-- 分类 -->
        <div>
            <div class="custom-title  text-left">
              
                        <p class="sub_title">${ChangeDateFormat(Operatime)} </p>
                    </div>
        </div>

        <!-- 标题 -->
        <div class="custom-title-noline" style="background-color: #ebebeb;">
            <div class="custom-title  text-left">
                <h2 class="title">
                   
                    ${Title}                    </h2>
                        <p class="sub_title">${Summary}</p>
                    </div>
        </div>
            
        <!-- 图片广告
        <div class="custom-image-swiper custom-image-swiper-single js-swp swp">
            <div class="swiper-wrapper js-swp-wrap js-view-image-list" style="height: 254px;">
               <a class="swp-page" href="javascript:void(0);">
                   <img class="js-view-image-item" src="${Imgpath}">
               </a>
            </div>
            <div class="swiper-pagination js-swiper-pagination"></div>
        </div> -->
    
        <div class="custom-richtext js-view-image-list">
	        <blockquote>{{html Article}}</blockquote>
        </div>
        <div class="custom-line-wrap">
            <hr class="custom-line">
        </div>      
    
    </script>

<input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
    <input id="hid_id" type="hidden" value="<%=id %>" />
    <input id="hid_promotetypeid" type="hidden" value="<%=promotetypeid %>" />
    <input id="pageindex" type="hidden" value="1" />
    <div class="footer">
        <div class="container">
          <a href="javascript:;"><%=Copyright %></a>
        </div>
    </div>
   <div id="loading" class="loading" style="display: none;">
            正在加载...
   </div>
</body></html>