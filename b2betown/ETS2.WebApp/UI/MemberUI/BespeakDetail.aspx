<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BespeakDetail.aspx.cs"    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.MemberUI.BespeakDetail" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            if ($("#hid_closeteamnum").trimVal() == 0) {
                $("#down_namelist").attr("disabled", "disabled");
                $("#Button1").attr("disabled", "disabled");
            }

            $("#hid_busid").val("");
            $("#hid_travelbus_model").val("");
            $("#hid_seatnum").val("");
            $("#hid_licenceplate").val("");
            $("#hid_drivername").val("");
            $("#hid_driverphone").val("");

            //查看客户订单
            $("#Button1").click(function () {
                if ($("#hid1").trimVal() == 0) {
                    $("#tblist").show();
                    $("#hid1").val(1);
                    $("#Button1").val("关闭客户名单");

                    $.post("/JsonFactory/OrderHandler.ashx?oper=travelbusordertravalerdetail", { gooutdate: $("#hid_gooutdate").trimVal(), proid: $("#hid_proid").trimVal(), order_state: $("#hid_orderstatus_hasfin").trimVal() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            var t_head = "<tr><td>订单号</td><td>乘车人姓名</td><td>乘车人身份证</td><td>乘车人民族</td><td>预订人</td><td>预订人数</td><td>预订时间</td><td>上车地点</td><td>下车地点</td><td>购票地点</td><td>备注</td> </tr>";
                            var t_body = "";
                            for (var i = 0; i < data.msg.length; i++) {
                                t_body += "<tr><td>" + data.msg[i].orderid + "</td><td>" + data.msg[i].name + "</td><td>" + data.msg[i].IdCard + "</td><td>" + data.msg[i].Nation + "</td><td>" + data.msg[i].yuding_name + "</td><td>1</td><td>" + jsonDateFormatKaler(data.msg[i].yuding_time) + "</td><td>" + data.msg[i].pickuppoint + "</td><td>" + data.msg[i].dropoffpoint + "</td><td>" + data.msg[i].agentname + "</td><td>" + data.msg[i].orderremark + "</td>  </tr>";
                            }
                            $("#tblist").html(t_head + t_body);
                        }
                    })

                } else {
                    $("#tblist").hide();
                    $("#hid1").val(0);
                    $("#Button1").val("查看客户名单");
                    $("#tblist").html("");

                }
            })

            //获取派车情况
            $("#div_sendbus").html("");
            var operlogid = $("#hid_operlogid").trimVal();

            //得到原来的派车情况
            $.post("/JsonFactory/OrderHandler.ashx?oper=Gettravelbusorder_sendbusBylogid", { logid: operlogid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //增加操作类型
                    var issavebuss = $("#hid_issavebus").val() + "0,";
                    $("#hid_issavebus").val(issavebuss);
                    //增加车辆id
                    var busids = $("#hid_busid").val() + "0,";
                    $("#hid_busid").val(busids);
                    //增加车辆总数
                    if ($("#hid_bustotal").val() == "") {
                        $("#hid_bustotal").val("0");
                    }
                    var bustotal = parseInt($("#hid_bustotal").val()) + 1;
                    $("#hid_bustotal").val(bustotal);
                    //增加车型，座位数，车票号码，司机，司机号码
                    var travelbus_model = $("#hid_travelbus_model").val() + ",";
                    $("#hid_travelbus_model").val(travelbus_model);
                    var seatnum = $("#hid_seatnum").val() + "0,";
                    $("#hid_seatnum").val(seatnum);
                    var licenceplate = $("#hid_licenceplate").val() + ",";
                    $("#hid_licenceplate").val(licenceplate);
                    var drivername = $("#hid_drivername").val() + ",";
                    $("#hid_drivername").val(drivername);
                    var driverphone = $("#hid_driverphone").val() + ",";
                    $("#hid_driverphone").val(driverphone);

                    var d = '<ul id="ul_0">' +
                        '<li id="li_0">' +
                            '<div class="content-list-app">' +
                                '<b class="content-list-add"><input type="button" id="btnedit_0" value="编辑" style="display:none;" disabled="disabled" onclick="editbus(0)"/></b>' +
                                '<div class="popover_content">' +
                                    '<div class="rich_buddy_hd">详细资料</div>' +
                                    '<div class="rich_buddy_bd buddyRichContent" style="display: block;">' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">车型:</label><label id="lblbusmodel_0"></label>' +
                                            '<input type="text" id="busmodel_0" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">座位数:</label><label id="lblseatnum_0"></label>' +
                                            '<input type="text" id="seatnum_0" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">车牌号码:</label><label id="lbllicenceplate_0"></label>' +
                                            '<input type="text" id="licenceplate_0" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">司机:</label><label id="lbldrivername_0"></label>' +
                                            '<input type="text" id="drivername_0" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">司机电话:</label><label id="lbldriverphone_0"></label>' +
                                            '<input type="text" id="driverphone_0" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<input type="button" id="btnsave_0" onclick="savebus(0)" value="保存" />' +
                                            '<input type="button" id="btndel_0" onclick="delbus(0)" value="删除" />' +
                                        '</div>' +
                                    '</div>' +
                                '</div>' +
                                '<div class="content-list-des text-overflow">' +
                                '</div>' +
                        '</li>' +
                    '</ul>';
                    $("#div_sendbus").append(d);
                }
                if (data.type == 100) {
                    if (data.totle > 0) {

                        var issavebus = "";
                        var busid = "";
                        var travelbus_model = "";
                        var seatnum = "";
                        var licenceplate = "";
                        var drivername = "";
                        var driverphone = "";

                        var d = "";
                        for (var jj = 0; jj < data.totle; jj++) {
                            issavebus += "1,";
                            busid += data.msg[jj].id + ",";
                            travelbus_model += data.msg[jj].travelbus_model + ",";
                            seatnum += data.msg[jj].seatnum + ",";
                            licenceplate += data.msg[jj].licenceplate + ",";
                            drivername += data.msg[jj].drivername + ",";
                            driverphone += data.msg[jj].driverphone + ",";

                            d += '<ul id="ul_' + jj + '">' +
                                            '<li id="li_' + jj + '">' +
                                                '<div class="content-list-app">' +
                                                    '<b class="content-list-add"><input type="button" id="btnedit_' + jj + '" value="编辑"  onclick="editbus(' + jj + ')"/></b>' +
                                                    '<div class="popover_content">' +
                                                        '<div class="rich_buddy_hd">详细资料</div>' +
                                                        '<div class="rich_buddy_bd buddyRichContent" style="display: block;">' +
                                                            '<div class="frm_control_group nickName">' +
                                                                '<label class="bustitle">车型:</label><label id="lblbusmodel_' + jj + '">' + data.msg[jj].travelbus_model + '</label>' +
                                                                '<input type="text" id="busmodel_' + jj + '" value="' + data.msg[jj].travelbus_model + '" style="display:none;" />' +
                                                            '</div>' +
                                                            '<div class="frm_control_group nickName">' +
                                                                '<label class="bustitle">座位数:</label><label id="lblseatnum_' + jj + '">' + data.msg[jj].seatnum + '</label>' +
                                                                '<input type="text" id="seatnum_' + jj + '" value="' + data.msg[jj].seatnum + '"  style="display:none;"/>' +
                                                            '</div>' +
                                                            '<div class="frm_control_group nickName">' +
                                                                '<label class="bustitle">车牌号码:</label><label id="lbllicenceplate_' + jj + '">' + data.msg[jj].licenceplate + '</label>' +
                                                                '<input type="text" id="licenceplate_' + jj + '" value="' + data.msg[jj].licenceplate + '" style="display:none;"/>' +
                                                            '</div>' +
                                                            '<div class="frm_control_group nickName">' +
                                                                '<label class="bustitle">司机:</label><label id="lbldrivername_' + jj + '">' + data.msg[jj].drivername + '</label>' +
                                                                '<input type="text" id="drivername_' + jj + '" value="' + data.msg[jj].drivername + '"  style="display:none;"/>' +
                                                            '</div>' +
                                                            '<div class="frm_control_group nickName">' +
                                                                '<label class="bustitle">司机电话:</label><label id="lbldriverphone_' + jj + '">' + data.msg[jj].driverphone + '</label>' +
                                                                '<input type="text" id="driverphone_' + jj + '" value="' + data.msg[jj].driverphone + '" style="display:none;"/>' +
                                                            '</div>' +
                                                            '<div class="frm_control_group nickName">' +
                                                                '<input type="button" id="btnsave_' + jj + '" onclick="savebus(' + jj + ')" value="保存" style="display:none;" disabled="disabled" />' +
                                                                '<input type="button" id="btndel_' + jj + '" onclick="delbus(' + jj + ')" value="删除"  style="display:none;" disabled="disabled"/>' +
                                                            '</div>' +
                                                        '</div>' +
                                                    '</div>' +
                                                    '<div class="content-list-des text-overflow">' +
                                                    '</div>' +
                                            '</li>' +
                                        '</ul>';
                        }
                        $("#div_sendbus").html(d);

                        $("#hid_bustotal").val(data.totle);
                        $("#hid_issavebus").val(issavebus);
                        $("#hid_busid").val(busid);
                        $("#hid_travelbus_model").val(travelbus_model);
                        $("#hid_seatnum").val(seatnum);
                        $("#hid_licenceplate").val(licenceplate);
                        $("#hid_drivername").val(drivername);
                        $("#hid_driverphone").val(driverphone);

                    }
                }
            })


            //增加派车
            $("#Button5").click(function () {
                //判断以前的派车是否都已经保存了
                var arr_issavebus = $("#hid_issavebus").val().split(',');
                var iscanaddbus = 1; //是否可以派车:1可以;0不可以
                for (var ii = 0; ii < arr_issavebus.length; ii++) {
                    if (arr_issavebus[ii] != "") {
                        if (arr_issavebus[ii] == 0) {//如果有没有保存的车量信息，则不可以在派车
                            iscanaddbus = 0;
                        }
                    }
                }
                if (iscanaddbus == 0) {
                    alert("请先把车辆信息保存");
                    return;
                }
                //增加操作类型
                var issavebuss = $("#hid_issavebus").val() + "0,";
                $("#hid_issavebus").val(issavebuss);
                //增加车辆id
                var busids = $("#hid_busid").val() + "0,";
                $("#hid_busid").val(busids);
                //增加车辆总数
                if ($("#hid_bustotal").val() == "") {
                    $("#hid_bustotal").val("0");
                }
                var bustotal = parseInt($("#hid_bustotal").val()) + 1;
                $("#hid_bustotal").val(bustotal);
                //增加车型，座位数，车票号码，司机，司机号码
                var travelbus_model = $("#hid_travelbus_model").val() + ",";
                $("#hid_travelbus_model").val(travelbus_model);
                var seatnum = $("#hid_seatnum").val() + "0,";
                $("#hid_seatnum").val(seatnum);
                var licenceplate = $("#hid_licenceplate").val() + ",";
                $("#hid_licenceplate").val(licenceplate);
                var drivername = $("#hid_drivername").val() + ",";
                $("#hid_drivername").val(drivername);
                var driverphone = $("#hid_driverphone").val() + ",";
                $("#hid_driverphone").val(driverphone);


                var busindex = parseInt(bustotal) - 1;
                var d = '<ul id="ul_' + busindex + '">' +
                        '<li id="li_' + busindex + '">' +
                            '<div class="content-list-app">' +
                                '<b class="content-list-add"><input type="button" id="btnedit_' + busindex + '" value="编辑" style="display:none;" disabled="disabled" onclick="editbus(' + busindex + ')"/></b>' +
                                '<div class="popover_content">' +
                                    '<div class="rich_buddy_hd">详细资料</div>' +
                                    '<div class="rich_buddy_bd buddyRichContent" style="display: block;">' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">车型:</label><label id="lblbusmodel_' + busindex + '"></label>' +
                                            '<input type="text" id="busmodel_' + busindex + '" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">座位数:</label><label id="lblseatnum_' + busindex + '"></label>' +
                                            '<input type="text" id="seatnum_' + busindex + '" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">车牌号码:</label><label id="lbllicenceplate_' + busindex + '"></label>' +
                                            '<input type="text" id="licenceplate_' + busindex + '" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">司机:</label><label id="lbldrivername_' + busindex + '"></label>' +
                                            '<input type="text" id="drivername_' + busindex + '" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<label class="bustitle">司机电话:</label><label id="lbldriverphone_' + busindex + '"></label>' +
                                            '<input type="text" id="driverphone_' + busindex + '" value="" />' +
                                        '</div>' +
                                        '<div class="frm_control_group nickName">' +
                                            '<input type="button" id="btnsave_' + busindex + '" onclick="savebus(' + busindex + ')" value="保存" />' +
                                            '<input type="button" id="btndel_' + busindex + '"  onclick="delbus(' + busindex + ')"  value="删除" />' +
                                        '</div>' +
                                    '</div>' +
                                '</div>' +
                                '<div class="content-list-des text-overflow">' +
                                '</div>' +
                        '</li>' +
                    '</ul>';
                $("#div_sendbus").append(d);
            })

            //提交派车情况
            $("#Button2").click(function () {
                var paysucnum = $("#hid_paysucnum").trimVal();
                if (paysucnum == 0) {
                    alert("预订人数为0，不用处理");
                    return;
                }


                //判断以前的派车是否都已经保存了
                var arr_issavebus = $("#hid_issavebus").val().split(',');
                var iscanaddbus = 1; //是否可以派车:1可以;0不可以
                for (var ii = 0; ii < arr_issavebus.length; ii++) {
                    if (arr_issavebus[ii] != "") {
                        if (arr_issavebus[ii] == 0) {//如果有没有保存的车量信息，则不可以在派车
                            iscanaddbus = 0;
                        }
                    }
                }
                if (iscanaddbus == 0) {
                    alert("请先把车辆信息保存");
                    return;
                }

                if (parseInt($("#hid_bustotal").trimVal()) > 0) {
                    //判断车辆座位数是否为数字
                    var arr_seatnum = $("#hid_seatnum").val().split(',');
                    var seat_isnum = 1; //车辆座位数是否为整数
                    for (var ii = 0; ii < arr_seatnum.length; ii++) {
                        if (arr_seatnum[ii] != "") {
                            if (isNaN(arr_seatnum[ii])) {
                                seat_isnum = 0;
                            }
                        }
                    }
                    if (seat_isnum == 0) {
                        alert("车辆座位数必须为数字");
                        return;
                    }
                }



                //判断处理详情是否为空
                var remark = $("#txtoperremark").trimVal();
                if (remark == "") {
                    alert("请把处理情况简单记录到处理详情中..");
                    return;
                }

                var operlogid = $("#hid_operlogid").trimVal();
                var proid = $("#hid_proid").trimVal();
                var proname = $("#hid_proname").trimVal().replace(/\'/g, "").replace(/\"/g, "");
                var gooutdate = $("#hid_gooutdate").trimVal();
                var operremark = $("#txtoperremark").trimVal().replace(/\'/g, "").replace(/\"/g, "");
                var bustotal = $("#hid_bustotal").trimVal();
                var busids = $("#hid_busid").trimVal();
                var travelbus_model = $("#hid_travelbus_model").trimVal();
                var seatnum = $("#hid_seatnum").trimVal();
                var licenceplate = $("#hid_licenceplate").trimVal();
                var drivername = $("#hid_drivername").trimVal();
                var driverphone = $("#hid_driverphone").trimVal();
                var issavebus = $("#hid_issavebus").trimVal();


                $.post("/JsonFactory/OrderHandler.ashx?oper=edittravelbusorder_operlog", { userid: $("#hid_userid").trimVal(), comid: $("#hid_comid").trimVal(), operlogid: operlogid, proid: proid, proname: proname, gooutdate: gooutdate, operremark: operremark, bustotal: bustotal, busids: busids, travelbus_model: travelbus_model, seatnum: seatnum, licenceplate: licenceplate, drivername: drivername, driverphone: driverphone, issavebus: issavebus }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        window.open("TravelbusOrderCount.aspx?gooutdate=" + $("#hid_gooutdate").trimVal(), target = "_self");
                    }
                })

            })
        })

        //编辑 派车信息
        function editbus(busindex) {
            //显示效果调整
            $("#btnedit_" + busindex).hide().attr("disabled", "disabled");
            $("#btnsave_" + busindex).show().removeAttr("disabled");
            $("#btndel_" + busindex).show().removeAttr("disabled");
            $("#busmodel_" + busindex).show();
            $("#lblbusmodel_" + busindex).hide().html($("#busmodel_" + busindex).val());
            $("#seatnum_" + busindex).show();
            $("#lblseatnum_" + busindex).hide().html($("#seatnum_" + busindex).val());
            $("#licenceplate_" + busindex).show();
            $("#lbllicenceplate_" + busindex).hide().html($("#licenceplate_" + busindex).val());
            $("#drivername_" + busindex).show();
            $("#lbldrivername_" + busindex).hide().html($("#drivername_" + busindex).val());

            $("#driverphone_" + busindex).show();
            $("#lbldriverphone_" + busindex).hide().html($("#driverphone_" + busindex).val());

            //操作类型:编辑操作
            var issavebusstr = $("#hid_issavebus").val();
            var arr_issavebus = issavebusstr.substr(0, issavebusstr.length - 1).split(',');
            arr_issavebus[busindex] = 0;
            var nowissavebus = "";
            for (var ii = 0; ii < arr_issavebus.length; ii++) {
                nowissavebus += arr_issavebus[ii] + ",";
            }
            $("#hid_issavebus").val(nowissavebus);
        }
        //保存 派车信息
        function savebus(busindex) {
            var busmodel = $("#busmodel_" + busindex).trimVal();
            var seatnum = $("#seatnum_" + busindex).trimVal();
            var licenceplate = $("#licenceplate_" + busindex).trimVal();
            var drivername = $("#drivername_" + busindex).trimVal();
            var driverphone = $("#driverphone_" + busindex).trimVal();


            if (busmodel == "") {
                alert("车型不可为空");
                return;
            }
            if (seatnum == "") {
                alert("座位数不可为空");
                return;
            }
            if (licenceplate == "") {
                alert("车牌号码不可为空");
                return;
            }
            if (drivername == "") {
                alert("司机姓名不可为空");
                return;
            }
            if (driverphone == "") {
                alert("司机电话不可为空");
                return;
            }

            //车型
            var travelbus_modelstr = $("#hid_travelbus_model").val();
            var arr_busmodel = travelbus_modelstr.substr(0, travelbus_modelstr.length - 1).split(',');
            arr_busmodel[busindex] = busmodel;
            var nowbusmodel = "";
            for (var ii = 0; ii < arr_busmodel.length; ii++) {
                nowbusmodel += arr_busmodel[ii] + ",";
            }
            $("#hid_travelbus_model").val(nowbusmodel);
            //座位数
            var seatnumstr = $("#hid_seatnum").val();
            var arr_seatnum = seatnumstr.substr(0, seatnumstr.length - 1).split(',');
            arr_seatnum[busindex] = seatnum;
            var nowseatnum = "";
            for (var ii = 0; ii < arr_seatnum.length; ii++) {
                nowseatnum += arr_seatnum[ii] + ",";
            }
            $("#hid_seatnum").val(nowseatnum);
            //车牌号
            var licenceplatestr = $("#hid_licenceplate").val();
            var arr_licenceplate = licenceplatestr.substr(0, licenceplatestr.length - 1).split(',');
            arr_licenceplate[busindex] = licenceplate;
            var nowlicenceplate = "";
            for (var ii = 0; ii < arr_licenceplate.length; ii++) {
                nowlicenceplate += arr_licenceplate[ii] + ",";
            }
            $("#hid_licenceplate").val(nowlicenceplate);
            //司机
            var drivernamestr = $("#hid_drivername").val();
            var arr_drivername = drivernamestr.substr(0, drivernamestr.length - 1).split(',');
            arr_drivername[busindex] = drivername;
            var nowdrivername = "";
            for (var ii = 0; ii < arr_drivername.length; ii++) {
                nowdrivername += arr_drivername[ii] + ",";
            }
            $("#hid_drivername").val(nowdrivername);
            //司机电话
            var driverphonestr = $("#hid_driverphone").val();
            var arr_driverphone = driverphonestr.substr(0, driverphonestr.length - 1).split(',');
            arr_driverphone[busindex] = driverphone;
            var nowdriverphone = "";
            for (var ii = 0; ii < arr_driverphone.length; ii++) {
                nowdriverphone += arr_driverphone[ii] + ",";
            }
            $("#hid_driverphone").val(nowdriverphone);





            //操作类型:保存操作
            var issavebusstr = $("#hid_issavebus").val();
            var arr_issavebus = issavebusstr.substr(0, issavebusstr.length - 1).split(',');
            arr_issavebus[busindex] = 1;
            var nowissavebus = "";
            for (var ii = 0; ii < arr_issavebus.length; ii++) {
                nowissavebus += arr_issavebus[ii] + ",";
            }
            $("#hid_issavebus").val(nowissavebus);


            //显示效果调整
            $("#btnedit_" + busindex).show().removeAttr("disabled");
            $("#btnsave_" + busindex).hide().attr("disabled", "disabled");
            $("#btndel_" + busindex).hide().attr("disabled", "disabled");
            $("#lblbusmodel_" + busindex).show().html($("#busmodel_" + busindex).val());
            $("#busmodel_" + busindex).hide();
            $("#lblseatnum_" + busindex).show().html($("#seatnum_" + busindex).val());
            $("#seatnum_" + busindex).hide();
            $("#lbllicenceplate_" + busindex).show().html($("#licenceplate_" + busindex).val());
            $("#licenceplate_" + busindex).hide();
            $("#lbldrivername_" + busindex).show().html($("#drivername_" + busindex).val());
            $("#drivername_" + busindex).hide();
            $("#lbldriverphone_" + busindex).show().html($("#driverphone_" + busindex).val());
            $("#driverphone_" + busindex).hide();
        }

        //删除派车信息
        function delbus(busindex) {
            if (confirm("确认删除此车吗?")) {
                var div_sendbushtml = $("#div_sendbus").html();
                var del_ulhtml = $("#ul_" + busindex).html();
                div_sendbushtml = div_sendbushtml.replace(del_ulhtml, "");
                $("#div_sendbus").html(div_sendbushtml);

                //操作类型:删除操作
                var issavebusstr = $("#hid_issavebus").val();
                var arr_issavebus = issavebusstr.substr(0, issavebusstr.length - 1).split(',');
                arr_issavebus[busindex] = 2;
                var nowissavebus = "";
                for (var ii = 0; ii < arr_issavebus.length; ii++) {
                    nowissavebus += arr_issavebus[ii] + ",";
                }
                $("#hid_issavebus").val(nowissavebus);
            }
        }
     
    </script>
    <style type="text/css">
        .content-list-app .content-list-add
        {
            width: 38px;
        }
        .content-list li
        {
            width: 242px;
            height: 208px;
        }
        .frm_control_group
        {
            padding:2px;
            }
        .bustitle
        {
            width: 60px;
            float:left;
            display:block;
         }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
      <%--  <div id="secondary-tabs" class="navsetting ">
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
                    旅游大巴订单详情</h3>
                <table border="0">
                    <tr>
                        <td colspan="10">
                            <p align="left">
                                <%=proname %>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <p align="left">
                                <%=gooutdate%>(支付成功人数:<%=paysucnum%>人;截团人数:<%=closeteamnum %>人)
                            </p>
                        </td>
                        <td colspan="3">
                            <input type="button" id="Button1" value="查看客户名单" />
                            <a style="color: Blue; text-decoration: underline;" href="/Excel/DownExcel.aspx?oper=outlvyoubusorderlistbyorderstate&proid=<%=proid %>&gooutdate=<%=gooutdate %>&orderstate=<%=orderstatus_hasfin %>"
                                id="down_namelist">下载客户名单</a>
                            <input type="hidden" id="hid1" value="0" />
                        </td>
                    </tr>
                    <tbody id="tblist" style="display: none; background: #DDDDDD;">
                    </tbody>
                </table>
                <h3>
                    派车情况
                    <input type="button" id="Button5" value="加车" />
                </h3>
                <div class="fn-clear content-list" id="div_sendbus">
                </div>
                <table>
                    <tr>
                        <td>
                            处理详情
                        </td>
                        <td colspan="4">
                            <textarea id="txtoperremark" cols="100" rows="3"><%=operremark %></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <input type="button" id="Button2" value="提 交" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
        <input type="hidden" id="hid_gooutdate" value="<%=gooutdate %>" />
        <input type="hidden" id="hid_proid" value="<%=proid %>" />
        <input type="hidden" id="hid_proname" value="<%=proname %>" />
        <input type="hidden" id="hid_orderstatus_hasfin" value="<%=orderstatus_hasfin %>" />
        <input type="hidden" id="hid_paysucnum" value="<%=paysucnum %>" />
        <input type="hidden" id="hid_closeteamnum" value="<%=closeteamnum %>" />
        <!-操作日志情况-!>
        <input type="hidden" id="hid_operlogid" value="<%=operlogid %>" />
        <input type="hidden" id="hid_operremark" value="<%=operremark %>" />
        <!-派车总数量-!>
        <input type="hidden" id="hid_bustotal" value="" />
        <!-操作类型:0编辑车辆操作；1保存车辆操作；2删除车辆操作-!>
        <input type="hidden" id="hid_issavebus" value="" />
        <!-派车车辆id集合-!>
        <input type="hidden" id="hid_busid" value="" />
        <!-派车详情-!>
        <input type="hidden" id="hid_travelbus_model" value="" />
        <input type="hidden" id="hid_seatnum" value="" />
        <input type="hidden" id="hid_licenceplate" value="" />
        <input type="hidden" id="hid_drivername" value="" />
        <input type="hidden" id="hid_driverphone" value="" />
    </div>
</asp:Content>
