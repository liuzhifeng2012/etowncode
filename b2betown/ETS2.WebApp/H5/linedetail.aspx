<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="linedetail.aspx.cs" Inherits="ETS2.WebApp.H5.linedetail" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="/Styles/h5/travel.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/h5/chutuandate.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/fontcss/font-awesome.min.css" />
    <!--[if IE 7]>
		  <link rel="stylesheet" href="source/module/admin/template/assets/css/font-awesome-ie7.min.css" />
    <![endif]-->
    <title>
        <%=pro_name%></title>
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <meta name="keywords" content="" />
    <meta name="description" content="" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta name="HandheldFriendly" content="true" />
    <meta name="MobileOptimized" content="width" />
    <meta id="viewport" content="width=device-width, user-scalable=yes,initial-scale=1"
        name="viewport" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <link rel="stylesheet" type="text/css" href="/Styles/h5/detail-scss.css" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="/Scripts/common.js"></script>
    <link href="/Scripts/JUI/jquery-rili.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <style id="tnCalenderStyle">
         /*以下一行代码是日历选中样式*/
        .ui-datepicker-current-day {
            color: #FFF !important;
            background: none repeat scroll 0% 0% #FF6000;
            background-color: #0099FF !important;
            text-align:center;
        }
        .ui-datepicker-current-day a
        {
             background-color: #FF6000 !important;
        }
        
        .tn-calendar
        {
            width: 100%;
            overflow: hidden;
        }
        .tn-c-header
        {
            text-align: center;
            height: 75px;
            line-height: 75px;
            font-size: 1.4rem;
            color: #333;
        }
        .tn-c-header .icon-arrow-right
        {
            float: right;
            padding: 24px;
            color: #ff6000;
        }
        .tn-c-header .icon-arrow-left
        {
            float: left;
            padding: 24px;
            color: #ff6000;
        }
        .tn-c-title
        {
        }
        .tn-c-body
        {
            clear: both;
        }
        .tn-c-body table, .tn-c-body tbody
        {
            width: 100%;
        }
        .tn-c-body table th
        {
            text-align: center;
            height: 48px;
            vertical-align: middle;
            background: #dfdfdf;
            font-size: 1.2rem;
            color: #666;
        }
        .tn-c-body table td
        {
            text-align: center;
            height: 80px;
            vertical-align: middle;
            border: 2px solid #d0d0d0;
            font-size: 1.5rem;
            color: #ff6000;
            width: 14.3%;
            font-family: "Arial";
        }
        .tn-c-body table td span.rmb
        {
            font-family: "微软雅黑";
        }
        .tn-c-body table td span.price
        {
            font-size: 1.2rem;
        }
        .tn-c-body table td.disabled
        {
            color: #999;
        }
        .tn-c-body table td.normal
        {
            color: #ff6000;
        }
        .tn-c-body table td.selected
        {
            color: #ffffff;
            background: #ff6000;
        }
        .tn-c-body table tr:nth-child(2) td
        {
            border-top: none;
        }
        .tn-c-body table tr td:first-child
        {
            border-left: none;
        }
        .tn-c-body table tr td:last-child
        {
            border-right: none;
        }
        .ui-datepicker-current-day
        {
            color: #FFF !important;
            background: none repeat scroll 0% 0% #FF6000;
            background-color: #FF6000 !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {


            //加载日历
            $(document).ready(function () {

                $("#calendar").datepicker({
                    minDate: $("#hidMinLeavingDate").val(),
                    onSelect: function (dateText) {
                        $("#outdate").html(dateText);
                        var datearr = $("#hidLeavingDate").val().split(",");
                        var pricearr = $("#hidLinePrice").val().split(",");
                        var childreduce = $("#hidchildreduce").val();
                        var emptyarr = $("#hidEmptyNum").val().split(",");
                        for (var i = 0; i < datearr.length; i++) {
                            if (datearr[i] == dateText) {
                                $("#adultPrice").html(pricearr[i]);
                                $("#childPrice").html(parseInt(pricearr[i]) - parseInt(childreduce));
                                $("#" + dateText).addClass("selected"); //对选中的增加
                            } else {
                                //$("#" + datearr[i]).removeClass("selected"); //对未选中的日期移除
                            }

                        }

                    },
                    beforeShowDay: function (date) {

                        var dt = formatDate(date);
                        var data = [];
                        var dateTimearr = $("#hidLeavingDate").val().split(',');
                        if (dateTimearr != null && dateTimearr.length > 0) {
                            data = $.merge(dateTimearr, data);
                        }

                        var price = [];
                        var pricearr = $("#hidLinePrice").val().split(',');
                        if (pricearr != null && pricearr.length > 0) {
                            price = $.merge(pricearr, price);
                        }

                        var empty = [];
                        var emptyarr = $("#hidEmptyNum").val().split(',');
                        if (emptyarr != null && emptyarr.length > 0) {
                            empty = $.merge(emptyarr, empty);
                        }

                        var dayprice = '';
                        var dayempty = '';

                        var result = false;
                        $(data).each(function (i, n) {
                            if (n == dt) {
                                result = true;
                                dayprice = price[i];
                                dayempty = empty[i];
                            }
                        });

                        if (result) {
                            return [true, "hasCourse", "<p>￥" + dayprice + "</p><p style=\"font-size:13px;color:black;\">余" + dayempty + "</p>"];

                        } else {
                            return [false, "noCourse", '无团期'];
                        }
                    }
                });
            });

        })
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


    </script>
    <script type="text/javascript">
        //计时器         
        function timer(intDiff) {
            window.setInterval(function () {
                var day = 0,
                                hour = 0,
                                minute = 0,
                                second = 0; //时间默认值

                if (intDiff > 0) {
                    day = Math.floor(intDiff / (60 * 60 * 24));
                    hour = Math.floor(intDiff / (60 * 60)) - (day * 24);
                    minute = Math.floor(intDiff / 60) - (day * 24 * 60) - (hour * 60);
                    second = Math.floor(intDiff) - (day * 24 * 60 * 60) - (hour * 60 * 60) - (minute * 60);
                } else {
                    qinggou();
                }
                if (minute <= 9) minute = '0' + minute;
                if (second <= 9) second = '0' + second;
                $('#day_show').html(day + "天");
                $('#hour_show').html('<s id="h"></s>' + hour + '时');
                $('#minute_show').html('<s></s>' + minute + '分');
                $('#second_show').html('<s></s>' + second + '秒');
                intDiff--;
            }, 1000);
        }
        function qinggou() {
            $("#qianggoujishi").html("火热抢购中..");
            $("#goToOrder").addClass("active");
            $("#hid_dinggou").val("1");
        }
    </script>
    <script type="text/javascript">
    var pageSize = 10; //只显示条数

    $(function () {
        var comid = $("#hid_comid").val();
        var lineid = $("#hid_lineid").val();
       <% 
        // ispanicbuy 抢购
        // panic_begintime  开始时间
        // panicbuy_endtime 抢购结束
        // limitbuytotalnum 抢购总数
        if (ispanicbuy ==1){
        %>
              $('#telConsult').attr('href','#'); 
             <%
                if (panic_begintime <= nowdate && nowdate<panicbuy_endtime){//抢购在有效期范围内
             %>
               $('#telConsult').html('<div style="color: #666;;" id="qianggoujishi">火热抢购中..</div>')
             <%
             }else if(panic_begintime>nowdate){         
             %>
                   $("#goToOrder").removeClass("active"); 
                   $("#hid_dinggou").val("0");
                  
                   $('#telConsult').html('<div style="color: #666; font-size:20px;line-height: 25px; padding-top:30px;" id="qianggoujishi">距开始时间还剩 <br/> <span id="day_show"></span><span id="hour_show"></span><span id="minute_show"></span><span id="second_show"></span></div>')
                   var jishicount=$('#hid_jishicount').val();

                   var intDiff = parseInt(jishicount); //倒计时
                   timer(intDiff);
             <%
              }else{
             %>
                $('#telConsult').html('<div style="color: #666;;" id="qianggoujishi">抢购已结束</div>')
                $("#goToOrder").removeClass("active"); 
                $("#hid_dinggou").val("0");
              <%} 
              %>
        <%} 
        %>

        //加载 ;
        $.ajax({
            type: "post",
            url: "/JsonFactory/ProductHandler.ashx?oper=GetLineById",
            data: { lineid: lineid },
            async: false,
            success: function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("查询错误");
                    return;
                }
                if (data.type == 100) {

                    //行程处理
                    var linestr = "";
                    if (data.msg != null) {

                        for (var i = 0; i < data.msg.length; i++) {
                            linestr += "<li>  <time class=\"tmtime\" datetime=\"\">DAY" + (i + 1) + "</time><div class=\"tmcircle\"></div><div class=\"tmlabel\">" + data.msg[i].ActivityArea + "&nbsp;" + data.msg[i].Traffic + " </div></li>";
                        }
                        $(".timeline").html(linestr);

                    }

                    //団期处理
                    var datestr = "";
                    var datestr_zhehang = "<br>";
                    var firstdate = "";
                    var listdate = "";
                    var listprice = "";
                     var listempty = "";
                    if (data.linedate != null) {
                        for (var i = 0; i < data.linedate.length; i++) {
                            if (((i + 1) % 4) == 0) {
                                datestr_zhehang = "<br>";
                            } else {
                                datestr_zhehang = "";
                            }
                            datestr += ChangeDate_MDFormat(data.linedate[i].Daydate) + "," + datestr_zhehang;

                            if (i == 0) {
                                firstdate = ChangeDateFormat(data.linedate[i].Daydate);
                            }

                            listdate += ChangeDateFormat(data.linedate[i].Daydate) + ",";

                            listprice += data.linedate[i].Menprice + ","
                            listempty += data.linedate[i].Emptynum + ",";
                        }
                        $("#dateinfo").html(datestr); //显示団期
                        $("#hidLeavingDate").val(listdate); //日期列表
                        $("#hidMinLeavingDate").val(firstdate); //第一天报价
                        $("#hidLinePrice").val(listprice); //价格列表
                        $("#hidEmptyNum").val(listempty); //空位列表
                        // $("#outdate").html(firstdate);//默认选择日期

                    }
                }
            }
        })


        $("#adult-add").click(function () {
            var count = parseFloat($("#adultnum").val());
            $("#adultnum").val(count + 1)
            //单房差显示
            if ($("#adultnum").val() % 2 == 0) {
                //$("#supplement").addClass("hide");
            } else {
                //$("#supplement").removeClass("hide");
            }

        });

        $("#adult-reduce").click(function () {
            var count = parseFloat($("#adultnum").val());
            if (count > 1) {
                $("#adultnum").val(count - 1)
            } else {
                $("#adultnum").val(1)
            }
            //单房差显示
            if ($("#adultnum").val() % 2 == 0) {
                //$("#supplement").addClass("hide");
            } else {
                //$("#supplement").removeClass("hide");
            }
        });

        $("#child-add").click(function () {
            var count = parseFloat($("#childnum").val());
            $("#childnum").val(count + 1)
        });

        $("#child-reduce").click(function () {
            var count = parseFloat($("#childnum").val());
            if (count > 0) {
                $("#childnum").val(count - 1)
            } else {
                $("#childnum").val(0)
            }
        });


        $("#goToOrder").click(function () {
            var adultnum = $("#adultnum").val();
            var childnum = $("#childnum").val();
            var outdate = $("#outdate").html();
            var lineid = $("#hid_lineid").val();
            var dinggou = $("#hid_dinggou").val();
            if (dinggou == "1"){
                if (outdate == "" || outdate == "--") {

                    $("#body").append("<div style=\"background-color: rgba(0, 0, 0, 0.6); color: rgb(255, 255, 255); border: medium none; border-radius: 5px; text-align: center; top: 245.333px;\" class=\"dialog-box show\"><div style=\"text-align: center;\" class=\"dialog-body\"><span class=\"\"></span><span style=\"display: inline-block;\" class=\"dialog-message\">请选择出游日期！</span></div></div>");
                    $(".dialog-box").show(300).delay(2000).hide(1000);
                    $("#showDate").click();
                    return;
                }

                location.href = "lineBook.aspx?lineid=" + lineid+"&adultnum="+adultnum+"&childnum="+childnum+"&outdate="+outdate;
            }
        });

    })


    </script>
    <script type="text/javascript">
        //手机显示控制
        var viewPortScale;
        var dpr = window.devicePixelRatio;
        viewPortScale = 0.5;
        //
        var detectBrowser = function (name) {
            if (navigator.userAgent.toLowerCase().indexOf(name) > -1) {
                return true;
            } else {
                return false;
            }
        };
        if (detectBrowser('ipad')) {
            document.getElementById('viewport').setAttribute(
        'content', 'width=device-width, user-scalable=no,initial-scale=1');
        } else if (detectBrowser('ucbrowser')) {
            document.getElementById('viewport').setAttribute(
        'content', 'user-scalable=no, width=device-width, minimum-scale=0.5, initial-scale=' + viewPortScale);
        } else if (detectBrowser('360browser')) {
            document.getElementById('viewport').setAttribute(
        'content', 'target-densitydpi=320,user-scalable=no, width=640, minimum-scale=1, initial-scale=1');
        } else {
            document.getElementById('viewport').setAttribute(
        'content', 'target-densitydpi=320, user-scalable=no,width=640, minimum-scale=0.5, initial-scale=' + viewPortScale);
        }

    
    </script>
    <script type="text/javascript">
        $(function () {//団期日历
            $("#showDate").click(function () {
                if ($('#showDate').hasClass('no-date')) return;
                var container = $('#orderTime');
                var header = $('#dataHeader');
                container.css('height', $('body').height() + 'px');
                container.toggleClass('hide');
                header.css('position', 'fixed');
                var isOpera = !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
                var isChrome = !!window.chrome && !isOpera;
                if (!isChrome && !isOpera) {
                    header.css('left', '0');
                }
                window.scrollTo(0, 0);
            })

            $("#dateBack").click(function () {
                var container = $('#orderTime');
                container.toggleClass('hide');
            })
        })
 
    </script>
