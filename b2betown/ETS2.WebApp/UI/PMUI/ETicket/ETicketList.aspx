<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ETicketList.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ETicket.ETicketList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">

        var pageSize = 10; //每页显示条数
        $(function () {

            var eclass = $("#hid_eclass").trimVal();
            var proid = $("#hid_proid").trimVal();
            var jsid = $("#hid_jsid").trimVal();
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var agentid = $("#hid_agentid").trimVal();
            SearchList(1);

            selprojectlist(0);
            selproductlist(0, 0);
            $("#sel_project").change(function () {
                var item = $("#sel_project").trimVal();
                selproductlist(0, item);
            })

            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });


            $("#Search").click(function () {
                SearchList(1);
            })

            $("#downtoexcel").click(function () {
                window.open("eticketlist_Down2.aspx?comid=" + comid + "&proid=" + $("#sel_product").trimVal() + "&key=" + $("#key").trimVal() + "&startime=" + $("#startime").trimVal() + "&endtime=" + $("#endtime").trimVal() + "&projectid=" + $("#sel_project").trimVal(), target = "_blank");
            })

            //验票明细列表
            function SearchList(pageindex) {
                if ($("#startime").trimVal() != "" || $("#endtime").trimVal() != "") {
                    if ($("#startime").trimVal() == "" || $("#endtime").trimVal() == "") {
                        alert("开始时间和结束时间需要同时选择");
                        return;
                    }
                }

                var key = $("#key").trimVal();
                var startime = $("#startime").trimVal();
                var endtime = $("#endtime").trimVal();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=pagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, eclass: eclass, proid: $("#sel_product").trimVal(), jsid: jsid, key: key, startime: startime, endtime: endtime, projectid: $("#sel_project").trimVal(), agentid: agentid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询验票明细错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("<tr><td height=\"26\" colspan=\"8\">查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })


            }

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

                        SearchList(page);

                        return false;
                    }
                });
            }
        })
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
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()" target=""><span>
                    电子凭证验证</span></a></li>
                <li class="on"><a href="/ui/pmui/eticket/eticketlist.aspx" onfocus="this.blur()"
                    target=""><span>验证明细</span></a></li>
                <li><a href="/ui/pmui/eticket/InterfaceUseLog.aspx" onfocus="this.blur()"
                    target=""><span>Wl接口验证明细</span></a></li>
                 <li class=""><a href="/ui/pmui/eticket/Eticket_safety.aspx" onfocus="this.blur()"
                    target=""><span>安全码查询</span></a></li>
                <li><a href="/ui/pmui/eticket/ReserveproVerify.aspx" onfocus="this.blur()" target="">
                    <span>预订产品验证</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <%--<h3>
                    <a href="eticketlist_Down2.aspx">下载验票数据</a> </h3>--%>
                <div style="text-align: center;">
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
                        日期查询<input name="startime" type="text" id="startime" isdate="yes" placeholder="开始日期" class="mi-input"/>
                    </label>
                    <label>
                        <input name="endtime" type="text" id="endtime" isdate="yes" placeholder="截止日期" class="mi-input"/>
                    </label>
                    <label>
                        电子凭证
                        <input name="key" type="text" id="key" style="width: 120px; height: 20px;" placeholder="电子码,订单号">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;">
                    </label>
                  
                </div>
                <div style=" float:right;padding-right:30px;">
                  <label>
                        <input type="button" id="downtoexcel" value="下载到Excel" style="width: 120px; height: 26px;">
                    </label>
                </div>
                <table border="0" width="780" class="mail-list-title">
                    <tr>
                        <td width="6%" align="center" bgcolor="#CCCCCC">
                            编号
                        </td>
                        <td width="26%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                产品名称
                            </p>
                        </td>
                        <td width="15%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                出票单位
                            </p>
                        </td>
                        <td width="6%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                订单ID</p>
                        </td>
                        <td width="6%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                面值
                            </p>
                        </td>
                        <td width="4%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                使用
                            </p>
                        </td>
                        <td width="10%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                有效期
                            </p>
                        </td>
                        <td width="10%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                票号
                            </p>
                        </td>
                        <td width="6%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                验票账户
                            </p>
                        </td>
                        <td width="15%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                确认日期
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                    <%-- <tr>
                        <td height="26" colspan="8">
                            <p align="center">
                                &nbsp;</p>
                            <p align="center">
                                总共1页&nbsp;&nbsp;&nbsp;&nbsp; 总共5条记录 第1页/共1页&nbsp;&nbsp; <strong>1</strong> 尾页</p>
                        </td>
                    </tr>--%>
                </table>
                <div id="divPage">
                </div>
                <p>
                </p>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td >
                            <p align="center">
                                ${id}</p>
                        </td>
                        <td >
                            <p align="center">
                                ${ProName}
                            </p>
                        </td>
                        <td >
                            <p align="center">
                            {{if Outcompany==""}}
                               --
                            {{else}}
                               ${Outcompany}
                               {{/if}}
                            </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${Orderid}
                            </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${FacePrice}</p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${UseNum}</p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${ChangeDateFormat(ProEnd)}
                            </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${Pno}</p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                {{if PosId !="0"}}
                                ${PosId} 
                                {{/if}}
                                {{if Pcaccount !=""}}
                                ${Pcaccount}
                                {{/if}}
                                </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${ConfirmDate}</p>
                        </td>
                    </tr>
    </script>
    <input type="hidden" id="hid_eclass" value="<%=eclass %>" />
    <input type="hidden" id="hid_proid" value="<%=proid %>" />
    <input type="hidden" id="hid_jsid" value="<%=jsid %>" />
    <input type="hidden" id="hid_agentid" value="<%=agentid %>" />
</asp:Content>
