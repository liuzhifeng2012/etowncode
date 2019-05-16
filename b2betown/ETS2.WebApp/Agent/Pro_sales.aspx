<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"
    CodeBehind="Pro_sales.aspx.cs" Inherits="ETS2.WebApp.Agent.Pro_sales" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-orderrili.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script src="/Scripts/convertToPinyinLower.js" type="text/javascript"></script>
    <script src="/Scripts/shopcart.js" type="text/javascript"></script>
    <link    href="http://shop.etown.cn/h5/order/css/base.css" rel="stylesheet">
    <script src="/Scripts/common.js" type="text/javascript"></script> 
    <style type="text/css">
        /*儿童人数增加减少*/
        .btn-reduce2
        {
            left: 0px;
            background-position: -216px -190px;
            background-image: url('/images/newicon.png');
            background-repeat: no-repeat;
        }
        .btn-add2
        {
            right: 0px;
            background-position: -232px -190px;
            background-image: url('/images/newicon.png');
            background-repeat: no-repeat;
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
        .dialog
        {
            position: fixed;
            _position: absolute;
            z-index: 10;
            border: 1px solid #CCC;
            text-align: center;
            font-size: 14px;
            background-color: #F4F4F4;
            overflow: hidden;
        }
    </style>
    <%if (Server_type == 14)
      { %>
    <!--电脑端手机端兼容日历 js css-->
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.core-2.5.2.js"
        type="text/javascript"></script>
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.core-2.5.2-zh.js"
        type="text/javascript"></script>
    <link href="http://shop.etown.cn/Scripts/mobiscroll/dev/css/mobiscroll.core-2.5.2.css"
        rel="stylesheet" type="text/css" />
    <link href="http://shop.etown.cn/Scripts/mobiscroll/dev/css/mobiscroll.animation-2.5.2.css"
        rel="stylesheet" type="text/css" />
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.datetime-2.5.1.js"
        type="text/javascript"></script>
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.datetime-2.5.1-zh.js"
        type="text/javascript"></script>
    <!-- S 可根据自己喜好引入样式风格文件 -->
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.android-ics-2.5.2.js"
        type="text/javascript"></script>
    <link href="http://shop.etown.cn/Scripts/mobiscroll/dev/css/mobiscroll.android-ics-2.5.2.css"
        rel="stylesheet" type="text/css" />
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

			$("#u_traveldate").val('').scroller('destroy').scroller($.extend(opt['date'], opt['default']));
		    
        });
    </script>
    <!--电脑端手机端兼容日历 js css-->
    <%} %>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var proid = $("#hid_proid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            SearchList(1); 
      
            function SearchList(pageindex) {
                var pro_youxiaoqi = "";
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
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
                            $("#Pro_name").text(data.msg.Pro_name);

                            if (data.msg.Server_type == 10)//旅游大巴
                            { 

                              //日历
                                $("#u_traveldate").bind("click",function () {
                                    scrollTo(0, 1);
                                    $("#calDiv").fadeIn();
                                    $("#setting-home").hide();
                                    $("#secondary-tabs").hide();
                                });

                                $("#u_num").attr("readonly", "readonly");

                                $("#travel_tb").show();
                                $("#tbody_busplus").show();
                                $("#order_remark").parent().parent().parent().hide();

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





                                $("#pointuppoint").html(pickuppointstr);
                                $("#dropoffpoint").html(dropoffpointstr);

                                $("#travel_tbody").append(
                 '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                    '<label>姓名：</label><input type="text" id="travelname1" value="" style="width:50px;"/>' +
                    '<label>联系手机：</label><input type="text" id="travelphone1" value=""  style="width:80px;" />' +
                    '<label>身份证：</label><input type="text" id="travelidcard1" value="" style="width:140px;" />' +
                    '<label style="display:none;">民族：</label><input type="text" id="travelnation1" value="汉族"   style="width:40px;display:none;"/>' +
                    '<label>备注：</label><input type="text" id="travelremark1" value=""  style="width:100px;" />' +
                    '<input name="chkItem" type="checkbox" value="1" id="chkItem" />与预定人相同' +
                    '</td>' +
                '</tr> ');

                                $("#u_traveldate").parent().parent().show();
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

                                    //                                    $("#u_traveldate").val(firstdate);
                                    $("#u_traveldate").val("");
                                    $("#Perioddate").html(firstdate);
                                     
                              




                                    //団期日历
                                    $("#calDiv").datepicker({
                                        minDate: $("#hidMinLeavingDate").val(),
                                        onSelect: function (dateText) {
                                            $("#u_traveldate").val(dateText);
                                            $("#Perioddate").html(dateText);
                                            var datearr = $("#hidLeavingDate").val().split(",");
                                            var pricearr = $("#hidLinePrice").val().split(",");
                                            var emptyarr = $("#hidEmptyNum").val().split(",");
                                            var childreduce = $("#hidchildreduce").val();

                                            for (var i = 0; i < datearr.length; i++) {
                                                if (datearr[i] == dateText) {
                                                    <%if (manyspeci == 1){%>  
                                                     
                                                     <%}else{ %>
                                                        $("#Advise_price").html(pricearr[i]);
                                                        $("#hid_payprice").val(pricearr[i]);
                                                        $("#heji").html('￥' + parseInt(pricearr[i]) * parseInt($("#u_num").val()));
                                                        //$("#childPrice").html(parseInt(pricearr[i]) - parseInt(childreduce));
                                                     <%}%>
                                                    $("#" + dateText).addClass("selected"); //对选中的增加
                                                } else {
                                                    //$("#" + datearr[i]).removeClass("selected"); //对未选中的日期移除
                                                }
                                            }


                                            $("#calDiv").hide();
                                            $("#setting-home").show();
                                            $("#secondary-tabs").show();

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
                                                return [true, "hasCourse", "<p>￥" + dayprice + "</p><p style=\"display:;font-size:12px;color:black;\">余" + dayempty + "</p>"];

                                            } else {
                                                return [false, "noCourse", '无团期'];
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

                                   <%if (manyspeci == 1){%> 
                                    $("#hid_payprice").val('<%=price %>');
                                    $("#heji").text('￥<%=price %>' );
                                    $("#Advise_price").html('<%=price %>');
                                    $("#travel_tb").hide();
                                  <%}else{ %>
                                    $("#hid_payprice").val(firstdate_price);
                                    $("#heji").text('￥' + firstdate_price);
                                    $("#Advise_price").html(firstdate_price);
                                       $("#travel_tb").show();
                                  <%} %>
                            } 
                           else if (data.msg.Server_type == 9)//订房
                            {
                              //日历
                                $("#u_traveldate").bind("click",function () {
                                    scrollTo(0, 1);
                                    $("#calDiv").fadeIn();
                                    $("#setting-home").hide();
                                    $("#secondary-tabs").hide();
                                });

                                $("#u_traveldate_out").bind("click",function () {
                                    scrollTo(0, 1);
                                    $("#calDivOut").fadeIn();
                                    $("#setting-home").hide();
                                    $("#secondary-tabs").hide();
                                });

                                $("#u_num").attr("readonly", "readonly");

                                //$("#travel_tb").show();
                                $("#hoteloutdate").show();
                                $("#order_remark").parent().parent().parent().hide();
                                $("#pointuppoint").parent().parent().parent().hide();
                                $("#travel_tbody").parent().parent().parent().hide();

                                $("#u_traveldate").parent().parent().show();

                                $("#u_traveldate").prev().text("入住日期");


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

                                    //                                    $("#u_traveldate").val(firstdate);
                                    $("#u_traveldate").val("");
                                    $("#Perioddate").html(firstdate);

                                    <%if (manyspeci == 1){%>
                                    
                                     $("#hid_payprice").val('<%=price %>');
                                    $("#heji").text('￥<%=price %>' );
                                    $("#Advise_price").html('<%=price %>');
                                    <%}else{ %>
                                    $("#hid_payprice").val(firstdate_price);
                                    $("#heji").text('￥' + firstdate_price);
                                    $("#Advise_price").html(firstdate_price);
                                    <%} %>




                                    //入住日历
                                    $("#calDiv").datepicker({
                                        minDate: $("#hidMinLeavingDate").val(),
                                        onSelect: function (dateText) {
                                            $("#u_traveldate").val(dateText);
                                            $("#Perioddate").html(dateText);
                                            var datearr = $("#hidLeavingDate").val().split(",");
                                            var pricearr = $("#hidLinePrice").val().split(",");
                                            var emptyarr = $("#hidEmptyNum").val().split(","); 
                                            var childreduce = $("#hidchildreduce").val();

                                            hoteldate();

                                            $("#calDiv").hide();
                                            $("#setting-home").show();
                                            $("#secondary-tabs").show();

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
                                                    return [true, "hasCourse", "<p>￥" +dayprice + "<laber style=color:#ff0000;>("+dayempty+")</laber></p>"];
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
                                            $("#Perioddate").html(dateText);
                                            hoteldate();


                                            $("#calDivOut").hide();
                                            $("#setting-home").show();
                                            $("#secondary-tabs").show();

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
                                                    return [true, "hasCourse", "<p>￥" +dayprice + "<laber style=color:#ff0000;>("+dayempty+")</laber></p>"];
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

                                $("#pro_youxiaoqi").html("仅限当天使用")


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
                           else  if (data.msg.Server_type == 14)//保险产品
                            {
                                $("#u_num").attr("readonly", "readonly");
                                $("#u_num").parents("td").prev().text("被保险人数量:");

                                $("#u_traveldate").parents(".bookOrderDetails").show();
                                $("#u_traveldate").prev("span").text("起保日期:");
                                $("#u_traveldate").parent().next(".bookLimit").hide();
                                $("#u_traveldate").prev().css("margin-right","60px");
                                

                               
                                $("#u_name").parents(".grid").prev().text("提单人信息");
                                $("#u_name").siblings().hide();
                                $("#u_name").next().show();

                                $("#baoxian_tb").show();
                                $("#baoxian_tbody").append(
                 '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                    '<label>姓名：</label><input type="text" name="baoxianname" id="baoxianname1" value="" style="width:50px;"/>' +
                    '<label>拼音/英文名：</label><input type="text" name="baoxianpinyinname" id="baoxianpinyinname1" value="" style="width:80px;"/>' + 
                    '<label>身份证：</label><input type="text" name="baoxianidcard" id="baoxianidcard1" value="" style="width:140px;" />' +  
                    '</td>' +
                '</tr> ');
                  
                                //名称
                                $(".titlepng").html(data.msg.Pro_name);
                                //单价暂时设为0
                                $("#Advise_price").text("0");
                                //合计价格设为0
                                $("#heji").text("0");
                                 
                                //使用限制隐藏
                                $("#pro_youxiaoqi").parents(".shiwupro").hide();
                                //有限期隐藏
                                $("#Perioddate").html(ChangeDateFormat(data.msg.Pro_start) + " 至 " + ChangeDateFormat(data.msg.Pro_end));
                                $("#Perioddate").parents(".shiwupro").hide();

                                //▼预订详情
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
                                //▲预订详情

                                //提单备注隐藏
                                $("#order_remark").parent().parent().parent().hide();
                            }
                           else if (data.msg.Server_type == 2 || data.msg.Server_type == 8) //当地游，跟团游
                            {
                                //日历
                                $("#u_traveldate").bind("click",function () {
                                    scrollTo(0, 1);
                                    $("#calDiv").fadeIn();
                                    $("#setting-home").hide();
                                    $("#secondary-tabs").hide();
                                });

                                $("#secondary-tabs").show();
                                $("#setting-home").show();
                                $(".titlepng").html(data.msg.Pro_name);
                                $("#u_traveldate").parent().parent().show();

                                $("#labelmsg").text("成人");
                                $("#labelmsg2").text("儿童");
                                $("#labelmsg2").parent().parent().parent().show();

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
                                    //                                $("#dateinfo").html(datestr); //显示団期
                                    $("#hidLeavingDate").val(listdate); //日期列表
                                    $("#hidMinLeavingDate").val(firstdate); //第一天
                                    $("#hidLinePrice").val(listprice); //价格列表
                                    $("#hidEmptyNum").val(listempty); //空位列表

                                    $("#u_traveldate").val(firstdate);
                                    $("#Perioddate").html(firstdate);
                                    $("#hid_payprice").val(firstdate_price);
                                    $("#heji").text('￥' + firstdate_price);
                                    //$("#Advise_price").html(firstdate_price);

                                    $("#Advise_price").parent().html('<label id="Advise_price">成人:￥' + firstdate_price + ',儿童:￥' + parseInt(firstdate_price - childreduce) + '</label>');

                                    //団期日历
                                    $("#calDiv").datepicker({
                                        minDate: $("#hidMinLeavingDate").val(),
                                        onSelect: function (dateText) {
                                            $("#u_traveldate").val(dateText);
                                            $("#Perioddate").html(dateText);
                                            var datearr = $("#hidLeavingDate").val().split(",");
                                            var pricearr = $("#hidLinePrice").val().split(",");
                                            var emptyarr = $("#hidEmptyNum").val().split(",");


                                            for (var i = 0; i < datearr.length; i++) {
                                                if (datearr[i] == dateText) {
                                                    //$("#Advise_price").html(pricearr[i]);
                                                    $("#Advise_price").parent().html('<label id="Advise_price">成人:￥' + pricearr[i] + ',儿童:￥' + parseInt(pricearr[i] - childreduce) + '</label>');
                                                    $("#hid_payprice").val(pricearr[i]);
                                                    $("#heji").html('￥' + parseInt(parseInt(pricearr[i]) * parseInt($("#u_num").val()) + parseInt(pricearr[i] - childreduce) * parseInt($("#u_childnum").val())));
                                                    //$("#childPrice").html(parseInt(pricearr[i]) - parseInt(childreduce));
                                                    $("#" + dateText).addClass("selected"); //对选中的增加
                                                } else {
                                                    //$("#" + datearr[i]).removeClass("selected"); //对未选中的日期移除
                                                }
                                            }


                                            $("#calDiv").hide();
                                            $("#setting-home").show();
                                            $("#secondary-tabs").show();

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

                                $("#u_num").attr("readonly", "readonly");
                                $("#u_childnum").attr("readonly", "readonly");
                                $("#order_remark").parent().parent().parent().show();
                                $("#ticket_remark").html(data.msg.Pro_Remark);
                                $(".bookLimit").hide();
                                $("#Perioddate").parent().parent().hide(); //有效期隐藏
                                $("#pro_youxiaoqi").parent().parent().hide(); //使用限制隐藏

                                $("[name='saveaddress']").parent().hide(); //保存常用地址隐藏
                                $("[name='uesoldaddress']").hide(); //选择常用地址隐藏
                                $("[name='confirmButton']").next().hide(); //选择常用地址隐藏
                                $("#a_linedetail").parent().parent().show();

                                var yuding_msg = "<span id=\"msgview\" style=\"padding-left:10px; color:#ff0000;cursor:pointer;\">查看详情+</span><div id=\"msg\" style=\"display:none;\">";
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
                                yuding_msg += "</div>";
                                $("#yuding_msg").html(yuding_msg);
                            }
                            else //票务；实物产品
                            {
                             
                                $("#u_num").removeAttr("readonly");

                                if (data.msg.Server_type == 11) {
                                    $("#tbody_address").show();
                                    if (data.msg.Ispanicbuy == 2) {//如果是限购
                                        //显示库存量
                                        $("#labelmsg").html("库存量:<span style='color:red; '>" + data.msg.Limitbuytotalnum + "</span>");
                                    }
                                    $(".shiwupro").hide(); //如果是实物 ，则 隐藏时间
                                }

                                 
                                //如果是实物产品显示出运费模块
                                $("#heji").text("￥" + data.msg.Advise_price);
                                $("#Advise_price").html(data.msg.Advise_price);
                                $("#hid_payprice").val(data.msg.Advise_price);
                                  
                                  
                                $("#ticket_remark").html(data.msg.Pro_Remark);
                                $(".titlepng").html(data.msg.Pro_name);
                                $("#Perioddate").html(ChangeDateFormat(data.msg.Pro_start) + " 至 " + ChangeDateFormat(data.msg.Pro_end));

                                if (data.msg.Iscanuseonsameday == 0) {
                                    pro_youxiaoqi += "购买当天不可使用，";
                                }



                                if (data.msg.ProValidateMethod == "2") {

                                    //结束时间
                                    if (data.msg.Appointdata == 1) {
                                        pro_youxiaoqi += "出票一周有效";
                                    }

                                    if (data.msg.Appointdata == 2) {
                                        pro_youxiaoqi += "出票一月有效";
                                    }

                                    if (data.msg.Appointdata == 3) {
                                        pro_youxiaoqi += "出票三月有效";
                                    }

                                    if (data.msg.Appointdata == 4) {
                                        pro_youxiaoqi += "出票半年有效";
                                    }

                                    if (data.msg.Appointdata == 5) {
                                        pro_youxiaoqi += "出票一年有效";
                                    }


                                } else {
                                    pro_youxiaoqi += "同产品有效期";
                                }



                                $("#pro_youxiaoqi").html(pro_youxiaoqi)
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
                                yuding_msg += "</div>"
                                $("#yuding_msg").html(yuding_msg);
                            }

                            //如果服务类型是 票务； 则产品说明信息中 显示 电子码使用限制
                            if (data.msg.Server_type == 1) {
                                var remarkk = $("#yuding_msg").html();
                                if (data.msg.Iscanuseonsameday == 0)//电子码当天不可用
                                {
                                    $("#yuding_msg").html("此产品当天预订不可用" + remarkk);
                                }
                                if (data.msg.Iscanuseonsameday == 1)//电子码当天可用
                                {
                                    $("#yuding_msg").html("当天可用" + remarkk);
                                }
                                if (data.msg.Iscanuseonsameday == 2)//电子码出票2小时内不可用
                                {
                                    $("#yuding_msg").html("此产品出票2小时内不可用" + remarkk);
                                }
                                 //票务增加预约日期选择
                                 var curentDate=CurentDate(); 
                                 $("#u_traveldate").val(curentDate);  
                               
                                if(data.msg.isSetVisitDate==1){ 
                                     //日历
                                    $("#u_traveldate").bind("click",function () {
                                        scrollTo(0, 1);
                                        $("#calDiv").fadeIn();
                                        $("#setting-home").hide();
                                        $("#secondary-tabs").hide();
                                    }); 
                                   $("#u_traveldate").parent().parent().show(); 
                                     //団期日历
                                    $("#calDiv").datepicker({
                                        minDate: curentDate,
                                        onSelect: function (dateText) {
                                            $("#u_traveldate").val(dateText);
                                            $("#Perioddate").html(dateText);
                                            
                                            $("#calDiv").hide();
                                            $("#setting-home").show();
                                            $("#secondary-tabs").show(); 
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
                        }
                    }
                })


                function hoteldate(){
                        var indate=$("#u_traveldate").val();
                        var outdate=$("#u_traveldate_out").val();
                        var proid=$("#hid_proid").val();
                        var comid=$("#hid_comid_temp").val();
//                      var hotel_agent=$("#hid_hotel_agent").val();
                        var  Agentlevel='<%=Agentlevel %>';

                        if(indate !="" &&outdate !=""){

                        if(!dateduibi(indate,outdate)){
                            $("#hid_payprice").val(0);
                            $("#Advise_price").html("离店时间要大于入住时间，请重新选择日期");
                        }



                        $.ajax({
                        type: "post",
                        url: "/JsonFactory/ProductHandler.ashx?oper=Hotelpagelist",
                        data: {projectid:$("#hid_projectid").val(), comid: comid,startdate:indate,enddate:outdate,proid:proid,Agentlevel:Agentlevel },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                 $("#Advise_price").html("房满，请选择其他日期");
                                 $("#hid_payprice").val(0);
                                 $("#heji").html("--");
                                return;
                            }
                            if (data.type == 100) {

                                $("#hid_MinEmptynum").val(data.msg[0].MinEmptynum);
                                if (data.msg[0].MinEmptynum == 0) {
                                    if(data.msg[0].allprice == 0){
                                        $("#Advise_price").html("房满，请选择其他日期");
                                        $("#hid_payprice").val(0);
                                        $("#heji").html("--");
                                    }else{
                                    //无控房，随时查房状态
                                        $("#Advise_price").html(data.msg[0].allprice);
                                        $("#hid_payprice").val(data.msg[0].allprice);
                                        $("#heji").html('￥' + parseInt(data.msg[0].allprice) * parseInt($("#u_num").val()));
                                    }

                                } else {
                                    $("#Advise_price").html(data.msg[0].allprice);
                                    $("#hid_payprice").val(data.msg[0].allprice);
                                    $("#heji").html('￥' + parseInt(data.msg[0].allprice) * parseInt($("#u_num").val()));

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



                $("#msgview").click(function () {
                    var msgview = $("#msgview").html();
                    if (msgview == "查看详情+") {
                        $("#msg").show();
                        $("#msgview").html("隐藏-")
                    } else {
                        $("#msgview").html("查看详情+")
                        $("#msg").hide();
                    }


                })





                //提交订单
                $("#confirmButton").click(function () {
                    var proid = $("#hid_proid").trimVal();
                    var u_num = $("#u_num").trimVal();
                    var u_name = $("#u_name").trimVal();
                    var u_phone = $("#u_phone").trimVal();
                    var u_idcard = $("#u_idcard").trimVal();
                    var u_traveldate = $("#u_traveldate").trimVal();
                    var u_traveldate_out = $("#u_traveldate_out").trimVal();


                    var deliverytype = $("input:radio[name='deliverytype']:checked").trimVal();
                    var province = $("#com_province").trimVal();
                    var city = $("#com_city").trimVal();
                    var address = $("#txtaddress").trimVal();
                    var txtcode = $("#txtcode").trimVal();

                    var saveaddress = $("input:checkbox[name='saveaddress']:checked").trimVal(); ;
                    var u_childnum = $("#u_childnum").trimVal();
                    var childreduce = $("#hidchildreduce").trimVal();


                    var manyspeci = $("#hid_manyspeci").val();
                    var speciid = $("#hid_speciid").val();

                    if(manyspeci==1){
                        if(speciid==0){
                            alert("请选择具体规格");
                            $("#loading").hide();
                            return;
                        }
                    }

                    if (u_name == "") {
                        $.prompt("请填写订单姓名");
                        $("#u_name").focus();
                        return;
                    }
                    if (u_phone == "") {
                        $.prompt("请填写订单手机号");
                        $("#u_phone").focus();
                        return;
                    } else {
                        if (!isMobel(u_phone)) {
                            $.prompt("请正确填写订单手机号");
                            $("#u_phone").focus();
                            return;
                        }
                    }

                    <%if (issetidcard==1){%>
                        if(u_idcard==""){
                              $.prompt("请填写身份证号");
                              $("#u_idcard").focus();
                              $("#loading").hide();
                            return;
                        }
                
                    <%} %>


                    if ($("#hid_server_type").trimVal() == 11) {//实物产品
                        if (deliverytype == 2) {
                            if (province == "省份") {
                                alert("请选择省份");
                                return;
                            }
                            if (city == "城市") {
                                alert("请选择城市");
                                return;
                            }
                            if (address == "") {
                                alert("请输入详细地址");
                                return;
                            }

                        }
                    }

                    if ($("#hid_server_type").trimVal() == 10) {
                        //判断游玩日期 不可为空
                        if (u_traveldate == "") {
                            alert("请选择游玩日期");
                            return;
                        }
                    }


                    var travelnames = ""; //乘车人姓名列表
                    var travelidcards = ""; //乘车人身份证列表
                    var travelnations = ""; //乘车人民族列表
                    var travelphones = ""; //乘车人联系电话列表
                    var travelremarks = ""; //乘车人备注列表


                    var errid = "0"; // 输入格式错误的控件id
                    if ($("#hid_server_type").trimVal() == 10) {

                     <%if (manyspeci != 1){%> 

                        //判断乘车人信息不可为空
                        for (var i = 1; i <= u_num; i++) {
                            var travel_name = $("#travelname" + i).trimVal();
                            var travel_idcard = $("#travelidcard" + i).trimVal();
                            var travel_nation = $("#travelnation" + i).trimVal();
                            var travel_phone = $("#travelphone" + i).trimVal();
                            var travel_remark = $("#travelremark" + i).trimVal();


                            if (travel_name == "") {
                                alert("乘车人的姓名不可为空");
                                errid = "travelname" + i;
                                break;
                            }
                            if (travel_phone == "") {
                                alert("乘车人的电话不可为空");
                                errid = "travelphone" + i;
                                break;
                            }
//                            if (travel_idcard == "") {
//                                alert("乘车人的身份证号不可为空");
//                                errid = "travelidcard" + i;
//                                break;

//                            } else {
//                                if (!IdCardValidate(travel_idcard)) {
//                                    alert("乘车人的身份证号(" + travel_name + ")格式错误");
//                                    errid = "travelidcard" + i;
//                                    break;
//                                }
//                            }
                            if(travel_idcard!="")
                            {
                                if (!IdCardValidate(travel_idcard)) {
                                    alert("乘车人的身份证号(" + travel_name + ")格式错误");
                                    errid = "travelidcard" + i;
                                    break;
                                }
                            }
                            if (travel_nation == "") {
                                alert("乘车人的民族不可为空");
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
                      <%} %>
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


                        if (!dateduibi(u_traveldate,u_traveldate_out)){
                           
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
                                alert("被保险人的身份证号不可为空");
                                baoxian_errid = "baoxianidcard" + i;
                                $("#baoxianidcard"+i).css("background-color","aliceblue");
                                break;

                            } else {
                                if (!IdCardValidate(baoxian_idcard)) {
                                    alert("被保险人的身份证号(" + baoxian_idcard + ")格式错误");
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
                    $.post("/JsonFactory/OrderHandler.ashx?oper=agentorder", {baoxiannames:baoxiannames,baoxianpinyinnames:baoxianpinyinnames,baoxianidcards:baoxianidcards, u_childnum: u_childnum, deliverytype: deliverytype, province: province, city: city, address: address, txtcode: txtcode, agentid: agentid, comid: comid, proid: proid,speciid:speciid, u_num: u_num, u_name: u_name, u_phone: u_phone,u_idcard:u_idcard, u_traveldate: u_traveldate, travelnames: travelnames, travelidcards: travelidcards, travelnations: travelnations, travelphones: travelphones, travelremarks: travelremarks, travel_pickuppoints: $("#pointuppoint").trimVal(), travel_dropoffpoints: $("#dropoffpoint").trimVal(), order_remark: $("#order_remark").trimVal(), saveaddress: saveaddress, payprice: $("#hid_payprice").trimVal(),start_date:u_traveldate,end_date:u_traveldate_out,lastarrivaltime:"6点",fangtai:"" }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            if (data.msg == "yfkbz") {
                                alert("您的预付款不足，立即跳转到支付页面");

                                //创建支付订单
                                $.post("/JsonFactory/OrderHandler.ashx?oper=agentRecharge", { agentid: agentid, comid: comid, payprice: data.money, u_name: "分销客户:" + u_name, u_phone: u_phone, handlid: data.id }, function (data1) {
                                    data1 = eval("(" + data1 + ")");
                                    if (data1.type == 1) {
                                        $.prompt(data1.msg);
                                        
                                        return;
                                    }
                                    if (data1.type == 100) {
                                        location.href = "pay.aspx?orderid=" + data1.msg + "&comid=" + comid + "&agentorderid=" + data.id;
                                        return;
                                    }
                                })


                                //$('#confirmButton').show();
                                $('#spLoginLoading').hide();

                                return;
                            } else {
                                alert(data.msg);
                                $('#confirmButton').show();
                                $('#spLoginLoading').hide();
                                
                                return;
                            }
                            $(this).removeAttr("disabled");
                        }
                        if (data.type == 100) {
                            location.href = "Order.aspx?comid=" + comid;
                            return;
                        }
                    })
                    function callbackfunc(e, v, m, f) {
                        if (v == true)
                            location.href = "Order.aspx?comid=" + comid;
                    }

                })

            }


            //人数增
            $(".btn-add").click(function () {
                var i = $("#u_num").val();
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    $.prompt("请填写正确地数量");
                    return;
                }
                else {
                    i++;
                    $("#u_num").val(i);

                    //如果是实物产品的话需要计算运费
                    if ($("#hid_server_type").val() == 11) {
                        var proid = $("#hid_proid").trimVal();
                        var city = $("#com_city").trimVal();
                        if (city != "城市") {
                            getDeliveryFee(proid, i, city);
                        }
                    }


                    //如果是旅游大巴产品，则需要增加乘车人信息
                    if ($("#hid_server_type").val() == 10) {
                        //旅游大巴产品，限制提单数量最高60
                        if (parseInt(i) > 60) {
                            alert("大巴产品限制预订数量最高为60人");
                            $("#u_num").val(60);
                            return;
                        }

                        $("#travel_tbody").append(
                 '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                   '<label>姓名：</label><input type="text" id="travelname' + i + '" value=""  style="width:50px;"/>' + '<label>联系手机：</label><input type="text" id="travelphone' + i + '" value=""  style="width:80px;" />' +
                  '<label>身份证：</label><input type="text" id="travelidcard' + i + '" value="" style="width:140px;" />' +
                   '<label  style="display:none;">民族：</label><input type="text" id="travelnation' + i + '" value="汉族"  style="width:40px;display:none;"/>' +
                   '<label>备注：</label><input type="text" id="travelremark' + i + '" value=""  style="width:100px;" />' +

                    '</td>' +
                '</tr> ');

                    }


                     //如果是保险产品，则需要增加被保险人信息
                    if ($("#hid_server_type").val() == 14) {
                        //保险产品，限制提单数量最高50
                        if (parseInt(i) > 50) {
                            alert("保险产品限制预订数量最高为50人");
                            $("#u_num").val(50);
                            return;
                        }

                        $("#baoxian_tbody").append(
                 '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                   '<label>姓名：</label><input type="text" name="baoxianname" id="baoxianname' + i + '" value=""  style="width:50px;"/>' + '<label>拼音/英文名：</label><input type="text" name="baoxianpinyinname"   id="baoxianpinyinname' + i + '" value=""  style="width:80px;" />' +
                  '<label>身份证：</label><input type="text" name="baoxianidcard" id="baoxianidcard' + i + '" value="" style="width:140px;" />' + 
                    '</td>' +
                '</tr> ');

                    }


                    if ($("#hid_server_type").val() == 2 || $("#hid_server_type").val() == 8) {//如果是旅游产品，则需要添加儿童价格
                        var totalcount = parseInt($("#u_num").trimVal() * $("#hid_payprice").trimVal()) + $("#u_childnum").trimVal() * parseInt($("#hid_payprice").trimVal() - $("#hidchildreduce").trimVal());
                        $("#heji").text("￥" + fmoney(totalcount, 2));
                    }
                    else { //其他产品无需计算儿童价格
                        var totalcount = $("#u_num").trimVal() * $("#hid_payprice").trimVal();
                        $("#heji").text("￥" + fmoney(totalcount, 2));
                    }
                }
            })
            //人数减
            $(".btn-reduce").click(function () {
                var i = $("#u_num").val();
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    $.prompt("请填写正确地数量");
                    return;
                }
                else {
                    i--;
                    if (i > 0) {
                        $("#u_num").val(i);

                        //如果是实物产品的话需要计算运费
                        if ($("#hid_server_type").val() == 11) {
                            var proid = $("#hid_proid").trimVal();
                            var city = $("#com_city").trimVal();
                            if (city != "城市") {
                                getDeliveryFee(proid, i, city);
                            }
                        }

                        //如果是旅游大巴产品，则需要增加乘车人信息
                        if ($("#hid_server_type").val() == 10) {

                            var ihtml = "";
                            for (var j = 1; j <= i; j++) {
                                var minzu = "汉族";
                                if ($("#travelnation" + j).trimVal() != '') {
                                    minzu = $("#travelnation" + j).trimVal();
                                }

                                ihtml += '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                            '<label>姓名：</label><input type="text" id="travelname' + j + '" value="' + $("#travelname" + j).trimVal() + '" style="width:50px;"/>' +
                             '<label>联系手机：</label><input type="text" id="travelphone' + j + '" value="' + $("#travelphone" + j).trimVal() + '"  style="width:80px;" />' +
                            '<label>身份证：</label><input type="text" id="travelidcard' + j + '" value="' + $("#travelidcard" + j).trimVal() + '" style="width:140px;"  />' +
                            '<label  style="display:none;">民族：</label><input type="text" id="travelnation' + j + '" value="' + minzu + '" style="width:40px;display:none;"/>' +
                            '<label>备注：</label><input type="text" id="travelremark' + j + '" value="' + $("#travelremark" + j).trimVal() + '"  style="width:100px;" />';
                                if (j == 1) {
                                    ihtml += '<input name="chkItem" type="checkbox" value="1" id="chkItem" />与预定人相同';
                                }
                                ihtml += '</td>' +
                '</tr> ';

                            }
                            $("#travel_tbody").html(ihtml);

                        }

                          //如果是保险产品，则需要增加被保险人信息
                        if ($("#hid_server_type").val() == 14) {

                            var ihtml = "";
                            for (var j = 1; j <= i; j++) {
                              

                                ihtml += '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                            '<label>姓名：</label><input type="text" name="baoxianname" id="baoxianname' + j + '" value="' + $("#travelname" + j).trimVal() + '" style="width:50px;"/>' +
                             '<label>拼音/英文名：</label><input type="text" name="baoxianpinyinname" id="baoxianpinyinname' + j + '" value="' + $("#travelphone" + j).trimVal() + '"  style="width:80px;" />' +
                            '<label>身份证：</label><input type="text" name="baoxianidcard" id="baoxianidcard' + j + '" value="' + $("#travelidcard" + j).trimVal() + '" style="width:140px;"  />';
                                
                                ihtml += '</td>' +
                '</tr> ';

                            }
                            $("#baoxian_tbody").html(ihtml);

                        }

                        if ($("#hid_server_type").val() == 2 || $("#hid_server_type").val() == 8) {//如果是旅游产品，则需要添加儿童价格
                            var totalcount = parseInt($("#u_num").trimVal() * $("#hid_payprice").trimVal()) + $("#u_childnum").trimVal() * parseInt($("#hid_payprice").trimVal() - $("#hidchildreduce").trimVal());
                            $("#heji").text("￥" + fmoney(totalcount, 2));
                        }
                        else { //其他产品无需计算儿童价格
                            var totalcount = $("#u_num").trimVal() * $("#hid_payprice").trimVal();
                            $("#heji").text("￥" + fmoney(totalcount, 2));
                        }

                    } else {

                        return false;
                    }
                }
            })
            //儿童人数增
            $(".btn-add2").click(function () {
                var i = $("#u_childnum").val();

                i++;
                $("#u_childnum").val(i);

                if ($("#hid_server_type").val() == 2 || $("#hid_server_type").val() == 8) {//如果是旅游产品，则需要添加儿童价格
                    var totalcount = parseInt($("#u_num").trimVal() * $("#hid_payprice").trimVal()) + $("#u_childnum").trimVal() * parseInt($("#hid_payprice").trimVal() - $("#hidchildreduce").trimVal());
                    $("#heji").text("￥" + fmoney(totalcount, 2));
                }
                else { //其他产品无需计算儿童价格
                    var totalcount = $("#u_num").trimVal() * $("#hid_payprice").trimVal();
                    $("#heji").text("￥" + fmoney(totalcount, 2));
                }

            })
            //儿童人数减
            $(".btn-reduce2").click(function () {
                var i = $("#u_childnum").val();
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    $.prompt("请填写正确地数量");
                    return;
                }
                else {
                    i--;
                    if (i >= 0) {
                        $("#u_childnum").val(i);
                        if ($("#hid_server_type").val() == 2 || $("#hid_server_type").val() == 8) {//如果是旅游产品，则需要添加儿童价格
                            var totalcount = parseInt($("#u_num").trimVal() * $("#hid_payprice").trimVal()) + $("#u_childnum").trimVal() * parseInt($("#hid_payprice").trimVal() - $("#hidchildreduce").trimVal());
                            $("#heji").text("￥" + fmoney(totalcount, 2));
                        }
                        else { //其他产品无需计算儿童价格
                            var totalcount = $("#u_num").trimVal() * $("#hid_payprice").trimVal();
                            $("#heji").text("￥" + fmoney(totalcount, 2));
                        }
                    }
                }
            })

            //产品是旅游大巴时，u_num 加了只读属性，所以时间不会触发，不用考虑旅游大巴
            $("#u_num").keyup(function () {
                var i = $(this).val();
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    $.prompt("请填写正确地宝贝数量");
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

                    var totalcount = $("#u_num").trimVal() * $("#hid_payprice").trimVal();
                    $("#heji").text("￥" + fmoney(totalcount, 2));
                }
            });
            //与预定人相同
            $("#chkItem").live("click", function () {
                if ($(this).is(":checked")) {
                    if ($("#u_name").trimVal() != "") {
                        $("#travelname1").val($("#u_name").trimVal());
                        $("#travelphone1").val($("#u_phone").trimVal());
                    }
                    $("#hid_issamewithbooker").val("1");
                } else {
                    $("#hid_issamewithbooker").val("0");
                }
            });
          

            $("input:radio[name='deliverytype']").click(function () {
                var chked = $("input:radio[name='deliverytype']:checked").val();
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
            })


            $("#com_city").change(function () {

                var city = $("#com_city").trimVal();
                if (city == "城市") {
                    alert("请选择送达城市");
                    return;
                }
                //根据产品id和城市获得运费
                var proid = $("#hid_proid").trimVal();
                var num = $("#u_num").trimVal();


                getDeliveryFee(proid, num, city);
            })

            //加载常用地址
            var pageindex = 1;
            SearchAddressList(1);
            function SearchAddressList(pageindex) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=addresspagelist",
                    data: { pageindex: pageindex, pagesize: 10, agentid: agentid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();

                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {

                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })
            }

            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchAddressList(page);

                        return false;
                    }
                });
            }


            $("#uesoldaddress").click(function () {
                $("#bindingagent").show();
            })

            $("#cancel").click(function () {
                $("#bindingagent").hide();
            })

            /*线路行程弹出层*/
            $("#a_linedetail").click(function () {
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
                            $(".main_cont").html(linedefautop + linestr + linedefaubot);

                        }

                    }
                })
                $("#ProInfo").show();
            })
            $("#closeProInfo").click(function () {
                $("#ProInfo").hide();
            })



            //加载qq
            $("#loading").show();
            $.ajax({
                type: "post",
                url: "/JsonFactory/CrmMemberHandler.ashx?oper=channelqqList",
                data: { comid: comid, pageindex: 1, pagesize: 12 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    $("#loading").hide();
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        var qqstr = "";
                        for (var i = 0; i < data.msg.length; i++) {
                            qqstr += '<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&amp;uin=' + data.msg[i].QQ + '&amp;site=qq&amp;menu=yes"><img style="vertical-align:bottom;padding-left:5px;" src="/images/qq.png" alt="' + data.msg[i].QQ + '" title="' + data.msg[i].QQ + '" border="0"></a>'
                        }
                        $("#contentqq").append(qqstr);

                    }
                }
            })

                         //装载产品规格
         <%if (manyspeci==1){ %>
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Selectguigelist",
                    data: { comid: comid,proid: $("#hid_proid").val(),agentid: $("#hid_agentid").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {
                            $(".sys_item_spec").empty();
                            $("#guige").tmpl(data.msg).appendTo(".sys_item_spec");

                        }
                    }
                })
        <%} %>

        })

        function getDeliveryFee(proid, num, city) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=getexpressfee", { proid: proid, city: city, num: num }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {

                    $("#bfee").html(data.msg);
                }
            })
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


        function useaddress(str1, str2, str3, str4, str5, str6) {

            $("#u_name").val(str1);
            $("#u_phone").val(str2);
            $("#com_province").val(str3);
            $("#com_city").append('<option value="' + str4 + '" selected="selected">' + str4 + '</option>');
            $("#txtaddress").val(str5);
            $("#txtcode").val(str6);
            $("#bindingagent").hide();

            var proid_temp = $("#hid_proid").trimVal();
            var num = $("#u_num").val();
            getDeliveryFee(proid_temp, num, str4);
        }

        function deladdress(id) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=deladdress", { id: id }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {

                    alert(data.msg);
                }
            })
        }

        function addcart(proid) {

            var manyspeci = $("#hid_manyspeci").val();
            var speciid = $("#hid_speciid").val();

                    if(manyspeci==1){
                        if(speciid==0){
                            alert("请选择具体规格");
                            $("#loading").hide();
                            return;
                        }
                    }


            alert("添加到购物车");
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comid_temp").trimVal(), proid: proid,speciid:speciid, u_num: $("#u_num").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    $("#cart").show();
                }
            })
        }


    </script>
    <style type="text/css">
        .Bty
        {
            background-position: 0 -23px;
            color: #fff;
            font-weight: bold;
            text-shadow: 0 0 2px #D84803;
        }
        .Bt
        {
            display: inline-block;
            position: relative;
            z-index: 0;
            padding: 6px 12px;
            padding: 7px 12px\0;
            font-size: 12px;
            line-height: 14px;
            cursor: pointer;
        }
        .Bty
        {
            background-position: 0 -23px;
            color: #fff;
            font-weight: bold;
            text-shadow: 0 0 2px #D84803;
        }
        .Bt
        {
            display: inline-block;
            position: relative;
            z-index: 0;
            padding: 6px 12px;
            padding: 7px 12px\0;
            font-size: 12px;
            line-height: 14px;
            cursor: pointer;
        }
        .Bt, .Bt s
        {
            background-image: url(http://shop.etown.cn/images/bgAddressNew.png);
            background-repeat: no-repeat;
        }
        
        .btSubOrder1, .noOrderSubmit1
        {
            width: 150px;
            height: 45px;
            border: none;
            background: #ff4800 url(http://shop.etown.cn/images/bgRedBt.png) no-repeat;
            font-family: microsoft yahei;
            color: #fff;
            font-weight: bold;
            font-size: 22px;
            line-height: 35px;
        }
        .payTotal strong
        {
            display: block;
            padding-top: 10px;
            text-align: right;
            font-size: 14px;
        }
        
        .wrap-input
        {
            float: left;
        }
        .ui-datepicker-today a {
          background-color: #FFFF;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <table>
            <tr>
                <td class="tdHead" id="contentqq" style="font-size: 14px; height: 26px;">
                    <div class="left">
                        <img id="comlogo_sale" src="<%=comlogo %>" class="" height="42"></div>
                    <div class="left comleft">
                        <div>
                            <span>商户名称：
                                <%=company %>
                            </span><span>授权类型：
                                <%=Warrant_type_str%>；</span> <span>
                                    <%if (contact_phone != "")
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
            <ul class="composetab">
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="ProjectList.aspx?comid=<%=comid_temp %>">项目列表</a></div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_sel">
                        <div>
                            <a href="Manage_sales.aspx?comid=<%=comid_temp%>">产品列表</a></div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="Order.aspx?comid=<%=comid_temp%>">订单记录</a>
                        </div>
                    </div>
                </li>
                <%if (Warrant_type == 2)
                  { %>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="EticketCount.aspx?comid=<%=comid_temp %>">验码统计</a>
                        </div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="Verification.aspx?comid=<%=comid_temp %>">验码记录</a>
                        </div>
                    </div>
                </li>
                <% } %>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="Finane.aspx?comid=<%=comid_temp %>">财务列表</a>
                        </div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img" style="z-index: 2;">
                    </div>
                    <div class="composetab_unsel" style="position: relative;">
                        <div>
                            <a href="EticketBack.aspx?comid=<%=comid_temp %>">退订/订单状态</a>
                        </div>
                    </div>
                </li>
            </ul>
            <div class="toolbg toolbgline toolheight nowrap" style="">
                <div class="right">
                </div>
                <div class="nowrap left" unselectable="on" onselectstart="return false;">
                    <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
                </div>
            </div>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3 style="width: 95%;">
                    产品基本信息</h3>
                <table width="95%" class="grid">
                    <tr>
                        <td class="tdHead">
                            <%if (Warrant_type == 1)
                              {%>
                            <table border="0">
                                <tr>
                                    <td>
                                        <div class="steps step1">
                                            <ol>
                                                <li class="s1">1. 提交订单 </li>
                                                <li class="s2">2. 扣除预付款</li>
                                                <li class="s3">3. 发送到手机</li>
                                            </ol>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%} %>
                            <%if (Warrant_type == 2)
                              { %>
                            <table border="0">
                                <tr>
                                    <td>
                                        <div class="steps step1">
                                            <ol>
                                                <li class="s1">1. 提交订单 </li>
                                                <li class="s2">2. 商家审核确认</li>
                                                <li class="s3">3. 下载电子码EXCEL</li>
                                            </ol>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%} %>
                        </td>
                    </tr>
                </table>
                <table width="95%" class="grid">
                    <tr>
                        <td style="width: 85px;">
                            名 称 :
                        </td>
                        <td>
                            <h3 class="titlepng">
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 85px;">
                            单 价 :
                        </td>
                        <td>
                            <div class="bookOrderDetailsRight dataMarginBox" vfn="calendarVerify">
                                ￥<label id="Advise_price" class="sys_item_price">
                                </label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 85px;">
                            预订数量 :
                        </td>
                        <td>
                            <div class="wrap-input">
                                <a href="javascript:void(0);" class="btn-reduce">减少数量</a>
                                <input id="u_num" type="text" value="1" class="input-ticket" autocomplete="off" maxlength="4"
                                    size="4">
                                <a href="javascript:void(0);" class="btn-add">增加数量</a>
                            </div>
                            <div>
                                <label id="labelmsg" style="padding-left: 10px;">
                                </label>
                            </div>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td style="width: 85px;">
                        </td>
                        <td>
                            <div class="wrap-input">
                                <a href="javascript:void(0);" class="btn-reduce2">减少数量</a>
                                <input id="u_childnum" type="text" value="0" class="input-ticket" autocomplete="off"
                                    maxlength="4" size="4">
                                <a href="javascript:void(0);" class="btn-add2">增加数量</a>
                            </div>
                            <div>
                                <label id="labelmsg2" style="padding-left: 10px;">
                                </label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="bookOrderDetails" style="display: none">
                                <div class="bookOrderDetailsRight dataMarginBox" vfn="calendarVerify">
                                    <span class="bookOrderDetailsLeft" style="margin-right: 40px;">游玩日期：</span>
                                    <input value="" class="dataNum dataIcon" readonly="readonly" id="u_traveldate">
                                    <span id="hoteloutdate" style="display: none;"><span style="margin-right: 40px;">离店日期：</span>
                                        <input value="" class="dataNum dataIcon" readonly="readonly" id="u_traveldate_out">
                                    </span>
                                </div>
                                <span class="bookLimit"></span>
                            </div>
                        </td>
                    </tr>
                    <%if (manyspeci == 1)
                      {//当多规格，显示规格 %>
                    <tr>
                        <td style="width: 85px;">
                            请选择票的规格：
                        </td>
                        <td>
                            <div class="" style="border: 1px solid #CCC; margin: 10px;">
                                <div class="iteminfo_buying">
                                    <!--规格属性-->
                                    <div class="sys_item_spec">
                                    </div>
                                    <!--规格属性-->
                                </div>
                            </div>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td style="width: 85px;">
                            合 计 :
                        </td>
                        <td>
                            <span class="pricefont" id="heji"></span>(<%=yufukuan %>)
                        </td>
                    </tr>
                    <tr class="shiwupro">
                        <td style="width: 85px;">
                            有效期 :
                        </td>
                        <td>
                            <label id="Perioddate">
                            </label>
                        </td>
                    </tr>
                    <tr class="shiwupro">
                        <td style="width: 85px;">
                            使用限制:
                        </td>
                        <td>
                            <label id="pro_youxiaoqi">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 85px;" valign="top">
                            预订说明 :
                        </td>
                        <td>
                            <div id="yuding_msg">
                            </div>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td style="width: 85px;" valign="top">
                            线路行程:
                        </td>
                        <td>
                            <a href="javascript:void(0);" id="a_linedetail" style="padding-left: 10px; color: #ff0000;
                                cursor: pointer; text-decoration: underline;">查看详情</a>
                        </td>
                    </tr>
                </table>
                <hr style="width: 95%;" />
                <h3 style="width: 95%;">
                    <%if (Warrant_type == 1)
                      {%>
                    <%if (Server_type != 9)
                      { %>
                    接收人信息
                    <%}
                      else
                      { %>
                    入住人信息
                    <%} %>
                    <%} %>
                    <%if (Warrant_type == 2)
                      { %>
                    联系人（此订单不发送电子码）
                    <%} %>
                </h3>
                <table width="95%" class="grid">
                    <tr>
                        <td style="width: 85px;">
                            姓 名
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="u_name" value="" /><span style="color: Red;">*</span>
                            <label>
                                <input name="saveaddress" value="1" type="checkbox" />
                                保存到常用地址</label>
                            <input type="button" name="uesoldaddress" id="uesoldaddress" value="使用常用地址" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 85px;">
                            接收手机
                        </td>
                        <td>
                            <input name="u_phone" class="dataNum dataIcon" id="u_phone" value="" /><span style="color: Red;">*</span>
                        </td>
                    </tr> 
                    <tr style="display:<%if (issetidcard==0){%>none<%} %>;">
                        <td style="width: 85px;">
                            身份证
                        </td>
                        <td>
                            <input name="u_idcard" class="dataNum dataIcon" id="u_idcard" value="" /><span style="color: Red;">*</span>
                        </td>
                    </tr> 
                    <tbody id="tbody_address" style="display: none;">
                        <!--实物产品收货人地址信息-->
                        <tr>
                            <td style="width: 85px;">
                                运送方式
                            </td>
                            <td>
                                <label>
                                    <input name="deliverytype" type="radio" value="2" checked>
                                    快递(需缴纳运费)</label>
                                <label>
                                    <input name="deliverytype" type="radio" value="4">
                                    自提(免运费)</label>
                            </td>
                        </tr>
                        <tr id="delivery_tr1">
                            <td style="width: 85px;">
                                收货地址
                            </td>
                            <td>
                                <select name="com_province" id="com_province" class="ui-input">
                                    <option value="省份" selected="selected">省份</option>
                                </select>
                                <select name="com_city" id="com_city" class="ui-input">
                                    <option value="城市" selected="selected">城市</option>
                                </select><span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr id="delivery_tr2">
                            <td style="width: 85px;">
                                详细地址
                            </td>
                            <td>
                                <input name="txtaddress" class="dataNum dataIcon" id="txtaddress" value="" style="width: 350px;" /><span
                                    style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr id="delivery_tr3">
                            <td style="width: 85px;">
                                邮编
                            </td>
                            <td>
                                <input name="txtcode" class="dataNum dataIcon" id="txtcode" value="" style="width: 122px;" />
                            </td>
                        </tr>
                        <tr id="delivery_tr4">
                            <td colspan="2">
                                <%-- <span id="saveEditAddress" class="Bt Bty saveEditAddress">确定<s></s></span>--%>
                                <strong>您需为订单支付<b id="bfee" style="font-size: 12px; color: #ff0000;">0</b>元运费</strong>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </tbody>
                    <tbody id="tbody_busplus" style="display: none;">
                        <!--旅游大巴附加信息-->
                        <tr>
                            <td style="width: 85px;">
                                上车地点
                            </td>
                            <td>
                                <select id="pointuppoint">
                                </select>
                            </td>
                        </tr>
                        <tr id="tr_dropoffpoint" style="display: none;">
                            <td style="width: 85px;">
                                下车地点
                            </td>
                            <td>
                                <select id="dropoffpoint">
                                </select>
                            </td>
                        </tr>
                    </tbody>
                    <tr>
                        <td class="tdHead" colspan="2">
                        </td>
                    </tr>
                </table>
                <table width="95%" class="grid" id="travel_tb" style="display: none;">
                    <tr>
                        <td class="tdHead" valign="top" colspan="2">
                            <h3>
                                乘车人信息:</h3>
                        </td>
                    </tr>
                    <tbody id="travel_tbody">
                    </tbody>
                </table>
                <table width="95%" class="grid" id="baoxian_tb" style="display: none;">
                    <tr>
                        <td class="tdHead" valign="top" colspan="2">
                            <h3>
                                乘车人信息:</h3>
                        </td>
                    </tr>
                    <tbody id="baoxian_tbody">
                    </tbody>
                </table>
                <table width="95%" class="grid">
                    <tr>
                        <td style="width: 85px;">
                            备注
                        </td>
                        <td>
                            <input class="dataNum dataIcon" id="order_remark" value="" style="width: 350px;" />
                        </td>
                    </tr>
                </table>
                <hr style="width: 95%;" />
                <table width="900px" class="grid">
                    <tr>
                        <td class="tdHead" style="text-align: right;">
                            <%if (iscanbook == 1)
                              {%>
                            <button id="confirmButton" name="confirmButton" type="button" class="btSubOrder1 noOrderSubmit1">
                                提交订单</button>
                            <%if (Server_type == 11)
                              { %>
                            <a href="javascript:;" onclick="addcart('<%=id %>')" class="a_anniu">加入购物车</a>
                            <%} %>
                            <%}
                              else
                              { 
                            %>
                            <button id="Button1" type="button" class="btSubOrder1 noOrderSubmit1">
                                已售完</button>
                            <%
                                } %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="calDiv" style="display: none; margin-top: -40px">
    </div>
    <div id="calDivOut" style="display: none; margin-top: -40px">
    </div>
    <div class="data">
    </div>
    <div id="bindingagent" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 760px; height: 450px; display: none; z-index: 10; left: 20%;
        top: 20%;" class="dialog">
        <table width="95%" border="0" cellpadding="10" cellspacing="1" style="padding: 10px;">
            <tr>
                <td height="95%">
                    <div id="agentlist" style="margin: 20px;">
                        <table width="720" border="0" cellpadding="5">
                            <tr style="background-color: #999999; border-width: 1px; border-style: solid; border-color: #A6A6A6 #CCC #CCC;">
                                <td width="50" height="30">
                                    <p align="left" style="padding-left: 5px;">
                                        姓名
                                    </p>
                                </td>
                                <td width="60">
                                    <p align="left">
                                        手机
                                    </p>
                                </td>
                                <td width="120">
                                    <p align="left">
                                        城市
                                    </p>
                                </td>
                                <td width="350">
                                    <p align="left">
                                        地址
                                    </p>
                                </td>
                                <td width="100">
                                    <p align="left">
                                        &nbsp;</p>
                                </td>
                            </tr>
                            <tbody id="tblist">
                            </tbody>
                        </table>
                        <div id="divPage">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel" id="cancel" type="button" class="formButton" value="  关  闭  " />
                    <br />
                    注：如需对常用地址编辑，您可提交新的订单时填写新的地址并选择 保存到常用地址，对无效地址进行删除。
                </td>
            </tr>
        </table>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr style="background-color:#ffffff;border-width: 1px;border-style: solid;border-color: #A6A6A6 #CCC #CCC;">
                        <td>
                            <p align="left" style=" padding-left:5px;">
                                ${U_name}
                            </p>
                        </td>
                         <td>
                            <p align="left">
                                ${U_phone}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${Province}/${City}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Address}(${Code})</p>
                        </td>
                        <td>
                            <p align="left" id="agent${Agentid}">
                            <input name="setw" onclick="useaddress('${U_name}','${U_phone}','${Province}','${City}','${Address}','${Code}')" type="button" class="formButton" value="  使用  " />
                             <input name="setw" onclick="deladdress('${Id}')" type="button" class="formButton" value="  删除  " />
                            </p>
 
                        </td>
                    </tr>
    </script>
    <!--购物车图标--->
    <div id='cart' style="display: none; position: absolute; bottom: 6em; right: 2em;
        width: 55px; height: 55px; background-color: #FFFAFA; margin: 10px; border-radius: 60px;
        border: solid rgb(55,55,55)  #FF0000   1px; cursor: pointer;">
        <a href="ShopCart.aspx?comid=<%=comid_temp %>">
            <img src="/images/cart.gif" width="39" style="padding: 8px 0 0 9px;" /></a>
    </div>
    <!--线路详情弹出层--->
    <div id="ProInfo" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 850px; display: none; left: 5%; position: absolute; top: 20%; overflow: auto;">
        <table width="95%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="margin: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span class="STYLE1">&nbsp;&nbsp;&nbsp;&nbsp;产品介绍</span><span style="float: right;
                        font-size: 18px; padding-right: 10px; cursor: pointer;" id="closeProInfo">X</span>
                </td>
            </tr>
            <tr>
                <td width="80" height="30" bgcolor="#E7F0FA">
                    &nbsp;&nbsp;&nbsp;&nbsp;产品名称:
                </td>
                <td height="30" bgcolor="#E7F0FA">
                    <span id="Pro_name"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    &nbsp;&nbsp;&nbsp;&nbsp;线路行程:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="main_cont">
                </td>
            </tr>
        </table>
    </div>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <input id="hid_proid" type="hidden" value="<%=id %>" />
    <input id="hid_payprice" type="hidden" value="0" />
<%--    <input id="hid_hotel_agent" type="hidden" value="<%=hotel_agent %>" />--%>
    <input id="hid_server_type" type="hidden" value="0" />
    <input id="hidLeavingDate" type="hidden" value="" />
    <input id="hidMinLeavingDate" type="hidden" value="" />
    <input id="hidLinePrice" type="hidden" value="" />
    <input id="hidchildreduce" type="hidden" value="0" />
    <input id="hidEmptyNum" type="hidden" value="" />
    <input id="hid_MinEmptynum" type="hidden" value="0" />
    <!--上车地点，下车地点-->
    <input id="hid_pickuppoint" type="hidden" value="<%=pickuppoint %>" />
    <input id="hid_dropoffpoint" type="hidden" value="<%=dropoffpoint %>" />
    <!--乘车人信息 与预订人相同-->
    <input type="hidden" id="hid_issamewithbooker" value="0" />
    <input id="hid_manyspeci" value="<%=manyspeci %>" type="hidden" />
    <input id="hid_speciid" value="0" type="hidden" />
    <script type="text/x-jquery-tmpl" id="guige"> 

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
                    $("#hid_speciid").val(0);
                }
                //输出价格
                $(_resp.mktprice).text(_mktprice);  ///其中的math.round为截取小数点位数
                $(_resp.price).text(_price);
                $("#hid_payprice").val(_price);
                 var totalcount = $("#u_num").val() * $("#hid_payprice").val();
                 $("#heji").text("￥" + fmoney(totalcount, 2));
            }
        })
    </script>
    <script type="text/javascript">
        var province = document.getElementById('com_province');
        var city = document.getElementById('com_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</asp:Content>
