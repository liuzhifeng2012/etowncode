<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineGroupDate.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.LineGroupDate" %>

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
                    $('#dateselect').append('<span class="category-visited1"><a href="javascript:void(0)" class="catico" onclick="selectDate(this)">' + array[i] + '</a><span class="catico1" onclick="removeDate(this)">x　</span></span>');
                }
            }

            $('#datepicker').datepicker({
                minDate: +1,
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
                                $('#dateselect').append('<span class="category-visited1"><a href="javascript:void(0)" class="catico" onclick="selectDate(this)">' + dateSort[i] + '</a><span class="catico1" onclick="removeDate(this)" style="">x </span></span>');
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
                $(obj).parent().remove();
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
            var seleddate = $("#hid_seleddate").val();
            var LeavingDate = $("#hidLeavingDate").val();
            var initLeavingDate = $("#hidinitLeavingDate").val();

            if (seleddate.indexOf(daydate) == -1) {//判断是否在选中的日期中  
                if ($('#hid_seleddate').val() == '') {
                    $('#hid_seleddate').val(daydate);
                } else {
                    $('#hid_seleddate').val($('#hid_seleddate').val() + ',' + daydate);
                }


                //判断是否在线路的团期中
                if (initLeavingDate.indexOf(daydate) == -1) {//不存在线路团期中，直接添加新的团期
                    $("#tbody1").append('<tr id="tr' + daydate + '"><td colspan="2">' +
                        daydate + ' 当日价格:<input type="text" id="dayprice' + daydate + '" value="0">当日空位:<input type="text"' +
                            'id="emptynum' + daydate + '" value="0">' +
                    '</td>' +
                '</tr>');
                }
                else {//存在于线路团期中，查询团期中当天情况
                    $.post("/JsonFactory/ProductHandler.ashx?oper=GetLineDayGroupDate", { daydate: daydate, lineid: $("#hid_lineid").val() }, function (dataa) {
                        dataa = eval("(" + dataa + ")");
                        if (dataa.type == 1) {

                        }
                        if (dataa.type == 100) {
                            $("#tbody1").append('<tr id="tr' + daydate + '"><td colspan="2">' +
                        daydate + ' 当日价格:<input type="text" id="dayprice' + daydate + '" value="' + dataa.msg.Menprice + '">当日空位:<input type="text"' +
                            'id="emptynum' + daydate + '" value="' + dataa.msg.Emptynum + '">' +
                    '</td>' +
                '</tr>');
                        }
                    })
                }
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

//            if ($("#hid_seleddate").val() == "") {
//                alert("请编辑团期后提交");
//                return;
//            }
            var dayprice = '';
            var emptynum = '';

            var dateTimearr = $("#hid_seleddate").val().split(',');
            if (dateTimearr != null && dateTimearr.length > 0) {
                for (var i = 0; i < dateTimearr.length; i++) {
                    dayprice += $("#dayprice" + dateTimearr[i]).val() + ",";
                    emptynum += $("#emptynum" + dateTimearr[i]).val() + ",";


                }
                dayprice = dayprice.substr(0, dayprice.length - 1);
                emptynum = emptynum.substr(0, emptynum.length - 1);
            }

            $.post("/JsonFactory/ProductHandler.ashx?oper=Uplinegroupdate", { initdatestr: $("#hidinitLeavingDate").val(), datestr: datestr, dayprice: dayprice, emptynum: emptynum, lineid: lineid }, function (dataa) {
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
            width: 505px;
            padding-top: 300px;
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divLine">
        <p class="sr">
            直接点击日历上的日期，即可选择机票执行日期：</p>
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
            <tbody id="tbody1" style="background-color: rgb(239, 239, 239);">
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
    <div>
        <input type="button" id="btnclick" onclick="upgroupdate()" value="提 交" />
    </div>
    </form>
</body>
</html>
