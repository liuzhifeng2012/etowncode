<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"
    CodeBehind="EticketBack.aspx.cs" Inherits="ETS2.WebApp.Agent.EticketBack" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            var key = $("#key").trimVal();


            //查询
            $("#Search").click(function () {
                var pageindex = 1;
                var key = $("#key").val();
                var select = '';

                if (key == "" || key == null) {
                    $.prompt("查询条件为空，请到电子票销售记录中根据手机号查询");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=getagentsearchlist",
                    data: { agentid: agentid, comid: comid, key: key },
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
                            }
                        }
                    }
                })
            })


            //退票
            $("#Enter").click(function () {
                var id = $("#hid_id").val();
                var pno = $("#hid_pno").val();

                if (pno == "" || id == "") {
                    $.prompt("退票参数错误");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=backagentticket",
                    data: { agentid: agentid, comid: comid, pno: pno, id: id },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //                            $.prompt("退票错误");
                            $.prompt(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("退票成功");
                            window.location.reload();
                        }
                    }
                })
            })
            SearchList(1);

            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=getagentbacklist",
                    data: { agentid: agentid, comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist2").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist2").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist2");
                            }
                            setpage(data.totalCount, pageSize, pageindex);
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

        function referrer_ch(str1, str2, str3) {
            $("#span_rh").text("用户：" + str1 + " 订购:" + str2 + " 短信:");
            $("#smstext").text(str3);
            $("#rhshow").show();
        }

        function cancel() {
            $("#rhshow").hide();
            $("#span_rh").text("");
            $("#smstext").text("");
            $("#hid_id").val("");
            $("#hid_pno").val("");
            //退票
            $("#showticket").hide();
        }

        //退票
        function backticket_ch(type, id, e_proname, pno, p_num, use_pnum, pirce) {
            $("#span_ticket").text(type);
            $("#pro_name").html(e_proname);
            $("#pro_num").html(use_pnum);
            $("#tnum").val(use_pnum);
            $("#tnum").attr("disabled", "disabled");
            $("#hid_id").val(id);
            $("#hid_pno").val(pno);
            $("#span_price").html(pirce + ' 元');
            $("#showticket").show();
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
                    <div ><span> 商户名称：
                    <%=company %> </span>
                    <span>授权类型：
                    <%=Warrant_type_str%>；</span> <span><%if (contact_phone != "")
                     {%>客服电话：<%=contact_phone %><%} %></span>
                     </div>
                      <div>
                      <%=yufukuan%>
                    <a class="a_anniu" href="Recharge.aspx?comid=<%=comid_temp %>" target="_blank">在线充值</a> <span id="messagenew"
                        style="padding-left: 30px;"></span><span id="shopcart" style="padding-left: 30px;">
                    </span>
                    </div>
                     </div>
                </td>
            </tr>
           
        </table>
        <div id="secondary-tabs" class="navsetting ">
            <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="ProjectList.aspx?comid=<%=comid_temp %>">项目列表</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div>
                <a href="Manage_sales.aspx?comid=<%=comid_temp%>">产品列表</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Order.aspx?comid=<%=comid_temp%>">订单记录</a>
                </div></div>
            </li>
            <%if (Warrant_type == 2)
                  { %>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="EticketCount.aspx?comid=<%=comid_temp %>">验码统计</a>
                </div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Verification.aspx?comid=<%=comid_temp %>">验码记录</a>
                </div></div>
            </li>
            <% } %>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Finane.aspx?comid=<%=comid_temp %>">财务列表</a>
                </div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_sel" style="position:relative;"><div>
                <a href="EticketBack.aspx?comid=<%=comid_temp %>">退订/订单状态</a>
                </div></div>
            </li>
            <% if (ishaslvyoubusproorder == 1)
                   {%>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="/Agent/travelbusordercount.aspx?comid=<%=comid_temp %>">旅游大巴统计</a>
                </div></div>
            </li>
            <% } %>
         </ul>
         <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right">
                    <label>
                        电子码

                        <input name="key" type="text" id="key" class="mi-input" style="width: 160px;" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         
         </div></div>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">

                <table width="780" border="0" class="O2">
                    <tr class="O2title">
                        <td width="100">
                            <p align="left">
                                提交订单时间
                            </p>
                        </td>
                        <td width="150">
                            <p align="left">
                                服务内容
                            </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                结算类型
                            </p>
                        </td>
                        <td width="66">
                            <p align="left">
                                电子码
                            </p>
                        </td>
                        <td width="90">
                            <p align="left">
                                订购数/验证数/未用数/退票数
                            </p>
                        </td>
                        <td width="72">
                            <p align="left">
                                结算价格
                            </p>
                        </td>
                        <td width="72">
                            <p align="left">
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist"  class="O2title">
                    </tbody>
                </table>
                <h3>
                    退订列表</h3>
                <table width="780" border="0" class="O2">
                    <tr class="O2title">
                        <td width="100">
                            <p>
                                提交订单时间
                            </p>
                        </td>
                        <td width="150">
                            <p>
                                服务内容
                            </p>
                        </td>
                        <td width="60">
                            <p>
                                结算类型
                            </p>
                        </td>
                        <td width="66">
                            <p>
                                电子码
                            </p>
                        </td>
                        <td width="90">
                            <p>
                                订购数/验证数/未用数/退票数
                            </p>
                        </td>
                        <td width="72">
                            <p>
                                结算价格
                            </p>
                        </td>
                        <td width="72">
                            <p>
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist2"  class="O2title">
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
                <tr  class="d_out" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td>
                            <p >
                                ${jsonDateFormatKaler(Subdate)}
                            </p>
                        </td>
                        <td>
                            <p >
                                ${E_proname}</p>
                        </td>
                                                <td>
                            <p >
                                {{if Warrant_type==1}}出票扣款{{else}}验证扣款{{/if}}</p>
                        </td>
                        <td >
                            <p>
                                ${Pno}</p>
                        </td>
                        <td>
                            <p>
                                ${Pnum}/${VerifyNum}/${Use_pnum}/${Cancelnum}</p>
                        </td>
                         
                        <td>
                            <p >
                            ${E_sale_price}元
                            </p>
                        </td>
                            <td>
                            <p>
                            {{if Use_pnum ==0 }}
                                {{if V_state ==4 }}
                                    此电子票已作废
                                {{else}}
                                    此电子票已使用，剩余0张
                                {{/if}}
                            {{else}}       

                                  <input type="button" onclick="backticket_ch('退票申请','${Oid}','${E_proname}','${Pno}','${Pnum}','${Use_pnum}','${E_sale_price}')"  value="退票"/>

                            {{/if}}
                            </p>
                        </td>
                        
                    </tr>
                    
    </script>
    <div id="showticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: auto; height: auto; display: none; left: 20%; position: absolute;
        top: 20%; width: 450px;">
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
                    订购数量：<span id="pro_num"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    单 价：<span id="span_price"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    可退票数量：<input type="text" id="tnum" value="" style="width: 30px;" />*按实际未使用的数量退票
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="Enter" name="Enter" type="button" class="formButton" value="  申请退票  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_pno" value="" />
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>
