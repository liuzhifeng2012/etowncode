<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="AgentVerification.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentVerification" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
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
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=agentEticketlog",
                    data: { comid: comid, agentid: agentid, pageindex: pageindex, pagesize: pageSize },
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
                    
                    <table width="780" border="0">
                      <tr>
                        <td width="51">
                            <p align="left">
                                流水号</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                验证时间
                            </p>
                        </td>
                        <td width="150">
                            <p align="left">
                                服务内容
                            </p>
                      </td>
                        <td width="60">
                            <p align="left">
                                订单号
                            </p>
                      </td>
                        <td width="66">
                            <p align="left">
                                电子票
                            </p>
                      </td>
                        <td width="66">
                            <p align="left">
                                订购/使用数
                            </p>
                      </td>
                      <td width="72">
                            <p align="left">
                               验票机ID
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
                        <td >
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${jsonDateFormatKaler(Actiondate)}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${E_proname}</p>
                        </td>
                                                <td>
                            <p align="left">
                                ${Oid}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Pno}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Pnum}/${Use_pnum}</p>
                        </td>
                                                <td>
                            <p align="left">
                            ${PosId}
                            </p>
                        </td>
                    </tr>
    </script>
    
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />
    <input type="hidden" id="hid_agentid" value='<%=agentid %>' />
</asp:Content>

