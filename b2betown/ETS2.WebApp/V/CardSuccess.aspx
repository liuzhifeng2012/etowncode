<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/V/Member.Master" CodeBehind="CardSuccess.aspx.cs" Inherits="ETS2.WebApp.V.CardSuccess" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
 
 <div class="grid-780 grid-780-border fn-clear">
 
			
    	<form class="ui-form ui-form-bg" name="regCompleteForm" method="post" action="#" id="J-complete-form" novalidate="novalidate" data-widget-cid="widget-0">
	
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
	<label for="payPwdConfirm" class="ui-label">电子邮箱</label>
	<%=Email%></div>
        <div class="ui-form-item">
	<label for="payPwdConfirm" class="ui-label"></label>
       </div>
 
					</div>	
		  <p style="margin-left:100px" >  祝贺您卡开成功！您是我们尊贵的会员，请 <b><a href="login.aspx" style="color:#FF6600">  登 录 </a> </b> 查询更多会员专享服务</p>
       
 
		        <div class="ui-form-dashed"></div>
 
 
		  <div class="ui-form-item">
	<div id="submitBtn" class="ui-button  ui-button-morange "></div>
	</div>
												
			
			            
    </form>
 
</div>
 

</asp:Content>