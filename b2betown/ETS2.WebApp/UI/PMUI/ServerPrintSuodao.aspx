<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServerPrintSuodao.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.ServerPrintSuodao"  MasterPageFile="/UI/Etown.Master"%>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });

            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            SearchList(1, 10);

            $("#Search").click(function () {
                SearchList(1, 10);
            })

        })
        function SearchList(pageindex, pagesize) {
            var comid = $("#hid_comid").trimVal();
            var startime = $("#startime").trimVal();
            var endtime = $("#endtime").trimVal();
            var key = $("#key").trimVal();
            if (startime != "" || endtime != "") {
                if (startime == "" || endtime == "") {
                    alert("开始时间和结束时间需要同时选择");
                    return;
                }
            }


            $.post("/JsonFactory/ProductHandler.ashx?oper=serverSuodaoPagelist", { comid: comid, startime: startime, endtime: endtime, key: key }, function (data) {
                data = eval("(" + data + ")");

                if (data.type == 1) {
                    //                        $.prompt(data.msg);
                    $("#tblist").empty();
                    $("#divPage").empty();
                    $("#tblist").html(data.msg);


                    return;
                }
                if (data.type == 100) {
                    $("#tblist").empty();
                    $("#divPage").empty();
                    if (data.msg.Count == 0) {
                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
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

                    SearchList(page, newpagesize);

                    return false;
                }
            });
        } 
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
<%--        <div class="navsetting ">
            <ul class="composetab">
               <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/serverlist.aspx">终端服务管理</a></div>
                    </div>
                </li>
                <li class="on" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerPrintSuodao.aspx">索道票打印统计</a></div>
                    </div>
                </li>

                <li  style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerFakaStat.aspx">发卡统计</a></div>
                    </div>
                </li>
                <li  style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerTimeoutStat.aspx">归还超时统计</a></div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_sel">
                        <div>
                            <a href="/ui/pmui/servercardinputlist.aspx">服务卡管理</a></div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/servercardinput.aspx">录入服务卡</a></div>
                    </div>
                </li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h4>
                    <label>
                        <input class="mi-input" name="startime" id="startime" placeholder="开始时间" value=""
                            isdate="yes" type="text" style="width: 120px;">-
                        <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes"
                            type="text" style="width: 120px;">
                    </label>
                    <label style=" display:none;">
                        关键字
                        <input name="key" type="text" id="key" class="mi-input" style="width: 280px;" placeholder="产品名称" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                </h4>

                <table width="780" border="0">
                    <tr>
                        <td width="40%">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                        <td width="30%">
                            <p align="left">
                               日期 </p>
                        </td>
                        
                        <td width="30%">
                            <p align="left">
                                验证数量
                            </p>
                        </td>
                       
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        
                        <td>
                            <p>${proname}</p>
                        </td>
                        <td>
                            <p>${startime} - ${endtime}</p>
                        </td>
                        <td>
                            <p>${num}</p>
                        </td>
                        
                    </tr>
    </script>
</asp:Content>
