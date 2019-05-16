<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="outinterface.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.Agent.outinterface" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#inter_sendmethod").val('<%=inter_sendmethod %>');

            $("#btn").click(function () {
                var agentid = $("#hid_agentid").val();
                var agent_updateurl = $("#agent_updateurl").val();
                var txtagentip = $("#txtagentip").val();
                $.post("/JsonFactory/AgentHandler.ashx?oper=editagentoutinterface", { agentid: agentid, agent_updateurl: agent_updateurl, txtagentip: txtagentip, inter_sendmethod: $("#inter_sendmethod").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { alert(data.msg); return; }
                    if (data.type == 100) {
                        alert("编辑成功");
                        return;
                    }
                });
            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="Agentinfo.aspx" onfocus="this.blur()"><span>对外接口参数</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    对外接口参数</h3>
                <table class="grid">
                    <tr>
                        <td class="tdHead">
                            机构号:
                        </td>
                        <td>
                            <%=agent_no%>
                        </td>
                    </tr>
                    <tr id="edu">
                        <td class="tdHead">
                            deskey :
                        </td>
                        <td>
                            <%=deskey%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            webservice引用地址:
                        </td>
                        <td>
                            <%=outinterurl%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            分销商验证通知地址 :
                        </td>
                        <td>
                            <input type="text" id="agent_updateurl" value="<%=agent_updateurl%>" size="150" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            分销商绑定的ip :
                        </td>
                        <td>
                            <textarea id="txtagentip" rows="3" cols="80"> <%=agentbind_ip%></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            分销商验证通知发送方式:
                        </td>
                        <td>
                            <select id="inter_sendmethod">
                                <option value="post">post</option>
                                <option value="get">get</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead" colspan="2">
                            <input type="button" id="btn" value="编 辑" />
                            <input type="hidden" id="hid_agentid" value="<%=Agentid%>" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
