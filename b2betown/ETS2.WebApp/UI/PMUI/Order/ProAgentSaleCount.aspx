<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="ProAgentSaleCount.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.Order.ProAgentSaleCount" %>

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
            SearchList(1);

            selprojectlist(0);
            selproductlist(0, 0);

            $("#sel_project").change(function () {
                var item = $("#sel_project").trimVal();
                selproductlist(0, item);
            })

            $("#Search").click(function () {
                SearchList(1);
            })
        })

        function SearchList(pageindex) {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var projectid=$("#sel_project").trimVal();
            var productid=$("#sel_product").trimVal();

            if(projectid=="" || projectid=="0"){
                if(productid=="" || productid=="0"){
                   alert("请选择项目或产品进行查询");
                    return;
                }
            }

            if ($("#startime").trimVal() != "" || $("#endtime").trimVal() != "") {
                if ($("#startime").trimVal() == "" || $("#endtime").trimVal() == "") {
                    alert("开始时间和结束时间需要同时选择");
                    return;
                }
            }


            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=prosaleordercount", //产品分销统计
                data: { comid: comid, begindate: $("#startime").trimVal(), enddate: $("#endtime").trimVal(), projectid: $("#sel_project").trimVal(), productid: $("#sel_product").trimVal(), key: $("#key").trimVal(), orderstate: "2,4,8,22" },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    $("#tblist").empty();
                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        if (data.totalcount == 0) {
                            $("#tblist").html($("#sel_project").find("option:selected").text() + $("#sel_product").find("option:selected").text() + "查询数据为空"); 
                        } else {
                            $("#FinanceItemEdit").tmpl(data.msg).appendTo("#tblist");
                        } 
                    }
                }
            })
        }
      
        function selprojectlist(seled) {
            $.post("/JsonFactory/FinanceHandler.ashx?oper=selprojectlist", { comid: $("#hid_comid").trimVal() }, function (data) {
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
                            rstr += '<option value="' + data.msg[i].Id + '" selected ="selected">' + data.msg[i].Pro_name + '</option>';
                        }
                        else {
                            rstr += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Pro_name + '</option>';
                        }
                    }
                    $("#sel_product").html(rstr);
                }
            })
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
                <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/order/HotelOrderStat.aspx">订房统计</a></div>
                    </div>
                </li>
                <li class="on" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/order/ProAgentSaleCount.aspx">产品销售统计</a></div>
                    </div>
                </li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <div style="text-align: left; margin: 10px;">
                    <label>
                        <select id="sel_project" class="mi-input" style="width: 120px;">
                            <option value="0">请选择项目</option>
                        </select>
                    </label>
                    <label>
                        <select id="sel_product" class="mi-input" style="width: 120px;">
                            <option value="0">请选择产品</option>
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
                        关键词查询
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;" placeholder="产品编号、产品名称"
                            class="mi-input" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                </div>
                <table width="780" border="0" style="margin-left: 15px;">
                    <tr>
                        <th width="20%">
                            项目
                        </th>
                        <th width="30%">
                            产品信息
                        </th>
                        <th width="20%">
                            分销商
                        </th>
                        <th width="10%">
                            平均价格
                        </th>
                        <th width="10%">
                            订购数量
                        </th>
                        <th width="10%">
                            验证数量
                        </th>
                        <th>
                        </th>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <%-- <div id="divPage">
                </div>--%>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="FinanceItemEdit">   
        <tr>
        <td title="${projectname}">${projectname}</td>
        <td title="${productname}">${productname}</td>
        <td>{{if Agentcompany==null}}直销{{else}}${Agentcompany.Company}{{/if}}</td>
        <td>${Pay_price}</td> 
        <td>${u_num}</td> 
        <td>${yanzhengnum}</td> 
        <td></td>
        </tr>
    </script>
</asp:Content>
