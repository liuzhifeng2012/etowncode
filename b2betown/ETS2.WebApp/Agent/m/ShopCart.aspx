<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShopCart.aspx.cs" Inherits="ETS2.WebApp.Agent.m.ShopCart"
    MasterPageFile="/Agent/m/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8">
    <meta name="keywords" content="微商城">
    <meta name="description" content="">
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">
    <title></title>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!-- CSS -->
    <link rel="stylesheet" href="/agent/m/css/cart.css" onerror="_cdnFallback(this)">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/morder.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comidtemp").trimVal();

            $("#div_comname").html("商户:" + $("#hid_company").trimVal());
            $("#h_comname").text($("#hid_company").trimVal());
            SearchList();

            function SearchList() {
                $.post("/JsonFactory/OrderHandler.ashx?oper=agentcartlist", { agentid: agentid, comid: comid }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                        $.prompt("查询渠道列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        if (data.msg == "") {
                            //                            $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                            $(".empty-list").show();
                            $("#backs-list-container").find("li").hide();
                        } else {
                            $(".c-orange").text(data.totalprice);
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            $(".empty-list").hide();
                            $("#backs-list-container").find("li").show();
                        }
                    }
                })
            }



            $("#confirmButton").click(function () {
//                var proid = "";

//                $("[name='proid']").each(function () {
//                    proid += $(this).val() + ",";
//                })

//                if (proid == "") {
//                    showErr("请选择结算的产品");
//                    return;
                //                }
                var proid = "";

                $("[name='Id']").each(function () {
                    if ($(this).attr("checked")) {
                        proid += $(this).val() + ",";
                    }
                })

                if (proid == "") {
                    showErr("请选择结算的产品");
                    return;
                }


                location.href = "ShopCartSales.aspx?comid=" + comid + "&agentid=" + agentid + "&proid=" + proid;


            })
        })
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
        


        function gobackprolist() {
            location.href = "/agent/m/Manage_sales.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function addnum(proid, money) {
            var num = $("#u_num" + proid).val();
            num++;
            $("#u_num" + proid).val(num);
            addcart(proid);

            $("#sum" + proid).html(num * money);

            GetTotalPrice();
        }

        function reducenum(proid, money) {
            var num = $("#u_num" + proid).val();
            num--;
            if (num == 0) {
                if (confirm("是否产品移除购物车?")) {

                    $.post("/JsonFactory/OrderHandler.ashx?oper=agentdelcart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comidtemp").trimVal(), proid: proid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                        }
                        if (data.type == 100) {
                            //判断购物车中是否含有产品，不含有需要刷新一次页面
                            if (data.total == 0) {
                                location.reload();
                            } else {
                                $("#prolist" + proid).remove();
                            }
                            return;
                        }
                    })
                }
            }

            $("#u_num" + proid).val(num);
            $("#sum" + proid).html(num * money);
            GetTotalPrice();


            if (num <= 0) {
                num = 1;

            }

            reducecart(proid, num);


        }

        function addcart(proid) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comidtemp").trimVal(), proid: proid, u_num: 1 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    //$("#cart").show(); 
                }
            })
        }
        function reducecart(proid, num) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentreducecart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comidtemp").trimVal(), proid: proid, u_num: num }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    //$("#cart").show();
                 


                }
            })
        }

        function GetTotalPrice() {
            $(".c-orange").text("0");
            $("[name='sum']").each(function () {
                //                alert($(this).text());
                var lasttotal = parseInt($(".c-orange").text());
                $(".c-orange").text(lasttotal + parseInt($(this).text()));
            });
        }

        function showErr(a) {
            $("html").css({
                "overflow-y": "hidden"
            });
            if ($("#bgDiv").html() == null) {
                $('<div id="bgDiv"></div>').appendTo("body")
            }
            if ($("#showMsg").html() != null) {
                $("#showMsg").remove()
            }
            $('<div id="showMsg"><div class="msg-title">温馨提示</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr()">知道了</a></div></div>').appendTo("body");
            var b = $(window).height();
            var d = $(window).scrollTop();
            var c = $("#showMsg").height();
            $("#bgDiv").css({
                height: $(document).innerHeight()
            }).show();
            $("#showMsg").css({
                top: (b - c) / 2
            }).show()
        }
        function hideErr() {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #showMsg").hide();

        }
        function openlink(id) {
            var comid = $("#hid_comid_temp").trimVal();
            location.href = "pro_sales.aspx?id=" + id + "&comid=" + $("#hid_comidtemp").trimVal();
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
            <a href="javascript:goshopproject()">产品列表</a> <a class="" href="javascript:goorder()">
                订单列表</a> <a class="" href="javascript:gofinane()">财务记录</a><a class="active" href="javascript:goshopcart()">购物车</a>
        </div>
        <div class="layout-full-box pr mt10" id="div_comname">
        </div>
        <div id="backs-list-container" class="block">
            <li class="block block-order animated">
                <div class="block block-list block-border-top-none block-border-bottom-none" id="tblist">
                </div>
                <hr class="margin-0 left-10">
                <div class="bottom">
                    小计(不含运费)：¥<span class="c-orange"></span>
                    <div class="opt-btn">
                        <a class="btn btn-orange btn-in-order-list" href="#" id="confirmButton">结算</a>
                    </div>
                </div>
            </li>
            <div class="empty-list" style="margin-top: 30px;">
                <!-- 文本 -->
                <div>
                    <h4>
                        哎呀，购物车？</h4>
                    <%--  <p class="font-size-12">
                        不落单一起团</p>--%>
                </div>
                <!-- 自定义html，和上面的可以并存 -->
                <div>
                    <a href="#" onclick="javascript:gobackprolist();" class="tag tag-big tag-orange"
                        style="padding: 8px 30px; color: #F15A0C; text-decoration: underline;">去逛逛</a></div>
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
    {{if Agent_price !=0}}
     <div  id="prolist${Id}" class="block-item name-card name-card-3col clearfix" >
        <input id="pro${Id}" name="proid" value="${Id}" type="hidden" >
        <input class="sn-input-checked" type="checkbox" checked="true" value="${Id}" name="Id">
        <a href="javascript:void(0);" class="thumb">
            <img src="${ProImg}" width="50px" height="50px">
        </a>
        <div class="detail">
            <a href="javascript:void(0);">
                <h3 style="width:60%;">
                    ${Pro_name}{{if ishasdeliveryfee==0}}包邮{{/if}}</h3>
            </a>
            {{if Server_type ==11}}
             {{else}}
            <p class="sku-detail ellipsis js-toggle-more">
                <span class="c-gray">产品有效期:${ChangeDateFormat(Pro_end)} </span>
            </p>
            {{/if}}
        </div>
        <div class="right-col">
            <div class="price">
                ¥<span> ${Agent_price}</span></div>
            <div class="num">
                
                <div class="wrap-input">
                    <a href="javascript:void(0);" class="btn-reduce" onclick="reducenum('${Id}','${Agent_price}')">减少数量</a>
                    <input id="u_num${Id}" name="u_num" value="${U_num}" class="input-ticket" autocomplete="off"
                        maxlength="4" size="5" type="text">
                    <a href="javascript:void(0);" class="btn-add" onclick="addnum('${Id}','${Agent_price}')" >增加数量</a>
                        <span id="sum${Id}" name="sum" style="padding-left:10px;display:none;">${U_num*Agent_price} </span> 
                </div>
            </div>
        </div>
     </div>
        {{/if}} 
    </script>
    <div id='cart' style="display: none; position: absolute; bottom: 6em; right: 2em;
        width: 55px; height: 55px; background-color: #FFFAFA; margin: 10px; border-radius: 60px;
        border: solid rgb(55,55,55)  #FF0000   1px; cursor: pointer;">
        <img src="/images/cart.gif" width="39" style="padding: 8px 0 0 9px;" />
    </div>
</asp:Content>
