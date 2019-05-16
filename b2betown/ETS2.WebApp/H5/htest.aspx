<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="htest.aspx.cs" Inherits="ETS2.WebApp.H5.htest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" style=" display:none;">
    <asp:TextBox ID="TextBox1" runat="server" Text=""></asp:TextBox>
    <asp:Button ID="Button2" runat="server" Text="确 定" OnClick="Button2_Click" />
    <asp:Label ID="Label1" runat="server" Text="Label"> </asp:Label>
    <div>
        <%--<asp:Label ID="Label1" runat="server" Text="Label">amr路径</asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" Width="568px" Text="D:/201508111913327661484.amr"></asp:TextBox><br />
        <asp:Label ID="Label2" runat="server" Text="Label">转出的mp3路径</asp:Label>
        <asp:TextBox ID="TextBox2" runat="server" Width="535px" Text="d:/1.mp3"></asp:TextBox><br />
        <asp:Button ID="Button2" runat="server" Text="转 换" onclick="Button2_Click"   /><br />
        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>--%>
    </div>
    </form>
</body>
</html>
