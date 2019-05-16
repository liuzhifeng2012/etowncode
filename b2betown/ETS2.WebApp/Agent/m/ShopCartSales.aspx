<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShopCartSales.aspx.cs"
    Inherits="ETS2.WebApp.Agent.m.ShopCartSales" MasterPageFile="/Agent/m/Site1.Master" %>

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
        Math.formatFloat = function (f, digit) {
            var m = Math.pow(10, digit);
            return parseInt(f * m, 10) / m;
        }
        $(function () {
            //使用常用地址
            $("#a_ChooseAddress").click(function () {
                window.open("CommonAddrlist.aspx?isshopcart=1&proid=" + $("#hid_proid").trimVal() + "&unum=0&comid=" + $("#hid_comidtemp").trimVal());
            })

            $("#div_comname").html("商户:" + $("#hid_company").trimVal());

            $("#h_comname").text($("#hid_company").trimVal());
            SearchList();

            $("#submitBtn1").click(function () {
                var agentid = $("#hid_agentid").trimVal();
                var proid = $("#hid_proid").trimVal();
                var num = $("#hid_num").trimVal();
                var comid = $("#hid_comidtemp").trimVal();

                var proid = $("#hid_proid").trimVal();
                var u_num = $("#hid_num").trimVal();
                var u_name = $("#u_name").trimVal();
                var u_phone = $("#u_phone").trimVal();
                var payprice = $("#hid_subtotal").val();


                var deliverytype = $("input:radio[name='deliverytype']:checked").trimVal();
                var province = $("#com_province").trimVal();
                var city = $("#com_city").trimVal();
                var address = $("#txtaddress").trimVal();
                var txtcode = $("#txtcode").trimVal();

                if (u_name == "") {
                    showErr("请填写接收人姓名");
                    return;
                }
                if (u_phone == "") {
                    showErr("请填写接收人手机号，来接收电子票短信");
                    return;
                } else {
                    if (!isMobel(u_phone)) {
                        showErr("请正确填写接收人手机号");
                        return;
                    }
                }

                if (deliverytype == 2) {
                    if (province == "省份") {
                        showErr("请选择省份");
                        return;
                    }
                    if (city == "城市") {
                        showErr("请选择城市");
                        return;
                    }
                    if (address == "") {
                        showErr("请输入详细地址");
                        return;
                    }

                }

                //以下参数暂时没有用到
                var saveaddress = "";
                var u_traveldate = "";
                var travelnames = ""; //乘车人姓名列表
                var travelidcards = ""; //乘车人身份证列表
                var travelnations = ""; //乘车人民族列表
                var travelphones = ""; //乘车人联系电话列表
                var travelremarks = ""; //乘车人备注列表
                var travel_pickuppoints = "";
                var travel_dropoffpoints = "";
                var order_remark = "";


                //创建订单
                $('#confirmButton').hide().after('<span id="spLoginLoading" style="margin-left:10px;color:#f39800; font-size:16px;">提交中……</span>');
                $.post("/JsonFactory/OrderHandler.ashx?oper=agentcartorder", { deliverytype: deliverytype, province: province, city: city, address: address, txtcode: txtcode, agentid: agentid, comid: comid, proid: proid, u_num: u_num, u_name: u_name, u_phone: u_phone, u_traveldate: u_traveldate, travelnames: travelnames, travelidcards: travelidcards, travelnations: travelnations, travelphones: travelphones, travelremarks: travelremarks, travel_pickuppoints: travel_pickuppoints, travel_dropoffpoints: travel_dropoffpoints, order_remark: order_remark, saveaddress: saveaddress, payprice: payprice }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        if (data.msg == "yfkbz") {
                            alert("您的预付款不足，先充足预付款后再提交订单");

                            //创建支付订单
                            $.post("/JsonFactory/OrderHandler.ashx?oper=agentRecharge", { agentid: agentid, comid: comid, payprice: data.money, u_name: "分销客户:" + u_name, u_phone: u_phone }, function (data1) {
                                data1 = eval("(" + data1 + ")");
                                if (data1.type == 1) {
                                    showErr(data1.msg);
                                    return;
                                }
                                if (data1.type == 100) {
                                    location.href = "/h5/pay.aspx?orderid=" + data1.msg + "&comid=" + comid + "&act=cart";
                                    return;
                                }
                            })
                            //$('#confirmButton').show();
                            $('#spLoginLoading').hide();
                        } else {
                            showErr(data.msg);
                            $('#confirmButton').show();
                            $('#spLoginLoading').hide();
                        }

                        return;
                    }
                    if (data.type == 100) {
                        showErr("提单成功");
                        return;
                    }
                })

            })

            $("#com_city").change(function () {

                var city = $("#com_city").trimVal();
                if (city == "城市") {
                    showErr("请选择送达城市");
                    return;
                }
                //根据产品id和城市获得运费
                var proid = $("#hid_proid").trimVal();
                var num = $("#u_num").trimVal();


                getDeliveryFee(proid, num, city);
            })

            $("input:radio[name='deliverytype']").click(function () {
                var chked = $("input:radio[name='deliverytype']:checked").val();
                if (chked == 2)//快递
                {
                    $("#delivery_tr1").show();
                    $("#delivery_tr2").show();
                    $("#delivery_tr3").show();
                    $("#delivery_tr4").show();
                } else {//自提
                    $("#delivery_tr1").hide();
                    $("#delivery_tr2").hide();
                    $("#delivery_tr3").hide();
                    $("#delivery_tr4").hide();
                }

                getDeliveryFee("", "", "城市");
            })


            //实物产品常用地址,针对实物产品
            var addrid = $("#hid_addrid").trimVal();
            if (addrid != 0) {
                $.post("/JsonFactory/OrderHandler.ashx?oper=getagentaddrbyid", { addrid: addrid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        $("#u_name").val(data.msg.U_name);
                        $("#u_phone").val(data.msg.U_phone);
                        $("input:radio[name='deliverytype'][value='2']").attr("checked", true);
                        $("#com_province").val(data.msg.Province);
                        $("#com_city").append('<option value="' + data.msg.City + '" selected="selected">' + data.msg.City + '</option>');
                        $("#txtaddress").val(data.msg.Address);
                        $("#txtcode").val(data.msg.Code);


                        //根据产品id和城市获得运费
                        var proid = $("#hid_proid").trimVal();
                        var num = $("#u_num").trimVal();


                        getDeliveryFee(proid, num, city);
                    }
                })
            }
        })
        function openlink(id) {
            var comid = $("#hid_comid_temp").trimVal();
            location.href = "pro_sales.aspx?id=" + id + "&comid=" + $("#hid_comidtemp").trimVal();
        }
        function SearchList() {
            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=agentcartlist",
                data: { proid: $("#hid_proid").trimVal(), agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comidtemp").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                            $.prompt("查询错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        if (data.totalCount == 0) {
                            //                            $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                            $(".empty-list").show();
                        } else {
                            $("#hid_subtotal").val(data.totalprice);
                            //小计(不含运费)
                            $(".c-orange").text(data.totalprice);
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            getDeliveryFee($("#hid_proid").trimVal(), $("#hid_num").trimVal(), $("#com_city").trimVal());
                        }


                    }
                }
            })

        }

        function getDeliveryFee(proid, num, city) {
            if (city == "城市") {
                $("#bfee").html("0");

                $("#b1").html("¥" + $("#hid_subtotal").val());
                $("#b2").html("+¥" + "0");
                $("#hid_payprice").val($("#hid_subtotal").val());
               
            } else {
                var chked = $("input:radio[name='deliverytype']:checked").val();
                $.post("/JsonFactory/OrderHandler.ashx?oper=getshopcartexpressfee", { proidstr: $("#hid_proid").val(), numstr: $("#hid_num").val(), citystr: $("#com_city").val() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {

                        if (chked == 4) {
                            $("#bfee").html("0");

                            $("#b1").html("¥" + $("#hid_subtotal").val());
                            $("#b2").html("+¥" + "0");
                            $("#b3").html("¥" + Math.formatFloat(parseFloat($("#hid_subtotal").trimVal()), 2));

                        } else {
                            $("#bfee").html(data.msg);

                            $("#b1").html("¥" + $("#hid_subtotal").val());
                            $("#b2").html("+¥" + data.msg);
                            $("#b3").html("¥" + Math.formatFloat(parseFloat(data.msg) + parseFloat($("#hid_subtotal").trimVal()), 2));

                        }


                    }
                })
            }
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
            $('<div id="showMsg"><div class="msg-title">温馨提示</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr(\'' + a + '\')">知道了</a></div></div>').appendTo("body");
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
        function hideErr(a) {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #showMsg").hide();
            if (a == '提单成功') {
                location.href = "/agent/m/order.aspx?comid=" + $("#hid_comidtemp").trimVal();
            }
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
    <style type="text/css">
        .btn_code
        {
            line-height: 30px;
            display: inline-block;
            padding: 0 5px;
            color: #6d6d6d;
            border: 1px solid #d1d1d1;
            background-color: #e6e6e6;
            background-image: -webkit-gradient(linear,left top,left bottom,from(#f8f8f8),to(#e6e6e6));
            background-image: -webkit-linear-gradient(top,#f8f8f8,#e6e6e6);
            background-image: linear-gradient(to bottom,#f8f8f8,#e6e6e6);
            border-radius: 5px;
        }
        .w-item
        {
            padding: 0px 10px;
            background: rgb(255, 255, 255);
            margin-bottom: 10px;
            line-height: 1.5em;
            border-radius: 4px;
            box-shadow: rgb(203, 205, 205) 0px 1px 1px;
            color: rgb(109, 109, 109);
        }
        .in-item
        {
            line-height: 44px;
        }
        .mi-input
        {
            border-width: 1px;
            border-style: solid;
            border-color: #A6A6A6 #CCC #CCC;
            -moz-border-top-colors: none;
            -moz-border-right-colors: none;
            -moz-border-bottom-colors: none;
            -moz-border-left-colors: none;
            border-image: none;
            color: #4D4D4D;
            font: 14px tahoma,arial,Hiragino Sans GB,宋体;
            padding: 6px 4px;
            vertical-align: top;
            width: 180px;
            height: 38px;
        }
        .in-item table input[type="text"]
        {
            width: 100%;
            margin-right: -15px;
            height: 44px;
            border: 0px;
            border-image-source: initial;
            border-image-slice: initial;
            border-image-width: initial;
            border-image-outset: initial;
            border-image-repeat: initial;
            background: 0px 50%;
            color: rgb(26, 158, 217);
            outline: 0px;
            -webkit-box-shadow: none;
            border-radius: 0px;
        }
        .in-item table input[type="tel"]
        {
            width: 100%;
            margin-right: -15px;
            height: 44px;
            border: 0px;
            border-image-source: initial;
            border-image-slice: initial;
            border-image-width: initial;
            border-image-outset: initial;
            border-image-repeat: initial;
            background: 0px 50%;
            color: rgb(26, 158, 217);
            outline: 0px;
            -webkit-box-shadow: none;
            border-radius: 0px;
        }
        .fn-clear::after
        {
            clear: both;
            content: " ";
            display: block;
            font-size: 0;
            height: 0;
            visibility: hidden;
        }
        .fn-clear
        {
        }
        .order-btn
        {
            -moz-border-bottom-colors: none;
            -moz-border-left-colors: none;
            -moz-border-right-colors: none;
            -moz-border-top-colors: none;
            background: none repeat scroll 0 0 #fe932b;
            border-color: #f3773d #f3773d -moz-use-text-color;
            border-image: none;
            border-radius: 4px;
            border-style: solid solid none;
            border-width: 1px 1px 0;
            box-shadow: 0 1px 1px #b5b3b2;
            color: #fff;
            font-size: 18px;
            height: 44px;
            line-height: 44px;
            margin: 10px 10px 20px;
            text-align: center;
        }
        .order-btn input
        {
            background: none repeat scroll 0 center rgba(0, 0, 0, 0);
            border: 0 none;
            color: #fff;
            height: 40px;
            width: 100%;
        }
        .tdleft
        {
            float: left;
            width: 25%;
        }
        .tdright
        {
            position: relative;
            overflow: hidden;
            padding: 0 15px 0 5px;
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <header class="header"  style=" background-color: #3CAFDC;">
         <h1 id="h_comname"></h1>
        <div class="left-head"> 
              <a href="javascript:history.go(-1)" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="head-return"></span></span>
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
                订单列表</a> <a class="" href="javascript:gofinane()">财务记录</a> <a class="active" href="javascript:goshopcart()">
                    购物车</a>
        </div>
        <div class="layout-full-box pr mt10" id="div_comname">
        </div>
        <div id="backs-list-container" class="block" style="margin-top: 20px;">
            <li class="block block-order animated">
                <div class="block block-list block-border-top-none block-border-bottom-none" id="tblist">
                </div>
                <div class="bottom" style="display: none;">
                    小计(不含运费)：<span class="c-orange"></span>
                </div>
            </li>
        </div>
       
        <!--实物产品收货人地址信息-->
        <div class="w-item" id="tbody_address">
            <dl class="in-item fn-clear">
                <table style="width: 100%;">
                    <tr>
                        <td valign="top" colspan="2">
                            <label>
                                收货人信息</label>
                        
                             
                              <span style=" float:right;"><a class="btn_code" href="javascript:void(0)" id="a_ChooseAddress">使用常用地址</a></span>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="tdleft">
                            <label>
                                姓名</label>
                        </td>
                        <td class="tdright">
                            <input name="txtaddress" type="text" class="dataNum dataIcon" id="u_name" value=""
                                placeholder="请输入姓名" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="tdleft">
                            <label>
                                手机</label>
                        </td>
                        <td class="tdright">
                            <input name="txtaddress" type="text" class="dataNum dataIcon" id="u_phone" value=""
                                placeholder="请输入手机" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdleft" valign="top">
                            <label>
                                运送方式</label>
                        </td>
                        <td class="tdright">
                            <label>
                                <input name="deliverytype" type="radio" value="2" checked>
                                快递(需运费)</label>
                            <label>
                                <input name="deliverytype" type="radio" value="4">
                                自提(免运费)</label>
                        </td>
                    </tr>
                    <tr id="delivery_tr1">
                        <td valign="top" class="tdleft">
                            <label>
                                收货地址</label>
                        </td>
                        <td class="tdright">
                            <select name="com_province" id="com_province" class="mi-input" style="margin-bottom: 10px;">
                                <option value="省份" selected="selected">省份</option>
                            </select>
                            <br />
                            <select name="com_city" id="com_city" class="mi-input" style="">
                                <option value="城市" selected="selected">城市</option>
                            </select>
                        </td>
                    </tr>
                    <tr id="delivery_tr2">
                        <td valign="top" class="tdleft">
                            <label>
                                详细地址</label>
                        </td>
                        <td class="tdright">
                            <input name="txtaddress" type="text" class="dataNum dataIcon" id="txtaddress" value=""
                                placeholder="请输入详细地址" />
                        </td>
                    </tr>
                    <tr id="delivery_tr3">
                        <td valign="top" class="tdleft">
                            <label>
                                邮&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;编</label>
                        </td>
                        <td class="tdright">
                            <input name="txtcode" type="tel" class="dataNum dataIcon" id="txtcode" value="" placeholder="请输入邮编(选填)" />
                        </td>
                    </tr>
                    <tr id="delivery_tr4">
                        <td valign="top" colspan="2">
                            <strong>您需为订单支付<b id="bfee" style="font-size: 12px; color: #ff0000;">0</b>元运费</strong>
                        </td>
                    </tr>
                </table>
            </dl>
        </div>
        <div class="w-item">
            <dl class="in-item fn-clear">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            商品金额
                        </td>
                        <td style="text-align: right;">
                            <b id="b1" style="color: #ff0000;">0</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            运费
                        </td>
                        <td style="text-align: right;">
                            <b id="b2" style="color: #ff0000;">0</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            实付款
                        </td>
                        <td style="text-align: right;">
                            <b id="b3" style="color: #ff0000;">0</b>
                        </td>
                    </tr>
                </table>
            </dl>
        </div>
        <div class="order-btn fn-clear" id="suborder">
            <div class="">
                <input type="button" class="btn" id="submitBtn1" value="在线购买">
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
    {{if Agent_price !=0}}
         <div  id="prolist${Id}" class="block-item name-card name-card-3col clearfix">
            <input id="pro${Id}" name="proid" value="${Id}" type="hidden" >
            <a href="javascript:openlink('${Id}');" class="thumb">
                <img src="${ProImg}" width="50px" height="50px">
            </a>
            <div class="detail">
                <a href="javascript:openlink('${Id}');">
                    <h3 style="width:60%;">
                        ${Pro_name} {{if ishasdeliveryfee==0}}包邮{{/if}}</h3>
                </a>
                <p class="sku-detail ellipsis js-toggle-more">
                    <span class="c-gray">产品有效期:${ChangeDateFormat(Pro_end)} </span>
                </p>
            </div>
            <div class="right-col">
                <div class="price">
                    ¥<span> ${Agent_price}</span></div>
                <div class="num">
                
                    <div class="wrap-input"> 
                            <span id="sum${Id}" name="sum" style="padding-left:10px;">x${U_num} </span> 
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
    <input id="hid_proid" type="hidden" value="<%=proid %>" />
    <input id="hid_num" type="hidden" value="<%=num %>" />
     <!--常用地址id; ，目前以下参数都是实物产品用到，其他产品不用关心-->
    <input id="hid_addrid" type="hidden" value="<%=addrid %>" />
    <!--小计(不含运费)-->
    <input id="hid_subtotal" type="hidden" value="" />
    <script type="text/javascript">
        var province = document.getElementById('com_province');
        var city = document.getElementById('com_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</asp:Content>
