<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HasVisitDateOrderCount.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.HasVisitDateOrderCount" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //日历
            var startdate = '<%=this.nowdate %>';
            var enddate = '<%=this.nextdate %>';

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {

                $("#startdate").val(startdate);
                $($(this)).datepicker();
            });

            var comid = $("#hid_comid").trimVal();
            SearchList(startdate, enddate, 1, comid);



            $("#chaxun").click(function () {
                var startdate = $("#startdate").trimVal();
                var enddate = $("#startdate").trimVal();
                var comid = $("#hid_comid").trimVal();
                var servertype = 1;

                SearchList(startdate, enddate, servertype, comid);
            })

            $("#closebtn").click(function () {
                $("#proqrcode_rhshow").hide();
            })
        })
        function chuli(obj, orderid) {
            if (confirm("确认处理吗?")) {
                $.post("/JsonFactory/OrderHandler.ashx?oper=HasFinOrder", { orderid: orderid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        $(obj).text('已经处理');
                    }
                });
            }
        }

        function viewdetail(index, daydate) {
            if ($("#tr_hid_" + index).trimVal() == 0) {
                $("#tr_hid_" + index).val(1);
                $("#btn_" + index).val("关闭");

                $.post("/jsonfactory/orderhandler.ashx?oper=GetNeedvisitdateProByTraveldate", { daydate: daydate, comid: $("#hid_comid").trimVal(), servertype: "1" }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1)
                    { }
                    if (data.type == 100) {
                        var tbodystr = '<tbody id="tbody_' + index + '" style="display: ; background: #DDDDDD;">';
                        for (var i = 0; i < data.msg.length; i++) {
                            tbodystr += '<tr>' +
                                     '<td>' +
                                         '<p align="left">' +
                                            data.msg[i].Proname + '(支付成功人数:' + data.msg[i].paysucbooknum + '人)';
                            tbodystr += '</p>' +
                                     '</td>' +
                                     '<td>';

                            tbodystr += '<input type="button" id="Btndown_' + i + '" onclick="chakankehu_paysuc(\'' + daydate + '\',' + data.msg[i].Proid + ',' + data.msg[i].paysucbooknum + ')" value="查看支付成功名单" />';

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
            }
            else {
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

            if (parseInt(day) > 7) {
                alert("查询区间不可大于1星期");
                return;
            }

            //购买人需要提交使用日期 的产品的销售统计
            $.post("/jsonfactory/orderhandler.ashx?oper=needvisitdateordercountbyday", { startdate: startdate, enddate: enddate, servertype: servertype, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#tb").html("");
                    var str1 = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        str1 += '<tr id="tr_' + i + '">' +
                                    '<td>' +
                                        '<p align="left">' +
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
        //查看支付成功订单列表
        function chakankehu_paysuc(daydate, proid, paysucbooknum) {
            if (paysucbooknum == 0) {
                alert("支付成功订单为空");
                return;
            }
            $.post("/JsonFactory/OrderHandler.ashx?oper=getNeedvisitdatePaysucorderlist", { gooutdate: daydate, proid: proid, paystate: 2, orderstate: '4,8,22' }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#proqrcode_rhshow").show();
                    var t_head = '<tr><td>订单号</td><td>产品名称</td><td>预订姓名</td><td>预订人数</td><td>预订手机</td><td>出游日期</td><td>提单时间</td><td>备注</td></tr>';
                    var t_body = '';
                    for (var i = 0; i < data.msg.length; i++) {
                        t_body += '<tr><td>' + data.msg[i].Id;
                        if (data.msg[i].Order_state == '22') {
                            t_body += '<a href="javascript:void(0)" )">已经处理</a>';
                        }
                        else {
                            t_body += '<a href="javascript:void(0)" onclick="chuli(this,' + data.msg[i].Id + ')">处理</a>';
                        }

                        t_body += '</td><td>' + data.msg[i].Pro_name + '(' + data.msg[i].Pro_id + ')</td><td>' + data.msg[i].U_name + '</td><td>' + data.msg[i].U_num + '</td><td>' + data.msg[i].U_phone + '</td><td>' + jsonDateFormatKaler(data.msg[i].U_traveldate) + '</td><td>' + jsonDateFormatKaler(data.msg[i].U_subdate) + '</td><td>' + data.msg[i].Order_remark + '</td></tr>';
                    }
                    $("#tbody_orderlist").html(t_head + t_body);
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
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    订单统计</h3>
                <div>
                </div>
                <p>
                    自定义查询：从
                    <input type="text" maxlength="100" size="15" id="startdate" isdate="yes">
                    <input type="button" value="查询" method="post" id="chaxun">
                </p>
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
                    <span style="font-size: 14px;">订单列表</span>
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
    <div class="data">
    </div>
</asp:Content>
