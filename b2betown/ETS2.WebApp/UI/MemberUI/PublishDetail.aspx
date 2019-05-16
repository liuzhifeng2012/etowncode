<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="PublishDetail.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.PublishDetail" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //编辑发行管理
        $(function () {
            var issueid = $("#hid_issueid").trimVal();
            if (issueid != "0") {//编辑发行操作
                //根据发行id得到发行信息
                $.post("/JsonFactory/IssueHandler.ashx?oper=GetIssueDetail2", { issueid: issueid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("根据发行id得到发行信息出错");
                        return;
                    }
                    if (data.type == 100) {
                        $("#issuetitle").text(data.msg[0].Title);
                        $("#issueid").text(data.msg[0].Id);
                        $("#act").text(data.msg[0].ActName);
                        $("#issuechannel").text(data.msg[0].IssueType + " " + data.msg[0].ChannelUnit);
                        $("#Name").text(data.msg[0].Name);
                        $("#Num").text(data.msg[0].Num + "张");
                        $("#CName").text(data.msg[0].CName);
                        $("#enterednum").text(data.msg[0].EnteredNumber + "张");
                        $("#spanopencardnum").text(data.msg[0].OpenCardNum+"张");
                    }
                })
            }
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="PublishList.aspx" onfocus="this.blur()"><span>发行管理</span></a></li>
                <li><a href="Publishedit.aspx" onfocus="this.blur()">添加发行</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <span class="font_14">发行详情</span> <a href="PublishList.aspx">返回</a></h3>
                <h3>
                    &nbsp;</h3>
                <table width="780" border="0">
                    <tr>
                        <td width="212">
                            <p align="left">
                                发行标题
                            </p>
                        </td>
                        <td colspan="3">
                            <p align="left" id="issuetitle">
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td width="212">
                            <p align="left">
                                发行编号</p>
                        </td>
                        <td colspan="3">
                            <p align="left" id="issueid">
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            促销活动
                        </td>
                        <td colspan="3" id="act">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            卡种类名称
                        </td>
                        <td colspan="3" id="CName">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            发卡类型
                        </td>
                        <td colspan="3">
                            实体卡
                        </td>
                    </tr>
                    <tr>
                        <td width="212">
                            <p align="left">
                                发行渠道</p>
                        </td>
                        <td colspan="3">
                            <p align="left" id="issuechannel">
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td width="212">
                            <p align="left">
                                发行执行人</p>
                        </td>
                        <td colspan="3" id="Name">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            发行有效期
                        </td>
                        <td colspan="3">
                            查看活动详情
                        </td>
                    </tr>
                    <tr>
                        <td>
                            发行张数
                        </td>
                        <td width="249" id="Num">
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            已录入实体卡号
                        </td>
                        <td width="249" id="enterednum">
                        </td>
                        <td width="152" colspan="2">
                            <a href="enteringcard.aspx?issueid=<%=issueid %>" style="color: #0000FF"><strong>点击录入实体卡卡号</strong></a>
                        </td>
                    </tr>
                    <%--  <tr>
                        <td>
                            剩余实体卡回收
                        </td>
                        <td>
                            <a href="#"><strong>108 张 </strong></a>
                        </td>
                        <td colspan="2">
                            <input name="textfield3223" type="text" id="textfield3223" value="6006 1309 8880 0001" />
                            <span class="STYLE3">回收卡号</span>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            已开卡用户
                        </td>
                        <td colspan="3">
                            <span id="spanopencardnum"></span> <a href="MemberCardList.aspx?issueid=<%=issueid %>&isopencard=1"><strong>（点击查看列表） </strong></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            已验证消费/核销张数
                        </td>
                        <td colspan="3">
                            ---
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_issueid" value="<%=issueid %>" />
</asp:Content>
