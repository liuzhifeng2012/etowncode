<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ChannelFinance.aspx.cs" Inherits="ETS2.WebApp.UI.MemberUI.ChannelFinance" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 15; //每页显示条数

        $(function () {
            var channelcompanyid = $("#hid_channelcompanyid").trimVal();
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
                    url: "/JsonFactory/FinanceHandler.ashx?oper=ChannelFlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, channelcompanyid: channelcompanyid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

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

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/FinanceHandler.ashx?oper=ChannelFcount",
                    data: { comid: comid, channelcompanyid: channelcompanyid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 100) {
                            $("#channelmoney").html(data.msg)

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
               <li  ><a href="/ui/pmui/order/orderlist.aspx" onfocus="this.blur()" target="">
                    订单列表</a></li>
                <li class="on"><a href="/ui/MemberUI/ChannelFinance.aspx" onfocus="this.blur()">门票返佣 </a></li>
                <%if (ishaslvyoubusproorder == 1)
                  { %>
                <li ><a href="/ui/MemberUI/travelbusordercount.aspx" onfocus="this.blur()">旅游大巴订单统计 </a>
                </li><%} %>
        </div>

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
			<table width="780" border="0">
                    <tr>
                      <td width="221" height="32">您的账户总余额：<span id="channelmoney"></span>   <u></u></td>
                      <td width="402">当前时间：<%=DateTime.Now %></td>
                      <td width="143"></td>
                    </tr>
        </table>
                <%--<h3>
                    收支明细</h3>--%>
                <table width="780" border="0">
                    <tr>
                        <td width="51">
                            <p align="left">
                                流水号</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                操作时间
                            </p>
                        </td>
                        <td width="244">
                            <p align="left">
                                内容
                            </p>
                      </td>
                        <td width="76">
                            <p align="left">
                                收支类型
                            </p>
                      </td>
                        <td width="50">
                            <p align="left">
                                收入
                            </p>
                      </td>
                        <td width="44">
                            <p align="left">
                                支出
                            </p>
                      </td>
                      <td width="179">
                            <p align="left">
                                资金渠道
                            </p>
                      </td>

                      <td width="72">
                            <p align="left">&nbsp;
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
                        <td >
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${ChangeDateFormat(Subdate)}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Servicesname}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Payment_type}</p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Money>= 0}}${Money}{{/if}}</p>
                        </td>
                        <td>
                            <p align="left">
                              {{if Money< 0}}${Money}{{/if}}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Money_come}</p>
                        </td>
                        <td>
                        <p>${ChannelName}</p>
                        </td>
                    </tr>
    </script>
    <input type="hidden" id="hid_channelcompanyid" value="<%=channelcompanyid %>" />
</asp:Content>
