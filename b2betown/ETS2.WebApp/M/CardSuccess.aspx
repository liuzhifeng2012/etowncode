<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/M/MemberH5.Master" CodeBehind="CardSuccess.aspx.cs" Inherits="ETS2.WebApp.M.CardSuccess" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
<title>微旅行 无V不至</title> 
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
<div id="header">
		<span class="left btn_back" onClick="goBack();"></span> 
		<!-- <span class="left btn_set"></span> -->设置我的微旅行会员帐号<!-- <span class="right btn_more"></span> --> 
	</div> 
 
	<div class="info_wrapper"> 
		<form action="" id="loginForm" method="post"> 
<p class="ui-tiptext">
			成功开卡啦！
        </p>
					
			            <div class="ui-form-dashed"></div>
						
            <h3 class="ui-form-title"><strong> 持卡人信息：</strong></h3>
				
                     <div class="ui-form-group">
 
        <div class="ui-form-item">
	<label for="payPwd" class="ui-label">您的姓名</label>
	<%=Name_temp%>        </div>
	  <div class="ui-form-item">
	<label for="payPwd" class="ui-label">您的卡号</label>
	<%=IDCard%>        </div>
        <div class="ui-form-item">
	<label for="payPwdConfirm" class="ui-label">手机号</label>
	<%=Phone%></div>
        <div class="ui-form-item">
        <div class="ui-form-item">
	<label for="payPwdConfirm" class="ui-label"></label>
       </div>
 
					</div>	
		  <p style="margin-left:100px" >  祝贺您卡开成功！您是我们尊贵的会员，请 <b><a href="login.aspx" style="color:#FF6600">  登 录 </a> </b> 查询更多会员专享服务</p>
       
		</form> 
	</div> 

</asp:Content>