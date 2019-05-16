<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="h5test112.aspx.cs" Inherits="ETS2.WebApp.H5.h5test112" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title></title> 
</head>
<body> 
  <form id="form1" runat="server">
    <div style="display:none;"> 
        操作:<asp:TextBox ID="t_caozuo" runat="server" ></asp:TextBox>
        <br />
        <br />
        发送内容:<asp:TextBox ID="t_xml" runat="server" Height="368px" TextMode="MultiLine" Width="1025px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">承保</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton2" runat="server" onclick="LinkButton2_Click">退保</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton3" runat="server" onclick="LinkButton3_Click">保单下载</asp:LinkButton>
          <br />
        <asp:LinkButton ID="LinkButton4" runat="server" onclick="LinkButton4_Click">投保单查询</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton5" runat="server" onclick="LinkButton5_Click">批量查询保单（分页）</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton6" runat="server" onclick="LinkButton6_Click">投保单详情</asp:LinkButton>
    </div>
    </form>
</body>
</html>
