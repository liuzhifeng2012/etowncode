<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ETS2.WebApp.yanzheng._default"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no;">
    <meta name="description" content="">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="format-detection" content="telephone=no">
    <title></title>
    <!-- Bootstrap core CSS -->
   <%-- <link href="/Scripts/bootstrap-3.3.4-dist/css/bootstrap.css" rel="stylesheet" />--%>
    <!-- Custom styles for this template -->
    <link href="/agent/m/css/password-reset.css" rel="stylesheet" />
    <!-- Bootstrap core JavaScript == -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
  <%--  <script src="/Scripts/bootstrap-3.3.4-dist/js/bootstrap.min.js"></script>--%>
    <!--[if lt IE 9]>
    <style>
    .inpbox .tell, .inpbox .yzm{ line-height:47px;}
    .wrap-placeholder{ left:5px; top:0px;}
    </style>
    <![endif]-->
    <script type="text/javascript">
        $(function () {
            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#getPsw").click();    //这里添加要处理的逻辑  
                }
            });
            //验证码
            $("#validateCodetext").click(function () {
                $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })
            $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })

        })
        function checkForm() {
            if ($.trim($("input:text[name=new_account]").val()) == "") {
                alert("请输入登录账户!");
                return;
            }
            if ($.trim($("input:text[name=new_pass]").val()) == "") {
                alert("请输入密码!");
                return;
            }
            if ($.trim($("#getcode").val()) == "") {
                alert("请输入图形验证码!");
                return;
            }

            $.post("/JsonFactory/RegisterUser.ashx?oper=login", { ts: Math.random(), username: $.trim($("input:text[name=new_account]").val()), pwd: $.trim($("input:text[name=new_pass]").val()), getcode: $.trim($("#getcode").val()) }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                    return;
                }
                else {
                    window.open("/ui/pmui/ETicket/mETicketIndex.aspx", target = "_self");
                }
            })
        }

        /*取消事件的默认动作*/
        function stopDefault(e) {

            if (e && e.preventDefault) {//如果是FF下执行这个

                e.preventDefault();

            } else {

                window.event.returnValue = false; //如果是IE下执行这个

            }

            return false;
        }
        $(function () {
            $("#getPsw")
                .click(function (event) {
                    stopDefault(event);
                    checkForm();
                });
        }) 
    </script>
</head>
<body>
    <div class="weixin_bangding">
        <h1 style="text-align: center; font-size: 24px; margin: 20px 0px;">
            易城商户系统手机验证登录</h1>
       
        <ul>
            <li>
                <input type="text" name="new_account" placeholder="登陆账户" class="wx_input_s" value="" /></li>
            <li>
                <input type="text" name="new_pass" placeholder="密码" class="wx_input_s" value="" /></li>
            <li>
                <input type="tel" name="getcode" id="getcode" placeholder="图形验证码" class="wx_input_s"
                    value="" style="width: 60%;" />
                <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码"
                    class="wx_button_s1" />
                <%--<a href="javascript:;" id="validateCodetext">更换</a>--%>
            </li>
        </ul>
        <div>
            <button id="getPsw" class="wx_button_s  btnSubmit">
                登录</button>
        </div>
        
    </div>
</body>
</html>
