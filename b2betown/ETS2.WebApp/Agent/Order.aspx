<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"
    CodeBehind="Order.aspx.cs" Inherits="ETS2.WebApp.Agent.Order" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var account = $("#hid_account").trimVal();
            var comid = $("#hid_comid_temp").trimVal();

            $("#agentordertoexcel").click(function () {
                window.open("/excel/DownExcel.aspx?oper=agentordertoexcel&order_state=" + $("#sel_orderstate").trimVal() + "&servertype=" + $("#sel_servertype").trimVal() + "&agentid=" + agentid + "&account=" + account + "&comid=" + comid + "&key=" + $("#key").trimVal() + "&beginDate=" + $("#startime").trimVal() + "&endDate=" + $("#endtime").trimVal(), target = "_blank");
            })
            //服务类型
            $.post("/JsonFactory/ProductHandler.ashx?oper=getservertypelist", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#sel_servertype").html('<option value="0">全部</option>');
                }
                if (data.type == 100) {
                    var optionstr = '<option value="0">全部</option>';
                    for (var i = 0; i < data.msg.length; i++) {
                        optionstr += '<option value="' + data.msg[i].ID + '">' + data.msg[i].Value + '</option>'
                    }
                    $("#sel_servertype").html(optionstr);
                }
            })

            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });


            SearchList(1);

            //查询
            $("#Search").click(function () {
                var pageindex = 1;
                SearchList(pageindex);
            })

            $("#0").click(function () {
                select(0, 1);
                $("#0").css("color", "red");
            })
            $("#1").click(function () {
                select(1, 1);
                $("#1").css("color", "red");
            })
            $("#3").click(function () {
                select(3, 1);
                $("#3").css("color", "red");
            })
            $("#4").click(function () {
                select(4, 1);
                $("#4").css("color", "red");
            })


            $("#Enter").click(function () {
                var id = $("#hid_id").val();
                var proid = $("#hid_proid").val();
                var num = $("#tnum").val();
                var testpro = $("#testpro").val();
                var comid = $("#hid_comid_temp").trimVal();
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
                    url: "/JsonFactory/OrderHandler.ashx?oper=getticket",
                    data: { comid: comid, id: id, proid: proid, num: num, testpro: testpro },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //$.prompt("提交退款申请错误");
                            $.prompt(data.msg);
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

            //加载qq
            $("#loading").show();
            $.ajax({
                type: "post",
                url: "/JsonFactory/CrmMemberHandler.ashx?oper=channelqqList",
                data: { comid: comid, pageindex: 1, pagesize: 12 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    $("#loading").hide();
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        var qqstr = "";
                        for (var i = 0; i < data.msg.length; i++) {
                            qqstr += '<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&amp;uin=' + data.msg[i].QQ + '&amp;site=qq&amp;menu=yes"><img style="vertical-align:bottom;padding-left:5px;" src="/images/qq.png" alt="' + data.msg[i].QQ + '" title="' + data.msg[i].QQ + '" border="0"></a>'
                        }
                        $("#contentqq").append(qqstr);

                    }
                }
            })

        })

        //显示备注
        function showremark(str) {
            if (str != '') {
                $("#span_rh").text("提单备注:");
                $("#smstext").text(str);
                $("#rhshow").show();
            }
        }

        function sendticketsms(oid) {
            if (oid == '') {
                $.prompt("参数传递错误");
                return;
            }
            var comid = $("#hid_comid_temp").trimVal();
            if (confirm("确认发送吗")) {
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

        }
        function restticketsms(oid) {
            if (oid == '') {
                $.prompt("参数传递错误");
                return;
            }
            var comid = $("#hid_comid_temp").trimVal();
            if (confirm("确认重发吗")) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=restticketsms",
                    data: { comid: comid, oid: oid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //                            $.prompt("重发短信失败");
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("重发成功", { submit: function (e, v, m, f) {
                                if (v == true) {
                                    window.location.reload();
                                }
                            }
                            });

                        }
                    }
                })
            }

        }

        function agentorderpay(oid) {
            var comid = $("#hid_comid_temp").trimVal();
            if (confirm("确认结算吗")) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=agentorderpay",
                    data: { comid: comid, id: oid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            alert("结算成功");
                            location.href = "Order.aspx?comid=" + comid;
                        }
                    }
                })
            }
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
        function backticket_ch(type, id, proid, proname, pronum, pirce, backnum) {
            $("#span_ticket").text(type);
            $("#pro_name").html(proname);
            $("#pro_num").html(pronum);
            $("#tnum").val(backnum);
            $("#tnum").attr("readonly", "readonly");

            $("#hid_id").val(id);
            $("#hid_proid").val(proid);
            $("#span_price").html(pirce);
            $("#showticket").show();
        }
        //淘宝发码
        function taobaosendcoderet(selforderid) {
            if (confirm("确认淘宝发码吗")) {
                $.post("/JsonFactory/OrderHandler.ashx?oper=taobaosendcoderet", { selforderid: selforderid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        alert("淘宝发码成功");
                        location.reload();
                    }
                })
            }
        }

        document.onkeydown = keyDownSearch;

        function keyDownSearch(e) {
            // 兼容FF和IE和Opera  
            var theEvent = e || window.event;
            var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
            if (code == 13) {
                $("#Search").click(); //具体处理函数  
                return false;
            }
            return true;
        }

        //装载产品列表
        function SearchList(pageindex) {
            var agentid = $("#hid_agentid").trimVal();
            var account = $("#hid_account").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            var key = $("#key").trimVal();

            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }

            if ($("#startime").trimVal() != "" || $("#endtime").trimVal() != "") {
                if ($("#startime").trimVal() == "" || $("#endtime").trimVal() == "") {
                    alert("开始时间和结束时间需要同时选择");
                    return;
                }
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=getagentorderlist",
                data: { order_state: $("#sel_orderstate").trimVal(), servertype: $("#sel_servertype").trimVal(), pageindex: pageindex, pagesize: pageSize, agentid: agentid, account: account, comid: comid, key: $("#key").trimVal(), beginDate: $("#startime").trimVal(), endDate: $("#endtime").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pageSize, pageindex);

                            var oidstr = ""; //订单号列表
                            $.each($("input[name='hid_hzinsorderid']"), function (i, item) {
                                oidstr += $(item).val() + ",";
                            })
                            if (oidstr != "") {
                                oidstr = oidstr.substr(0, oidstr.length - 1);
                                //查询慧择网保单列表
                                $.post("/JsonFactory/OrderHandler.ashx?oper=GethzinsorderSearch", { oidstr: oidstr }, function (datat) {
                                    datat = eval("(" + datat + ")");
                                    if (datat.type == 1) {
                                        //                                            alert("查询慧择网保单状态出错");
                                        return;
                                    }
                                    if (datat.type == 100) {
                                        for (var ii = 0; ii < datat.msg.length; ii++) {
                                            //                                                alert(datat.msg[ii].insureNum);
                                            $("#span_" + datat.msg[ii].insureNum).text(datat.msg[ii].effectiveStateStr);
                                            if (datat.msg[ii].effectiveState == 1)//未生效之前可以退保
                                            {
                                                $("#back_" + datat.msg[ii].insureNum).show();
                                            }

                                        }
                                    }
                                })
                            }
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
        
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <table>
            <tr>
                <td class="tdHead" id="contentqq" style="font-size: 14px; height: 26px;">
                    <div class="left">
                        <img id="comlogo" src="" class="" height="42"></div>
                    <div class="left comleft">
                        <div>
                            <span>商户名称：
                                <%=company %>
                            </span><span>授权类型：
                                <%=Warrant_type_str%>；</span> <span>
                                    <%if (contact_phone != "")
                                      {%>客服电话：<%=contact_phone %><%} %></span>
                        </div>
                        <div>
                            <%=yufukuan%>
                            <a class="a_anniu" href="Recharge.aspx?comid=<%=comid_temp %>" target="_blank">在线充值</a>
                            <span id="Span1" style="padding-left: 30px;"></span><span id="Span2" style="padding-left: 30px;">
                            </span>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div id="secondary-tabs" class="navsetting ">
            <ul class="composetab">
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="ProjectList.aspx?comid=<%=comid_temp %>">项目列表</a></div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="Manage_sales.aspx?comid=<%=comid_temp%>">产品列表</a></div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_sel" style="position: relative;">
                        <div>
                            <a href="Order.aspx?comid=<%=comid_temp%>">订单记录</a>
                        </div>
                    </div>
                </li>
                <%if (Warrant_type == 2)
                  { %>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="EticketCount.aspx?comid=<%=comid_temp %>">验码统计</a>
                        </div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="Verification.aspx?comid=<%=comid_temp %>">验码记录</a>
                        </div>
                    </div>
                </li>
                <% } %>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="Finane.aspx?comid=<%=comid_temp %>">财务列表</a>
                        </div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="EticketBack.aspx?comid=<%=comid_temp %>">退订/订单状态</a>
                        </div>
                    </div>
                </li>
                <% if (ishaslvyoubusproorder == 1)
                   {%>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="/Agent/travelbusordercount.aspx?comid=<%=comid_temp %>">旅游大巴统计</a>
                        </div>
                    </div>
                </li>
                <% } %>
            </ul>
            <div class="toolbg toolbgline toolheight nowrap" style="">
                <div class="right">
                    <label>
                        <select id="sel_servertype">
                        </select>
                    </label>
                    <label>
                        <select id="sel_orderstate">
                            <option value="0">全部</option>
                            <option value="1">待支付款</option>
                            <option value="2">支付成功(未处理/未发码)</option>
                            <option value="4">已发码/已处理完成</option>
                            <option value="8">已使用/已发货</option>
                            <option value="16">退款</option>
                        </select>
                    </label>
                    <label>
                        提单时间
                        <input class="mi-input" name="startime" id="startime" placeholder="开始时间" value=""
                            isdate="yes" type="text" style="width: 120px;">-
                        <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes"
                            type="text" style="width: 120px;">
                    </label>
                    <label>
                        关键字
                        <input name="key" type="text" id="key" class="mi-input" style="width: 280px;" placeholder="姓名、手机号、电子码、订单号、产品名称" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                </div>
                <div class="nowrap right" unselectable="on" onselectstart="return false;">
                    <a class="btn_gray btn_space" id="agentordertoexcel" href="javascript:;">导出到Excel</a>
                </div>
            </div>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0" class="O2">
                    <tr class="O2title">
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
                        <td width="180px">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                购买人
                            </p>
                        </td>
                         <td width="45px">
                            <p align="center">
                                门市价
                            </p>
                        </td>
                        <td width="45px">
                            <p align="center">
                                单价
                            </p>
                        </td>
                        <td width="30px">
                            <p align="left">
                                数量
                            </p>
                        </td>
                        <td width="30px">
                            <p align="center">
                                优惠
                            </p>
                        </td>
                        <td width="30px">
                            <p align="center">
                                运费
                            </p>
                        </td>
                        <td width="40px">
                            <p align="center">
                                实收
                            </p>
                        </td>
                        <td width="45px">
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
                <tr class="fontcolor d_out" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td valign="top">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${jsonDateFormatKaler(U_subdate)}</p>
                        </td>
                        <td valign="top" title="${Proname}">
                            <p align="left">
                               ${Proname}
                            </p>
                        </td>
                        <td valign="top" title=" ${U_name}(${U_phone})">
                            <p align="left">
                                ${U_name}(${U_phone})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${Face_price}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                            {{if Server_type==9}}
                                {{if Hotelinfo != null}}
                                     ${Hotelviewdanjia(Pay_price,ChangeDateFormat(Hotelinfo.Start_date),ChangeDateFormat(Hotelinfo.End_date))}
                                {{else}}
                                    ${Pay_price}
                                {{/if}}

                            {{else}}
                                ${Pay_price}
                            {{/if}}
                               </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${U_num}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                 {{if Order_state>1}} ${Integral1}{{/if}}</p>
                        </td>
                          <td valign="top">
                            <p align="center">
                                ${Express}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Order_state>1}} ${Paymoney} {{if Imprest1>=1}}+ ${Imprest1}{{/if}}{{if childreduce*Child_u_num>0}}-${childreduce*Child_u_num}{{/if}}{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                {{if Warrant_type=="1"}}出票扣款{{else}}验证扣款{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center"> 
                            {{if Order_eticket_code!=""}}
                              <input type="button" onclick="referrer_ch('${U_name}(${U_phone})',' ${Proname}','${Order_eticket_code}')"  value="短信"/>
                            {{/if}}
                           {{if OrderType_Hzins==0}}<!--非慧择网订单-->
                             {{if Iscantaobo_sendret==1}}
                                <input type="button" onclick="taobaosendcoderet('${Id}')" value="淘宝发码">
                             {{/if}}
                             {{if Source_type!=4 }}
                             {{if Server_type==11}}
                                        {{if Order_state==1}}
                                             {{if Order_type==1}}
                                             
                                             未付款 

                                               {{if Iscanjiesuan==1}}
                                               <input type="button" onclick="agentorderpay('${Id}')"  value="立即结算"/> 
                                               {{/if}}
                                             {{else}}
                                             未付款，充值失败
                                             {{/if}}
                                        {{/if}}
                                        {{if Order_state==2}}
                                             已付款，等待商家发货
                                        {{/if}}
                                        {{if Order_state==4}}
                                            {{if Deliverytype==4}}
                                             自提，已发码<input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                            {{else}}
                                             已发货，${Expresscom}：${Expresscode}
                                            {{/if}}
                                        {{/if}}
                                        {{if Order_state==8}}
                                             已提货${Expresscom}：${Expresscode}
                                        {{/if}}
                                        {{if Order_state==16}}
                                             已退款${Expresscom}：${Expresscode}
                                        {{/if}}
                           {{else}}
                           {{if Server_type==2||Server_type==8}}
                                        {{if Order_state==1}}
                                             {{if Order_type==1}}
                                             未付款 
                                             {{if Iscanjiesuan==1}}
                                             <input type="button" onclick="agentorderpay('${Id}')"  value="立即结算"/> 
                                             {{/if}}
                                             {{else}}
                                             未付款，充值失败
                                             {{/if}}
                                        {{/if}}
                                        {{if Order_state==2}}
                                             已付款(短信发送失败)，等待商家处理<input type="button" onclick="restticketsms('${Id}')"  value="重发"/>
                                        {{/if}}
                                        {{if Order_state==4}} 
                                             已付款，等待商家处理  <input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                        {{/if}}
                                         {{if Order_state==22}} 
                                         订单已处理
                                         {{/if}}
                                         {{if Order_state==23}} 
                                         超时订单
                                         {{/if}}
                                        {{if Order_state==16}}
                                             订单已退款${Expresscom}：${Expresscode}
                                        {{/if}}
                           
                            {{else}}
                                    {{if Order_state==1}}
                                        {{if  Warrant_type=="1"}}
                                            {{if Order_type==1}}
                                             {{if Server_type==9}}
                                                 <input type="button" onclick="agentorderpay('${Id}')"  value="立即结算"/> 
                                             {{else}} 
                                                未付款
                                             {{/if}}
                                             {{if Iscanjiesuan==1}}
                                             <input type="button" onclick="agentorderpay('${Id}')"  value="立即结算"/> 
                                             {{/if}}
                                             {{else}}
                                             未付款，充值失败
                                             {{/if}}
                                        {{else}}
                                            等待商家确认
                                        {{/if}}
                                    {{/if}}

                                    {{if Order_state==19}}
                                       作废订单
                                    {{/if}}
                                    {{if Order_state==20}}
                                       发送失败,请联系客服，我们将核实此订单发送状态
                                    {{/if}}
                                    {{if Order_state==8}}
                                       已消费

                                       
                                        {{if Unuse_Ticket>0}}
                                         {{if yiguoqi==1}}
                                        已过期
                                        {{else}}
                                        <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Unuse_Ticket}')"  value="退票"/>
                                        {{/if}}
                                        {{/if}}
                                    {{/if}}

                                    {{if Order_state==2}}

                                        {{if Order_type==1}}
                                            {{if Server_type==9}}
                                                订单提交成功，等待酒店确认中
                                            {{else}}
                                         <input type="button" onclick="sendticketsms('${Id}')" style="color:#ff0000"  value="立即发码"/>(付款成功)
                                             <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退票"/>
                                            {{/if}}
                                        
                                        {{else}}充值失败，请联系客服
                                         <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退票"/>
                                        {{/if}}
                                       
                                    {{/if}}
                                    {{if Order_state==4}}
                                        {{if Warrant_type=="1"}}
                                        {{if yiguoqi==0}}
                                            <input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                           {{/if}}
                                            <!--<input type="button" onclick="referrer_ch('${U_name}(${U_phone})',' ${Proname}','${Order_eticket_code}')"  value="短信"/>-->
                                             
                                            {{if Source_type==2}}
                                                不支持自动退款
                                            {{else}}
                                                 {{if Server_type==10}}
                                                退票请联系商家
                                                 {{else}}
                                                     {{if Server_type==9}}
                                                     订房已处理
                                                     {{else}}
                                                        {{if yiguoqi==1}}
                                                        已过期
                                                        {{else}}

                                                         <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退票"/>
                                                        {{/if}}
                                                    {{/if}}
                                                {{/if}}
                                            {{/if}}
                                        {{else}}
                                            {{if Agentlevel==0}}
                                            <a href="DownExcel.aspx?comid=${Comid}&agentid=${Agentid}&orderid=${Id}&md5info=${Returnmd5}"  style="font-size: 14px; font-weight: bold;color:#333933">下载电子码</a>
                                            <a href="PrintTicket.aspx?comid=${Comid}&agentid=${Agentid}&orderid=${Id}&md5info=${Returnmd5}"  style="font-size: 14px; font-weight: bold;color:#333933">打印纸质票</a>
                                            
                                            {{else}}
                                                倒码订单
                                            {{/if}}

                                        {{/if}}
                                    {{/if}}
                                    {{if Order_state==6}}已充值
                                    {{/if}} 
                                    {{if Order_state==24}}
                                        ${Order_remark}取消
                                    {{/if}} 
                                    {{if Order_state==25}}
                                        已过期
                                    {{/if}} 
                                    {{if Order_state==16}}
                                    {{if U_num>Cancelnum}}
                                        部分退票 ${Cancelnum}张
                                    {{else}}
                                        订单退票
                                    {{/if}}
                                    
                                    ,${Ticketinfo}{{/if}}
                                    {{if Order_state==17}}退票申请中{{/if}} 
                                    {{if Order_state==18}}退票处理中{{/if}} 
                                    {{if Order_state==23}}取消订单{{/if}} 
                                    {{if Order_state==22}}
                                        {{if Server_type==10}}
                                            已截团
                                        {{else}}
                                            {{if Server_type==9}}
                                                房间预订成功
                                                {{if Hotelinfo != null}}
                                                 <input type="button" onclick="alert('入住日期：${ChangeDateFormat(Hotelinfo.Start_date)}-离店日期：${ChangeDateFormat(Hotelinfo.End_date)}')"  value="详情"/> 
                                                 {{/if}}
                                            {{else}}
                                                已处理
                                            {{/if}}
                                        {{/if}}
                                    {{/if}} 
                                    {{/if}} 
                                   {{/if}}
                          {{else}}

                          
                            {{if BindingOrder != null}}
                            {{if Server_type==11}}
                                {{if BindingOrder.Order_state==1}}
                                    {{if Order_type==1}}
                                             未付款 
                                            {{if Iscanjiesuan==1}}
                                             <input type="button" onclick="agentorderpay('${Id}')"  value="立即结算"/> 
                                             {{/if}}
                                             {{else}}
                                             未付款，充值失败
                                             {{/if}}
                                {{/if}}
                                {{if BindingOrder.Order_state==2}}
                                     已付款，等待商家发货
                                {{/if}}
                                {{if BindingOrder.Order_state==4}}
                                    {{if Deliverytype==4}}
                                     自提，已发码<input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                    {{else}}
                                     已发货，${BindingOrder.Expresscom}：${BindingOrder.Expresscode}
                                    {{/if}}
                                {{/if}}
                                {{if BindingOrder.Order_state==8}}
                                     已提货${BindingOrder.Expresscom}：${BindingOrder.Expresscode}
                                {{/if}}
                                {{if BindingOrder.Order_state==16}}
                                     已退款${BindingOrder.Expresscom}：${BindingOrder.Expresscode}
                                {{/if}}
                           {{else}}

                                    {{if BindingOrder.Order_state==1}}

                                             未付款 
                                             {{if Iscanjiesuan==1}}
                                             <input type="button" onclick="agentorderpay('${Id}')"  value="立即结算"/> 
                                             {{/if}}

                                    {{/if}}

                                    {{if BindingOrder.Order_state==19}}
                                       作废订单
                                    {{/if}}
                                    {{if BindingOrder.Order_state==20}}
                                       发送失败,请联系客服，我们将核实此订单发送状态
                                    {{/if}}
                                    {{if BindingOrder.Order_state==8}}
                                       已消费

                                       

                                            {{if Unuse_Ticket>0}}
                                            {{if yiguoqi==1}}
                                                        已过期
                                                {{else}}
                                                <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Unuse_Ticket}')"  value="退票"/>
                                            {{/if}}
                                        {{/if}}
                                    {{/if}}

                                    {{if BindingOrder.Order_state==2}}
                                        {{if BindingOrder.Order_type==1}}
                                             {{if Server_type==9}}
                                            订单提交成功，等待酒店确认
                                        {{else}}
 <input type="button" onclick="sendticketsms('${Id}')" style="color:#ff0000"  value="立即发码"/>(付款成功)
  <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退票"/>
                                        {{/if}}
                                        {{else}}充值失败，请联系客服
                                         <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退票"/>
                                        {{/if}}
                                       
                                    {{/if}}
                                    {{if BindingOrder.Order_state==4}}
                                        {{if BindingOrder.Warrant_type=="1"}}
                                        {{if yiguoqi==0}}
                                            <input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                              {{/if}}
                                           <!-- <input type="button" onclick="referrer_ch('${U_name}(${U_phone})',' ${Proname}','${Order_eticket_code}')"  value="短信"/>-->
                                              
                                            {{if BindingOrder.Source_type==2}}
                                                不支持自动退款
                                            {{else}}
                                                {{if Server_type==10}}
                                             退票请联系商家
                                             {{else}}
                                                {{if yiguoqi==1}}
                                                        已过期
                                                {{else}}
                                                <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退票"/>
                                                {{/if}}
                                            {{/if}}
                                            {{/if}}
                                        {{else}}
                                        {{if Warrant_type=="1"}}
                                        {{if yiguoqi==0}}
                                         <input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                            {{/if}}
                                            <!--<input type="button" onclick="referrer_ch('${U_name}(${U_phone})',' ${Proname}','${Order_eticket_code}')"  value="短信"/>-->
                                          
                                            {{if BindingOrder.Source_type==2}}
                                                不支持自动退款
                                            {{else}}
                                                {{if yiguoqi==1}}
                                                        已过期
                                                {{else}}
                                                <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退票"/>
                                                {{/if}}
                                            {{/if}}
                                            {{else}}
                                                倒码订单
                                            {{/if}}
                                        {{/if}}
                                    {{/if}}
                                    {{if BindingOrder.Order_state==6}}已充值
                                    {{/if}} 
                                    
                                    
                                    {{if BindingOrder.Order_state==16}}
                                    {{if BindingOrder.U_num>Cancelnum}}
                                        部分退票 ${Cancelnum}张
                                    {{else}}
                                        订单退票
                                    {{/if}}
                                    
                                    ,${BindingOrder.Ticketinfo}{{/if}}
                                    {{if BindingOrder.Order_state==17}}退票申请中{{/if}} 
                                    {{if BindingOrder.Order_state==18}}退票处理中{{/if}} 

                                    {{if BindingOrder.Order_state==22}}
                                        {{if BindingOrder.Server_type==10}}
                                            已截团
                                        {{else}}
                                            已处理
                                        {{/if}}
                                    {{/if}} 
                                    {{if BindingOrder.Order_state==23}}
                                        超时订单,已取消
                                    {{/if}} 
                                    {{if BindingOrder.Order_state==24}}
                                        房满，取消
                                    {{/if}} 

                               {{else}}
                                订单失败
                               {{/if}}
                               {{else}}
                                订单失败
                          {{/if}}
                           {{/if}}
                           {{if Order_remark!=''}}
                            {{if isNaN(Order_remark)}}
                             <input type="button" onclick="showremark('${Order_remark}')"  value="备注"/>
                              {{/if}} 
                            {{/if}}
                                      
                             
                            {{/if}}
                            {{if  OrderType_Hzins==1}}<!--慧择网订单 但没有生成真实保险订单-->
                                 <span id="span_hzinsorderstaus">${Order_state_str}</span>

                                 {{if Pay_state==2}}
                                   {{if Order_state==2||Order_state==20}}<!--订单状态是已付款或者发码出错的可以退保-->
                                 <input type="button" id="back_hzinsorderstaus${Id}" onclick="backticket_ch('退保申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退保"/> 
                                   {{/if}}
                                 {{/if}} 

                                  {{if Order_remark!=''}}
                                   {{if isNaN(Order_remark)}}
                                      <input type="button" onclick="showremark('${Order_remark}')"  value="备注"/>                     {{/if}}
                                 {{/if}}
                            {{/if}}

                            {{if  OrderType_Hzins==2}}<!--慧择网订单 并且已经生成真实保险订单-->
                                 <span id="span_${InsureNum}"></span>
                                  
                                  {{if Order_state==4}}
                                 <input type="button" id="back_${InsureNum}" onclick="backticket_ch('退保申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退保" style="display:;"/>            
                                 {{/if}}
                                 
                                  {{if Bindingagentorderid>0}}
                                  <a href="/agent/Hzins_detail.aspx?orderid=${Bindingagentorderid}" target="_blank" style="text-decoration: underline">保单详情</a>   
                                  <!--保存生成真实保险订单的 本系统订单号-->
                                  <input type="hidden" name="hid_hzinsorderid" value="${Id}">
                                 {{else}}
                                  <a href="/agent/Hzins_detail.aspx?orderid=${Id}" target="_blank" style="text-decoration: underline">保单详情</a>   
                                  <!--保存生成真实保险订单的 本系统订单号-->
                                  <input type="hidden" name="hid_hzinsorderid" value="${Id}">
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
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />
    <input id="hid_account" type="hidden" value="<%=Account %>" />
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>