</head>
<body id="body">
    <div class="header-box" id="header">
        <div class="header clearfix" id="topHeader">
            <div class="back_icon_control" id="mainBack">
                <!--<span class="icon-arrow-left"></span>-->
            </div>
            <div class="right_icon_control" id="Div1">
                <%if (Wxfocus_url != "")
                  { %>
                <a href="<%=Wxfocus_url %>"><span style="font-size: 16px;">
                    <%=Wxfocus_author%></span></a>
                <%} %>
            </div>
            <div class="title with-desc">
                <div class="name">
                    线路详情</div>
                <div class="desc">
                    线路编号：<%=travelproductid %></div>
            </div>
        </div>
    </div>
    <div class="wrapper" id="wrapper">
        <div id="index">
            <!-- gallery start -->
            <div class="bueaty-head" style="background-image: url(<%=imgaddress%>)">
                <div class="info-bar-back">
                </div>
                <div class="info-bar">
                    <i class="icon  on"></i><span>
                        <%if (server_type == 2)
                          {%>跟团旅游<%} %></span> <span class="separator">
                              <%=travelstarting %>出发</span><!-- <i class="icon icon-star fr" id="collectIcon">-->
                    </i>
                </div>
            </div>
            <!-- gallery end -->
            <div class="main">
                <!--  产品名称开始 -->
                <h2 class="product-title">
                    <%=pro_name %></h2>
                <div class="price-box">
                    <section class="price-detail">
            <span class="highlight">促销价：
                        <span class="price">￥<span class="em"><%=advise_price%></span></span></span><del class="price">￥<%=face_price%></del>
                    </section>
                    <hr>
                </div>
                <!--  产品名称结束 -->
                <div class="info-box cell-margin" id="showDate">
                    <div class="info-row">
                        <div class="info-cell title">
                            <p>
                                <i class="icon icon-calendar"></i>
                            </p>
                            <p>
                                点击选择团期</p>
                        </div>
                        <div class="info-cell content" id="departureBox">
                            <span id="dateinfo" style="word-break: break-all;color:#666666;"></span>
                            <div class="arrow">
                            </div>
                        </div>
                    </div>
                </div>
                <a href="lineTrip.aspx?lineid=<%=lineid %>">
                    <div class="info-box vertical cell-margin">
                        <div class="info-row">
                            <div class="info-cell title">
                                <i class="icon icon-plane"></i><span>点击查看行程详情</span>
                            </div>
                        </div>
                        <div class="info-row">
                            <div class="info-cell content">
                                <ul class="timeline">
                                </ul>
                                <div class="arrow">
                                </div>
                            </div>
                        </div>
                    </div>
                </a>
                <!-- 产品特色 start -->
                <ul class="spec-detail">
                    <li><span class="spec-title"><span class="icon-shopping-cart"></span>产品特色</span>
                        <p class="spec-p">
                            <%=pro_Remark%></p>
                    </li>
                    <li><span class="spec-title"><span class="icon-comments"></span>包含服务</span>
                        <p class="spec-p">
                            <%=service_Contain%></p>
                    </li>
                    <li><span class="spec-title"><span class="icon-comment"></span>不包含服务/自费项目</span>
                        <p class="spec-p">
                            <%=service_NotContain%></p>
                    </li>
                    <li><span class="spec-title"><span class="icon-exclamation-sign"></span>重要提示</span>
                        <p class="spec-p">
                            <%=precautions%></p>
                    </li>
                </ul>
            </div>
            <div class="btn-box fixed">
                <a href="tel:<%=tel %>" class="tel-consult" id="telConsult"><i class="icon-phone"></i>
                    电话咨询</a> <a href="javascript:void(0)" class="btn active fr middle" id="goToOrder"><i
                        class="icon-legal"></i>立即预订</a>
            </div>
            <!-- 弹出层开始 -->
            <div class="float-box hide " id="orderTime" style="height: 4194px;">
                <div class="header-box">
                    <div class="header" id="dataHeader" style="position: fixed;">
                        <div class="back_icon_control" id="dateBack">
                            <i class="icon-arrow-left"></i><a href="javascript:void(0);" class="back_icon_info"></a></div>

                    </div>
                </div>
                <div class="date-box">
                    <div class="calendar" id="calendar" lang="zh-cn">
                    </div>
                    <div class="date-detail" id="dateDetail">
                        <section>
                <span class="half">
                    <span id="adultNumObj" class="ver-m btn-plus-minus"><span id="adult-reduce">-</span><input readonly="readonly" value="2" type="text" id="adultnum"><span id="adult-add">+</span></span>成人
                </span>
                <span id="childTips">
                    <span id="childNumObj" class="ver-m btn-plus-minus"><span id="child-reduce">-</span><input readonly="readonly" value="0" type="text" id="childnum"><span id="child-add">+</span></span>
                    <span id="childTips" class="qa-tips-sec qa" title="儿童价标准：身高0.8-1.2米（含），不占床，只含车位费、导游服务费；其他费用自理。">儿童<i class="icon-question"></i>
                    <div id="qaTipsContentSec" class="qa-sec"><div class="tooltip"><div style="left: 565px;" class="arrow"></div><div class="content-box"><p class="tips-content">儿童价标准：身高0.8-1.2米（含），不占床，只含车位费、导游服务费；其他费用自理。</p></div></div></div></span></span>
            </section>
                        <section>
                <span class="half">
                    成人价： <label class="highlight" id="adultPrice">--</label> 元
                </span>
                <span>
                    儿童价： <label class="highlight" id="childPrice">--</label> 元
                </span>
            </section>
                        <section>
                <span >
                    出行日期： <label class="highlight" id="outdate">--</label>
                </span>
            </section>
                        <section class="hide" id="supplement">
                <span>
                    单房差：
                    <label class="highlight" id="dfNum">--</label>
                    元/人
                </span>
                <span class="extra">(成人为单数时补双人房费)</span>
            </section>
                        <p class="extra hide" id="diyTip">
                            (儿童暂按成人价计算，客服确认后会根据房间数量调整总价)</p>
                    </div>
                    <div class="date-tips qa-tips-sec qa" title="1. 选择出游日期和出游人数\n2. 填写您的姓名和手机号，提交订单\n3. 等待客服与您联系，确认优惠后的订单金额\n4. 网上付款或前往当地门市付款，享受开心出游">
                    </div>
                </div>
            </div>
            <!-- 弹出层结束 -->
            <p class="back-top">
                <a href="#header">回到顶部</a>
            </p>
            <div class="placeholder">
            </div>
        </div>
    </div>
    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=imgaddress%>",
                desc: "<%=pro_Remark%>",
                title: '<%=pro_name %> ￥<%=advise_price%>'
            });
        });
    </script>
    <input type="hidden" id="hid_lineid" value="<%=lineid %>" />
    <input type="hidden" id="hidLeavingDate" value="" runat="server" />
    <input type="hidden" id="hidMinLeavingDate" value="" runat="server" />
    <input type="hidden" id="hidLinePrice" value="" runat="server" />
    <input type="hidden" id="hidEmptyNum" value="" runat="server" />
    <input type="hidden" id="hidchildreduce" value="<%=childreduce %>" />
    <input type="hidden" id="hid_dinggou" value="1" />
    <input type="hidden" id="hid_jishicount" value="<%=shijiacha %>" />
</body>
</html>
