<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="RePrintEticket.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ETicket.RePrintEticket" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()"
                    target=""><span>电脑验码</span></a></li>
                <li ><a href="/ui/pmui/eticket/ETicketPos.aspx" onfocus="this.blur()" target="">
                                    Pos验证</a></li>
                <li><a href="/ui/pmui/eticket/eticketlist.aspx" onfocus="this.blur()" target="">
                    <span>验码明细</span></a></li>
                <li><a href="dayjiesuan.aspx" onfocus="this.blur()" target=""><span>
                    每日结算</span></a></li>
            </ul>
        </div>

        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    重打最后一张电子票</h3>
                <table class="grid">
                    <tr>
                        <td>
                           
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="48" colspan="4" align="center">
                                        <h2>
                                            &quot;<strong>最后验证小票</strong>&quot;重打</h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="22%" height="31" align="right" bgcolor="#00CC33">
                                        服务内容：
                                    </td>
                                    <td height="31" colspan="3" bgcolor="#00CC33" class="STYLE4">
                                        <strong>最后验证小票</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="27" align="right" bgcolor="#00CC33">
                                        使用数量：
                                    </td>
                                    <td width="24%" height="27" bgcolor="#00CC33">
                                        <span class="STYLE9">1</span> 张
                                    </td>
                                    <td width="46%" colspan="2" bgcolor="#00CC33" class="STYLE8">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <p>
                            </p>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </div>
    <div class="data">
    </div>
</asp:Content>
