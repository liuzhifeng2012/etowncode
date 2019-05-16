<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberShipCardSort.aspx.cs"
    Inherits="ETS2.WebApp.MemberShipCard.MemberShipCardSort" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <title>微信端会员卡素材排序</title>
    <link rel="stylesheet" href="/Scripts/jquery-ui-1.10.3.custom/themes/base/jquery.ui.all.css">
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.sortable.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.cookie.2.2.0.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/Scripts/jquery-ui-1.10.3.custom/demos.css" />
    <style type="text/css">
        #sortable1, #sortable2
        {
            list-style-type: none;
            margin: 0;
            padding: 0;
            margin-bottom: 15px;
            zoom: 1;
        }
        #sortable1 li, #sortable2 li
        {
            margin: 0 5px 5px 5px;
            padding: 5px;
            font-size: 1.2em;
            width: 95%;
        }
    </style>
    <script type="text/javascript">
        $(function () {
           
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Membershipcardpagelist", { comid: $("#hid_comid").val(), applystate: true, pageindex: 1, pagesize: 100 }, function (data2) {
                data2 = eval("(" + data2 + ")");
                if (data2.type == 1) {
                    $.prompt("素材列表获取出现问题");
                    return;
                }
                if (data2.type == 100) {
                    $("#myList").empty();
                    var str = "";
                    for (var i = 0; i < data2.totalCount; i++) {
                        str += '<li id="' + data2.msg[i].MaterialId + '"><a href="/MemberShipCard/MemberShipCardDetail.aspx?materialid=' + data2.msg[i].MaterialId + '">MaterialId:' + data2.msg[i].MaterialId + '--Title:' + data2.msg[i].Title + '</a></li>';
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

                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=MemberShipCardMaterialSort", { materialids: ids }, function (data2) {
                        data2 = eval("(" + data2 + ")");
                        if (data2.type == 1) {
                            alert("素材操作出现问题");
                            return;
                        }
                        if (data2.type == 100) {
                            alert("素材排序成功");
                            $.cookie('myCookie', null);
                            window.open("MemberShipCardList.aspx", target = "_self");
                        }
                    })
                } else {
                    alert("请先对素材排序");
                }
            })

        }); 
    </script>
</head>
<body>
    <div>
        <ul id="myList">
        </ul>
        <input type="button" id="btnsort" value="排序" />
    </div>
    <input type="hidden" id="hid_comid" value="<%=comid %>" />
</body>
</html>
