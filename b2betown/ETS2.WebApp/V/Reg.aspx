<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/V/Member.Master" CodeBehind="Reg.aspx.cs" Inherits="ETS2.WebApp.V.Reg" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            //账号名
            $("#Email").blur(function () {
                $("#EmailVer").html(""); //离开后先清空备注
                var Email = $("#Email").val();
                //判断邮箱不为空
                if (Email != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getEmail", { Email: Email, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#EmailVer").html("√");
                                $("#EmailVer").css("color", "green");
                                $("#VEmail").val(1);
                            } else {
                                $("#EmailVer").html(data.msg);
                                $("#EmailVer").css("color", "red");
                                $("#VEmail").val(0);
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
                                $("#PhoneVer").html("√");
                                $("#PhoneVer").css("color", "green");
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


            //提交按钮
            $("#btn-submit").click(function () {
                var Email = $("#Email").val();
                var Password1 = $("#Password1").val();
                var QPassword1 = $("#QPassword1").val();
                var Name = $("#Name").val();
                var Phone = $("#Phone").val();
                //var Sex = $('input:radio[name="Sex"]:checked').val();
                var Sex = "";

                if (Email == "") {
                    $("#EmailVer").html("请填电子邮件");
                    $("#EmailVer").css("color", "red");
                    return;
                }
                if ($("#VEmail").val() == 0) {
                    $("#EmailVer").html("电子邮箱有误");
                    $("#EmailVer").css("color", "red");
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
                }

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


                $("#loading").html("正在提交开卡信息，请稍后...")

                //创建订单
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Useregcard", { comid: comid, Email: Email, Password1: Password1, Name: Name, Phone: Phone, Sex: Sex }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("注册出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            location.href = "/V/CardSuccess.aspx?Name=" + Name + "&Email=" + Email + "&Phone=" + Phone
                            return;
                        }
                        else {
                            $.prompt("参数传递出错，请重新操作");
                            return;
                        }
                    }
                })

            })
        })
    </script>
    <style type="text/css">
     .ui-name{border: 1px solid #C1C1C1;
color: #343434;
font-size: 14px;
height: 25px;
line-height: 25px;
padding: 2px;
vertical-align: middle;
width: 110px;}
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
 
 
 <div class="grid-780 grid-780-border fn-clear">
 
			
    	<form class="ui-form ui-form-bg" name="regCompleteForm" method="post" action="#" id="J-complete-form" novalidate="novalidate" data-widget-cid="widget-0">
	
        <p class="ui-tiptext">
			只需3分钟即可完成注册！
        </p>
			
                        <div class="ui-form-dashed"></div>
            <h3 class="ui-form-title"><strong>填写登录信息</strong><span class="explain">请填写您的 常用电子邮箱 做为登录账户名</span></h3>
				
                     <div class="ui-form-group">
 
        <div class="ui-form-item">
	<label for="payPwd" class="ui-label">账户名</label>
	<input autocomplete="off" class="ui-input" type="text" id="Email" name="Email" data-error="    " seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2" data-validator-set="widget-3"/> 
	（常用电子邮件）<span id="EmailVer"></span>
</div>
        <div class="ui-form-item">
	<label for="payPwdConfirm" class="ui-label">登录密码</label>
	<input autocomplete="off" class="ui-input" type="password" id="Password1" name="Password1" data-error="    " data-explain="请再次输入登录密码。" seed="JCompleteForm-payPwdConfirm" smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-4"/>
    <span id="Password1Ver"></span>
</div>
   <div class="ui-form-item">
	<label for="payPwdConfirm" class="ui-label">再输入一次</label>
	<input autocomplete="off" class="ui-input" type="password" id="QPassword1" name="QPassword1" data-error="    " data-explain="请再次输入登录密码。" seed="JCompleteForm-payPwdConfirm" smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-4"/>
    <span id="QPassword1Ver"></span>
</div>
					</div>	
					
			            <div class="ui-form-dashed"></div>
            <h3 class="ui-form-title"><strong>填写会员信息</strong><span class="explain">请填写会员信息，已帮助我们为您提供会员专享服务</span></h3>
				
                     <div class="ui-form-group">
 
        <div class="ui-form-item">
	<label for="payPwd" class="ui-label">您的姓名</label>
	<input autocomplete="off" class="ui-name" type="text" id="Name" name="Name" data-error="    " data-explain="" seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2" data-validator-set="widget-3"/>
      <span id="NameVer"></span>
</div>
        <div class="ui-form-item">
	<label for="payPwdConfirm" class="ui-label">手机</label>
	<input autocomplete="off" class="ui-input" type="text" id="Phone" name="Phone" data-error="    " data-explain="。" seed="JCompleteForm-payPwdConfirm" smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-4"/>
    <span id="PhoneVer"></span>
</div>
 
					</div>	
						
						<div class="ui-form-dashed"></div>
           
		
<div class="ui-form-item">
	<div id="submitBtn" class="ui-button  ">
<input name="按钮" type="button" class="btn-login" id="btn-submit" tabindex="4" value="提交注册信息" seed="B-login-button1"/>
	</div>
	<span class="ui-form-confirm"><span id="loading"></span></span>
</div>
			            
    </form>
 
</div>

<input type="hidden" name="VEmail" id="VEmail" value="0"/>
<input type="hidden" name="VQPassword1" id="VQPassword1" value="0"/>
<input type="hidden" name="VPhone" id="VPhone" value="0">
</asp:Content>