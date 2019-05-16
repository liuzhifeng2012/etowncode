<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recharge.aspx.cs" Inherits="ETS2.WebApp.Agent.m.Project"
    MasterPageFile="/Agent/m/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>分销商管理系统</title>
    <script type="text/javascript">
        $(function () {
            $("#lblcompany").html("商户名称:" + $("#hid_company").trimVal());
            $("#confirmButton").click(function () {
                var payprice = $("#payprice").trimVal();
                var u_name = $("#u_name").trimVal();
                var u_phone = $("#u_phone").trimVal();


                if (payprice == "") {
                    showErr("请填写充值金额");
                    return;
                } else {

                    var floatreg = new RegExp("^\\d+(?:\\.\\d+)?$", "g");
                    if (!floatreg.test(payprice)) {
                        showErr("充值金额格式不正确");
                        return;
                    }
                }
                if (u_name == "") {
                    showErr("请填写姓名");
                    return;
                }
                if (u_phone == "") {
                    showErr("请填写手机号");
                    return;
                }
                //                else {
                //                    if (!isMobel(u_phone)) {
                //                        showErr("请正确填写手机号");
                //                        return;
                //                    }
                //                }
                //创建订单
                $.post("/JsonFactory/OrderHandler.ashx?oper=agentRecharge", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comidtemp").trimVal(), payprice: payprice, u_name: u_name, u_phone: u_phone }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        //                        $.prompt(data.msg);
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        location.href = "http://shop" + $("#hid_comidtemp").trimVal() + ".etown.cn/h5/pay.aspx?orderid=" + data.msg + "&comid=" + $("#hid_comidtemp").trimVal();
                        return;
                    }
                })
            })

            $.ajax({
                type: "post",
                url: "/JsonFactory/AgentHandler.ashx?oper=getagentinfo",
                data: { agentid: $("#hid_agentid").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                        $.prompt("查询错误"); 
                        return;
                    }
                    if (data.type == 100) {
                        $("#u_name").val(data.msg.Contentname);
                        $("#u_phone").val(data.msg.Mobile);
                    }
                }
            })
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
    </script>
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        html, body
        {
            background: #ffffff;
            height: 100%;
        }
        body, div, ol, ul, li, dl, dt, dd, table, tr, td, h1, h2, h3, h4, h5, h6, img, p, input, textarea, select, form, button
        {
            margin: 0;
            padding: 0;
        }
        body, input, textarea, select, form, button
        {
            font: 12px/1.5 tahoma,\5b8b\4f53, "微软雅黑";
        }
        ul
        {
            list-style: none;
        }
        table
        {
            border-collapse: collapse;
            border-spacing: 0;
        }
        img, table, tr, td
        {
            border: 0;
        }
        h1, h2, h3, h4, h5, h6
        {
            font-size: 100%;
        }
        img, textarea, form, button, input, select
        {
            vertical-align: middle;
        }
        .login_button
        {
            font-family: "微软雅黑";
        }
        a, a:visited, a:hover, a:active
        {
            text-decoration: none;
        }
        input[type="button"], input[type="text"], input[type="password"]
        {
            outline: none;
            font-family: "微软雅黑";
        }
        body
        {
            background-color: #FAFAFA;
        }
        .weixin_bangding
        {
            padding: 10px;
            min-width: 300px;
        }
        .weixin_bangding ul li
        {
            margin: 10px 0 0 0;
            padding: 5px;
            background-color: #fff;
            border: 1px solid #dcdcdc;
            border-radius: 2px;
            box-shadow: 1px 1px 1px rgba(0,0,0,.05);
        }
        .weixin_bangding ul li:hover
        {
            border-color: #B2D7B0;
        }
        .wx_input_s
        {
            color: #c8c8c8;
            line-height: 20px;
            height: 30px;
            border: none;
            width: 100%;
        }
        .wx_input_s:focus
        {
            outline: none;
            color: #505050;
        }
        .wx_button_s
        {
            border-radius: 2px;
            width: 100%;
            line-height: 40px;
            height: 40px;
            color: #fff;
            font-size: 14px;
            margin-top: 10px;
            border: 1px solid #3CAFDC;
            border-bottom-color: #26922C;
            background-color: #3CAFDC;
            box-shadow: 0 1px 0 rgba(255,255,255,.2) inset, 1px 1px 2px rgba(0,0,0,.2);
            background: -webkit-gradient(linear,0 0,0 100%,color-stop(0,#3CAFDC),color-stop(100%,#3CAFDC));
            background: -moz-linear-gradient(top,#3CAFDC,#3CAFDC);
            background: -o-linear-gradient(top,#3CAFDC,#3CAFDC);
            background: -ms-linear-gradient(top,#3CAFDC,#3CAFDC);
            background: linear-gradient(top,#3CAFDC,#3CAFDC);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="weixin_bangding">
        <h1 style="text-align: center; font-size: 24px; margin: 20px 0px;">
            分销在线充值</h1>
        <h1 id="lblcompany" style="text-align: left; font-size: 18px; margin: 5px 0px;">
        </h1>
        <ul>
            <li style="width: 50%;">
                <input type="tel" id="payprice" placeholder="充值金额" class="wx_input_s" style="width: 50%;
                    font-size: 18px; color: #333; font-weight:bold;" /></li>
            <li style="display: none;">
                <input type="text" id="u_name" placeholder="联系人姓名" class="wx_input_s" /></li>
            <li style="display: none;">
                <input type="tel" id="u_phone" placeholder="联系人电话" class="wx_input_s" /></li>
        </ul>
        <div>
            <input type="button" class="wx_button_s  btnSubmit" value="为此商户充值" id="confirmButton" />
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
</asp:Content>
