<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="period.aspx.cs" Inherits="ETS2.WebApp.M.period" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=comname%></title>
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
    <link onerror="_cdnFallback(this)" href="/h5/order/css/css.css" rel="stylesheet"> 
    <link onerror="_cdnFallback(this)" href="/h5/order/css/css1.css" rel="stylesheet"> 
    <style type="text/css">
        .none
        {
            display: none;
        }
    </style>
    <!-- 页面样式表 -->
    <link href="../Styles/H5/scenery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>

    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;
        $(function () {

            var lastperiodcaurl = $("#hid_lastperiodcaurl").val();
            var nextperiodcaurl = $("#hid_nextperiodcaurl").val();
            if (lastperiodcaurl == "#") {
                $(".left-head").hide();
            } else {
                $(".left-head").show();
            }

            if (nextperiodcaurl == "#") {
                $(".right-head").hide();
            } else {
                $(".right-head").show();
            }

            var comid = $("#hid_comid").val();
            getmenubutton(comid, 'js-navmenu');

            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage btn btn-follow\">关注我们</a>");
                }
            });

            SearchList(pageindex, 16);




            //装载列表
            function SearchList(pageindex, pageSize) {
                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=periodicaltypelist",
                    data: { pageindex: pageindex, pagesize: pageSize, applystate: 1, promotetypeid: $("#hid_id").val(), type: $("#hid_type").val(), comid: comid },
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
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                            //setpage(data.totalCount, pageSize, pageindex);
                        }
                    }
                })
            }

            //下来加载上一期
            var stop = true;
            $(window).scroll(function () {

                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        // var pageindex = parseInt($("#pageindex").val()) + 1;

                        stop = false;
                        $("#loading").show();
                        $.ajax({
                            type: "post",
                            url: "/JsonFactory/WeiXinHandler.ashx?oper=periodicaltypelastlist",
                            data: { pageindex: 1, pagesize: 16, applystate: 10, promotetypeid: $("#hid_lastid").val(), type: $("#hid_type").val(), comid: comid },
                            async: false,
                            success: function (data) {
                                data = eval("(" + data + ")");
                                stop = true;
                                $("#loading").hide();
                                if (data.type == 1) {
                                    return;
                                }
                                if (data.type == 100) {
                                    $("#loading").hide();
                                    $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                    $("#hid_lastid").val(data.lastid);
                                }
                            }
                        })
                    }
                }
            });


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
    </script>
</head>
<body>
    <div>
<div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>
        <!-- 公共页头  -->
        <header class="header">
                    <div class="js-mp-info share-mp-info">
            <a href="/h5/order/default.aspx" class="page-mp-info">
                <img width="24" height="24" src="<%=comlogo %>" class="mp-image">
                <i class="mp-nickname">
                    <%=comname%> </i>
            </a>
            <div class="links "></div>
        </div>


    </header>
        <!-- 页面内容块 -->
        <div id="page1">
            <div id="list">
            </div>
            <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
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

            </script>

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
            </div> 
        </div>
        

        <input id="hid_comid" type="hidden" value="<%=comid %>" />
        <input id="hid_id" type="hidden" value="<%=id %>" />
        <input id="hid_lastid" type="hidden" value="<%=id %>" />
        <input id="hid_type" type="hidden" value="<%=type %>" />
        <input id="hid_over" type="hidden" value="<%=over %>" />
        <input id="hid_periodca" type="hidden" value="<%=periodca %>" />

        <input type="hidden" id="hid_lastperiodcaurl" value="<%=lastperiodcaurl %>" />
         <input type="hidden" id="hid_nextperiodcaurl" value="<%=nextperiodcaurl %>" />
    </div>
    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
        <script type="text/javascript">
            $(function () {
                //分享  
                $.ppkWeiShare({
                    path: location.href,
                    image: "<%=comlogo %>",
                    desc: "",
                    title: ' <%=comname %>'
                });
            });
    </script>
</body>
</html>
