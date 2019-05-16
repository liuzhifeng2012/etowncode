<%@ Page Title="" Language="C#" MasterPageFile="~/V/Member.Master" AutoEventWireup="true" CodeBehind="Recharge.aspx.cs" Inherits="ETS2.WebApp.V.Recharge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/Recharge/Recss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">        //delete minus_disable
        $(function () {
            $("#500d").bind("click", function () {
                var a = $("#txtEntity500").val();
                var price = $(".c_price_big").html();
                if (parseInt(a) - 1 == 0) {
                    $("#500d").removeClass("create plus");
                    $("#500d").addClass("delete minus_disable");
                }


                if (parseInt(price) - 500 >= 0 && parseInt(a) > 0) {
                    $("#txtEntity500").val(parseInt(a) - 1);

                    $("#c_price_big").html(parseInt(price) - 500);
                }

            });

            $("#500a").bind("click", function () {
                var a = $("#txtEntity500").val();

                if (parseInt(a) == 0) {
                    $("#500d").removeClass("delete minus_disable");
                    $("#500d").addClass("delete minus");
                }
                $("#txtEntity500").val(parseInt(a) + 1);

                var price = $(".c_price_big").html();
                $("#c_price_big").html(parseInt(price) + 500);
            });

            $("#1000d").bind("click", function () {
                var a = $("#txtEntity1000").val();
                var price = $(".c_price_big").html();
                if (parseInt(a) - 1 == 0) {
                    $("#1000d").removeClass("create plus");
                    $("#1000d").addClass("delete minus_disable");
                }


                if (parseInt(price) - 1000 >= 0 && parseInt(a) > 0) {
                    $("#txtEntity1000").val(parseInt(a) - 1);

                    $("#c_price_big").html(parseInt(price) - 1000);
                }
            });

            $("#1000a").bind("click", function () {
                var a = $("#txtEntity1000").val();

                if (parseInt(a) == 0) {
                    $("#1000d").removeClass("delete minus_disable");
                    $("#1000d").addClass("delete minus");
                }
                $("#txtEntity1000").val(parseInt(a) + 1);

                var price = $(".c_price_big").html();
                $("#c_price_big").html(parseInt(price) + 1000);
            });

            $("#2000d").bind("click", function () {
                var a = $("#txtEntity2000").val();
                var price = $(".c_price_big").html();
                if (parseInt(a) - 1 == 0) {
                    $("#2000d").removeClass("create plus");
                    $("#2000d").addClass("delete minus_disable");
                }


                if (parseInt(price) - 2000 >= 0 && parseInt(a) > 0) {
                    $("#txtEntity2000").val(parseInt(a) - 1);

                    $("#c_price_big").html(parseInt(price) - 2000);
                }
            });

            $("#2000a").bind("click", function () {
                var a = $("#txtEntity2000").val();

                if (parseInt(a) == 0) {
                    $("#2000d").removeClass("delete minus_disable");
                    $("#2000d").addClass("delete minus");
                }
                $("#txtEntity2000").val(parseInt(a) + 1);

                var price = $(".c_price_big").html();
                $("#c_price_big").html(parseInt(price) + 2000);
            });

            $("#5000d").bind("click", function () {
                var a = $("#txtEntity5000").val();
                var price = $(".c_price_big").html();
                if (parseInt(a) - 1 == 0) {
                    $("#5000d").removeClass("create plus");
                    $("#5000d").addClass("delete minus_disable");
                }


                if (parseInt(price) - 5000 >= 0 && parseInt(a) > 0) {
                    $("#txtEntity5000").val(parseInt(a) - 1);

                    $("#c_price_big").html(parseInt(price) - 5000);
                }
            });

            $("#5000a").bind("click", function () {
                var a = $("#txtEntity5000").val();

                if (parseInt(a) == 0) {
                    $("#5000d").removeClass("delete minus_disable");
                    $("#5000d").addClass("delete minus");
                }
                $("#txtEntity5000").val(parseInt(a) + 1);

                var price = $(".c_price_big").html();
                $("#c_price_big").html(parseInt(price) + 5000);
            });

            $("#c_inputd").bind("click", function () {
                var a = $("#EntitySelfFirstQty").val();
                var price = $(".c_price_big").html();
                var c_input = $("#c_input").val();

                if (c_input == "") {
                    c_input = "0";
                }
                if (parseInt(a) - 1 == 0) {
                    $("#c_inputd").removeClass("create plus");
                    $("#c_inputd").addClass("delete minus_disable");
                }


                if (parseInt(price) - parseInt(c_input) >= 0 && parseInt(a) > 0) {
                    $("#EntitySelfFirstQty").val(parseInt(a) - 1);

                    $("#c_price_big").html(parseInt(price) - parseInt(c_input));
                }
            });

            $("#c_inputa").bind("click", function () {
                var a = $("#EntitySelfFirstQty").val();
                var c_input = $("#c_input").val();

                if (c_input == "") {
                    $("#c_inputerr").show();
                    return;
                }
                else {
                    $("#c_inputerr").hide();
                }

                if (parseInt(a) == 0) {
                    $("#c_inputd").removeClass("delete minus_disable");
                    $("#c_inputd").addClass("delete minus");
                }
                $("#EntitySelfFirstQty").val(parseInt(a) + 1);

                var price = $(".c_price_big").html();
                $("#c_price_big").html(parseInt(price) + parseInt(c_input));
            });

            $("#SecondPriced").bind("click", function () {
                var a = $("#EntitySelfSecondQty").val();
                var price = $(".c_price_big").html();
                var c_input = $("#EntitySelfSecondPrice").val();

                if (c_input == "") {
                    c_input = "0";
                }
                if (parseInt(a) - 1 == 0) {
                    $("#SecondPriced").removeClass("create plus");
                    $("#SecondPriced").addClass("delete minus_disable");
                }


                if (parseInt(price) - parseInt(c_input) >= 0 && parseInt(a) > 0) {
                    $("#EntitySelfSecondQty").val(parseInt(a) - 1);

                    $("#c_price_big").html(parseInt(price) - parseInt(c_input));
                }
            });

            $("#SecondPricea").bind("click", function () {
                var a = $("#EntitySelfSecondQty").val();
                var c_input = $("#EntitySelfSecondPrice").val();

                if (c_input == "") {
                    $("#Seconderr").show();
                    return;
                }
                else {
                    $("#Seconderr").hide();
                }

                if (parseInt(a) == 0) {
                    $("#SecondPriced").removeClass("delete minus_disable");
                    $("#SecondPriced").addClass("delete minus");
                }
                $("#EntitySelfSecondQty").val(parseInt(a) + 1);

                var price = $(".c_price_big").html();
                $("#c_price_big").html(parseInt(price) + parseInt(c_input));
            });

            $(".add_item").click(function () {
                $("#add_price").show();
                $("#ico_add").hide();
            })
            $("#del_price").click(function () {
                $("#add_price").hide();
                $("#ico_add").show();
            })
            $("#BuEntity").click(function () {
                var price = $(".c_price_big").html();
                if (parseInt(price) == 0) {
                    $(".jmp_info").show();
                    return;
                }
                else {
                    $(".jmp_info").hide();
                }
            })

        }); 
        
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   <div class="base_main">
            <h2 class="h2_tabtit">
                <span id="entitycard" class="current">会员充值</span>
            </h2>
            <div id="diventitycard" class="tabcn entity_card">
                <p class="h2_tit">
                    EMS邮寄配送</p>
                <div class="card_cn">
                    <div class="card_category">
                        <label class="base_label"><input type="radio" name="EntityTicketType" checked="checked" value="3">礼品卡</label><label class="base_label" style="display:none;"><input type="radio" name="EntityTicketType" value="2">礼品卡</label>
                        
                    </div>
                    <ul class="card_list">
                        <li>
                            <div class="list_lt">
                                <span class="c_price">500</span>
                                <input type="text" value="500" style="display:none;">
                            </div>
                            <div class="list_rt choos_num">
                                <a id="500d" class="delete minus_disable" href="javascript:void(0);">-</a>
                                <input type="text" id="txtEntity500" name="Entity500" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off">
                                <a id="500a" class="create plus" href="javascript:void(0);">+</a>
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <span class="c_price">1000</span>
                                <input type="text" value="1000" style="display:none;">
                            </div>
                            <div class="list_rt choos_num" price="1000">
                                <a id="1000d" class="delete minus_disable" href="javascript:void(0);">-</a>
                                <input type="text" id="txtEntity1000" name="Entity1000" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off">
                                <a id="1000a" class="create plus" href="javascript:void(0);">+</a>
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <span class="c_price">2000</span>
                                <input type="text" value="2000" style="display:none;">
                            </div>
                            <div class="list_rt choos_num" price="2000">
                                <a id="2000d" class="delete minus_disable" href="javascript:void(0);">-</a>
                                <input type="text" id="txtEntity2000" name="Entity2000" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off">
                                <a id="2000a" class="create plus" href="javascript:void(0);">+</a>
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <span class="c_price">5000</span>
                                <input type="text" value="5000" style="display:none;">
                            </div>
                            <div class="list_rt choos_num" price="5000">
                                <a id="5000d" class="delete minus_disable" href="javascript:void(0);">-</a>
                                <input type="text" id="txtEntity5000" name="Entity5000" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off">
                                <a id="5000a" class="create plus" href="javascript:void(0);">+</a>
                            </div>
                        </li>
                        <li class="custom_price">
                            <div class="list_lt">
                                <span class="c_price">&nbsp;</span>
                                <input name="EntitySelfFirstPrice" type="text" id="c_input" class="c_input input_s" maxlength="4" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" placeholder="请输入金额" autocomplete="off">
                                <span class="fc01" id="c_inputerr" style="display:none;">(1~5000以内的整数)</span>
                            </div>
                            <div class="list_rt choos_num" price="0">
                                <a id="c_inputd" class="delete minus_disable" href="javascript:void(0);">-</a>
                                <input id="EntitySelfFirstQty" name="EntitySelfFirstQty" type="text" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off">
                                <a id="c_inputa" class="create plus" href="javascript:void(0);">-</a>
                            </div>
                        </li>
                        <li id="add_price" class="custom_price" style="display: none;">
                            <div class="list_lt">
                                <span class="c_price">&nbsp;</span>
                                <input id="EntitySelfSecondPrice" name="EntitySelfSecondPrice" type="text" class="c_input input_s" maxlength="4" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" placeholder="请输入金额" autocomplete="off">
                                <span class="fc01" id="Seconderr" style="display:none;">(1~5000以内的整数)</span>
                            </div>
                            <div class="list_rt choos_num" price="0">
                                <a id="SecondPriced" class="delete minus_disable" href="javascript:void(0);">-</a>
                                <input id="EntitySelfSecondQty" name="EntitySelfSecondQty" type="text" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off">
                                <a id="SecondPricea" class="create plus" href="javascript:void(0);">-</a>
                            </div>
                            <div class="delete_box" style="display: block;">
                                <a id="del_price" href="javascript:void(0);">删除</a></div>
                        </li>
                        <li class="add_item"><a id="ico_add" href="javascript:void(0);" class="ico_add">添加</a></li>
                        <li>
                            <div class="list_lt price_box">
                                <strong class="total">合计：</strong><span class="c_price_big" id="c_price_big">0</span>
                                <span class="jmp_info" style="display: none;">
                                    <b></b><i></i><div class="jmp_cn">请确认购买金额及数量</div>
                                </span>
                            </div>
                            <div class="list_rt">
                                <input id="BuEntity" class="btn01" type="button" value="立即购买" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="diveleccard" class="tabcn e_card" style="display:none;">
                <p class="h2_tit">
                    短信获取</p>
                <div class="card_cn">
                    <div class="card_category">
                        <label class="base_label"><input type="radio" name="ElecTicketType" checked="checked" class="rwx" value="3">礼品卡(任我行)</label><label class="base_label"><input type="radio" name="ElecTicketType" class="rwy" value="2">礼品卡(任我游)</label><a class="unknown" href="javascript:void(0);">[?]</a>
                        <div class="card_jmp" style="display:none;">什么是任我游？<a href="http://help.ctrip.com/QuestionDetail.aspx?questionId=236" target="_blank">详细介绍</a></div>
                    </div>
                    <ul class="card_list">
                        <li>
                            <div class="list_lt">
                                <span class="c_price">500</span>
                                <input type="text" value="500" style="display:none;">
                            </div>
                            <div class="list_rt choos_num">
                                <a class="delete minus_disable" href="javascript:void(0);">-</a><input type="text" id="txtElec500" name="Elec500" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off"><a class="create plus" href="javascript:void(0);">+</a>
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <span class="c_price">1000</span>
                                <input type="text" value="1000" style="display:none;">
                            </div>
                            <div class="list_rt choos_num">
                                <a class="delete minus_disable" href="javascript:void(0);">-</a><input type="text" id="txtElec1000" name="Elec1000" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off"><a class="create plus" href="javascript:void(0);">+</a>
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <span class="c_price">2000</span>
                                <input type="text" value="2000" style="display:none;">
                            </div>
                            <div class="list_rt choos_num">
                                <a class="delete minus_disable" href="javascript:void(0);">-</a><input type="text" id="txtElec2000" name="Elec2000" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off"><a class="create plus" href="javascript:void(0);">+</a>
                            </div>
                        </li>
                        <li>
                            <div class="list_lt">
                                <span class="c_price">5000</span>
                                <input type="text" value="5000" style="display:none;">
                            </div>
                            <div class="list_rt choos_num">
                                <a class="delete minus_disable" href="javascript:void(0);">-</a><input type="text" id="txtElec5000" name="Elec5000" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off"><a class="create plus" href="javascript:void(0);">+</a>
                            </div>
                        </li>
                        <li class="custom_price">
                            <div class="list_lt">
                                <span class="c_price">&nbsp;</span><input name="ElecSelfFirstPrice" type="text" class="c_input input_s" maxlength="4" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" placeholder="请输入金额" autocomplete="off">
                                <span class="fc01" style="display:none;">(10~5000以内的整数)</span>
                                <div class="jmp_info" style="display:none;"><b></b><i></i><div class="jmp_cn">请输入10~5000以内的整数</div></div>
                            </div>
                            <div class="list_rt choos_num" price="0">
                                <a class="delete minus_disable" href="javascript:void(0);">-</a>
                                <input name="ElecSelfFirstQty" maxlength="3" type="text" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off">
                                <a class="create plus" href="javascript:void(0);">-</a>
                            </div>
                            <div class="delete_box remove" style="display: block;">
                                <a href="javascript:void(0);" style="visibility:hidden;">删除</a></div>
                        </li>
                        <li class="custom_price" style="display: none;">
                            <div class="list_lt">
                                <span class="c_price">&nbsp;</span><input name="ElecSelfSecondPrice" type="text" class="c_input input_s" maxlength="4" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" placeholder="请输入金额" autocomplete="off">
                                <span class="fc01" style="display:none;">(10~5000以内的整数)</span>
                                <div class="jmp_info" style="display:none;"><b></b><i></i><div class="jmp_cn">请输入10~5000以内的整数</div></div>
                            </div>
                            <div class="list_rt choos_num" price="0">
                                <a class="delete minus_disable" href="javascript:void(0);">-</a>
                                <input name="ElecSelfSecondQty" type="text" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off">
                                <a class="create plus" href="javascript:void(0);">-</a>
                            </div>
                            <div class="delete_box" style="display: block;">
                                <a href="javascript:void(0);" style="visibility: hidden;">删除</a></div>
                        </li>
                        <li class="custom_price" style="display: none;">
                            <div class="list_lt">
                                <span class="c_price">&nbsp;</span><input name="ElecSelfThirdPrice" type="text" class="c_input input_s" maxlength="4" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" placeholder="请输入金额" autocomplete="off">
                                <span class="fc01" style="display:none;">(10~5000以内的整数)</span>
                                <div class="jmp_info" style="display:none;"><b></b><i></i><div class="jmp_cn">请输入10~5000以内的整数</div></div>
                            </div>
                            <div class="list_rt choos_num" price="0">
                                <a class="delete minus_disable" href="javascript:void(0);">-</a>
                                <input name="ElecSelfThirdQty" type="text" maxlength="3" onkeyup="value=value.replace(/[^\d]/g, '')" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g, ''))" onblur="value=value.replace(/[^\d$]/g, '')" value="0" autocomplete="off">
                                <a class="create plus" href="javascript:void(0);">-</a>
                            </div>
                            <div class="delete_box" style="display: block;">
                                <a href="javascript:void(0);" style="visibility: hidden;">删除</a></div>
                        </li>
                        <li class="add_item add"><a href="javascript:void(0);" class="ico_add">添加</a></li>
                        <li>
                            <div class="list_lt price_box">
                                <strong class="total">合计：</strong><span class="c_price_big">0</span>
                                <span class="jmp_info" style="display: none;">
                                    <b></b><i></i><div class="jmp_cn">请确认购买金额及数量</div>
                                </span>
                            </div>
                            <div class="list_rt">
                                <a id="ctl00_MainContentPlaceHolder_lkb_BuyElecCard" class="btn01" href='javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("ctl00$MainContentPlaceHolder$lkb_BuyElecCard", "", false, "", "Online/ypbooking/InputInfo.aspx", false, true))'>立即购买</a>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>

            
            <div class="tabs tab1">
                <ul class="card_tab">
                    <li><a class="current" href="javascript:void(0);">购买流程</a></li>
                    <li><a href="javascript:void(0);">使用帮助</a></li>
                    <li><a href="javascript:void(0);">规则说明</a></li>
                    <li><a href="javascript:void(0);">常见说明</a></li>
                    <li><a href="javascript:void(0);">如何送礼</a></li>
                </ul>
                <div class="tabs-content">
                    <!-- 购买流程Start -->
                    <div class="card_tab_box option">
                        <h3 class="h3_tit">
                            电子礼品卡</h3>
                        <div class="flow_box">
                            <ul class="flow_list">
                                <li><span class="no">1</span>
                                    <div class="flow_txt">
                                        <span>注册登录</span><i></i></div>
                                </li>
                                <li><span class="no">2</span>
                                    <div class="flow_txt">
                                        <span>订单信息填写</span><i></i></div>
                                </li>
                                <li><span class="no">3</span>
                                    <div class="flow_txt">
                                        <span>订单支付</span><i></i></div>
                                </li>
                                <li><span class="no">4</span>
                                    <div class="flow_txt">
                                        <span>获取卡号和密码</span><i></i></div>
                                </li>
                                <li><span class="no">5</span>
                                    <div class="flow_txt">
                                        <span>购买完成</span><i></i></div>
                                </li>
                            </ul>
                        </div>
                        <div class="split_line">
                            &nbsp;</div>
                        <h3 class="h3_tit">
                            实体礼品卡</h3>
                        <div class="flow_box">
                            <ul class="flow_list">
                                <li><span class="no">1</span>
                                    <div class="flow_txt">
                                        <span>注册登录</span><i></i></div>
                                </li>
                                <li><span class="no">2</span>
                                    <div class="flow_txt">
                                        <span>订单信息填写</span><i></i></div>
                                </li>
                                <li><span class="no">3</span>
                                    <div class="flow_txt">
                                        <span>订单支付</span><i></i></div>
                                </li>
                                <li><span class="no">4</span>
                                    <div class="flow_txt">
                                        <span>确认收卡</span><i></i></div>
                                </li>
                                <li><span class="no">5</span>
                                    <div class="flow_txt">
                                        <span>激活礼品卡</span><i></i></div>
                                </li>
                                <li><span class="no">6</span>
                                    <div class="flow_txt">
                                        <span>购买完成</span><i></i></div>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <!-- 购买流程End -->
                    <!-- 使用帮助Start -->
                    <div class="card_tab_box option" style="display: none;">
                        <div class="ly_step">
                            <a class="current" href="javascript:void(0);">第一步</a><span></span><a href="javascript:void(0);">第二步</a><span></span>
                            <a href="javascript:void(0);">第三步</a><span></span><a href="javascript:void(0);">第四步</a>
                        </div>
                        <div class="ly_pic">
                            <img src="http://pic.c-ctrip.com/gift_card/card_direction_05.jpg" alt="">
                        </div>
                        <div class="ly_pic" style="display: none;">
                            <img src="http://pic.c-ctrip.com/gift_card/card_direction_06.jpg" alt="">
                        </div>
                        <div class="ly_pic" style="display: none;">
                            <img src="http://pic.c-ctrip.com/gift_card/card_direction_07.jpg" alt="">
                        </div>
                        <div class="ly_pic" style="display: none;">
                            <img src="http://pic.c-ctrip.com/gift_card/card_direction_08.jpg" alt="">
                        </div>
                    </div>
                    <!-- 使用帮助End -->
                    <!-- 规则说明Start -->
                    <div class="card_tab_box option" style="display: none;">
                        <p class="p_text">
                            1. 携程发行的礼品卡内预付价值一元等值于一元人民币。 
                            <br>
                            2. 通过lipin.ctrip.com在线购买携程礼品卡后将获得携程积分奖励，规则为：基本积分=购买金额×25%；获得积分=基本积分×所属用户级别的电话预订积分系数。公司或团体线下购买礼品卡是否累加携程积分，视合作而定。 
                            <br>
                            3. 携程礼品卡（任我行）可预订预付费类酒店、惠选酒店、机票、旅游度假产品、火车票产品、团购产品（注：自由机+酒产品、部分门票类产品、代驾租车产品及银行专享类旅游度假产品等暂不支持礼品卡支付）。 
                            <br>
                            4. 携程礼品卡（任我游）可预订预付费类酒店、惠选酒店、旅游度假产品、团购产品（注：自由机+酒产品、门票类产品、代驾租车产品及银行专享类旅游度假产品等暂不支持礼品卡支付）。
                            <br>
                            5. 通过lipin.ctrip.com购买的携程礼品卡实体卡，需要在点击“确认收货”后方可激活领用；通过线下采购，采取支票、汇款等方式支付的携程礼品卡，一般在资金到帐后7个工作日方可激活领用。
                            <br>
                            6. 携程礼品卡需要领用至携程会员帐户内方可使用。 携程礼品卡在被领用至携程会员账户时需要设置交易密码，密码可以由顾客自行修改。
                            <br>
                            7. 已领用至携程会员帐户内的携程礼品卡可以分次消费，单个携程会员帐户内的携程礼品卡金额在有效期内不限额、不限人次、不限次数使用。
                            <br>
                            8. 在预订一项产品时，多张携程礼品卡可领用到同一个携程会员帐户内合并金额使用，不同携程会员帐户的金额不能合并使用。
                            <br>
                            9. 携程礼品卡一经售出，使用范围不可改变，不合并、不挂失、不能兑换现金。
                            <br>
                            10. 用于销售的携程礼品卡自生效之日起不少于三年内有效。携程礼品卡应该在有效期内使用。
                            <br>
                            11. 用于销售的携程礼品卡，超过有效期仍有未消费余额，公司将在过期日后的每个月5日零时扣取余额的1%作为当月的账户管理费。（最低10元，管理费金额向下取整，按元递增）。所有收取的费用均在礼品卡内余额中扣取，扣完为止。 
                            <br>
                            12. 携程礼品卡购买时若需要发票，请提前说明，或在具体选项前标明；在预订产品时，礼品卡支付部分不再提供发票。
                            <br>
                        </p>
                    </div>
                    <!-- 规则说明End -->
                    <!-- 常见说明Start -->
                    <div class="card_tab_box option" style="display: none;">
                        <div class="ask">
                            <span>问：</span>我在你们网上购买了携程礼品卡，你们提供发票吗？</div>
                        <div class="answer">
                            <span>答：</span> 您通过网站在线购买携程礼品卡时，如需发票，请在订单填写过程中勾选“需要发票”，仔细阅读并填写相关明细。<br>
                            购买“电子券”礼品卡：发票将以挂号信的方式邮寄。挂号信费用由携程承担。<br>
                            购买“实体卡”礼品卡：相关物料（实体卡、包装、发票等）将通过EMS的方式邮寄。单张订单金额满1000元，免邮寄资费。<br>
                            发票开具公司:上海携程国际旅行社有限公司<br>
                            发票内容:1、旅游服务费 2、机票+酒店 3、代订酒店款 4.代订住宿费。（四项任选其一）<br>
                            因礼品卡售卖时已提供相应发票，所以预订成功后，对于订单中使用了礼品卡的部分将不再开具发票。<br>
                            如礼品卡和其他支付方式混合支付的订单，使用礼品卡支付的部分不开具发票。
                        </div>
                        <div class="ask">
                            <span>问：</span>为什么要设置礼品卡的交易密码？该怎样设置？</div>
                        <div class="answer">
                            <span>答：</span> 交易密码通过携程旅行网在线设置，是保障您礼品卡、现金账户安全的一种工具，密码由六位数字组成。<br>
                            您可以在登录个人账户后，于“我的携程”－“资产账户”－“交易密码”中进行设置/修改/重置。 如果您忘记交易密码或有其他问题，请致电400-820-3300/800-820-3300转8处理。
                        </div>
                        <div class="ask">
                            <span>问：</span>我已经得到礼品卡，该怎么使用？</div>
                        <div class="answer">
                            <span>答：</span> 拥有礼品卡后，可以在电话预订或网上预订时使用礼品卡支付。<br>
                            1、电话预订——需要提前在网上设置礼品卡交易密码（如何设置礼品卡交易密码，请<a href="http://help.ctrip.com/QuestionList.aspx?parentId=1&amp;directoryId=16" target="_blank">点击这里</a>），来电预订时告诉预订员本次预订需要使用礼品卡；<br>
                            2、网上预订——在酒店预订频道查询酒店时，勾选“礼品卡”进行查询，查询所列酒店都是可以用礼品卡支付的，填写订单到支付环节，选择使用礼品卡支付即可。在预订机票、旅游度假、惠选酒店、团购产品时，当填写订单到支付环节，选择使用礼品卡支付即可。
                        </div>
                    </div>
                    <!-- 常见说明End -->
                    <!-- 如何送礼Start -->
                    <div class="card_tab_box option" style="display: none;">
                        <table class="carb_tb">
                            <tbody><tr>
                                <td>
                                    <div class="setp">
                                        1</div>
                                </td>
                                <td>
                                    选择个性化卡面
                                </td>
                                <td>
                                    <div class="arr">
                                        arr</div>
                                </td>
                                <td>
                                    <div class="setp">
                                        2</div>
                                </td>
                                <td>
                                    填写收件人邮箱
                                </td>
                                <td>
                                    <div class="arr">
                                        arr</div>
                                </td>
                                <td>
                                    <div class="setp">
                                        3</div>
                                </td>
                                <td>
                                    支付成功，发送礼<br>
                                    品卡信息给收礼人
                                </td>
                                <td>
                                    <div class="arr">
                                        arr</div>
                                </td>
                                <td>
                                    <div class="setp">
                                        4</div>
                                </td>
                                <td>
                                    收礼人在礼品卡<br>
                                    领用后可以使用
                                </td>
                            </tr>
                        </tbody></table>
                    </div>
                    <!-- 如何送礼End -->
                </div>
            </div>
        </div>
</asp:Content>
