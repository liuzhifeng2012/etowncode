<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Repass.aspx.cs" Inherits="ETS2.WebApp.Account.Repass" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>忘记密码</title>
    <link charset="utf-8" rel="stylesheet" href="/Styles/style-4.css" />
    <link charset="utf-8" rel="stylesheet" href="/Styles/reg.css" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery-impromptu.4.0.min.js"></script>
    <link rel="stylesheet" type="text/css" href="/Scripts/Impromptu.css" />
    <script type="text/javascript" src="/Scripts/common.js"></script>
    <script type="text/javascript">
        $(function () {
            function isMobel(value) {
                if (/^13\d{9}$/g.test(value) || /^1\d{10}$/g.test(value)) {
                    return true;
                } else {
                    return false;
                }
            }

            $("#validateCode").click(function () {
                this.src = 'GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })
            $("#validateCodetext").click(function () {
                $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })


            $("#regi").click(function () {
                var Account = $("#Account").trimVal();

                var phone = $("#phone").trimVal();
                var getcode = $("#getcode").trimVal();
                var findway = $('input:radio[name="findway"]:checked').val();

                if (Account == "") {
                    $.prompt("登录账户不可为空");
                    return;
                }
                //if (!CheckEmail(Account, "电子邮箱")) { return false };

                if (phone == "") {
                    $.prompt("联系人手机不可为空！");
                    return;
                } else {
                    if (!isMobel(phone)) {
                        $.prompt("请正确填写联系人手机！");
                        return;
                    }
                }
                if (getcode == "") {
                    $.prompt("请填写验证码！");
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: "/JsonFactory/RegisterUser.ashx?oper=findpass",
                    data: { Account: Account, phone: phone, getcode: getcode, findway: findway,accounttype:1 },
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.type == 1) {
                            $.prompt("找回密码错误！" + data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("密码已经发送，请注意接收", {
                                buttons: [
                                 { title: '确定', value: true }
                                ],
                                opacity: 0.1,
                                focus: 0,
                                show: 'slideDown',
                                submit: function (e, v, m, f) {
                                    if (v == true)
                                        location.href = "/";
                                }
                            });
                        }
                    }
                });
            });

        })

        function isEmail(str) {
            var supported = 0;
            if (window.RegExp) {
                var tempStr = "a";
                var tempReg = new RegExp(tempStr);
                if (tempReg.test(tempStr)) supported = 1;
            }
            if (!supported)
                return (str.indexOf(".") > 2) && (str.indexOf("@") > 0);
            var r1 = new RegExp("(@.*@)|(\\.\\.)|(@\\.)|(^\\.)");
            var r2 = new RegExp("^.+\\@(\\[?)[a-zA-Z0-9\\-\\.]+\\.([a-zA-Z]{2,3}|[0-9]{1,3})(\\]?)$");
            return (!r1.test(str) && r2.test(str));
        }
        function CheckEmail(myform, name) {
            if (isEmail(myform)) {
                return true;
            }
            else $.prompt("请正确填写电子邮箱！");
            return false;
        }     
        </script>

</head>
<body>
    <div class="topbar">
        <div class="grid-990 topbar-wrap fn-clear">
            <ul class="topmenu">

            </ul>
        </div>
    </div>
    <div class="header" coor="header">
        <div class="grid-780 grid-780-pd fn-hidden fn-clear">
            <div id="et-img-logo1" class="fn-left">
              <a href="/"><!-- <img id="imglogo" height="75" alt="" src="/manage/images/v-logo-1.jpg"></img>--></a>
            </div>
        </div>
    </div>
    <div id="banner" class="slide-1" coor-rate="0.1" coor="default-banner" role="banner"
        data-banner="false" style="height: 850px;">
        <div class="grid-780 grid-780-pd fn-hidden fn-clear">

        </div>
        <form name="Regi" method="post" action="" target="_parent">
        <div class="grid-780 grid-780-border fn-clear">
            <p class="ui-tiptext">
                找回密码 
            </p>
            <div class="ui-form-dashed">
            </div>
            <h3 class="ui-form-title">
               <span class="explain">请输入您需要密码找回的登录帐户名和注册手机 </span></h3>
            <div class="ui-form-group">
                <div class="ui-form-item">
                    <label for="payPwd" class="ui-label">
                        登陆账户</label>
                    <input name="Account" type="text" id="Account" maxlength="250" size="20" value=""
                        class="ui-input" autocomplete="off"> (电子邮箱)
                    <label id="lblaccountmsg">
                    </label>
                </div>
                <div class="ui-form-item">
                    <label for="payPwdConfirm" class="ui-label">
                        手机号码</label>
                    <input name="phone" type="text" id="phone" size="12" maxlength="50" class="ui-input"
                        data-explain="预留的手机号码">
                </div>
                 <div class="ui-form-item">
                    <label for="payPwdConfirm" class="ui-label">
                        找回方式</label>
                    <label><input name="findway" type="radio" value="sms" checked /> 短信找回</label>  <!--<label><input name="findway" type="radio" value="email" /> 邮箱找回</label> -->
                </div>
                <div class="ui-form-item">
                    <label for="payPwdConfirm" class="ui-label">
                        验证码</label>
                                <input name="getcode" type="text" placeholder="验证码" id="getcode" size="10" class="i-text i-text-authcode ui-input sl-checkcode-input" />
                                <img id="validateCode" src="GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码" />
                                <a href="javascript:;" id="validateCodetext">点击更换</a>
                </div>

            </div>
            <div class="ui-form-item">
                <div id="submitBtn" class="ui-button  ui-button-morange ">
                    <input id="regi" type="button" class="ui-button-text" value="确  定" />

                </div>
                                    <br/>
                    <br/>
                <span class="ui-form-confirm"><span class="loading-text fn-hide">正在提交信息</span></span>
            </div>
        </div>
        </form>
    </div>
    <div class="ui-form-dashed">
    </div>
    <div class="footer" coor="footer">
        <div class="grid-780 sitelink fn-clear" coor="sitelink" role="contentinfo">
            <ul class="ui-link fn-clear">
                <li class="ui-link-item">Copyright Vctrip@2014. All Right Reserved    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;北京微旅程科技有限公司</li>
        </div>
    </div>
    

</body>
</html>
