<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ETS2.WebApp.H5.order.Login" %>

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
    <link href="/Scripts/bootstrap-3.3.4-dist/css/bootstrap.css" rel="stylesheet" />
    <link href="/agent/m/css/password-reset.css" rel="stylesheet" />
    <script src="/Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/bootstrap-3.3.4-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript">
     $(function(){
        //验证码
            $("#validateCodetext").click(function () {
                $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })
             $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })

            $("#send_sms_btn").click(function (event) {
                    stopDefault(event);
                    sendSms();
                });
             
            $("#getPsw").click(function (event) {
                    stopDefault(event);
                    checkForm();
                });
       
       })
        function checkForm() {

            if ($.trim($("#new_tel").val()) == "") {
                alert("请输入手机号码!");
                 $("#send_sms_btn").removeAttr("disabled").css("background-color","#FFFFFF");
                return;
            }
            if ($.trim($("#sms_code").val()) == "") {
                alert("请输入手机动态密码!");
                 $("#send_sms_btn").removeAttr("disabled").css("background-color","#FFFFFF");
                return;
            }
            //判断验证码输入是否正确
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=SmsphoneLogin", { mobile: $("#new_tel").val(),smscode:$("#sms_code").val(),source:"用户短信登录",comid:<%=comid %>},function(data){
            data=eval("("+data+")");
            if(data.type==1){
              alert(data.msg);
               $("#send_sms_btn").removeAttr("disabled").css("background-color","#FFFFFF");
              return ;
            }
            if(data.type==100){
//              $("#get-form").submit();

                var come="<%=come %>";

                if(come !=""){
                 location.href = come;
                }else{
                 location.href = "Order.aspx";
                }
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

            if ($.trim($("#new_tel").val()) == "") {
                alert("请输入手机号码!");
                  $("#send_sms_btn").removeAttr("disabled").css("background-color","#FFFFFF");
                return;
            }
            if (!checkMobile($("#new_tel").val())) {
                alert("请正确输入手机号!");
                  $("#send_sms_btn").removeAttr("disabled").css("background-color","#FFFFFF");
                return;
            }
           
            if ($.trim($("#send_sms_btn").text()) == "获取动态密码") {
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
                $("#send_sms_btn").text("获取动态密码");
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
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=SendSmsLogin", {imgcode:imgcode, mobile: $("#new_tel").val(),comid:<%=comid %>,source:"手机动态密码" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    location.reload();
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

 


    </script>
</head>
<body style="background-color: #F9F9F9;">
      <div class="">
        <h1 class="form-title big">
            请先登录账号 </h1>


            <ul class="block block-form">
            <li class="block-item">
                <label for="phone">手机号码</label>
                <input id="new_tel" name="new_tel" maxlength="11" class="js-login-phone" autocomplete="off" placeholder="请输入您的手机号" value="" type="tel">
            </li>
            <li class="block-item hide" style="margin-bottom: -1px">
                <label for="password">登录密码</label>
                <input id="password" name="password" autocomplete="off" class="js-login-pwd" placeholder="请填写您的密码" type="password">
            </li>
            <li class="js-image-verify relative block-item">
                <label>图形验证码</label>
                <input  id="getcode" name="getcode" class="js-verify-code" placeholder="请输入图像验证码" maxlength="6" type="tel">
                <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码"  class="wx_button_s1 js-verify-image verify-image"/>
            </li>
            <li class="relative block-item js-auth-code-li " style="margin-top: 1px">
                <label for="code">动态密码</label>
                <input id="sms_code" name="sms_code" class="js-auth-code" placeholder="请输入动态密码" maxlength="6" type="tel">
                <button id="send_sms_btn" type="button" class="tag tag-green btn-auth-code font-size-12 js-auth-code-btn" data-text="获取动态密码">
                    获取动态密码
                </button>
            </li>
        </ul>


         <div class="action-container">
          <button id="getPsw"  class="js-submit btn btn-green btn-block">
                        登录</button>
            
        </div>
        <div class="action-links hide">
            <p class="center">
                 <a class="js-login-mode c-blue" data-login-mode="signup" href="javascript:;">手机短信验证登陆</a>
               
            </p>
        </div>

    </div>
    <input type="hidden" id="hid_logintype" value="0" />
</body>
</html>

