<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MTSendToMe.aspx.cs" Inherits="ETS2.WebApp.Meituan_jk.MTSendToMe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>模拟美团向我们发送请求</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="display:none ;">
        操作:<asp:TextBox ID="t_caozuo" runat="server" Style="display: none;"></asp:TextBox>
        <br />
        <br />
        发送内容:<asp:TextBox ID="t_xml" runat="server" Height="368px" TextMode="MultiLine" Width="1025px"></asp:TextBox>
        <br />
        <br />
        <asp:LinkButton ID="LinkButton8" runat="server" OnClick="LinkButton8_Click">拉取账户余额接口</asp:LinkButton>
        <asp:Button ID="Button8" runat="server" Text="Button" OnClick="Button8_Click" />
        <br />
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">拉取Poi接口</asp:LinkButton>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        <br />
        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">拉取Deal接口</asp:LinkButton>
        <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
        <br />
          <asp:LinkButton ID="LinkButton7" runat="server" OnClick="LinkButton7_Click">拉取价格日历接口</asp:LinkButton>
        <asp:Button ID="Button7" runat="server" Text="Button" OnClick="Button7_Click" />
        <br />
        <asp:LinkButton ID="LinkButton5" runat="server" OnClick="LinkButton5_Click">订单创建接口</asp:LinkButton>
        <asp:Button ID="Button5" runat="server" Text="Button" OnClick="Button5_Click" />
        <br />
        <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click">订单支付接口</asp:LinkButton>
        <asp:Button ID="Button3" runat="server" Text="Button" OnClick="Button3_Click" />
        <br />
        <asp:LinkButton ID="LinkButton4" runat="server" OnClick="LinkButton4_Click">订单查询接口</asp:LinkButton>
        <asp:Button ID="Button4" runat="server" Text="Button" OnClick="Button4_Click" />
        <br />
        
        <asp:LinkButton ID="LinkButton6" runat="server" OnClick="LinkButton6_Click">订单退款接口</asp:LinkButton>
        <asp:Button ID="Button6" runat="server" Text="Button" OnClick="Button6_Click" />
        <br />

        <asp:LinkButton ID="LinkButton9" runat="server" OnClick="LinkButton9_Click">合作方心跳接口测试</asp:LinkButton>
        <asp:Button ID="Button9" runat="server" Text="Button" OnClick="Button9_Click" />
        <br />
        <asp:LinkButton ID="LinkButton10" runat="server" OnClick="LinkButton10_Click">产品编审状态通知接口测试</asp:LinkButton>
        <asp:Button ID="Button10" runat="server" Text="Button" OnClick="Button10_Click" />
        <br />
        <asp:LinkButton ID="LinkButton11" runat="server" OnClick="LinkButton11_Click">美团已退款消息通知接口测试(这种情况基本不会出现)</asp:LinkButton>
        <asp:Button ID="Button11" runat="server" Text="Button" OnClick="Button11_Click" />
        <br />
        
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
