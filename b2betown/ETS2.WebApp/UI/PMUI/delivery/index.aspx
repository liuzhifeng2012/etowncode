<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.delivery.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#bt").click(function () {
                $.post("/JsonFactory/OrderHandler.ashx?oper=getshopcartexpressfee", { proidstr: $("#proidstr").val(), numstr: $("#numstr").val(), citystr: $("#citystr").val() }, function (data) {
                    data = eval("(" + data + ")");
                    $("#lblmsg").text(data.msg + "<br>" + data.feedetail);
                })

            })
     
        })
      
    </script>
    <style type="text/css">
        label
        {
            display: inline-block;
            padding: 0 10px;
            vertical-align: middle;
        }
        
        input
        {
            outline: none;
            border: 1px solid rgb(216, 216, 216);
            padding: 2px 10px 2px 10px;
        }
        
        input[type="text"]
        {
            height: 14px;
            line-height: 14px;
            border-radius: 5px;
            padding: 10px 10px;
            vertical-align: middle;
            color: #666;
        }
        input[type="button"]
        {
            padding: 0px 10px;
            height: 40px;
        }
        
        .login-box p
        {
            vertical-align: middle;
            padding: 10px 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <label>
            产品字符串</label>
        <input type="text" id="proidstr" value="1083,1086,1088" /><br />
        <label>
            城市字符串</label>
        <input type="text" id="citystr" value="南京市,南京市,南京市" /><br />
        <label>
            数量字符串</label>
        <input type="text" id="numstr" value="1,2,3" /><br />
        <input type="button" id="bt" value="计算运费"  /><br />
        <label id="lblmsg" style="color:Red;">
        </label>
    </div>
    </form>
</body>
</html>
