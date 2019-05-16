<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="linegroupdate.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.travel.linegroupdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/Styles/base2.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/common.css" />
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/Impromptu.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <link href="/Scripts/poshytip-1.1/tip-yellowsimple/tip-yellowsimple.css" rel="stylesheet"
        type="text/css" />
    <script src="/Scripts/poshytip-1.1/jquery.poshytip.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        var currentDate = "";
        $(function () {
            $("#hidLeavingDate").val($("#hidinitLeavingDate").val()); //防止刷新
            var array = $("#hidLeavingDate").val().split(',').sort(function (a, b) {
                return new Date(a) - new Date(b);
            });
            $('#dateselect').empty();


            for (var i = 0; i < array.length; i++) {
                if ($.trim(array[i]) != "") { 
                        $('#dateselect').append('<span class="category-visited1" style="display:none;"><a href="javascript:void(0)" class="catico" onclick="selectDate(this)">' + array[i] + '</a><span id="spanrr_' + array[i] + '" class="catico1" onclick="removeDate(this)">x　</span></span>');
                        $('#divLine .tip-hook').inputTip("none");
                        loadLineData(array[i]);
                        currentDate = array[i];
                        setClass(currentDate); 
                }
            }




            $('#datepicker').datepicker({
                minDate: +0,
                onSelect: function (dateText) {
                    $(".tip-yellowsimple").remove();

                    var result = false;

                    var data = [];
                    var dateTimearr = $("#hidLeavingDate").val().split(',');
                    if (dateTimearr != null && dateTimearr.length > 0) {
                        data = $.merge(dateTimearr, data);
                    }

                    $(data).each(function (i, n) {
                        if (n == dateText) {
                            result = true;
                        }
                    });

                    if (!result) {

                        currentDate = dateText;

                        if ($('#hidLeavingDate').val() == '') {
                            $('#hidLeavingDate').val(dateText);
                        } else {
                            $('#hidLeavingDate').val($('#hidLeavingDate').val() + ',' + dateText);
                        }
                        var dateSort = $("#hidLeavingDate").val().split(',').sort(function (a, b) {
                            return new Date(a) - new Date(b);
                        });
                        if (dateSort != null && dateSort.length > 0) {
                            $('#dateselect').empty();
                            for (var i = 0; i < dateSort.length; i++) {
                                $('#dateselect').append('<span class="category-visited1"  style="display:none;"><a href="javascript:void(0)" class="catico" onclick="selectDate(this)">' + dateSort[i] + '</a><span  id="spanrr_' + dateSort[i] + '" class="catico1" onclick="removeDate(this)" style="">x </span></span>');

                                $('#divLine .tip-hook').inputTip("none");

                                loadLineData(dateSort[i]);
                                currentDate = dateSort[i];
                                setClass(currentDate);
                            }

                        }
                        setClass(currentDate);
                    }

                },
                beforeShowDay: function (date) {

                    var dt = formatDate(date);
                    var data = [];
                    var dateTimearr = $("#hidLeavingDate").val().split(',');
                    if (dateTimearr != null && dateTimearr.length > 0) {
                        data = $.merge(dateTimearr, data);
                        //currentDate = dateTimearr[0];
                    }

                    var result = false;
                    $(data).each(function (i, n) {
                        if (n == dt) {
                            result = true;
                        }
                    });

                    if (result) {
                        return [true, formatDate(date) + " pickerselected pickerspanTime", ''];

                    } else {
                        return [true, formatDate(date) + " pickerspanTime", ''];
                    }

                }

            });

        });

        function initDate() {
            var dateTimearr = $("#hidLeavingDate").val().split(',');
            if (dateTimearr != null && dateTimearr.length > 0) {
                currentDate = dateTimearr[0];
                setClass(currentDate);
            }
        }

        function setClass(date) {
            $("#dateselect .category-visited1").find("a").each(function () {

                if ($(this).html() == date) {
                    $(this).attr("style", "background:#64b7f1; color:white;");
                } else
                    $(this).attr("style", "");
            });
        }


        function removeDate(obj) {
            if (confirm("确定删除此日期行程吗？")) {

                var className = $(obj).parent().find("a").html();
                var dateArray = $("#hidLeavingDate").val().split(',');
                dateArray.splice($.inArray(className, dateArray), 1);
                $("#hidLeavingDate").val(dateArray.join(","));

                if ($("#hid_seleddate").val().indexOf(className) != -1) {
                    var dateArray2 = $("#hid_seleddate").val().split(',');
                    dateArray2.splice($.inArray(className, dateArray2), 1);
                    $("#hid_seleddate").val(dateArray2.join(","));

                    $("#tr" + className).remove();

                }
                $('.' + className).removeClass("pickerselected");

                $(obj).parent().remove();
                removeLineData(className);

                initDate();

            }

        }

        function removeLineData(daydate) {
            $.post("/JsonFactory/ProductHandler.ashx?oper=DelLineGroupDate", { lineid: $("#hid_lineid").val(), daydate: daydate }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {

                }
                if (data.type == 100) {

                }
            });
        }

        function selectDate(obj) {
            $('#divLine .tip-hook').inputTip("none");

            loadLineData($(obj).html());
            currentDate = $(obj).html();
            setClass(currentDate);
        }
        function loadLineData(daydate) {
            var dt = new Date(Date.parse(daydate.replace(/-/g, "/"))); //转换成Data(); 
            var weekDay = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];
            //alert(weekDay[dt.getDay()]);

            var seleddate = $("#hid_seleddate").val();
            var LeavingDate = $("#hidLeavingDate").val();
            var initLeavingDate = $("#hidinitLeavingDate").val();

            var agent1_back = 0;
            var agent2_back = 0;
            var agent3_back = 0;

            if (seleddate.indexOf(daydate) == -1) {//判断是否在选中的日期中  
                


                if ($('#hid_seleddate').val() == '') {
                    $('#hid_seleddate').val(daydate);
                } else {
                    $('#hid_seleddate').val($('#hid_seleddate').val() + ',' + daydate);
                }


                //判断是否在线路的团期中
                if (initLeavingDate.indexOf(daydate) == -1) {//不存在线路团期中，直接添加新的团期
                    agent1_back = '<%=basic_agent1_price %>';
                    agent2_back = '<%=basic_agent2_price %>';
                    agent3_back = '<%=basic_agent3_price %>';

                    //酒店客房9 
                    if ($("#hid_servertype").val() == 9) {
                        $("#tbody1").append('<tr id="tr' + daydate + '"><td colspan="2">' +
                        daydate + '(' + weekDay[dt.getDay()] + ') &nbsp;&nbsp;价格: <input type="text" id="dayprice' + daydate + '" value="' + $("#hid_adviseprice").val() + '"  style="width:40px;">&nbsp;&nbsp;空位: <input type="text"' + 'id="emptynum' + daydate + '" value="0" style="width:40px;">&nbsp;&nbsp;一级分销<span style="font-size:15px;color:blue;">返还</span>: <input type="text"' +
                            'id="agent1_back' + daydate + '" value="' + agent1_back + '" style="width:40px;">&nbsp;&nbsp;二级分销<span style="font-size:15px;color:blue;">返还</span>: <input type="text"' +
                            'id="agent2_back' + daydate + '" value="' + agent2_back + '" style="width:40px;">&nbsp;&nbsp;三级分销<span style="font-size:15px;color:blue;">返还</span>: <input type="text"' +
                            'id="agent3_back' + daydate + '" value="' + agent3_back + '" style="width:40px;">&nbsp;&nbsp;已订数量:0 &nbsp;&nbsp;待支付数量:0 &nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                    '</td>' +
                '</tr>');
                    }
                    //旅游大巴10
                    else if ($("#hid_servertype").val() == 10) {
                        $("#tbody1").append('<tr id="tr' + daydate + '"><td colspan="2">' +
                        daydate + '(' + weekDay[dt.getDay()] + ') &nbsp;&nbsp;价格: <input type="text" id="dayprice' + daydate + '" value="' + $("#hid_adviseprice").val() + '"  style="width:40px;">&nbsp;&nbsp;空位: <input type="text"' + 'id="emptynum' + daydate + '" value="0" style="width:40px;">&nbsp;&nbsp;一级分销价: <input type="text"' +
                            'id="agent1_back' + daydate + '" value="' + agent1_back + '" style="width:40px;">&nbsp;&nbsp;二级分销价: <input type="text"' +
                            'id="agent2_back' + daydate + '" value="' + agent2_back + '" style="width:40px;">&nbsp;&nbsp;三级分销价: <input type="text"' +
                            'id="agent3_back' + daydate + '" value="' + agent3_back + '" style="width:40px;">&nbsp;&nbsp;已订数量:0 &nbsp;&nbsp;待支付数量:0 &nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                    '</td>' +
                '</tr>');
                    }
                    //跟团游2 、 当地游8
                    else if ($("#hid_servertype").val() == 2 || $("#hid_servertype").val() == 8) {
                        $("#tbody1").append('<tr id="tr' + daydate + '"><td colspan="2">' +
                        daydate + '(' + weekDay[dt.getDay()] + ') &nbsp;&nbsp;价格: <input type="text" id="dayprice' + daydate + '" value="0">&nbsp;&nbsp;空位: <input type="text"' +
                            'id="emptynum' + daydate + '" value="0">&nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                    '</td>' +
                '</tr>');
                    }

                }
                else {
                    $("#tbody1").append('<tr id="tr' + daydate + '"></tr>');
                     
                    //存在于线路团期中，查询团期中当天情况
                    $.post("/JsonFactory/ProductHandler.ashx?oper=GetLineDayGroupDate", { daydate: daydate, lineid: $("#hid_lineid").val() }, function (dataa) {
                        dataa = eval("(" + dataa + ")");
                        if (dataa.type == 1) { }
                        if (dataa.type == 100) {
                            //酒店客房9
                            if ($("#hid_servertype").val() == 9) {
                               
                                if (dataa.msg.agent1_back > 0) {
                                    agent1_back = dataa.msg.agent1_back;
                                   
                                }
                                if (dataa.msg.agent2_back > 0) {
                                    agent2_back = dataa.msg.agent2_back;
                                }
                                if (dataa.msg.agent3_back > 0) {
                                    agent3_back = dataa.msg.agent3_back;
                                }
                                //                                $("#tbody1").append('<tr id="tr' + daydate + '"><td colspan="2">' +
                                //                        daydate + '&nbsp;&nbsp; 价格: <input type="text" id="dayprice' + daydate + '" value="' + dataa.msg.Menprice + '" style="width:40px;">&nbsp;&nbsp;空位: <input type="text"' + 'id="emptynum' + daydate + '" value="' + dataa.msg.Emptynum + '" style="width:40px;">&nbsp;&nbsp;一级分销返还: <input type="text"' + 'id="agent1_back' + daydate + '" value="' + agent1_back + '" style="width:40px;">&nbsp;&nbsp;二级分销返还: <input type="text"' + 'id="agent2_back' + daydate + '" value="' + agent2_back + '" style="width:40px;">&nbsp;&nbsp;三级分销返还: <input type="text"' + 'id="agent3_back' + daydate + '" value="' + agent3_back + '" style="width:40px;">&nbsp;&nbsp; 已订数量: ' + dataa.msg.paysucnum + '&nbsp;&nbsp;待支付数量: ' + dataa.msg.waitpaynum + '&nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                                //                    '</td>' +
                                //                '</tr>');
                                $("#tr" + daydate).html('<td colspan="2">' +
                                                  daydate + '(' + weekDay[dt.getDay()] + ')&nbsp;&nbsp; 价格: <input type="text" id="dayprice' + daydate + '" value="' + dataa.msg.Menprice + '" style="width:40px;">&nbsp;&nbsp;空位: <input type="text"' + 'id="emptynum' + daydate + '" value="' + dataa.msg.Emptynum + '" style="width:40px;">&nbsp;&nbsp;一级分销<span style="font-size:15px;color:blue;">返还</span>: <input type="text"' + 'id="agent1_back' + daydate + '" value="' + agent1_back + '" style="width:40px;">&nbsp;&nbsp;二级分销<span style="font-size:15px;color:blue;">返还</span>: <input type="text"' + 'id="agent2_back' + daydate + '" value="' + agent2_back + '" style="width:40px;">&nbsp;&nbsp;三级分销<span style="font-size:15px;color:blue;">返还</span>: <input type="text"' + 'id="agent3_back' + daydate + '" value="' + agent3_back + '" style="width:40px;">&nbsp;&nbsp; 已订数量: ' + dataa.msg.paysucnum + '&nbsp;&nbsp;待支付数量: ' + dataa.msg.waitpaynum + '&nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                                                '</td>');
                            }
                            //旅游大巴10 
                            if ($("#hid_servertype").val() == 10) {
                                if (dataa.msg.agent1_back > 0) {
                                    agent1_back = dataa.msg.agent1_back;
                                }
                                if (dataa.msg.agent2_back > 0) {
                                    agent2_back = dataa.msg.agent2_back;
                                }
                                if (dataa.msg.agent3_back > 0) {
                                    agent3_back = dataa.msg.agent3_back;
                                }
                                //                                $("#tbody1").append('<tr id="tr' + daydate + '"><td colspan="2">' +
                                //                        daydate + '&nbsp;&nbsp; 价格: <input type="text" id="dayprice' + daydate + '" value="' + dataa.msg.Menprice + '" style="width:40px;">&nbsp;&nbsp;空位: <input type="text"' + 'id="emptynum' + daydate + '" value="' + dataa.msg.Emptynum + '" style="width:40px;">&nbsp;&nbsp;一级分销返还: <input type="text"' + 'id="agent1_back' + daydate + '" value="' + agent1_back + '" style="width:40px;">&nbsp;&nbsp;二级分销返还: <input type="text"' + 'id="agent2_back' + daydate + '" value="' + agent2_back + '" style="width:40px;">&nbsp;&nbsp;三级分销返还: <input type="text"' + 'id="agent3_back' + daydate + '" value="' + agent3_back + '" style="width:40px;">&nbsp;&nbsp; 已订数量: ' + dataa.msg.paysucnum + '&nbsp;&nbsp;待支付数量: ' + dataa.msg.waitpaynum + '&nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                                //                    '</td>' +
                                //                '</tr>');
                                $("#tr" + daydate).html('<td colspan="2">' +
                                                  daydate + '(' + weekDay[dt.getDay()] + ')&nbsp;&nbsp; 价格: <input type="text" id="dayprice' + daydate + '" value="' + dataa.msg.Menprice + '" style="width:40px;">&nbsp;&nbsp;空位: <input type="text"' + 'id="emptynum' + daydate + '" value="' + dataa.msg.Emptynum + '" style="width:40px;">&nbsp;&nbsp;一级分销价: <input type="text"' + 'id="agent1_back' + daydate + '" value="' + agent1_back + '" style="width:40px;">&nbsp;&nbsp;二级分销价: <input type="text"' + 'id="agent2_back' + daydate + '" value="' + agent2_back + '" style="width:40px;">&nbsp;&nbsp;三级分销价: <input type="text"' + 'id="agent3_back' + daydate + '" value="' + agent3_back + '" style="width:40px;">&nbsp;&nbsp; 已订数量: ' + dataa.msg.paysucnum + '&nbsp;&nbsp;待支付数量: ' + dataa.msg.waitpaynum + '&nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                                                '</td>');
                            }
                            //跟团游2 当地游8
                            else if ($("#hid_servertype").val() == 2 || $("#hid_servertype").val() == 8) {
                                //                                $("#tbody1").append('<tr id="tr' + daydate + '"><td colspan="2">' +
                                //                        daydate + '&nbsp;&nbsp; 价格: <input type="text" id="dayprice' + daydate + '" value="' + dataa.msg.Menprice + '">&nbsp;&nbsp;空位: <input type="text"' +
                                //                            'id="emptynum' + daydate + '" value="' + dataa.msg.Emptynum + '">&nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                                //                    '</td>' +
                                //                '</tr>');
                                $("#tr" + daydate).html('<td colspan="2">' +
                        daydate + '(' + weekDay[dt.getDay()] + ')&nbsp;&nbsp; 价格: <input type="text" id="dayprice' + daydate + '" value="' + dataa.msg.Menprice + '">&nbsp;&nbsp;空位: <input type="text"' +
                           'id="emptynum' + daydate + '" value="' + dataa.msg.Emptynum + '">&nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                    '</td>');
                            }

                        }
                    })
                }
            } 
        }
        function deletetqq(ddealdate) {

            if (confirm("确定删除此日期行程吗？")) {

                var className = $("#spanrr_" + ddealdate).parent().find("a").html();

                //团期当天收人了，则不再可以删除团期(目前只是针对大巴、客房、跟团游、当地游，其他产品没设置)
                $.post("/JsonFactory/ProductHandler.ashx?oper=WhetherSaled", { lineid: $("#hid_lineid").val(), daydate: className }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        if (data.msg == 'True' || data.msg == 'true') {
                            alert("当前日期已经收人，不可删除；如果希望当天不再收人，可设置空位数量为0");
                            return;
                        }
                        else {
                            var dateArray = $("#hidLeavingDate").val().split(',');
                            dateArray.splice($.inArray(className, dateArray), 1);
                            $("#hidLeavingDate").val(dateArray.join(","));

                            if ($("#hid_seleddate").val().indexOf(className) != -1) {
                                var dateArray2 = $("#hid_seleddate").val().split(',');
                                dateArray2.splice($.inArray(className, dateArray2), 1);
                                $("#hid_seleddate").val(dateArray2.join(","));

                                $("#tr" + className).remove();

                            }
                            $('.' + className).removeClass("pickerselected");

                            $("#spanrr_" + ddealdate).parent().remove();
                            removeLineData(className);

                            initDate();

                        }
                    }
                })





            }

        }
        function formatDate(datetime) {

            var dateObj = new Date(datetime);
            var month = dateObj.getMonth() + 1;
            if (month < 10) {
                month = "0" + month;
            }
            var day = dateObj.getDate();
            if (day < 10) {
                day = "0" + day;
            }
            return dateObj.getFullYear() + "-" + month + "-" + day;
        }

        function upgroupdate() {
            var lineid = $("#hid_lineid").val();
            var datestr = $("#hid_seleddate").val();

            if ($("#hid_seleddate").val() == "") {
                alert("请添加团期");
                return;
            }
            var dayprice = '';
            var emptynum = '';
            var agent1_back = '';
            var agent2_back = '';
            var agent3_back = '';

            var dateTimearr = $("#hid_seleddate").val().split(',');
            if (dateTimearr != null && dateTimearr.length > 0) {
                for (var i = 0; i < dateTimearr.length; i++) {
                    dayprice += $("#dayprice" + dateTimearr[i]).trimVal() + ",";
                    emptynum += $("#emptynum" + dateTimearr[i]).trimVal() + ",";


                    if (isNaN($("#dayprice" + dateTimearr[i]).val())) {
                        $.prompt(dateTimearr[i] + "价格格式不正确");
                        return;
                    }
                    if (isNaN($("#emptynum" + dateTimearr[i]).val())) {
                        $.prompt(dateTimearr[i] + "空位格式不正确");
                        return;
                    }
                    //现在阶段只有 订房、大巴 加分销返还，其他服务类型没有
                    if ('<%=ServerType %>' == '9' || '<%=ServerType %>' == '10') {
                        agent1_back += $("#agent1_back" + dateTimearr[i]).trimVal() + ",";
                        agent2_back += $("#agent2_back" + dateTimearr[i]).trimVal() + ",";
                        agent3_back += $("#agent3_back" + dateTimearr[i]).trimVal() + ",";

                        if (isNaN($("#agent1_back" + dateTimearr[i]).trimVal())) {
                            $.prompt(dateTimearr[i] + "一级分销返回金额格式不正确");
                            return;
                        }
                        if (isNaN($("#agent2_back" + dateTimearr[i]).trimVal())) {
                            $.prompt(dateTimearr[i] + "二级分销返回金额格式不正确");
                            return;
                        }
                        if (isNaN($("#agent3_back" + dateTimearr[i]).trimVal())) {
                            $.prompt(dateTimearr[i] + "三级分销返回金额格式不正确");
                            return;
                        }
                    }
                }
                dayprice = dayprice.substr(0, dayprice.length - 1);
                emptynum = emptynum.substr(0, emptynum.length - 1);
                if ('<%=ServerType %>' == '9' || '<%=ServerType %>' == '10') {
                    agent1_back = agent1_back.substr(0, agent1_back.length - 1);
                    agent2_back = agent2_back.substr(0, agent2_back.length - 1);
                    agent3_back = agent3_back.substr(0, agent3_back.length - 1);
                }
            }

            $.post("/JsonFactory/ProductHandler.ashx?oper=Uplinegroupdate", { initdatestr: $("#hidinitLeavingDate").val(), datestr: datestr, dayprice: dayprice, emptynum: emptynum, lineid: lineid, agent1_back: agent1_back, agent2_back: agent2_back, agent3_back: agent3_back }, function (dataa) {
                dataa = eval("(" + dataa + ")");
                if (dataa.type == 1) {

                }
                if (dataa.type == 100) {
                    $.prompt("调整团期成功", {
                        buttons: [
                                 { title: '确定', value: true }
                                ],
                        opacity: 0.1,
                        focus: 0,
                        show: 'slideDown',
                        submit: function (e, v, m, f) { if (v == true) { window.open("/ui/pmui/productlist.aspx", target = '_self') } }
                    });
                }
            })
        }
    </script>
    <style type="text/css">
        #dateline
        {
            width: 1005px;
            padding-top: 295px;
            padding-left: 0;
        }
        #mianDate
        {
            clear: both;
        }
        #datepicker
        {
            float: left;
            width: 240px;
        }
        #dateselect
        {
            float: left;
            width: 505px;
            padding-top: 10px;
            padding-left: 50px;
        }
        .ui-datepicker
        {
            width: 255px;
            padding: .2em .2em 0;
            display: none;
        }
        .pickerspanTime a.ui-state-default
        {
            background: #E4F1FB;
            color: #0074A3;
        }
        .pickerselected a.ui-state-default
        {
            background: #3BAAE3;
            border: 1px solid #74B2E2;
        }
        .radio
        {
            float: left;
            min-height: 25px;
        }
        .spanRadio
        {
            padding-top: 3px;
        }
        
        .youb
        {
            background: #f1f2f4;
            width: 485px;
            padding: 10px 0 15px 10px;
            float: right;
        }
        .youb span
        {
            width: auto;
        }
        
        a:link, a:visited
        {
            color: black;
        }
        .a_anniu
        {
            height: 25px;
            line-height: 25px;
            border: 2px outset #eee;
            background: #ccc;
            text-align: center;
            font-size: 12px;
            color: #000;
            text-decoration: none;
            padding: 0 15px;
            margin: 5px 0;
        }
    </style>
    <script type="text/javascript" src="/Scripts/jquery.cookie.2.2.0.min.js"></script>
    <script type="text/javascript">
        $(function () {

            $("#dh_4").hide();
            $("#dh_21").hide();
            $("#dh_15").hide();
            $("#dh_6").addClass("bold");
            $("#dh_5").addClass("bold");
            $("#dh_10").addClass("bold")


            if ($.cookie($("#hid_comid").val() + "_navigationid")) {

                var seledid = $.cookie($("#hid_comid").val() + "_navigationid");

                $("#dh_" + seledid).addClass("seled");
            }
        })
        function dhclick(dhid) {

            $.cookie($("#hid_comid").val() + "_navigationid", dhid, { path: '/' });
        }
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <div id="mainhome" class="main">
        <div style="width: 1024px; height: 1px; overflow: hidden; display: none; _display: block;
            display: none;">
            min-width</div>
        <div class="home-hd" style="z-index: 1; display: block;">
            <div class="home-hd-top">
                <div class="home-hd-lf-bg">
                    <div>
                        <img src="<%=comlogo %>" alt="" width="190px" height="68px" /></div>
                    <div id="account">
                        您好，
                        <%=comname%>&gt;<%=groupname%>&gt;<%=username %>
                        <div class="shortcut">
                            <a href="/manage.aspx" class="ui-exec exec-mail-homeload" onfocus="this.blur()">首页</a>
                            | <a href="/Account/AccountManager.aspx" onfocus="this.blur()" onclick="dhclick('0')">
                                账户管理</a> | 账户充值 | <a href="/ui/userui/bangdingprint.aspx" onfocus="this.blur()" onclick="dhclick('0')">
                                    打印设置</a>
                        </div>
                    </div>
                    <div id="min_nav">
                        平台技术支持电话：010-59059052 | <a href="/Logout.aspx" target="_self" style="color: Black;
                            font-weight: bold;" onfocus="this.blur()">退 出</a><br>
                        <div class="shortcut">
                            建议使用chrome浏览器访问本系统
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="mail-nav" class="">
            <%if (iscanverify == 1)
              { %>
            <div id="mail_write_button">
                <ul>
                    <li class="mail_write"><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()"
                        class="ui-act act-getMailTop"><span class="nav_btn_text">验电子码</span></a> <a href="/V/VerCard.aspx"
                            onfocus="this.blur()" class="ui-exec exec-mail-compose"><span class="nav_btn_text">验会员卡</span></a>
                    </li>
                    <li class="empty-line" style="height: 0px;"></li>
                </ul>
            </div>
            <%} %>
            <div id="all_folder_list">
                <div id="system_folder_list">
                    <ul id="left_folder_list">
                        <asp:Repeater ID="rptTopMenuList" runat="server">
                            <ItemTemplate>
                                <!--<h2 data="<%#Eval("Actioncolumnid") %>">
                                    <%#Eval("Actioncolumnname")%></h2>-->
                                <ul data="<%#Eval("Actioncolumnid") %>">
                                    <asp:HiddenField ID="HideFuncId" runat="server" Value='<%#Eval("Actioncolumnid") %>' />
                                    <asp:Repeater ID="rptMenuList" runat="server">
                                        <ItemTemplate>
                                            <li id="dh_<%#Eval("Actionid") %>" class="app-<%#Eval("Actionid") %>"><a href="<%#Eval("Actionurl") %>"
                                                onfocus="this.blur()" onclick="dhclick('<%#Eval("Actionid") %>')">
                                                <%#Eval("Actionname")%></a> </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <li class="cut-line"></li>
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%--   <li class="" id="folder-1"><a href="/ui/pmui/eticket/eticketindex.aspx" title=""
                            onfocus="this.blur()" class="ui-exec exec-mail-maillist item folder_inbox" target="">
                            <span class="folder_icon"></span><span class="folderName">电脑验码</span></a></li>--%>
                    </ul>
                </div>
            </div>
        </div>
        <div id="mail-main" class="mail-main ">
            <div class="frame">
                <div class="othmailouter" style="display: none">
                    <div id="othermailpop">
                    </div>
                </div>
                <div id="apps-view">
                    <div>
                        <%-- 团期日历Begin--%>
                        <div id="divLine">
                            <div class="title" style="padding: 15px 15px 15px 0; font-size: 15px; color: Black;">
                                <span><b><a href="/ui/pmui/ProductAdd.aspx?proid=<%=lineid%>">基本信息</a></b>
                                    <%if (ServerType == 2 || ServerType == 8)
                                      { %>
                                    <b><a href="/ui/pmui/travel/linetrip.aspx?lineid=<%=lineid%>">详细行程</a></b> <b><a
                                        href="/ui/pmui/travel/linegroupdate.aspx?lineid=<%=lineid%>">出团日期</a></b> <b><a href="/ui/pmui/productlist.aspx">
                                            产品列表</a></b>
                                    <%}
                                      if (ServerType == 9)
                                      {
                                    %>
                                    <b><a href="/ui/pmui/travel/linegroupdate.aspx?lineid=<%=lineid%>">房态信息</a></b>
                                    <b><a href="/ui/pmui/productlist.aspx">产品列表</a></b>
                                    <%
                                      } %>
                                </span>
                            </div>
                            <h2 style="font-size: 15px; margin-bottom: 10px; font-weight: bold;">
                                <%=linename %></h2>
                            <h4>
                                点击日历上的日期，开始选择线路团期：</h4>
                            <p class="sr" style="padding: 10px 15px;">
                            </p>
                            <div class="mission">
                                <div id="datepicker" class="tip-hook lf">
                                </div>
                                <div id="dateselect">
                                </div>
                                <input id="hid_seleddate" type="hidden" value="" autocomplete="off" />
                                <input type="hidden" id="hidLeavingDate" value="" runat="server" />
                                <input type="hidden" id="hidinitLeavingDate" value="" runat="server" />
                                <input type="hidden" id="hid_lineid" value="<%=lineid %>" />
                            </div>
                        </div>
                        <div id="dateline">
                            <table>
                                <tbody id="tbody1" style="color: #656565;">
                                    <%--<tr>
                    <td colspan="2">
                        2014-03-05 当日价格:<input type="text" id="dayprice20140305" value="1">当日空位:<input type="text"
                            id="emptynum20140305" value="1">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        2014-03-06 当日价格:<input type="text" id="dayprice20140306" value="1">当日空位:<input type="text"
                            id="emptynum20140305" value="1">
                    </td>
                </tr>--%>
                                </tbody>
                            </table>
                        </div>
                        <%--  <div style=" font-size:15px; font-weight:bold; color:#006BB7;">
                        分销返还默认用基本信息中设定的，如果没有特殊情况无需再另外设定
                        </div>--%>
                        <div>
                            <input type="button" id="btnclick" onclick="upgroupdate()" value="提 交" class="a_anniu" />
                        </div>
                        <%-- 团期日历End--%>
                    </div>
                </div>
                <!--<div class="copyLine"></div>-->
            </div>
            <div id="mail-msg-fixed" style="z-index: 2;">
                <div id="mail-msg-outer">
                    <div id="mail-msg-inner">
                    </div>
                </div>
            </div>
            <div id="mail-msg">
                <div id="gmsg">
                </div>
                <div id="progress" style="visibility: hidden;">
                    数据加载中...</div>
                <div id="dialog">
                </div>
            </div>
        </div>
        <div id="divBackToTop" style="display: none">
        </div>
        <div class="sessionToken">
        </div>
        <input type="hidden" id="hid_comid" value="<%=comid %>" />
        <input type="hidden" id="hid_comname" value="<%=comname %>" />
        <input type="hidden" id="hid_userid" value="<%=userid %>" />
        <input type="hidden" id="hid_comlogo" value="<%=comlogo %>" />
        <!--直销价格-->
        <input type="hidden" id="hid_adviseprice" value="<%=adviseprice %>" />
        <input type="hidden" id="hid_servertype" value="<%=ServerType %>" />
        <!--判断当日团期是否加载完-->
        <input type="hidden" id="hid_isloaded" value="1">
    " />
    </form>
</body>
</html>
