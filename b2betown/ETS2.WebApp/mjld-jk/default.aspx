<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ETS2.WebApp.mjld_jk._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="display:none;">
        操作:<asp:TextBox ID="t_caozuo" runat="server"></asp:TextBox>
        <br />
        <br />
        发送内容:<asp:TextBox ID="t_xml" runat="server" Height="368px" TextMode="MultiLine" Width="1025px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">2.5、	提交订单</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">2.6、	订单浏览</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click">2.7、	短信重发</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton4" runat="server" OnClick="LinkButton4_Click">2.8、	查询验证码信息</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton5" runat="server" OnClick="LinkButton5_Click">2.9、	退单</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton6" runat="server" OnClick="LinkButton6_Click">2.10、	订单整单快速退单</asp:LinkButton>
        <br />
         
         <asp:LinkButton ID="LinkButton7" runat="server" OnClick="LinkButton7_Click">2.1、	查询地区</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton8" runat="server" OnClick="LinkButton8_Click">2.2、	查询主题</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton9" runat="server" OnClick="LinkButton9_Click">2.3、	查询产品列表</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton10" runat="server" OnClick="LinkButton10_Click">2.4、	查询产品详情</asp:LinkButton> 
    </div>
    </form>
</body>
</html>
