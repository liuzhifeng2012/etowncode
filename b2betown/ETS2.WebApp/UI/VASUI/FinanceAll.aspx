<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="FinanceAll.aspx.cs" Inherits="ETS2.WebApp.UI.VASUI.FinanceAll" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
   <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 15; //每页显示条数

        $(function () {

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });

            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            $("#FinanceToExcel").click(function () {
                window.open("/excel/DownExcel.aspx?oper=Financelisttoexcel&comid=" + comid + "&beginDate=&endDate=" , target = "_blank");
            })

            //装载财务列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/FinanceHandler.ashx?oper=Financelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
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
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li  class="on"><a href="FinanceAll.aspx" target="" title="">收支明细</a></li>
                <li ><a href="Withdraw.aspx"  onfocus="this.blur()" target="">账户提现</a></li>
                <li ><a href="Withdraw_oldlist.aspx"  onfocus="this.blur()" target="">历史提现记录</a></li>
                <li ><a href="Serverpay.aspx"  onfocus="this.blur()" target="">收款设置</a></li>
                <li><a href="PaySet.aspx"  onfocus="this.blur()" target="">网上支付收款设置</a></li>
            </ul>
        </div>--%>

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
			<table width="780" border="0">
                    <tr>
                      <td width="221" height="32">您的账户总余额：<%=imprest%>   <u><a href="Withdraw.aspx">立即提现</a></u></td>
                      <td width="402">当前时间：<%=DateTime.Now %></td>
                      <td width="143">  <input type="button" value="导出到Excel" style="width: 120px;
                            height: 26px;" id="FinanceToExcel"></td>
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
                        <td width="66">
                            <p align="left">
                                余额
                            </p>
                      </td>
                        <td width="79">
                            <p align="left">
                                资金渠道
                            </p>
                      </td>
                        <td width="72">
                            <p align="left">&nbsp;
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
                                {{if Agent_id ==0}}
                                ${Servicesname}
                                {{else}}
                                <a href="/ui/pmui/AgentManageOrder.aspx?agentid=${Agent_id}&key=${Order_id}" target="_blank" style="color: #2E64AA;" >${Servicesname}</a>
                                {{/if}}


                                [${Agentname}${Pno}]</p>
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
                        <td >
                            <p align="left">
                                ${Over_money}    
                            </p>
                        </td>
                        <td>
                            <p align="left">
                            {{if Money_come=="wx"}}
                            微信支付
                            {{else}}
                            ${Money_come}
                            {{/if}}
                                </p>
                        </td>
                        <td>
                            <p align="left">
                            {{if Payment_type=="商家提现"}}{{if Con_state==0}}财务处理中...{{else}}${Remarks}{{/if}}{{/if}}
                            </p>
                        </td>
                        <td>
                        <p>${ChannelName}</p>
                        </td>
                    </tr>
    </script>
</asp:Content>
