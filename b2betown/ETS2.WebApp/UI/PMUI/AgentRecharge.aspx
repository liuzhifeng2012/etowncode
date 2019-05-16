<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="AgentRecharge.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentRecharge" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var pageSize = 10; //每页显示条数
            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();

            //获取订单列表
            SearchList(1);
            //装载产品列表
            function SearchList(pageindex) {
                var key = $("#key").trimVal();
                var order_state = $("#order_state").trimVal();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getorderlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, order_state: order_state, ordertype: 4 },
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
//                                $("#tblist").html("查询数据为空");
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

        //关闭窗口
        function w_cancel() {
            //退票
            $("#showwarrant").hide();
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                <li ><a href="AgentManage.aspx" onfocus="this.blur()" target=""><span>易城商户分销商</span></a></li>
                <li><a href="AgentSalesCode.aspx" onfocus="this.blur()" target=""><span>后台销售订单</span></a></li>
                <li><a href="AgentBackCode.aspx" onfocus="this.blur()" target=""><span>导码订单</span></a></li>
                <li class="on"><a href="AgentRecharge.aspx" onfocus="this.blur()" target=""><span>充值订单</span></a></li>
                   <li ><a href="AgentRecharge_Person.aspx" onfocus="this.blur()" target=""><span>人工充值记录</span></a></li>
                <li><a href="AgentMessage.aspx" onfocus="this.blur()" target="">管理分销通知</a></li>
                <li><a href="AgentFinanceCount.aspx" onfocus="this.blur()" target="">分销商统计</a>
                
                
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    订单管理</h3>
                <div style="text-align: center;">
                    <label>
                        请输入(手机，姓名)
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <select id="order_state">
                        <option value="0" selected>全部</option>
                        <option value="1">待支付款</option>
                        <option value="2">支付成功（短信发送失败）</option>
                        <option value="4">已发送短信（已经支付成功）</option>
                    </select>
                    <label>
                        <input name="Search" type="button" id="Search" value="订单查询" style="width: 120px;
                            height: 26px;" >
                    </label>
                </div>
                <table width="780px" border="0">
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
                        <td width="70px">
                            <p align="left">
                                交易号
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                购买人
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
                        <td width="35px">
                            <p align="center">
                                渠道
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
                <p>
                    &nbsp;
                </p>
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
                               ${Proname}(${Agentname})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                {{if Server_type==3}}${ChangeDateFormat(U_traveldate)}{{else}}订单号:${Id}{{/if}}
                                
                                </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${U_name}(${U_phone})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Server_type==3}}${Pay_price}{{else}} ${Totalcount}{{/if}}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                 {{if Order_state>1}} ${Integral1}{{/if}}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Server_type==3}}${Pay_price}{{else}}{{if Order_state>1}} ${Paymoney} {{if Imprest1>=1}}+ ${Imprest1}{{/if}}{{/if}}{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Agentid!="0"}} 分销 {{if Warrant_type=="1"}}(出){{else}}(验){{/if}} {{else}}{{if Openid==""}}网站{{else}}微信{{/if}}{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                            {{if Order_state==19}}
                                 作废订单
                            {{else}}
                            {{if Server_type==3}}
                                    旅游优惠券
                            {{else}}
                                   {{if Agentid=="0"}}
                                        ${Order_state_str}
                                        {{if Order_state==2}}
                                            {{if Order_type==1}}
                                                <input type="button" onclick="sendticketsms('${Id}')" style="color:#ff0000"  value="立即发码"/>(可能码原不足)
                                            {{else}}
                                                （充值失败，请手工为此会员充值）
                                            {{/if}}
                                        {{/if}}
                                        {{if Order_state==4}} 
                                            {{if Warrant_type==1}}
                                            <input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                            <input type="button" onclick="referrer_ch('${U_name}(${U_phone})',' ${Proname}','${Order_eticket_code}')"  value="短信"/>
                                            <input type="button" onclick="referrer_ch('查看本订单支付详情','','${Pay_str}')"  value="支付"/>
                                            <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}')"  value="退票"/>
                                            {{else}}
                                                <a href="/Agent/DownExcel.aspx?comid=${Comid}&agentid=${Agentid}&orderid=${Id}&md5info=${Returnmd5}" style="font-size: 14px; font-weight: bold;color:#333933">下载电子码</a>
                                            {{/if}}
                                        {{/if}}
                                   {{else}}
                                        {{if Order_state==4}} 
                                            {{if Warrant_type==1}}
                                            <input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                            <input type="button" onclick="referrer_ch('${U_name}(${U_phone})',' ${Proname}','${Order_eticket_code}')"  value="短信"/>
                                            <input type="button" onclick="referrer_ch('查看本订单支付详情','','${Pay_str}')"  value="支付"/>
                                            <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}')"  value="退票"/>
                                            {{else}}
                                                <a href="/Agent/DownExcel.aspx?comid=${Comid}&agentid=${Agentid}&orderid=${Id}&md5info=${Returnmd5}" style="font-size: 14px; font-weight: bold;color:#333933">下载电子码</a>
                                            {{/if}}
                                        {{/if}}
                                        
                                        {{if Order_state==1}} 
                                            {{if Warrant_type==1}}
                                                {{if Order_type==1}}
                                                    分销购票失败
                                                {{else}}
                                                    充值等待支付
                                                {{/if}}
                                            {{else}}
                                                {{if Order_type==1}}
                                                <input type="button" onclick="agentwarrant_ch('${Agent_company} 分销倒码确认','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}')"  value="分销倒码, 待确认"/>
                                                {{else}}
                                                    充值等待支付
                                                {{/if}}

                                            {{/if}}
                                        {{/if}}
                                        {{if Order_state==2}} 
                                            支付成功
                                            {{if Order_type==1}}
                                                    ，出票出错
                                            {{else}}
                                                    ，分销充值出错，请为其手工充值
                                            {{/if}}
                                        {{/if}}
                                   {{/if}}
                                   {{if Order_state==6}}已充值{{/if}}  
                                   {{if Order_state==16}}${Ticketinfo}{{/if}}
                                   
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
</asp:Content>
