<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ETS2.WebApp.H5.pay_by.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <style>body,html{margin:0px;}</style>
</head>
<body>
    <form id="Form1" runat="server">
        
    </form>


    <img id='wechat-tips' src='/images/alipay_wechat_tips.jpg' style='width:100%;display:none;'/>
                        <input id='submit-btn' type='submit' value='确认'/>
                  
						<script>
						    var useragent = navigator.userAgent;
						    if (useragent.indexOf('MicroMessenger') == -1) {
						        document.forms['alipaysubmit'].submit();
						    } else {
						        document.getElementById('wechat-tips').style.display = 'block';
						        document.getElementById('submit-btn').style.display = 'none';
						    }
						</script>
</body>
</html>
