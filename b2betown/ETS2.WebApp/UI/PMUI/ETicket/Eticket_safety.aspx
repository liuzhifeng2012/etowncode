<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"CodeBehind="Eticket_safety.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.ETicket.Eticket_safety" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">

        var pageSize = 24; //每页显示条数
        $(function () {
            var comid = $("#hid_comid").trimVal();

            SearchList(1);


            //日历
            var dateinput = $("input[isdate=yes]");
            var startdate = "<%=today %>";

            $.each(dateinput, function (i) {
                $($(this)).datepicker();
               

            });


            $("#Search").click(function () {
                SearchList(1);
            })

            $("#downtoexcel").click(function () {
               // window.open("eticketlist_Down2.aspx?comid=" + comid + "&proid=" + $("#sel_product").trimVal() + "&key=" + $("#key").trimVal() + "&startime=" + $("#startime").trimVal() + "&endtime=" + $("#endtime").trimVal() + "&projectid=" + $("#sel_project").trimVal(), target = "_blank");
            })


            $("#createsafety").click(function () {
                var startime = $("#startime").trimVal();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=createeticketsafety",
                    data: { comid: comid, startime: startime },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询验票明细错误");
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("生成成功");
                            SearchList(1);
                            return;
                        }
                    }
                })


            })

            //验票明细列表
            function SearchList(pageindex) {

                var startime = $("#startime").trimVal();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=eticketsafety",
                    data: { comid: comid, startime: startime },
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

                            }


                        }
                    }
                })


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
                <li class=""><a href="/ui/pmui/eticket/eticketlist.aspx" onfocus="this.blur()"
                    target=""><span>验证明细</span></a></li>
                    <li><a href="/ui/pmui/eticket/InterfaceUseLog.aspx" onfocus="this.blur()"
                    target=""><span>Wl接口验证明细</span></a></li>
                <li class="on"><a href="/ui/pmui/eticket/Eticket_safety.aspx" onfocus="this.blur()"
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
                        查询日期 <input name="startime" type="text" id="startime" isdate="yes" placeholder="查询日期" value="<%=today %>" class="mi-input"/>
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;">
                    </label>
                  
                </div>
                <div style=" float:right;padding-right:30px;">
                    <label>
                        <input type="button" id="createsafety" value="生成安全码" style="width: 120px; height: 26px;">
                    </label>
                </div>
                <table border="0" width="780" class="mail-list-title">
                    <tr>
                        <td width="6%" align="center" bgcolor="#CCCCCC">
                            编号
                        </td>
                        <td width="26%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                日期
                            </p>
                        </td>
                        <td width="15%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                安全码
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
                                ${id}</p>
                        </td>
                        <td >
                            <p align="center">
                                ${jsonDateFormatKaler(nowdate)}
                            </p>
                        </td>
                        <td >
                            <p align="center">
                               ${randomstr}
                            </p>
                        </td>
                        
                    </tr>
    </script>

</asp:Content>
