<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/M/MemberH5.Master" CodeBehind="Login.aspx.cs" Inherits="ETS2.WebApp.M.Login" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
<title>微旅行 无V不至</title> 
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            //判断密码
            $("#Account").blur(function () {
                $("#AccountVer").html(""); //离开后先清空备注
                $("#login_err").html("");
                var Account = $("#Account").val();
                if (Account == "") {
                    $("#AccountVer").html("请填写账户");
                    $("#AccountVer").css("color", "red");
                    return;
                } 
            })

            //判断密码
            $("#Pwd").blur(function () {
                $("#PwdVer").html(""); //离开后先清空备注
                $("#login_err").html("");
                var Pwd = $("#Pwd").val();
                if (Pwd == "") {
                    $("#PwdVer").html("填写密码");
                    $("#PwdVer").css("color", "red");
                    return;
                } 
            })

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#btn-login").click();    //这里添加要处理的逻辑  
                }
            });


            //提交按钮
            $("#confirmBtn").click("click", function () {
                var Account = $("#Account").val();
                var Pwd = $("#Pwd").val();

                if (Account == "") {
                    $("#AccountVer").html("请填写账户");
                    $("#AccountVer").css("color", "red");
                    return;
                }

                if (Pwd == "") {
                    $("#PwdVer").html("填写密码");
                    $("#PwdVer").css("color", "red");
                    return;
                } 

                $(".loading-text").html("正在登陆信息，请稍后...")

                //创建订单
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Login", { comid: comid, Account: Account, Pwd: Pwd }, function (data) {

                    if (data == "OK") {
                            location.href = "/M/Default.aspx"; 
                        } else {
                            $("#login_err").html(data);
                            $("#login_err").css("color", "red");
                            return;
                        }
                })

            })

        })

    </script>

</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
<div id="header">
		<span class="left btn_back" onClick="goBack();"></span> 
		登陆我的微旅行会员帐号
	</div> 
 
	<div class="info_wrapper"> 
		<form action="" id="loginForm" method="post"> 
        <div id="login_err" style="padding-left:30px;"></div>
			<p class="input_group"> 
			<span id="AccountVer"></span>
				<input id="Account" name="Account" type="text" placeholder="输入您的卡号、手机号" required> 
			</p> 
			<p class="input_group"> 
			<span id="PwdVer"></span>
				<input id="Pwd" name="Pwd" type="password" placeholder="密码" required> 
			</p> 
			<p><a href="javascript:;" class="btn_big btn_blue" id="confirmBtn">确定</a></p> 
		</form> 
		  <h3 class="ui-form-title"><strong>您还不是会员？</strong><span class="ft-orange"><a href="Card.aspx">现在立即开卡，专享更多服务优惠</a></span><%=pass %></h3>
	</div> 
</asp:Content>