<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true" CodeBehind="Orderinfo.aspx.cs" Inherits="ETS2.WebApp.H5.Orderinfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title><%=title %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 公共页头  -->
      <header class="header">
                    <h1><%=title %></h1>
        <div class="left-head">
          <a id="goBack" href="Orderlist.aspx" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
    <!-- 页面内容块 -->
     <span id="tickets-url" class="fn-hide"></span>
<span id="sceneryId" class="fn-hide"></span>
<div class="content-wrap">
  <article class="scenerys">
  <div class="body">
    <div class="pic">
      <a href="">
      <img src="<%=headPortraitImgSrc %>" alt="<%=title %>" title="<%=title %>"></a>
      <div class="pic-info">
        <%--10张--%>
      </div> 
      <%--<div class="info-bg">
      </div>--%>
    </div>
    <section class="scenerys-cont">
        <a href="OrderinfoTitle.aspx?id=<%=id %>">
        <h3><%=title %></h3><em style="position: absolute;right: 30px;bottom: 45px;color: #1a9ed9;"></em></a>
        <p class="scenerys-level"></p>
      <p class="scenery-conment">
        <%--<a class="comment-url" href="#"><em class="url-ico"></em></a>--%>
        <em class="ico-like"></em><%=remark%>
      </p>
    </section>
    <div class="clear"></div>
    </div>
    <%--<div class="bottomBox">
      <a href="#"><span class="map"></span>地图查询地址<em class="gray-ico"></em></a>   </div> --%>
  </article>
</div>

<div class="content-wrap">
  <div class="noticeTip fn-clear" id="noticeTip"><%=author%><%--<font class="font-orange">当天8:00</font>--%><%--<a href="#" class="notice-url">预订须知<em class="url-ico"></em></a>--%></div>
  <article class="tickets" id="tickets">
  <section class="ticket"><a href="OrderEnter.aspx?id=<%=id %>" class="btn">预订</a>
  <h4><%=title %></h4>
  <span class="price left">原价：<%=price%></span>
  <span class="tc-price right">会员价：<em><%=advise_price%></em></span></br>
  <span class="pay-online left">在线支付</span>
  </section>
  </article>
</div>

</asp:Content>
