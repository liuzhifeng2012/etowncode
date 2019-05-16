<%@ Page Title="" Language="C#" MasterPageFile="~/V/Member.Master" AutoEventWireup="true" CodeBehind="info.aspx.cs" Inherits="ETS2.WebApp.V.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
  <div class="main">
	<div class="row" id="subHeader">
            <div class="leftCol">
                <div class="mediumBottomMargin">
				<small><a href="http://www.vctrip.com/">首页</a> > <%=type%>
				</small>
				</div>
            </div>
     </div>
	<div class="inner">
       <div style="margin-bottom:6px">
          <h1>
            ￥<%=price%>起 -- <%=titile%></h1>
       </div>
        <p>
            <strong><span style="color: #ff0000;">
                <img title="u6.jpg" src="<%=imgurl %>"></span>
			</strong>
		</p>
		<div class="row semiLargeBottomMargin">
                <small class="darkgray">vcTrip.com微旅行<sup>®</sup> 精选推荐</small>
            </div>
		<br>		
        <p><strong><span style="font-size: medium;">￥<%=price%>起 -- <%=titile%></span>
		</strong>
		</p>
        <p><%=contxt%></p>
        <p><strong>咨询电话：</strong><span style="font-size: medium; font-family: 'arial black', 'avant garde';"><%=phone %> &nbsp;</span></p>
        <br>
    </div>			
</div>
</asp:Content>
