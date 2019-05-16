<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hotellist.aspx.cs" Inherits="ETS2.WebApp.H5.Hotel.Hotellist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title><%=title %></title>
     <script type="text/javascript">
         var _tcopentime = new Date().getTime();
         var _hmt = _hmt || [];
    </script>
    <!-- meta信息，可维护 -->
    <meta charset="UTF-8" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta content="telephone=no" name="format-detection" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <!-- 页面样式表 -->    
    <link href="../../Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .none{ display:none;}
    </style>
    <!-- 页面样式表 -->
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="../../Styles/H5/scenery.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <meta name="description" content="北京景点门票，北京景点门票预订，北京旅游景点大全" />
    <meta name="keywords" content="是您旅行的好伙伴。" />
     <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
     <!-- 页面样式表 -->
    <style type="text/css">
        .none
        {
            display: none;
        }
        #loading
        {
        position:absolute;
        left:50%;top:60px;
        z-index:99;}
        #loading,#loading .lbk,#loading .lcont
        {
            width:146px;height:146px;}
        #loading .lbk,#loading .lcont
        {
            position:relative;}
        #loading .lbk
        {
            background-color:#000;
            opacity:.5;border-radius:10px;
            margin:-73px 0 0 -73px;z-index:0;}
       #loading .lcont{margin:-146px 0 0 -73px;
                       text-align:center;
                       color:#f5f5f5;
                       font-size:14px;
                       line-height:35px;z-index:1;}
      #loading img{width:35px;height:35px;margin:30px auto;display:block;}
    </style>
    <!-- 页面样式表 -->
    <link href="../../Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;
        $(function () {

            //            $("#list .list-item").clock(function () {
            //                $("#list").css("background", "#e9eff5");
            //            })

            var comid = $("#hid_comid").val();
            SearchList(pageindex, 16);
            $("#search_botton").click(function () {
                $("#loading").show();
                var key = $("#search_name").val();
                if (key == "") {
                    $("#loading").hide();
                    $("#search_name").css("border", "1px solid #FF0000");
                    return;
                }
                else {
                    $("#search_name").css("border", "0px solid #fff");
                }
                var pageSize = 16;
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=ComSelectpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#list").hide();
                            $("#black_top").hide();
                            $("#page").hide();
                            $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            $("#list").empty();
                            $("#divPage").empty();
                            if (data.totalCount - pageindex * pageSize > 0) {
                                $("#pagea").html("查看更多(" + (data.totalCount - pageindex * pageSize) + ")");
                            } else {
                                $("#pagea").html("查看更多(0)");
                            }
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                            setpage(data.totalCount, pageSize, pageindex);


                        }
                    }
                })

            })



            $("#pagea").click(function () {
                $("#loading").show();
                var pageSize = parseInt($("#num").val()) + 10;
                var key = $("#search_name").val();
                if (key == "") {
                    SearchList(pageindex, pageSize);
                    $("#num").val(pageSize);
                }
                else {
                    pageSize = parseInt($("#num").val()) + 10;
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/ProductHandler.ashx?oper=ComSelectpagelist",
                        data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $("#list").hide();
                                $("#black_top").hide();
                                $("#page").hide();
                                $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                                return;
                            }
                            if (data.type == 100) {
                                $("#loading").hide();
                                $("#list").empty();
                                $("#divPage").empty();
                                if (data.totalCount - pageindex * pageSize > 0) {
                                    $("#pagea").html("查看更多(" + (data.totalCount - pageindex * pageSize) + ")");
                                } else {
                                    $("#pagea").html("查看更多(0)");
                                }
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                setpage(data.totalCount, pageSize, pageindex);


                            }
                        }
                    })
                    $("#num").val(pageSize);
                }
            })

            //装载产品列表
            function SearchList(pageindex, pageSize) {
                if (pageindex == '') {
                    $("#list").html("点击查看更多");
                    return;
                }
                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Compagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#list").hide();
                            $("#black_top").hide();
                            $("#page").hide();
                            $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            $("#list").empty();
                            $("#divPage").empty();
                            if (data.totalCount - pageindex * pageSize > 0) {
                                $("#pagea").html("查看更多(" + (data.totalCount - pageindex * pageSize) + ")");
                            } else {
                                $("#pagea").html("查看更多(0)");
                            }
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                            setpage(data.totalCount, pageSize, pageindex);


                        }
                    }
                })
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

                    SearchList(page, newpagesize);

                    return false;
                }
            });
        }
        function SubstrDome(s, num) {
            var ss;
            if (s.length > num) {
                ss = s.substr(0, num) + "..";
                return (ss);
            }
            else {
                return (s);
            }

        }
        function csss(id) {
            location.href = "Hotelshow.aspx?id=" + id;
        }
        //截取金额
        function Subprice(price) {
            var arr = new Array();
            arr = price.split(".");
            price = arr[0].toString();
            return price;
        }
    </script>
</head>
<body>
    <div>
    <div id="loading" style="top: 150px; display: none;">
            <div class="lbk">
            </div>
            <div class="lcont">
                <img src=".../../Images/loading.gif" alt="loading..."/>正在加载...</div>
        </div>
     <!-- 公共页头  -->
    <header class="header">
                    <h1>在线预订</h1>
        <div class="left-head">
          <a id="goBack" href="#" class="tc_back head-btn"><span class="inset_shadow"><span class=""></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class=""><span class="inset_shadow"><span class=""></span></span></a>
        </div>
    </header>
    <!-- 页面内容块 -->
    <div id="page1">
        <div class="list-search">
            <dl class="fn-clear">
                <dt>
                    <input type="text" placeholder="输入酒店名或品牌" id="search_name"/>
                </dt>
                <dd>
                    <s  id="search_botton"></s>
                </dd>
            </dl>
        </div>
        <div id="list">
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
                 <div  class="list-item fn-clear" onclick="csss('${ID}');this.style.background = '#e9eff5';">
                        <div class="pic">
                            <img src="${Imprest}">
                        </div>
                        <div class="info">
                            <h5>${SubstrDome(Com_name,11)}</h5>
                            <div class="i-note">
                                <p style="color: #1a9ed9;margin-right:70px;"><span>
                                     ${SubstrDome(Cominfo,18)}
                                </span>
                                </p>
                            </div>
                        </div>
                        <div class="price">
                            ¥${Subprice(stata_price)}<em>起</em>
                        </div>
                        <span class="arrow-right"></span>
                    </div>
            </script>
        <div  id="page" class="pagelist" style="display: block;">
            <a class="pageList-more" id="pagea" href="javascript:;"></a></div>
        <div class="topdiv">
            <a id="black_top" href="#" style="display: inline;">返回顶部</a></div>
             <!-- 公共页脚  -->
      <%--<footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                  <span style="display:block; padding-bottom:5px;  line-height:20px;">服务热线：<a href="tel:400-8088-666">400-8088-666</a></span>
          </div>
      </footer>--%>
    </div>
   
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="num" type="hidden" value="10" />
    </div>
</body>
</html>
