<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pro_sales.aspx.cs" Inherits="ETS2.WebApp.Agent.m.pro_sales"
    MasterPageFile="/Agent/m/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/H5/Order.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/JUI/jquery-orderrili.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/cart.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/CartPromptDiv.css" rel="stylesheet" type="text/css" />
    <link  href="http://shop.etown.cn/h5/order/css/base.css" rel="stylesheet">  
    <!-- 页面样式表 -->
    <style type="text/css">
        /*行程样式*/
        .container .w-item #Dd1 .ico-right {
            width: 9px;
            height: 13px;
            overflow: hidden;
            position: absolute;
            bottom: 0px;
            right: 2px;
            background: url(/Images/icon_you_com.png) 50% 50% / 9px 13px no-repeat;
            -webkit-transition: -webkit-transform 0.1s ease-in;
            transition: -webkit-transform 0.1s ease-in;
            -webkit-transform: rotate(90deg);
            -webkit-transform-origin-x: 50%;
            -webkit-transform-origin-y: 50%;
            -webkit-transform-origin-z: initial;
            }
        /*以下一行代码是日历选中样式*/
        .ui-datepicker-current-day
        {
            color: #FFF !important;
            background: none repeat scroll 0% 0% #FF6000;
            background-color: #0099FF !important;
            text-align: center;
        }
        .ui-datepicker td
        {
            text-align: center;
        }
        
        .none
        {
            display: none;
        }
        input:-moz-placeholder
        {
            color: #DDE;
        }
        input::-webkit-input-placeholder
        {
            color: #DDE;
        }
        .qb_icon
        {
            display: inline-block;
            background-repeat: no-repeat;
            background-size: 100%;
        }
        .qb_clearfix:after
        {
            clear: both;
            content: ".";
            display: block;
            height: 0;
            visibility: hidden;
        }
        .qb_mb10
        {
            margin-bottom: 10px;
        }
        .qb_mb20
        {
            margin-bottom: 20px;
        }
        .qb_mr10
        {
            margin-right: 10px;
        }
        .qb_pt10
        {
            padding-top: 10px;
        }
        .qb_fs_s
        {
            font-size: 12px;
        }
        .qb_fs_m, body
        {
            font-size: 14px;
        }
        .qb_fs_l
        {
            font-size: 15px;
        }
        .qb_fs_xl
        {
            font-size: 17px;
        }
        .mod_btn
        {
            font-size: 34px;
        }
        .qb_fr
        {
            float: right;
        }
        .qb_gap
        {
            padding-left: 10px;
            padding-right: 10px;
            margin-bottom: 10px;
        }
        .qb_none
        {
            display: none !important;
        }
        .qb_tac
        {
            text-align: center;
        }
        .qb_tar
        {
            text-align: right;
        }
        .qb_flex
        {
            display: -webkit-box;
        }
        .qb_flex .flex_box
        {
            -webkit-box-flex: 1;
            display: block;
        }
        .qb_hr
        {
            border: 0;
            border-top: 1px solid #eeeff0;
            border-bottom: 1px solid #FFF;
            margin: 20px 0;
            clear: both;
        }
        .qb_quick_tip
        {
            position: fixed;
            height: 23px;
            padding: 3px 5px;
            background: rgba(0,0,0,.8);
            color: #FFF;
            border-radius: 5px;
            text-align: center;
            z-index: 202;
            top: 5px;
            left: 10px;
            right: 10px;
        }
        .icon_checkbox, .pg_upgrade_title li, .mod_input .x
        {
            background-image: url('/Images/sprite_upgrade.png');
            background-size: 25px;
            background-repeat: no-repeat;
        }
        .pg_upgrade_title li
        {
            background-position: -8px -169px;
            padding-left: 30px;
        }
        .pg_upgrade_content .icon_checkbox
        {
            background-position: 0 -40px;
            width: 25px;
            height: 25px;
            vertical-align: -8px;
            margin-right: 5px;
        }
        .pg_upgrade_content .icon_checkbox.checked
        {
            background-position: 0 0;
        }
        .mod_input
        {
            background-color: #FFF;
            border: 1px solid #c5c5c5;
            border-radius: 5px;
            padding: 2px 10px;
            line-height: 37px;
            box-shadow: 0 0 2px rgba(0,0,0,.2) inset;
            position: relative;
        }
        .qb_mb10
        {
            margin-bottom: 10px;
        }
        .pg_upgrade_content .icon_checkbox
        {
            background-position: 0 -40px;
            width: 25px;
            height: 25px;
            vertical-align: -8px;
            margin-right: 5px;
        }
        .pg_upgrade_content .icon_checkbox.checked
        {
            background-position: 0 0;
        }
        .qb_gap
        {
            padding-left: 10px;
            padding-right: 10px;
            margin-bottom: 10px;
        }
        
        .tdleft
        {
            float: left;
            width: 25%;
        }
        .tdright
        {
            position: relative;
            overflow: hidden;
            padding: 0 15px 0 5px;
            display: block;
        }
        .in-item table input[type="text"]
        {
            width: 100%;
            margin-right: -15px;
            height: 44px;
            border: 0px;
            border-image-source: initial;
            border-image-slice: initial;
            border-image-width: initial;
            border-image-outset: initial;
            border-image-repeat: initial;
            background: 0px 50%;
            color: rgb(26, 158, 217);
            outline: 0px;
            -webkit-box-shadow: none;
            border-radius: 0px;
        }
        .in-item table input[type="tel"]
        {
            width: 100%;
            margin-right: -15px;
            height: 44px;
            border: 0px;
            border-image-source: initial;
            border-image-slice: initial;
            border-image-width: initial;
            border-image-outset: initial;
            border-image-repeat: initial;
            background: 0px 50%;
            color: rgb(26, 158, 217);
            outline: 0px;
            -webkit-box-shadow: none;
            border-radius: 0px;
        }
        
        .btn_code
        {
            line-height: 30px;
            display: inline-block;
            padding: 0 5px;
            color: #6d6d6d;
            border: 1px solid #d1d1d1;
            background-color: #e6e6e6;
            background-image: -webkit-gradient(linear,left top,left bottom,from(#f8f8f8),to(#e6e6e6));
            background-image: -webkit-linear-gradient(top,#f8f8f8,#e6e6e6);
            background-image: linear-gradient(to bottom,#f8f8f8,#e6e6e6);
            border-radius: 5px;
        }
        .btn_code .disabled
        {
            color: #FFF;
            background-color: #c3c3c3;
            background-image: -webkit-gradient(linear,left top,left bottom,from(#d0d0d0),to(#c3c3c3));
            background-image: -webkit-linear-gradient(top,#d0d0d0,#c3c3c3);
            background-image: linear-gradient(to bottom,#d0d0d0,#c3c3c3);
        }
    </style>
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script> 
    <%if (servertype == 14)
      { %>
    <!--电脑端手机端兼容日历 js css-->
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.core-2.5.2.js" type="text/javascript"></script>
	<script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.core-2.5.2-zh.js" type="text/javascript"></script>

	<link href="http://shop.etown.cn/Scripts/mobiscroll/dev/css/mobiscroll.core-2.5.2.css" rel="stylesheet" type="text/css" />
	<link href="http://shop.etown.cn/Scripts/mobiscroll/dev/css/mobiscroll.animation-2.5.2.css" rel="stylesheet" type="text/css" />
	<script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.datetime-2.5.1.js" type="text/javascript"></script>
	<script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.datetime-2.5.1-zh.js" type="text/javascript"></script>

	<!-- S 可根据自己喜好引入样式风格文件 -->
	<script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.android-ics-2.5.2.js" type="text/javascript"></script>
	<link href="http://shop.etown.cn/Scripts/mobiscroll/dev/css/mobiscroll.android-ics-2.5.2.css" rel="stylesheet" type="text/css" />
	<!-- E 可根据自己喜好引入样式风格文件 -->

     <script type="text/javascript">
        $(function () {
			var currYear = (new Date()).getFullYear();	
			var opt={};
			opt.date = {preset : 'date'};
			//opt.datetime = { preset : 'datetime', minDate: new Date(2012,3,10,9,22), maxDate: new Date(2014,7,30,15,44), stepMinute: 5  };
			opt.datetime = {preset : 'datetime'};
			opt.time = {preset : 'time'};
			opt.default = {
				theme: 'android-ics light', //皮肤样式
		        display: 'modal', //显示方式 
		        mode: 'scroller', //日期选择模式
				lang:'zh',
		        startYear:currYear - 10, //开始年份
		        endYear:currYear + 10 //结束年份
			};

			$("#datetime").text('').scroller('destroy').scroller($.extend(opt['date'], opt['default']));
		    
        });
    </script>
   <!--电脑端手机端兼容日历 js css-->
   <%}%>
    <script type="text/javascript">
        $(function () {


            var agentid = $("#hid_agentid").trimVal();
            var proid = $("#hid_proid").trimVal();
            var comid = $("#hid_comidtemp").trimVal();

            //使用常用地址
            $("#a_ChooseAddress").click(function () {
                window.open("CommonAddrlist.aspx?proid=" + proid + "&unum=" + $("#orderNum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal());
            })

            $("#h_comname").text($("#hid_company").trimVal());
            SearchList();
            function SearchList() {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=getagentProById",
                    data: { proid: proid, agentid: agentid, comid: comid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            var rebate = data.msg.Advise_price; //分销差价
                            var childreduce = data.msg.Childreduce; //儿童减免
                            $("#hidchildreduce").val(data.msg.Childreduce);

                            $("#hid_server_type").val(data.msg.Server_type);
                            $("#sName").html(data.msg.Pro_name);

                            var pro_iscanuseonsameday = '同产品有效期'; //使用限制
                            if (data.msg.Server_type == 10)//旅游大巴
                            {
                             //日历
                            $("#selDate").bind("click",function () {
                                scrollTo(0, 1);

                                $("#orderFome").removeClass().addClass("translate3d");
                                $("#calDiv").fadeIn(); $("#inner").hide();
                                $("footer").hide();
                            });


                                $("#orderNum").attr("readonly", "readonly");


                                $("#selDate").parent().show();
                                $("div:[name='tbody_busplus']").show();
                                $("#div_travelbus").show();
                                $("#travel_tb").show();

                                var pickuppointstr = ''; //上车地点下拉框字符串
                                var dropoffpointstr = ''; //下车地点下拉框字符串
                                if ($("#hid_pickuppoint").trimVal() != "") {
                                    var arrpickuppoint = $("#hid_pickuppoint").trimVal().split('，');
                                    for (var a = 0; a < arrpickuppoint.length; a++) {
                                        pickuppointstr += '<option value="' + arrpickuppoint[a] + '">' + arrpickuppoint[a] + '</option>';
                                    }
                                }
                                else {
                                    pickuppointstr = '<option value="张家口火车站">张家口火车站</option>';
                                }

                                if ($("#hid_dropoffpoint").trimVal() != '') {
                                    $("#tr_dropoffpoint").show();
                                    var arrdropoffpoint = $("#hid_dropoffpoint").trimVal().split('，');
                                    for (var a = arrdropoffpoint.length - 1; a >= 0; a--) {
                                        dropoffpointstr += '<option value="' + arrdropoffpoint[a] + '">' + arrdropoffpoint[a] + '</option>';
                                    }
                                }
                                else {
                                    $("#tr_dropoffpoint").hide();
                                }

                                $("#pointuppoint").html(pickuppointstr);
                                $("#dropoffpoint").html(dropoffpointstr);

                                $("#travel_tbody").append(
                                 '<tr>' +
                                    '<td class="tdHead"  valign="top" colspan="2"><div class="w-item">' +
                                            '<label>姓&nbsp;&nbsp;&nbsp;&nbsp;名：</label><input type="text" id="travelname1" placeholder="姓名" value="" style="width:60px;" />' +
                                             '<label>联系电话：</label><input type="text" id="travelphone1" placeholder="联系电话" value="" style="width:100px;" />' +
                                            '<br><label>身份证：</label><input type="text" id="travelidcard1" placeholder="身份证"  value="" style="width:180px;"/>' +
                                             '<label style="display:none;">民&nbsp;&nbsp;&nbsp;&nbsp;族：</label><input type="text" id="travelnation1" value="汉族" style="width:60px;display:none;" />' +
                                             '<label style="display:none;">备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注：</label><input type="text" id="travelremark1" placeholder="备注" value="" style="width:100px;display:none;" />' +
                                       '<br><input name="chkItem" type="checkbox" value="1" id="chkItem" style="width: auto;margin-right: auto;height: auto;"/>与预定人相同' +
                                    '</div></td>' +
                                '</tr> ');

                                //団期处理
                                var datestr = "";
                                var datestr_zhehang = "<br>";
                                var firstdate = "";
                                var firstdate_price = "0";
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
                                            firstdate_price = data.linedate[i].Menprice;
                                        }

                                        listdate += ChangeDateFormat(data.linedate[i].Daydate) + ",";

                                        listprice += data.linedate[i].Menprice + ","

                                        listempty += data.linedate[i].Emptynum + ",";
                                    }
                                    $("#hidLeavingDate").val(listdate); //日期列表
                                    $("#hidMinLeavingDate").val(firstdate); //第一天
                                    $("#hidLinePrice").val(listprice); //价格列表
                                    $("#hidEmptyNum").val(listempty); //空位列表

                                    $("#datetime").html(firstdate);
                                    $("#Span1").text(firstdate);
                                    $("#sPrice").text(firstdate_price);
                                    $("#hid_payprice").val(firstdate_price);
                                    $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + firstdate_price + '</b>');
                                      //団期日历
                                      $("#calDiv").datepicker({
                                            minDate: $("#hidMinLeavingDate").val(),
                                            onSelect: function (dateText) {
                                                $("#datetime").html(dateText);
                                                $("#Span1").html(dateText);
                                                var datearr = $("#hidLeavingDate").val().split(",");
                                                var pricearr = $("#hidLinePrice").val().split(",");
                                                var emptyarr = $("#hidEmptyNum").val().split(",");
                                                var childreduce = $("#hidchildreduce").val();
                                                for (var i = 0; i < datearr.length; i++) {
                                                    if (datearr[i] == dateText) {
                            //                            $("#sPrice").html('成人:￥' + pricearr[i] + ',儿童:￥' + parseInt(pricearr[i] - childreduce));
                                                        $("#sPrice").html('￥' + pricearr[i]);
                                                        $("#hid_payprice").val(pricearr[i]);
                                                        var totalcount = parseInt($("#orderNum").trimVal() * pricearr[i]);
                                                        $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                                                        $("#" + dateText).addClass("selected"); //对选中的增加
                                                    } else {
                                                        //$("#" + datearr[i]).removeClass("selected"); //对未选中的日期移除
                                                    }
                                                }

                                                $("#orderFome").removeClass("translate3d");
                                                $("#calDiv").hide();
                                                $("#bgDiv").css("display", "none");
                                                $("#inner").show();

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
                                                    return [true, "hasCourse", "<p>￥" + dayprice + "</p><p style=\"font-size:12px;color:#ff0000;\">" + dayempty + "</p>"];

                                                } else {
                                                    return [false, "noCourse", '无团期'];
                                                }
                                            }
                                        });
                                }
                            }
                            else if (data.msg.Server_type == 9)//订房
                            {
                              //日历
                                $("#selDate").bind("click",function () {
                                    scrollTo(0, 1);
                                   $("#orderFome").removeClass().addClass("translate3d");
                                   $("#calDiv").fadeIn(); $("#inner").hide();
                                   $("footer").hide();
                                });

                                $("#u_traveldate_out").bind("click",function () {
                                    scrollTo(0, 1);
                                    $("#orderFome").removeClass().addClass("translate3d");
                                    $("#calDivOut").fadeIn(); $("#inner").hide();
                                    $("footer").hide();
                                });

                                $("#orderNum").attr("readonly", "readonly");

                                $("#selDate").parent().show();
                                $("#dl_hoteloutdate").parent().show();
                                 
                                  
                                $("#selDate").find("dt").html("入住日期");
                                $("#dl_hoteloutdate").find("dt").html("离店日期");


                                //団期处理
                                var datestr = "";
                                var datestr_zhehang = "<br>";
                                var firstdate = "";
                                var firstdate_price = "0";
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
                                            firstdate_price = data.linedate[i].Menprice;
                                        }

                                        listdate += ChangeDateFormat(data.linedate[i].Daydate) + ",";

                                        listprice += data.linedate[i].Menprice + ",";

                                        listempty += data.linedate[i].Emptynum + ",";
                                    }
                                    //                                $("#dateinfo").html(datestr); //显示団期
                                    $("#hidLeavingDate").val(listdate); //日期列表
                                    $("#hidMinLeavingDate").val(firstdate); //第一天
                                    $("#hidLinePrice").val(listprice); //价格列表
                                    $("#hidEmptyNum").val(listempty); //空位列表
                                     
                                    $("#datetime").html(firstdate);
                                    $("#Span1").text(firstdate);
                                    $("#sPrice").text(firstdate_price);
                                    $("#hid_payprice").val(firstdate_price);
                                    $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + firstdate_price + '</b>');



                                    //入住日历
                                    $("#calDiv").datepicker({
                                        minDate: $("#hidMinLeavingDate").val(),
                                        onSelect: function (dateText) {
                                            $("#datetime").val(dateText);   
                                            $("#Span1").html(dateText);
                                            var datearr = $("#hidLeavingDate").val().split(",");
                                            var pricearr = $("#hidLinePrice").val().split(",");
                                            var emptyarr = $("#hidEmptyNum").val().split(",");
                                            var childreduce = $("#hidchildreduce").val();

                                            hoteldate();

                                            $("#orderFome").removeClass("translate3d");
                                            $("#calDiv").hide();
                                            $("#bgDiv").css("display", "none");
                                            $("#inner").show();

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
                                                 if(dayempty>0){
                                                    return [true, "hasCourse", "<p>￥" +dayprice + "</p>"];
                                                }else{
                                                    return [true, "hasCourse", "<p>￥" +dayprice + "(申请)</p>"];
                                                }

                                            } else {
                                                return [true, "hasCourse", ''];
                                            }
                                        }
                                    });


                                     //离店日期
                                    $("#calDivOut").datepicker({
                                        minDate: $("#hidMinLeavingDate").val(),
                                        onSelect: function (dateText) {
                                            $("#u_traveldate_out").val(dateText); 
                                            $("#Span1").html(dateText);
                                            hoteldate();


                                            $("#orderFome").removeClass("translate3d");
                                            $("#calDivOut").hide();
                                            $("#bgDiv").css("display", "none");
                                            $("#inner").show();

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
                                               if(dayempty>0){
                                                    return [true, "hasCourse", "<p>￥" +dayprice + "</p>"];
                                                }else{
                                                    return [true, "hasCourse", "<p>￥" +dayprice + "(申请)</p>"];
                                                }

                                            } else {
                                                return [true, "hasCourse", '<p>申请</p>'];
                                            }
                                        }
                                    });

                                }

                                $("#setting-home").show();
                                $("#secondary-tabs").show();

                                $("#ticket_remark").html(data.msg.Pro_Remark);
                                $(".titlepng").html(data.msg.Pro_name);
                                //                                $("#hid_pro_Number").val(data.msg.Pro_number);

                                $("#pro_youxiaoqi").html("仅限当天使用");


                                var yuding_msg = "<span id=\"msgview\" style=\"padding-left:10px; color:#ff0000;cursor:pointer;\">查看详情+</span><div id=\"msg\" style=\"display:none;\">";
                                if (data.msg.Pro_Remark != "") {
                                    yuding_msg += "<b>产品介绍: </b>" + data.msg.Pro_Remark + "<br /><br />"; ;
                                }

                                if (data.msg.Service_Contain != "") {
                                    yuding_msg += "<b>产品包含:</b> " + data.msg.Service_Contain + "<br /><br />";
                                }
                                if (data.msg.Service_NotContain != "") {
                                    yuding_msg += "<b>产品不包含:</b> " + data.msg.Service_NotContain + "<br /><br />";
                                }
                                if (data.msg.Precautions != "") {
                                    yuding_msg += "<b>注意事项: </b>" + data.msg.Precautions;
                                }
                                yuding_msg += "</div>";
                                $("#yuding_msg").html(yuding_msg);
                            } 
                            else if (data.msg.Server_type == 14)//保险产品
                            {
                                $("#Span1").parents("dl").hide();
                                $("#Span2").parents("dl").hide();

                                $("#orderNum").attr("readonly", "readonly");
                                $("#orderNum").parents("dd").prev().text("被保险人数量:");

                                $("#datetime").parents("#selDate").parent().show();
                                $("#datetime").parent().prev("dt").text("起保日期:");
                             
                                 
                                  
                                $("#getName_1").parent().prev().text("投保人姓名"); 
                                $("#getName_1").attr("placeholder","请填写投保人姓名");
                                $("#getPhone_1").parent().prev().text("投保人手机"); 
                                $("#getPhone_1").attr("placeholder","请填写投保人手机");


                                  $("#sPrice").text("0");
                                  $("#hid_payprice").val("0");
                                  $("#priceTotal").html('￥<b style=" font-size:18px;">0</b>');

                                $("#baoxian_tb").show();
                                $("#baoxian_tbody").append(
                                 '<tr>' +
                                    '<td class="tdHead"  valign="top" colspan="2"><div class="w-item">' +
                                            '<label>姓&nbsp;&nbsp;&nbsp;&nbsp;名：</label><input type="text" name="baoxianname" id="baoxianname1" placeholder="姓名" value="" style="width:60px;" />' +
                                             '<label>拼音/英文名：</label><input type="text" name="baoxianpinyinname" id="baoxianpinyinname1" placeholder="拼音/英文名" value="" style="width:100px;" />' +
                                            '<br><label>身份证：</label><input type="text"  name="baoxianidcard" id="baoxianidcard1" placeholder="身份证"  value="" style="width:180px;"/>' + 
                                    '</div></td>' +
                                '</tr> ');
                    
                            }
                            else if (data.msg.Server_type == 2 || data.msg.Server_type == 8) //当地游，跟团游
                            {
                                $("#sName").html(data.msg.Pro_name);
                                $("#selDate").parent().show();
                                $("#Span2").parent().parent().hide();

                                $("#orderNum").parent().children().last().show(); //成人数量显示
                                $("#child_orderNum").parent().parent().show(); //儿童数量选择显示
                                $("#child_orderNum").parent().children().last().show();
                                //団期处理
                                var datestr = "";
                                var datestr_zhehang = "<br>";
                                var firstdate = "";
                                var firstdate_price = "0";
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
                                            firstdate_price = parseInt(data.linedate[i].Menprice - rebate);
                                        }

                                        listdate += ChangeDateFormat(data.linedate[i].Daydate) + ",";

                                        listprice += parseInt(data.linedate[i].Menprice - rebate) + ",";
                                        listempty += data.linedate[i].Emptynum + ",";
                                    }
                                    $("#hidLeavingDate").val(listdate); //日期列表
                                    $("#hidMinLeavingDate").val(firstdate); //第一天
                                    $("#hidLinePrice").val(listprice); //价格列表
                                    $("#hidEmptyNum").val(listempty); //空位列表

                                    $("#datetime").html(firstdate);
                                    $("#Span1").text(firstdate);
                                    $("#sPrice").text('成人:￥' + firstdate_price + ',儿童:￥' + parseInt(firstdate_price - childreduce));
                                    $("#hid_payprice").val(firstdate_price);
                                    $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + firstdate_price + '</b>');
                                     //団期日历
                                      $("#calDiv").datepicker({
                minDate: $("#hidMinLeavingDate").val(),
                onSelect: function (dateText) {
                    $("#datetime").html(dateText);
                    $("#Span1").html(dateText);
                    var datearr = $("#hidLeavingDate").val().split(",");
                    var pricearr = $("#hidLinePrice").val().split(",");
                    var emptyarr = $("#hidEmptyNum").val().split(",");
                    var childreduce = $("#hidchildreduce").val();
                    for (var i = 0; i < datearr.length; i++) {
                        if (datearr[i] == dateText) {
//                            $("#sPrice").html('成人:￥' + pricearr[i] + ',儿童:￥' + parseInt(pricearr[i] - childreduce));
                            $("#sPrice").html('￥' + pricearr[i]);
                            $("#hid_payprice").val(pricearr[i]);
                            var totalcount = parseInt($("#orderNum").trimVal() * pricearr[i]);
                            $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                            $("#" + dateText).addClass("selected"); //对选中的增加
                        } else {
                            //$("#" + datearr[i]).removeClass("selected"); //对未选中的日期移除
                        }
                    }

                    $("#orderFome").removeClass("translate3d");
                    $("#calDiv").hide();
                    $("#bgDiv").css("display", "none");
                    $("#inner").show();

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
                        return [true, "hasCourse", "<p>￥" + dayprice + "</p><p style=\"font-size:12px;color:black;\">余" + dayempty + "</p>"];

                    } else {
                        return [false, "noCourse", '无团期'];
                    }
                }
            });
                                }
                                $("#order_remark").parent().parent().parent().show();
                                $("#Dd1").parent().parent().show();
                            }
                            else //票务；实物产品
                            {
                                $("#orderNum").removeAttr("readonly");

                                if (data.msg.Ispanicbuy == 2) {//如果是限购,显示库存量

                                    $("#sName").html(data.msg.Pro_name + "(库存量:<span style='color:red; '>" + data.msg.Limitbuytotalnum + "</span>)");
                                }
                                //如果是实物 ，则 隐藏时间
                                if (data.msg.Server_type == 11) {
                                    $(".shiwupro").hide();

                                }




                                $("#Span1").text(ChangeDateFormat(data.msg.Pro_start) + " 至 " + ChangeDateFormat(data.msg.Pro_end));
                                $("#sPrice").text(data.msg.Advise_price);
                                $("#hid_payprice").val(data.msg.Advise_price);
                                $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + data.msg.Advise_price + '</b>');


                                //产品使用限制
                                if (data.msg.ProValidateMethod == "2") {

                                    //结束时间
                                    if (data.msg.Appointdata == 1) {
                                        pro_iscanuseonsameday += "出票一周有效";
                                    }

                                    if (data.msg.Appointdata == 2) {
                                        pro_iscanuseonsameday += "出票一月有效";
                                    }

                                    if (data.msg.Appointdata == 3) {
                                        pro_iscanuseonsameday += "出票三月有效";
                                    }

                                    if (data.msg.Appointdata == 4) {
                                        pro_iscanuseonsameday += "出票半年有效";
                                    }

                                    if (data.msg.Appointdata == 5) {
                                        pro_iscanuseonsameday += "出票一年有效";
                                    }
                                }
                                else {
                                    pro_iscanuseonsameday = "同产品有效期";
                                }
                            }


                            $("#p_proexplain").html(data.msg.Pro_explain);
                            $("#Span2").html(pro_iscanuseonsameday);

                            var yuding_msg = "";
                            if (data.msg.Pro_Remark != "") {
                                yuding_msg += "<b>产品介绍: </b>" + data.msg.Pro_Remark + "<br /><br />";
                            }

                            if (data.msg.Service_Contain != "") {
                                yuding_msg += "<b>产品包含:</b> " + data.msg.Service_Contain + "<br /><br />";
                            }
                            if (data.msg.Service_NotContain != "") {
                                yuding_msg += "<b>产品不包含:</b> " + data.msg.Service_NotContain + "<br /><br />";
                            }
                            if (data.msg.Precautions != "") {
                                yuding_msg += "<b>注意事项: </b>" + data.msg.Precautions;
                            }

                            $("#yuding_msg").html(yuding_msg);

                            //如果服务类型是 票务； 则产品说明信息中 显示 电子码使用限制
                            if (data.msg.Server_type == 1) {
                                var remarkk = $("#yuding_msg").html();
                                if (data.msg.Iscanuseonsameday == 0)//电子码当天不可用
                                {
                                    $("#yuding_msg").html("此产品当天预订不可用<br>" + remarkk);
                                }
                                if (data.msg.Iscanuseonsameday == 1)//电子码当天可用
                                {
                                    $("#yuding_msg").html("当天可用<br>" + remarkk);
                                }
                                if (data.msg.Iscanuseonsameday == 2)//电子码出票2小时内不可用
                                {
                                    $("#yuding_msg").html("此产品出票2小时内不可用<br>" + remarkk);
                                }
                                  //票务增加预约日期选择
                                 var curentDate=CurentDate(); 
                                 $("#datetime").text(curentDate);  
                               
                                if(data.msg.isSetVisitDate==1){ 
                                     //日历
                                    $("#selDate").bind("click",function () {
                                         scrollTo(0, 1);
                                         $("#orderFome").removeClass().addClass("translate3d");
                                         $("#calDiv").fadeIn(); $("#inner").hide();
                                         $("footer").hide(); 
                                    }); 
                                   $("#selDate").parent().show();
                                     //団期日历
                                    $("#calDiv").datepicker({
                                        minDate: curentDate,
                                        onSelect: function (dateText) {
                                            $("#datetime").text(dateText); 
                                            
                                             $("#orderFome").removeClass("translate3d");
                                             $("#calDiv").hide();
                                             $("#bgDiv").css("display", "none");
                                             $("#inner").show();
                                        },
                                        beforeShowDay: function (date) {
                                            var dt = formatDate(date);
                                               
                                            var result = true;
                                           
                                            if (result) {
                                                return [true, "hasCourse", "<p></p>"]; 
                                            }  
                                        }
                                    });
                                } 
                            }

                            //如果服务类型是 实物，显示使用常用地址 按钮 和 运费模块
                            //现在加购物车功能，则使用常用地址按钮、运费模板、预订人、预订人手机 都隐藏
                            if (data.msg.Server_type == 11) {
                                $("#a_ChooseAddress").parent().show();
                                $("#tbody_address").show();
                                $("#getName_1").parent().parent().parent().show();
                                $("#getPhone_1").parent().parent().parent().show();
                            }
                        }
                    }
                })
            }

            //            //实物产品立即购买
            //            $("#buyNow").click(function () {
            //                showErr("请填写购买信息");
            //             
            //            })

          

           
            //产品说明显示
            $("#sDes").click(function () {
                var c = $(this).find(".ico-right");
                if (c.size() > 0) {
                    if (c.hasClass("return")) {
                        $(this).find(".isover_jianjie").show();
                        $(this).find(".isover").hide();
                        $(this).find(".border").show();
                        c.removeClass("return")
                    } else {
                        $(this).find(".isover_jianjie").hide();
                        $(this).find(".isover").show();
                        $(this).find(".border").hide();
                        c.addClass("return")
                    }
                }
            })

            //行程显示
            $("#Dd1").click(function () {

                var c = $(this).find(".ico-right");
                if (c.size() > 0) {
                    if (c.hasClass("return")) {
                        $(this).find(".isover_jianjie").show();
                        $(this).find(".isover").hide();
                        $(this).find(".border").show();
                        c.removeClass("return")
                    } else {
                        $("#Span3").html("");
                        //获取行程信息
                        var proid = $("#hid_proid").trimVal();
                        $.post("/JsonFactory/ProductHandler.ashx?oper=getLinetripById", { lineid: proid }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                //                        $.prompt("查询错误");
                                return;
                            }
                            if (data.type == 100) {
                                //行程处理
                                var linestr = "";
                                var linecenter = "";
                                var linetop = "";
                                var linebotton = "";
                                if (data.msg != null) {

                                    for (var i = 0; i < data.msg.length; i++) {

                                        if (i == 0) {
                                            linetop = " <p class=\"date_plan_top\">      第" + (i + 1) + "天: " + data.msg[i].ActivityArea + "&nbsp;" + data.msg[i].Traffic + "       </p>";
                                        } else {
                                            linetop = " <p class=\"date_plan\">        第" + (i + 1) + "天: " + data.msg[i].ActivityArea + "&nbsp;" + data.msg[i].Traffic + "     </p>";
                                        }
                                        linecenter = "    <div class=\"plan_box\">      " + data.msg[i].Description + " <br>酒店:" + data.msg[i].Hotel + "<br>用餐:" + data.msg[i].Dining + "  </div>";

                                        if (i == 0) {//第一天不加，第二天开始头部加
                                            linebotton = "";
                                        } else {
                                            linebotton = "<div class=\"explain_back\">                <div class=\"explain\">                                    <p class=\"explain_cont_bottom\"></p>                </div>                </div>";
                                        }
                                        linestr += linebotton + linetop + linecenter
                                    }

                                    var linedefautop = "<p class=\"mc_lists\"></p>";
                                    var linedefaubot = "<div class=\"slide_block\" id=\"slideBlock\">                 </div>";
                                    $("#Span3").html(linedefautop + linestr + linedefaubot);

                                }

                            }
                        })


                        $(this).find(".isover_jianjie").hide();
                        $(this).find(".isover").show();
                        $(this).find(".border").hide();
                        c.addClass("return")
                    }
                }
            })

            $("#reduceP").bind("click", function () {
                var c = $("#orderNum").val();
                if (c > 2) {
                    $("#reduceP").removeClass("enabled");
                } else {
                    $("#reduceP").addClass("enabled");
                }
                if (c > 1) {
                    if (/^\d+$/g.test(c)) {

                        c = parseInt(c, 10) - 1;
                        $("#orderNum").val(c);

                        //如果是实物产品的话需要计算运费
                        if ($("#hid_server_type").val() == 11) {
                            var proid = $("#hid_proid").trimVal();
                            var city = $("#com_city").trimVal();
                            if (city != "城市") {
                                getDeliveryFee(proid, c, city);
                            }
                        }


                        //如果是旅游大巴产品，则需要增加乘车人信息
                        if ($("#hid_server_type").val() == 10) {

                            var ihtml = "";
                            for (var j = 1; j <= c; j++) {
                                var minzu = "汉族";
                                if ($("#travelnation" + j).trimVal() != '') {
                                    minzu = $("#travelnation" + j).trimVal();
                                }

                                ihtml += '<tr class="travelcontentstyle">' +
                    '<td class="tdHead"  valign="top" colspan="2"><div class="w-item">' +
                              '<label>姓&nbsp;&nbsp;&nbsp;&nbsp;名：</label><input type="text" id="travelname' + j + '"  placeholder="姓名" value="' + $("#travelname" + j).trimVal() + '"  style="width:60px;"  />' +
                              '<label>联系电话：</label><input type="text" id="travelphone' + j + '"      placeholder="联系电话" value="' + $("#travelphone" + j).trimVal() + '"  style="width:100px;"  />' +
                              '<br><label>身份证</label><input type="text" id="travelidcard' + j + '"   placeholder="身份证"  value="' + $("#travelidcard" + j).trimVal() + '"   style="width:180px;" />' +
                              '<label style="display:none;">民&nbsp;&nbsp;&nbsp;&nbsp;族：</label><input type="text" id="travelnation' + j + '" value="' + minzu + '"  style="width:60px;display:none;"  />' +
                              '<label style="display:none;">备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注：</label><input type="text" id="travelremark' + j + '"  placeholder="备注" value="' + $("#travelremark" + j).trimVal() + '"  style="width:100px;display:none;"  />' ;
                                  if (j == 1) {
                                    ihtml += '<br><input name="chkItem" type="checkbox" value="1" id="chkItem" style="width: auto;margin-right: auto;height: auto;"/>与预定人相同';
                                }
                    ihtml +=  '</div></td>' +
                '</tr> ';
                            }
                            $("#travel_tbody").html(ihtml);

                        }
                          //如果是保险产品，则需要增加被保险人信息
                        if ($("#hid_server_type").val() == 14) {

                            var ihtml = "";
                            for (var j = 1; j <= c; j++) {
                               ihtml += '<tr>' +
                                 '<td class="tdHead"  valign="top" colspan="2"><div class="w-item">' +
                                            '<label>姓&nbsp;&nbsp;&nbsp;&nbsp;名：</label><input type="text" name="baoxianname" id="baoxianname' + j + '" placeholder="姓名" value="" style="width:60px;" />' +
                                             '<label>拼音/英文名：</label><input type="text" name="baoxianpinyinname" id="baoxianpinyinname' + j + '" placeholder="拼音/英文名" value="" style="width:100px;" />' +
                                            '<br><label>身份证：</label><input type="text"  name="baoxianidcard" id="baoxianidcard' + j + '" placeholder="身份证"  value="" style="width:180px;"/>' + 
                                    '</div></td>' +
                                '</tr> '
                            }
                            $("#baoxian_tbody").html(ihtml);

                        }

                        if ($("#hid_server_type").val() == 2 || $("#hid_server_type").val() == 8) {//如果是旅游产品，则需要添加儿童价格
                            var totalcount = parseInt($("#orderNum").trimVal() * $("#hid_payprice").trimVal()) + $("#child_orderNum").trimVal() * parseInt($("#hid_payprice").trimVal() - $("#hidchildreduce").trimVal());

                            $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                        }
                        else { //其他产品无需计算儿童价格
                            var totalcount = $("#orderNum").trimVal() * $("#hid_payprice").trimVal();

                            $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                        }
                    }
                } else {

                    return false;
                }


            });

            $("#plusP").bind("click", function () {
                var d = $("#orderNum").val();
                if (/^\d+$/g.test(d)) {
                    d = parseInt(d, 10) + 1;
                    $("#orderNum").val(d);
                    $("#reduceP").removeClass("enabled");

                    //如果是实物产品的话需要计算运费
                    if ($("#hid_server_type").val() == 11) {
                        var proid = $("#hid_proid").trimVal();
                        var city = $("#com_city").trimVal();
                        if (city != "城市") {
                            getDeliveryFee(proid, d, city);
                        }
                    }


                    //如果是旅游大巴产品，则需要增加乘车人信息
                    if ($("#hid_server_type").val() == 10) {
                        //旅游大巴产品，限制提单数量最高60
                        if (parseInt(d) > 10) {
                            alert("大巴产品限制预订数量最高为10人");
                            $("#orderNum").val(10);
                            return;
                        }

                        $("#travel_tbody").append('<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2"><div class="w-item">' +
                            '<label>姓&nbsp;&nbsp;&nbsp;&nbsp;名：</label><input type="text" id="travelname' + d + '" placeholder="姓名" value=""  style="width:60px;" />' +
                            '<label>联系电话：</label><input type="text" id="travelphone' + d + '"  placeholder="联系电话" value=""  style="width:100px;" />' +
                            '<br><label>身份证：</label><input type="text" id="travelidcard' + d + '" placeholder="身份证" value=""  style="width:180px;" />' +
                            '<label style="display:none;">民&nbsp;&nbsp;&nbsp;&nbsp;族：</label><input type="text" id="travelnation' + d + '" value="汉族"  style="width:60px;display:none;"  />' +
                            '<label style="display:none;">备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注：</label><input type="text" id="travelremark' + d + '"  placeholder="备注" value=""  style="width:100px;display:none;" />' +
                    '</div></td>' +
                '</tr> ');
                    }
                     
                     //如果是保险产品，则需要增加被保险人信息
                    if ($("#hid_server_type").val() == 14) {
                       
                        //保险产品，限制提单数量最高10
                        if (parseInt(d) > 10) {
                            alert("保险产品限制预订数量最高为10人");
                            $("#orderNum").val(10);
                            return;
                        }
                       
                        $("#baoxian_tbody").append(
                        '<tr>' +
                                 '<td class="tdHead"  valign="top" colspan="2"><div class="w-item">' +
                                            '<label>姓&nbsp;&nbsp;&nbsp;&nbsp;名：</label><input type="text" name="baoxianname" id="baoxianname' + d + '" placeholder="姓名" value="" style="width:60px;" />' +
                                             '<label>拼音/英文名：</label><input type="text" name="baoxianpinyinname" id="baoxianpinyinname' + d + '" placeholder="拼音/英文名" value="" style="width:100px;" />' +
                                            '<br><label>身份证：</label><input type="text"  name="baoxianidcard" id="baoxianidcard' + d + '" placeholder="身份证"  value="" style="width:180px;"/>' + 
                                    '</div></td>' +
                       '</tr> ');

                    }

                    if ($("#hid_server_type").val() == 2 || $("#hid_server_type").val() == 8) {//如果是旅游产品，则需要添加儿童价格
                        var totalcount = parseInt($("#orderNum").trimVal() * $("#hid_payprice").trimVal()) + $("#child_orderNum").trimVal() * parseInt($("#hid_payprice").trimVal() - $("#hidchildreduce").trimVal());

                        $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                    }
                    else { //其他产品无需计算儿童价格
                        var totalcount = $("#orderNum").trimVal() * $("#hid_payprice").trimVal();

                        $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                    }
                }
            });

            //儿童人数增
            $("#Span5").click(function () {
                var i = $("#child_orderNum").val();

                i++;
                $("#child_orderNum").val(i);
                $("#Span4").removeClass("enabled");

                if ($("#hid_server_type").val() == 2 || $("#hid_server_type").val() == 8) {//如果是旅游产品，则需要添加儿童价格
                    var totalcount = parseInt($("#orderNum").trimVal() * $("#hid_payprice").trimVal()) + $("#child_orderNum").trimVal() * parseInt($("#hid_payprice").trimVal() - $("#hidchildreduce").trimVal());
                    $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                }
                else { //其他产品无需计算儿童价格
                    var totalcount = $("#orderNum").trimVal() * $("#hid_payprice").trimVal();
                    $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                }

            });
            //儿童人数减
            $("#Span4").click(function () {
                var i = $("#child_orderNum").val();
                if (i > 1) {
                    $("#Span4").removeClass("enabled");
                } else {
                    $("#Span4").addClass("enabled");
                }
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    //showErr("请填写正确地数量");
                    return;
                }
                else {
                    i--;
                    if (i >= 0) {
                        $("#child_orderNum").val(i);
                        if ($("#hid_server_type").val() == 2 || $("#hid_server_type").val() == 8) {//如果是旅游产品，则需要添加儿童价格
                            var totalcount = parseInt($("#orderNum").trimVal() * $("#hid_payprice").trimVal()) + $("#child_orderNum").trimVal() * parseInt($("#hid_payprice").trimVal() - $("#hidchildreduce").trimVal());
                            $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                        }
                        else { //其他产品无需计算儿童价格
                            var totalcount = $("#orderNum").trimVal() * $("#hid_payprice").trimVal();
                            $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + totalcount + '</b>');
                        }
                    }
                }
            });

            //产品是旅游大巴时，u_num 加了只读属性，所以时间不会触发，不用考虑旅游大巴
            $("#orderNum").keyup(function () {
                var i = $(this).val();
                if (i > 2) {
                    $("#Span4").removeClass("enabled");
                } else {
                    $("#Span4").addClass("enabled");
                }

                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    showErr("请填写正确地数量");
                    return;
                } else {

                    //如果是实物产品的话需要计算运费
                    if ($("#hid_server_type").val() == 11) {
                        var proid = $("#hid_proid").trimVal();
                        var city = $("#com_city").trimVal();
                        if (city != "城市") {
                            getDeliveryFee(proid, i, city);
                        }
                    }

                    $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + parseInt($("#hid_payprice").trimVal()) * parseInt(i) + '</b>');
                }
            });

            $("#submitBtn1").click(function () {
//            alert("tijiaodingdan");
                var proid = $("#hid_proid").trimVal();
                var u_num = $("#orderNum").trimVal();
                var u_name = $("#getName_1").trimVal();
                var u_phone = $("#getPhone_1").trimVal();
                var u_idcard = $("#getIdcard").trimVal();
                var u_childnum = $("#child_orderNum").trimVal();
                
                    var manyspeci = $("#hid_manyspeci").val();
                    var speciid = $("#hid_speciid").val();

                    if(manyspeci==1){
                        if(speciid==0){
                            alert("请选择具体规格");
                            $("#loading").hide();
                            return;
                        }
                    }



                var u_traveldate = $("#datetime").html();
                 if (u_traveldate == "") {
                  u_traveldate = $("#datetime").trimVal();
                 }
                var u_traveldate_out = $("#u_traveldate_out").trimVal();
                if ($("#hid_server_type").trimVal() == 10) {
                    //判断游玩日期 不可为空
                    if (u_traveldate == "") {
                        showErr("请选择游玩日期");
                        return;
                    }
                }

                if (u_name == "") {
                    showErr("请填写姓名");
                    return;
                }
                if (u_phone == "") {
                    showErr("请填写手机号，来接收电子票短信");
                    return;
                } else {
                    if (!isMobel(u_phone)) {
                        showErr("请正确填写手机号");
                        return;
                    }
                }

                  <%if (issetidcard==1){%>
                        if(u_idcard==""){
                              showErr("请填写身份证号");
                              $("#getIdcard").focus();
                              $("#loading").hide();
                            return;
                        }
                
                    <%} %>

                var travelnames = ""; //乘车人姓名列表
                var travelidcards = ""; //乘车人身份证列表
                var travelnations = ""; //乘车人民族列表
                var travelphones = ""; //乘车人联系电话列表
                var travelremarks = ""; //乘车人备注列表


                var errid = "0"; // 输入格式错误的控件id
                if ($("#hid_server_type").trimVal() == 10) {

                    //判断乘车人信息不可为空
                    for (var i = 1; i <= u_num; i++) {
                        var travel_name = $("#travelname" + i).trimVal();
                        var travel_idcard = $("#travelidcard" + i).trimVal();
                        var travel_nation = $("#travelnation" + i).trimVal();
                        var travel_phone = $("#travelphone" + i).trimVal();
                        var travel_remark = $("#travelremark" + i).trimVal();


                        if (travel_name == "") {
                            alert("乘车人姓名不可为空");
                            errid = "travelname" + i;
                            break;
                        }
                        if (travel_phone == "") {
                            alert("乘车人" + travel_name + "电话不可为空");
                            errid = "travelphone" + i;
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
                            if(travel_idcard!="")
                            {
                                if (!IdCardValidate(travel_idcard)) {
                                    alert("乘车人的身份证号(" + travel_name + ")格式错误");
                                    errid = "travelidcard" + i;
                                    break;
                                }
                            }
                        if (travel_nation == "") {
                            alert("乘车人" + travel_name + "民族不可为空");
                            errid = "travelnation" + i;
                            break;
                        }
                        travelnames += travel_name + ",";
                        travelidcards += travel_idcard + ",";
                        travelnations += travel_nation + ",";
                        travelphones += travel_phone + ",";
                        travelremarks += travel_remark + ",";


                    }
                    if (errid != "0") {
                        $(errid).focus();
                        return;
                    }
                }

                //以下为实物运送数据
                var deliverytype = $("input:radio[name='deliverytype']:checked").trimVal();
                var province = $("#com_province").trimVal();
                var city = $("#com_city").trimVal();
                var address = $("#txtaddress").trimVal();
                var txtcode = $("#txtcode").trimVal();



                if ($("#hid_server_type").trimVal() == 11) {//实物产品
                    if (deliverytype == 2) {
                        if (province == "省份") {
                            showErr("请选择省份");
                            return;
                        }
                        if (city == "城市") {
                            showErr("请选择城市");
                            return;
                        }
                        if (address == "") {
                            showErr("请输入详细地址");
                            return;
                        }
                    }
                }

                if ($("#hid_server_type").trimVal() == 9) {

                        var  payprice = $("#hid_payprice").val();
                         //判断游玩日期 不可为空
                        if (u_traveldate == "") {
                            alert("请选择入住日期");
                            return;
                        }

                        if (u_traveldate_out == "") {
                            alert("请选择离店日期");
                            return;
                        }

                        if(!dateduibi(u_traveldate,u_traveldate_out)){
                            alert("离店日期必须大于入住日期");
                            return;
                        }

//                        if($("#hid_MinEmptynum").val()==0){
//                            alert("入住日期满房请选择其他日期");
//                        }
                        if(payprice==0){
                            alert("入住日期满房请选择其他日期");
                            return;
                        }
                        var minnum=$("#hid_MinEmptynum").trimVal()
                      
                        if(minnum<u_num){
                          if (!confirm("您预定的房间数大于剩余控房数，需要客服与酒店联系，确认时间较长，请耐心等待。您可以点击 “确定” 继续提交订单并完成付款手续。 如不能确认入住 可点击“取消”则放弃提单。 您可点击确认提单，我们客服将与酒店协调，一经确认成功，该订单不可取消或变更，已付款不予退还。当房间确认无房，此订单将自动退款，款项将退还到您的账户中！")) {
                                return;
                          }
                        
                        }


                    }
                    var baoxiannames = ""; //被保险人姓名列表
                    var baoxianpinyinnames = ""; //被保险人拼音/英文名列表
                    var baoxianidcards = ""; //乘车人民族列表
                     
                    $("input[name='baoxianname']").css("background-color","");
                    $("input[name='baoxianpinyinname']").css("background-color","");
                    $("input[name='baoxianidcard']").css("background-color","");

                    var baoxian_errid = "0"; // 输入格式错误的控件id
                    if ($("#hid_server_type").trimVal() == 14) {

                        //判断被保险人信息不可为空
                        for (var i = 1; i <= u_num; i++) {
                            var baoxian_name = $("#baoxianname" + i).trimVal();
                            var baoxian_pinyinname = $("#baoxianpinyinname" + i).trimVal();
                            var baoxian_idcard = $("#baoxianidcard" + i).trimVal();
                          


                            if (baoxian_name == "") {
                                alert("被保险人的姓名不可为空");
                                baoxian_errid = "baoxianname" + i;
                                $("#baoxianname"+i).css("background-color","aliceblue");
                                break;
                            }
                            if (baoxian_pinyinname == "") {
                                alert("被保险人的拼音/英文名不可为空");
                                baoxian_errid = "#baoxianpinyinname" + i;
                                $("#baoxianpinyinname"+i).css("background-color","aliceblue");
                                break;
                            }
                            if (baoxian_idcard == "") {
//                                alert("被保险人的身份证号不可为空");
//                                baoxian_errid = "baoxianidcard" + i;
//                                $("#baoxianidcard"+i).css("background-color","aliceblue");
//                                break;

                            } else {
                                if (!IdCardValidate(baoxian_idcard)) {
                                    alert("身份证号(" + baoxian_idcard + ")格式错误");
                                    baoxian_errid = "baoxianidcard" + i;
                                    $("#baoxianidcard"+i).css("background-color","aliceblue");
                                    break;
                                }
                            }
                            
                            baoxiannames += baoxian_name + ","; 
                            baoxianpinyinnames += baoxian_pinyinname + ",";
                            baoxianidcards += baoxian_idcard + ","; 
                        }
                        if (baoxian_errid != "0") { 
                            return;
                        }
                    }
                 $(this).val("提交中...").attr("disabled", "disabled");

                //创建订单
                $('#confirmButton').hide().after('<span id="spLoginLoading" style="margin-left:10px;color:#f39800; font-size:16px;">提交中……</span>');
                $.post("/JsonFactory/OrderHandler.ashx?oper=agentorder", {baoxiannames:baoxiannames,baoxianpinyinnames:baoxianpinyinnames,baoxianidcards:baoxianidcards, u_childnum: u_childnum, deliverytype: deliverytype, province: province, city: city, address: address, txtcode: txtcode, agentid: agentid, comid: comid, proid: proid,speciid:speciid, u_num: u_num, u_name: u_name, u_phone: u_phone,u_idcard:u_idcard, u_traveldate: u_traveldate, travelnames: travelnames, travelidcards: travelidcards, travelnations: travelnations, travelphones: travelphones, travelremarks: travelremarks, travel_pickuppoints: $("#pointuppoint").trimVal(), travel_dropoffpoints: $("#dropoffpoint").trimVal(), order_remark: $("#order_remark").trimVal(), payprice: $("#hid_payprice").trimVal(),start_date:u_traveldate,end_date:u_traveldate_out,lastarrivaltime:"6点",fangtai:"" }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {

                        if (data.msg == "yfkbz") {
                            alert("您的预付款不足，先充足预付款后，再到订单列表对未付款订单进行结算。");

                            //创建支付订单
                            $.post("/JsonFactory/OrderHandler.ashx?oper=agentRecharge", { agentid: agentid, comid: comid, payprice: data.money, u_name: "分销客户:" + u_name, u_phone: u_phone, handlid: data.id }, function (data1) {
                                data1 = eval("(" + data1 + ")");
                                if (data1.type == 1) {
                                    showErr(data1.msg);
                                    return;
                                }
                                if (data1.type == 100) {
                                    location.href = "/h5/pay.aspx?orderid=" + data1.msg + "&comid=" + comid + "&agentorderid=" + data.id;
                                    return;
                                }
                            })
                        } else {
                            showErr("提单失败");
                        }
                        return;
                    }
                    if (data.type == 100) {
                        //                            location.href = "Order.aspx?comid=" + comid;
                        showErr("提单成功");
                        return;
                    }
                })


            })

            $("input:radio[name='deliverytype']").click(function () {
                var chked = $("input:radio[name='deliverytype']:checked").val();
                Viewdelivery(chked);

            })

            $("#com_city").change(function () {

                var city = $("#com_city").trimVal();
                if (city == "城市") {
                    alert("请选择送达城市");
                    return;
                }
                //根据产品id和城市获得运费
                var proid = $("#hid_proid").trimVal();
                var num = $("#orderNum").trimVal();


                getDeliveryFee(proid, num, city);
            })

            //实物产品常用地址,针对实物产品
            var addrid = $("#hid_addrid").trimVal();
            if (addrid != 0) {
                $.post("/JsonFactory/OrderHandler.ashx?oper=getagentaddrbyid", { addrid: addrid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        $("#getName_1").val(data.msg.U_name);
                        $("#getPhone_1").val(data.msg.U_phone);
                        $("input:radio[name='deliverytype'][value='2']").attr("checked", true);
                        Viewdelivery(2);
                        $("#com_province").val(data.msg.Province);
                        $("#com_city").append('<option value="' + data.msg.City + '" selected="selected">' + data.msg.City + '</option>');
                        $("#txtaddress").val(data.msg.Address);
                        $("#txtcode").val(data.msg.Code);

                        //根据产品id和城市获得运费
                        var proid = $("#hid_proid").trimVal();
                        var num = $("#orderNum").trimVal();


                        getDeliveryFee(proid, num, data.msg.City);
                    }
                })
            }

            //订单数量为1，则减少数量不可用；否则可用。针对实物产品
            if ($("#orderNum").trimVal() != "1") {
                $("#reduceP").removeClass("enabled");
                $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + parseInt($("#hid_payprice").trimVal()) * parseInt($("#orderNum").trimVal()) + '</b>');
            }



            
         //装载产品规格
         <%if (manyspeci==1){ %>
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Selectguigelist",
                    data: { comid: comid,proid: $("#hid_proid").val()  },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {
                            $(".sys_item_spec").empty();
                            $("#ProductItemEdit").tmpl(data.msg).appendTo(".sys_item_spec");

                        }
                    }
                })
        <%} %>

           //与预定人相同
            $("#chkItem").live("click", function () {
                if ($(this).is(":checked")) {
                    if ($("#getName_1").trimVal() != "") {
                        $("#travelname1").val($("#getName_1").trimVal());
                        $("#travelphone1").val($("#getPhone_1").trimVal());
                    }
                    $("#hid_issamewithbooker").val("1");
                } else {
                    $("#hid_issamewithbooker").val("0");
                }
            });

        });

        function getDeliveryFee(proid, num, city) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=getexpressfee", { proid: proid, city: city, num: num }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {

                    $("#bfee").html(data.msg);
                }
            })
        }

        function showErr(a) {
            $("html").css({
                "overflow-y": "hidden"
            });
            if ($("#bgDiv").html() == null) {
                $('<div id="bgDiv"></div>').appendTo("body")
            }
            if ($("#showMsg").html() != null) {
                $("#showMsg").remove()
            }
            $('<div id="showMsg"><div class="msg-title">温馨提示</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr(\'' + a + '\')">知道了</a></div></div>').appendTo("body");
            var b = $(window).height();
            var d = $(window).scrollTop();
            var c = $("#showMsg").height();
            $("#bgDiv").css({
                height: $(document).innerHeight()
            }).show();
            $("#showMsg").css({
                top: (b - c) / 2
            }).show()
        }
        function hideErr(a) {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #showMsg").hide();
            if (a == '提单成功') {
                location.href = "/agent/m/order.aspx?comid=" + $("#hid_comidtemp").trimVal(); 
            }
            if (a == '请填写购买信息') {
                $("#a_ChooseAddress").parent().show();
                $("#tbody_address").show();
                $("#getName_1").parent().parent().parent().show();
                $("#getPhone_1").parent().parent().parent().show();
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

        function Viewdelivery(chked) {
            if (chked == 2)//快递
            {
                $("#delivery_tr1").show();
                $("#delivery_tr2").show();
                $("#delivery_tr3").show();
                $("#delivery_tr4").show();
            } else {//自提
                $("#delivery_tr1").hide();
                $("#delivery_tr2").hide();
                $("#delivery_tr3").hide();
                $("#delivery_tr4").hide();
            }
        }

        function addcart(proid) {
            //            showErr("添加到购物车成功");
            //            $(".m-dialog").css("display", "");
            //            $(".m-dialog-overlay").css("display", "");
            showCart("");

            $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comidtemp").trimVal(), proid: proid, u_num: $("#orderNum").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    searchcart();
                }
            })
        }
        function showCart(a) {
            $("html").css({
                "overflow-y": "hidden"
            });
            if ($("#bgDiv").html() == null) {
                $('<div id="bgDiv"></div>').appendTo("body")
            }
            if ($("#showcartmsg").html() != null) {
                $("#showcartmsg").remove()
            }
            $('<div id="showcartmsg" class="m-dialog" >  <div class="container" style="display: block;"> <div class="title">  <h3>  温馨提示</h3>  </div>  <a href="javascript:hideCart(\'' + a + '\');" class="btn close" title="关闭" name="item_guanbi">X</a><div class="content1"> <div class="pop-car-win"> <div class="pop-content">  <div class="pop-success no-products">  <h4>  <b></b>添加成功！</h4> <div class="clearfix">   <a name="item_goshopping" href="javascript:toShopping();" class="car-btn shopping-btn close l"> <span>继续购物</span></a><a name="item__gocart" href="javascript:toShoppingCart();" class="car-btn shopping-btn close l"><span>购物车结算</span></a></div>  </div>  </div>  </div> </div> </div> </div>').appendTo("body");
            var b = $(window).height();
            var d = $(window).scrollTop();
            var c = $("#showcartmsg").height();
            $("#bgDiv").css({
                height: $(document).innerHeight()
            }).show();
            $("#showcartmsg").css({
                top: (b - c) / 2
            }).show()
        }
        function hideCart(a) {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #showcartmsg").hide();

        }


        function toShoppingCart() {
            location.href = "ShopCart.aspx?comid=<%=comid_temp %>";
        }
        function toShopping() {
            location.href = "Manage_sales.aspx?comid=<%=comid_temp %>";
        }

        function goshopproject() {
            location.href = "/agent/m/Manage_sales.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goshopcart() {
            location.href = "/agent/m/ShopCart.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goorder() {
            location.href = "/agent/m/order.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function gofinane() {
            location.href = "/agent/m/Finane.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }

          function hoteldate(){
                        var indate=$("#datetime").val();
                        var outdate=$("#u_traveldate_out").val();
                        var proid=$("#hid_proid").val();
                        var comid=$("#hid_comid_temp").val();
                        var  Agentlevel='<%=Agentlevel %>';

                        if(indate !="" &&outdate !=""){

                        if(!dateduibi(indate,outdate)){
                            $("#hid_payprice").val(0);
                            $("#sPrice").html("离店时间要大于入住时间，请重新选择日期");
                        }



                        $.ajax({
                        type: "post",
                        url: "/JsonFactory/ProductHandler.ashx?oper=Hotelpagelist",
                        data: {comid: comid,startdate:indate,enddate:outdate,proid:proid,Agentlevel:Agentlevel  },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                 $("#sPrice").html("房满，请选择其他日期");
                                 $("#hid_payprice").val(0);
                                 $("#priceTotal").find("b").html("--");
                                return;
                            }
                            if (data.type == 100) {

                                $("#hid_MinEmptynum").val(data.msg[0].MinEmptynum);
                                if (data.msg[0].MinEmptynum == 0) {
                                    if(data.msg[0].allprice == 0){
                                         $("#sPrice").html("房满，请选择其他日期");
                                         $("#hid_payprice").val(0);
                                         $("#priceTotal").find("b").html("--");
                                    }else{
                                    //无控房，随时查房状态
                                        $("#sPrice").html(data.msg[0].allprice);
                                        $("#hid_payprice").val(data.msg[0].allprice);
                                        $("#priceTotal").find("b").html(parseInt(data.msg[0].allprice) * parseInt($("#orderNum").val()));
                                    }

                                } else {
                                    $("#sPrice").html(data.msg[0].allprice);
                                    $("#hid_payprice").val(data.msg[0].allprice);
                                    $("#priceTotal").html(parseInt(data.msg[0].allprice) * parseInt($("#orderNum").val()));

                                }

                            }
                       }
                    })
                    
                    }
                }


                function dateduibi(a, b) {

                      if (DateDiff(b,a)>0) {
                          return true;
                      } else {
                          alert("离店日期必须大于入住日期");
                          return false;
                      }
                 }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="inner">
        <!-- 公共页头  -->
        <header class="header"  style=" background-color: #3CAFDC;">
            <h1 id="h_comname"></h1>
            <div class="left-head"> 
              <a href="javascript:history.go(-1)" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="head-return"></span></span>
              </a> 
            </div>
            <div class="right-head"> 
                    <a href="loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>  
            </div> 
       </header>
        <div class="container ">
            <div class="tabber  tabber-n3 tabber-double-11 clearfix">
                <a href="javascript:goshopproject()" class="active">
                    产品列表</a> <a class="" href="javascript:goorder()">订单列表</a> <a href="javascript:gofinane()">
                        财务记录</a><a href="javascript:goshopcart()">购物车</a> 
            </div>
        </div>
        <!-- 页面内容块 -->
        <div id="orderFome" class="order-form">
            <div class="t-head">
                <div class="order-price" id="priceInfo">
                    在线支付：<span id="priceTotal">￥<b style="font-size: 18px;"></b></span></div>
            </div>
            <div class="container">
                <div class="w-item conlist">
                    <dl class="fn-clear">
                        <dt>产品：</dt>
                        <dd id="sName">
                        </dd>
                    </dl>
                    <dl class="fn-clear">
                        <dt>价格：</dt>
                        <dd>
                            <span class="f60  sys_item_price" id="sPrice"></span>
                        </dd>
                    </dl>
                    <dl class="fn-clear  shiwupro">
                        <dt>使用日期：</dt>
                        <dd>
                            <span class="f60" id="Span1"></span>
                        </dd>
                    </dl>
                    <dl class="fn-clear shiwupro">
                        <dt>使用限制：</dt>
                        <dd>
                            <span class="f60" id="Span2"></span>
                        </dd>
                    </dl>
                    <dl class="fn-clear">
                        <dt>说明：</dt>
                        <dd id="sDes">
                            <span class="isover" id="yuding_msg" style="display: none;"></span><span class="border">
                                ...</span> <s class="ico-right"></s>
                        </dd>
                    </dl>
                  
                </div>
                 <div class="w-item conlist"  style="display: none;">
                    <dl class="fn-clear"  >
                        <dt>行程：</dt>
                        <dd id="Dd1">
                            <span class="isover" id="Span3" style="display: none;"></span>
                            <span class="border"> ...</span> 
                            <s class="ico-right"></s>
                        </dd>
                    </dl>
                 </div>
                <div class="w-item" style="display: none;">
                    <dl class="in-item fn-clear" id="selDate">
                        <dt>日期</dt>
                        <dd>
                        <%if(servertype==14){%>
                          <input type="text" id="datetime" placeholder="请选择起保日期" value="" class="writeok">
                        <%}
                          else if(servertype==9){%>
                           <input type="text" id="datetime" placeholder="请选择入住日期" value="" class="writeok">
                          <%}
                          else{%>
                         <span id="datetime" class="writeok" size="12" value=""></span> 
                        <%}%> 
                            <em class="icon-right">
                            </em>
                        </dd>
                    </dl>
                </div>
                 <div class="w-item" style="display: none;">
                    <dl class="in-item fn-clear" id="dl_hoteloutdate">
                        <dt>日期</dt>
                        <dd> 
                            <input type="text" id="u_traveldate_out" placeholder="请选择离店日期" value="" class="writeok">
                            <em class="icon-right">
                            </em>
                        </dd>
                    </dl>
                </div>
                <p class="p-tips" id="p_proexplain">
                </p>
                <div class="w-item">
                    <dl class="in-item-number fn-clear">
                        <dt>数量</dt>
                        <dd>
                            <span id="reduceP" class="btn enabled" style="padding:0;"></span>
                            <input id="orderNum" type="text" value="<%=unum %>" class="ipt-numbers notice" maxlength="3"
                                readonly="readonly">
                            <span id="plusP" class="btn" style="padding:0;"></span><span style="vertical-align: top;
