<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuitOrder.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.Order.QuitOrder"
    MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $.post("/JsonFactory/OrderHandler.ashx?oper=getorderdetail", { orderid: $("#hid_orderid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    $("#lbl_prodetail").text("产品详情:" + data.msg[0].Pro_name + "(产品编号:" + data.msg[0].Pro_id + ")");
                    if (data.msg[0].Pro_servertype == 1) {//电子票
                        $("#lbl_orderdetail").text("订单详情:总计" + data.msg[0].Totalnum + "份 ¥" + data.msg[0].order_TotalFee + "，可用" + data.msg[0].CanUsenum + "份，已消费" + data.msg[0].ConsumeNum + "份(含退票" + data.msg[0].Cancelnum + "份)");
                    }
                    else if (data.msg[0].Pro_servertype == 11) {//实物
                        $("#lbl_orderdetail").text("订单详情:总计" + data.msg[0].Totalnum + "份 ¥" + data.msg[0].order_TotalFee + "(含运费¥" + data.msg[0].DeliveryFee + ")");
                    }
                    else {//其他产品
                        $("#lbl_orderdetail").text("订单详情:总计" + data.msg[0].Totalnum + "份 ¥" + data.msg[0].order_TotalFee);
                    }

                    //退票则退掉全部
                    var canusestr = '';
                    for (var i = 1; i <= data.msg[0].CanUsenum; i++) {
                        if (i == data.msg[0].CanUsenum) {
                            canusestr += '<option value="' + i + '"  selected="true">' + i + '份</option>';
                        }
                        //                        else{
                        //                         canusestr += '<option value="' + i + '">' + i + '份</option>';
                        //                        }
                    }
                    if (canusestr == '') {
                        canusestr = '<option value="0">0份</option>';
                    }
                    $("#quit_num").append(canusestr);

                    $("#quit_fee").val(data.msg[0].order_TotalCanQuitFee);

                    $("#hid_singleprice").val(data.msg[0].SinglePrice);
                    $("#hid_jifen").val(data.msg[0].jifen);
                    $("#hid_yufu").val(data.msg[0].yufu);
                    //                    $("#hid_TotalCanQuitFee").val(data.msg[0].TotalCanQuitFee); //可退款总金额
                    $("#hid_order_TotalCanQuitFee").val(data.msg[0].order_TotalCanQuitFee); //订单可退款总金额

                    $("#hid_proid").val(data.msg[0].Pro_id);
                    $("#hid_totalfee").val(data.msg[0].TotalFee);
                }
            })
            //            $("#quit_num").change(function () {
            //                var quit_num = $("#quit_num").trimVal();
            //                var singleprice = $("#hid_singleprice").val();

            //                var tprice = quit_num * singleprice;
            //                var pay_tquitprice = $("#hid_order_TotalCanQuitFee").trimVal(); //订单可退款总金额
            //                if (tprice > pay_tquitprice) {
            //                    tprice = pay_tquitprice;
            //                }
            //                $("#quit_fee").val(tprice);
            //            })

            $("#button").click(function () {
                var order_TotalCanQuitFee = $("#hid_order_TotalCanQuitFee").trimVal(); //订单可退款总金额
                //                var TotalCanQuitFee = $("#hid_TotalCanQuitFee").trimVal(); //可退款总金额
                //                if (order_TotalCanQuitFee * 100 > TotalCanQuitFee * 100) {
                //                    order_TotalCanQuitFee = TotalCanQuitFee;
                //                }

                if ($("#quit_num").trimVal() == 0 || $("#quit_num").trimVal() == "") {
                    alert("退票数量不可为0");
                    return;
                }
                var realquitfee = $("#quit_fee").val(); //实际退款金额
                if (parseInt(realquitfee * 100) > parseInt(order_TotalCanQuitFee * 100)) {
                    alert("可退款总金额最大为" + order_TotalCanQuitFee);
                    return;
                }


                var paytotalfee = $("#hid_totalfee").val();
                if (parseInt(paytotalfee) > 0) {
                    if (parseInt(realquitfee * 100) > parseInt(paytotalfee * 100)) {
                        alert("可退款总金额最大为" + order_TotalCanQuitFee);
                        return;
                    } 
                }


                var quit_Reason = $("#quit_Reason").val();
                if (quit_Reason == -1) {
                    alert("请选择退款原因");
                    return;
                }

                $("#button").val("提交中..");
                $("#button").attr("disabled", "disabled");

                if (confirm("退票后钱款将直接退给客户，是否确认退款？")) {
                    $.post("/JsonFactory/OrderHandler.ashx?oper=quitorderfee", { total_fee: $("#hid_totalfee").trimVal(), orderid: $("#hid_orderid").trimVal(), quit_num: $("#quit_num").trimVal(), quit_fee: $("#quit_fee").trimVal(), quit_Reason: $("#quit_Reason").trimVal(), quit_info: $("#quit_info").trimVal(), comid: $("#hid_comid").trimVal(), userid: $("#hid_userid").trimVal(), proid: $("#hid_proid").trimVal() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            alert(data.msg);
                            location.href = "OrderList.aspx";
                            return;
                        }
                        if (data.type == 100) {
                            alert(data.msg);
                            location.href = "OrderList.aspx";
                            return;
                        }
                    })
                }
            })
        })
         
    </script>
    <style type="text/css">
        .button
        {
            width: 120px;
            line-height: 33px;
            text-align: center;
            font-weight: bold;
            color: #fff;
            text-shadow: 1px 1px 1px #333;
            border-radius: 5px;
            margin: 0 20px 20px 0;
            position: relative;
            overflow: hidden;
        }
        .button.blue
        {
            border: 1px solid #1e7db9;
            box-shadow: 0 1px 2px #8fcaee inset,0 -1px 0 #497897 inset,0 -2px 3px #8fcaee inset;
            background: -webkit-linear-gradient(top,#42a4e0,#2e88c0);
            background: -moz-linear-gradient(top,#42a4e0,#2e88c0);
            background: linear-gradient(top,#42a4e0,#2e88c0);
        }
        .blue:hover
        {
            background: -webkit-linear-gradient(top,#70bfef,#4097ce);
            background: -moz-linear-gradient(top,#70bfef,#4097ce);
            background: linear-gradient(top,#70bfef,#4097ce);
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="secondary-tabs" class="navsetting ">
        <ul>
            <li class="on"><a href="javascript:void(0)" onfocus="this.blur()" target=""><span>订单退票</span></a>
            </li>
        </ul>
    </div>
    <div id="setting-home" class="vis-zone">
        <div class="inner">
            <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 10px;
                position: relative; z-index: 10;">
                <h2 class="p-title-area">
                    申请退款</h2>
                <div class="mi-form-item">
                    <label class="mi-label" id="lbl_prodetail">
                        产品详情:</label>
                </div>
                <div class="mi-form-item">
                    <label class="mi-label" id="lbl_orderdetail">
                        订单详情:</label>
                </div>
                <div class="mi-form-item">
                    <label class="mi-label">
                        *本次退款张数</label>
                    <select id="quit_num">
                    </select>
                </div>
                <div class="mi-form-item">
                    <label class="mi-label">
                        *实际退款金额</label>
                    <input type="text" id="quit_fee" value="" class="mi-input" />
                </div>
                <div class="mi-form-item" style="display: none;">
                    <label class="mi-label">
                        退款原因:</label>
                    <select id="quit_Reason">
                        <option value="-1">请选择退款原因</option>
                        <option value="行程变化">行程变化</option>
                        <option value="未收到入园凭证/短信">未收到入园凭证/短信</option>
                        <option value="产品价格变更">产品价格变更</option>
                        <option value="产品信息与购买时不一致">产品信息与购买时不一致</option>
                        <option value="其他" selected="selected">其它</option>
                    </select>
                </div>
                <div class="mi-form-item">
                    <label class="mi-label">
                        退款说明</label>
                    <input type="text" id="quit_info" value="" class="mi-input" />
                </div>
                <div class="mi-form-explain">
                </div>
            </div>
            <table border="0" width="780" class="grid">
                <tr>
                    <td height="80" colspan="2" align="center">
                        <input type="button" name="button" id="button" value="  确认提交  " class="button blue" />
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </div>
    <input type="hidden" id="hid_orderid" value="<%=orderid  %>" />
    <input type="hidden" id="hid_singleprice" value="" />
    <input type="hidden" id="hid_jifen" value="" />
    <input type="hidden" id="hid_yufu" value="" />
    <%--  <input type="hidden" id="hid_TotalCanQuitFee" value="" />--%>
    <input type="hidden" id="hid_order_TotalCanQuitFee" value="" />
    <input type="hidden" id="hid_proid" value="" />
    <input type="hidden" id="hid_totalfee" value="" />
</asp:Content>
