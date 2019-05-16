<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="CoopCount_Verification.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.CoopCount_Verification" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {
            var comid = $("#hid_comid").trimVal();
            $.post("/JsonFactory/OrderHandler.ashx?oper=coopcount", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取数据出现错误");
                    return;
                }
                if (data.type == 100) {
                    $("#Allnum").html(data.Allnum);
                    $("#Todaynum").html(data.Todaynum);
                    $("#Yesterdaynum").html(data.Yesterdaynum);
                    $("#Transactionnum").html(data.Transactionnum);
                }
            })

            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=coopcountverlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })


            }


            $("#Search").click(function () {
                SearchList(1);
            })


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
                <li class="on"><a href="CoopCount.aspx" onfocus="this.blur()" target=""><span>合作商预订统计</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
             <h3>
                   提交及验证统计</h3>
                    <table >
                    <tr>
                        <td class="tdHead">
                           提交总数笔数：<span id="Allnum"></span> </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           今日提交笔数： <span id="Todaynum"></span>
                        </td>

                    </tr>
                     <tr>
                        <td class="tdHead">
                           昨日提交笔数： <span id="Yesterdaynum"></span>
                        </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           实际成交人数： <span id="Transactionnum"></span>
                        </td>

                    </tr>
                </table>


                <h3>
                   <a href="CoopCount.aspx">订单列表</a>  <a href="CoopCount_Verification.aspx" class="on">验证列表</a></h3>
                <table width="780" border="0">
                       <tr>
                        <td width="45px" height="30px">
                            <p align="left">
                                ID
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                提交时间
                            </p>
                        </td>
                        <td width="147px">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                购买人
                            </p>
                        </td>
                        <td width="45px">
                            <p align="center">
                                单价
                            </p>
                        </td>
                        <td width="70px">
                            <p align="left">
                               验证电子码
                            </p>
                        </td>
                        <td width="70px">
                            <p align="left">
                               验证时间
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
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
                   <tr class="fontcolor">
                        <td valign="top">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                              ${jsonDateFormatKaler(U_subdate)}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                               ${Proname}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${U_name}(${U_phone})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               ${Pay_price}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                               ${Pno}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${jsonDateFormatKaler(U_traveldate)}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                ${U_num}</p>
                        </td>
                    </tr>
    </script>
    
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />
</asp:Content>

