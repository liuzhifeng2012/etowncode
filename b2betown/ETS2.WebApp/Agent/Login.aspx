<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Agent.Master" CodeBehind="Login.aspx.cs" Inherits="ETS2.WebApp.Agent.Login" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
     a:hover{ text-decoration:underline;}
 .btn_code
{
    line-height:25px;
    text-align:center;
    display:inline-block;
    padding:0 5px;
    color:#6d6d6d;
    border:1px solid #d1d1d1;
    background-color:#e6e6e6;
    background-image:-webkit-gradient(linear,left top,left bottom,from(#f8f8f8),to(#e6e6e6));
    background-image:-webkit-linear-gradient(top,#f8f8f8,#e6e6e6);
    background-image:linear-gradient(to bottom,#f8f8f8,#e6e6e6);
    border-radius:5px;
 }
 .btn_code.disabled
 {
     color:#FFF;
     background-color:#c3c3c3;
     background-image:-webkit-gradient(linear,left top,left bottom,from(#d0d0d0),to(#c3c3c3));
     background-image:-webkit-linear-gradient(top,#d0d0d0,#c3c3c3);
     background-image:linear-gradient(to bottom,#d0d0d0,#c3c3c3)
  }
 </style>
  <style type="text/css">

.modal {
position: fixed;
top: 10%;
left: 50%;
z-index: 1050;
width: 560px;
margin-left: -280px;
background-color: white;
border: 1px solid #999;
-webkit-border-radius: 6px;
-moz-border-radius: 6px;
border-radius: 6px;
-webkit-box-shadow: 0 3px 7px rgba(0, 0, 0, 0.3);
-moz-box-shadow: 0 3px 7px rgba(0, 0, 0, 0.3);
box-shadow: 0 3px 7px rgba(0, 0, 0, 0.3);
-webkit-background-clip: padding-box;
-moz-background-clip: padding-box;
background-clip: padding-box;
outline: none;
}
.modal-header {
padding: 5px 15px;
}
button.close {
padding: 0;
cursor: pointer;
background: transparent;
border: 0;
-webkit-appearance: none;
float:right;
}
.modal-header h3 {
margin: 0;
line-height: 30px;
}

</style>
 <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
 <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
 <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/Impromptu.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
 <link rel="stylesheet" href="Styles/login.css" type="text/css" />

    <script type="text/javascript">
        $(function () {
            //图形验证码
            $("#rvalidateCodetext").click(function () {
                $("#rvalidateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })
            $("#rvalidateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })

            var comid = $("#hid_comid").trimVal();
            comid = "106";
            //判断密码
            $("#Account").blur(function () {
                $("#AccountVer").html(""); //离开后先清空备注
                var Account = $("#Account").val();
                if (Account == "") {
                    $("#AccountVer").html("X");
                    $("#AccountVer").css("color", "red");
                    return;
                } else {
                    $("#AccountVer").html("√");
                    $("#AccountVer").css("color", "green");

                }
            })

            //判断密码
            $("#Pwd").blur(function () {
                $("#PwdVer").html(""); //离开后先清空备注
                var Pwd = $("#Pwd").val();
                if (Pwd == "") {
                    $("#PwdVer").html("X");
                    $("#PwdVer").css("color", "red");
                    return;
                } else {
                    $("#PwdVer").html("√");
                    $("#PwdVer").css("color", "green");

                }
            })

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#btn-login").click();    //这里添加要处理的逻辑  
                }
            });

            $("#phoneshow").click(function () {
                $("#passlog").css("display", "none");
                $("#passlog").removeClass("ui - form - group");
                $("#passlog").addClass("ui - form - group");
                $("#phonelog").css("display", "block");
                $("#hid_num").val(1);
            })
            $("#logshow").click(function () {
                $("#phonelog").css("display", "none");
                $("#phonelog").removeClass("ui - form - group");
                $("#passlog").css("display", "block");
                $("#hid_num").val(0);
            })

            //提交按钮
            $("#btn-login").click("click", function () {

                var Account = $("#Account").val();
                var Pwd = $("#Pwd").val();
                var getcode = $("#getcode").val();
                if (Account == "") {
                    $("#AccountVer").html("X");
                    $("#AccountVer").css("color", "red");
                    $("#login_err").html("请输入账户名");
                    $("#login_err").css("color", "red");
                    return;
                }
                else {
                    $("#AccountVer").html("√");
                    $("#AccountVer").css("color", "green");
                    $("#login_err").html("");
                }

                if (Pwd == "") {
                    $("#PwdVer").html("X");
                    $("#PwdVer").css("color", "red");
                    $("#login_err").html("请输入登录密码");
                    $("#login_err").css("color", "red");
                    return;
                } else {
                    $("#PwdVer").html("√");
                    $("#PwdVer").css("color", "green");
                    $("#login_err").html("");

                }

                if (getcode == "") {
                    $("#login_err").html("请填写验证吗");
                    $("#login_err").css("color", "red");

                    return;
                }

                $(".loading-text").html("正在登陆信息，请稍后...")


                $.post("/JsonFactory/AgentHandler.ashx?oper=Login", { comid: comid, Account: Account, Pwd: Pwd, getcode: getcode }, function (data) {
                    $("#login_err").html("");
                    if (data == "OK") {
                        location.href = "Default.aspx";
                    } else {
                        $("#login_err").html(data);
                        $("#login_err").css("color", "red");
                        $(".loading-text").html("")
                        $("#getcode").val("");
                        $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                        return;
                    }


                })
            })
        })

    </script>

     <script type="text/javascript">
         $(function () {
             var comid = $("#hid_comid").trimVal();
             comid = "106";
             $("#regia").click(function () {
                 $("#regiagent").show();
             })
             $("#closeregiagent").click(function () {
                 $("#regiagent").hide();
             })

             //账号名
             $("#RAgent_Email").blur(function () {
                 $("#EmailVer").html(""); //离开后先清空备注
                 var Email = $("#RAgent_Email").val();
                 //判断邮箱不为空
                 if (Email != "") {
                     $.post("/JsonFactory/AgentHandler.ashx?oper=getEmail", { Email: Email, comid: comid }, function (data) {
                         data = eval("(" + data + ")");
                         if (data.type == 100) {
                             if (data.msg == "OK") {
                                 //                                 $("#EmailVer").html("√");
                                 //                                 $("#EmailVer").css("color", "green");
                                 //                                 $("#VEmail").val(1);
                             } else {
                                 alert(data.msg);
                                 $("#RAgent_Email").val("");
                                 return;
                             }

                         }
                     })
                 }
             })

             //账号名
             $("#RAgent_Phone").blur(function () {
                 $("#PhoneVer").html(""); //离开后先清空备注
                 var Phone = $("#RAgent_Phone").val();
                 //判断手机不为空
                 if (Phone != "") {
                     $.post("/JsonFactory/AgentHandler.ashx?oper=getPhone", { Phone: Phone, comid: comid }, function (data) {
                         data = eval("(" + data + ")");
                         if (data.type == 100) {
                             if (data.msg == "OK") {
                             } else {
                                 alert(data.msg);
                                 $("#RAgent_Phone").val("");
                                 return;
                             }

                         }
                     })
                 }
             })

             //判断密码
             $("#RAgent_QPassword1").blur(function () {
                 $("#QPassword1Ver").html(""); //离开后先清空备注
                 var QPassword1 = $("#RAgent_QPassword1").val();
                 var Password1 = $("#RAgent_Password1").val();
                 if (QPassword1 == "" || QPassword1 != Password1) {
                     alert("再次填写密码错误");
                     $("#RAgent_QPassword1").val("");
                     return false;
                 }
             })


             //提交按钮
             $("#RAgent_regisub").click(function () {
                 var Email = $("#RAgent_Email").val();
                 var Password1 = $("#RAgent_passwords").val();
                 var QPassword1 = $("#RAgent_qpasswords").val();
                 var Name = $("#RAgent_Name").val();
                 var Phone = $("#RAgent_Phone").val();
                 var phonecode = $("#RAgent_phonecode").val();

                 var Company = $("#RAgent_Email").val();

                 if (Email == "") {
                     alert("请填账户");
                     return;
                 }
                 if ($("#RAgent_VEmail").val() == 0) {
                     alert("账户有误!");
                     return;
                 }
                 if (Password1 == "") {
                     alert("请填写密码!");
                     return;
                 }
                 if (QPassword1 == "") {
                     alert("请再次确认密码!");
                     return;
                 }

                 if (QPassword1 != Password1) {
                     alert("两次密码输入不符！");
                     return;
                 }

                 if ($("#RAgent_VQPassword1").val() == 0) {
                     alert("密码有误!");
                     return;
                 };

                 if (Name == "") {
                     alert("请填写姓名!");
                     return;
                 };

                 if (Phone == "") {
                     alert("请填写手机!");
                     return;
                 };
                 if ($("#RAgent_VPhone").val() == 0) {
                     alert("手机有误!");
                     return;
                 };
                 if (phonecode == "") {
                     alert("请填写短信验证码!");
                     return;
                 };

                 //判断验证码输入是否正确
                 $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms", { mobile: $("#RAgent_Phone").val(), smscode: $("#RAgent_phonecode").val(), source: "分销注册验证码" }, function (data) {
                     data = eval("(" + data + ")");
                     if (data.type == 1) {
                         alert("验证码不相符");
                         return;
                     }
                     if (data.type == 100) {


                         $("#loading").html("正在提交注册信息，请稍后...")

                         //创建订单
                         $.post("/JsonFactory/AgentHandler.ashx?oper=Agentregi", { agentsourcesort: 0, Email: Email, Password1: Password1, Name: Name, Tel: Phone, Phone: Phone, Company: Company }, function (data1) {
                             data1 = eval("(" + data1 + ")");
                             if (data1.type == 1) {
                                 $.prompt("注册出现错误，请刷新重新提交！");
                                 return;
                             }
                             if (data1.type == 100) {
                                 if (data1.msg == "OK") {
                                     alert(" 您已注册成功，请等待商家为您授权！");
                                     location.href = "/Agent/Login.aspx?Email=" + Email
                                     return;
                                 }
                                 else {
                                     $.prompt("参数传递出错，请重新操作");
                                     return;
                                 }
                             }
                         })

                     }
                 })

             })

             //获取手机验证码
             $("#RAgent_getphonecode").click(function () {

                 if ($.trim($("#RAgent_Phone").val()) == "") {
                     alert("请输入手机号码!");
                     return;
                 }
                 if (!checkMobile($("#RAgent_Phone").val())) {
                     alert("请正确输入手机号!");
                     return;
                 }

                 if ($.trim($("#RAgent_getphonecode").html()) == "获取短信验证码") {
                     $("#RAgent_getphonecode").attr("disabled", "disabled").css("background-color", "#f4f4f4");
                     _callSmsApi();

                 }
             })
         })
       
         function checkMobile(tel) {
            tel = $.trim(tel);
            if (tel.length != 11 || tel.substr(0, 1) != 1) {
                return false;
            }
            return true;
        }
        function _sendSmsCD() {
            var sec = parseInt($("#RAgent_getphonecode").html());
            if (sec > 1) {
                $("#RAgent_getphonecode").html((sec - 1) + "秒后可再次发送短信");
                window.setTimeout(_sendSmsCD, 1000);
            } else {
                $("#RAgent_getphonecode").html("获取短信验证码");
                $("#RAgent_getphonecode").removeAttr("disabled").css("background-color","#FFFFFF");
            }
        }

        function _callSmsApi() {
            var imgcode = $("#rgetcode").trimVal();
            if (imgcode == "") {
                alert("请输入图形验证码!");
                return;
            }
            $.post("/JsonFactory/AgentHandler.ashx?oper=callvalidsms", { imgcode: imgcode, mobile: $("#RAgent_Phone").val(), comid:106, source: "分销注册验证码" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                }
                if (data.type == 100) { 
                $("#RAgent_getphonecode").html("30秒后可再次发送短信");
                window.setTimeout(_sendSmsCD, 1000);
                }
            })

        }
    </script>


</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
 <div class="grid-780-border fn-clear">
 

    <div id="header">
    <div class="logo1"></div>
</div> 

    <!--artist end-->

<div class="login_box clear">
 <div class="login_inputs">

   <div class="login_M clear">
     <h1 class="c33"><strong>分销账户登陆</strong></h1>

     <ul class="inputslist mt10">
       <li><span class="c4">账户名：</span>

         <input name="Account"id="Account" type="text" value="<%=Email%>" style=" width:170px;" class="input floatleft" size="28" />
         <div class="tips c8" id="AccountVer"></div></li>
       <li><span class="c4">登录密码：</span>
         <input name="Pwd" id="Pwd" type="password" class="input floatleft" style=" width:170px;" size="28" /><div class="tips c8" id="PwdVer"></div></li>
       <li><span class="c4">验证码：</span>
          <input id="getcode" name="getcode" type="text" class="input floatleft"style=" width:95px;" size="13"  /><div id="imgcode"><img id="validateCode" src="/Account/GetValidateCode.ashx" alt="验证码" onclick="this.src='/Account/GetValidateCode.ashx?rnd=' + Math.random();"/></div><div class="tips c8" id="codemsg"></div>
       </li>
       <li class="btn"><input type="button" class="btn_login" value="" id="btn-login" />
             <a href="Repass.aspx" target="_blank" class="c2">忘记密码</a></li>
     </ul>
     <div id="login_err"></div>
    <div class="l_share">
    	<div class="con">
    	  &nbsp;<a href="javascript:;" id="regia" title="注册">我也要成为分销商</a>  </div>

    </div>
    <p><br />
      <br />
    </p>
   </div>
 </div>
 <div class="login_pic">

 </div>
</div>



<div id="regiagent" class="modal hide  in"  style="position: absolute;height: 600px; width: 650px; left: 45%; border-top-left-radius: 12px; border-top-right-radius: 12px; border-bottom-right-radius: 12px; border-bottom-left-radius: 12px; z-index: 901; display: none; " >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true" id="closeregiagent" style=" font-size:18px;">
                    ×</button>
            </div>
            <div id="contentinfo" style="padding: 0; line-height: 25px; width: 650px; height: 355px; overflow-y: auto; margin: 10px auto 0 auto;">
            <div id="showli" style="height: 430px; width: 350px; left: 0; top: 30px; z-index: 99999; position: absolute; display: ">

                <h3 class="ui-form-title" style="padding-left:430px;">
                        <strong>分销商注册</strong></h3>

                <form name="Regi" method="post" action="" target="_parent">
                <div class="grid-780  fn-clear" style=" padding-top:15px;">
                    <h3 class="ui-form-title">
                        <strong>填写登录信息</strong><span class="explain">请填写您的账户登录名和密码</span></h3>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                                登陆账户</label>
                            <input name="RAgent_Email" type="text" id="RAgent_Email" maxlength="250" size="20" value=""
                                class="ui-input" autocomplete="off">
                            <label id="EmailVer">
                            </label>
                        </div>
                        <div class="ui-form-item">
                            <label for="payPwdConfirm" class="ui-label">
                                登录密码</label>
                            <input name="RAgent_passwords" type="password" id="RAgent_passwords" size="12" maxlength="50" class="ui-input"
                                data-explain="请输入登录密码">
                        </div>
                        <div class="ui-form-item">
                            <label for="payPwdConfirm" class="ui-label">
                                再次输入密码</label>
                            <input name="RAgent_qpasswords" type="password" id="RAgent_qpasswords" size="12" maxlength="50"
                                class="ui-input" data-explain="请再次输入登录密码。">
                        </div>
                    </div>
                    <div class="ui-form-dashed">
                    </div>
                    
                    <h3 class="ui-form-title">
                        <strong>联系人信息</strong><span class="explain"><span class="ft-orange">请准确填写联系人信息</span></span></h3>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="realName" class="ui-label">
                                联系人姓名</label>
                            <input name="RAgent_Name" type="text" id="RAgent_Name" maxlength="250" size="20" class="ui-input"
                                autocomplete="off">
                        </div>
                          <div class="ui-form-item">
                <label class="ui-label">
                    图形验证码
                </label>
                <input name="rgetcode" type="text" placeholder="图形验证码" id="rgetcode" size="10" class="i-text i-text-authcode ui-input sl-checkcode-input" />
                <img id="rvalidateCode" src="/Account/GetValidateCode.ashx" alt="rValidateCode" title="点击图片刷新验证码" />
                <a href="javascript:;" id="rvalidateCodetext">更换</a>
            </div>
                        <div class="ui-form-item">
                            <label for="IDCardNo" class="ui-label">
                                联系人手机</label>
                            <input name="RAgent_Phone" type="text" id="RAgent_Phone" maxlength="50" size="20" autocomplete="off"
                                class="ui-input ui-input-170"><a  id="RAgent_getphonecode" style="text-decoration:underline; cursor:pointer;">获取短信验证码</a>
                            <label id="lbltel">
                            </label>
                        </div>
                        <div class="ui-form-item">
                            <label for="IDCardNo" class="ui-label">
                                短信验证码</label>
                            <input name="RAgent_phonecode" type="text" id="RAgent_phonecode" maxlength="50" size="20" autocomplete="off"
                                class="ui-input ui-input-170">
                            <label id="Label1">
                            </label>
                        </div>
                    </div>
                    <div class="ui-form-item">
                        <div style=" float:left;" id="submitBtn" class="ui-button-morange ">
                            <input id="RAgent_regisub" type="button" class="ui-button-text" style="padding: 0 22px;
background-position: right -503px;
line-height: 31px;
height: 31px;" value="确  定" />
                        </div>
        
                                            <br/>
                                          
                            <br/>
                       <%-- <span class="ui-form-confirm"><span class="loading-text fn-hide">正在提交信息</span>--%>
                        
                    </div>
                </div>
                </form>



                <div style="padding: 0; line-height: 25px; width: 350px; height: 355px; overflow-y: auto; margin: 10px auto 0 auto;">
                    <dl id="titellist">
                    </dl>
                </div>
            </div>
        </div>
</div>



   <input id="hid_num" type="hidden" value="0" />

</div>
</asp:Content>