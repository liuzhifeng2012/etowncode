<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/M/MemberH5.Master"  CodeBehind="Card.aspx.cs" Inherits="ETS2.WebApp.M.Card" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            //判断卡号
            $("#Cardcode").blur(function () {
                $("#CardcodeVer").html(""); //离开后先清空备注
                var Cardcode = $("#Cardcode").val();
                //判断卡号不为空
                if (Cardcode != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getCard", { Card: Cardcode, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#CardcodeVer").html("");
                                $("#VCardcode").val(1);

                            } else {
                                $("#CardcodeVer").html(data.msg);
                                $("#CardcodeVer").css("color", "red");
                                $("#VCardcode").val(0);
                                return;
                            }

                        }
                    })
                }
            })

            //判断手机
            $("#Phone").blur(function () {
                $("#PhoneVer").html(""); //离开后先清空备注
                var Phone = $("#Phone").val();
                //Phone
                if (Phone != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getPhone", { Phone: Phone, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#PhoneVer").html("");
                                $("#VPhone").val(1);
                            } else {
                                $("#PhoneVer").html(data.msg);
                                $("#PhoneVer").css("color", "red");
                                $("#VPhone").val(0);
                                return;
                            }

                        }
                    })
                }
            })

            //判断密码
            $("#QPassword1").blur(function () {
                $("#QPassword1Ver").html(""); //离开后先清空备注
                var QPassword1 = $("#QPassword1").val();
                var Password1 = $("#Password1").val();
                //Phone
                if (QPassword1 == "" || QPassword1 != Password1) {
                    $("#QPassword1Ver").html("再次填写密码错误");
                    $("#QPassword1Ver").css("color", "red");
                    return false;
                } else {
                    $("#VQPassword1").val(1);
                }
            })



            //提交按钮
            $("#confirmBtn").click("click", function () {
                var Cardcode = $("#Cardcode").val();
                var Email = $("#Email").val();
                var Password1 = $("#Password1").val();
                var QPassword1 = $("#QPassword1").val();
                var Name = $("#Name").val();
                var Phone = $("#Phone").val();

                if (Cardcode == "") {
                    $("#CardcodeVer").html("请填写卡号");
                    $("#CardcodeVer").css("color", "red");
                    return;
                }
                if ($("#VCardcode").val() == 0) {
                    $("#CardcodeVer").html("卡号有误");
                    $("#CardcodeVer").css("color", "red");
                    return;
                };

                if (Password1 == "") {
                    $("#Password1Ver").html("请填写密码");
                    $("#Password1Ver").css("color", "red");
                    return;
                }
                if (QPassword1 == "") {
                    $("#QPassword1Ver").html("再次填写密码错误");
                    $("#QPassword1Ver").css("color", "red");
                    return;
                }
                if ($("#VQPassword1").val() == 0) {
                    $("#QPassword1Ver").html("密码有误");
                    $("#QPassword1Ver").css("color", "red");
                    return;
                };

                if (Name == "") {
                    $("#NameVer").html("请填写姓名");
                    $("#NameVer").css("color", "red");
                    return;
                } else {
                    $("#NameVer").html("");
                };
                if (Phone == "") {
                    $("#PhoneVer").html("请填写手机");
                    $("#PhoneVer").css("color", "red");
                    return;
                }
                if ($("#VPhone").val() == 0) {
                    $("#PhoneVer").html("手机号码有误");
                    $("#PhoneVer").css("color", "red");
                    return;
                };
                $(".loading-text").html("正在提交开卡信息，请稍后...")

                //创建订单
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=webregcard", { comid: comid, Cardcode: Cardcode, Email: Email, Password1: Password1, Name: Name, Phone: Phone }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.msg == 0) {
                        $.prompt("注册账户错误，请重新提交");
                        return;
                    } else if (data.type == 1) {
                        $.prompt(data.msg, {
                            buttons: [{ title: "确定", value: true}],
                            submit: function (e, v, m, f) {
                                if (v == true) {
                                    location.reload();
                                }
                            }
                        });
                    }
                    else {
                        location.href = "/M/CardSuccess.aspx?Name=" + Name + "&Cardcode=" + Cardcode + "&Email=" + Email + "&Phone=" + Phone
                        return;
                    }
                })
            })
        })
    </script>
<title>微旅行 无V不至</title> 
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
<div id="header">
		<span class="left btn_back" onClick="goBack();"></span> 
		设置我的微旅行会员帐号
	</div> 
 
	<div class="info_wrapper"> 
		<form action="" id="loginForm" method="post"> 
			<p class="input_group"> 
				<span id="CardcodeVer"></span>
				<input id="Cardcode" name="Cardcode" type="number" placeholder="输入您的微旅行会员卡号" required /> 
			</p> 
			<p>您可通过卡号或手机号+密码方式登录微旅行会员帐户，专享会员服务</p> 

			<p class="input_group"> 
				<span id="Password1Ver"></span>
				<input id="Password1" name="Password1" type="password" placeholder="输入密码" required> 
				<span id="QPassword1Ver"></span>
				<input id="QPassword1" name="QPassword1" type="password" placeholder="再次输入密码" required> 
			</p> 
			<p>无V不至 会员专享</p> 
				<p class="input_group"> 
				<span id="NameVer"></span>
				<input id="Name" name="Name" type="text" placeholder="您的姓名" required> 
			</p> 
			<p class="input_group"> 
				<span id="PhoneVer"></span>
				<input id="Phone" name="Phone" type="tel" placeholder="输入您手机卡号" required> 
			</p>
			<p><a href="javascript:;" class="btn_big btn_blue" id="confirmBtn">确定</a></p> 
		</form> 
	</div> 
    <input type="hidden" name="VCardcode" id="VCardcode" value="0">
    <input type="hidden" name="VQPassword1" id="VQPassword1" value="0">
    <input type="hidden" name="VPhone" id="VPhone" value="0">
</asp:Content>
