<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/V/Member.Master" CodeBehind="CreateOrder.aspx.cs"
    Inherits="ETS2.WebApp.UI.ShangJiaUI.CreateOrder" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <%if (pro_servertype == 10)//如果服务类型是旅游大巴
      {
    %>
    <link href="/Scripts/JUI/jquery-rili.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <%}
      else
      {
    %>
    <link href="/Scripts/JUI/jquery-rili.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
        <link onerror="_cdnFallback(this)" href="/h5/order/css/base.css" rel="stylesheet">  
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <%
        } %>
    <script type="text/javascript">
        //验证手机号
        function isMobel(value) {
            if (/^13\d{9}$/g.test(value) || /^15\d{9}$/g.test(value) || /^1\d{10}$/g.test(value) ||
	/^18\d{9}$/g.test(value)) {
                return true;
            } else {
                return false;
            }
        }


        $(function () {


            var comid = $("#hid_comid").val();
            var uid = $("#hid_uid").val();
            //日历
            var nowdate = '<%=this.nowdate %>';
            var enddate = '<%=this.enddate %>';

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {

                //$(this).val(nowdate);
                $("#stardate").val(nowdate);
                $("#enddate").val(enddate);
                $($(this)).datepicker({
                    

                      <%if(isSetVisitDate==0){ %>
                   numberOfMonths: 2,
                    minDate: 0,
                    defaultDate: +4,
                    maxDate: '+8m +1w'

                    <%}else{ %>
                    minDate:'<%=pro_start.ToString("yyyy-MM-dd") %>',
                    maxDate:'<%=pro_end.ToString("yyyy-MM-dd") %>'
                      

                    <%} %>

                });
            });
            //产品名称
            //var proname = $("#hid_proname").val();


            //根据产品id获得产品信息
            $.post("/JsonFactory/ProductHandler.ashx?oper=ClientProById", { proid: $("#hid_proid").val() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    location.href = "/";

                }
                if (data.type == 100) {

                    $(".titlepng").html(data.msg.Pro_name);
                    $("#hid_server_type").val(data.msg.Server_type);
                    if (data.msg.Server_type == 9)//酒店
                    {
                        $("#ticket").hide();
                        $("#hotel").show();
                        //$("#hid_payprice").val(data.msg.Advise_price); //限购价
                        //$("#h_heji").text("￥" + data.msg.Advise_price);
                        //$("#h_Advise_price").html(data.msg.Advise_price);
                        //$("#ticket_remark").html(data.msg.Service_Contain);
                        $(".piaowubeizhu").hide();



                        $("#hid_pro_Number").val(data.msg.Pro_number);
                        $("#hid_ReserveType").val(data.msg.Housetype.ReserveType);

                        if (data.msg.Housetype.ReserveType == "1") {
                            $("#zaixianzhifu").html("前台现付");
                        }


                        var chanpinshuoming = "";
                        $.post(" /JsonFactory/ProductHandler.ashx?oper=GetHouseType", { proid: $("#hid_proid").val(), comid: comid }, function (data2) {
                            data2 = eval("(" + data2 + ")");
                            if (data2.type == 100) {

                                if (data2.msg.Bedtype != "") {
                                    chanpinshuoming += "<b>床型:</b> " + data2.msg.Bedtype + "<br /><br />";
                                }
                                if (data2.msg.Bedwidth != "") {
                                    chanpinshuoming += "<b>床宽:</b> " + data2.msg.Bedwidth + "<br /><br />";
                                }
                                if (data2.msg.Whetherextrabed == true) {
                                    chanpinshuoming += "<b>加床:</b> 可以 <br /><br />";
                                    if (data2.msg.Extrabedprice != "") {
                                        chanpinshuoming += "<b>加床费用:</b> " + data2.msg.Extrabedprice + "<br /><br />";
                                    }
                                } else {
                                    chanpinshuoming += "<b>加床:</b> 不可以加床 <br /><br />";
                                }
                                if (data2.msg.Builtuparea != "") {
                                    chanpinshuoming += "<b>建筑面积:</b> " + data2.msg.Builtuparea + "<br /><br />";
                                }
                                if (data2.msg.Floor != "") {
                                    chanpinshuoming += "<b>楼层:</b> " + data2.msg.Floor + "<br /><br />";
                                }
                                if (data2.msg.Largestguestnum != "") {
                                    chanpinshuoming += "<b>最多入住人数:</b> " + data2.msg.Largestguestnum + "<br /><br />";
                                }

                                if (data2.msg.Wifi != "") {
                                    chanpinshuoming += "<b>支持Wifi:</b> " + data2.msg.Wifi + "<br /><br />";
                                }
                                if (data2.msg.Breakfast == "1") {
                                    chanpinshuoming += "<b>早餐:</b> 无 <br /><br />";

                                } else if (data2.msg.Breakfast == "2") {
                                    chanpinshuoming += "<b>早餐:</b> 单早 <br /><br />";
                                } else {
                                    chanpinshuoming += "<b>早餐:</b> 双早 <br /><br />";
                                }


                                if (data2.msg.Whethernonsmoking == true) {
                                    chanpinshuoming += "<b>可否安排无烟楼层:</b>  可以 <br /><br />";
                                } else {
                                    chanpinshuoming += "<b>可否安排无烟楼层:</b> 无 <br /><br />";
                                }

                                if (data2.msg.Amenities != "") {
                                    chanpinshuoming += "<b>便利设施:</b> " + data2.msg.Amenities + "<br /><br />";
                                }
                                if (data2.msg.Mediatechnology != "") {
                                    chanpinshuoming += "<b多媒体支持:</b> " + data2.msg.Mediatechnology + "<br /><br />";
                                }
                                if (data2.msg.Foodanddrink != "") {
                                    chanpinshuoming += "<b>食品饮料:</b> " + data2.msg.Foodanddrink + "<br /><br />";
                                }
                                if (data2.msg.ShowerRoom != "") {
                                    chanpinshuoming += "<b>浴室:</b> " + data2.msg.ShowerRoom + "<br /><br />";
                                }

                                if (data2.msg.Roomtyperemark != "") {
                                    chanpinshuoming += "<b>备注:</b> " + data2.msg.Roomtyperemark + "<br /><br />";
                                }

                                $("#yuding_msg").html(chanpinshuoming);
                            }
                        })

                        searchHouseType();

                        $("#Perioddate").html(data.msg.Pro_youxiaoqi);
                    }
                    else if (data.msg.Server_type == 10)//旅游大巴
                    {
                        //日历
                        $("#u_traveldate").click(function () {
                            scrollTo(0, 1);
                            $("#calDiv").fadeIn();
                            $("#hotel").hide();
                            $(".submitAll").hide();
                            $(".separationBox").hide();
                        });

                        $("#travel_div").show();
                        $("#travel_tb").show();
                        $("div:[name='tbody_busplus']").show();

                        var pickuppointstr = ''; //上车地点下拉框字符串
                        var dropoffpointstr = ''; //下车地点下拉框字符串
                        if ($("#hid_pickuppoint").val() != "") {
                            var arrpickuppoint = $("#hid_pickuppoint").val().split('，');
                            for (var a = 0; a < arrpickuppoint.length; a++) {
                                pickuppointstr += '<option value="' + arrpickuppoint[a] + '">' + arrpickuppoint[a] + '</option>';
                            }
                        }
                        else {
                            pickuppointstr = '<option value="张家口火车站">张家口火车站</option>';
                        }

                        if ($("#hid_dropoffpoint").val() != '') {
                            $("#tr_dropoffpoint").show();
                            var arrdropoffpoint = $("#hid_dropoffpoint").val().split('，');
                            for (var a = arrdropoffpoint.length - 1; a >= 0; a--) {
                                dropoffpointstr += '<option value="' + arrdropoffpoint[a] + '">' + arrdropoffpoint[a] + '</option>';
                            }
                        } else {
                            $("#tr_dropoffpoint").hide();
                        }





                        $("#pointuppoint").html(pickuppointstr);
                        $("#dropoffpoint").html(dropoffpointstr);


                        $("#travel_tbody").append(
                 '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                            '<label> 姓  名：</label><input type="text" id="travelname1" value="" style="width:60px;" />' +
                            '<label> 联系电话：</label><input type="text" id="travelphone1" value="" style="width:100px;" />' +
                            '<label> 身份证：</label><input type="text" id="travelidcard1" value="" style="width:150px;"/>' +
                            //'<label>民  族：</label><input type="text" id="travelnation1" value="汉族" style="width:40px;" />' +
                            // '<label>备注：</label><input type="text" id="travelremark1" value=""  style="width:100px;" />' +
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

                                listprice += data.linedate[i].Menprice + ","

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
                            $("#Advise_price").html(firstdate_price);


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
                                            $("#Advise_price").html(pricearr[i]);
                                            $("#hid_payprice").val(pricearr[i]);
                                            $("#heji").html('￥' + parseInt(pricearr[i]) * parseInt($("#u_num").val()));
                                            //$("#childPrice").html(parseInt(pricearr[i]) - parseInt(childreduce));
                                            $("#" + dateText).addClass("selected"); //对选中的增加
                                        } else {
                                            //$("#" + datearr[i]).removeClass("selected"); //对未选中的日期移除
                                        }
                                    }


                                    $("#calDiv").hide();
                                    $("#hotel").hide();
                                    $(".submitAll").show();
                                    $(".separationBox").show();
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

                        $("#ticket").show();
                        $("#hotel").hide();

                        $("#hid_pro_Number").val(data.msg.Pro_number);

                        var yuding_msg = "";
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
                    }
                    else {
                        if (data.msg.Server_type == 11 || data.msg.Server_type == 1) {
                            if (data.msg.Ispanicbuy == 2) {//如果是限购
                                $("#tbody_address").show();
                                //显示库存量
                                
                                $("#marpeapal_error34009").html("库存量:<span style='color:red; '>" + data.msg.Limitbuytotalnum + "</span>");
                            }
                        }


                        $("#ticket").show();
                        $("#hotel").hide();
                        $("#hid_payprice").val(data.msg.Advise_price); //限购价
                        $("#heji").text("￥" + data.msg.Advise_price);
                        $("#Advise_price").html(data.msg.Advise_price);
                        //$("#ticket_remark").html(data.msg.Service_Contain);
                        $(".piaowubeizhu").hide();
                        $("#hid_pro_Number").val(data.msg.Pro_number);

                        var yuding_msg = "";
//                        if (data.msg.Service_Contain != "") {
//                            yuding_msg += "<b>产品包含:</b> " + data.msg.Service_Contain + "<br /><br />";
//                        }
                        if (data.msg.Service_NotContain != "") {
                            yuding_msg += "<b>产品不包含:</b> " + data.msg.Service_NotContain + "<br /><br />";
                        }
                        if (data.msg.Precautions != "") {
                            yuding_msg += "<b>注意事项: </b>" + data.msg.Precautions;
                        }

                        $("#yuding_msg").html(yuding_msg);
                        $("#Perioddate").html(data.msg.Pro_youxiaoqi);
                    }

                    //如果服务类型是 票务；则备注信息中 显示 电子码使用限制
                    if (data.msg.Server_type == 1) {
                        var remarkk = $("#ticket_remark").html();
                        if (data.msg.Iscanuseonsameday == 0)//电子码当天不可用
                        {
                            $("#ticket_remark").html("此产品当天预订不可用<br>" + remarkk);
                            $(".piaowubeizhu").show();
                        }
                        if (data.msg.Iscanuseonsameday == 1)//电子码当天可用
                        {
                            $("#ticket_remark").html("此产品当天预订可用<br>" + remarkk);
                             $(".piaowubeizhu").show();
                        }
                        if (data.msg.Iscanuseonsameday == 2)//电子码出票2小时内不可用
                        {
                            $("#ticket_remark").html("此产品出票2小时内不可用<br>" + remarkk);
                             $(".piaowubeizhu").show();
                        }
                         if(data.msg.isSetVisitDate==1){ 
                                   
                                    //日历
                                    $("#u_traveldate").click(function () {
                                        scrollTo(0, 1);
                                        $("#calDiv").fadeIn();
                                        $("#hotel").hide();
                                        $(".submitAll").hide();
                                        $(".separationBox").hide();
                                    });
                                   $("#u_traveldate").parent().parent().show(); 
                                     //団期日历
                                    $("#calDiv").datepicker({
                                        minDate: nowdate,
                                        onSelect: function (dateText) {
                                            $("#u_traveldate").val(dateText);
                                            $("#Perioddate").html(dateText);
                                            
                                            $("#calDiv").hide();
                                            $("#hotel").hide();
                                            $(".submitAll").show();
                                            $(".separationBox").show();
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
            })

            //
            $.post("/JsonFactory/OrderHandler.ashx?oper=B2bcrmreader", { comid: comid, uid: uid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    location.href = "/";

                }
                if (data.type == 100) {
                    if (data.msg == null) {
                        $("#inte").css("display", "none");
                        $("#impr").css("display", "none");
                    }
                    if (data.msg != null) {
                        if (data.msg.Integral == 0) {
                            $("#inte").css("display", "none");
                        }
                        if (data.msg.Imprest == 0) {
                            $("#impr").css("display", "none");
                        }
                        $("#Integral_span").html(data.msg.Integral);
                        $("#Imprest_span").html(data.msg.Imprest);
                    }


                }
            })

            $("#confirmButton").click(function () {
            $("#loading").show();
                var server_type = $("#hid_server_type").val();
                var proid = $("#hid_proid").val();
                var ordertype = $("#hid_ordertype").val();
                var payprice = $("#hid_payprice").val();
                var cost = $("#hid_cost").val();
                var profit = $("#hid_profit").val();
                var u_num = $("#u_num").val();
                var u_name = $("#u_name").val();
                var u_phone = $("#u_phone").val();
                var u_idcard = $("#u_idcard").val();
                var u_traveldate = $("#u_traveldate").val();
                var Integral = $("#Integral_span").html();
                var Imprest = $("#Imprest_span").html();

                var manyspeci = $("#hid_manyspeci").val();
                var speciid = $("#hid_speciid").val();
                 var serverid= $("#hid_serverid").val();
                if(manyspeci==1){
                    if(speciid==0){
                        alert("请选择具体规格");
                        $("#loading").hide();
                        return;
                    }
                }
                <%if (issetidcard==1){%>
                    if(u_idcard==""){
                        alert("请填写身份证号");
                        $("#loading").hide();
                        return;
                    }
                
                <%} %>
                <%if (isSetVisitDate==1){%>
                    if(u_traveldate==""){
                        alert("请选择出游日期");
                        $("#loading").hide();
                        return;
                    }
                
                <%} %>
                


                var stardate = "";
                var enddate = "";
                var h_num = 1;
                if (server_type == "9") {
                    stardate = $("#stardate").val();
                    enddate = $("#enddate").val();
                    h_num = $("#h_num").val();
                    u_name = $("#h_name").val();
                    u_phone = $("#h_phone").val();
                }

                if ($("#Integral").attr('checked') == undefined) {
                    Integral = "";
                }
                if ($("#Imprest").attr('checked') == undefined) {
                    Imprest = "";
                }
                if ($("#hid_server_type").val() == 10) {
                    //判断游玩日期 不可为空
                    if (u_traveldate == "") {
                     $("#loading").hide();
                        alert("请选择游玩日期");
                        return;
                    }
                }
                if (u_name == "") {
                 $("#loading").hide();
                    $.prompt("请填写姓名");
                    return;
                }
                if (payprice == "") {
                 $("#loading").hide();
                    $.prompt("产品信息错误");
                    return;
                }
                if (u_phone == "") {
                 $("#loading").hide();
                    $.prompt("请填写手机号，来接收电子票彩信");
                    return;
                } else {
                    if (!isMobel(u_phone)) {
                     $("#loading").hide();
                        $.prompt("请正确填写手机号");
                        return;
                    }

                }

                var travelnames = ""; //乘车人姓名列表
                var travelidcards = ""; //乘车人身份证列表
                var travelnations = ""; //乘车人民族列表
                var travelphones = ""; //乘车人电话列表
                var travelremarks = ""; //乘车人备注列表

                var errid = "0"; // 输入格式错误的控件id
                if ($("#hid_server_type").val() == 10) {

                    //判断乘车人信息不可为空
                    for (var i = 1; i <= u_num; i++) {
                        var travel_name = $("#travelname" + i).val();
                        var travel_idcard = $("#travelidcard" + i).val();
                        var travel_nation = $("#travelnation" + i).val();
                        var travel_phone = $("#travelphone" + i).val();
                        var travel_remark = $("#travelremark" + i).val();


                        if (travel_name == "") {
                         $("#loading").hide();
                            alert("乘车人姓名不可为空");
                            errid = "travelname" + i;
                            break;
                        }
                        if (travel_phone == "") {
                         $("#loading").hide();
                            alert("乘车人" + travel_name + "电话不可为空");
                            errid = "travelname" + i;
                            break;
                        }
//                        if (travel_idcard == "") {
//                         $("#loading").hide();
//                            alert("乘车人" + travel_name + "身份证号不可为空");
//                            errid = "travelidcard" + i;
//                            break;

//                        } else {
//                            if (!IdCardValidate(travel_idcard)) {
//                             $("#loading").hide();
//                                alert("乘车人" + travel_name + "身份证格式错误");
//                                errid = "travelidcard" + i;
//                                break;
//                            }
//                        }
                        if (travel_idcard != "") 
                        {
                            if (!IdCardValidate(travel_idcard)) {
                             $("#loading").hide();
                                alert("乘车人" + travel_name + "身份证格式错误");
                                errid = "travelidcard" + i;
                                break;
                            }
                        }
//                        if (travel_nation == "") {
//                            alert("乘车人" + travel_name + "民族不可为空");
//                            errid = "travelnation" + i;
//                            break;
//                        }
                        travelnames += travel_name + ",";
                        travelidcards += travel_idcard + ",";
                        travelnations += travel_nation + ",";
                        travelphones += travel_phone + ",";
                        travelremarks += travel_remark + ",";



                    }
                    if (errid != "0") {
                     $("#loading").hide();
                        $(errid).focus();
                        return;
                    }
                }

                if (server_type == "9") {//订房订单
                    $.post("/JsonFactory/OrderHandler.ashx?oper=createorder", { Integral: 0, Imprest: 0, openid: '', proid: proid,speciid:speciid, ordertype: 1, payprice: payprice, u_num: h_num, u_name: u_name, u_phone: u_phone, u_traveldate: stardate, comid: comid, buyuid: 0, tocomid: 0, start_date: stardate, end_date: enddate, bookdaynum: $("#hid_hotel_day").val(), lastarrivaltime: $("#cometime").find("option:selected").text(), fangtai: $("#hid_hotel_dayprice").val() ,sid:serverid}, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#loading").hide();
                            alert("创建订单失败");
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            //                            if (data.dikou != "") {
                            //                                location.href = "backUrl.aspx?orderid=" + data.msg + " &comid=" + comid + " &dikou=" + data.dikou;
                            //                                return;
                            //                            }

                            if ($("#hid_ReserveType").val() == "1")//酒店客房 预订类型：1.不用支付直接发送短信2.预付发送短信3预付发送电子码
                            {
                                alert("预定成功，稍后酒店客服为您确认！");
                                return;
                            } else {
                                location.href = "/ui/vasui/pay.aspx?orderid=" + data.msg + "&comid=" + comid;
                                return;
                            }
                        }

                    })

                } else {
                    //创建订单
                    $.post("/JsonFactory/OrderHandler.ashx?oper=weborder", { uid: uid, Integral: Integral, Imprest: Imprest, proid: proid,speciid:speciid, ordertype: ordertype, payprice: payprice, cost: cost, profit: profit, u_num: u_num, u_name: u_name, u_phone: u_phone,u_idcard:u_idcard, u_traveldate: u_traveldate, travelnames: travelnames, travelidcards: travelidcards, travelnations: travelnations, travelphones: travelphones, travelremarks: travelremarks, travel_pickuppoints: $("#pointuppoint").val(), travel_dropoffpoints: $("#dropoffpoint").val(), order_remark: $("#order_remark").val(),sid:serverid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                         $("#loading").hide();
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {

                            location.href = "/ui/vasui/pay.aspx?orderid=" + data.msg; ;
                            return;
                        }
                    })
                }



                function callbackfunc(e, v, m, f) {
                    if (v == true)
                        location.href = "/ui/vasui/pay.aspx?orderid=" + m.children('#hid_orderid').val(); ;
                }

            })
            //人数增
            $(".btn-add").click(function () {

                if ($("#hid_server_type").val() == 9) {
                    var i = $("#h_num").val();
                    var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                    if (!patrn.exec(i)) {
                        $.prompt("请正确填写预订间数");
                        return;
                    }
                    else {
                        i++;
                        //限定预订数量
                        if (i > $("#hid_pro_Number").val() && $("#hid_pro_Number").val() != 0) {
                            i = $("#hid_pro_Number").val();
                            $.prompt("此产品限购" + $("#hid_pro_Number").val() + "间");
                        }

                        $("#h_num").val(i);

                        searchHouseType();
                    }
                } else {
                    var i = $("#u_num").val();
                    var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                    if (!patrn.exec(i)) {
                        $.prompt("请填写正确地宝贝数量");
                        return;
                    }
                    else {
                        i++;
                        //限定预订数量
                        if (i > $("#hid_pro_Number").val() && $("#hid_pro_Number").val() != 0) {
                            i = $("#hid_pro_Number").val();
                            $.prompt("此产品限购" + $("#hid_pro_Number").val() + "张");
                        }

                        $("#u_num").val(i);
                        //如果是旅游大巴产品，则需要增加乘车人信息
                        if ($("#hid_server_type").val() == 10) {
                            //旅游大巴产品，限制提单数量最高60
                            if (parseInt(i) > 60) {
                                alert("大巴产品限制预订数量最高为60人");
                                $("#u_num").val(60);
                                return;
                            }
                            $("#travel_div").css("height", 38 * i + "px");
                            $("#travel_tbody").append('<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                            '<label> 姓  名：</label><input type="text" id="travelname' + i + '" value=""  style="width:60px;" />' +
                              '<label> 联系电话：</label><input type="text" id="travelphone' + i + '" value="" style="width:100px;" />' +
                            '<label> 身份证：</label><input type="text" id="travelidcard' + i + '" value=""  style="width:150px;" />' +
                            //'<label>民  族：</label><input type="text" id="travelnation' + i + '" value="汉族"  style="width:40px;"  />' +
   //'<label>备注：</label><input type="text" id="travelremark' + i + '" value=""  style="width:100px;" />' +
                    '</td>' +
                '</tr> ');
                        }
                        var totalcount = $("#u_num").val() * $("#hid_payprice").val();
                        $("#heji").text("￥" + fmoney(totalcount, 2));
                    }
                }
            })
            //人数减
            $(".btn-reduce").click(function () {
                if ($("#hid_server_type").val() == 9) {
                    var i = $("#h_num").val();
                    var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                    if (!patrn.exec(i)) {
                        $.prompt("请正确填写预订间数");
                        return;
                    }
                    else {
                        i--;
                        if (i > 0) {
                            $("#h_num").val(i);
                            searchHouseType();
                        } else {

                            return false;
                        }
                    }
                } else {
                    var i = $("#u_num").val();
                    var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                    if (!patrn.exec(i)) {
                        $.prompt("请填写正确地宝贝数量");
                        return;
                    }
                    else {
                        i--;

                        if (i > 0) {
                            $("#u_num").val(i);

                            //如果是旅游大巴产品，则需要增加乘车人信息
                            if ($("#hid_server_type").val() == 10) {
                                $("#travel_div").css("height", 38 * i + "px");
                                var ihtml = "";
                                for (var j = 1; j <= i; j++) {
                                    var minzu = "汉族";
                                    if ($("#travelnation" + j).val() != '') {
                                        minzu = $("#travelnation" + j).val();
                                    }

                                    ihtml += '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                            '<label> 姓  名：</label><input type="text" id="travelname' + j + '" value="' + $("#travelname" + j).val() + '"  style="width:60px;"  />' +
                              '<label> 联系电话：</label><input type="text" id="travelphone' + j + '" value="' + $("#travelphone" + j).val() + '" style="width:100px;" />' +
                            '<label> 身份证：</label><input type="text" id="travelidcard' + j + '" value="' + $("#travelidcard" + j).val() + '"   style="width:150px;" />' ;
                            //'<label>民族：</label><input type="text" id="travelnation' + j + '" value="' + minzu + '"  style="width:40px;"  />' +
                                 //'<label>备注：</label><input type="text" id="travelremark' + j + '" value="' + $("#travelremark" + j).val() + '"  style="width:100px;" />';
                                    if (j == 1) {
                                        ihtml += '<input name="chkItem" type="checkbox" value="1" id="chkItem" />与预定人相同';
                                    }
                                    ihtml += '</td>' +
                '</tr> ';
                                }
                                $("#travel_tbody").html(ihtml);

                            }

                            var totalcount = $("#u_num").val() * $("#hid_payprice").val();
                            $("#heji").text("￥" + fmoney(totalcount, 2));
                        } else {

                            return false;
                        }
                    }
                }


            })
            $("#u_num").keyup(function () {

                var i = $(this).val();
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    $.prompt("请填写正确地宝贝数量");
                    return;
                } else {
                    //限定预订数量
                    if (i > $("#hid_pro_Number").val() && $("#hid_pro_Number").val() != 0) {
                        i = $("#hid_pro_Number").val();
                        $("#u_num").val($("#hid_pro_Number").val());
                        $.prompt("此产品限购" + $("#hid_pro_Number").val() + "张");
                    }

                    //如果是旅游大巴产品，则需要增加乘车人信息
                    if ($("#hid_server_type").val() == 10) {
                        //旅游大巴产品，限制提单数量最高60
                        if (parseInt(i) > 60) {
                            alert("大巴产品限制预订数量最高为60人");
                            $("#u_num").val(60);
                            i = 60;
                            //                            return;
                        }
                        $("#travel_div").css("height", 38 * i + "px");
                        var ihtml = "";
                        for (var j = 1; j <= i; j++) {
                            var minzu = "汉族";
                            if ($("#travelnation" + j).val() != '') {
                                minzu = $("#travelnation" + j).val();
                            }

                            ihtml += '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                            '<label> 姓  名：</label><input type="text" id="travelname' + j + '" value="' + $("#travelname" + j).val() + '"  style="width:80px;"  />' +
                               '<label> 联系电话：</label><input type="text" id="travelphone' + j + '" value="' + $("#travelphone" + j).val() + '" style="width:100px;" />' +
                            '<label> 身份证：</label><input type="text" id="travelidcard' + j + '" value="' + $("#travelidcard" + j).val() + '"   style="width:180px;" />' ;
                            //'<label>民族：</label><input type="text" id="travelnation' + j + '" value="' + minzu + '"  style="width:50px;"  />' +
                               //'<label>备注：</label><input type="text" id="travelremark' + j + '" value="' + $("#travelremark" + j).val() + '"  style="width:100px;" />';
                            if (j == 1) {
                                ihtml += '<input name="chkItem" type="checkbox" value="1" id="chkItem" />与预定人相同';
                            }
                            ihtml += '</td>' +
                '</tr> ';
                        }
                        $("#travel_tbody").html(ihtml);
                    }

                    var totalcount = $("#u_num").val() * $("#hid_payprice").val();
                    $("#heji").text("￥" + fmoney(totalcount, 2));
                }
            });


            $("#h_num").keyup(function () {

                var i = $(this).val();
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    $.prompt("请正确填写预订间数");
                    return;
                } else {
                    //限定预订数量
                    if (i > $("#hid_pro_Number").val() && $("#hid_pro_Number").val() != 0) {
                        i = $("#hid_pro_Number").val();
                        $("#h_num").val($("#hid_pro_Number").val());
                        $.prompt("此产品限购" + $("#hid_pro_Number").val() + "间");
                    }
                    searchHouseType();
                }
            });



            $("#stardate").change(function () {
                searchHouseType();

            });
            $("#enddate").change(function () {
                searchHouseType();
            });


          

            //与预定人相同
            $("#chkItem").live("click", function () {
                if ($(this).is(":checked")) {
                    if ($("#u_name").val() != "") {
                        $("#travelname1").val($("#u_name").val());
                        $("#travelphone1").val($("#u_phone").val());
                    }
                    $("#hid_issamewithbooker").val("1");
                } else {
                    $("#hid_issamewithbooker").val("0");
                }
            });



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

        <%if(Wrentserver==1){ //加载 服务%>

            $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=SelectServerlist",
                    data: { comid: comid,proid: $("#hid_proid").val()  },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {
                            //$(".sys_item_spec").empty();
                            $(".sys_item_specpara_server").show();
                            $("#ServerItemEdit").tmpl(data.msg).appendTo("#serverlist");
                        }
                    }
            })
        <%} %>

        })

        function GetDateDiff(startTime, endTime, diffType) {
            //将xxxx-xx-xx的时间格式，转换为 xxxx/xx/xx的格式
            startTime = startTime.replace(/\-/g, "/");
            endTime = endTime.replace(/\-/g, "/");
            //将计算间隔类性字符转换为小写
            diffType = diffType.toLowerCase();
            var sTime = new Date(startTime);      //开始时间
            var eTime = new Date(endTime);  //结束时间
            //作为除数的数字
            var divNum = 1;
            switch (diffType) {
                case "second":
                    divNum = 1000;
                    break;
                case "minute":
                    divNum = 1000 * 60;
                    break;
                case "hour":
                    divNum = 1000 * 3600;
                    break;
                case "day":
                    divNum = 1000 * 3600 * 24;
                    break;
                default:
                    break;
            }
            return parseInt((eTime.getTime() - sTime.getTime()) / parseInt(divNum));
        }

        function AddDays(date, days) {
            var nd = new Date(date);
            nd = nd.valueOf();
            nd = nd + days * 24 * 60 * 60 * 1000;
            nd = new Date(nd);
            //alert(nd.getFullYear() + "年" + (nd.getMonth() + 1) + "月" + nd.getDate() + "日"); 
            var y = nd.getFullYear();
            var m = nd.getMonth() + 1;
            var d = nd.getDate();
            if (m <= 9) m = "0" + m;
            if (d <= 9) d = "0" + d;
            var cdate = y + "-" + m + "-" + d;
            return cdate;
        }

        //查询房态
        function searchHouseType() {
            var nowdate_temp = $("#stardate").val();
            var enddate_temp = $("#enddate").val();

            var h_num = $("#h_num").val();

            var DateDiff = GetDateDiff(nowdate_temp, enddate_temp, "day");
            if (DateDiff < 1) {
                $.prompt("入住日期必须小于离店日期！");
                enddate_temp = AddDays(nowdate_temp, 1);
                $("#enddate").val(enddate_temp);
                return;
            }


            $("#hid_hotel_day").val(DateDiff);
            var h_heji = 0;
            var day_price = "";
            $.post("/JsonFactory/ProductHandler.ashx?oper=GetHouseTypeDayList", { proid: $("#hid_proid").val(), startdate: nowdate_temp, enddate: enddate_temp }, function (data1) {
                data1 = eval("(" + data1 + ")");
                if (data1.type == 1) {
                    $.prompt(data1.msg);
                    //$("#orderstate").vla("0");
                    return;
                }
                if (data1.type == 100) {
                    for (var i = 0; i < data1.msg.length; i++) {
                        h_heji += data1.msg[i].Menprice;
                        day_price += data1.msg[i].Menprice + ","
                    }
                    //平均单价
                    $("#h_Advise_price").text(fmoney(h_heji / data1.msg.length, 2));
                    $("#hid_payprice").val(fmoney(h_heji, 2));

                    h_heji = h_num * h_heji; //计算总价
                    $("#hid_hotel_dayprice").val(day_price);
                    $("#h_heji").text("￥" + fmoney(h_heji, 2));
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

    </script>
    <style type="text/css">
     /*以下一行代码是日历选中样式*/
        .ui-datepicker-current-day {
            color: #FFF !important;
            background: none repeat scroll 0% 0% #FF6000;
            background-color: #0099FF !important;
            text-align:center;
        }
         .ui-datepicker td {
        text-align: center;
        }
        td label
        {
            padding:5px; 
        }
        .ui-datepicker-today a {
          background-color: #FFF;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div class="main">
        <h3 class="titlepng">
        </h3>
        <div id="ticket" style="display: none;">
            <div class="separationBox">
                <div class="bottomLineBanner">
                    <span class="bottomLineName">预订信息</span>
                </div>
                <div class="bookOrderDetails">
                    <span class="bookOrderDetailsLeft">单  价：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" vfn="calendarVerify">
                        ￥<label id="Advise_price" class="sys_item_price">
                        </label>
                    </div>
                </div>
                
                 
                <div class="bookOrderDetails">
                    <span class="bookOrderDetailsLeft">数  量：</span>
                    <div class="bookOrderDetailsRight wrap-input">
                        <a href="javascript:void(0);" class="btn-reduce">减少数量</a>
                        <input id="u_num" name="u_num" type="text" value="1" class="input-ticket" autocomplete="off"
                            maxlength="4" style="width: 50px;" readonly="readonly">
                        <a href="javascript:void(0);" class="btn-add">增加数量</a>
                    </div>
                    <span id="marpeapal_error34009" class="prompt none"><span class="tpo"></span><b class="arialFont">
                        1</b>张起订 </span>
                </div>
                <div class="" style="border: 1px solid #CCC;  margin:10px;">
               <%if (manyspeci == 1 || Wrentserver==1)//多规格，绑定服务 都显示出多规格选项
          {//当多规格，显示规格 %>
                <span class="bookOrderDetailsLeft">请选择票的规格：</span>
                 

                    <div class="iteminfo_buying">
                        <!--规格属性-->
	                    <div class="sys_item_spec">


                        <dl class="clearfix iteminfo_parameter sys_item_specpara_server" data-sid="" style=" display:none;">
			            <dt>更多服务及押金</dt>
			                <dd>
				                <ul class="sys_spec_text" id="serverlist">

                            
               
                                </ul>

			                </dd>
		                </dl>
	                    </div>
	                    <!--规格属性-->
                    </div>
                    <%} %>
                 </div>

                <div class="bookOrderDetails" style="display: none">
                    <span class="bookOrderDetailsLeft">日  期：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" vfn="calendarVerify">
                        <input value="" class="dataNum dataIcon" readonly="readonly" id="u_traveldate">
                    </div>
                    <span class="bookLimit" style="display:none;">(为了能成功提交订单，您需在游玩当天12:00前下单，请尽早预订。)</span>
                </div>
                <div class="fanBox fanANDdi piaowubeizhu" style=" display:none;">
                    <span class="fanIcon"></span><span>票务备注：<label id="ticket_remark"></label>
                    </span>
                </div>
            </div>
            <div class="separationBox">
                <div class="bottomLineBanner">
                    <span class="bottomLineName">购买说明</span>
                </div>
                <div class="bookOrderDetails line">
                    <span class="bookOrderDetailsLeft">产品有效期 ：</span>
                    <label id="Perioddate">
                    </label>
                </div>
                <div class="bookOrderDetails countprice">
                    合计(<span class="pricefont">需在线支付</span>):<span class="pricefont" id="heji"></span></div>
            </div>
            <div class="separationBox">
                <div class="bottomLineBanner">
                    <%if (pro_servertype == 10)
                      {
                    %>
                    <span class="bottomLineName">预订人信息</span>
                    <%
                        }
                      else
                      {
                    %>
                    <span class="bottomLineName">取票人信息</span>
                    <%
                        } %>
                </div>
                <div class="bookOrderDetails">
                    <div id="inte" style="display: block; float: left">
                        <input type="checkbox" id="Integral" />积分余额<span id="Integral_span"></span>元</div>
                    <div id="impr" style="display: block; float: left">
                        <input type="checkbox" id="Imprest" />预付款余额<span id="Imprest_span"></span>元</div>
                </div>
                <div class="bookOrderDetails">
                    <%if (pro_servertype == 10)
                      {
                    %>
                    <span class="bookOrderDetailsLeft">预 订 人 ：</span>
                    <%
                        }
                      else
                      {
                    %>
                    <span class="bookOrderDetailsLeft">取 票 人 ：</span>
                    <%
                        } %>
                    <div class="bookOrderDetailsRight dataMarginBox" id="div" name="choseTime_34009"
                        vtype="rq" data-err-rq="" data-err-fn="" vfn="calendarVerify">
                        <input name="Input" class="dataNum dataIcon" id="u_name" value="" />
                    </div>
                    <span class="bookLimit">(实名认证，请填写出游人姓名)</span>
                </div>
                <div class="bookOrderDetails">
                    <span class="bookOrderDetailsLeft">手 机 号 ：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" id="div2" name="choseTime_34009"
                        vtype="rq" data-err-rq="" data-err-fn="" vfn="calendarVerify">
                        <input name="u_phone" class="dataNum dataIcon" id="u_phone" value="" />
                    </div>
                    <span class="bookLimit">(免费接收订单确认短信，请务必正确填写)</span>
                </div>
                <div class="bookOrderDetails"  id="idcardview" style=" display: <%if (issetidcard==0){%>none<%} %>;">
                    <span class="bookOrderDetailsLeft">身 份 证 ：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" id="div3" name="choseTime_34009"
                        vtype="rq" data-err-rq="" data-err-fn="" vfn="calendarVerify">
                        <input name="u_idcard" class="dataNum dataIcon" id="u_idcard" value="" />
                    </div>
                    <span class="bookLimit">(刷身份证取票)</span>
                </div>
                <div class="bookOrderDetails" name="tbody_busplus" style="display: none;">
                    <!--旅游大巴附加信息-->
                    <span class="bookOrderDetailsLeft">上车地点 ：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" vtype="rq" data-err-rq="" data-err-fn=""
                        vfn="calendarVerify">
                        <select id="pointuppoint">
                        </select>
                    </div>
                </div>
                <div class="bookOrderDetails" name="tbody_busplus" id="tr_dropoffpoint" style="display: none;">
                    <span class="bookOrderDetailsLeft">下车地点 ：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" vtype="rq" data-err-rq="" data-err-fn=""
                        vfn="calendarVerify">
                        <select id="dropoffpoint">
                        </select>
                    </div>
                </div>
                <div class="bookOrderDetails" name="tbody_busplus" style="display: none;">
                    <span class="bookOrderDetailsLeft">更多需求 ：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" vtype="rq" data-err-rq="" data-err-fn=""
                        vfn="calendarVerify">
                         <input  class="dataNum dataIcon" id="order_remark" value="" size="100" />
                        
                    </div>
                </div>
            </div>
            <div class="separationBox" id="travel_div" style="display: none;">
                <div class="bottomLineBanner">
                    <span class="bottomLineName">乘车人信息(请务必填写用于保险):</span>
                </div>
                <div class="bookOrderDetails">
                    <table width="900px" class="grid" id="travel_tb" style="display: none;">
                        <tbody id="travel_tbody">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div id="hotel">
            <div class="separationBox">
                <div class="bottomLineBanner">
                    <span class="bottomLineName">预订信息</span>
                </div>
                <div class="bookOrderDetails">
                    <span class="bookOrderDetailsLeft">房间单价：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" vfn="calendarVerify">
                        ￥<label id="h_Advise_price">
                        </label>
                    </div>
                </div>
                <div class="bookOrderDetails">
                    <span class="bookOrderDetailsLeft">预订房间：</span>
                    <div class="bookOrderDetailsRight wrap-input">
                        <a href="javascript:void(0);" class="btn-reduce">减少数量</a>
                        <input id="h_num" name="h_num" type="text" value="1" class="input-ticket" autocomplete="off"
                            maxlength="4">
                        <a href="javascript:void(0);" class="btn-add">增加数量</a>
                    </div>
                    <span class="prompt none"><span class="tpo"></span>间 </span>
                </div>
                <div class="bookOrderDetails">
                    <span class="bookOrderDetailsLeft">入住日期：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" vfn="calendarVerify">
                        入住
                        <input value="" class="dataNum dataIcon " readonly="readonly" id="stardate" isdate="yes">
                        离店
                        <input value="" class="dataNum dataIcon" readonly="readonly" id="enddate" isdate="yes">
                    </div>
                </div>
                <div class="bookOrderDetails">
                    <span class="bookOrderDetailsLeft">最晚到店：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" vfn="calendarVerify">
                        <select id="cometime">
                            <option value="1">当日20:00之前</option>
                            <option value="2">当日20:00之后(需担保)</option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="separationBox">
                <div class="bottomLineBanner">
                    <span class="bottomLineName">合计</span>
                </div>
                <div class="bookOrderDetails countprice">
                    合计(<span class="pricefont" id="zaixianzhifu">需在线支付</span>):<span class="pricefont"
                        id="h_heji"></span></div>
            </div>
            <div class="separationBox">
                <div class="bottomLineBanner">
                    <span class="bottomLineName">预订信息</span>
                </div>
                <div class="bookOrderDetails">
                    <span class="bookOrderDetailsLeft">入住人姓名 ：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" name="choseTime_34009" vtype="rq"
                        data-err-rq="" data-err-fn="" vfn="calendarVerify">
                        <input name="h_name" class="dataNum dataIcon" id="h_name" value="" />
                    </div>
                    <span class="bookLimit">(实名认证，请填写出游人姓名)</span>
                </div>
                <div class="bookOrderDetails">
                    <span class="bookOrderDetailsLeft">联系人手机 ：</span>
                    <div class="bookOrderDetailsRight dataMarginBox" id="div6" name="choseTime_34009"
                        vtype="rq" data-err-rq="" data-err-fn="" vfn="calendarVerify">
                        <input name="h_phone" class="dataNum dataIcon" id="h_phone" value="" />
                    </div>
                    <span class="bookLimit">(免费接收订单确认短信，请务必正确填写)</span>
                </div>
            </div>
        </div>
        <div id="calDiv" style="display: none; margin-top: -40px">
        </div>
        <div class="submitAll" title="提交订单">
            <div class="submitAllIcon" id="confirmButton">
                提交订单</div>
            <span class="payOnlineIcon none"></span>
        </div>
        <div class="separationBox">
            <div class="bottomLineBanner">
                <span class="bottomLineName">说明</span>
            </div>
            <div id="yuding_msg">
            </div>
        </div>
        <input type="hidden" id="hidLeavingDate" value="" />
        <input type="hidden" id="hidMinLeavingDate" value="" />
        <input type="hidden" id="hidLinePrice" value="" />
        <input type="hidden" id="hidchildreduce" value="<%=childreduce %>" />
         <input id="hidEmptyNum" type="hidden" value="" />
    </div>
    <input type="hidden" id="hid_payprice" value="" />
    <input type="hidden" id="hid_cost" value="" />
    <input type="hidden" id="hid_profit" value="" />
    <input type="hidden" id="hid_pro_Number" value="0" />
    <input type="hidden" id="hid_server_type" value="" />
    <input type="hidden" id="orderstate" value="1" />
    <input type="hidden" id="hid_hotel_day" value="1" />
    <input type="hidden" id="hid_hotel_dayprice" value="" />
    <input type="hidden" id="hid_ReserveType" value="" />
    <!--上车地点，下车地点-->
    <input id="hid_pickuppoint" type="hidden" value="<%=pickuppoint %>" />
    <input id="hid_dropoffpoint" type="hidden" value="<%=dropoffpoint %>" />

    <!--乘车人信息 与预订人相同-->
    <input type="hidden" id="hid_issamewithbooker" value="0" />
    <input id="hid_manyspeci" value="<%=manyspeci %>" type="hidden"  />
    <input id="hid_speciid" value="0" type="hidden"  />
    	<input id="hid_serverid" value="" type="hidden">
	<input id="hid_server_price" value="0" type="hidden">

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
<script type="text/x-jquery-tmpl" id="ServerItemEdit"> 

        <li data-aid="${id}" data-dprice="${serverDepositprice}" data-sprice="${saleprice}"><a href="javascript:;" title="${servername}">${servername}</a><i></i></li>

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
                    if(_val !=""){
                    _mktprice = sys_item['sys_attrprice'][_val]['mktprice'];
                    _price = sys_item['sys_attrprice'][_val]['price'];
                     $("#hid_speciid").val(sys_item['sys_attrprice'][_val]['speciid'])
                    }else{
                     _mktprice = sys_item['mktprice'];
                    _price = sys_item['price'];
                    $("#hid_speciid").val(0);
                    }
                } else {
                    _mktprice = sys_item['mktprice'];
                    _price = sys_item['price'];
                    $("#hid_speciid").val(0);
                }


                //增加读取服务
				var sid="";
				var sprice=0;
				var dprice=0;
						
				$(".sys_item_spec .sys_item_specpara_server ul li").each(function () {
                    var selveri = $(this);
                    if($(this).hasClass("selected")){
						 sid += $(this).attr("data-aid")+",";
						 sprice += parseInt($(this).attr("data-sprice")*100);
						 dprice += parseInt($(this).attr("data-dprice")*100);
					}
                })
				if(sid !=""){
					$("#hid_serverid").val(sid);
					$("#hid_server_price").val((Number(sprice+dprice)/100).toFixed(2));
					$("#serverfuwu").show();
					$(".serverfuwu").html("含服务费:￥"+ sprice/100 +" 押金：￥"+dprice/100).show();;
					_price = (sprice+dprice+(_price*100))/100;
				}else{
					$("#hid_serverid").val("");
					$("#hid_server_price").val(0);
                    $(".serverfuwu").hide();
				}

                //输出价格
                $(_resp.mktprice).text(_mktprice);  ///其中的math.round为截取小数点位数
                $(_resp.price).text(_price);
                $("#hid_payprice").val(_price);
                 var totalcount = $("#u_num").val() * $("#hid_payprice").val();
                 $("#heji").text("￥" + fmoney(totalcount, 2));
            }

            //-------------------------------------服务选择--------------------------
			$(".sys_item_spec .sys_item_specpara_server").each(function () {
                var i = $(this);
                var p = i.find("ul>li");
                p.click(function () {
                    if (!!$(this).hasClass("selected")) {
                        $(this).removeClass("selected");
                        i.removeAttr("data-attrval");
                    } else {
                        $(this).addClass("selected");
                        i.attr("data-attrval", $(this).attr("data-aid"))
                    }
                    getattrprice(); //输出价格
                })
            })
        })
</script>
   <div id="loading" class="loading" style="display: none;">
            正在加载...
   </div>
</asp:Content>
