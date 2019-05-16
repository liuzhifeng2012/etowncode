<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ETS2.WebApp.H5.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=title %></title>
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
    <link href="../Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .none
        {
            display: none;
        }
    </style>
    <!-- 页面样式表 -->
    <link href="../Styles/H5/scenery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/H5/mh5pro.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/Order.js" type="text/javascript"></script>
    <!-- 页面样式表 -->
    <style type="text/css">
        .none
        {
            display: none;
        }
    </style>
    <!-- 页面样式表 -->
    <link href="../Styles/H5/list.css" rel="stylesheet" type="text/css" />
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
                    url: "/JsonFactory/ProductHandler.ashx?oper=ProjectSelectpagelist",
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
                                $("#pagea").html("还有(" + (data.totalCount - pageindex * pageSize) + ")");
                            } else {
                                $("#pagea").html("");
                            }
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                            //setpage(data.totalCount, pageSize, pageindex);
                            $("#pageindex").val(pageindex);

                        }
                    }
                })

            })


            var stop = true;


            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#pageindex").val()) + 1;
                        var pageSize = parseInt($("#num").val()) + 10;
                        var key = $("#search_name").val();
                        var proclass = "<%=proclass%>";
                        var price = "<%=price%>";


                        stop = false;
                        $("#loading").show();
                        $.ajax({
                            type: "post",
                            url: "/JsonFactory/ProductHandler.ashx?oper=ProjectSelectpagelist",
                            data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, proclass: proclass, price: price },
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
                                    //$("#line-list").empty();
                                    $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                    $("#pageindex").val(pageindex);

                                }
                            }
                        })


                    }
                }
            });


            $("#confirm").click(function () {
                $("#loading").show();
                $("#page1").show();
                $("#selectTypePage").hide();
                var key = $("#f-level-val").val();
                var theme = $("#f-theme-val").val();
                var price = $("#f-price-val").val();

                location.href = "/h5/List.aspx?key=" + key + "&class=" + theme + "&price=" + price + "&buyuid=<%=uid %>&tocomid=<%=comid %>&uid=<%=uid %>";

            })



            $("#pagea").click(function () {
                $("#loading").show();
                var pageSize = parseInt($("#num").val()) + 10;
                var key = $("#search_name").val();
                var proclass = "<%=proclass%>";
                var price = "<%=price%>";
                if (key == "") {
                    SearchList(pageindex, pageSize);
                    $("#num").val(pageSize);
                }
                else {
                    pageSize = parseInt($("#num").val()) + 10;
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/ProductHandler.ashx?oper=ProjectSelectpagelist",
                        data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, proclass: proclass, price: price },
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
                                    $("#pagea").html("还有(" + (data.totalCount - pageindex * pageSize) + ")");
                                } else {
                                    $("#pagea").html("");
                                }
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                //setpage(data.totalCount, pageSize, pageindex);
                                $("#pageindex").val(pageindex);


                            }
                        }
                    })
                    $("#num").val(pageSize);
                }
            })

            //装载产品列表
            function SearchList(pageindex, pageSize) {

                var proclass = "<%=proclass%>";
                var key = "<%=key %>";
                var price = "<%=price %>";

                if (pageindex == '') {
                    $("#list").html("点击查看更多");
                    return;
                }
                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=ProjectSelectpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, proclass: proclass, price: price },
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
                                $("#pagea").html("还有(" + (data.totalCount - pageindex * pageSize) + ")");
                            } else {
                                $("#pagea").html("");
                            }
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                            //setpage(data.totalCount, pageSize, pageindex);
                            $("#pageindex").val(pageindex);

                        }
                    }
                })
            }


            $("#shaixuan").click(function () {
                $("#page1").hide();
                $("#selectTypePage").show();

            })

            $("#f-level").click(function () {
                if ($("#f-level-ul").is(":hidden") == true) {
                    $("#f-level-ul").show();
                } else {
                    $("#f-level-ul").hide();
                }
            })

            $("#f-level-ul li").click(function () {
                $("#f-level-val").val($(this).attr("data-id"));
                $("#f-level-viw").text($(this).attr("data-type-name"));

                $(this).parent.hide();


            })



            $("#f-theme").click(function () {
                if ($("#f-theme-ul").is(":hidden") == true) {
                    $("#f-theme-ul").show();
                } else {
                    $("#f-theme-ul").hide();
                }
            })

            $("#f-theme-ul li").click(function () {
                $("#f-theme-val").val($(this).attr("data-id"));
                $("#f-theme-viw").text($(this).attr("data-type-name"));
                $(this).parent.hide();
            })

            $("#f-price").click(function () {
                if ($("#f-price-ul").is(":hidden") == true) {
                    $("#f-price-ul").show();
                } else {
                    $("#f-price-ul").hide();
                }
            })

            $("#f-price-ul li").click(function () {
                $("#f-price-val").val($(this).attr("data-id"));
                $("#f-price-viw").text($(this).attr("data-type-name"));
                $(this).parent.hide();
            })

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
            location.href = "/h5/Orderlist.aspx?proclass=<%=proclass %>&id=" + id + "&buyuid=<%=buyuid %>&tocomid=<%=tocomid %>&uid=<%=uid %>";
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
        <div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>
        <!-- 公共页头  -->
        <header class="header">
                    <h1><%=biaoti %></h1>
        <div class="left-head">
        <%if (comid == 106)
          {
              if (uid > 0)
              {
              %>
          <a href="prodefault.aspx?buyuid=<%=buyuid%>&tocomid=<%=tocomid %>" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>       
              <%}
              
          } %>
        </div>
        <div class="right-head">

         <%if (Wxfocus_url != "")
           { %>
                <%if (comid != 106 || tocomid == 0 || tocomid == 106)
                  { %>
                <a href="<%=Wxfocus_url %>" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;"><%=Wxfocus_author%></span></span></a>
         <%}
           } %>



        </div>
    </header>
        <!-- 页面内容块 -->
        <div id="page1">
            <%if (comid == 106 && comid == 0)
              { %>
            <div class="order-by" data-id="0">
                <ul class="fn-clear">
                    <li data-id="0" class="act">推荐</li>
                    <li id="shaixuan">筛选</li>
                </ul>
            </div>
            <%}%>
            <div class="list-search">
                <dl class="fn-clear">
                    <dt>
                        <input type="text" id="search_name" />
                    </dt>
                    <dd>
                        <s id="search_botton"></s>
                    </dd>
                </dl>
            </div>
            <div id="list">
            </div>
            <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
                 <div  class="list-item fn-clear" onclick="csss('${ID}');this.style.background = '#e9eff5';">
                        <div class="pic">
                            <img src="${Project_img}">
                        </div>
                        <div class="info">
                            <h5>${SubstrDome(Project_name,11)}   </h5>
                            <div class="i-note">
                                <p style="color: #1a9ed9;margin-right:70px;"><span>
                                     ${SubstrDome(BriefIntroduce,18)}
                                </span>
                                </p>
                            </div>
                        </div>
                        <div class="price">
                            {{if Lowerprice=="0"||Lowerprice==""}}
                            {{else}}
                               ¥${Subprice(Lowerprice)}<em>起</em>
                            {{/if}}
                        </div>
                        <div style="position: absolute; right: 5px; top: 60px; font-size: 12px;"> ${ViewDistance(Distance)} </div>
                        <span class="arrow-right"></span>
                    </div>
            </script>
            <div id="page" class="pagelist" style="display: block;">
                <a class="pageList-more" id="pagea" href="javascript:;"></a>
            </div>
            <div class="topdiv">
                <a id="black_top" href="#" style="display: inline;">返回顶部</a></div>
            <!-- 公共页脚  -->
            <%--<footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                  <span style="display:block; padding-bottom:5px;  line-height:20px;">服务热线：<a href="tel:400-8088-666">400-8088-666</a></span>
          </div>
      </footer>--%>
        </div>
        <%if (comid == 106)
          { %>
        <!-- 筛选 -->
        <div style="display: none;" id="selectTypePage" class="page current">
            <div class="page-header">
                <a href="javascript:void(0);" class="page-back touchable"></a>
                <h2>
                    景点筛选</h2>
                <!-- 需要头部菜单加上下面内容 -->
                <a href="javascript:void(0);" class="header-menu-btn touchable"><i></i></a>
            </div>
            <div style="transform-origin: 0px 0px 0px; opacity: 1; transform: scale(1, 1);">
                <div class="filter" id="f-level" data-id="0">
                    <div class="f-title">
                        <h6>
                            按景点位置</h6>
                        <span id="f-level-viw">不限</span> <em class="icon-right"></em>
                    </div>
                    <ul id="f-level-ul">
                        <li class="act" data-id="0" data-type-name="不限">不限<s></s></li>
                        <li data-id="北京市" data-type-name="北京">北京</li>
                        <li data-id="河北省" data-type-name="河北">河北</li>
                        <li data-id="山东省" data-type-name="山东">山东</li>
                        <li data-id="山西省" data-type-name="山西">山西</li>
                        <li data-id="江苏省" data-type-name="江苏">江苏</li>
                        <li data-id="福建省" data-type-name="福建">福建</li>
                        <li data-id="天津市" data-type-name="天津">天津</li>
                        <li data-id="广西壮族" data-type-name="广西">广西</li>
                        <li data-id="湖北省" data-type-name="湖北">湖北</li>
                        <li data-id="内蒙古自治区" data-type-name="内蒙古">内蒙古</li>
                        <li data-id="浙江省" data-type-name="浙江">浙江</li>
                        <li data-id="上海市" data-type-name="上海">上海</li>
                        <li data-id="海南省" data-type-name="海南">海南</li>
                    </ul>
                </div>
                <div class="filter" id="f-theme" data-id="0">
                    <div class="f-title">
                        <h6>
                            按旅游主题</h6>
                        <span id="f-theme-viw">不限</span> <em class="icon-right"></em>
                    </div>
                    <ul id="f-theme-ul">
                        <li class="act" data-id="0" data-type-name="不限">不限<s></s></li>
                        <li data-id="4" data-type-name="主题乐园">主题乐园</li>
                        <li data-id="6" data-type-name="景点门票">景点门票</li>
                        <li data-id="2" data-type-name="温泉洗浴">温泉洗浴</li>
                        <li data-id="5" data-type-name="休闲运动">休闲运动</li>
                        <li data-id="11" data-type-name="戏水漂流">戏水漂流</li>
                        <li data-id="12" data-type-name="采摘烧烤">采摘烧烤</li>
                        <li data-id="8" data-type-name="度假酒店">度假酒店</li>
                        <li data-id="14" data-type-name="功夫杂技">功夫杂技</li>
                        <li data-id="16" data-type-name="博物馆与名人故居">博物馆与名人故居</li>
                        <li data-id="15" data-type-name="电影">电影</li>
                    </ul>
                </div>
                <div class="filter" id="f-price" data-id="0">
                    <div class="f-title">
                        <h6>
                            按价格</h6>
                        <span id="f-price-viw">不限</span> <em class="icon-right"></em>
                    </div>
                    <ul id="f-price-ul">
                        <li class="act" data-id="0">不限<s></s></li>
                        <li data-id="1" data-type-name="¥1-¥50">¥1-¥50</li>
                        <li data-id="2" data-type-name="¥50-¥100">¥50-¥100</li>
                        <li data-id="3" data-type-name="¥100以上">¥100以上</li>
                    </ul>
                </div>
                <div class="confirm" id="confirm">
                    确定</div>
            </div>
        </div>
        <%} %>
        <input id="hid_comid" type="hidden" value="<%=comid %>" />
        <input id="num" type="hidden" value="10" />
        <input id="f-level-val" type="hidden" value="" />
        <input id="f-theme-val" type="hidden" value="0" />
        <input id="f-price-val" type="hidden" value="0" />
        <input id="pageindex" type="hidden" value="1" />
    </div>
</body>
</html>
