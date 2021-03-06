﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ad_image_sort.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.Ad_image_sort" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <title>产品排序</title>

    <%--<link rel="stylesheet" href="/Scripts/jquery-ui-1.10.3.custom/themes/base/jquery.ui.all.css"/>--%>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.sortable.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.cookie.2.2.0.min.js" type="text/javascript"></script>
    <%--<link rel="stylesheet" href="/Scripts/jquery-ui-1.10.3.custom/demos.css" />--%>
    <style type="text/css">
        ul{list-style-type:none; margin:10px 10px 0 0;width:100%; }
        ul  li{ width:100%;margin:5px;}
    </style>
    <script type="text/javascript">
        $(function () {
            var adid = $("#hid_adid").val();
            var comid = $("#hid_comid").val();

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getwxadimagespagelist", { comid: comid, pageindex: 1, pagesize: 100, adid: adid }, function (data2) {
                data2 = eval("(" + data2 + ")");
                if (data2.type == 1) {
                    $.prompt("列表获取出现问题");
                    return;
                }
                if (data2.type == 100) {
                    $("#myList").empty();
                    var str = "";
                    for (var i = 0; i < data2.totalCount; i++) {
                        str += '<li id="' + data2.msg[i].Id + '"><a href="#">ID:' + data2.msg[i].Id + '<img src="' + data2.msg[i].Imageurl + '" height="45px"></a></li>';
                    }
                    $("#myList").append(str);
                }
            })


            $("#myList").sortable({ delay: 1, stop: function () {

                $.cookie("myCookie", $("#myList").sortable('toArray'), { expires: 7 })
                //                            alert($.cookie("myCookie"));
            }
            });

            $("#btnsort").click(function () {
                if ($.cookie("myCookie")) {
                    var ids = $.cookie("myCookie");

                    if (ids == null) {
                        window.open("ProductList.aspx", target = "_self");
                    } else {
                        $.post("/JsonFactory/WeiXinHandler.ashx?oper=upWxadimages_sort", { menuids: ids }, function (data2) {
                            data2 = eval("(" + data2 + ")");
                            if (data2.type == 1) {
                                alert("操作出现问题");
                                return;
                            }
                            if (data2.type == 100) {
                                alert("排序成功");
                                $.cookie('myCookie', null);
                                window.open("Ad_image_manage.aspx?adid=" + adid, target = "_self");
                            }
                        })
                    }
                } else {
                    alert("请先对产品排序");
                }
            })

        }); 
    </script>
</head>
<body>
    <div>
        <ul id="myList">
        </ul>
    </div>
    <input style=" margin: 20px 0 0 100px;" type="button" id="btnsort" value="保存排序" />
    <a href="Ad_image_manage.aspx?adid=<%=adid %>">返回列表</a>
    <input type="hidden" id="hid_adid" value="<%=adid %>" />
    <input type="hidden" id="hid_comid" value="<%=comid %>" />
</body>
</html>
