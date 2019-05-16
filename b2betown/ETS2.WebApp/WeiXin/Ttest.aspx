<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ttest.aspx.cs" Inherits="ETS2.WebApp.WeiXin.Ttest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>测试页面</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            if (isWeiXin() && '<%=scopeurl %>' != '') {
                window.open('<%=scopeurl %>', target = '_self');
            }
        })
    </script>
    <script type="text/javascript">
        //判断微信版本,微信版本5.0以上
        function isWeiXin() {
            var ua = window.navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == 'micromessenger' && parseFloat(navigator.appVersion) >= 5) {
                return true;
            } else {
                return false;
            }
        }

         
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <label>
            时间， <%=currentTime%></label>
        <label>
            签名：<%=d1  %></label>
        <label>
           返回: <%=d2 %></label>
        <asp:Button ID="Button1" runat="server" Text="清除cookie" 
            onclick="Button1_Click" />
    </div>
    </form>
</body>
</html>
