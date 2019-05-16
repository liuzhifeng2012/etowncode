<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eticket_useset.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.eticket_useset" %>

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


            //获取电子票使用规则
            $.post("/JsonFactory/ProductHandler.ashx?oper=Geteticket_usesetlist", { comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    $("input:checkbox[name='cb_ordinarydays']").attr("checked", false);
                    $("input:checkbox[name='cb_weekdays']").attr("checked", false);
                    $("input:checkbox[name='cb_holidays']").attr("checked", false);

                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i].datetype == 0) {

                            $("input:checkbox[name='cb_ordinarydays'][value='" + data.msg[i].etickettype + "']").attr("checked", true);
                        }
                        if (data.msg[i].datetype == 1) {
                            $("input:checkbox[name='cb_weekdays'][value='" + data.msg[i].etickettype + "']").attr("checked", true);
                        }
                        if (data.msg[i].datetype == 2) {
                            $("input:checkbox[name='cb_holidays'][value='" + data.msg[i].etickettype + "']").attr("checked", true);
                        }
                    }
                }
            })


           
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
            $.post("/JsonFactory/ProductHandler.ashx?oper=Delblackoutdate", { comid: $("#hid_comid").val(), daydate: daydate }, function (data) {
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


                //判断是否在设定的特定日期中
                if (initLeavingDate.indexOf(daydate) == -1) {//不存在，直接添加新的 
                    $("#tbody1").append('<tr id="tr' + daydate + '"><td colspan="2">' +
                        daydate + ' &nbsp;&nbsp;日期类型: <label><input name="datetype' + daydate + '" type="radio" value="0" checked="checked" />平日</label><label><input name="datetype' + daydate + '" type="radio" value="1" />周末</label><label><input name="datetype' + daydate + '" type="radio" value="2"   />节假日</label>&nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                    '</td>' +
                '</tr>');
                }
                else {//存在，查询当天情况
                    $.post("/JsonFactory/ProductHandler.ashx?oper=Getblackoutdate", { daydate: daydate, comid: $("#hid_comid").val() }, function (dataa) {
                        dataa = eval("(" + dataa + ")");
                        if (dataa.type == 1) {

                        }
                        if (dataa.type == 100) {
                            var ht = '<tr id="tr' + daydate + '"><td colspan="2">' + daydate + '&nbsp;&nbsp;日期类型:';
                            if (dataa.msg.datetype == 0) {
                                ht += '<label><input name="datetype' + daydate + '" type="radio" value="0" checked="checked" />平日</label><label><input name="datetype' + daydate + '" type="radio" value="1" />周末</label><label><input name="datetype' + daydate + '" type="radio" value="2"   />节假日</label>';
                            }
                            if (dataa.msg.datetype == 1) {
                                ht += '<label><input name="datetype' + daydate + '" type="radio" value="0" />平日</label><label><input name="datetype' + daydate + '" type="radio" value="1" checked="checked" />周末</label><label><input name="datetype' + daydate + '" type="radio" value="2"   />节假日</label>';
                            }
                            if (dataa.msg.datetype == 2) {
                                ht += '<label><input name="datetype' + daydate + '" type="radio" value="0"  />平日</label><label><input name="datetype' + daydate + '" type="radio" value="1" />周末</label><label><input name="datetype' + daydate + '" type="radio" value="2"   checked="checked"  />节假日</label>';
                            }

                            ht += '&nbsp;&nbsp; <a href="javascript:void(0)" onclick="deletetqq(\'' + daydate + '\')" class="a_anniu">删除</a>' +
                          '</td>' +
                               '</tr>';

                            $("#tbody1").append(ht);
                        }
                    })
                }
            }
        }
        function deletetqq(ddealdate) {

            if (confirm("确定删除此日期吗？")) {

                var className = $("#spanrr_" + ddealdate).parent().find("a").html();
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
            var comid = $("#hid_comid").val();
            var datestr = $("#hid_seleddate").val();

            var ordinaryday_etickettypes = "";
            var weekday_etickettypes = "";
            var holiday_etickettypes = "";
            $("input:checkbox[name='cb_ordinarydays']:checked").each(function () {
                ordinaryday_etickettypes += $(this).val() + ",";
            });
            $("input:checkbox[name='cb_weekdays']:checked").each(function () {
                weekday_etickettypes += $(this).val() + ",";
            });
            $("input:checkbox[name='cb_holidays']:checked").each(function () {
                holiday_etickettypes += $(this).val() + ",";
            });
            if (ordinaryday_etickettypes == '') {
                alert("请选择平日可以验证的票种");
                return;
            } else {
                ordinaryday_etickettypes = ordinaryday_etickettypes.substr(0, ordinaryday_etickettypes.length - 1);
            }
            if (weekday_etickettypes == '') {
                alert("请选择周末可以验证的票种");
                return;
            } else {
                weekday_etickettypes = weekday_etickettypes.substr(0, weekday_etickettypes.length - 1);
            }
            if (holiday_etickettypes == '') {
                alert("请选择节假日可以验证的票种");
                return;
            } else {
                holiday_etickettypes = holiday_etickettypes.substr(0, holiday_etickettypes.length - 1);
            }

            if ($("#hid_seleddate").val() == "") {
                alert("请添加特定日期");
                return;
            }

            var datetype = '';

            var dateTimearr = $("#hid_seleddate").val().split(',');
            if (dateTimearr != null && dateTimearr.length > 0) {
                for (var i = 0; i < dateTimearr.length; i++) {
                    datetype += $("input:radio[name='datetype" + dateTimearr[i] + "']:checked").val() + ",";
                }
                datetype = datetype.substr(0, datetype.length - 1);

            }

            $.post("/JsonFactory/ProductHandler.ashx?oper=Upcomblackoutdate", { ordinaryday_etickettypes: ordinaryday_etickettypes, weekday_etickettypes: weekday_etickettypes, holiday_etickettypes: holiday_etickettypes, initdatestr: $("#hidinitLeavingDate").val(), datestr: datestr, datetype: datetype, comid: comid, userid: $("#hid_userid").val() }, function (dataa) {
                dataa = eval("(" + dataa + ")");
                if (dataa.type == 1) {

                }
                if (dataa.type == 100) {
                    $.prompt("调整特定日期成功", {
                        buttons: [
                                 { title: '确定', value: true }
                                ],
                        opacity: 0.1,
                        focus: 0,
                        show: 'slideDown',
                        submit: function (e, v, m, f) { if (v == true) { window.open("/ui/pmui/eticket_useset.aspx", target = '_self') } }
                    });
                }
            })
        }
    </script>
    <style type="text/css">
        #dateline
        {
            width: 505px;
            padding-top: 195px;
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


            if ($("#secondary-tabs").length == 0) {
                //判断虚拟路径是否在子导航列表中，没有的话则 赋值虚拟路径为 cookie中保存的虚拟路径
                var vurl = $("#hid_dVirtualUrl").trimVal();
                var parastr = $("#hid_parastr").trimVal();
                //alert("1-" + vurl + "--" + parastr);
                $.post("/JsonFactory/PermissionHandler.ashx?oper=getsys_subnav", { vurl: vurl, parastr: parastr }, function (data1) {
                    data1 = eval("(" + data1 + ")");
                    if (data1.type == 1) {
                        //alert("2-" + $.cookie($("#hid_comid").val() + "_vurl"));
                        if ($.cookie($("#hid_comid").val() + "_vurl")) {
                            var fullurl = $.cookie($("#hid_comid").val() + "_vurl");
                            //                                alert("88-" + fullurl);
                            if (fullurl != "") {
                                if (fullurl.toLowerCase() != "/manage.aspx" && fullurl.toLowerCase() != "/default.aspx") {
                                    var vvurl = ""; //虚拟路径（不包含参数）
                                    var vparastr = ""; //虚拟参数字符串
                                    if (fullurl.indexOf('?') > -1) {
                                        vvurl = fullurl.substr(0, fullurl.indexOf('?'));
                                        vparastr = fullurl.substr(fullurl.indexOf('?') + 1);
                                    } else {
                                        vvurl = fullurl;
                                        vparastr = "";
                                    }

                                    //                                        alert("554-" + vvurl + "---" + vparastr);
                                    $("#hid_dVirtualUrl").val(vvurl);
                                    $("#hid_parastr").val(vparastr);

                                    viewrightsubnav();
                                } else {//如果是首页或者默认页，则跳过权限

                                    return;
                                }
                            } else {

                                return;
                            }
                        } else {
                            return;
                        }
                    }
                    if (data1.type == 100) {
                        viewrightsubnav();
                    }
                })


            }
        })
        function viewrightsubnav() {
            //                alert($("#hid_dVirtualUrl").trimVal() + "--" + $("#hid_parastr").trimVal());
            //根据虚拟路径得到需要展示的右侧子导航
            $.post("/JsonFactory/PermissionHandler.ashx?oper=getsys_subnavlistbyvirtualurl", { virtualurl: $("#hid_dVirtualUrl").trimVal(), viewcode: 1, groupid: $("#hid_dgroupid").trimVal(), parastr: $("#hid_parastr").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("你暂时没有权限访问");
                    history.go(-1);
                    return;
                }
                if (data.type == 100) {
                    var subnav = "<div id=\"secondary-tabs\" class=\"navsetting\"><ul>";
                    for (var i = 0; i < data.msg.length; i++) {
                        var subnav_url = data.msg[i].subnav_url;
                        //                                if (subnav_url.indexOf('?') > -1) {
                        //                                    subnav_url = subnav_url.substr(0, subnav_url.indexOf('?'));
                        //                                }
                        var subnav_name = data.msg[i].subnav_name;


                        if (subnav_url == $("#hid_dVirtualUrl").trimVal() + $("#hid_parastr").trimVal()) {//设置class="on"
                            subnav += '<li class="on" ><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl(\'' + subnav_url + '\')"><span>' + subnav_name + '</span></a></li>';
                        } else {
                            subnav += '<li  ><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl(\'' + subnav_url + '\')"><span>' + subnav_name + '</span></a></li>';
                        }
                    }
                    subnav += "</ul> </div>";
                    $("#apps-view").prepend(subnav);
                }
            })

        }

        //左侧栏点击事件
        function dhclick(dhid, vurl) {
            $.cookie($("#hid_comid").val() + "_navigationid", dhid, { path: '/' });
            //保存导航路径
            $.cookie($("#hid_comid").val() + "_vurl", vurl.toLowerCase(), { path: '/' });
        }
        //右侧子导航点击事件
        function dbsavevurl(vurl) {

            //保存导航路径
            $.cookie($("#hid_comid").val() + "_vurl", vurl.toLowerCase(), { path: '/' });
            //                alert(vurl.toLowerCase());
            location.href = vurl;
        }

        //退出系统
        function logout() {
            $.cookie($("#hid_comid").val() + "_navigationid", 0, { path: '/' });
            $.cookie($("#hid_comid").val() + "_vurl", "", { path: '/' });
            location.href = "/Logout.aspx";
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
                        平台技术支持电话：010-59059052 | <a href="javascript:void(0)" target="_self" style="color: Black;
                            font-weight: bold;" onfocus="this.blur()" onclick="logout()"><br>
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
           <%-- <div id="secondary-tabs" class="navsetting ">
                <ul>
                    <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                    <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                    <li><a href="/ui/pmui/ProductList.aspx?projectid=<%=projectid %>" onfocus="this.blur()"
                        target=""><span>产品列表</span></a></li>
                    <li><a href="/ui/pmui/ProductServerTypeList.aspx?projectid=<%=projectid %>" onfocus="this.blur()"
                        target=""><span>添加产品</span></a></li>
                    <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                    <li><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                    <li class="on"><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target="">
                        <span>商户特定日期设定</span></a></li>
                          <li><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()" target="">
                    <span>运费模版管理</span></a></li>
                </ul>
            </div>--%>
            <div class="frame">
                <div class="othmailouter" style="display: none">
                    <div id="othermailpop">
                    </div>
                </div>
                <div id="apps-view">
                    <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                        position: relative; z-index: 10;">
                        <div id="div_eticketuseset" style="margin-bottom: 30px;">
                            <h2 class="p-title-area">
                                1.电子票使用规则设定</h4>
                                <div class="mi-form-item">
                                    <div class="mi-label">
                                        平日&nbsp;&nbsp;&nbsp;&nbsp;
                                    </div>
                                    可以验证
                                    <label>
                                        <input type="checkbox" name="cb_ordinarydays" value="0" checked="checked">
                                        平日票
                                    </label>
                                    <label>
                                        <input type="checkbox" name="cb_ordinarydays" value="1"  checked="checked">
                                        周末票
                                    </label>
                                    <label>
                                        <input type="checkbox" name="cb_ordinarydays" value="2"  checked="checked">
                                        节假日票</label>
                                </div>
                                <div class="mi-form-item">
                                    <div class="mi-label">
                                        周末&nbsp;&nbsp;&nbsp;&nbsp;
                                    </div>
                                    可以验证
                                    <label>
                                        <input type="checkbox" name="cb_weekdays" value="0"  >
                                        平日票
                                    </label>
                                    <label>
                                        <input type="checkbox" name="cb_weekdays" value="1" checked="checked">
                                        周末票
                                    </label>
                                    <label>
                                        <input type="checkbox" name="cb_weekdays" value="2" checked="checked">
                                        节假日票</label>
                                </div>
                                <div class="mi-form-item">
                                    <div class="mi-label">
                                        节假日&nbsp;
                                    </div>
                                    可以验证
                                    <label>
                                        <input type="checkbox" name="cb_holidays" value="0">
                                        平日票
                                    </label>
                                    <label>
                                        <input type="checkbox" name="cb_holidays" value="1">
                                        周末票
                                    </label>
                                    <label>
                                        <input type="checkbox" name="cb_holidays" value="2" checked="checked">
                                        节假日票</label>
                                </div>
                        </div>
                        <%-- 团期日历Begin--%>
                        <div id="divLine">
                            <h2 class="p-title-area">
                                2.点击日历上的日期，开始设置商户特定日期：</h2>
                            <div class="mission" style="margin-left: 30px;">
                                <div id="datepicker" class="tip-hook lf">
                                </div>
                                <div id="dateselect">
                                </div>
                                <input id="hid_seleddate" type="hidden" value="" autocomplete="off" />
                                <input type="hidden" id="hidLeavingDate" value="" runat="server" />
                                <input type="hidden" id="hidinitLeavingDate" value="" runat="server" />
                            </div>
                        </div>
                        <div id="dateline" style="margin-left: 30px;">
                            <table>
                                <tbody id="tbody1" style="color: #656565;">
                                    <%--<tr>
                    <td colspan="2">
                        2014-03-05 日期类型: <label>
                            <input name="datetype20140305" type="radio" value="0" checked="checked" />
                            平日
                        </label>
                        <label>
                            <input name="datetype20140305" type="radio" value="1" />
                            周末
                        </label>
                        <label>
                            <input name="datetype20140305" type="radio" value="2"   />
                            节假日
                        </label>
                    </td>
                </tr>
                                    --%>
                                </tbody>
                            </table>
                        </div>
                        <div style="margin-left: 30px;">
                            <input type="button" id="btnclick" onclick="upgroupdate()" value="提 交" class="a_anniu" />
                        </div>
                        <%-- 团期日历End--%>
                        <div class="mi-form-item" style="margin-bottom:20px;">
                            <label class="mi-label">
                                备注：
                            </label>
                            1.电子票使用默认规则<br />
                            平日：可以验证 平日，周末，节假日票；<br />
                            周末：可以验证 周末，节假日票；<br />
                            节假日：只可以验证节假日票 ；<br />
                            2.商户日期类型默认规则 周六，周日 为周末 其他日期为平日。
                        </div>
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
        <input type="hidden" id="hid_userid" value="<%=userid
    %>" />
        <input type="hidden" id="hid_comlogo" value="<%=comlogo %>" />

       <input type="hidden" id="hid_dgroupid" value="<%=groupid %>" />
       <input type="hidden" id="hid_dVirtualUrl" value="<%=VirtualUrl %>" />
        <input type="hidden" id="hid_parastr" value="<%=parastr %>" />
    </div>
    </form>
</body>
</html>
