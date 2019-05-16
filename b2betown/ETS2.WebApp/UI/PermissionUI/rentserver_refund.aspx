<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rentserver_refund.aspx.cs"
    Inherits="ETS2.WebApp.UI.PermissionUI.rentserver_refund" MasterPageFile="~/UI/Etown.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            var pageSize = 15; //每页显示条数 
            //获取订单列表
            SearchList(1);
            //装载产品列表
            function SearchList(pageindex) {
                var refundstate = $("#refundstate").trimVal();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                var key = $("#key").val();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getyajinrefundLoglist",
                    data: { pageindex: pageindex, pagesize: pageSize, refundstate: refundstate, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
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
        })

        function directreturnticket(rentserverid, orderid, refundfee, paycome, b2b_eticket_Depositid) {
            if (paycome == "alipay" || paycome == "malipay") {
                if (refundfee == "0") {
                    alert("退款金额不可为0");
                    return;
                }
                window.open("/ui/vasui/alipay/refund_subpay.aspx?orderid=" + orderid + "&quit_fee=" + refundfee + "&rentserverid=" + rentserverid + "&b2b_eticket_Depositid=" + b2b_eticket_Depositid, target = "_blank");
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
                alert("订单是" + paycomedes + ",退押金只支持订单来源是支付宝支付的");
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
                <li class="on"><a href="rentserver_refund.aspx" onfocus="this.blur()"><span>退押金管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <li><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>产品类目</span></a></li>
            </ul>
        </div>
        <div style="text-align: center;">
            <label>
                请输入(手机，姓名,订单号)
                <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
            </label>
            <select id="refundstate">
                <option value="-1">全部</option>
                <option value="0">未退押金</option>
                <option value="1">已退押金</option>
                <option value="2" selected="selected">退押金处理中</option>
            </select>
            <label>
                <input name="Search" type="button" id="Search" value="退押金查询" style="width: 120px;
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
                                产品名称
                            </p>
                        </td>
                        <td width="87">
                            <p align="center">
                                服务名称
                            </p>
                        </td>
                        <td width="147">
                            <p align="left">
                                商户
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                支付方式
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                总金额
                            </p>
                        </td>
                        <td width="30">
                            <p align="center">
                                退款金额
                            </p>
                        </td>
                        <td width="30">
                            <p align="center">
                                提交时间
                            </p>
                        </td>
                        <td width="60">
                            <p align="center">
                                退款状态
                            </p>
                        </td>
                        <td width="137">
                            <p align="center">
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
                                ${orderid}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${proname}</p>
                        </td>
                        <td valign="top">
                            <p align="center" >
                                ${rentservername}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left"  >
                              ${comname}
                            </p>
                        </td>
                        <td valign="top">
                            ${pay_com}
                        </td>
                        <td valign="top">
                            <p align="left" title="${U_name}(${U_phone})">
                              ${ordertotalfee}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                ${refundfee}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                ${jsonDateFormatKaler(subtime)}
                            </p>
                        </td>
                        <td >
                            <p align="center">
                              {{if  refundstate==0}}
                                 失败
                              {{else}}
                                  {{if refundstate==1}} 
                                     成功
                                  {{else}}
                                     退押金处理中
                                  {{/if}}
                              {{/if}}
                                ${refundremark}
                             </p>       
                        </td>
                        <td valign="top">
                            <p align="center">
                                  {{if refundstate==2}}
                                  <input type="button" onclick="directreturnticket('${rentserverid}','${orderid}','${refundfee}','${pay_com}','${b2b_eticket_Depositid}')"  value="支付宝接口退款"/>{{/if}}   
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
