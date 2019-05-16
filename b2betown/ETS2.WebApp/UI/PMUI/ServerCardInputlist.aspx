<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServerCardInputlist.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ServerCardInputlist" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 20; //每页显示条数
        $(function () {
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            $("#Search").click(function () {
                SearchList(1);
            })
        })
        function SearchList(pageindex) {
            var beginprintid = $("#beginprintid").trimVal();
            if (isNaN(beginprintid)) {
                alert("开始卡面编号需要为数字");
                return;
            }
            var endprintid = $("#endprintid").trimVal();
            if (isNaN(endprintid)) {
                alert("结束卡面编号需要为数字");
                return;
            }
            var cardchipid = $("#cardchipid").trimVal();
            $.post("/JsonFactory/ProductHandler.ashx?oper=Relationserver_printid_chipidList", { comid: $("#hid_comid").trimVal(), beginprintid: beginprintid, endprintid: endprintid, cardchipid: cardchipid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#tblist").empty();
//                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    $("#tblist").empty();
                    $("#divPage").empty();
                    if (data.totalCount == 0) {
                        //                                $("#tblist").html("查询数据为空");
                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                        setpage(data.totalCount, pageSize, pageindex);
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

        function delrelation(obj, id) {

            $.post("/JsonFactory/ProductHandler.ashx?oper=delRelationserver_printid_chipid", { relationid: id }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    $(obj).attr("disabled", "disabled").css("background-color", "gray").css("text-decoration", "line-through").text("已删除");
                    return;
                }
            })
        }
    </script>
    <style type="text/css">
        .d_out
        {
            background-color: #FFF;
        }
        .d_over
        {
            background-color: #C1D9F3;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting">
            <ul>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/productservertypelist.aspx')">
                    <span>添加产品</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/projectlist.aspx')">
                    <span>项目管理</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/projectedit.aspx')">
                    <span>添加项目</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/productlist.aspx')">
                    <span>产品列表</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/order/salecount.aspx')">
                    <span>产品统计</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/bindingagentlist.aspx')">
                    <span>导入分销系统产品</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/eticket_useset.aspx')">
                    <span>商户特定日期设定</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/delivery/deliverylist.aspx')">
                    <span>运费模板管理</span></a></li>
                <li class="on"><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/serverlist.aspx')">
                    <span>终端服务管理</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/cablewayeticket_useset.aspx')">
                    <span>下班时间管理</span></a></li>
            </ul>
        </div>--%>
            <div class="navsetting ">
            <ul class="composetab">
               <%-- <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/serverlist.aspx">终端服务管理</a></div>
                    </div>
                </li>
                <li style="width: 110px; padding-right: 2px;">
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
                <li   style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerTimeoutStat.aspx">归还超时统计</a></div>
                    </div>
                </li>--%>
                <li class="on" style="width: 110px; padding-right: 2px;">
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
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3 style="font-size: 20px;">
                    服务卡片列表</h3>
                <div style="text-align: center;">
                    <label>
                        开始卡面编号
                        <input type="text" id="beginprintid" style="width: 160px; height: 20px;" />
                        结束卡面编号
                        <input type="text" id="endprintid" style="width: 160px; height: 20px;" />
                        卡芯片ID
                        <input type="text" id="cardchipid" style="width: 160px; height: 20px;" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                </div>
                <table style="border: none;">
                    <tr>
                        <td style="font-size: 18px;">
                            卡片芯片ID
                        </td>
                        <td style="font-size: 18px;">
                            卡面印刷编号
                        </td>
                        <td>
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
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                  <tr  class="d_out" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td>
                        ${cardchipid}
                        </td>
                         <td>
                         ${cardprintid}
                        </td>
                         <td>
                         <a href="javascript:void(0)" style="color: #FFF; border-color: rgb(28, 189, 241);
                                background-color: rgb(60, 175, 220); font-size: 14px;  margin-top: 10px;
                                padding: 5px 10px; height: 18px;" onclick="delrelation(this,${id})">删 除</a>
                        </td>
                  </tr>
    </script>
</asp:Content>
