<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResendVerifyNotice.aspx.cs"
    Inherits="ETS2.WebApp.UI.BusinessesUI.ResendVerifyNotice" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/ui/BusinessesUI/ResendVerifyNotice.aspx" onfocus="this.blur()"
                    target=""><span>重新发送验证通知</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <dl class="content">
                    <dt>订单号/电子码：</dt>
                    <dd>
                        <span class="null-star">*</span>
                        <asp:TextBox ID="txtkey" runat="server"></asp:TextBox>
                        <span>必填 </span>
                    </dd>
                    <dd>
                        <span class="new-btn-login-sp">
                            <asp:Button ID="Button1" Text="重新发送消费通知" Style="text-align: center;" runat="server"
                                OnClick="Button1_Click" /></span>
                        <label runat="server" id="Label222">
                        </label>
                    </dd>
                </dl>
            </div>
        </div>
    </div>
</asp:Content>
