<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="native.aspx.cs" Inherits="ETS2.WebApp.wxpay.native" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <center><a href="<%= parm %>">点击支付(微信浏览器)</a><br>扫描支付</br><img src="<%= parm%>" alt="QR code"/></center>
    </div>
    </form>
</body>
</html>
