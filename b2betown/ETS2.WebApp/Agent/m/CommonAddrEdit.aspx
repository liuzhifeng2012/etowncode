<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommonAddrEdit.aspx.cs"
    Inherits="ETS2.WebApp.Agent.m.CommonAddrEdit" MasterPageFile="/Agent/m/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <title>常用地址编辑</title>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <!-- CSS -->
    <link rel="stylesheet" href="/h5/order/css/css4.css" />
    <link rel="stylesheet" href="/h5/order/css/css1.css" />
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            var addrid = $("#hid_addrid").trimVal();
            if (addrid != "0") {
                $.post("/JsonFactory/OrderHandler.ashx?oper=getagentaddrbyid", { addrid: addrid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        $("#in_name").val(data.msg.U_name);
                        $("#in_phone").val(data.msg.U_phone);
                        $("#in_province").val(data.msg.Province);
                        $("#in_city").append('<option value="' + data.msg.City + '" selected="selected">' + data.msg.City + '</option>');
                        $("#in_address").val(data.msg.Address);
                        $("#in_code").val(data.msg.Code);

                    }
                })
            }
            $("#submitsave").click(function () {
                var agentid = $("#hid_agentid").trimVal();
                var u_name = $("#in_name").trimVal();
                var u_phone = $("#in_phone").trimVal();
                var province = $("#in_province").trimVal();
                var city = $("#in_city").trimVal();
                var address = $("#in_address").trimVal();
                var txtcode = $("#in_code").trimVal();
                if (agentid == 0) {
                    showErr("分销id不可为空，请重新登录");
                    return;
                }
                if (u_name == "") {
                    showErr("收货人姓名不可为空");
                    return;
                }
                if (u_phone == "") {
                    showErr("联系电话不可为空");
                    return;
                } else {
                    if (!isMobel(u_phone)) {
                        showErr("请正确填写接收人手机号");
                        return;
                    }
                }
                if (province == "省份") {
                    showErr("请选择省份");
                    return;
                }
                if (city == "城市") {
                    showErr("请选择城市");
                    return;
                }
                if (address == "") {
                    showErr("详细地址不可为空");
                    return;
                }


                $.post("/JsonFactory/OrderHandler.ashx?oper=editagentaddr", { addrid: $("#hid_addrid").trimVal(), agentid: agentid, u_name: u_name, u_phone: u_phone, province: province, city: city, address: address, txtcode: txtcode }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        showErr("编辑成功");
                    }
                })
            })

            $("#submitcannel").click(function () {
                //                history.go(-1);
                //判断常用地址列表是否含有数据，不含有，直接返回提单页面；否则返回常用地址列表页面
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=addresspagelist",
                    data: { pageindex: 1, pagesize: 10, agentid: $("#hid_agentid").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            var isshopcart = $("#hid_isshopcart").trimVal();
                            //alert("h--" + $("#hid_isshopcart").trimVal());
                            if (isshopcart == 1) {
                                window.open("ShopCartSales.aspx?agentid=" + $("#hid_agentid").trimVal() + "&proid=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self");
                            } else {
                                window.open("pro_sales.aspx?id=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self");
                            }
                        }
                        if (data.type == 100) {

                            if (data.totalCount == 0) {

                                var isshopcart = $("#hid_isshopcart").trimVal();
                                //alert("a--" + $("#hid_isshopcart").trimVal());
                                if (isshopcart == 1) {
                                    window.open("ShopCartSales.aspx?agentid=" + $("#hid_agentid").trimVal() + "&proid=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self");
                                } else {
                                    window.open("pro_sales.aspx?id=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self");
                                }
                            } else {
                                //alert("b--" + $("#hid_isshopcart").trimVal());
                                window.open("CommonAddrlist.aspx?isshopcart=" + $("#hid_isshopcart").trimVal() + "&proid=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self");
                            }
                        }
                    }
                })
            })
        })
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
            if (a == '编辑成功') {
                location.href = "CommonAddrlist.aspx?isshopcart=" + $("#hid_isshopcart").trimVal() + "&proid=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div class="app-inner inner-order peerpay-gift address-fm" style="display: ; height: 100%;"
        id="sku-message-poppage">
        <div class="js-list block block-list">
        </div>
        <div class="block" style="margin-bottom: 10px;">
            <div class="block-item">
                <label class="form-row form-text-row">
                    <em class="form-text-label">收货人</em> <span class="input-wrapper">
                        <input id="in_name" name="in_name" class="form-text-input" value="" placeholder="名字"
                            type="text"></span>
                </label>
            </div>
            <div class="block-item">
                <label class="form-row form-text-row">
                    <em class="form-text-label">联系电话</em> <span class="input-wrapper">
                        <input id="in_phone" name="in_phone" class="form-text-input" value="" placeholder="手机或固话"
                            type="tel"></span>
                </label>
            </div>
            <div class="block-item No-Eticket">
                <div class="form-row form-text-row">
                    <em class="form-text-label">选择地区</em>
                    <div class="input-wrapper input-region js-area-select">
                        <span>
                            <select id="in_province" name="in_province" class="address-province" data-next-type="城市"
                                data-next="city">
                                <option data-code="" value="省份">省份</option>
                            </select>
                        </span><span>
                            <select id="in_city" name="in_city" class="address-city" data-next-type="区县" data-next="county">
                                <option data-code="0" value="城市">城市</option>
                            </select>
                        </span>
                    </div>
                </div>
            </div>
            <div class="block-item No-Eticket">
                <label class="form-row form-text-row">
                    <em class="form-text-label">详细地址</em> <span class="input-wrapper">
                        <input id="in_address" name="in_address" class="form-text-input" value="" placeholder="街道门牌信息"
                            type="text"></span>
                </label>
            </div>
            <div class="block-item No-Eticket">
                <label class="form-row form-text-row">
                    <em class="form-text-label">邮政编码</em> <span class="input-wrapper">
                        <input id="in_code" maxlength="6" name="in_code" class="form-text-input" value=""
                            placeholder="邮政编码" type="tel"></span>
                </label>
            </div>
        </div>
        <div>
            <div class="action-container">
                <a id="submitsave" class="js-address-save btn btn-block btn-blue">保存</a> <a id="submitcannel"
                    class="js-address-cancel btn btn-block" >取消</a>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var province = document.getElementById('in_province');
        var city = document.getElementById('in_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
    <input type="hidden" id="hid_addrid" value="<%=addrid %>" />
    <input type="hidden" id="hid_proid" value="<%=proid %>" />
     <input type="hidden" id="hid_unum" value="<%=unum %>" />
      <input type="hidden" id="hid_isshopcart" value="<%=isshopcart %>"/>
    <div style="height: 565px; display: none;" id="bgDiv">
    </div>
    <div id="showMsg" style="top: 352px; display: none;">
        <div class="msg-title">
            温馨提示</div>
        <div class="msg-content">
            请填写预订人姓名！</div>
        <div class="msg-btn">
            <a href="javascript:;" onclick="hideErr()">知道了</a></div>
    </div>
</asp:Content>
