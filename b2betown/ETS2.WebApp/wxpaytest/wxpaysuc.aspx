<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxpaysuc.aspx.cs" Inherits="ETS2.WebApp.wxpaytest.wxpaysuc"
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
                    恭喜你，支付成功!</dd>
            </dl>
            <%--    <dl>
                <dt>商家：</dt>
                <dd>
                </dd>
            </dl>
            <dl>
                <dt>手机号：</dt>
                <dd>
                </dd>
            </dl>
            <dl>
                <dt>数量：</dt>
                <dd>
                </dd>
            </dl>
            <dl>
                <dt>订单金额：</dt>
                <dd class="font-orange">
                </dd>
            </dl>--%>
        </div>
    </div>
    <footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                  <span style="display:block; padding-bottom:5px;  line-height:20px;">服务热线：<a href="tel:59059150">59059150</a></span>
          </div>
      </footer>
</asp:Content>
