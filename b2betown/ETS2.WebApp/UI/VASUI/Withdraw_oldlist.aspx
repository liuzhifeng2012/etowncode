<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Withdraw_oldlist.aspx.cs"
    Inherits="ETS2.WebApp.UI.VASUI.Withdraw_oldlist" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 8; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载财务列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/FinanceHandler.ashx?oper=Financelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: '商家提现' },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询财务列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
//                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#FinanceItemEdit").tmpl(data.msg).appendTo("#tblist");
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
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="FinanceAll.aspx" target="" title="">收支明细</a></li>
                <li><a href="Withdraw.aspx" onfocus="this.blur()" target="">账户提现</a></li>
                <li class="on"><a href="Withdraw_oldlist.aspx" onfocus="this.blur()" target="">历史提现记录</a></li>
                <li><a href="Serverpay.aspx" onfocus="this.blur()" target="">收款设置</a></li>
                <li><a href="PaySet.aspx"  onfocus="this.blur()" target="">网上支付收款设置</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                        <td width="221" height="32">
                            您的账户总余额：<%=imprest%>
                            <u><a href="Withdraw.aspx">立即提现</a></u>
                        </td>
                        <td width="402">
                            当前时间：<%=DateTime.Now %>
                        </td>
                        <td width="143">
                        </td>
                    </tr>
                </table>
                <h3>
                    历史提现记录</h3>
                <table width="780" border="0">
                    <tr>
                        <td width="25">
                            <p align="left">
                                流水号</p>
                        </td>
                        <td width="50">
                            <p align="left">
                                操作时间
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                收款账户
                            </p>
                        </td>
                        <td width="20">
                            <p align="left">
                                收支类型
                            </p>
                        </td>
                        <td width="20">
                            <p align="left">
                                收入
                            </p>
                        </td>
                        <td width="20">
                            <p align="left">
                                支出
                            </p>
                        </td>
                        <td width="20">
                            <p align="left">
                                余额
                            </p>
                        </td>

                        <td width="50">
                            <p align="left">
                                备注
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                截图
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
    <script type="text/x-jquery-tmpl" id="FinanceItemEdit">   
                    <tr>
                        <td  >
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td  >
                            <p align="left">
                                ${ChangeDateFormat(Subdate)}
                            </p>
                        </td>
                        <td >
                            <p align="left" title="${Servicesname}"  style="cursor:pointer;">
                                ${ShortServicesname}</p>
                        </td>
                        <td  >
                            <p align="left">
                                ${Payment_type}</p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Money>= 0}}${Money}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                              {{if Money< 0}}${Money}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Over_money}    
                            </p>
                        </td>

                        <td >
                            <p align="left">
                              {{if Payment_type=="商家提现"}}{{if Con_state==0}}财务处理中...{{else}}${Remarks}{{/if}}{{/if}} </p>
                        </td>
                        <td>
                       {{if PrintscreenUrl != ""}}<a href="${PrintscreenUrl}"> <img src="${PrintscreenUrl}"  width="40" height="30"/></a>{{/if}}
                        </td>
                    </tr>
    </script>
</asp:Content>
