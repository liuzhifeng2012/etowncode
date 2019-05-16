<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AgentManageOrder.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentManageOrder" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 25; //每页显示条数

        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid").trimVal();



            $.post("/JsonFactory/AgentHandler.ashx?oper=getAgentId", { agentid: agentid, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取数据出现错误");
                    return;
                }
                if (data.type == 100) {
                    $("#company").html(data.msg.Company);
                    $("#imprest").html("￥ " + data.msg.Imprest);
                    $("#credit").html(data.msg.Credit);
                    $("#warrant_level").html(data.msg.Warrant_level);
                    if (data.msg.Warrant_type == 1) {
                        $("#warrant_type").html("出票扣款");
                    } else if (data.msg.Warrant_type == 2) {
                        $("#warrant_type").html("验证扣款");
                    } else {
                        $("#warrant_type").html("尚未设定或设定错误");
                    }
                }

            })



            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                var key = $("#key").trimVal();


                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getagentorderlist",
                    data: { comid: comid, agentid: agentid, pageindex: pageindex, pagesize: pageSize ,key:key},
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询财务列表错误");
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

            
            $("#Search").click(function () {
                SearchList(1);
            })

            $("#Enter").click(function () {
                var id = $("#hid_id").val();
                var proid = $("#hid_proid").val();
                var num = $("#tnum").val();
                var testpro = $("#testpro").val();

                if (id == 0) {
                    $.prompt("错误id！");
                    return;
                }
                if (num == "") {
                    num = 1;
                }
                if (testpro == "") {
                    $.prompt("请填写退款说明备注！");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=com-getticket",
                    data: { comid: comid, userid: userid, id: id, proid: proid, num: num, testpro: testpro },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("提交退款申请错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#hid_id").val(0);
                            $.prompt("提交退款申请成功");
                            window.location.reload();
                            return;
                        }
                    }
                })

                })


                

            $("#w_Enter").click(function () {
                var id = $("#hid_id").val();
                var proid = $("#hid_proid").val();
                var testpro = $("#w_testpro").val();
                var confirmstate = $('input:radio[name="confirmstate"]:checked').val();
                if (id == 0) {
                    $.prompt("订单ID错误！");
                    return;
                }
                if (proid == "") {
                    $.prompt("产品ID错误！");
                    return;
                }
                if (confirmstate == "") {
                    $.prompt("确认状态错误！");
                    return;
                }


               
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=createdaoma",
                    data: { comid: comid, id: id, proid: proid, confirmstate: confirmstate, testpro: testpro },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("返回错误，请刷新后重新操作");
                            return;
                        }
                        if (data.type == 100) {
                            $("#hid_id").val(0);
                            $.prompt("已经确认");
                            window.location.reload();
                            return;
                        }
                    }
                })


            })


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
        })

        function sendticketsms(oid) {
            if (oid == '') {
                $.prompt("参数传递错误");
                return;
            }
            var comid = $("#hid_comid").trimVal();
            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=sendticketsms",
                data: { comid: comid, oid: oid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("发送短信错误，请查看码库存是否充足！");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("发送成功");
                        window.location.reload();
                    }
                }
            })


        }
        function restticketsms(oid) {
            if (oid == '') {
                $.prompt("参数传递错误");
                return;
            }
            var comid = $("#hid_comid").trimVal();
            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=restticketsms",
                data: { comid: comid, oid: oid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("重发短信失败");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("重发成功");
                        window.location.reload();
                    }
                }
            })


        }


        function referrer_ch(str1, str2, str3) {
            $("#span_rh").text("用户：" + str1 + " 订购:" + str2 + " 短信:");
            $("#smstext").text(str3);
            $("#rhshow").show();
        }

        function cancel() {
            $("#rhshow").hide();
            $("#span_rh").text("");
            $("#smstext").text("");

            //退票
            $("#showticket").hide();
        }

        //退票
        function backticket_ch(type, id, proid, proname, pronum, pirce) {
            $("#span_ticket").text(type);
            $("#pro_name").html(proname);
            $("#pro_num").html(pronum);
            $("#tnum").val(pronum);
            $("#tnum").attr("disabled", "disabled");
            $("#hid_id").val(id);
            $("#hid_proid").val(proid);
            $("#span_price").html(pirce);
            $("#showticket").show();
        }

        
        //倒码确认
        function agentwarrant_ch(type, id, proid, proname, pronum, pirce) {
            $("#w_span_ticket").text(type);
            $("#w_pro_name").html(proname);
            $("#w_num").html(pronum);
            $("#w_num").val(pronum);
            $("#w_num").attr("disabled", "disabled");
            $("#hid_id").val(id);
            $("#hid_proid").val(proid);
            $("#w_price").html(pirce);
            $("#showwarrant").show();
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
    <%--    <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                 
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
             <h3>
                    授权分销商</h3>
                    <table >
                    <tr>
                        <td class="tdHead">
                            公司名称：<span id="company"></span> </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           预付款： <span id="imprest"></span> 
                        </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           信用额度： <span id="credit"></span> 
                        </td>

                    </tr>
                     <tr>
                        <td class="tdHead">
                           授权类型： <span id="warrant_type"></span>
                        </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           授权级别： <span id="warrant_level"></span>
                        </td>

                    </tr>
                </table>


                <h3>
                    <a href="AgentManageOrder.aspx?agentid=<%=agentid %>" class="a_anniu">订单列表</a>  &nbsp; <a href="AgentVCount.aspx?agentid=<%=agentid %>" class="a_anniu">分销验票统计</a> &nbsp; <a href="AgentVerification.aspx?agentid=<%=agentid %>" class="a_anniu" >分销验票记录</a>  &nbsp;  <a href="AgentManageFinance.aspx?agentid=<%=agentid %>" class="a_anniu" >财务管理</a>&nbsp;  <a href="AgentRechargeFinance.aspx?agentid=<%=agentid %>" class="a_anniu" >充值记录</a>  &nbsp;  <a href="AgentManageSet.aspx?agentid=<%=agentid %>"  class="a_anniu">管  理</a></h3><label>
                        关键词查询
                        <input name="key" id="key" style="width: 160px; height: 20px;" type="text" value="<%=key %>">
                    </label>
                     <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                <table width="780" border="0">
                     <tr>
                        <td width="45px" height="30px">
                            <p align="left">
                                ID
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                提交时间
                            </p>
                        </td>
                        <td width="147px">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                        <td width="5px">
                            <p align="left">
                                
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                购买人
                            </p>
                        </td>
                                                <td width="5px">
                            <p align="left">
                                
                            </p>
                        </td>
                        <td width="15px">
                            <p align="center">
                                数量
                            </p>
                        </td>
                        <td width="45px">
                            <p align="center">
                                应收
                            </p>
                        </td>
                        <td width="30px">
                            <p align="center">
                                优惠
                            </p>
                        </td>
                        <td width="45px">
                            <p align="center">
                                实收
                            </p>
                        </td>
                        <td width="45px">
                            <p align="center">
                                毛利
                            </p>
                        </td>
                        <td width="30px">
                            <p align="center">
                                类型
                            </p>
                        </td>
                        <td width="180px">
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
                                ${jsonDateFormatKaler(U_subdate)}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                               ${Proname}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                               </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${U_name}(${U_phone})
                            </p>
                        </td>
                         <td valign="top">
                            <p align="left">
                               </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${U_num}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                ${Totalcount}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                 {{if Order_state>1}} ${Integral1}{{/if}}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Order_state>1}} ${Paymoney} {{if Imprest1>=1}}+ ${Imprest1}{{/if}}{{/if}}
                            </p>
                        </td>
                         <td valign="top">
                            <p align="center">
                               {{if Pay_state==2}}{{if Order_state!=19}} ${Profit}{{/if}}{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                {{if Warrant_type=="1"}}出票扣款{{else}}验证扣款{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                           {{if Source_type!=4 }}
                           {{if Server_type==11}}
                                {{if Order_state==1}}
                                     未付款
                                {{/if}}
                                {{if Order_state==2}}
                                     已付款，待商家发货
                                {{/if}}
                                {{if Order_state==4}}
                                    {{if Deliverytype==4}}
                                     自提，已发码<input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                    {{else}}
                                     订单已发货，${Expresscom}：${Expresscode}
                                    {{/if}}
                                {{/if}}
                                {{if Order_state==8}}
                                     订单已提货
                                {{/if}}
                                {{if Order_state==16}}
                                    {{if U_num>Cancelnum}}
                                        部分退票 ${Cancelnum}张
                                    {{else}}
                                        订单退票
                                    {{/if}}
                                {{/if}}
                                {{if Order_state==23}}
                                     超时订单,已取消
                                {{/if}}
                           {{else}}
                            {{if Order_state==1}}
                                {{if Server_type==3}}
                                    旅游优惠券
                                {{else}}
                                    {{if Server_type!=1 }}
                                        完成
                                    {{else}}
                                        {{if Order_type==1}}
                                             {{if Warrant_type==1}}
                                                 {{if Agentid=="0"}}
                                                     等待对方付款
                                                 {{else}}
                                                     分销购票失败
                                                 {{/if}}
                                             {{else}}
                                                 待确认倒码
                                             {{/if}}
                                        {{else}}
                                            充值等待支付
                                        {{/if}}
                                    {{/if}}
                                {{/if}}
                            {{/if}}

                            {{if Order_state==2}}
                                ${Order_state_str}
                                {{if Order_type==1}}
                                     <input type="button" onclick="sendticketsms('${Id}')" style="color:#ff0000"  value="立即发码"/>
                                {{else}}
                                     （充值失败，请手工为此会员充值）
                                {{/if}}
                            {{/if}}
                            {{if Order_state==4}}
                                
                                 {{if Warrant_type==1}}
                                     已发送
                                     {{else}}
                                     已生成
                                 {{/if}}
                            {{/if}}
                            {{if Order_state==6}}
                            已充值
                            {{/if}}
                            {{if Order_state==8}}
                            已消费
                             {{if Unuse_Ticket>0}}
                            部分使用
                            {{/if}}
                            {{/if}}
                            {{if Order_state==16}}
                            订单退票
                            {{/if}}
                            {{if Order_state==17}}
                            申请退票中
                            {{/if}}
                            {{if Order_state==18}}
                            退票处理中
                            {{/if}}
                            {{if Order_state==19}}
                            作废
                            {{/if}}
                            {{if Order_state==20}}
                            发码出错
                            {{/if}}
                            {{if Order_state==21}}
                            重新发码出错
                            {{/if}}
                            {{if Order_state==22}}
                            已处理
                            {{/if}}
                            {{if Order_state==23}}
                                     超时订单,已取消
                            {{/if}}

                            {{/if}}
                       {{else}}

                          
                            {{if BindingOrder != null}}

                                {{if BindingOrder.Order_state==1}}
                                    {{if BindingOrder.Server_type==3}}
                                        旅游优惠券
                                    {{else}}
                                        {{if BindingOrder.Server_type!=1 }}
                                            完成
                                        {{else}}
                                            {{if BindingOrder.Order_type==1}}
                                                 {{if BindingOrder.Warrant_type==1}}
                                                     {{if BindingOrder.Agentid=="0"}}
                                                         等待对方付款
                                                     {{else}}
                                                         分销购票失败
                                                     {{/if}}
                                                 {{else}}
                                                     待确认倒码
                                                 {{/if}}
                                            {{else}}
                                                充值等待支付
                                            {{/if}}
                                        {{/if}}
                                    {{/if}}
                                {{/if}}

                                {{if BindingOrder.Order_state==2}}
                                    ${BindingOrder.Order_state_str}
                                    {{if BindingOrder.Order_type==1}}
                                         <input type="button" onclick="sendticketsms('${Id}')" style="color:#ff0000"  value="立即发码"/>
                                    {{else}}
                                         （充值失败，请手工为此会员充值）
                                    {{/if}}
                                {{/if}}
                                {{if BindingOrder.Order_state==4}}
                                
                                     {{if Warrant_type==1}}
                                       已发送
                                     {{else}}
                                         已生成
                                     {{/if}}
                                {{/if}}
                                {{if BindingOrder.Order_state==6}}
                                已充值
                                {{/if}}
                                {{if BindingOrder.Order_state==8}}
                                已消费
                                {{if Unuse_Ticket>0}}
                                部分使用
                               {{/if}}
                               {{/if}}
                                {{if BindingOrder.Order_state==16}}
                                    {{if BindingOrder.U_num>Cancelnum}}
                                        部分退票 ${Cancelnum}张
                                    {{else}}
                                        订单退票
                                    {{/if}}
                                {{/if}}
                                {{if BindingOrder.Order_state==17}}
                                申请退票中
                                {{/if}}
                                {{if BindingOrder.Order_state==18}}
                                退票处理中
                                {{/if}}
                                {{if BindingOrder.Order_state==19}}
                                作废
                                {{/if}}
                                {{if BindingOrder.Order_state==20}}
                                发码出错
                                {{/if}}
                                {{if BindingOrder.Order_state==21}}
                                重新发码出错
                                {{/if}}
                                {{if BindingOrder.Order_state==22}}
                                已处理
                                {{/if}}
                                {{if BindingOrder.Order_state==23}}
                                     超时订单,已取消
                                {{/if}}


                            {{/if}}
                            {{/if}}
                                </p>
                        </td>
                    </tr>
    </script>
    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px;" id="span_rh"></span>
                </td>
            </tr>
            <tr>
                <td height="60" align="right" bgcolor="#E7F0FA" class="tdHead">
                </td>
                <td height="60" bgcolor="#E7F0FA" class="tdHead" id="tdgroups">
                    <span id="smstext" style="font-size: 14px;"></span>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" onclick="cancel();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="showticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: auto; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span_ticket">
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    产 品：<span id="pro_name"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    数 量：<span id="pro_num"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    价 格：<span id="span_price"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    退 量：<input type="text" id="tnum" value="" style="width: 30px;" />*按实际未使用的数量退票
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    备 注：<textarea id="testpro" cols="35" rows="2" style="margin-right: 10px;"></textarea>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="Enter" name="Enter" type="button" class="formButton" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

     <div id="showwarrant" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: auto; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="w_span_ticket">
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    产 品：<span id="w_pro_name"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    倒码数量量：<span id="w_num"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    价 格：<span id="w_price"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    备 注：<textarea id="w_testpro" cols="35" rows="2" style="margin-right: 10px;"></textarea>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    <label> <input type="radio" name="confirmstate" value="1" checked="checked" /> 生成电子码</label>
                    <label><input type="radio" name="confirmstate" value="0"/> 作废此笔订单</label>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="w_Enter" name="w_Enter" type="button" class="formButton" value="  确   认  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="w_cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />
    <input type="hidden" id="hid_agentid" value='<%=agentid %>' />
</asp:Content>

