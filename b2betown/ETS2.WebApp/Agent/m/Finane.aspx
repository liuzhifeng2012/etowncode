<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Finane.aspx.cs" Inherits="ETS2.WebApp.Agent.m.Finane"
    MasterPageFile="/Agent/m/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8">
    <meta name="keywords" content="微商城">
    <meta name="description" content="">
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">
    <title> </title>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!-- CSS -->
    <link rel="stylesheet" href="/agent/m/css/cart.css" >
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/morder.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comidtemp").trimVal();

            $("#div_comname").html("商户:" + $("#hid_company").trimVal());

            $("#h_comname").text($("#hid_company").trimVal());
            SearchList(1);

            function SearchList(pageindex) {
                $.post("/JsonFactory/AgentHandler.ashx?oper=agentFinacelist", { comid: comid, agentid: agentid, pageindex: pageindex, pagesize: pageSize }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $(".empty-list").show();
                        $("#backs-list-container").find("li").hide();
                        return;
                    }
                    if (data.type == 100) {

                        if (data.totalCount == 0) {
                            if (pageindex == 1) {
                                //                                    $("#list").html("查询数据为空");
                                $(".empty-list").show();
                                $("#backs-list-container").find("li").hide();
                            }
                        } else {
                            if (pageindex == 1) {
                                $("#list").empty();
                            }

                            stop = true;
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                            $("#hid_pageindex").val(pageindex);

                            $(".empty-list").hide();
                            $("#backs-list-container").find("li").show();
                        }

                    }
                })
            }

            var stop = true;
            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#hid_pageindex").val()) + 1;

                        stop = false;
                        SearchList(pageindex);
                    }
                }
            });
        })
        function gobackprolist() {
            location.href = "/agent/m/Manage_sales.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goshopproject() {
            location.href = "/agent/m/Manage_sales.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goshopcart() {
            location.href = "/agent/m/ShopCart.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goorder() {
            location.href = "/agent/m/order.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function gofinane() {
            location.href = "/agent/m/Finane.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <header class="header"  style=" background-color: #3CAFDC;">
          <h1 id="h_comname"></h1>
        <div class="left-head"> 
              <a href="/agent/m/default.aspx" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="home-10"></span></span>
              </a> 
            </div>
        <div class="right-head"> 
                <a href="loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>  
        </div>
        </header>
    <!-- container -->
    <div class="container ">
        <div class="tabber  tabber-n3 tabber-double-11 clearfix">
             <a href="javascript:goshopproject()"   >产品列表</a><a class="" href="javascript:goorder()">订单列表</a>
            <a href="javascript:gofinane()" class="active">财务记录</a> <a href="javascript:goshopcart()">购物车</a>
        </div>
        <div class="layout-full-box pr mt10" id="div_comname">
        </div>
        <div id="backs-list-container" class="block">
            <li class="block block-order animated">
                <div class="block block-list block-border-top-none block-border-bottom-none" id="list"
                    style="padding-left: 0;">
                </div>
            </li>
            <div class="empty-list" style="margin-top: 30px;">
                <!-- 文本 -->
                <div>
                    <h4>
                        哎呀，暂不记录？</h4>
                </div>
                <!-- 自定义html，和上面的可以并存 -->
                <div>
                    <a href="#" onclick="javascript:gobackprolist();" class="tag tag-big tag-orange"
                        style="padding: 8px 30px; color: #F15A0C; text-decoration: underline;">去逛逛</a></div>
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
    <div class="layout-box">
        <ul class="list-a">
            <li><span>流水：${Id}</span><span style="float: right;">${jsonDateFormat(Subdate)}</span>  </li>
            <li>内容： ${Servicesname}  {{if pno!=""}}  [${pno}]  {{/if}} 
             ${Payment_type}${Remarks}</li>
            <li>收入：<strong class="r1">&nbsp; {{if Money>= 0}}${Money}元{{/if}}</strong></li>
            <li>支出：<strong class="r1">&nbsp;{{if Money< 0}}${Money}元{{/if}}</strong></li>
             <li>余额：<strong class="r1">&nbsp;${Over_money}元</strong></li>   
        </ul>
    </div> 
    </script>
    <input id="hid_pageindex" type="hidden" value="1" />
</asp:Content>
