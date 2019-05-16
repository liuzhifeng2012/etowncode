<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendToMT.aspx.cs" Inherits="ETS2.WebApp.Meituan_jk.SendToMT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="display: ;">
        产品变化通知发送内容:<asp:TextBox ID="TextBox1" runat="server" Height="368px" TextMode="MultiLine" Width="1025px"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="发送产品变化通知" OnClick="Button1_Click" />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

        消费通知发送内容:<asp:TextBox ID="TextBox2" runat="server" Height="368px" TextMode="MultiLine" Width="1025px"></asp:TextBox><br />
        <asp:Button ID="Button2" runat="server" Text="发送消费通知" OnClick="Button2_Click" />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>



        <%--操作:<asp:TextBox ID="t_caozuo" runat="server" Style="min-width: 500px;"></asp:TextBox>
        <br />
        <br />
        发送内容:<asp:TextBox ID="t_xml" runat="server" Height="368px" TextMode="MultiLine" Width="1025px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">产品变化通知</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">推送deal到美团旅游</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click">推送POI景点信息</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton4" runat="server" OnClick="LinkButton4_Click">推送消费通知</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton5" runat="server" OnClick="LinkButton5_Click">推送退款通知(我们退款不需审核，所以退款通知不需要)</asp:LinkButton>
        <br />--%>
    </div>
    </form>
</body>
</html>
