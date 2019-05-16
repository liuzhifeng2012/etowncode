<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ETicketList.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ETicket.InterfaceUseLog" %>

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

            var comid = $("#hid_comid").trimVal();
          
            SearchList(1);

            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });


            $("#Search").click(function () {
                SearchList(1);
            })
            $("#downtoexcel").click(function () {
                window.open("interfaceUseLog_Down.aspx?comid=" + comid + "&key=" + $("#key").trimVal() + "&startime=" + $("#startime").trimVal() + "&endtime=" + $("#endtime").trimVal(), target = "_blank");
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
                    url: "/JsonFactory/EticketHandler.ashx?oper=interfaceuselogpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, startime: startime, endtime: endtime},
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
                                //$("#tblist").html("<tr><td height=\"26\" colspan=\"8\">查询数据为空</td></tr>");
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
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()" target=""><span>
                    电子凭证验证</span></a></li>
                <li ><a href="/ui/pmui/eticket/eticketlist.aspx" onfocus="this.blur()"
                    target=""><span>验证明细</span></a></li>
                 <li class="on"><a href="/ui/pmui/eticket/InterfaceUseLog.aspx" onfocus="this.blur()"
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
                        日期查询<input name="startime" type="text" id="startime" isdate="yes" placeholder="开始日期" class="mi-input"/>
                    </label>
                    <label>
                        <input name="endtime" type="text" id="endtime" isdate="yes" placeholder="截止日期" class="mi-input"/>
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
                            订单号
                        </td>
                        <td width="26%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                产品名称
                            </p>
                        </td>
                        <td width="10%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                姓名</p>
                        </td>
                        <td width="12%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                手机
                            </p>
                        </td>
                        <td width="4%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                出票单位
                            </p>
                        </td>
                        <td width="15%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                wl订单号
                            </p>
                        </td>
                        <td width="10%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                结算价
                            </p>
                        </td>
                        <td width="4%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                使用数量
                            </p>
                        </td>
                        <td width="10%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                有效期
                            </p>
                        </td>
                        <td width="15%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                核销时间
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>

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
                                ${orderid}</p>
                        </td>
                        <td >
                            <p align="center">
                             {{if prodatainfo != null}}
                                    ${prodatainfo.Pro_name}
                             {{/if}}
                            </p>
                        </td>
                        <td >
                            <p align="center">
                             {{if orderdatainfo != null}}
                                    ${orderdatainfo.U_name}
                             {{/if}}
                            </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                               {{if orderdatainfo != null}}
                                    ${orderdatainfo.U_phone}
                               {{/if}}
                            </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${Outcompany}</p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${wlorder}</p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                {{if orderdatainfo != null}}
                                    ${orderdatainfo.Pay_price}
                               {{/if}}
                            </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${usedQuantity}</p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                               {{if prodatainfo != null}}
                                    ${ChangeDateFormat(prodatainfo.Pro_end)}
                             {{/if}}
                                </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${jsonDateFormat(usetime)}</p>
                        </td>
                    </tr>
    </script>
</asp:Content>
