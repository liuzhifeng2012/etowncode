<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AgentManage.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentManage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var pageSize = 50; //每页显示条数
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();

            SearchList(1);

            $.ajax({
                type: "post",
                url: "/JsonFactory/AgentHandler.ashx?oper=etownagentwarrant",
                data: { comid: comid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询分销商列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            $("#etownagent").hide();
                        } else {
                            $("#etownagent").show();
                        }
                    }
                }
            })

            //查询
            $("#Search").click(function () {
                SearchList(1);
            });

            function SearchList(pageindex) {


                var key = $("#key").val();
                var select = '';

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=unagentlist",
                    data: { pageindex: pageindex, pagesize: pageSize, key: key, comid: comid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询分销商列表错误");
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
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                <li class="on"><a href="AgentManage.aspx" onfocus="this.blur()" target=""><span>易城商户分销商</span></a></li>
                <li><a href="AgentSalesCode.aspx" onfocus="this.blur()" target=""><span>后台销售订单</span></a></li>
                <li><a href="AgentBackCode.aspx" onfocus="this.blur()" target=""><span>导码订单</span></a></li>
                <li><a href="AgentRecharge.aspx" onfocus="this.blur()" target=""><span>充值订单</span></a></li>
                <li><a href="AgentMessage.aspx" onfocus="this.blur()" target="">管理分销通知</a></li>
                <li><a href="AgentFinanceCount.aspx" onfocus="this.blur()" target="">分销商统计</a>
            </ul>
        </div>--%>

        <input type="hidden" id="hid_agentid" value='<%=agentid %>' />
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <div id="etownagent" style=" display:none;">
                    <h3>
                    您尚未授权易城商户销售（当您授权易城商户后，以下易城分销商都可以销售您的产品了）</h3>
                    <a href="AgentManageSet.aspx?agentid=9" class="a_anniu">立即 授权易城商户</a>
                </div>
                <h3>
                    易城商户分销商</h3>

                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="200">
                            <p align="left">
                                分销商公司名称
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                省市
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                类别
                            </p>
                        </td>
                        <td width="71">
                            <p align="left">
                               系统内销售数量</p>
                        </td>
                       <td width="71">
                            <p align="left">
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
                        <td >
                            <p align="left" >
                               ${Company}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Com_province}</p>
                        </td>
                        <td >
                            <p align="left">
                            {{if Agent_sourcesort==0}}
                             --
                             {{/if}}
                             {{if Agent_sourcesort==3}}
                             在线旅游(OTA)
                             {{/if}}
                             {{if Agent_sourcesort==4}}
                             团购网
                             {{/if}}

                             {{if Agent_sourcesort==5}}
                             平台电商
                             {{/if}}

                             {{if Agent_sourcesort==6}}
                             垂直网站及社区论坛
                             {{/if}}

                             {{if Agent_sourcesort==7}}
                             票务系统(公司)
                             {{/if}}

                             {{if Agent_sourcesort==8}}
                             旅行社
                             {{/if}}

                             {{if Agent_sourcesort==9}}
                             酒店服务
                             {{/if}}
                             {{if Agent_sourcesort==10}}
                             俱乐部
                             {{/if}}
                             {{if Agent_sourcesort==11}}
                             户外实体店
                             {{/if}}
                             {{if Agent_sourcesort==12}}
                             礼品特产专卖店
                             {{/if}}
                             {{if Agent_sourcesort==13}}
                             会议公司
                             {{/if}}
                             {{if Agent_sourcesort==14}}
                             其 他
                             {{/if}}

                               </p>
                        </td>
                        <td style="color="#ff0000">
                            {{if Sale<500}}
                             ★
                            {{else}}
                                {{if Sale<2000}}
                                 ★★
                                {{else}}
                                    {{if Sale<5000}}
                                     ★★★
                                    {{else}}
                                        {{if Sale<10000}}
                                        ★★★★
                                        {{else}}
                                        ★★★★★
                                        {{/if}}
                                    {{/if}}
                                {{/if}}
                            {{/if}}
                        </td>
                        <td >
                        <%if (comid==106){ %>
                        <a href="AgentManageSet.aspx?agentid=${Id}" class="a_anniu">立即授权</a>  &nbsp;
                        <%} %>
                        </td>
                    </tr>
                    
    </script>

</asp:Content>
