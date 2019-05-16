<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="ticketlist.aspx.cs" Inherits="ETS2.WebApp.UI.ticketlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            var pageSize = 15; //每页显示条数
            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();

            //获取订单列表
            SearchList(1);
            //装载产品列表
            function SearchList(pageindex) {
                var order_state = $("#order_state").trimVal();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                var key = $("#key").val();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getticketlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, order_state: order_state, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询订单列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })
            }



            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchList(page);

                        return false;
                    }
                });
            }

            $("#Search").click(function () {
                SearchList(1);
            })

            //提交
            $("#submit_conf").click(function () {

                var id_temp = $("#F1").html();
                if (id_temp == "") {
                    $.prompt("操作出现错误");
                    return;
                }
                var remarks = $("#remarks").val(); //  电子码
                if (remarks == "") {
                    $.prompt("请填写完成备注");
                    return;
                }
                var price = $("#F3").val();
                var tee = $("#F9").val();
                if (parseInt(price) > parseInt(tee)) {
                    $.prompt("金额有误，请重新填写");
                    return;
                }


                $.post("/JsonFactory/OrderHandler.ashx?oper=Upticket", { remarks: remarks, id: id_temp, price: price }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#F1").html("");
                        $("#F2").html("");
                        $("#F3").val("");
                        $("#F4").html("");
                        $("#F5").html("");
                        $("#F6").html("");
                        $("#F7").html("");
                        $("#F8").html("");
                        $("#F9").html("");
                        $("#F10").html("");
                        $("#remarks").val("")
                        $("#remarks").removeAttr("disabled");
                        $("#F3").removeAttr("disabled");
                        $("#manageWithdraw").hide();

                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#F1").html("");
                        $("#F2").html("");
                        $("#F3").val("");
                        $("#F4").html("");
                        $("#F5").html("");
                        $("#F6").html("");
                        $("#F7").html("");
                        $("#F8").html("");
                        $("#F9").html("");
                        $("#F10").html("");
                        $("#remarks").val("")
                        $("#remarks").removeAttr("disabled");
                        $("#F3").removeAttr("disabled");
                        $("#manageWithdraw").hide();

                        $.prompt("操作成功" + data.msg + "返回插入日志ID" + data.finaceback + "作废码" + data.eticket, {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true) {
                                    window.location.reload();

                                }
                            }
                        });
                        //                        $.prompt("操作成功" + data.msg + "返回插入日志ID" + data.finaceback + "作废码" + data.eticket, {
                        //                            buttons: [{ title: '马上退款给客户', value: true }, { title: '稍后退款给用户', value: false}],
                        //                            opacity: 0.1,
                        //                            focus: 0,
                        //                            show: 'slideDown',
                        //                            submit: function (e, v, m, f) {
                        //                                if (v == true) {
                        //                                     
                        //                                    window.open("http://test.etown.cn/ui/vasui/alipay/refund_subpay.aspx?orderid=" + id_temp + "&quit_fee=" + price, target = "_blank");

                        //                                } else {
                        //                                    window.location.reload();
                        //                                }
                        //                            }
                        //                        });
                    }
                })


            })

            //关闭窗口
            $("#cancel_conf").click(function () {
                $("#F1").html("");
                $("#F2").html("");
                $("#F3").val("");
                $("#F4").html("");
                $("#F5").html("");
                $("#F6").html("");
                $("#F7").html("");
                $("#F8").html("");
                $("#F9").html("");
                $("#F10").html("");
                $("#remarks").val("")
                $("#remarks").removeAttr("disabled");
                $("#F3").removeAttr("disabled");
                $("#manageWithdraw").hide();
            })


        })

        //弹出窗口
        function manageWithdraw(id, money, tmoney, remarks, num, ma, pno, come_pay, trade_no, Total_fee, Use_pno, Order_state) {
            $("#title").html("退票处理");
            var mastr = "";
            if (ma == 1) {
                mastr = "本系统电子码";
            }
            if (ma == 2) {
                mastr = "导入电子码";
            }
            if (ma == 3) {
                mastr = "接口电子码";
            }
            //常用的系统码，接口码
            if (Order_state == 18) {
                $("#title").html("直销退款处理");
                $("#show").css('display', 'block');
                $("#F1").html(id);
                $("#F2").html("￥" + money);
                $("#F3").val(tmoney);
                $("#F4").html(remarks);
                $("#F5").html(num + "");
                $("#F6").html(mastr + "【电子码已作废】:" + pno);
                $("#F7").html(come_pay);
                $("#F8").html(trade_no);
                $("#F9").html(Total_fee);
                $("#F10").html(Use_pno);
            }
            //异常，身体退票，数量为0，主要是老数据
            if (Order_state == 17) {
                $("#title").html("直销退票");
                $("#remarks").val("退票申请有误，请核实");
                $("#F3").val(0);
                //$("#F3").attr("disabled", "disabled");
                $("#F1").html(id);
                $("#F2").html("￥" + money);
                $("#F4").html(remarks);
                $("#F5").html(num);
                $("#F6").html(mastr + ":" + pno);
                $("#F7").html(come_pay);
                $("#F8").html(trade_no);
                $("#F9").html(Total_fee);
                $("#F10").html(Use_pno);
            }
            //对异常支付金额不符的处理
            if (Total_fee <= 0) {
                $("#remarks").val("支付金额为0");
                $("#F3").val(0);
                //$("#F3").attr("disabled", "disabled");
                $("#F1").html(id);
                $("#F2").html("￥" + money);
                $("#F4").html(remarks);
                $("#F5").html(num);
                $("#F6").html(mastr + ":" + pno);
                $("#F7").html(come_pay);
                $("#F8").html(trade_no);
                $("#F9").html(Total_fee);
                $("#F10").html(Use_pno);
            }
            $("#manageWithdraw").show();
        };

        function returnticket(id, money, tmoney, remarks, num, ma, pno, come_pay, trade_no, Total_fee, Use_pno, Order_state) {
            manageWithdraw(id, money, tmoney, remarks, num, ma, pno, come_pay, trade_no, Total_fee, Use_pno, Order_state);
        }
        function directreturnticket(id, refundfee, paycome) {
            if (paycome == "alipay" || paycome == "malipay") {
                if (refundfee=="0") {
                    alert("退款金额不可为0");
                    return;
                }

                window.open("/ui/vasui/alipay/refund_subpay.aspx?orderid=" + id + "&quit_fee=" + refundfee, target = "_blank");
            }
            else {
                var paycomedes = ""; //订单支付来源描述
                if (paycome == "alipay" || paycome == "malipay") {
                    paycomedes = "支付宝支付";
                }
                else if (paycome == "wx") {
                    paycomedes = "微信支付";
                }
                else if (paycome == "mtenpay") {
                    paycomedes = "财付通支付";
                }
                else if (paycome == "qunar") {
                    paycomedes = "去哪儿支付";
                    alert("去哪儿支付订单无需退款，款额没有支付到易城");
                    return;
                } else {
                    alert("订单来源有误");
                    return;
                }



                alert("订单是" + paycomedes + ",直接退款暂时只支持订单来源是支付宝支付的");
                return;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ComFinance.aspx" onfocus="this.blur()" target=""><span>财务对账表</span></a></li>
                <li><a href="Withdraw_handle.aspx" onfocus="this.blur()"><span>提现财务管理</span></a></li>
                <li class="on"><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <li><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>产品类目</span></a></li>
            </ul>
        </div>
        <div style="text-align: center;">
            <label>
                请输入(手机，姓名,订单号)
                <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
            </label>
            <select id="order_state">
                <option value="0">全部</option>
                <option value="16">已退票</option>
                <option value="17" selected>未退票</option>
            </select>
            <label>
                <input name="Search" type="button" id="Search" value="退票查询" style="width: 120px;
                    height: 26px;">
            </label>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0" align="left">
                    <tr>
                        <td width="45" height="30">
                            <p align="left">
                               订单号
                            </p>
                        </td>
                        <td width="90" height="30">
                            <p align="left">
                                提交时间
                            </p>
                        </td>
                        <td width="87">
                            <p align="center">
                                商户
                            </p>
                        </td>
                        <td width="147">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                支付方式
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                购买人
                            </p>
                        </td>
                        <td width="30">
                            <p align="center">
                                金额
                            </p>
                        </td>
                        <td width="30">
                            <p align="center">
                                处理人
                            </p>
                        </td>
                        <td width="60">
                            <p align="center">
                                申请退票金额
                            </p>
                        </td>
                        <td width="137">
                            <p align="center">
                                状态
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr class="fontcolor">
                        <td valign="top">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${jsonDateFormatKaler(Backtickettime)}</p>
                        </td>
                        <td valign="top">
                            <p align="center" title="${Comname}">
                                ${Comname}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left" title="${Proname}(数量:${U_num})">
                               ${Proname}(数量:${U_num})
                            </p>
                        </td>
                        <td valign="top">
                           {{if Agentid>0}}
                                   <p align="left" title="${Agentname}" >
                                   分销下单(${Agentname})</p>
                           {{else}}
                                  <p align="left" title="${Pay_str}">
                                 ${Pay_str}</p>
                           {{/if}}
                        </td>
                        <td valign="top">
                            <p align="left" title="${U_name}(${U_phone})">
                                ${U_name}(${U_phone})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                ${Totalcount}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                自动
                            </p>
                        </td>
                        <td >
                            <p align="center">
                                ${askquitfee}                            </p>                        </td>
                        <td valign="top">
                            <p align="center">
                             {{if Order_state==17 || Order_state==18}}<input type="button" onclick="returnticket('${Id}','${Totalcount} 快递费:${Express}','${askquitfee}','${Ticketinfo}','${U_num}','${ma}','${pno}','${come_pay}','${trade_no}','${Total_fee}','${Use_pno}',${Order_state})"  value="手动退款"/><input type="button" onclick="directreturnticket('${Id}','${askquitfee}','${come_pay}')"  value="支付宝接口退款"/>{{/if}}  {{if Order_state==16}}已处理退票{{/if}}
                            <p>
                        </td>
                        
                    </tr>
    </script>
    <div id="manageWithdraw" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 400px; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="title"></span>
                </td>
            </tr>
            <tr>
                <td width="21%" height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    订单号:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="">
                    <span style="padding-left: 5px;" id="F1"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    订单金额:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td1">
                    <span style="padding-left: 5px;" id="F2"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    预订数量:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td5">
                    <span style="padding-left: 5px;" id="F5"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    可退票数量:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td10">
                    <span style="padding-left: 5px;" id="F10"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    状态:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td6">
                    <span style="padding-left: 5px;" id="F6"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    支付来源:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td7">
                    <span style="padding-left: 5px;" id="F7"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    支付返回码:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td8">
                    <span style="padding-left: 5px;" id="F8"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    支付金额:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td9">
                    <span style="padding-left: 5px;" id="F9"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    退票金额:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td2">
                    <input id="F3" style="width: 30px;" type="text" value="" />元 【请按实际退票金额填写】
                </td>
            </tr>
            <tr>
                <td height="35" align="right" bgcolor="#E7F0FA" class="tdHead">
                    备注:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td3">
                    <span style="padding-left: 5px; word-break: break-all; width: 200px; overflow: auto;"
                        id="F4"></span>
                </td>
            </tr>
            <tr>
                <td height="35" align="right" bgcolor="#E7F0FA" class="tdHead">
                    退票成功:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td4">
                    <textarea name="remarks" cols="35" rows="3" id="remarks"></textarea>
                    <br>
                    退票成功后请详细说明，此内容商家可看到；
                </td>
            </tr>
            <tr>
                <td height="20" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_luru_proid" value="" />
                    <input name="submit_luru" id="submit_conf" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_luru" id="cancel_conf" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
