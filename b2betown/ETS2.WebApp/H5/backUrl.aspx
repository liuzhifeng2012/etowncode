<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true" CodeBehind="backUrl.aspx.cs" Inherits="ETS2.WebApp.H5.backUrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <!-- 公共页头  -->
      <header class="header" style="background-color: #3CAFDC;">
                    <h1><%=title %></h1>
        <div class="left-head">
          <a id="goBack" href="../H5/Orderlist.aspx" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
    <footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                 <span style="display:block; padding-bottom:5px;  line-height:20px;">服务热线：<a href="tel:<%=phone %>"><%=phone %></a></span>
          </div>
      </footer>
</asp:Content>
