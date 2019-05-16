<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ETS2.WebApp.TicketService.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom/ui/jquery.ui.accordion.js" type="text/javascript"></script>
    <link href="/Scripts/jquery-ui-1.10.3.custom/demos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        //collapsible: true不设这个属性的话，默认总会有一个展开；
        //设为true就可以全部折叠起来
        $(function () {
            $("#accordion").accordion({
                collapsible: true
            });
        });
   
    </script>
</head>
<body>
    <div class="demo" style="display:none;">
        <div id="accordion">
            <h3>
                <a href="#">第1个菜单 1</a></h3>
            <div>
                <p>
                    第1个菜单 1的内容</p>
            </div>
            <h3>
                <a href="#">第2个菜单 2</a></h3>
            <div>
                <p>
                    第2个菜单 2的内容
                </p>
            </div>
            <h3>
                <a href="#">第3个菜单 3</a></h3>
            <div>
                <p>
                    第3个菜单 3的内容
                </p>
                <ul>
                    <li>List item one</li>
                    <li>List item two</li>
                    <li>List item three</li>
                </ul>
            </div>
            <h3>
                <a href="#">第4个菜单 4</a></h3>
            <div>
                <p>
                    第4个菜单 4的内容</p>
            </div>
        </div>
    </div>
    <form id="form1" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <asp:TextBox ID="TextBox1" runat="server">电子码</asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="测试去哪消费通知" OnClick="Button1_Click" />
    <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Unnamed1_Click" />
    <br />
    <br />
    <br />
    <asp:TextBox ID="Transaction_id" runat="server">微信订单号</asp:TextBox><br />
    <asp:TextBox ID="Out_trade_no" runat="server">商户系统内部的订单号</asp:TextBox><br />
    <asp:TextBox ID="Out_refund_no" runat="server">商户退款单号</asp:TextBox><br />
    <asp:TextBox ID="Total_fee" runat="server">总金额</asp:TextBox>单位:分<br />
    <asp:TextBox ID="Refund_fee" runat="server">退款金额</asp:TextBox>单位:分<br />
    <asp:Button ID="Button3" runat="server" Text="微信退款" OnClick="Button3_Click" />
    <br />
    <br />
    <br />
    <asp:TextBox ID="TextBox2" runat="server">验证电子码</asp:TextBox><br />
    <asp:Button ID="Button4" runat="server" Text="验证通知" OnClick="Button4_Click" />
    <br />
    <br />
    <asp:TextBox ID="txtproid" runat="server">产品id</asp:TextBox><br />
    <asp:TextBox ID="txtnum" runat="server">数量</asp:TextBox><br />
    <asp:TextBox ID="txtprovince" runat="server">省份</asp:TextBox><br />
    <asp:TextBox ID="txtcity" runat="server">城市</asp:TextBox><br />
    <asp:Button ID="Button5" runat="server" Text="计算运费" onclick="Button5_Click" />
    </form>
    <!-- End demo -->
</body>
</html>
