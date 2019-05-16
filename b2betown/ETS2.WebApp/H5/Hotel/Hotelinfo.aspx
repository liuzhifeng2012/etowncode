<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true" CodeBehind="Hotelinfo.aspx.cs" Inherits="ETS2.WebApp.H5.Hotelinfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .intro {padding: 10px 0;}
        .introTop {background-color: white;line-height: 20px;margin: 0 10px 0 10px;padding: 10px;}
        .introTop h2{ font-weight:bold}
    </style>
    <link href="../../Styles/H5/Order.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#submitBtn1").click(function () {
                window.location.href = "Hotelshow.aspx?id=" + $("#com_id").val();
            })
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <!-- 公共页头  -->
    <%--<header class="header">
    <a  style="margin-top:-0px;" id="goBack" href="javascript:history.go(-1);" class="tc_back head-btn">
                  <h1><%=title%></h1>
      <div class="left-head">
        <span class="inset_shadow"><span class="head-return"></span></span>

      </div></a>
      <div class="right-head">
        <a href="#" class="head-btn"><span class="inset_shadow"></span></a>
      </div>
    </header>--%>
    <a id="goBack" href="javascript:history.go(-1);">
            <header class="header">
                    <h1><%=title%></h1>
        <div class="left-head">
          <a  href="javascript:history.go(-1);" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
        </a>
    <!-- 页面内容块 -->
    
<div class="intro">
    <div class="introTop">
        <h2>商家介绍：</h2><p><%=article%></p></br><p><%=serviceinfo%></p>
        <h2>公交路线：</h2><p><%=Scenic_Takebus%></p>
        <h2>开车路线：</h2><p><%=Scenic_Drivingcar%></p>
    </div>
    <div class="order-btn fn-clear">
            <div class="submit-btn">
                <input type="button" class="btn" id="submitBtn1" value="查看酒店房型"/>
            </div>
        </div>
</div>
<input id="com_id" type="hidden"  value="<%=comid %>" />
</asp:Content>
