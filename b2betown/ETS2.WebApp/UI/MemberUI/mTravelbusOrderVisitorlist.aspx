<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mTravelbusOrderVisitorlist.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.mTravelbusOrderVisitorlist" MasterPageFile="/UI/pmui/ETicket/mEtown.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8">
    <meta name="keywords" content="微商城">
    <meta name="description" content="">
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">
    <title></title>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!-- CSS -->
    <link rel="stylesheet" href="/agent/m/css/cart.css">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/morder.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#h_comname").text($("#hid_comname").trimVal());

            //            $("#search_botton").click(function () {
            //                SearchList(1);
            //            })

            var proid = '<%=this.proid %>';
            var daydate = '<%=this.daydate %>';
            var oper = '<%=this.oper %>';

            SearchList(proid, daydate, oper);

            //关闭提单页面
            $(".js-cancel").click(function () {
                $("#suborder").hide();
            })
            //临时提单
            $("#confirmsuborder").click(function () {
                $("#confirmsuborder").attr("disabled", "disabled");

                var fangzhichongfutijiao = $("#fangzhichongfutijiao").val();
                $("#fangzhichongfutijiao").val("1");

                var u_name = $("#u_name").trimVal();
                if (u_name == "") {
                    alert("预约人姓名不可为空");
                    $("#confirmsuborder").removeAttr("disabled");
                    $("#fangzhichongfutijiao").val("0");

                    return;
                }
                var u_phone = $("#u_phone").trimVal();
                if (u_phone == "") {
                    alert("预约人电话不可为空");
                    $("#confirmsuborder").removeAttr("disabled");
                    $("#fangzhichongfutijiao").val("0");

                    return;
                }
                else {
                    if (u_phone.length != 11) {
                        alert("预约人手机位数不正确");
                        $("#confirmsuborder").removeAttr("disabled");
                        $("#fangzhichongfutijiao").val("0");

                        return;
                    }
                }
                var u_num = $("#u_num").trimVal();

                var travelnames = ""; //乘车人姓名列表
                var travelidcards = ""; //乘车人身份证列表
                var travelnations = ""; //乘车人民族列表
                var travelphones = ""; //乘车人联系电话列表
                var travelremarks = ""; //乘车人备注列表

                var errid = "0"; // 输入格式错误的控件id 

                //判断乘车人信息不可为空
                for (var i = 1; i <= parseInt(u_num); i++) {
                    var travel_name = $("#u_name").trimVal();
                    var travel_idcard = "";
                    var travel_nation = "汉族";
                    var travel_phone = $("#u_phone").trimVal();
                    var travel_remark = "";


                    if (travel_name == "") {
                        alert("乘车人姓名不可为空");
                        errid = "travelname" + i;
                        $("#confirmsuborder").removeAttr("disabled");
                        $("#fangzhichongfutijiao").val("0");

                        break;
                    }
                    if (travel_phone == "") {
                        alert("乘车人" + travel_name + "电话不可为空");
                        errid = "travelname" + i;
                        $("#confirmsuborder").removeAttr("disabled");
                        $("#fangzhichongfutijiao").val("0");

                        break;
                    }
                    //                        if (travel_idcard == "") {
                    //                            alert("乘车人" + travel_name + "身份证号不可为空");
                    //                            errid = "travelidcard" + i;
                    //                            break;

                    //                        } else {
                    //                            if (!IdCardValidate(travel_idcard)) {
                    //                                alert("乘车人" + travel_name + "身份证格式错误");
                    //                                errid = "travelidcard" + i;
                    //                                break;
                    //                            }
                    //                        }
                    //                        if (travel_nation == "") {
                    //                            alert("乘车人" + travel_name + "民族不可为空");
                    //                            errid = "travelnation" + i;
                    //                            break;
                    //                        }
                    if (travel_idcard != "") {
                        if (!IdCardValidate(travel_idcard)) {
                            alert("乘车人" + travel_name + "身份证格式错误");
                            errid = "travelidcard" + i;
                            $("#confirmsuborder").removeAttr("disabled");
                            $("#fangzhichongfutijiao").val("0");

                            break;
                        }
                    }
                    travelnames += travel_name + ",";
                    travelidcards += travel_idcard + ",";
                    travelnations += travel_nation + ",";
                    travelphones += travel_phone + ",";
                    travelremarks += travel_remark + ",";

                }
                if (errid != "0") {
                    $(errid).focus();
                    alert("乘车人信息有误");
                    $("#confirmsuborder").removeAttr("disabled");
                    $("#fangzhichongfutijiao").val("0");

                    return;
                }

                if ($("#txt_payee").trimVal() == "") {
                    alert("请输入收款人姓名");
                    $("#fangzhichongfutijiao").val("0");

                    return;
                }
                if (fangzhichongfutijiao == "1") {
                    alert("已经提交订单，请稍后...，如果长时间没反应请先查询订单，确认未提交成功请刷新后重提。");

                    return;
                }
                
                //提交预订
                                $.post("/JsonFactory/OrderHandler.ashx?oper=createorder", { proid: proid, ordertype: 1, payprice: $("#hid_adviseprice").trimVal(), u_num: u_num, u_name: u_name, u_phone: u_phone, u_traveldate: daydate, comid: '<%=comid%>', buyuid: 0, tocomid: 0, travelnames: travelnames, travelidcards: travelidcards, travelnations: travelnations, travelphones: travelphones, travelremarks: travelremarks, travel_pickuppoints: $("#pointuppoint").trimVal(), travel_dropoffpoints: $("#dropoffpoint").trimVal(), order_remark: "上车收款,收款人:" + $("#txt_payee").trimVal(), autosuccess: 1, ignoredabatime: 1 }, function (data) {
                                    data = eval("(" + data + ")");
                                    if (data.type == 1) {
                                        alert(data.msg);
                                        $("#confirmsuborder").removeAttr("disabled");
                                        $("#fangzhichongfutijiao").val("0");

                                        return;
                                    }
                                    if (data.type == 100) {
                                        alert("提单成功");
                                        location.reload();
                                        $("#fangzhichongfutijiao").val("0");
                                        return;
                                    }

                                })
            })
            //预约人数变动
            $("#u_num").change(function () {
                var adviseprice = $("#hid_adviseprice").trimVal();
                var num = $("#u_num").trimVal();

                var totalprice = adviseprice * num;
                $("#suborder_price").text(totalprice + '(上车交费)');
            })
        })
        //上车标识
        function onaboardtag(orderbusrideid, orderid, idcard, name, traveltime, obj) {
            $(obj).text("标识中..").attr("disabled", "disabled");

            $.post("/JsonFactory/OrderHandler.ashx?oper=travelbusonboardtag", { orderid: orderid, idcard: idcard, name: name, traveltime: traveltime, comid: $("#hid_comid").trimVal(), orderbusrideid: orderbusrideid }, function (data1) {
                data1 = eval("(" + data1 + ")");
                if (data1.type == 1) {
                    alert(data1.msg);
                    return;
                }
                if (data1.type == 100) {
                    $(obj).text("已上车").attr("disabled", "disabled").css("border-color", "gray").css("background-color", "gray");
                    return;
                }
            })
        }
        function SearchList(proid, daydate, oper) {
            if (proid != 0 && daydate != "" && oper != "") {
                //                if (oper == "paysuc") {
                $.post("/JsonFactory/OrderHandler.ashx?oper=travelbustravelerlistBypaystate", { gooutdate: daydate, proid: proid, paystate: $("#hid_paystate_haspay").trimVal(), orderstate: '4,8,22' }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {


                        var t_body = "";
                        t_body += '<div class="layout-box">' +
                                        '<ul class="list-a">';
                        t_body += '<li style="color:#f15a0c;">名称:' + data.proname + '</li>';
                        t_body += '<li style="color:#f15a0c;">日期:' + data.gooutdate + '</li>';
                        t_body += '<li style="color:#f15a0c;">总人数:' + data.totalcount + '人</li>';
                        if (data.groupbydata.length>0) {
                            for (var k = 0; k < data.groupbydata.length; k++) {
                                t_body += '<li style="color:#f15a0c;">' + data.groupbydata[k].pickuppoint + ' (' + data.groupbydata[k].count + '人)</li>';
                            }
                        }


                        if (data.isreminded == 1) {
                            t_body += '<li style="color:#f15a0c;">车牌号:' + data.licenceplate + '<br> 电话:' + data.telphone + '<br><a href="javascript:void(0)"  style="border-color: gray;background-color: gray;"  class="js-buy-it btn btn-orange-dark btn-2-1">已群发完成</a><br><a href="javascript:void(0)" onclick="PreviewRemindSms(\'' + data.licenceplate + '\',\'' + data.telphone + '\')" >提醒短信预览</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="resetRemindSms()">重置群发</a></li>';
                        }
                        else {
                            t_body += '<li style="color:#f15a0c;">车牌号:<input type="text" id="licenceplate" value="' + data.licenceplate + '"><br>  电话:<input type="text" id="telphone" value="' + data.telphone + '"><br><a href="javascript:void(0)" onclick="QunfaRemindSms(this,\'paysuc\')" class="js-buy-it btn btn-orange-dark btn-2-1">群发提醒短信</a><br><a href="javascript:void(0)" onclick="PreviewRemindSms(\'\',\'\')">提醒短信预览</a></li>';
                        }
                        //临时加人
                        if ($("#hid_emptynum").trimVal() > 0) {
                            t_body += '<li style="color:#f15a0c; padding-top:20px;"><a href="javascript:void(0)" onclick="TempAddTraveler(\'' + proid + '\',\'' + data.proname + '\',\'' + daydate + '\')" class="js-buy-it btn btn-orange-dark btn-2-1">临时加人</a></li>';
                        }
                        else {
                            t_body += '<li style="color:#f15a0c; padding-top:20px;"><a href="javascript:void(0)"  class="js-buy-it btn btn-orange-dark btn-2-1" style="border-color: gray;background-color: gray;">车上人已满</a></li>';
                        }

                        if (data.issendtodriver == 1) {
                            t_body += '<li style="color:#f15a0c; padding-top:20px;"><a href="javascript:void(0)" style="border-color: gray;background-color: gray;"   class="js-buy-it btn btn-orange-dark btn-2-1">已经给送车员发送乘客名单</a></li>';
                        }
                        else {
                            t_body += '<li style="color:#f15a0c; padding-top:20px;"><a href="javascript:void(0)" onclick="SendTrvalNamelist(this,\'' + data.telphone + '\')" class="js-buy-it btn btn-orange-dark btn-2-1">给送车员发送乘客名单</a></li>';
                        }
                   


                        t_body += '</ul>' +
                                    '</div>';

                        var phonsarr = new Array();
                        if (data.sendtophones != "") {
                            phonsarr = data.sendtophones.split(',');
                        }
                        if(data.msg.length>0){
                        for (var i = 0; i < data.msg.length; i++) {
                            t_body += '<div class="layout-box">' +
                                        '<ul class="list-a">' +
                                            '<li>' + (i + 1) + '.姓名:' + data.msg[i].name + ' <a href="tel:' + data.msg[i].contactphone + '">' + data.msg[i].contactphone + '</a>';
                            if (data.isreminded == 1) {
                                if (phonsarr.indexOf(data.msg[i].contactphone) > -1) {//已经提醒
                                    t_body += '&nbsp;<a href="javascript:void(0)"  class="js-buy-it btn"  style="color: #FFF;border-color: gray;background-color: gray;" disabled="disabled">已发送短信</a>';
                                }
                                else { //补发提醒短信
                                    t_body += '&nbsp;<a href="javascript:void(0)"  class="js-buy-it btn"  style="color: #FFF;border-color: rgb(28, 189, 241);background-color: rgb(60, 175, 220);" onclick="AgainSendMsg(this,\'paysuc\',\'' + data.msg[i].contactphone + '\',\'' + data.licenceplate + '\',\'' + data.telphone + '\')">补发短信</a>';
                                }
                            }

                            t_body += '</li>' +
                                            '<li>上车:' + data.msg[i].pickuppoint + '</li>' +
                                            '<li>下车:' + data.msg[i].dropoffpoint + ' </li>';
                            if (data.msg[i].orderremark.indexOf("上车收款") > -1) {
                                t_body += '<li>备注:<label style="color:#f15a0c;">' + data.msg[i].orderremark + '</label></li>';
                            }
                            else {
                                t_body += '<li>备注:' + data.msg[i].orderremark + '</li>';
                            }

                            if (data.msg[i].isaboard == 0) {//未上车
                                t_body += '<li><a href="javascript:void(0)" onclick="onaboardtag(' + data.msg[i].id + ',\'' + data.msg[i].orderid + '\',\'' + data.msg[i].IdCard + '\',\'' + data.msg[i].name + '\',\'' + data.msg[i].travel_time + '\',this)" class="js-buy-it btn btn-orange-dark btn-2-1">上车</a></li>';
                            }
                            if (data.msg[i].isaboard == 1) {//已上车
                                t_body += '<li><a href="javascript:void(0)"   class="js-buy-it btn btn-orange-dark btn-2-1" style="border-color: gray;background-color: gray;">已上车</a></li>';
                            }
                            t_body += '</ul>' +
                                    '</div>';
                            //                                t_body += "<tr><td>" + data.msg[i].orderid + "</td><td>" + data.msg[i].name + "</td><td>" + data.msg[i].contactphone + "</td><td>" + data.msg[i].IdCard + "</td><td>" + data.msg[i].Nation + "</td><td>" + data.msg[i].yuding_name + "</td><td>1</td><td>" + jsonDateFormatKaler(data.msg[i].yuding_time) + "</td><td>" + data.msg[i].pickuppoint + "</td><td>" + data.msg[i].dropoffpoint + "</td><td>" + data.msg[i].agentname + "</td><td>" + data.msg[i].contactremark + "</td>   </tr>";
                        }
                        }
                        $("#list").html(t_body);


                    }
                })
                //                }
                //                else if (oper == "jietuansuc") {
                //                $.post("/JsonFactory/OrderHandler.ashx?oper=travelbusordertravalerdetail", { gooutdate: daydate, proid: proid, order_state: $("#hid_orderstate_paysuc").trimVal() }, function (data) {
                //                    data = eval("(" + data + ")");
                //                    if (data.type == 100) {
                //                        var t_body = "";
                //                        t_body += '<div class="layout-box">' +
                //                                                        '<ul class="list-a">';
                //                        t_body += '<li style="color:#f15a0c;">名称:' + data.proname + '</li>';
                //                        t_body += '<li style="color:#f15a0c;">日期:' + data.gooutdate + '</li>';
                //                        t_body += '<li style="color:#f15a0c;">总人数:' + data.totalcount + '人</li>';
                //                        for (var k = 0; k < data.groupbydata.length; k++) {
                //                            t_body += '<li style="color:#f15a0c;">' + data.groupbydata[k].pickuppoint + ' (' + data.groupbydata[k].count + '人)</li>';
                //                        }
                //                        if (data.isreminded == 1) {
                //                            t_body += '<li style="color:#f15a0c;">车牌号:' + data.licenceplate + '<br> 电话:' + data.telphone + '<br><a href="javascript:void(0)"  style="border-color: gray;background-color: gray;"  class="js-buy-it btn btn-orange-dark btn-2-1">已群发完成</a><br><a href="javascript:void(0)" onclick="PreviewRemindSms(\'' + data.licenceplate + '\',\'' + data.telphone + '\')">提醒短信预览</a></li>';
                //                        }
                //                        else {
                //                            t_body += '<li style="color:#f15a0c;">车牌号:<input type="text" id="licenceplate" value="' + data.licenceplate + '"><br>  电话:<input type="text" id="telphone" value="' + data.telphone + '"><br><a href="javascript:void(0)" onclick="QunfaRemindSms(this,\'jietuansuc\')" class="js-buy-it btn btn-orange-dark btn-2-1">群发提醒短信</a><br> <a href="javascript:void(0)" onclick="PreviewRemindSms(\'\',\'\')">提醒短信预览</a></li>';
                //                        }

                //                        t_body += '</ul>' +
                //                                                    '</div>';

                //                        var phonsarr = new Array();
                //                        if (data.sendtophones != "") {
                //                            phonsarr = data.sendtophones.split(',');
                //                        }
                //                        for (var i = 0; i < data.msg.length; i++) {
                //                            t_body += '<div class="layout-box">' +
                //                                                        '<ul class="list-a">' +
                //                                                            '<li>姓名:' + data.msg[i].name + '<a href="tel:' + data.msg[i].contactphone + '">' + data.msg[i].contactphone + '</a>';
                //                            if (data.isreminded == 1) {
                //                                if (phonsarr.indexOf(data.msg[i].contactphone) > -1) {//已经提醒
                //                                    t_body += '&nbsp;<a href="javascript:void(0)"  class="js-buy-it btn"  style="color: #FFF;border-color: gray;background-color: gray;" disabled="disabled">已发送短信</a>';
                //                                }
                //                                else { //补发提醒短信
                //                                    t_body += '&nbsp;<a href="javascript:void(0)"  class="js-buy-it btn" style="color: #FFF;border-color: rgb(28, 189, 241);background-color: rgb(60, 175, 220);"  onclick="AgainSendMsg(this,\'jietuansuc\',\'' + data.msg[i].contactphone + '\')">补发短信</a>';
                //                                }
                //                            }

                //                            t_body += '</li>' +
                //                                                            '<li>身份证: ' + data.msg[i].IdCard + '</li>' +
                //                                                            '<li>上车地点:' + data.msg[i].pickuppoint + '</li>' +
                //                                                            '<li>下车地点:' + data.msg[i].dropoffpoint + ' </li>';
                //                            if (data.msg[i].isaboard == 0) {//未上车
                //                                t_body += '<li><a href="javascript:void(0)" onclick="onaboardtag(' + data.msg[i].id + ',\'' + data.msg[i].orderid + '\',\'' + data.msg[i].IdCard + '\',\'' + data.msg[i].name + '\',\'' + data.msg[i].travel_time + '\',this)" class="js-buy-it btn btn-orange-dark btn-2-1">上车</a></li>';
                //                            }
                //                            if (data.msg[i].isaboard == 1) {//已上车
                //                                t_body += '<li><a href="javascript:void(0)"   class="js-buy-it btn btn-orange-dark btn-2-1" style="border-color: gray;background-color: gray;">已上车</a></li>';
                //                            }

                //                            t_body += '</ul>' +
                //                                                    '</div>';
                //                            //                                t_body += "<tr><td>" + data.msg[i].orderid + "</td><td>" + data.msg[i].name + "</td><td>" + data.msg[i].contactphone + "</td><td>" + data.msg[i].IdCard + "</td><td>" + data.msg[i].Nation + "</td><td>" + data.msg[i].yuding_name + "</td><td>1</td><td>" + jsonDateFormatKaler(data.msg[i].yuding_time) + "</td> <td>" + data.msg[i].pickuppoint + "</td><td>" + data.msg[i].dropoffpoint + "</td><td>" + data.msg[i].agentname + "</td><td>" + data.msg[i].contactremark + "</td></tr>";
                //                        }
                //                        $("#list").html(t_body);

                //                    }
                //                })
                //                }
                //                else {
                //                    alert("传递操作有误");
                //                    return;
                //                }
            }
            else {
                alert("传递参数有误");
                return;
            }
        }

        function goyanzheng() {
            location.href = "/ui/pmui/ETicket/mETicketIndex.aspx";
        }
        function goyanzhenglist() {
            location.href = "/ui/pmui/ETicket/mETicketList.aspx";
        }
        function golvyoudaba() {
            location.href = "/ui/MemberUI/mTravelbusOrderCount.aspx";
        }
        //补发短信
        function AgainSendMsg(obj, kerentype, againphone, licenceplate, telphone) {
            $(obj).text("发送中，请稍等..").attr("disabled", "disabled");

            var proid = '<%=this.proid %>';
            var daydate = '<%=this.daydate %>';
            var orderstate = '';
            if (kerentype == "paysuc") {
                orderstate = '4,8,22';
            }
            else if (kerentype == "jietuansuc") {
                orderstate = '22';
            }
            else {
                alert("客人类型有误");
                return;
            }

            $.post("/JsonFactory/OrderHandler.ashx?oper=travelbusQunfaRemindSms", { gooutdate: daydate, proid: proid, paystate: $("#hid_paystate_haspay").trimVal(), orderstate: orderstate, licenceplate: licenceplate, telphone: telphone, kerentype: kerentype, againphone: againphone }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    location.reload();
                    return;
                }
                if (data.type == 100) {

                    $(obj).text("已发送短信").attr("disabled", "disabled").css("border-color", "gray").css("background-color", "gray");
                    return;
                }
            })
        }
        //群发提醒短信
        function QunfaRemindSms(obj, kerentype) {
            var licenceplate = $("#licenceplate").val();
            if (licenceplate == "") {
                alert("车牌号不可为空");
                return;
            }
            var telphone = $("#telphone").val();
            if (telphone == "") {
                alert("司机电话不可为空");
                return;
            }

            $(obj).text("发送中，请稍等..").attr("disabled", "disabled");

            var proid = '<%=this.proid %>';
            var daydate = '<%=this.daydate %>';
            var orderstate = '';
            if (kerentype == "paysuc") {
                orderstate = '4,8,22';
            }
            else if (kerentype == "jietuansuc") {
                orderstate = '22';
            }
            else {
                alert("客人类型有误");
                return;
            }

            $.post("/JsonFactory/OrderHandler.ashx?oper=travelbusQunfaRemindSms", { gooutdate: daydate, proid: proid, paystate: $("#hid_paystate_haspay").trimVal(), orderstate: orderstate, licenceplate: licenceplate, telphone: telphone, kerentype: kerentype }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    location.reload();
                    return;
                }
                if (data.type == 100) {

                    $(obj).text("已群发完成").attr("disabled", "disabled").css("border-color", "gray").css("background-color", "gray");
                    return;
                }
            })
        }

        //重置群发短信
        function resetRemindSms() {
            var proid = '<%=this.proid %>';
            var daydate = '<%=this.daydate %>'; 
            var orderstate = '4,8,22';
           

            $.post("/JsonFactory/OrderHandler.ashx?oper=travelbusQunfaRemindSmsReset", { gooutdate: daydate, proid: proid, paystate: $("#hid_paystate_haspay").trimVal(), orderstate: orderstate}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    location.reload();
                    return;
                }
                if (data.type == 100) {
                    alert("重置完成");
                    location.reload();
                    return;
                }
            })
        }
        //提醒产品预览
        function PreviewRemindSms(licenceplate, telphone) {


            var proid = '<%=this.proid %>';
            var daydate = '<%=this.daydate %>';
            if (licenceplate == "" && telphone == "") {//发送前预览
                var licenceplate = $("#licenceplate").val();
                if (licenceplate == "") {
                    alert("车牌号不可为空");
                    return;
                }
                var telphone = $("#telphone").val();
                if (telphone == "") {
                    alert("司机电话不可为空");
                    return;
                }


                $.post("/JsonFactory/OrderHandler.ashx?oper=getPreviewRemindSms", { gooutdate: daydate, proid: proid, licenceplate: licenceplate, telphone: telphone }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        location.reload();
                        return;
                    }
                    if (data.type == 100) {
                        alert(data.msg);
                        return;
                    }
                })
            }
            else {//发送后预览
                $.post("/JsonFactory/OrderHandler.ashx?oper=getPreviewRemindSms", { gooutdate: daydate, proid: proid, licenceplate: licenceplate, telphone: telphone }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        location.reload();
                        return;
                    }
                    if (data.type == 100) {
                        alert(data.msg);
                        return;
                    }
                })
            }
        }

        //临时上车层展示
        function TempAddTraveler(proid, proname, daydate) {
            $("#suborder").show();
            $("#suborder_proname").text(proname + "(" + daydate + ")");
            $("#suborder_price").text('<%=advise_price %>(上车交费)');

            //空位数量
            var emptynum=10;
             if ($("#hid_emptynum").trimVal()<10){
                emptynum=$("#hid_emptynum").trimVal();
             }
             var emptynumstr="";
             for(var j=0;j<emptynum;j++)
             {
                 emptynumstr+='<option value="' + (j+1) + '">' + (j+1)+ '人</option>';
             }
             $("#u_num").html(emptynumstr);

            var pickuppointstr = ''; //上车地点下拉框字符串
            var dropoffpointstr = ''; //下车地点下拉框字符串
            if ($("#hid_pickuppoint").trimVal() != "") {
                var arrpickuppoint = $("#hid_pickuppoint").trimVal().split('，');
                for (var a = 0; a < arrpickuppoint.length; a++) {
                    pickuppointstr += '<option value="' + arrpickuppoint[a] + '">' + arrpickuppoint[a] + '</option>';
                }
            }
            else {
                $("div_pickuppoint").hide();
            }

            if ($("#hid_dropoffpoint").trimVal() != '') {
                var arrdropoffpoint = $("#hid_dropoffpoint").trimVal().split('，');
                for (var a = arrdropoffpoint.length - 1; a >= 0; a--) {
                    dropoffpointstr += '<option value="' + arrdropoffpoint[a] + '">' + arrdropoffpoint[a] + '</option>';
                }
            } else {
                $("#div_dropoffpoint").hide();
            }
            $("#pointuppoint").html(pickuppointstr);
            $("#dropoffpoint").html(dropoffpointstr);
        }

        //给送车员发送乘车人名单
        function SendTrvalNamelist(obj,telphone) {
            var proid = '<%=this.proid %>';
            var daydate = '<%=this.daydate %>';
            var firststationtime = '<%=this.firststationtime %>';
            var orderstate = '';
            orderstate = '4,8,22';
            if (telphone == "") {
                telphone = $("#telphone").trimVal();
            }
            if (telphone == "") {
                alert("请输入电话");
                return;
            }

            $.post("/JsonFactory/OrderHandler.ashx?oper=SendTrvalNamelist", {firststationtime:firststationtime, gooutdate: daydate, proid: proid, paystate: $("#hid_paystate_haspay").trimVal(), orderstate: orderstate,telphone: telphone }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    location.reload();
                    return;
                }
                if (data.type == 100) {

                    $(obj).text("已经给送车员发送乘客名单").attr("disabled", "disabled").css("border-color", "gray").css("background-color", "gray");
                    return;
                }
            })
        }
    </script>
    <style type="text/css">
        .btn.btn-orange-dark
        {
            color: #FFF;
            border-color: rgb(28, 189, 241);
            background-color: rgb(60, 175, 220);
            font-size: 14px;
            width: 120px;
            margin-top: 10px;
            padding: 7px 80px;
        }
    </style>
    <style type="text/css">
        .sku-box-shadow
        {
            box-shadow: 0 -1px 14px rgba(0, 0, 0, 0.9);
        }
        .sku-layout
        {
            background-color: #fff;
        }
        .sku-layout .layout-title
        {
            border-bottom: 2px solid #e5e5e5;
            border-image: url("") 2 2 2 2;
            border-top-width: 0;
            padding: 8px 0 6px;
            position: static;
        }
        .sku-box-shadow
        {
            box-shadow: 0 -1px 14px rgba(0, 0, 0, 0.9);
        }
        .name-card
        {
            margin-left: 0;
            overflow: hidden;
            padding: 5px 0;
            position: relative;
            width: auto;
        }
        .name-card.sku-name-card .thumb
        {
            height: 40px;
            margin: 2px 0 0 10px;
            width: 40px;
        }
        .name-card .thumb
        {
            background-size: cover;
            float: left;
            height: 58px;
            margin-left: auto;
            margin-right: auto;
            overflow: hidden;
            position: relative;
            width: 58px;
        }
        .clearfix::before, .clearfix::after
        {
            content: "";
            display: table;
            line-height: 0;
        }
        .clearfix::after
        {
            clear: both;
            content: " ";
            display: block;
            font-size: 0;
            height: 0;
            visibility: hidden;
        }
        .clearfix::after
        {
            clear: both;
            content: "";
            display: table;
        }
        .clearfix::after
        {
            clear: both;
        }
        .clearfix::before, .clearfix::after
        {
            content: "";
            display: table;
            line-height: 0;
        }
        .name-card.sku-name-card .detail
        {
            margin-left: 55px;
        }
        .name-card .detail
        {
            margin-left: 65px;
            position: relative;
            width: auto;
        }
        .name-card .detail
        {
            margin-left: 68px;
            position: relative;
            width: auto;
        }
        .sku-layout .sku-cancel
        {
            cursor: pointer;
            padding: 10px;
            position: absolute;
            right: 3px;
            top: 2px;
        }
        .sku-layout .layout-title .goods-base-info .title
        {
            line-height: 22px;
            padding-right: 45px;
        }
        .name-card .detail p
        {
            color: #ccc;
            font-size: 12px;
            line-height: 16px;
            margin: 0 0 2px;
            position: relative;
            white-space: nowrap;
        }
        .ellipsis
        {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        .c-black
        {
            color: #333;
        }
        .c-black
        {
            color: #333 !important;
        }
        .clearfix::before, .clearfix::after
        {
            content: "";
            display: table;
            line-height: 0;
        }
        .clearfix::after
        {
            clear: both;
            content: " ";
            display: block;
            font-size: 0;
            height: 0;
            visibility: hidden;
        }
        .clearfix::after
        {
            clear: both;
            content: "";
            display: table;
        }
        .clearfix::after
        {
            clear: both;
        }
        .clearfix::before, .clearfix::after
        {
            content: "";
            display: table;
            line-height: 0;
        }
        .sku-layout .layout-title .goods-base-info .goods-price
        {
            padding: 0 55px 0 0;
        }
        .goods-price
        {
            padding: 5px 10px 2px;
            text-align: left;
        }
        
        .sku-layout .layout-title .goods-base-info .goods-price .current-price
        {
            line-height: 20px;
        }
        .current-price
        {
            color: #f60;
            display: inline-block;
            font-size: 14px;
        }
        .pull-left
        {
            float: left;
        }
        .sku-layout .layout-title .goods-base-info .goods-price .current-price .price-name
        {
            padding-top: 1px;
        }
        .current-price > span
        {
            display: inline-block;
            vertical-align: middle;
        }
        .pull-left
        {
            float: left;
        }
        .font-size-14
        {
            font-size: 14px !important;
        }
        .sku-layout .vertical-middle
        {
            vertical-align: middle;
        }
        .current-price .price
        {
            display: inline-block;
            font-size: 20px;
            vertical-align: middle;
        }
        .goods-price i
        {
            font-size: 20px;
            font-style: normal;
            font-weight: bold;
            vertical-align: baseline;
        }
        .sys_item_price
        {
            color: #c00;
            font-size: 22px;
            vertical-align: middle;
        }
        .font-size-18
        {
            font-size: 18px !important;
        }
        .c-orangef60
        {
            color: #f60 !important;
        }
        .sku-layout .sku-cancel .cancel-img
        {
            background-image: url("http://shop.etown.cn/h5/order/image/showcase.png");
            background-position: 0 -120px;
            background-repeat: no-repeat;
            height: 27px;
            width: 27px;
        }
        .sku-layout .layout-content
        {
            background-color: #fff;
            border: 1px solid white;
            line-height: 20px;
            overflow-y: scroll;
        }
        .block.block-list + .block.block-list
        {
            margin-top: 12px;
        }
        .sku-layout .layout-content .goods-models
        {
            padding: 1px 10px;
        }
        .block.block-list
        {
            margin-top: 0 !important;
        }
        .block.block-list
        {
            box-sizing: border-box;
            font-size: 14px;
            list-style: outside none none;
            margin: 0;
            padding: 0 0 0 10px;
        }
        .block.block-border-top-none
        {
            border-top: 0 none;
        }
        .block
        {
            background-color: #fff;
            border-bottom: 2px solid #e5e5e5;
            border-image: url("/h5/order/image/border-line.png") 2 2 2 2;
            border-top: 2px solid #e5e5e5;
            display: block;
            font-size: 14px;
            margin: 10px 0;
            overflow: hidden;
            position: relative;
        }
        .sku-layout .layout-content .goods-models > dl .sku-num
        {
            line-height: 42px;
        }
        .sku-layout .model-title
        {
            width: 100px;
        }
        .sku-layout .model-title
        {
            font-size: 13px;
            padding-top: 3px;
        }
        .pull-left
        {
            float: left;
        }
        .sku-layout .quantity
        {
            float: right;
            margin-top: 10px;
        }
        .quantity
        {
            float: left !important;
            display: inline-block;
            font-size: 0;
            position: relative;
            vertical-align: middle;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <header class="header" style="background-color: #3CAFDC;">
          <h1 id="h_comname"></h1>
        <div class="left-head"> 
                 <a href="/ui/pmui/ETicket/mETicketIndex.aspx" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="home-10"></span></span>
              </a> 
            </div>
        <div class="right-head"> 
                <a href="/yanzheng/loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>    
        </div>
        </header>
    <!-- container -->
    <div class="container ">
        <div class="tabber  tabber-n3 tabber-double-11 clearfix">
            <a href="javascript:goyanzheng()" style="width: 33%;">验证电子凭证</a> <a href="javascript:goyanzhenglist()"
                style="width: 33%;">验证列表</a> <a class="active" href="javascript:golvyoudaba()" style="width: 33%;">
                    旅游大巴查询</a>
        </div>
        <%-- <div class="list-search" style="height: 32px; padding: 5px 10px 7px; background: #eee;">
            <dl class="fn-clear" style="height: 32px; background: #fff; border-radius: 5px; border: 1px solid #c9c9c9;
                position: relative;">
                <dt style="position: relative; overflow: hidden; padding-left: 5px; margin-right: 40px;">
                    <input type="tel" id="pno" placeholder="选择出车日期" style="height: 25px; margin-top: 4px;
                        border: 0; outline: 0; background: 0; width: 100%;" />
                </dt>
                <dd style="float: left; width: 30px; height: 25px; margin-top: 4px; position: absolute;
                    top: 0; right: 0;">
                    <s id="search_botton" style="width: 17px; height: 17px; display: block; vertical-align: middle;
                        margin: 4px auto 0; background: url(/Images/public_com.png) no-repeat -44px 0;
                        background-size: 64px 17.5px;"></s>
                </dd>
            </dl>
        </div>--%>
        <div id="backs-list-container" class="block">
            <li class="block block-order animated">
                <div class="block block-list block-border-top-none block-border-bottom-none" id="list"
                    style="padding-left: 0;">
                    <%--   <div class="layout-box">
                        <ul class="list-a">
                            <li>姓名: 手机: 身份证: </li>
                            <li>上车地点: 下车地点: </li>
                        </ul>
                    </div>--%>
                </div>
            </li>
            <div class="empty-list" style="margin-top: 30px; display: none;">
                <!-- 文本 -->
                <div>
                    <h4>
                        哎呀，暂不记录？</h4>
                </div>
            </div>
        </div>
    </div>
    <div style="overflow: hidden; visibility: visible; opacity: 1; bottom: 0px; left: 0px;
        right: 0px; transform: translate3d(0px, 0px, 0px); position: fixed; z-index: 98;
        transition: all 300ms ease 0s; display: none;" class="sku-layout sku-box-shadow"
        id="suborder">
        <div class="layout-title sku-box-shadow name-card sku-name-card">
            <div class="thumb">
                <img alt="" src="/Images/defaultThumb.png">
            </div>
            <div class="detail goods-base-info clearfix">
                <p id="suborder_proname" class="title c-black ellipsis">
                </p>
                <div class="goods-price clearfix">
                    <div class="current-price c-black pull-left">
                        <span class="price-name pull-left font-size-14 c-orangef60">￥</span> <i id="suborder_price"
                            class="js-goods-price price font-size-18 c-orangef60 vertical-middle sys_item_price">
                        </i><i class="sys_item_mktprice" style="display: none;"></i>
                    </div>
                </div>
            </div>
            <div class="js-cancel sku-cancel">
                <div class="cancel-img">
                </div>
            </div>
        </div>
        <div class="goods-models js-sku-views block block-list block-border-top-none" id="travlename_view">
            <dl class="clearfix block-item">
                <dt class="model-title sku-num pull-left">
                    <label>
                        预约人姓名</label>
                </dt>
                <dl class="clearfix">
                    <div class="quantity">
                        <input type="text" placeholder="姓名" value="" class="txtinputsub" id="u_name"
                            style="font-size: 12px;">
                    </div>
                </dl>
            </dl>
        </div>
        <div class="goods-models js-sku-views block block-list block-border-top-none" id="travlephone_view">
            <dl class="clearfix block-item">
                <dt class="model-title sku-num pull-left">
                    <label>
                        预约人手机</label>
                </dt>
                <dl class="clearfix">
                    <div class="quantity">
                        <input type="tel" placeholder="手机" value="" class="txtinputsub" id="u_phone"
                            style="font-size: 12px;">
                    </div>
                </dl>
            </dl>
        </div>
        <div class="adv-opts layout-content">
            <div class="goods-models js-sku-views block block-list block-border-top-none hide"
                id="travlenum_view">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            预约人数</label>
                    </dt>
                    <dd>
                        <dl class="clearfix">
                            <div class="quantity">
                                <select style="height: 30px; width: 123px; font-size: 12px;" class="txtinputsub"
                                    id="u_num">
                                    <option value="1">1人</option>
                                    <option value="2">2人</option>
                                    <option value="3">3人</option>
                                    <option value="4">4人</option>
                                    <option value="5">5人</option>
                                    <option value="6">6人</option>
                                    <option value="7">7人</option>
                                    <option value="8">8人</option>
                                    <option value="9">9人</option>
                                    <option value="10">10人</option>
                                </select>
                            </div>
                            <div class="stock pull-right font-size-12">
                            </div>
                        </dl>
                    </dd>
                </dl>
            </div>
            <div class="goods-models js-sku-views block block-list block-border-top-none" id="div_pickuppoint">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            上车地点</label>
                    </dt>
                    <dl class="clearfix">
                        <div class="quantity">
                            <select style="height: 30px; width: 123px; font-size: 12px;" class="txtinputsub"
                                id="pointuppoint">
                            </select>
                        </div>
                    </dl>
                </dl>
            </div>
            <div class="goods-models js-sku-views block block-list block-border-top-none" id="div_dropoffpoint">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            下车地点</label>
                    </dt>
                    <dl class="clearfix">
                        <div class="quantity">
                            <select style="height: 30px; width: 123px; font-size: 12px;" class="txtinputsub"
                                id="dropoffpoint">
                            </select>
                        </div>
                    </dl>
                </dl>
            </div>
            <div class="goods-models js-sku-views block block-list block-border-top-none" id="div_payee">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            收款人</label>
                    </dt>
                    <dl class="clearfix">
                        <div class="quantity">
                             <input type="text" placeholder="收款人姓名" value="" class="txtinputsub" id="txt_payee"
                            style="font-size: 12px;">
                        </div>
                    </dl>
                </dl>
            </div>
             
            <div class="confirm-action content-foot" style="padding: 10px 10px 50px 10px;">
                <a class="js-confirm-it btn btn-block btn-orange-dark" href="javascript:void(0);" id="confirmsuborder">提交订单</a>
                <br />
                <label style="color:red;">
                    </label>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_servertype" value="<%=servertype %>" />
    <input type="hidden" id="hid_orderstate_paysuc" value="<%=orderstate_paysuc %>" />
    <input type="hidden" id="hid_paystate_haspay" value="<%=paystate_haspay %>" />
    <!--上车地点，下车地点-->
    <input id="hid_pickuppoint" type="hidden" value="<%=pickuppoint %>" />
    <input id="hid_dropoffpoint" type="hidden" value="<%=dropoffpoint %>" />

    <input id="hid_adviseprice" type="hidden" value="<%=advise_price %>" />
    <input id="hid_emptynum" type="hidden" value="<%=emptynum %>" />
    <input id="fangzhichongfutijiao" type="hidden" value="0" />

</asp:Content>
