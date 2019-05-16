<%@ Page Title="注册" Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs"
    Inherits="ETS2.WebApp.Account.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>注册账户</title>
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


            //商户行业类型
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetIndustryList", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#sel_hangye").empty();
                    for (var i = 0; i < data.msg.length; i++) {
                        $("#com_type").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Industryname + '</option>');
                    }

                }
            })

            //加载一个通用行行业，已方便注册
            $("#com_type").append('<option value="0">其他行业</option>');


            $("#regi").click(function () {
                var Account = $("#Account").trimVal();
                var passwords = $("#passwords").trimVal();
                var qpasswords = $("#qpasswords").trimVal();

                var com_name = $("#com_name").trimVal();
                var Scenic_name = $("#Scenic_name").trimVal();
                var com_type = $("#com_type").trimVal();


                var Contact = $("#Contact").trimVal(); //联系人姓名
                var phone = $("#phone").trimVal();

                var province = $("#province").trimVal();
                var city = $("#city").trimVal();



                if (Account == "") {
                    $.prompt("登录账户不可为空");
                    return;
                }
                if (!CheckEmail(Account, "电子邮箱")) { return false };

                if (passwords == "") {
                    $.prompt("密码不可为空！");
                    return;
                }
                if (qpasswords == "") {
                    $.prompt("确认密码不可为空！");
                    return;
                }
                if (passwords != qpasswords) {
                    $.prompt("密码和确认密码不符！");
                    return;
                }
                if (com_name == "") {

                    $.prompt("单位名称不可为空！");
                    return;
                }

                if (province == "" || province == "省份") {
                    $.prompt("请选择所在省份");
                    return;
                }
                if (city == "" || city == "城市") {
                    $.prompt("请选择所属城市");
                    return;
                }
                if (com_type == "") {
                    $.prompt("请选择所属行业");
                    return;
                }

                if (Contact == "") {
                    $.prompt("联系人姓名不可为空！");
                    return;
                }

                if (phone == "") {
                    $.prompt("联系人手机不可为空！");
                    return;
                } else {
                    if (!isMobel(phone)) {
                        $.prompt("请正确填写联系人手机！");
                        return;
                    }
                }

                //                $('#regi').hide().after('<span id="spLoginLoading" style="margin-left:105px;height:30px;color:#f39800; ">登录中……</span>');
                $.ajax({
                    type: "POST",
                    url: "/JsonFactory/RegisterUser.ashx?oper=edit",
                    data: { Account: Account, passwords: passwords, com_name: com_name, Scenic_name: Scenic_name, Contact: Contact, phone: phone, province: province, city: city, com_type: com_type },
                    success: function (data) {
                        data = eval('(' + data + ')');
                        if (data.type == 1) {
                            $.prompt("账户注册提示:" + data.msg, { buttons: [{ title: "确定", value: true}], submit: function (e, v, m, f) { if (v == true) { } } });
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("账户信息注册成功，请等待管理员处理", {
                                buttons: [
                                 { title: '确定', value: true }
                                ],
                                opacity: 0.1,
                                focus: 0,
                                show: 'slideDown',
                                submit: function (e, v, m, f) {
                                    if (v == true)
                                        location.href = "/account/RegisterSuc.aspx";
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
            function CheckEmail(myform, name)
            {
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
            <div class="navi-container">
                <ol class="ui-step    ui-step-4 ">
                    <li class="ui-step-start ui-step-done ui-step-active">
                        <div class="ui-step-line">
                            -</div>
                        <div class="ui-step-icon">
                            <i class="ui-step-number">第一步： </i><span class="ui-step-text">提交注册信息</span>
                        </div>
                    </li>
                    <%-- <li>
                        <div class="ui-step-line">
                            -</div>
                        <div class="ui-step-icon">
                            <i class="ui-step-number">第二步：</i> <span class="ui-step-text">验证邮箱电话</span>
                        </div>
                    </li>--%>
                    <li>
                        <div class="ui-step-line">
                            -</div>
                        <div class="ui-step-icon">
                            <i class="ui-step-number">第二步：</i> <span class="ui-step-text">注册成功</span>
                        </div>
                    </li>
                </ol>
            </div>
        </div>
        <form name="Regi" method="post" action="" target="_parent">
        <div class="grid-780 grid-780-border fn-clear">
            <p class="ui-tiptext">
                只需几分钟即可完成注册！随后我们将为您免费开通商户平台服务。
            </p>
            <div class="ui-form-dashed">
            </div>
            <h3 class="ui-form-title">
                <strong>填写登录信息</strong><span class="explain">请填写您的账户登录名和密码</span></h3>
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
                        登录密码</label>
                    <input name="passwords" type="password" id="passwords" size="12" maxlength="50" class="ui-input"
                        data-explain="请输入登录密码">
                </div>
                <div class="ui-form-item">
                    <label for="payPwdConfirm" class="ui-label">
                        再次输入密码</label>
                    <input name="qpasswords" type="password" id="qpasswords" size="12" maxlength="50"
                        class="ui-input" data-explain="请再次输入登录密码。">
                </div>
            </div>
            <div class="ui-form-dashed">
            </div>
            <h3 class="ui-form-title">
                <strong>填写商家信息</strong><span class="explain">请准确填写商家单位名称和相关信息</span></h3>
            <div class="ui-form-group">
                <div class="ui-form-item">
                    <label for="payPwd" class="ui-label">
                        单位名称</label>
                    <input name="com_name" type="text" id="com_name" size="40" maxlength="250" class="ui-input2"
                        autocomplete="off">
                    <label id="lblcompanyname">
                    </label>
                </div>
            </div>
            <div class="ui-form-group">
                <div class="ui-form-item">
                    <label for="payPwdConfirm" class="ui-label">
                        景区名称/简称</label>
                    <input name="Scenic_name" type="text" id="Scenic_name" size="20" maxlength="250"
                        class="ui-input2" autocomplete="off">
                </div>
            </div>
            <div class="ui-form-group">
                <div class="ui-form-item">
                    <label for="payPwd" class="ui-label">
                     所在城市</label>
                    <select name="province" id="province" class="ui-input">  
                        <option value="省份" selected="selected" >省份</option>  
                    </select>  
                    <select name="city" id="city"  class="ui-input">  
                        <option value="城市" selected="selected">市县</option>  
                    </select>  

                </div>
            </div>
           <div class="ui-form-group">
                <div class="ui-form-item">
                    <label for="payPwd" class="ui-label">
                     所属行业</label>
                    <select name="com_type" id="com_type" class="ui-input">  
                        <option value="" selected="selected" >请选择所属行业</option>  
                    </select>  
                </div>
            </div>
            
            <h3 class="ui-form-title">
                <strong>联系人信息</strong><span class="explain"><span class="ft-orange">请准确填写联系人信息</span></span></h3>
            <div class="ui-form-group">
                <div class="ui-form-item">
                    <label for="realName" class="ui-label">
                        联系人姓名</label>
                    <input name="Contact" type="text" id="Contact" maxlength="250" size="20" class="ui-input"
                        autocomplete="off">
                </div>
                <div class="ui-form-item">
                    <label for="IDCardNo" class="ui-label">
                        联系人手机</label>
                    <input name="phone" type="text" id="phone" maxlength="50" size="20" autocomplete="off"
                        class="ui-input ui-input-170">
                    <label id="lbltel">
                    </label>
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
                <li class="ui-link-item">我们专注为中国旅游同业提供电子化营销整合方案。
           包括景区、酒店、旅行社的电子预订与验证、微信营销、分销渠道管理。 </li>

                <li class="ui-link-item">Copyright Vctrip@2014. All Right Reserved    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;北京微旅程科技有限公司</li>
                <li class="ui-link-item">北京市朝阳区劲松华腾大厦4层401   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;4006361919 010-59022350</li></ul>
        </div>
    </div>
    
    <script type="text/javascript">
            var province = document.getElementById('province');
            var city = document.getElementById('city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</body>
</html>
