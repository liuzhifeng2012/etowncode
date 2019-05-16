<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="LoginF.aspx.cs" Inherits="ETS2.WebApp.V.LoginF" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="/Styles/login.css" />
    <link href="../Scripts/Impromptu.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>



    <script type="text/javascript">
        $(function () {
            var comid=<%=comid %>;
            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#btn-submit").click();    //这里添加要处理的逻辑  
                }
            });

            $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })
            
            $("#validateCodetext").click(function () {
               $("#validateCode").attr("src","/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime()); 
            })


            //判断密码
            $("#Account").blur(function () {
                $("#AccountVer").html(""); //离开后先清空备注
                var Account = $("#Account").val();
                if (Account == "") {
                    $("#error-box").html("请填写账户");
                    $("#error-box").css("color", "red");
                    return;
                }
            })

            //判断密码
            $("#Pwd").blur(function () {
                $("#PwdVer").html(""); //离开后先清空备注
                var Pwd = $("#Pwd").val();
                if (Pwd == "") {
                    $("#error-box").html("填写密码");
                    $("#error-box").css("color", "red");
                    return;
                } 
            })

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#btn-submi").click();    //这里添加要处理的逻辑  
                }
            });


            //提交按钮
            $("#btn-submit").click("click", function () {
                var Account = $("#Account").val();
                var Pwd = $("#Pwd").val();

                var come =$("#hid_come").val();

                if (Account == "") {
                    $("#error-box").html("请填写账户");
                    $("#error-box").show();
                    return;
                }

                if (Pwd == "") {
                    $("#error-box").html("填写密码");
                    $("#error-box").show();
                    return;
                } 
                
                if (getcode == '') {
                    $("#error-box").html("验证码不可为空");
                    $("#error-box").show();

                    return;
                }



                //$(".loading-text").html("正在登陆信息，请稍后...")
                 $("#loading").show();
                //登陆
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Login", { comid:comid, Account: Account, Pwd: Pwd }, function (data) {
                    if (data == "OK") {
                       if(come=="")
                       {
                         window.open("/UI/ShangJiaUI/ProductList.aspx", target = "_parent"); 
                       }
                       else
                       {
                          window.open(come, target = "_parent"); 
                       }
                       $("#loading").hide();
                    } else {
                        $("#error-box").html(data);
                        $("#error-box").show();
                        $("#loading").hide();
                        return;
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
			<h1>登录会员账户</h1>
							
		</div>
		<div class="error-box " id="error-box" style="display:none;"></div>
		<div class="login-form-cnt">

			<fieldset>
				<div class="fm-item ui-form-item sl-account">
	<label class="fm-label ui-form-label" for="logonId">账户名：</label>
	
							<input name="Account" type="text" class="i-text ui-input" id="Account" size="20"
                                    maxlength="250" placeholder="电子邮件、卡号、手机"/>
			
    
            
</div>

		<div class="fm-item ui-form-item sl-aliedit">
		<label class="fm-label pwd-label ui-form-label" desc="登录密码">登录密码：</label>
							<span class="fm-link">
										<!--<a id="forgetPsw" href="#" target="_blank" seed="auth-findPWD">忘记登录密码？</a>-->
				            </span>
						
	  <input name="Pwd" type="password" class="i-text ui-input" id="Pwd" size="20"
                                maxlength="20"  placeholder="密码"/>

    </div>
              		
        		
	<div class="fm-item ui-form-item sl-checkcode">
		<label class="fm-label ui-form-label">验证码：</label>
		<div>
			 <input name="getcode" type="text" placeholder="验证码" id="getcode" size="10" class="i-text i-text-authcode ui-input sl-checkcode-input" />

						  <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码" />
                           <a href="javascript:;" id="validateCodetext">点击更换</a>
						
                      

								

            		</div>
	</div>
        
		
								<div class="fm-item ui-form-item ui-btn-cnt">
					<input name="按钮"   type="button" class="btn-login" id="btn-submit" tabindex="4" value="登  录" seed="B-login-button1">
				</div>

								<p class="login-help-cnt">
										
										您还不是会员？现在<a href="/V/reg.aspx"  class="ui-button-reg" seed="B-login-reg1" target="_blank">立即注册</a>，专享更多服务优惠

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
    <input type="hidden" id="hid_come" value="<%=comeurl %>" />
</body>
</html>