line-height: 50px;display:none;">成人</span>
                        </dd>
                    </dl>
                    <dl class="in-item-number fn-clear" style="display:none;">
                        <dt> </dt>
                        <dd>
                            <span id="Span4" class="btn enabled"></span>
                            <input id="child_orderNum" type="text" value="0" class="ipt-numbers notice" maxlength="3"
                                readonly="readonly">
                            <span id="Span5" class="btn"></span><span style="vertical-align: top;
line-height: 50px;display:none;">儿童</span>
                        </dd>
                    </dl>
                </div>
                <div style="width: 100%;   padding-bottom: 10px; display: none;">
                    <span style="  color: #f60; font-size: 15px; font-weight: bold; ">如果加入购物车购买，则可在购物车提单页面统一输入预订信息，以下预订信息无需填写</span> 
                     <a class="btn_code" href="javascript:void(0)" id="a_ChooseAddress" >使用常用地址</a>  
                  
                </div>
                 <div class="w-item">


                <!--规格属性-->
	            <div class="sys_item_spec">

	            </div>
	            <!--规格属性--> 
                </div>

                <div class="w-item">
                    <dl class="in-item fn-clear">
                        <dt>预订人</dt>
                        <dd>
                            <input type="text" id="getName_1" placeholder="请填写预订人姓名" value="" class="writeok">
                        </dd>
                    </dl>
                </div>
                <div class="w-item">
                    <dl class="in-item fn-clear borderTop">
                        <dt>手机号</dt>
                        <dd>
                            <input type="tel" id="getPhone_1" maxlength="11" placeholder="免费接收订单确认短信" value=""
                                class="writeok"></dd>
                    </dl>
                </div>
                <div class="w-item"  style="display:<%if (issetidcard==0){%>none<%} %>;">
                    <dl class="in-item fn-clear borderTop">
                        <dt>身份证</dt>
                        <dd>
                            <input type="tel" id="getIdcard" maxlength="18" placeholder="身份证号" value=""
                                class="writeok"></dd>
                    </dl>
                </div>
                <!--实物产品收货人地址信息-->
                <div class="w-item" id="tbody_address" style="display: none; padding-bottom: 20px;">
                    <dl class="in-item fn-clear">
                        <table style="width: 100%;">
                            <tr>
                                <td valign="top" colspan="2">
                                    <label>
                                        收货人地址:</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdleft" valign="top">
                                    <label>
                                        运送方式</label>
                                </td>
                                <td class="tdright">
                                    <label>
                                        <input name="deliverytype" type="radio" value="2" checked>
                                        快递(需运费)</label>
                                    <label>
                                        <input name="deliverytype" type="radio" value="4">
                                        自提(免运费)</label>
                                </td>
                            </tr>
                            <tr id="delivery_tr1">
                                <td valign="top" class="tdleft">
                                    <label>
                                        收货地址</label>
                                </td>
                                <td class="tdright">
                                    <select name="com_province" id="com_province" class="mi-input" style="width: 100%;
                                        margin-bottom: 10px;">
                                        <option value="省份" selected="selected">省份</option>
                                    </select>
                                    <br />
                                    <select name="com_city" id="com_city" class="mi-input" style="width: 100%;">
                                        <option value="城市" selected="selected">城市</option>
                                    </select>
                                </td>
                            </tr>
                            <tr id="delivery_tr2">
                                <td valign="top" class="tdleft">
                                    <label>
                                        详细地址</label>
                                </td>
                                <td class="tdright">
                                    <input name="txtaddress" type="text" class="dataNum dataIcon" id="txtaddress" value=""
                                        placeholder="请输入详细地址" />
                                </td>
                            </tr>
                            <tr id="delivery_tr3">
                                <td valign="top" class="tdleft">
                                    <label>
                                        邮&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;编</label>
                                </td>
                                <td class="tdright">
                                    <input name="txtcode" type="tel" class="dataNum dataIcon" id="txtcode" value="" placeholder="请输入邮编(选填)" />
                                </td>
                            </tr>
                            <tr id="delivery_tr4">
                                <td valign="top" colspan="2">
                                    <strong>您需为订单支付<b id="bfee" style="font-size: 12px; color: #ff0000;">0</b>元运费</strong>
                                </td>
                            </tr>
                        </table>
                    </dl>
                </div>
                <!--旅游大巴附加信息-->
                <div class="w-item" name="tbody_busplus" style="display: none;">
                    <dl class="in-item fn-clear">
                        <dt>上车地点</dt>
                        <dd>
                            <select id="pointuppoint" class="mi-input">
                            </select>
                        </dd>
                    </dl>
                </div>
                <div class="w-item" name="tbody_busplus" style="display: none;" id="tr_dropoffpoint">
                    <dl class="in-item fn-clear">
                        <dt>下车地点</dt>
                        <dd>
                            <select id="dropoffpoint" class="mi-input">
                            </select>
                        </dd>
                    </dl>
                </div>
                <div class="w-item" name="tbody_busplus" style="display: none;">
                    <dl class="in-item fn-clear">
                        <dt>更多需求</dt>
                        <dd>
                            <input type="text" id="order_remark" value="" />
                        </dd>
                    </dl>
                </div>
                <div class="w-item" style="display: none; margin-top: 50px;" id="div_travelbus">
                    <dl class="in-item fn-clear">
                        <dd>
                            乘车人信息:</dd>
                        <dd>
                            <table width="700px" class="grid" id="travel_tb" style="display: none;">
                                <tbody id="travel_tbody">
                                </tbody>
                            </table>
                        </dd>
                        <dd>
                            </dd>
                    </dl>
                </div>
                <div class="w-item" style="display: none; margin-top: 50px;" id="baoxian_tb">
                    <dl class="in-item fn-clear">
                        <dd>
                            被保险人信息:</dd>
                        <dd>
                            <table width="700px" class="grid" >
                                <tbody id="baoxian_tbody">
                                </tbody>
                            </table>
                        </dd>
                         
                    </dl>
                </div>
            </div>
            <%if (iscanbook == 1)
              {
                  if (servertype != 11)
                  { 
            %>
            <div class="order-btn fn-clear" id="suborder">
                <div class="">
                    <input type="button" class="btn" id="submitBtn1" value="在线购买">
                </div>
            </div>
            <%
                }
              }
              else
              { 
            %>
            <div class="order-btn fn-clear" id="suborder" style="background-color: #888888; border: 0;">
                <div class="">
                    <input type="button" class="btn" disabled="disabled" id="Button1" value="已售完" style="background-color: #888888;">
                </div>
            </div>
            <%
                } %>
        </div>
    </div>
    <div id="calDiv" style="display: none; margin-top: -40px">
    </div>
    <div id="calDivOut" style="display: none; margin-top: 20px">
    </div>
    <div style="height: 565px; display: none;" id="bgDiv">
    </div>
    <div id="showMsg" style="top: 352px; display: none;">
        <div class="msg-title">
            温馨提示</div>
        <div class="msg-content">
            请填写预订人姓名！</div>
        <div class="msg-btn">
            <a href="javascript:;" onclick="hideErr()">知道了</a></div>
    </div>
    <!--苏宁购物车begin-->
    <%if (iscanbook == 1)
      {
          if (servertype == 11)
          { %>
    <div class="sn-block sn-fixed buy-bar" style="">
        <div class="wbox" id="buyBtn">
            <a href="javascript:void(0)" class="buyNow" id="submitBtn1">立即购买</a> <span style="display: none;
                margin-right: 0.8px;" class="gray-detail-disable">立即购买</span> <a href="javascript:void(0)"
                    class="appendToCart" id="addShoppingCart" onclick="addcart('<%=id %>')">加入购物车</a>
            <span style="display: none;" class="gray-detail-disable">加入购物车</span>
            <%-- <div class="cart">
               <a href="ShopCart.aspx?comid=<%=comid_temp %>" id="cartNum"></a>
            </div>--%>
        </div>
    </div>
    <%}
      } %>
    <!--苏宁购物车end-->
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <%--<input id="hid_hotel_agent" type="hidden" value="<%=hotel_agent %>" />--%>
    <input id="hid_proid" type="hidden" value="<%=id %>" />
    <input id="hid_payprice" type="hidden" value="0" />
    <input id="hid_server_type" type="hidden" value="0" />
    <input id="hidLeavingDate" type="hidden" value="" />
    <input id="hidMinLeavingDate" type="hidden" value="" />
    <input id="hidLinePrice" type="hidden" value="" />
    <input id="hidchildreduce" type="hidden" value="0" />
    <input id="hidEmptyNum" type="hidden" value="" />
    <!--上车地点，下车地点-->
    <input id="hid_pickuppoint" type="hidden" value="<%=pickuppoint %>" />
    <input id="hid_dropoffpoint" type="hidden" value="<%=dropoffpoint %>" />
    <!--乘车人信息 与预订人相同-->
    <input type="hidden" id="hid_issamewithbooker" value="0" />
    <!--常用地址id; ，目前以下参数都是实物产品用到，其他产品不用关心-->
    <input id="hid_addrid" type="hidden" value="<%=addrid %>" />
    <input id="hid_manyspeci" value="<%=manyspeci %>" type="hidden"  />
    <input id="hid_speciid" value="0" type="hidden"  />

