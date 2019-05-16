<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="store.aspx.cs" Inherits="ETS2.WebApp.M.store" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8"></meta>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"
        name="viewport"></meta>
    <meta content="telephone=no" name="format-detection"></meta>
    <title>
        <%=name%></title>
    <style>
        abbr, article, aside, audio, canvas, datalist, details, dialog, eventsource, figure, figcaption, footer, header, hgroup, mark, menu, meter, nav, output, progress, section, small, time, video, legend
        {
            display: block;
        }
        
    </style>
    <link href="../Styles/weixin/weiweb.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;
        var pagesize = 16;

        $(function () {
            var comid = $("#hid_comid").val();
            $("#hid_pageindex").val("1");

            $("#list").empty();
            SearchList(pageindex, pagesize, "");

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
                $("#list").empty();
                SearchList(pageindex, pagesize, key);
                $("#hid_pageindex").val("1");
            })

            $("#pagea").click(function () {
                $("#loading").show();
                var pageindex = parseInt($("#hid_pageindex").val()) + 1;
                var key = $("#search_name").val();
                SearchList(pageindex, pagesize, key);
                $("#hid_pageindex").val(pageindex);
            })
        });
        //装载产品列表
        function SearchList(pageindex, pagesize, key) {

            $("#loading").show();
            $.ajax({
                type: "post",
                url: "/JsonFactory/ChannelHandler.ashx?oper=Channelcompanypagelist",
                data: { channelcompanyid: $("#hid_channelcompanyid").val(), comid: $("#hid_comid").val(), pageindex: pageindex, pagesize: pagesize, key: key },
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
                        //                        $("#list").empty();
                        $("#divPage").empty();
                        if (data.totalcount - pageindex * pagesize > 0) {
                            $("#pagea").html("查看更多(" + (data.totalcount - pageindex * pagesize) + ")");
                        } else {
                            $("#pagea").html("查看更多(0)");
                        }

                        var ht = "";
                        for (var i = 0; i < data.msg.length; i++) {
                            ht += "<ul class=\"round\">" +
                                      "<li class=\"com noArrow\"  onclick=\"csss('" + data.msg[i].Id + "');this.style.background = '#e9eff5';\"><span>" +
                                            "" + data.msg[i].Companyname + " </span></li>" +
                                      "<li class=\"addr noArrow\"><span>" +
                                            "地址：" + data.msg[i].Companyaddress + " </span></li>" +
                                      "<li class=\"tel\">" +
                                      "  <a href=\"tel:" + data.msg[i].Companyphone + " \">电话: " + data.msg[i].Companyphone + "</a>" +
                                      "</li>" +
                                  "</ul>" +
                                  "<div id=\"roundjiange\"> </div>";

                        }
                        $("#list").append(ht);
                    }
                }
            })
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
            location.href = "/H5/StoreDetail.aspx?menshiid=" + id;
        }
   
    </script>
</head>
<body id="page_intro" class="">
    <div id="mappContainer">
        <div class="footFix" id="topCity" data-ffix-top="0">
            <div class="inner">
                <span class="cityname">
                    <%=name%></span><%--<a class="toggle" href="javascript:void(0)"></a>--%></div>
        </div>
        <div class="inner root" style="height: auto; margin-top: 40px;">
            <h2>
            </h2>
            <div id="list">

            </div>
        </div>
        <div class="footFix" id="footReturn">
            <a href="indexcard.aspx"><span>返回会员卡首页</span></a>
        </div>
    </div>
    <script>
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            WeixinJSBridge.call('hideToolbar');
        });
    </script>
    <input id="hid_comid" type="hidden" value="<%=materialid %>" />
</body>
</html>
