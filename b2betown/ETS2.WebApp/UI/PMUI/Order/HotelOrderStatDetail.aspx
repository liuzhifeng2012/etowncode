﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotelOrderStatDetail.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.Order.HotelOrderStatDetail" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#Search").click();    //这里添加要处理的逻辑  
                    return false;
                }
            });

            $("#Search").click(function () {
                SearchList(1, 0);
            })

            $("#startime").val('<%=begindate %>');
            $("#endtime").val('<%=enddate %>');

            SearchList(1, '<%=productid %>');
            //查询必须精确到产品id
            $.post("/JsonFactory/ProductHandler.ashx?oper=getProById", { proid: '<%=productid %>' }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    return;
                }
                if (data.type == 100) {
                    selprojectlist(data.msg.Projectid);
                    selproductlist('<%=productid %>', data.msg.Projectid);
                }
            })

            $("#sel_project").change(function () {
                var item = $("#sel_project").trimVal();
                selproductlist(0, item);
            })
        })

        function SearchList(pageindex, productid) {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var begindate = $("#startime").trimVal();
            var enddate = $("#endtime").trimVal();
            if (productid > 0) { }
            else {
                productid = $("#sel_product").trimVal();
                if (productid == 0) {
                    alert("查询需要精确到产品!");
                    return;
                }
            }

            $.ajax({
                type: "post",
                url: "/JsonFactory/FinanceHandler.ashx?oper=HotelOrderlist",
                data: { comid: comid, begindate: begindate, enddate: enddate, productid: productid, orderstate: '<%=orderstate %>' },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");


                    $("#tblist").empty();
                    if (data.type == 1) {
                        //                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#FinanceItemEdit").tmpl(data.msg).appendTo("#tblist");

                    }
                }
            })
        }

        function selprojectlist(seled) {
            $.post("/JsonFactory/FinanceHandler.ashx?oper=selhotelprojectlist", { comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");

                if (data.type == 1) {
                    $("#sel_project").html('<option value="0">全部项目</option>');
                }
                if (data.type == 100) {
                    var rstr = '<option value="0">全部项目</option>';
                    for (var i = 0; i < data.msg.length; i++) {
                        if (seled == data.msg[i].Id) {
                            rstr += '<option value="' + data.msg[i].Id + '" selected ="selected">' + data.msg[i].Projectname + '</option>';
                        }
                        else {
                            rstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Projectname + '</option>';
                        }
                    }
                    $("#sel_project").html(rstr);
                }
            })
        }
        function selproductlist(seled, projectid) {
            $.post("/JsonFactory/FinanceHandler.ashx?oper=selhotelproductlist", { comid: $("#hid_comid").trimVal(), projectid: projectid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#sel_product").html('<option value="0">全部产品</option>');
                }
                if (data.type == 100) {
                    var rstr = '<option value="0">全部产品</option>';
                    for (var i = 0; i < data.msg.length; i++) {
                        if (seled == data.msg[i].Id) {
                            rstr += '<option value="' + data.msg[i].Id + '"  selected ="selected">' + data.msg[i].Pro_name + '</option>';
                        }
                        else {
                            rstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Pro_name + '</option>';
                        }
                    }
                    $("#sel_product").html(rstr);
                }
            })
        }

        function daochuexcel() {
            window.open("/excel/DownExcel.aspx?oper=hotelordertoexcel&comid=" + $("#hid_comid").trimVal() + "&beginDate=" + $("#startime").trimVal() + "&endDate=" + $("#endtime").trimVal() + "&productid=" + $("#sel_product").trimVal() + "&orderstate=2,4,8,22", target = "_blank");
        }

        function showbinder(bookpro_bindcompany, bookpro_bindconfirmtime, bookpro_bindname, bookpro_bindphone) {
            alert(bookpro_bindcompany + "--" + bookpro_bindname + "--" + bookpro_bindphone + "--" + bookpro_bindconfirmtime);
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div class="navsetting ">
            <ul class="composetab">
                <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/order/SaleCount.aspx">产品统计</a></div>
                    </div>
                </li>
                <li class="on" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/order/HotelOrderStat.aspx">订房统计</a></div>
                    </div>
                </li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <div style="text-align: left; margin: 10px;">
                    <label>
                        <select id="sel_project" class="mi-input" style="width: 120px;">
                            <option value="0">全部项目</option>
                        </select>
                    </label>
                    <label>
                        <select id="sel_product" class="mi-input" style="width: 120px;">
                            <option value="0">全部产品</option>
                        </select>
                    </label>
                    <label>
                        入住日期:
                        <input class="mi-input" name="startime" id="startime" placeholder="开始时间" value=""
                            isdate="yes" type="text" style="width: 120px;">-
                        <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes"
                            type="text" style="width: 120px;">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" /></label>
                    <label>
                        <input type="button" id="tbutton" onclick="daochuexcel()" value="导出订单到excel" style="width: 120px;
                            height: 26px;" /></label>
                </div>
                <table width="780" border="0" style="margin-left: 15px;">
                    <tr>
                        <th width="10%">
                            项目
                        </th>
                        <th width="20%">
                            产品信息
                        </th>
                        <th width="5%">
                            订单id
                        </th>
                        <th width="10%">
                            提单时间
                        </th>
                        <th width="10%">
                            入住人信息
                        </th>
                        <th width="10%">
                            入住间数
                        </th>
                        <th width="10%">
                            入住日期
                        </th>
                        <th width="10%">
                            离店日期
                        </th>
                        <th width="10%">
                            入住天数
                        </th>
                        <th>
                            绑定人
                        </th>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <%--   <div id="divPage">
                </div>--%>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="FinanceItemEdit">   
        <tr>
          <td title="${projectname}">${projectname}</td>
          <td title="(${pro_id})${pro_name}">(${pro_id})${pro_name}</td>
          <td>${ordernum}</td>
          <td title="${u_subdate}">${u_subdate}</td>
          <td title="${u_name}(${u_phone})">${u_name}(${u_phone})</td>
          <td>${u_num}</td>
          <td>${start_date}</td>
          <td>${end_date}</td>
          <td>${bookdaynum}</td>
          <td><a href="javascript:void(0)" onclick="showbinder('${bookpro_bindcompany}','${bookpro_bindconfirmtime}','${bookpro_bindname}','${bookpro_bindphone}')" style="text-decoration:underline;">绑定人信息</a></td>
        </tr>
    </script>
</asp:Content>
