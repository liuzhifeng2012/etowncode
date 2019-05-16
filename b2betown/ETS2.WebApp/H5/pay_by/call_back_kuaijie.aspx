<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true" CodeBehind="call_back_kuaijie.aspx.cs" Inherits="ETS2.WebApp.H5.call_back_kuaijie" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.head-return {
width: 11px;
height: 17px;
margin: 20px 15px 0;
overflow: hidden;
display: inline-block;
background: url(/Images/return.png) no-repeat;
background-size: 11px 17px;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <!-- 公共页头  -->
      <header class="header" style="height: 80px;line-height: 80px;">
                    <h1><%=title %></h1>
        <div class="left-head">
         
             <a id="A1" href="/h5/order" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a> 
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
    <footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                 <span style="display:block; padding-bottom:5px;  line-height:20px;"><%=comname %> 服务热线：<a href="tel:<%=phone %>"><%=phone %></a></span>
          </div>
      </footer>
</asp:Content>
