<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payReturnUrl.aspx.cs" Inherits="ETS2.WebApp.tenpay.payReturnUrl"
    MasterPageFile="/H5/Order.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 公共页头  -->
    <header class="header">
                    <h1><%=title %></h1>
        <div class="left-head">
          <% if (comid == 106)
             { %>
             <a id="A1" href="../Prodefault.aspx" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
          <%}
             else
             {%>
          <a id="goBack" href="../List.aspx" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
          <%} %>
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
