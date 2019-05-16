<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="travelbusordercount.aspx.cs"     MasterPageFile="/Agent/Manage.Master"   Inherits="ETS2.WebApp.Agent.travelbusordercount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //日历
            var startdate = '<%=this.nowdate %>';
            var enddate = '<%=this.monthdate %>';

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {

                $("#startdate").val(startdate);
                $("#enddate").val(enddate);
                $($(this)).datepicker();
            });

            var servertype = 10; //旅游大巴
            var comid = $("#hid_comid").trimVal();
            SearchList(startdate, enddate, servertype, comid);



            $("#chaxun").click(function () {
                var startdate = $("#startdate").trimVal();
                var enddate = $("#enddate").trimVal();
                var comid = $("#hid_comid").trimVal();
                var servertype = $("#hid_servertype").trimVal();


                SearchList(startdate, enddate, servertype, comid);
            })
        }) 

        function viewdetail(index, daydate) {
            if ($("#tr_hid_" + index).trimVal() == 0) {
                $("#tr_hid_" + index).val(1);
                $("#btn_" + index).val("关闭");

                $.post("/jsonfactory/AgentHandler.ashx?oper=agentGetb2bcomprobytraveldate", { daydate: daydate, comid: $("#hid_comid").trimVal(), servertype: $("#hid_servertype").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1)
                    { }
                    if (data.type == 100) {
                        var tbodystr = '<tbody id="tbody_' + index + '" style="display: ; background: #DDDDDD;">';
                        for (var i = 0; i < data.msg.length; i++) {
                            tbodystr += '<tr>' +
                                     '<td>' +
                                         '<p align="left">' +
                                             data.msg[i].Proname + '(支付成功人数:' + data.msg[i].paysucbooknum + '人;)';

                            tbodystr += '</p>' +
                                     '</td>' +
                                     '<td>';

                                if (data.msg[i].paysucbooknum == 0) { tbodystr += '<label>无需处理</label>'; } else {
                                    
                                    tbodystr +=  '<input type="button" id="Btndown_' + i + '" onclick="chakankehu_paysuc(\'' + daydate + '\',' + data.msg[i].Proid + ',' + data.msg[i].paysucbooknum + ')" value="查看支付成功名单" />' ;
                                }
                            

                            tbodystr += '</td>' +
                                 '</tr>';
                        }
                        tbodystr += '</tbody>';

                        var trhtml = $("#tr_" + index).html();

                        var tbhtml = $("#tb").html();
                        tbhtml = tbhtml.replace(trhtml, trhtml + tbodystr);
                        $("#tb").html(tbhtml);
                    }
                })
            } else {
                $("#tr_hid_" + index).val(0);
                $("#btn_" + index).val("展开");

                var tbodyhtml = $("#tbody_" + index).html();

                var tbhtml = $("#tb").html();
                tbhtml = tbhtml.replace(tbodyhtml, "");
                $("#tb").html(tbhtml);
            }
        }
        function SearchList(startdate, enddate, servertype, comid) {
            var day = DateDiff(enddate, startdate);
            if (parseInt(day) < 0) {
                alert("起始时间不可大于结束时间");
                return;
            }

            if (parseInt(day) > 14) {
                alert("查询区间不可大于15天");
                return;
            }

            $.post("/jsonfactory/AgentHandler.ashx?oper=agenttravelbusordercountbyday", { startdate: startdate, enddate: enddate, servertype: servertype, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#tb").html("");
                    var str1 = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        str1 += '<tr id="tr_' + i + '">' +
                                    '<td>' +
                                        '<p align="left">' +
                                            data.msg[i].daydate + '(支付成功人数:' + data.msg[i].paysucnum + '人;)' +
                                        '</p>' +
                                    '</td>' +
                                    '<td>';
                        if (parseInt(data.msg[i].paysucnum) > 0) {
                            str1 += '<input type="button" onclick="viewdetail(' + i + ',\'' + data.msg[i].daydate + '\')" id="btn_' + i + '" value="展开" />' +
                                        '<input type="hidden" id="tr_hid_' + i + '" value="0" />' +
                                    '</td>' +
                                '</tr>';
                        } else {
                            str1 += '<label>无需处理</label>' +
                                    '</td>' +
                                '</tr>';
                        }
                    }
                    $("#tb").html(str1);

                    if (parseInt(data.msg[0].paysucnum) > 0) {
                        viewdetail(0, startdate);
                    }
                }
            })
        } 
     
    </script>
    <style>
        .popover.arrow_left
        {
            margin-left: 8px;
            margin-top: 0;
        }
        .rich_buddy
        {
            z-index: 1;
            width: 240px;
            padding-top: 0;
        }
        .rich_buddy .popover_inner
        {
            padding: 25px 25px 35px;
        }
        .popover .popover_inner
        {
            border: 1px solid #D9DADC;
            word-wrap: break-word;
            word-break: break-all;
            padding: 30px 25px;
            background-color: white;
            box-shadow: none;
            -moz-box-shadow: none;
            -webkit-box-shadow: none;
            height: 180px;
        }
        .rich_buddy .popover_content
        {
            width: auto;
        }
        .rich_buddy_hd
        {
            padding-bottom: 10px;
        }
        v .vm_box
        {
            display: inline-block;
            height: 100%;
            vertical-align: middle;
        }
        .rich_buddy .frm_control_group
        {
            padding-bottom: 0;
            float: left;
            width: 160px;
            padding: 5px 0;
        }
        .rich_buddy .frm_label
        {
            width: 150px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            word-wrap: normal;
        }
        .frm_label
        {
            float: left;
            margin-right: 1em;
            font-size: 14px;
        }
        .frm_controls
        {
            display: table-cell;
            vertical-align: top;
            float: none;
            width: auto;
        }
        .frm_vertical_pt
        {
            padding-top: .3em;
        }
        .remark_name
        {
            margin-top: 5px;
            padding-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
    <table>
                    <tr>
                        <td class="tdHead" style="font-size:14px; height:26px;">
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
            <div class="navsetting ">
            
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
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="EticketBack.aspx?comid=<%=comid_temp %>">退订/订单状态</a>
                </div></div>
            </li>
            <% if (ishaslvyoubusproorder == 1)
                   {%>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_sel" style="position:relative;"><div>
                <a href="/Agent/travelbusordercount.aspx?comid=<%=comid_temp %>">旅游大巴统计</a>
                </div></div>
            </li>
            <% } %>
         </ul>
         <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right">
                    <label>
                       自定义查询：从
                    <input type="text" class="mi-input" maxlength="100" size="15" id="startdate" isdate="yes">
                     </label> <label>
                    到
                    <input type="text" class="mi-input" maxlength="100" size="15" id="enddate" isdate="yes">
                     </label> <label>
                    <input type="button" value="查询" method="post" id="chaxun">
                    </label>
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         
         </div></div>
            </div>
        </div>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    旅游大巴统计</h3>
                <div>
                </div>
                <table border="0" id="tb">
                </table>
            </div>
        </div>
    </div> 
    <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left: 2%; position: absolute; top: 20%;">
        <table width="1100px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr>
                <td align="center" colspan="5">
                    <span style="font-size: 14px;">客户名单</span>
                </td>
            </tr>
            <tbody id="tbody_orderlist">
            </tbody>
            <tr>
                <td colspan="5" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel_rh" id="closebtn" type="button" class="formButton" value="  关 闭  " />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        //查看支付成功名单(iscloseteam:1已经截团；0没有截团)
        function chakankehu_paysuc(daydate, proid, paysucbooknum) {
            if (paysucbooknum == 0) {
                alert("支付成功名单为空");
                return;
            }
            $.post("/JsonFactory/AgentHandler.ashx?oper=agenttravelbustravelerlistBypaystate", { gooutdate: daydate, proid: proid, paystate: $("#hid_paystate_haspay").trimVal(), orderstate: '4,8,22' }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#proqrcode_rhshow").show();
                    var t_head = "<tr><td>订单号</td><td>乘车人姓名</td><td>乘车人手机</td><td>乘车人身份证</td><td>乘车人民族</td><td>预订人</td><td>预订人数</td><td>预订时间</td><td>上车地点</td><td>下车地点</td><td>购票地点</td> <td>备注</td> </tr>";
                    var t_body = ""; 
                    for (var i = 0; i < data.msg.length; i++) {
                        t_body += "<tr><td>" + data.msg[i].orderid + "</td><td>" + data.msg[i].name + "</td><td>" + data.msg[i].contactphone + "</td><td>" + data.msg[i].IdCard + "</td><td>" + data.msg[i].Nation + "</td><td>" + data.msg[i].yuding_name + "</td><td>1</td><td>" + jsonDateFormatKaler(data.msg[i].yuding_time) + "</td><td>" + data.msg[i].pickuppoint + "</td><td>" + data.msg[i].dropoffpoint + "</td><td>" + data.msg[i].agentname + "</td><td>" + data.msg[i].contactremark + "</td> </tr>";
                    }
                    $("#tbody_orderlist").html(t_head + t_body);
                }
            })
        }
       
        $(function () {
            $("#closebtn").click(function () {
                $("#proqrcode_rhshow").hide();
            })
        })
    </script>
   
   
    <div class="data">
        <input type="hidden" id="hid_servertype" value="<%=servertype %>" />
        <input type="hidden" id="hid_orderstate_paysuc" value="<%=orderstate_paysuc %>" />
        <input type="hidden" id="hid_paystate_haspay" value="<%=paystate_haspay %>" />
        <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
         <input type="hidden" id="hid_comid" value="<%=comid_temp %>" />
         <input type="hidden" id="hid_comid_temp" value="<%=comid_temp %>" />
    </div>
</asp:Content>
