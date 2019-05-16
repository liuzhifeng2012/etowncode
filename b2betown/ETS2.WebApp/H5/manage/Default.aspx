<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ETS2.WebApp.H5.manage.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
<meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />

<title>微网站</title>
<link rel="stylesheet" type="text/css" href="/Styles/H5/mh5.css" />
<link href="/Styles/H5/model-tongyong.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/Scripts/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="/Scripts/swipe.js"></script>
<script type="text/javascript">
    var pageSize = 10; //只显示条数

    $(function () {
        var comid = $("#hid_comid").val();
        var bannerlist = "";
        var daohanglist = "";
        SearchList(1);

        //装载
        function SearchList(pageindex) {
            //Banner列表
            $.ajax({
                type: "post",
                url: "/JsonFactory/DirectSellHandler.ashx?oper=pagegetimagelist",
                data: { comid: comid, pageindex: pageindex, pagesize: pageSize, typeid: 0 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.totalCount == 0) {

                        }
                        else {
                            if (data.totalCount == 1) {
                                bannerlist = "<ul style=\"list-style: none outside none; width: 3200px; transition-duration: 500ms; transform: translate3d(0px, 0px, 0px);\">";
                            } else {
                                bannerlist = "<ul style=\"list-style: none outside none; width: 3200px; transition-duration: 500ms; transform: translate3d(-640px, 0px, 0px);\">";
                            }
                            for (i = 0; i < data.totalCount; i++) {
                                bannerlist = bannerlist + " <li style=\"width: 640px; display: table-cell; vertical-align: top;\"><a href=\"/ui/shangjiaui/H5Setlist.aspx\" target=\"_parent\"><img src=\"" + data.msg[i].Imgurl_address + "\" alt=\"" + data.msg[i].Title + "\"  style=\"width:100%;\"  /></a></li>"
                            }
                            bannerlist = bannerlist + " </ul>";

                            bannerlist = bannerlist + "<ol>"
                            for (i = 1; i <= data.totalCount; i++) {
                                if (i == 1) {
                                    bannerlist = bannerlist + " <li class=\"on\"> </li>"
                                } else {
                                    bannerlist = bannerlist + " <li class=\"\"> </li>"
                                }
                            }
                            bannerlist = bannerlist + "</ol>"

                            $("#banner_box").html(bannerlist);
                            //$("#slider").addClass("slider");
                        }

                    }
                }
            })


            //加载导航 ;
            $.ajax({
                type: "post",
                url: "/JsonFactory/DirectSellHandler.ashx?oper=getmenulist",
                data: { comid: comid, pageindex: pageindex, pagesize: 7, typeid: 0 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("查询错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.totalCount == 0) {
                        }
                        else {

                            daohanglist = "<div class=\"col-box\">"
                            var left_daohang = 3;
                            var ritht_daohang = 4;
                            if (data.totalCount > left_daohang) {
                                for (i = 0; i < left_daohang; i++) {
                                    daohanglist = daohanglist + "<a href=\"/ui/shangjiaui/H5SetMenu_manage.aspx?id=" + data.msg[i].Id + "\" title=\"编辑此栏目\" target=\"_parent\"  id=\"menu" + data.msg[i].Id + "\" onmouseover=\"addmengban('menu" + data.msg[i].Id + "')\" onmouseout=\"remengban('menu" + data.msg[i].Id + "')\"><img alt=\"" + data.msg[i].Name + "\" src=\"" + data.msg[i].Imgurl_address + "\" /></a>"
                                }
                            }
                            daohanglist = daohanglist + "</div><div class=\"col-box\">"
                            if (data.totalCount > left_daohang) {
                                for (i = 3; i < data.totalCount; i++) {
                                    daohanglist = daohanglist + "<a href=\"/ui/shangjiaui/H5SetMenu_manage.aspx?id=" + data.msg[i].Id + "\"  title=\"编辑此栏目\" target=\"_parent\" id=\"menu" + data.msg[i].Id + "\" onmouseover=\"addmengban('menu" + data.msg[i].Id + "')\" onmouseout=\"remengban('menu" + data.msg[i].Id + "')\"><img alt=\"" + data.msg[i].Name + "\" src=\"" + data.msg[i].Imgurl_address + "\" /></a>"
                                }
                            }
                            daohanglist = daohanglist + "</div>"
                            $(".menu-wrap").html(daohanglist);
                        }
                    }
                }
            })


        }


    })
    var Hx = 10;
    var Hy = 20;
    function addmengban(object) {
        $("#" + object).addClass("upmengban");
    }

    function remengban(object) {
        $("#" + object).removeClass("upmengban");
    }

    </script>
</head>
<body>
<div class="main">
	<!-- 首页焦点图开始 -->
    <div style="visibility: visible;" id="banner_box"  title="编辑此栏目" class="box_swipe" onmouseover="addmengban('banner_box')" onmouseout="remengban('banner_box')">
		<ol></ol>
	</div>
	<!-- 首页焦点图结束 -->
	<div class="menu-wrap">

	</div>
</div>

<!-- 底部菜单开始 -->
<!-- js在最后 -->
<script>
    $(function () {
        new Swipe(document.getElementById('banner_box'), {
            speed: 500,
            auto: 3000,
            callback: function () {
                var lis = $(this.element).next("ol").children();
                lis.removeClass("on").eq(this.index).addClass("on");
            }
        });
    });
	</script>
<script type="text/javascript" src="/Scripts/hilove.js"></script>
<input type="hidden" id="hid_comid" value="<%=comid %>" />
</body>
</html>