<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinanceStat.aspx.cs" Inherits="ETS2.WebApp.UI.VASUI.FinanceStat"
    MasterPageFile="/UI/Etown.Master" %>

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

        })

        //装载财务统计
        function SearchList(pageindex) {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            if ($("#startime").trimVal() != "" || $("#endtime").trimVal() != "") {
                if ($("#startime").trimVal() == "" || $("#endtime").trimVal() == "") {
                    alert("开始时间和结束时间需要同时选择");
                    return;
                }
            }

            $.ajax({
                type: "post",
                url: "/JsonFactory/FinanceHandler.ashx?oper=FinanceStat",
                data: { comid: comid, begindate: $("#startime").trimVal(), enddate: $("#endtime").trimVal(), projectid: $("#sel_project").trimVal(), productid: $("#sel_product").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        //                        $("#tblist").empty();
                        //                        $("#divPage").empty();
                        //                        if (data.totalCount == 0) {
                        //                            //                                $("#tblist").html("查询数据为空");
                        //                        } else {
                        //                            $("#FinanceItemEdit").tmpl(data.msg).appendTo("#tblist");
                        //                            setpage(data.totalCount, pageSize, pageindex);
                        //                        } 

                        $("#directDt1").html("直销订单:售卖" + data.directDt1[0].totalnum + "张 退票" + data.directDt1[0].cancelnum + "张 应收" + data.directDt1[0].shouldmoney + "元 退款" + data.directDt1[0].cancelmoney + "元 实收" + data.directDt1[0].factmoney + "元");
                        $("#agentDt2").html("分销订单(出票扣款):售卖" + data.agentDt2[0].totalnum + "张 退票" + data.agentDt2[0].cancelnum + "张 应收" + data.agentDt2[0].shouldmoney + "元 退款" + data.agentDt2[0].cancelmoney + "元 实收" + data.agentDt2[0].factmoney + "元");
                        $("#agentDt3").html("分销订单(验证扣款):售卖" + data.agentDt31[0].totalnum + "张 退票" + data.agentDt31[0].cancelnum + "张 应收" + data.agentDt31[0].shouldmoney + "元 退款" + data.agentDt31[0].cancelmoney + "元 实收" + data.agentDt32[0].factmoney + "元");
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
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
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
                        提单时间
                        <input class="mi-input" name="startime" id="startime" placeholder="开始时间" value=""
                            isdate="yes" type="text" style="width: 120px;">-
                        <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes"
                            type="text" style="width: 120px;">
                    </label>
                    <label>
                        关键词查询
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;" placeholder="产品编号、产品名称" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                </div>
                <table width="780" border="0">
                    <tr >
                        <td>
                            <p align="left" id="directDt1" style="padding-left: 20px;">
                                直销订单:售卖XXX张 退票XXX张 应收XXX元 退款XXX元 实收XXX元</p>
                        </td>
                    </tr>
                    <tr>
                        <td  >
                            <p align="left" id="agentDt2" style="padding-left: 20px;">
                                分销订单(出票扣款):售卖XXX张 退票XXX张 应收XXX元 退款XXX元 实收XXX元</p>
                        </td>
                    </tr>
                    <tr  >
                        <td>
                            <p align="left" id="agentDt3" style="padding-left: 20px;">
                                分销订单(验证扣款):售卖XXX张 退票XXX张 应收XXX元 退款XXX元 实收XXX元</p>
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
