<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ETS2.WebApp.Agent.m.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <title>欢迎登陆分销商管理系统</title>
    <script src="/Scripts/jquery-1.7.2.min.js"></script>
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/password-reset.css" rel="stylesheet" />
    <script type="text/javascript">

        $(function () {

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#loading-example-btn").click();    //这里添加要处理的逻辑  
                }
            });

            //非微信浏览器，微信免登陆不显示
             
            if (isWeiXin() == false) {
                $("#remAutoLogin").parent(".loginFormCbx").hide();
            }

            $("#appversion").click(function () {

                location.href = 'http://http://shop.etown.cn/Manage/default.html';
            });

            $('#loading-example-btn').click(function () {
                var Account = $("#LoginName").val();
                var Pwd = $("#PassWord").val();

                if (Account == "") {
                    showErr("请填写用户名");
                    return;
                }
                if (Pwd == "") {
                    showErr("填写密码");
                    return;
                }
                var isfreelanding = $("input:checkbox[id='remAutoLogin']:checked").val();

                showLoading();
                $.post("/JsonFactory/AgentHandler.ashx?oper=phoneLogin", { openid: '<%=openid %>', Account: Account, Pwd: Pwd, isfreelanding: isfreelanding }, function (data) {
                    if (data == "OK") {

                        location.href = "Default.aspx";
                    } else {
                        showErr(data);
                        hideLoading();
                        return;
                    }

                })
            });
        })

        function showErr(a) {
            $("html").css({
                "overflow-y": "hidden"
            });
            if ($("#bgDiv").html() == null) {
                $('<div id="bgDiv"></div>').appendTo("body")
            }
            if ($("#showMsg").html() != null) {
                $("#showMsg").remove()
            }
            $('<div id="showMsg"><div class="msg-title">温馨提示</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr()">知道了</a></div></div>').appendTo("body");
            var b = $(window).height();
            var d = $(window).scrollTop();
            var c = $("#showMsg").height();
            $("#bgDiv").css({
                height: $(document).innerHeight()
            }).show();
            $("#showMsg").css({
                top: (b - c) / 2
            }).show()
        }
        function hideErr() {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #showMsg").hide();

        }

        function showLoading() {
            $("html").css({
                "overflow-y": "hidden"
            });
            if ($("#bgDiv").html() == null) {
                $('<div id="bgDiv"></div>').appendTo("body")
            }
            if ($("#loading").html() != null) {
                $("#loading").remove()
            }
            $('<div id="loading" style="top: 352px;"><img src="/Images/loading.gif" alt="loading..."></div>').appendTo("body");
            var b = $(window).height();
            var d = $(window).scrollTop();
            var c = $("#loading").height();
            $("#bgDiv").css({
                height: $(document).innerHeight()
            }).show();
            $("#loading").css({
                top: (b - c) / 2
            }).show()
        }
        function hideLoading() {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #loading").hide();

        }

        //判断微信版本,微信版本5.0以上
        function isWeiXin() {
            var ua = window.navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == 'micromessenger' && parseFloat(navigator.appVersion) >= 5) {
                return true;
            } else {
                return false;
            }
        }
    </script>
   
</head>
<body>
    <div class="weixin_bangding">
        <h1 style="text-align: center; font-size: 24px; margin: 20px 0px;">
            欢迎登陆分销商管理系统</h1>
        <ul>
            <li>
                <input type="text" id="LoginName" placeholder="用户名" class="wx_input_s" value="<%=Email %>" /></li>
            <li>
                <input type="password" id="PassWord" placeholder="密码" class="wx_input_s" /></li>
        </ul>
        <div class="clearfix" style="padding-top: 20px; text-align: left;">
            <label class="loginFormCbx" >
                <input type="checkbox" id="remAutoLogin" value="微信免登录" title="微信免登录">
                微信免登录</label>
            <label class="fpass">
                <a  href="/agent/m/getPassword.aspx">忘记密码?</a>
            </label>
        </div>
        <div>
            <input type="button" class="wx_button_s  btnSubmit" value="登 陆" id="loading-example-btn" />
        </div>
        <div class="butbox">
            <div class="text-center lg">
                <a href="/agent/m/phoneverify.aspx">我也要成为分销商<i></i></a></div>
        </div>
    </div>
    <!-- Modal -->
    <div style="height: 565px; display: none;" id="bgDiv">
    </div>
    <div id="showMsg" style="top: 352px; display: none;">
        <div class="msg-title">
            温馨提示</div>
        <div class="msg-content">
            请填写登录名！</div>
        <div class="msg-btn">
            <a href="javascript:;" onclick="hideErr()">知道了</a></div>
    </div>
    <div id="loading" style="top: 352px; display: none;">
        <img src="/Images/loading.gif" alt="loading...">
    </div>
</body>
</html>
