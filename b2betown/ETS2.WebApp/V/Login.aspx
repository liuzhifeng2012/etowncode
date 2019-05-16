<%@ Page Language="C#" AutoEventWireup="true"   MasterPageFile="/V/Member.Master" CodeBehind="Login.aspx.cs" Inherits="ETS2.WebApp.V.Login" %>

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
    border-radius:5px
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
 <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            //判断密码
            $("#Account").blur(function () {
                $("#AccountVer").html(""); //离开后先清空备注
                var Account = $("#Account").val();
                if (Account == "") {
                    $("#AccountVer").html("请填写账户");
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
                    $("#PwdVer").html("填写密码");
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
                var num = $("#hid_num").val();
                if (num == 0) {
                    var Account = $("#Account").val();
                    var Pwd = $("#Pwd").val();

                    if (Account == "") {
                        $("#AccountVer").html("请填写账户");
                        $("#AccountVer").css("color", "red");
                        return;
                    }
                    else {
                        $("#AccountVer").html("√");
                        $("#AccountVer").css("color", "green");
                    }

                    if (Pwd == "") {
                        $("#PwdVer").html("填写密码");
                        $("#PwdVer").css("color", "red");
                        return;
                    } else {
                        $("#PwdVer").html("√");
                        $("#PwdVer").css("color", "green");

                    }

                    $(".loading-text").html("正在登陆信息，请稍后...")


                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Login", { comid: comid, Account: Account, Pwd: Pwd }, function (data) {
                        // data = eval("(" + data + ")");
                        //                    if (data.msg == 0) {
                        //                        $.prompt("登陆错误，请重新登陆");
                        //                        return;
                        //                    } else {

                        //                        if (data.msg == "OK") {
                        //                            location.href = "/V/Default.aspx"; 
                        //                        } else {
                        //                            $("#login_err").html(data.msg);
                        //                            $("#login_err").css("color", "red");
                        //                            return;
                        //                        }
                        //                    }
                        if (data == "OK") {
                            if (comid == 101) {
                                location.href = "/V/Default.aspx";
                            }
                            else {
                                location.href = "/UI/ShangJiaUI/ProductList.aspx";
                            }
                        } else {
                            $("#login_err").html(data);
                            $("#login_err").css("color", "red");
                            return;
                        }


                    })
                } else {
                    var phone = $("#phone").val();
                    var phonepass = $("#phonepass").val();
                    if (phone == "") {
                        $("#Span1").html("手机号码不能为空");
                        $("#Span1").css("color", "red");
                        return;
                    }
                    if (phonepass == "") {
                        $("#Span2").html("动态密码不能为空");
                        $("#Span2").css("color", "red");
                        return;
                    }
                    $(".loading-text").html("正在登陆信息，请稍后...")
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=phoneLogin", { comid: comid, phone: phone, phonepass: phonepass }, function (data) {

                        if (data == "OK") {
                            if (comid == 101) {
                                location.href = "/V/Default.aspx";
                            }
                            else {
                                location.href = "/UI/ShangJiaUI/ProductList.aspx";
                            }
                        } else {
                            //                            if (data == "3") {
                            //                             $("#login_err").html("输入错误3次错误，请30分钟后再次输入");
                            //                             $("#login_err").css("color", "red");
                            //                            }
                            $("#Span2").html(data);
                            $("#Span2").css("color", "red");
                            return;
                        }


                    })
                }

            })


            $("#recaptBtn").click(function () {
                var phone = $("#phone").val();
                $("#recaptBtn").html("获取验证码");
                if (phone != "") {

                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Webcode", { Phone: phone, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#recaptBtn").html("验证信息已发送，请注意查收");
                                //$("#confirmBtn").attr("disabled", "disabled");
                                $("#recaptBtn").removeClass("btn_code");
                                $("#recaptBtn").addClass("btn_code  disabled");
                                $("#Hid_js").html(60);
                                //                                run();
                                return;
                            }
                            if (data.msg == "No") {
                                $("#recaptBtn").html("点累了吧，请休息一会");
                                $("#recaptBtn").removeClass("btn_code");
                                $("#recaptBtn").addClass("btn_code disabled");
                                $("#Hid_js").html(60);
                                //run();
                                return;
                            }

                        }
                        else {
                            if (data.msg == "Notime") {
                                $("#recaptBtn").html("验证信息已发送，请稍后……");
                                $("#recaptBtn").removeClass("btn_code");
                                $("#recaptBtn").addClass("btn_code disabled");
                                return;
                            }
                            else {

                                $("#Span2").html("验证信息发送错误！");
                                return;
                            }

                        }

                    })
                } else {

                    $("#Span1").html("请填写手机");
                    $("#Span1").css("color", "red");
                    return;
                }
            })

        })

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
 <div class="grid-780 grid-780-border fn-clear">
 
			
    	<%--<form class="ui-form ui-form-bg" name="regCompleteForm" method="post" action="#" id="J-complete-form" novalidate="novalidate" data-widget-cid="widget-0">--%>
	
        <p class="ui-tiptext">
			登录会员账户 随时查看开卡优惠情况！
        </p>
			
                        <div class="ui-form-dashed"></div>
            <h3 class="ui-form-title"><strong>填写登录信息</strong></h3>
			
        <div class="ui-form-group" id="passlog" style=" display:block">
            <a href="#" id="phoneshow" style="padding-left:263px; font-size:12px">手机动态密码登陆</a>

            <div class="ui-form-item"><div id="login_err" style="padding-left:30px;"></div>
	            <label for="payPwd" class="ui-label">账户名</label>
	            <input autocomplete="off" class="ui-input" type="text" id="Account" name="Account" data-error="    " seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2" data-validator-set="widget-3" placeholder="手机、邮箱、卡号" ><span id="AccountVer"></span>
            </div>
            <div class="ui-form-item">
	        <label for="payPwdConfirm" class="ui-label">登录密码</label>
	        <input autocomplete="off" class="ui-input" type="password" id="Pwd" name="Pwd" data-error="    "  seed="JCompleteForm-payPwdConfirm" smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-5" placeholder="密码" ><span id="PwdVer"></span>
        </div>
		</div>	
	    
        <div class="ui-form-group"  id="phonelog" style=" display:none;">
            <a href="#" id="logshow" style="padding-left:263px; font-size:12px">使用普通方式登陆</a>

            <div class="ui-form-item"><div id="Div1" style="padding-left:30px;"></div>
	            <label for="payPwd" class="ui-label">手机号</label>
	            <input autocomplete="off" class="ui-input" type="text" id="phone" name="Account" data-error="    " seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2" data-validator-set="widget-3" placeholder="手机号" ><span id="Span1"></span>
            </div>
            <div class="ui-form-item" id="mima">
	          <label for="payPwdConfirm" class="ui-label">动态密码</label>
	          <input autocomplete="off" class="ui-input" type="password" id="phonepass" name="Pwd" data-error="    "  seed="JCompleteForm-payPwdConfirm" smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-5" placeholder="密码" />
              <a class="btn_code" href="#" id="recaptBtn">获取验证码</a>
              <span id="Span2"></span>
           </div>
        </div>

						<div class="ui-form-dashed"></div>
						
            <h3 class="ui-form-title"><strong>您还不是会员？</strong><span class="ft-orange"><a href="Reg.aspx">现在立即注册，专享更多服务优惠</a></span></h3>
 
			        				
<div class="ui-form-item">
	<div id="submitBtn" class="ui-button  ">
<input name="btn-login" type="button" class="btn-login" id="btn-login" tabindex="4" value="提交登录" seed="B-login-button1">
	</div>
	<span class="ui-form-confirm"><span class="loading-text fn-hide"></span></span>
</div>
			            
    <%--</form>--%>
   <input id="hid_num" type="hidden" value="0" />
   <%--<script type="text/javascript">
       function run() {
           var s = document.getElementById("Hid_js");
           if (s.innerHTML == 0) {
               $("#recaptBtn").html("获取验证码");
               $("#recaptBtn").removeClass("btn_code  disabled");
               $("#recaptBtn").addClass("btn_code");
               return false;
           }
           s.innerHTML = s.innerHTML * 1 - 1;
           $("#recaptBtn").html(s.innerHTML + "秒后获取验证码");
       }
       window.setInterval("run();", 1000);
</script>--%>
</div>
</asp:Content>