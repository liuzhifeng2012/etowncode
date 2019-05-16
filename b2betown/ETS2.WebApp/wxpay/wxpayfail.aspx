<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxpayfail.aspx.cs" Inherits="ETS2.WebApp.wxpay.wxpayfail" 
 MasterPageFile="/H5/Order.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 公共页头  -->
    <header class="header">
                    <h1>支付完成页面</h1>
        <div class="left-head">
          <a href="javascript:history.go(-1);" class="tc_back head-btn">
          <span class="inset_shadow"><span  ></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
    <!-- 页面内容块 -->
    <div class="body">
        <div class="body-content">
            <dl>
                <dd>
                    支付出现错误!</dd>
            </dl>
        </div>
    </div>
    <footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                  <span style="display:block; padding-bottom:5px;  line-height:20px;">服务热线：<a href="tel:59059150">59059150</a></span>
          </div>
      </footer>
</asp:Content>
