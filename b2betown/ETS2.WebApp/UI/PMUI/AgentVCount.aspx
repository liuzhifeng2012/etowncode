<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="AgentVCount.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentVCount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
        <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 100; //每页显示条数

        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid").trimVal();


            $.post("/JsonFactory/AgentHandler.ashx?oper=getAgentId", { agentid: agentid, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取数据出现错误");
                    return;
                }
                if (data.type == 100) {
                    $("#company").html(data.msg.Company);
                    $("#imprest").html("￥ " + data.msg.Imprest);
                    $("#warrant_level").html(data.msg.Warrant_level);
                    $("#credit").html(data.msg.Credit);
                    if (data.msg.Warrant_type == 1) {
                        $("#warrant_type").html("出票扣款");
                    } else if (data.msg.Warrant_type == 2) {
                        $("#warrant_type").html("验证扣款");
                    } else {
                        $("#warrant_type").html("尚未设定或设定错误");
                    }
                }

            })



            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                var key = $("#key").trimVal();
                var startime = $("#startime").trimVal();
                var endtime = $("#endtime").trimVal();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getagentordercount",
                    data: { comid: comid, agentid: agentid, pageindex: pageindex, pagesize: pageSize, key: key, startime: startime, endtime: endtime },
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

            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });

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
                <li class="on"><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                <li><a href="AgentManage.aspx" onfocus="this.blur()" target=""><span>未授权分销商</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
             <h3>
                    授权分销商</h3>
                    <table >
                    <tr>
                        <td class="tdHead">
                            公司名称：<span id="company"></span> </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           预付款： <span id="imprest"></span>
                        </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           信用额度： <span id="credit"></span> 
                        </td>

                    </tr>
                     <tr>
                        <td class="tdHead">
                           授权类型： <span id="warrant_type"></span>
                        </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           授权级别： <span id="warrant_level"></span>
                        </td>

                    </tr>
                </table>

                <h3>
                    <a href="AgentManageOrder.aspx?agentid=<%=agentid %>" class="a_anniu">订单列表</a>  &nbsp; <a href="AgentVCount.aspx?agentid=<%=agentid %>" class="a_anniu">分销验票统计</a> &nbsp; <a href="AgentVerification.aspx?agentid=<%=agentid %>" class="a_anniu" >分销验票记录</a>  &nbsp;  <a href="AgentManageFinance.aspx?agentid=<%=agentid %>" class="a_anniu" >财务管理</a> &nbsp;  <a href="AgentRechargeFinance.aspx?agentid=<%=agentid %>" class="a_anniu" >充值记录</a> &nbsp;  <a href="AgentManageSet.aspx?agentid=<%=agentid %>"  class="a_anniu">管  理</a></h3>

                    <div style="text-align: center;">
                    验票数量：
                    <label>
                       产品名称
                        <input name="key" type="text" id="key" style="width: 120px; height: 20px;">
                    </label>

                      日期  起始<input name="startime" type="text" id="startime"  isdate="yes" style="width: 120px; height: 20px;"/>
                        截止<input name="endtime" type="text" id="endtime"  isdate="yes" style="width: 120px; height: 20px;"/>

                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px;
                            height: 26px;" >
                    </label>
                </div>


                <table width="780" border="0">
                      <tr>
                        <td width="250px">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                出票数量
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                验证数量
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                分销结算价
                            </p>
                        </td>
                        <td width="180px">
                            <p align="center">
                                
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
                               ${Proname}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${TicketNum}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${VNum}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${Price}元
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                
                            </p>
                        </td>
                    </tr>
    </script>
    
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />
    <input type="hidden" id="hid_agentid" value='<%=agentid %>' />
</asp:Content>

