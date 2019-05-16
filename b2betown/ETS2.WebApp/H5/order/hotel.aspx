<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hotel.aspx.cs" Inherits="ETS2.WebApp.H5.order.hotel" %>

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
    <title>
        <%=title_view %>
        -
        <%=title %></title>
    <!-- meta viewport -->
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"
        name="viewport">
    <!-- CSS -->
    <link onerror="_cdnFallback(this)" href="css/css1.css" rel="stylesheet">
    <link onerror="_cdnFallback(this)" href="css/css.css" rel="stylesheet">
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?type=quick&ak=mKeOlGW2zgs8LAVf7ihHzSTD&v=1.0"></script>
    <script type="text/javascript">
        var pageindex = 1;
        var pageSize = 16;
        $(function () {


            var comid = $("#hid_comid").val();

            var projectid = $("#hid_projectid").val();

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


            //查询购物车数量
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentsearchcart", { userid: $("#hid_userid").val(), comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    $("#right-icon").removeClass("hide");
                }
            })

            SearchList(pageindex, 16);

            var stop = true;
            $(window).scroll(function () {

                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#pageindex").val()) + 1;

                        stop = false;
                        $("#loading").show();
                        $.ajax({
                            type: "post",
                            url: "/JsonFactory/ProductHandler.ashx?oper=webprojectpagelist",
                            data: { comid: comid, pageindex: pageindex, pagesize: pageSize, projectid: projectid, key: $("#key").val(), Servertype:9,projectstate:1 },
                            async: false,
                            success: function (data) {
                                data = eval("(" + data + ")");
                                stop = true;
                                $("#loading").hide();
                                if (data.type == 1) {
                                    //$("#line-list").hide();
                                    return;
                                }
                                if (data.type == 100) {
                                    $("#loading").hide();
                                    $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                    $("#pageindex").val(pageindex);
                                }
                            }
                        })
                    }
                }
            });

            //装载产品列表
            function SearchList(pageindex, pageSize) {

                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=webprojectpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, projectid: projectid, key: $("#key").val(), Servertype: 9, projectstate: 1 },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#list").hide();
                            $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            $("#list").empty();
                            $("#divPage").empty();
                            if (data.totalCount - pageindex * pageSize > 0) {
                                $("#pagea").html("还有(" + (data.totalCount - pageindex * pageSize) + ")");
                            } else {
                                $("#pagea").html("");
                            }
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                            $("#pageindex").val(pageindex);

                        }
                    }
                })
            }



            //立即购买，或加入购物车
            $(".js-confirm-it").click(function () {

                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val();
                var action = $("#hid_action").val();

                var issetfinancepaytype = false;
                if ('<%=issetfinancepaytype %>' == 'True' || '<%=issetfinancepaytype %>' == 'true') {
                    issetfinancepaytype = true;
                }
                if (action == "1") {//直接订购
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
                } else {//加入购物车
                    $("#QJwxuFqolZ").hide();
                    $("#zhegai").hide();
                    $('#cartloading').show(); ;
                    $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { userid: $("#hid_userid").val(), comid: comid, proid: proid, u_num: num }, function (data) {
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

        //产品id，名称，图片，action=1为直接购买，0为购物车
        function suborder(proid, title, img, price, action) {

            $("#hid_proid").val(proid);

            $("#hid_proid").val(proid);
            $("#buytitle").html(title);
            $("#buyprice").html(price);
            $("#buyimg").html('<img src="' + img + '" alt="">');

            if (action == 11) {
                $("#hid_action").val(0);
                $(".js-confirm-it").html("加入购物车");
                $("#QJwxuFqolZ").show();
            }
            else if (action == 9) {
                location.href = "/h5/hotel/Hotelshow.aspx?proid=" + proid;
                return;
            }
            else {
                location.href = "/h5/order/pro.aspx?id=" + proid;
                return;
                //                $("#hid_action").val(1);
                //                $(".js-confirm-it").html("下一步");
                //                $("#QJwxuFqolZ").show();
            }

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
    <link href="css/bottommenu.css" rel="stylesheet">
</head>
<body class=" ">
    <!-- container -->
    <div class="container ">
        <div class="header">
            <!-- ▼顶部通栏 -->
            <div class="js-mp-info share-mp-info">
                <a href="default.aspx" class="page-mp-info">
                    <img width="24" height="24" src="<%=logoimg%>" class="mp-image">
                    <i class="mp-nickname">
                        <%=title %>
                    </i></a>
                <div class="links">
                </div>
            </div>
            <!-- ▲顶部通栏 -->
        </div>
        <div class="content ">
            <div class="content-body">
                <div class="custom-search  hide">
                    <form action="list.aspx" method="GET">
                    <input id="key" class="custom-search-input" placeholder="商品搜索：请输入商品关键字" name="q"
                        value="<%=key %>" type="search">
                    <button type="submit" class="custom-search-button">
                        搜索</button>
                    </form>
                </div>
                <!-- 标题 -->
                <div>
                    <div class="custom-title  text-left">
                        <h2 class="title">
                            <%=title_view %>
                        </h2>
                        <p class="sub_title">
                        </p>
                    </div>
                </div>
                <%if (unyuding != 0)
                  { %>
                <%if (Coordinate != "")
                  { %>
                <div id="mapdaohang" class="custom-store">
                    <a class="custom-store-link clearfix" href="javascript:;" onclick="DRIVINGdaohang();">
                        <div class="custom-store-img">
                            <img class="mp-image" src="/images/red_marker.jpg" width="18"></div>
                        <div class="custom-store-name">
                            地址：<%=Address%></div>
                        <span class="custom-store-enter">进入导航 </span></a>
                </div>
                <div id="allmap" class="" style="width: 100%; height: 200px;">
                </div>
                <script type="text/javascript">
        // 百度地图API功能
        var map = new BMap.Map("allmap");
        map.centerAndZoom(new BMap.Point(<%=Coordinate %>), 13);

        map.addControl(new BMap.ZoomControl());  //添加地图缩放控件
        var marker1 = new BMap.Marker(new BMap.Point(<%=Coordinate %>));  //创建标注
        map.addOverlay(marker1);                 // 将标注添加到地图中
       


        //创建信息窗口
        var infoWindow1 = new BMap.InfoWindow("<%=title_view %> ");
        marker1.addEventListener("click", function () { this.openInfoWindow(infoWindow1); });

        function DRIVINGdaohang() {
            var start = {
                name: ""
            }
            var end = {
                name: "怀柔", latlng: new BMap.Point(116.699047, 40.666883)
            }
            var opts = {
                mode: BMAP_MODE_DRIVING,
                region: ""
            }
            var ss = new BMap.RouteSearch();
            ss.routeCall(start, end, opts);
        }

                </script>
                <%} %>
                <div class="custom-richtext">
                    <%=Serviceintroduce %>
                </div>
                <%} %>
                <!-- 商品区域 -->
                <!-- 展现类型判断 -->
                <ul id="list" class="recommend_box">
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
                        <p class="shop-detail">
                            <%=Scenic_intro %></p>
                        <p class="text-center weixin-title">
                            微信“扫一扫”立即关注</p>
                        <div class="js-follow-qrcode text-center qr-code">
                            <img width="158" height="158" src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>">
                        </div>
                        <p class="text-center weixin-no">
                            微信号：<%=weixinname %></p>
                    </div>
                </div>
            </div>
        </div>
        <div style="min-height: 1px;" class="js-footer">
            <div class="footer">
                <div class="copyright">
                    <div class="ft-links">
                        <span id="copydaohang"></span><span class="links"></span>
                    </div>
                    <div class="ft-copyright">
                        <a href="#">易城商户平台技术支持</a>
                    </div>
                </div>
            </div>
        </div>
        <!-- fuck taobao -->
        <div class="fullscreen-guide fuck-taobao hide" id="js-fuck-taobao">
            <span class="js-close-taobao guide-close">×</span> <span class="guide-arrow"></span>
            <div class="guide-inner">
                <div class="step step-1">
                </div>
                <div class="js-step-2 step">
                </div>
            </div>
        </div>
    </div>
    <!-- JS -->
    <div class="motify">
        <div class="motify-inner">
        </div>
    </div>
    <div id="right-icon" class="icon-hide   no-border hide" data-count="1">
        <div class="right-icon-container clearfix" style="width: 50px">
            <a id="global-cart" href="Cart.aspx" class="no-text" style="background-image: url(image/s0.png);
                background-size: 50px 50px; background-position: center;">
                <p class="right-icon-img">
                </p>
                <p class="right-icon-txt">
                    购物车</p>
            </a>
        </div>
    </div>
    <!-- 底部菜单 -->
    <div class="js-navmenu js-footer-auto-ele shop-nav nav-menu nav-menu-1 has-menu-3">
        <div class="nav-special-item nav-item">
            <a href="/h5/order/Default.aspx" class="home">主页</a>
        </div>
        <div class="nav-item">
            <a class="mainmenu js-mainmenu" href="/m/indexcard.aspx"><span class="mainmenu-txt">
                会员中心</span> </a>
            <!-- 子菜单 
                            </div>
                            <div class="nav-item">
                <a class="mainmenu js-mainmenu" href="wo ">
                                        <span class="mainmenu-txt">往期回顾</span>
                </a> -->
            <!-- 子菜单 -->
        </div>
        <div class="nav-item">
            <a class="mainmenu js-mainmenu" href="/h5/order/Order.aspx"><span class="mainmenu-txt">
                我的订单</span> </a>
            <!-- 子菜单 -->
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
                

        <li class="cnt_box clearfix" url-id="">
        <a href="/h5/hotel/Hotelshow.aspx?projectid=${Id}&amp;id=" class="link js-goods clearfix" target="_blank" data-goods-id="" title=""> 
          <div class="cnt_img"><img src="${Projectimgurl}" title="${Projectname}"></div>
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
    <div id="zhegai" style="z-index: 1009; position: absolute; left: 0; top: 0; bottom: 0;
        width: 100%; height: 1000%; filter: alpha(Opacity=80); -moz-opacity: 0.9; opacity: 0.9;
        display: none; background: #000000;">
    </div>
    <div id="QJwxuFqolZ" class="sku-layout sku-box-shadow" style="overflow: hidden; visibility: visible;
        opacity: 1; bottom: 0px; left: 0px; right: 0px; transform: translate3d(0px, 0px, 0px);
        position: fixed; z-index: 1100; transition: all 300ms ease 0s; display: none;">
        <div class="layout-title sku-box-shadow name-card sku-name-card">
            <div class="thumb" id="buyimg">
            </div>
            <div class="detail goods-base-info clearfix">
                <p class="title c-black ellipsis" id="buytitle">
                </p>
                <div class="goods-price clearfix">
                    <div class="current-price c-black pull-left">
                        <span class="price-name pull-left font-size-14 c-orangef60">￥</span> <i class="js-goods-price price font-size-18 c-orangef60 vertical-middle"
                            id="buyprice">0</i>
                    </div>
                </div>
            </div>
            <div class="js-cancel sku-cancel">
                <div class="cancel-img">
                </div>
            </div>
        </div>
        <div style="height: 123px;" class="adv-opts layout-content">
            <div class="goods-models js-sku-views block block-list block-border-top-none">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            数量</label>
                    </dt>
                    <dd>
                        <dl class="clearfix">
                            <div class="quantity">
                                <div class="response-area response-area-minus">
                                </div>
                                <button disabled="disabled" class="minus " type="button">
                                </button>
                                <input id="number" class="txt" value="1" type="number">
                                <button class="plus" type="button">
                                </button>
                                <div class="response-area response-area-plus">
                                </div>
                                <div class="txtCover">
                                </div>
                            </div>
                            <div class="stock pull-right font-size-12">
                            </div>
                        </dl>
                    </dd>
                </dl>
                <div style="display: none;" class="block-item block-item-messages">
                </div>
            </div>
            <div class="confirm-action content-foot">
                <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark">下一步</a>
            </div>
        </div>
    </div>
    <div id="loading" class="loading" style="display: none;">
        正在加载...
    </div>
    <div id="cartloading" class="loading" style="display: ; bottom: 220px;">
        已成功添加到购物车
    </div>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_projectid" type="hidden" value="<%=projectid %>" />
    <input id="hid_action" type="hidden" value="1" />
    <input id="hid_proid" type="hidden" value="0" />
    <input id="pageindex" type="hidden" value="1" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
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
</body>
</html>
