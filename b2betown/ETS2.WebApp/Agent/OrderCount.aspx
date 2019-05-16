<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="OrderCount.aspx.cs" Inherits="ETS2.WebApp.Agent.OrderCount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            var key = $("#key").trimVal();
            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getagentordercount",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid, comid: comid, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })


                $("#0").click(function () {
                    select(0, 1);
                    $("#0").css("color", "red");
                })
                $("#1").click(function () {
                    select(1, 1);
                    $("#1").css("color", "red");
                })
                $("#3").click(function () {
                    select(3, 1);
                    $("#3").css("color", "red");
                })
                $("#4").click(function () {
                    select(4, 1);
                    $("#4").css("color", "red");
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
                    <table>
                    <tr>
                        <td class="tdHead" style="font-size:14px; height:26px;">
                                              <div class="left"><img id="comlogo" src="" class="" height="42"></div>
                    
                    <div class="left comleft">
                    <div ><span> 商户名称：
                    <%=company %> </span>
                    <span>授权类型：
                    <%=Warrant_type_str%>；</span> <span><%if (contact_phone != "")
                     {%>客服电话：<%=contact_phone %><%} %></span>
                     </div>
                      <div>
                      <%=yufukuan%>
                    <a class="a_anniu" href="Recharge.aspx?comid=<%=comid_temp %>" target="_blank">在线充值</a> <span id="Span1"
                        style="padding-left: 30px;"></span><span id="Span2" style="padding-left: 30px;">
                    </span>
                    </div>
                     </div>
                           
                           
                           </td>
                    </tr>

                </table>
        <div id="secondary-tabs" class="navsetting ">
           </li>
            <%if (Warrant_type == 2)
                  { %>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="EticketCount.aspx?comid=<%=comid_temp %>">验码统计</a>
                </div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Verification.aspx?comid=<%=comid_temp %>">验码记录</a>
                </div></div>
            </li>
            <% } %>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Finane.aspx?comid=<%=comid_temp %>">财务列表</a>
                </div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="EticketBack.aspx?comid=<%=comid_temp %>">退订/订单状态</a>
                </div></div>
            </li>

        </div>

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                    
                    <div style="text-align: center;">
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
                                金额
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
                            <p align="center">
                                
                            </p>
                        </td>
                    </tr>
                    
    </script>

    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />


    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>

