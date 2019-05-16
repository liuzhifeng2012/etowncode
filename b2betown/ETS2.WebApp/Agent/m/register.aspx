<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="ETS2.WebApp.Agent.m.register" %>

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
        $(function () {
            //提交按钮
            $("#btn-submit").click(function (event) {
                stopDefault(event);
                var Email = $("#Email").val();
                var Password1 = $("#Password1").val();
                var QPassword1 = $("#QPassword1").val();
                var Name = $("#Name").val();
                var Tel = $("#Phone").val();
                var Phone = $("#Phone").val();

                var Company = $("#Email").val();
                var Address = "";
                var Sex = "";
                var agentsort = 0; //默认0票务分销
                var agentsourcesort = 0; //分销类型:默认0，没选择类型
                var com_province = ""; //默认""
                var com_city = "";

                if (Email == "") {
                    alert("请填写店铺名称");

                    return;
                }

                if (Password1 == "") {
                    alert("请填写密码");

                    return;
                }
                if (QPassword1 == "") {
                    alert("再次填写密码错误");

                    return;
                }
                if (Password1 != QPassword1) {
                    alert("两次输入密码不相符");

                    return;
                }

                if (Name == "") {
                    alert("请填写姓名");
                    return;
                }

                if (Phone == "") {
                    alert("请填写手机");

                    return;
                }
                 
                //创建订单
                $.post("/JsonFactory/AgentHandler.ashx?oper=Agentregi", { agentsourcesort: agentsourcesort, comid: '<%=comid %>', Email: Email, Password1: Password1, Name: Name, Tel: Tel, Phone: Phone, Company: Company, Address: Address, agentsort: agentsort, com_province: com_province, com_city: com_city }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert("注册出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            alert(" 您已注册成功，请等待商家为您授权！");
                            location.href = "/Agent/m/Login.aspx?Email=" + Email
                            return;
                        }
                        else {
                            alert("参数传递出错，请重新操作");
                            return;
                        }
                    }
                })

            })
        })
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
<body>
    <div class="weixin_bangding">
        <h1 style="text-align: center; font-size: 24px; margin: 20px 0px;">
            请填写下面开户信息</h1>
        <ul>
            <li>
                <input type="text" id="Email" placeholder="店铺名称(登陆账户名)" class="wx_input_s" value="" /></li>
            <li>
                <input type="password" id="Password1" placeholder="密码" class="wx_input_s" value="" /></li>
            <li>
                <input type="password" id="QPassword1" placeholder="确认密码" class="wx_input_s" value="" /></li>
            <li>
                <input type="text" id="Name" placeholder="联系人姓名" class="wx_input_s" value="" /></li>
            <li style="display: none;">
                <input type="text" id="Phone" placeholder="联系人手机" class="wx_input_s" value="<%=new_tel %>" /></li>
        </ul>
        <div>
            <button id="btn-submit" class="wx_button_s  btnSubmit">
                提交</button>
        </div>
        <div class="butbox">
            <div class="text-center lg">
                <a href="/agent/m/login.aspx">己有帐号？立即登录<i></i></a></div>
        </div>
    </div>
</body>
</html>