<script type="text/x-jquery-tmpl" id="ProductItemEdit"> 

        <dl class="clearfix iteminfo_parameter sys_item_specpara" data-sid="${GuigeNum}">
			<dt>${GuigeTitle}</dt>
			<dd>
				<ul class="sys_spec_text">
                
                    {{each GuigeValues}}  
                        <li data-aid="${$value.id}"><a href="javascript:;" title="${$value.Name}">${$value.Name}</a><i></i></li>
                    {{/each}}
               
                </ul>

			</dd>
		</dl>
</script>
    <script>

  //价格json


         var sys_item = {
            "mktprice": "<%=face_price %>",
            "price": "<%=price %>",
            "sys_attrprice": {<% if(gglist != null){
              for (int i=0;i<gglist.Count();i++){
              %>"<%=gglist[i].speci_type_nameid_Array.Replace("-","_") %>": { "price": "<%=gglist[i].speci_advise_price.ToString("0.00")%>", "mktprice": "<%=gglist[i].speci_face_price.ToString("0.00")%>", "speciid": "<%=gglist[i].id%>" } ,
              <%}} %>
              }
              };

        //商品规格选择
        $(function () {
            $(".sys_item_spec .sys_item_specpara").each(function () {
                var i = $(this);
                var p = i.find("ul>li");
                p.click(function () {
                    if (!!$(this).hasClass("selected")) {
                        $(this).removeClass("selected");
                        i.removeAttr("data-attrval");
                    } else {
                        $(this).addClass("selected").siblings("li").removeClass("selected");
                        i.attr("data-attrval", $(this).attr("data-aid"))
                    }
                    getattrprice() //输出价格
                })
            })

            //获取对应属性的价格
            function getattrprice() {
                var defaultstats = true;
                var _val = '';
                var _resp = {
                    mktprice: ".sys_item_mktprice",
                    price: ".sys_item_price"
                }  //输出对应的class
                $(".sys_item_spec .sys_item_specpara").each(function () {
                    var i = $(this);
                    var v = i.attr("data-attrval");
                    if (!v) {
                        defaultstats = false;
                    } else {
                        _val += _val != "" ? "_" : "";
                        _val += v;
                    }
                })
                if (!!defaultstats) {
                    _mktprice = sys_item['sys_attrprice'][_val]['mktprice'];
                    _price = sys_item['sys_attrprice'][_val]['price'];
                     $("#hid_speciid").val(sys_item['sys_attrprice'][_val]['speciid'])
                    
                } else {
                    _mktprice = sys_item['mktprice'];
                    _price = sys_item['price'];
                }
                //输出价格
                $(_resp.mktprice).text(_mktprice);  ///其中的math.round为截取小数点位数
                $(_resp.price).text(_price);
                $("#hid_payprice").val(_price);
                $("#priceTotal").html('￥<b style=" font-size:18px;">'+_price*$("#orderNum").trimVal()+'</b>');
            }
        })
</script>

    <script type="text/javascript">
        var province = document.getElementById('com_province');
        var city = document.getElementById('com_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</asp:Content>
