<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="phoneverify.aspx.cs" Inherits="ETS2.WebApp.Agent.m.phoneverify" %>

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
    <script src="http://shop.etown.cn/Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
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
            if ($.trim($("input:text[name=new_tel]").val()) == "") {
                alert("请输入手机号码!");
                return;
            }
            if ($.trim($("input:text[name=sms_code]").val()) == "") {
                alert("请输入短信验证码!");
                return;
            }
            //判断验证码输入是否正确
            $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms", { mobile: $("input:text[name=new_tel]").val(),smscode:$("input:text[name=sms_code]").val(),source:"分销注册验证码"},function(data){
            data=eval("("+data+")");
            if(data.type==1){
              alert("验证码不相符");
              return ;
            }
            if(data.type==100){
              $("#get-form").submit();
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
           

            if ($.trim($("input:text[name=new_tel]").val()) == "") {
                alert("请输入手机号码!");
                return;
            }
            if (!checkMobile($("input:text[name=new_tel]").val())) {
                alert("请正确输入手机号!");
                return;
            }

             var imgcode=$("#getcode").val();
             if(imgcode=="")
             {
               alert("请输入图形验证码!");
               return;
             }
              //判断图形验证码是否正确
             $.post("/JsonFactory/RegisterUser.ashx?oper=verifyimgcode",{imgcode:imgcode},function(dd){
                dd=eval("("+dd+")");
                if(dd.type==1){
                    alert("图形验证码输入不正确");
                    $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                   return;
                }
                else{
                    if ($.trim($("#send_sms_btn").text()) == "获取短信验证码") {
                        $("#send_sms_btn").attr("disabled", "disabled").css("background-color", "#f4f4f4");
                        _callSmsApi();
                
                    }
                }
            })
             
        }

        function _sendSmsCD() {
            var sec = parseInt($("#send_sms_btn").text());
            if (sec > 1) {
                $("#send_sms_btn").text((sec - 1) + "秒后可再次发送短信");
                window.setTimeout(_sendSmsCD, 1000);
            } else {
                $("#send_sms_btn").text("获取短信验证码");
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
            $.post("/JsonFactory/AgentHandler.ashx?oper=callvalidsms", {imgcode:imgcode, mobile: $("input:text[name=new_tel]").val(),comid:<%=comid %>,source:"分销注册验证码" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    $("#send_sms_btn").removeAttr("disabled").css("background-color","#FFFFFF");
                    return;
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
            /*获取短信验证码*/
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
            分销商注册</h1>
        <form action="/agent/m/register.aspx" method="post" id="get-form">
        <ul>
       
            <li>
                <input type="text" name="new_tel" placeholder="手机号码" class="wx_input_s" value="" /></li>  
                <li>
                <input type="text" name="getcode"  id="getcode" placeholder="图形验证码" class="wx_input_s" value=""  style="width: 60%;" />
                <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码"  class="wx_button_s1"/>
                <%--<a href="javascript:;" id="validateCodetext">更换</a>--%>
                </li>
           
            <li>
                <input type="text" name="sms_code" placeholder="短信验证码" class="wx_input_s" style="width: 60%;" />
                <button id="send_sms_btn" class="wx_button_s1">
                    获取短信验证码</button>
            </li>
        </ul>
        <div>
            <button id="getPsw" class="wx_button_s  btnSubmit">
                提交</button>
        </div>
        </form>
        <div class="butbox">
            <div class="text-center lg">
                <a href="/agent/m/login.aspx">己有帐号？立即登录<i></i></a></div>
        </div>
    </div>
</body>
</html>
