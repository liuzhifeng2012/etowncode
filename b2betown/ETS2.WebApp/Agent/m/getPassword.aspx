<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getPassword.aspx.cs" Inherits="ETS2.WebApp.Agent.m.getPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no;">
    <meta name="description" content="">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="format-detection" content="telephone=no">
    <title></title>
    <!-- Bootstrap core CSS -->
    <link href="/Scripts/bootstrap-3.3.4-dist/css/bootstrap.css" rel="stylesheet" />
    <!-- Custom styles for this template -->
    <link href="/agent/m/css/password-reset.css" rel="stylesheet" />
    <!-- Bootstrap core JavaScript == -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="/Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/bootstrap-3.3.4-dist/js/bootstrap.min.js"></script>
    <!--[if lt IE 9]>
    <style>
    .inpbox .tell, .inpbox .yzm{ line-height:47px;}
    .wrap-placeholder{ left:5px; top:0px;}
    </style>
    <![endif]-->
    <script type="text/javascript">
     $(function(){
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
            if ($.trim($("input:text[name=new_tel]").val()) == "") {
                alert("请输入手机号码!");
                return;
            }
            if ($.trim($("input:text[name=sms_code]").val()) == "") {
                alert("请输入手机动态密码!");
                return;
            }
            //判断验证码输入是否正确
            $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms", { mobile: $("input:text[name=new_tel]").val(),smscode:$("input:text[name=sms_code]").val(),source:"手机动态密码",Account:$.trim($("input:text[name=new_account]").val())},function(data){
            data=eval("("+data+")");
            if(data.type==1){
              alert(data.msg);
              return ;
            }
            if(data.type==100){
//              $("#get-form").submit();
                 location.href = "Default.aspx";
            }
            })
            
        }

        function checkMobile(tel) {
            tel = $.trim(tel);
            if (tel.length != 11 || tel.substr(0, 1) != 1) {
                return false;
            }
            return true;
        }

        function sendSms() {
            if ($.trim($("input:text[name=new_account]").val()) == "") {
                alert("请输入登录账户!");
                return;
            }
            if ($.trim($("input:text[name=new_tel]").val()) == "") {
                alert("请输入手机号码!");
                return;
            }
            if (!checkMobile($("input:text[name=new_tel]").val())) {
                alert("请正确输入手机号!");
                return;
            }
           
            if ($.trim($("#send_sms_btn").text()) == "获取手机动态密码") {
                $("#send_sms_btn").attr("disabled", "disabled").css("background-color", "#f4f4f4");
                _callSmsApi();
               
            }
        }

        function _sendSmsCD() {
            var sec = parseInt($("#send_sms_btn").text());
            if (sec > 1) {
                $("#send_sms_btn").text((sec - 1) + "秒后可再次发送短信");
                window.setTimeout(_sendSmsCD, 1000);
            } else {
                $("#send_sms_btn").text("获取手机动态密码");
                $("#send_sms_btn").removeAttr("disabled").css("background-color","#FFFFFF");
            }
        }

        function _callSmsApi() {
             var imgcode=$("#getcode").val();
             if(imgcode=="")
             {
               alert("请输入图形验证码!");
               $("#send_sms_btn").removeAttr("disabled").css("background-color","#FFFFFF");
               return;
             }
            $.post("/JsonFactory/AgentHandler.ashx?oper=callvalidsms", {imgcode:imgcode, mobile: $("input:text[name=new_tel]").val(),comid:<%=comid %>,source:"手机动态密码",account:$("input:text[name=new_account]").val() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    $("#send_sms_btn").removeAttr("disabled").css("background-color","#FFFFFF");
                    //location.reload();
                }
                if (data.type == 100) { 
                 $("#send_sms_btn").text("30秒后可再次发送短信");
                 window.setTimeout(_sendSmsCD, 1000);
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
             
            $("#send_sms_btn")
                .click(function (event) {
                    stopDefault(event);
                    sendSms();
                });
             
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
            手机动态密码登录</h1>
        <ul>
          <li>
                <input type="text" name="new_account" placeholder="登陆账户" class="wx_input_s" value="" /></li>
            <li>
                <input type="text" name="new_tel" placeholder="手

机号码" class="wx_input_s" value="" /></li>
 <li>
                <input type="text" name="getcode"  id="getcode" placeholder="图形验证码" class="wx_input_s" value=""  style="width: 60%;" />
                <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码"  class="wx_button_s1"/>
                <%--<a href="javascript:;" id="validateCodetext">更换</a>--%>
                </li>
            <li>
                <input type="text" name="sms_code" placeholder="手机动态密码" class="wx_input_s"  style="width:60%;"/>
 <button id="send_sms_btn" class="wx_button_s1">
                            获取手机动态密码</button> 
</li>
        </ul>
        <div>
          <button id="getPsw"  class="wx_button_s  btnSubmit">
                        登录</button>
            
        </div>
        <div class="butbox">
            <div class="text-center lg">
                <a href="/agent/m/login.aspx">己有帐号？立即登录<i></i></a></div>
        </div>
    </div>
</body>
</html>