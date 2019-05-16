<%@ Page Title="登录" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="ETS2.WebApp.Account.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/Styles/login.css" />
    <link href="../Scripts/Impromptu.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <%-- <script src="../Scripts/jquery.cookie.2.2.0.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {


            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#btn-submit").click();    //这里添加要处理的逻辑  
                }
            });

            $("#validateCode").click(function () {
                this.src = 'GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })
            $("#validateCodetext").click(function () {
                $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })

            //点击Enter键触发登录
            $("body").keydown(function (e) {
                if (e.keyCode == 13) {
                    $(".btn_login").click();
                }
            });
            $("#btn-submit").click(function () {
                var username = $.trim($("#username").val());
                var pwd = $.trim($("#password1").val());
                var getcode = $.trim($("#getcode").val());


                if (username == "" || pwd == "") {
                    $("#error-box").html("用户名或密码不可为空");
                    $("#error-box").show();

                    return;
                }
                if (getcode == '') {
                    $("#error-box").html("验证码不可为空");
                    $("#error-box").show();

                    return;
                }

                // $.cookie("username", username, { expires: 1 });


                $("#loading").show();

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/RegisterUser.ashx?oper=login",
                    data: { ts: Math.random(), username: username, pwd: pwd, getcode: getcode },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                            $("#error-box").html(data.msg);
                            $("#error-box").show();
                            $("#loading").hide();
                        }
                        if (data.type == 100) {
                            parent.location.href = "/Manage.aspx";
//                            if (navigator.userAgent.indexOf("Safari") > -1 && navigator.userAgent.indexOf("Chrome") < 1) {
//                                parent.location = '/Manage.aspx'
//                                return true;

//                            } else {
//                                parent.location.href = "/Manage.aspx";
//                            }
                        }

                    }
                })


            })
        })
    </script>
    <style type="text/css">
        .none
        {
            display: none;
        }
        #loading
        {
            position: absolute;
            left: 50%;
            top: 60px;
            z-index: 99;
        }
        #loading, #loading .lbk, #loading .lcont
        {
            width: 146px;
            height: 146px;
        }
        #loading .lbk, #loading .lcont
        {
            position: relative;
        }
        #loading .lbk
        {
            background-color: #000;
            opacity: .5;
            border-radius: 10px;
            margin: -73px 0 0 -73px;
            z-index: 0;
        }
        #loading .lcont
        {
            margin: -146px 0 0 -73px;
            text-align: center;
            color: #f5f5f5;
            font-size: 14px;
            line-height: 35px;
            z-index: 1;
        }
        #loading img
        {
            width: 35px;
            height: 35px;
            margin: 30px auto;
            display: block;
        }
    </style>
</head>
<body>
    <div class="login_box clear">
        <div class="login_inputs_right login_inputs ">
            <b></b>
            <div class="ui-login-aside">
                <form name="login" method="post" class="ui-form" onsubmit="return ckout()" action="login.asp">
                <div class="ui-form-title">
                   <ul class="nav-tabs2">
                    <%if (comid == 1305)
                      { %>
                       <li  style="margin-right:17px;"><a href="/Agent/maikexinglogin.html">分销商登录</a></li>
                    <%} else{%>
                        <li  style="margin-right:17px;"><a href="/Agent/Login.html">分销商登录</a></li>
                        <%} %>
                        <li class="active"><a href="/Account/Login.aspx">供应商登录</a></li>
                        </ul>
                </div>
                <div class="error-box " id="error-box" style="display: none;">
                </div>
                <div class="login-form-cnt">
                    <fieldset>
                        <div class="fm-item ui-form-item sl-account">
                            <label class="fm-label ui-form-label" for="logonId">
                                账户名：</label>
                            <input name="username" type="text" class="i-text ui-input" id="username" size="20"
                                maxlength="250" placeholder="账户名" />
                        </div>
                        <div class="fm-item ui-form-item sl-aliedit">
                            <label class="fm-label pwd-label ui-form-label" desc="登录密码">
                                登录密码：</label>
                            <span class="fm-link">
                                <!--<a id="forgetPsw" href="#" target="_blank" seed="auth-findPWD">忘记登录密码？</a>-->
                            </span>
                            <input name="password1" type="password" class="i-text ui-input" id="password1" size="20"
                                maxlength="20" />
                        </div>
                        <div class="fm-item ui-form-item sl-checkcode">
                            <label class="fm-label ui-form-label">
                                验证码：</label>
                            <div>
                                <input name="getcode" type="text" placeholder="验证码" id="getcode" size="10" class="i-text i-text-authcode ui-input sl-checkcode-input" />
                                <img id="validateCode" src="GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码" />
                                <a href="javascript:;" id="validateCodetext">点击更换</a>
                            </div>
                        </div>
                        <div class="fm-item ui-form-item ui-btn-cnt">
                            <input name="按钮" type="button" class="btn-login" id="btn-submit" tabindex="4" value="登  录"
                                seed="B-login-button1">
                        </div>
                        <p class="login-help-cnt">
                            <br />
                            <br /><a href="Repass.aspx" target="_blank">忘记密码</a> <%if (comid == 1305)
                                                                                   { %> <a href="/agent/help_xfgzs.html" target="_blank">消费者告知书</a>
                                                                                   <%}else{ %><!--<a href="/account/Register.aspx" target="_blank">商户注册</a>-->
                      <%} %>
                        </p>
                    </fieldset>
                </div>
                </form>
                <div class="l_share">
                    <div class="con">
                    </div>
                </div>
            </div>
            <b class="login_B"></b>
        </div>
        <div class="login_pic_regi">
        </div>
    </div>
    <div id="loading" style="top: 120px; display: none;">
        <div class="lbk">
        </div>
        <div class="lcont">
            <img src="../Images/loading.gif" alt="loading..." />正在加载...</div>
    </div>
</body>
</html>
