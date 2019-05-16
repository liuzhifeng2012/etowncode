<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TravelbusOrderCount.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.MemberUI.TravelbusOrderCount" %>

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
        function chuli(proid, daydate, paysucnum) {
            if (parseInt(paysucnum) == 0) {
                alert("无需结单处理");
                return;
            } else {
                if (confirm("截单后" + daydate + "将不可在收客，确认截单处理吗?")) {

                    $.post("/JsonFactory/OrderHandler.ashx?oper=CloseOrder", { proid: proid, daydate: daydate }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) { }
                        if (data.type == 100) {
                            window.open("TravelbusOrderDetail.aspx?proid=" + proid + "&gooutdate=" + daydate, target = "_self");
                        }
                    });
                }
            }
        }
        function showdetail(proid, daydate) {

            window.open("TravelbusOrderDetail.aspx?proid=" + proid + "&gooutdate=" + daydate, target = "_self");

        }

        function viewdetail(index, daydate) {
            if ($("#tr_hid_" + index).trimVal() == 0) {
                $("#tr_hid_" + index).val(1);
                $("#btn_" + index).val("关闭");

                $.post("/jsonfactory/orderhandler.ashx?oper=Getb2bcomprobytraveldate", { daydate: daydate, comid: $("#hid_comid").trimVal(), servertype: $("#hid_servertype").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1)
                    { }
                    if (data.type == 100) {
                        var tbodystr = '<tbody id="tbody_' + index + '" style="display: ; background: #DDDDDD;">';
                        for (var i = 0; i < data.msg.length; i++) {
                            tbodystr += '<tr>' +
                                     '<td>' +
                                         '<p align="left">' +
                            //                                             data.msg[i].Proname + '(支付成功人数:' + data.msg[i].paysucbooknum + '人;截团人数:' + data.msg[i].closeteamnum + ')';
                                            data.msg[i].Proname + '(支付成功人数:' + data.msg[i].paysucbooknum + '人)';

                            //已经处理过
                            if (data.msg[i].ishasplanbus == 1) {
                                //没有派车
                                if (data.msg[i].busdetail == "") {
                                    tbodystr += "没有派车";
                                }
                                else {
                                    var arr = data.msg[i].busdetail.split(',');

                                    for (var j = 0; j < arr.length; j++) {
                                        var arr_bus = arr[j].split('-');
                                        tbodystr += '<a href="javascript:void(0)" style="cursor: pointer; text-decoration: underline;" onclick="clickbusdetail(' + arr_bus[0] + ')"  id="divimg_' + arr_bus[0] + '">' + (j + 1) + '.' + arr_bus[1] + '</a>&nbsp;&nbsp;';
                                    }
                                }
                            }

                            tbodystr += '</p>' +
                                     '</td>' +
                                     '<td>';
                            if (data.msg[i].ishasplanbus == 1) {
                                if (data.msg[i].paysucbooknum == 0) { tbodystr += '<label>无需处理</label>'; } else {
                                    //                                    tbodystr += '<label>已截团</label>';
                                    //                                    tbodystr += '<input type="button" id="Btnchuli_' + i + '" onclick="showdetail(' + data.msg[i].Proid + ',\'' + daydate + '\')" value="查看详情" />' +
                                    tbodystr += '<input type="button" id="Btndown_' + i + '"  onclick="chakankehu_paysuc(\'' + daydate + '\',' + data.msg[i].Proid + ',' + data.msg[i].paysucbooknum + ')" value="查看支付成功名单" />';
                                    //                                    '<input type="button" id="Btndown_' + i + '"  onclick="chakankehu_closeteam(\'' + daydate + '\',' + data.msg[i].Proid + ',1,' + data.msg[i].closeteamnum + ')" value="查看截团名单" />';
                                }
                            }
                            else {

                                if (data.msg[i].paysucbooknum == 0) { tbodystr += '<label>无需处理</label>'; } else {
                                    //                                    tbodystr += '<label>未截团</label>';
                                    //                                    tbodystr += '<input type="button" id="Btnchuli_' + i + '" onclick="chuli(' + data.msg[i].Proid + ',\'' + daydate + '\',' + data.msg[i].paysucbooknum + ')" value="截单处理" />' +
                                    tbodystr += '<input type="button" id="Btndown_' + i + '" onclick="chakankehu_paysuc(\'' + daydate + '\',' + data.msg[i].Proid + ',' + data.msg[i].paysucbooknum + ')" value="查看支付成功名单" />';
                                    //                                    '<input type="button" id="Btndown_' + i + '"  onclick="chakankehu_closeteam(\'' + daydate + '\',' + data.msg[i].Proid + ',0,' + data.msg[i].closeteamnum + ')" value="查看截团名单" />';
                                }
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

            $.post("/jsonfactory/orderhandler.ashx?oper=travelbusordercountbyday", { startdate: startdate, enddate: enddate, servertype: servertype, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#tb").html("");
                    var str1 = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        str1 += '<tr id="tr_' + i + '">' +
                                    '<td>' +
                                        '<p align="left">' +
                        //                                            data.msg[i].daydate + '(支付成功人数:' + data.msg[i].paysucnum + '人;截团人数:' + data.msg[i].closeteamnu + '人;)' +
                                        data.msg[i].daydate + '(支付成功人数:' + data.msg[i].paysucnum + '人)' +
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
        function detail_show(divid, busid) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=gettravelbus", { busid: busid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {

                    var x = $("#" + divid).offset();
                    $('.rich_buddy').show();
                    $('.rich_buddy').css("top", x.top - 380 + "px").css("left", x.left - 180 + "px");

                    $("#buddy_busmodel").html(data.msg.travelbus_model);
                    $("#buddy_seatnum").html(data.msg.seatnum);
                    $("#buddy_licenceplate").html(data.msg.licenceplate);
                    $("#buddy_drivename").html(data.msg.drivername);
                    $("#buddy_drivephone").html(data.msg.driverphone);

                }
            })

        }
        function detail_hid(divid) {
            $('.rich_buddy').delay(500).hide(0); ;
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
        <%--<div id="secondary-tabs" class="navsetting ">
            <div class="navsetting ">
                <li><a href="/ui/pmui/order/orderlist.aspx" onfocus="this.blur()" target="">订单列表</a></li>
                <li><a href="/ui/MemberUI/ChannelFinance.aspx" onfocus="this.blur()">门票返佣 </a></li>
                <li class="on"><a href="/ui/MemberUI/travelbusordercount.aspx" onfocus="this.blur()">
                    旅游大巴订单统计 </a></li>
            </div>
        </div>--%>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    订单统计</h3>
                <div>
                </div>
                <p>
                    自定义查询：从
                    <input type="text" maxlength="100" size="15" id="startdate" isdate="yes">
                    到
                    <input type="text" maxlength="100" size="15" id="enddate" isdate="yes">
                    <input type="button" value="查询" method="post" id="chaxun">
                </p>
                <table border="0" id="tb">
                </table>
            </div>
        </div>
    </div>
    <div class="rich_buddy popover arrow_left" style="left: 200px; top: 80px; position: relative;
        opacity: 1; display: none;" onmousemove="$('.rich_buddy').show();" onmouseout="  $('.rich_buddy').hide();)">
        <div class="popover_inner">
            <div class="popover_content">
                <div class="rich_buddy_hd">
                    详细资料</div>
                <div class="rich_buddy_bd buddyRichContent" style="display: block;">
                    <div class="frm_control_group nickName">
                        <label class="">
                            车型:<span id="buddy_busmodel"></span></label>
                    </div>
                    <div class="frm_control_group nickName">
                        <label class="frm_label">
                            座位数:<span id="buddy_seatnum"></span></label>
                    </div>
                    <div class="frm_control_group nickName">
                        <label class="frm_label">
                            车牌号码:<span id="buddy_licenceplate"></span></label>
                    </div>
                    <div class="frm_control_group nickName">
                        <label class="frm_label">
                            司机:<span id="buddy_drivename"></span></label>
                    </div>
                    <div class="frm_control_group nickName">
                        <label class="frm_label">
                            司机电话:<span id="buddy_drivephone"></span></label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left:2%; position: absolute; top: 20%;">
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
            $.post("/JsonFactory/OrderHandler.ashx?oper=travelbustravelerlistBypaystate", { gooutdate: daydate, proid: proid, paystate: $("#hid_paystate_haspay").trimVal(), orderstate: '4,8,22' }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#proqrcode_rhshow").show();
                    var t_head = "<tr><td>订单号</td><td>乘车人姓名</td><td>乘车人手机</td><td>乘车人身份证</td><td>乘车人民族</td><td>预订人</td><td>预订人数</td><td>预订时间</td><td>上车地点</td><td>下车地点</td><td>购票地点</td><td>备注</td> </tr>";
                    var t_body = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        t_body += "<tr><td>" + data.msg[i].orderid;
                        if (data.msg[i].agentid==0) {
                            t_body += "<a href='/ui/pmui/order/orderlist.aspx?kkey=" + data.msg[i].orderid + "'>改期</a>";
                        }
                        else {
                            t_body += "<a href='/ui/pmui/agentsalescode.aspx?kkey=" + data.msg[i].orderid + "'>改期</a>";
                        } 
                        t_body += "</td><td>" + data.msg[i].name + "</td><td>" + data.msg[i].contactphone + "</td><td>" + data.msg[i].IdCard + "</td><td>" + data.msg[i].Nation + "</td><td>" + data.msg[i].yuding_name + "</td><td>1</td><td>" + jsonDateFormatKaler(data.msg[i].yuding_time) + "</td><td>" + data.msg[i].pickuppoint + "</td><td>" + data.msg[i].dropoffpoint + "</td><td>" + data.msg[i].agentname + "</td><td>" + data.msg[i].contactremark + "</td>   </tr>";
                    }
                    $("#tbody_orderlist").html(t_head + t_body);
                }
            })
        }
        //查看截团名单(iscloseteam:1已经截团；0没有截团)
        function chakankehu_closeteam(daydate, proid, iscloseteam, closeteamnum) {

            if (iscloseteam == 0) {
                alert("产品还尚未截团,截团名单为空");
                return;
            }
            if (closeteamnum == 0) {
                alert("截团名单为空");
                return;
            }


            $.post("/JsonFactory/OrderHandler.ashx?oper=travelbusordertravalerdetail", { gooutdate: daydate, proid: proid, order_state: $("#hid_orderstate_paysuc").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#proqrcode_rhshow").show();
                    var t_head = "<tr><td>订单号</td><td>乘车人姓名</td><td>乘车人电话</td><td>乘车人身份证</td><td>乘车人民族</td><td>预订人</td><td>预订人数</td><td>预订时间</td> <td>上车地点</td><td>下车地点</td><td>购票地点</td><td>备注</td> </tr>";
                    var t_body = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        t_body += "<tr><td>" + data.msg[i].orderid + "<a href='/ui/pmui/order/orderlist.aspx?kkey=" + data.msg[i].orderid + "'>改期</a></td><td>" + data.msg[i].name + "</td><td>" + data.msg[i].contactphone + "</td><td>" + data.msg[i].IdCard + "</td><td>" + data.msg[i].Nation + "</td><td>" + data.msg[i].yuding_name + "</td><td>1</td><td>" + jsonDateFormatKaler(data.msg[i].yuding_time) + "</td> <td>" + data.msg[i].pickuppoint + "</td><td>" + data.msg[i].dropoffpoint + "</td><td>" + data.msg[i].agentname + "</td><td>" + data.msg[i].contactremark + "</td></tr>";
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
    <div id="proqrcode_rhshow2" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left: 10%; position: absolute; top: 20%;">
        <table width="800px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr>
                <td align="center" colspan="5">
                    <span style="font-size: 14px;">车辆详情</span>
                </td>
            </tr>
            <tbody id="tbodyy1">
            </tbody>
            <tr>
                <td colspan="5" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel_rh" id="closebtn2" type="button" class="formButton" value="  关 闭  " />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function clickbusdetail(busid) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=gettravelbus", { busid: busid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {

                    $("#proqrcode_rhshow2").show();
                    var t_head = "<tr><td>车型号</td><td>座位数</td><td>车牌号</td><td>司机</td><td>司机电话</td></tr>";
                    var t_body = "<tr><td>" + data.msg.travelbus_model + "</td><td>" + data.msg.seatnum + "</td><td>" + data.msg.licenceplate + "</td><td>" + data.msg.drivername + "</td><td>" + data.msg.driverphone + "</td></tr>";

                    $("#tbodyy1").html(t_head + t_body);
                }
            })


        }
        $(function () {
            $("#closebtn2").click(function () {
                $("#proqrcode_rhshow2").hide();
            })
        })
    </script>
    <div class="data">
        <input type="hidden" id="hid_servertype" value="<%=servertype %>" />
        <input type="hidden" id="hid_orderstate_paysuc" value="<%=orderstate_paysuc %>" />
        <input type="hidden" id="hid_paystate_haspay" value="<%=paystate_haspay %>" />
    </div>
</asp:Content>
