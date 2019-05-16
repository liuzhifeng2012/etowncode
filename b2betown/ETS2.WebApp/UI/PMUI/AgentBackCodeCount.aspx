<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AgentBackCodeCount.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentBackCodeCount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 100; //每页显示条数

        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var orderid = $("#hid_orderid").trimVal();


            $.post("/JsonFactory/AgentHandler.ashx?oper=getAgentId", { agentid: agentid, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取数据出现错误");
                    return;
                }
                if (data.type == 100) {
                    $("#company").html(data.msg.Company);
                }
            })

            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=getIdagentordercount",
                data: { comid: comid, agentid: agentid, orderid: orderid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#Proname").html(data.msg);
                        $("#TicketNum").html(data.TicketNum);
                        $("#VNum").html(data.VNum);
                        $("#UnVNum").html(data.UnVNum);
                        $("#voidNum").html(data.voidNum);
                        $("#Price").html(data.Price + " 元");

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
                    url: "/JsonFactory/EticketHandler.ashx?oper=VagentEticketlog",
                    data: { comid: comid, agentid: agentid, orderid: orderid, pageindex: pageindex, pagesize: pageSize },
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


            $("#channeleticket").click(function () {
                if (confirm("确认作废此订单未使用的电子码！")) {

                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/OrderHandler.ashx?oper=EticketOrderVoid",
                        data: { comid: comid, agentid: agentid, orderid: orderid },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $.prompt("操作出错");
                                return;
                            }
                            if (data.type == 100) {
                                $.prompt("作废成功");
                                window.location.reload();
                                return;
                            }
                        }
                    })

                }
                else {
                    return false;
                }

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
                
                 
            </ul>
        </div>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
             <h3>
                    销售分销商</h3>
                    <table >
                    <tr>
                        <td class="tdHead">
                            公司名称：<span id="company"></span> </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           产品名称： <span id="Proname"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                           倒码数量： <span id="TicketNum"></span>  验证数量：<span id="VNum"></span> (<span id="Price"></span>)  作废数量：<span id="voidNum"></span>  未验证数量：<span id="UnVNum"></span>  <a href="javascript:;" class="a_anniu" id="channeleticket">未验证电子码整体作废 </a> 
                        </td>

                    </tr>
                     
                </table>

                 <h3></h3>

                 <input onclick="javascript:history.go(-1);" value="  <<<返回  "/>


                 <h3>已验证电子码列表</h3>

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
                               结算价格
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
                            ${E_sale_price}元
                            </p>
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
    <input type="hidden" id="hid_agentid" value='<%=agentid%>' />
    <input type="hidden" id="hid_orderid" value='<%=orderid%>' />
</asp:Content>

