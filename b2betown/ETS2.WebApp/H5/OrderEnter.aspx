<%@ Page Title="" Language="C#"  AutoEventWireup="true"
    CodeBehind="OrderEnter.aspx.cs" Inherits="ETS2.WebApp.H5.OrderEnter" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html class="no-js " lang="zh-CN">
<head>
 <!-- meta信息，可维护 -->
    <meta charset="utf-8" />
    <meta name="keywords" content="<%=title %>" />

    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta content="telephone=no" name="format-detection" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    
    <title>
        班车预订</title>

    <link href="../Styles/H5/Order.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-orderrili.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
     

    <!-- 页面样式表 -->
    <style type="text/css">
        dd
        {
            font-size: 12px !important;
        }
        select 
        {
            font-size: 12px !important;
        }
         td
        {
            font-size: 12px !important;
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
        .in-item-number dd {
            text-align: left !important;
        }
    .w-item label {
        padding-left: 15px;
    }
    .btn {
        font-size: 16px !important;
    }
#showMsg{position:fixed;z-index:99999;width:84%;margin:0 8%;border-radius:3px;box-shadow:0 0 5px #333;/*! display:none */}
.msg-title{height:35px;text-align:center;line-height:35px;font-size:14px;color:#fff;background:#1a9ed9;filter:alpha(opacity=80);-moz-opacity:.8;-khtml-opacity:.8;opacity:.8;text-shadow:0;font-weight:bold;border-top-left-radius:3px;border-top-right-radius:3px}
.msg-content{padding:10px 15px;line-height:1.8em;background:#fff}
.msg-btn{padding:8px 0 6px 0;height:35px;background:#f5f5f5;border-top:1px solid #ddd;border-bottom-left-radius:3px;border-bottom-right-radius:3px;text-align:center}
.msg-btn a{position:relative;display:inline-block;line-height:30px;padding:0 30px;font-size:12px;color:#fff;text-shadow:0;border-radius:3px;margin:0 5px;background:#ff8533}
.msg-btn a:hover{color:#fff;text-decoration:none}
.msg-btn a:visited{color:#fff}
.msg-btn a.disable{color:#666;border:solid 1px #999;background:#eee;background:-webkit-gradient(linear,left top,left bottom,from(#eee),to(#ccc));background:-moz-linear-gradient(top,#eee,#ccc);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#eee',endColorstr='#ccc')}

    </style>
    <script type="text/javascript">
        $(function () {
              <%if(comid==112){ %>
              //112商户暂时使用为预约，不显示班车的上下车信息
               $(".banche").hide();

              <%} %>

        
                <%if (Iuse==1){ %>
                  <%if (bindingname !=""){%>
                      $("#getName_1").attr("disabled", "disabled");
                      $("#getPhone_1").attr("disabled", "disabled");
                      $("#travelname1").attr("disabled", "disabled");
                      $("#travelphone1").attr("disabled", "disabled");

                      $("#getName_1").val("<%=bindingname%>");
                      $("#getPhone_1").val("<%=bindingphone%>");
                      $("#travelname1").val("<%=bindingname%>");
                      $("#travelphone1").val("<%=bindingphone%>");

                      <%}else{ %>
                       alert("您的预约码第一次使用，预约成功后 将绑定此次预约人,限定为绑定人使用。");
                      <%} %>
                  <%} %>

                  
                <%if (Iuse==2){ %>
                  <%if (bindingname !=""){%>
                      $("#getName_1").attr("disabled", "disabled");
                      $("#getPhone_1").attr("disabled", "disabled");
                      $("#getName_1").val("<%=bindingname%>");
                      $("#getPhone_1").val("<%=bindingphone%>");

                   <%} %>
               <%} %>
        //服务类型是旅游大巴，需要加载团期日历
        if(<%=pro_servertype %>==10){ 
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
                } else {
                     $("#tr_dropoffpoint").hide();
                }



                $("#pointuppoint").html(pickuppointstr);
                $("#dropoffpoint").html(dropoffpointstr);

                $("#travel_tbody").append(
                 '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2"><div class="w-item">' +
                            '<label>姓  名：</label><input type="text"  placeholder="姓名" id="travelname1" value="<%=bindingname%>" style="width:60px;" />' +
                            '<label><input name="chkItem" type="checkbox" value="1" id="chkItem" style="width: auto;margin-right: auto;height: auto;margin-left: 15px;"/>同预定人</label>' +
                            '<br><label>联系电话：</label><input type="tel"  placeholder="电话" id="travelphone1" value="<%=bindingphone%>" style="width:100px;" />' +
                            '<br><label>身份证：</label><input type="text"  placeholder="身份证" id="travelidcard1" value="" style="width:180px;"/>' +

                    '</div></td>' +
                '</tr> ');
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=GetLineById",
                    data: { lineid: <%=id %> },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
        //                    $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            //団期处理
                            var datestr = "";
                            var datestr_zhehang = "<br>";
                            var firstdate = "";
                            var firstdate_price="0";
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
                                        firstdate_price=data.linedate[i].Menprice;
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
                               
                                //$("#datetime").val(firstdate);
                                $("#Span1").html(firstdate);
                                $("#nowdate").val(firstdate);
                                $("#sPrice").html(firstdate_price);
                                $("#amount").val(firstdate_price);
                                $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + firstdate_price + '</b>');
                            }
                        }
                    }
                })

               //団期日历
                $("#calDiv").datepicker({
                    minDate: $("#hidMinLeavingDate").val(),
                    onSelect: function (dateText) {
                        $("#datetime").val(dateText);
                        $("#Span1").html(dateText);
                        $("#nowdate").val(dateText);
                        var datearr = $("#hidLeavingDate").val().split(",");
                        var pricearr = $("#hidLinePrice").val().split(",");
                        var emptyarr = $("#hidEmptyNum").val().split(",");
                        var childreduce = $("#hidchildreduce").val();
                        for (var i = 0; i < datearr.length; i++) {
                            if (datearr[i] == dateText) {
                                $("#sPrice").html(pricearr[i]);
                                 $("#amount").val(pricearr[i]);
                                $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + parseInt(pricearr[i])*parseInt($("#orderNum").val()) + '</b>');
                                //$("#childPrice").html(parseInt(pricearr[i]) - parseInt(childreduce));
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
          
                //日历
                $("#selDate").click(function () {
                    scrollTo(0, 1);
                    thisMonth("traveldate");
                    $("#orderFome").removeClass().addClass("translate3d");
                    $("#calDiv").fadeIn(); $("#inner").hide();
                    $("footer").hide();
                });
            }
                         
          //服务类型是保险产品
          if(<%=pro_servertype %>==14){
                 $("#selDate").parent().show();


                 $("#div_baoxian").show();
                 $("#baoxian_tbody").append(
                 '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                    '<label>姓名：</label><input type="text" name="baoxianname"  placeholder="姓名" id="baoxianname1" value="" style="width:60px;"/>' +
                    '<label>拼音/英文名：</label><input type="text" name="baoxianpinyinname"  placeholder="拼音/英文名" id="baoxianpinyinname1" value="" style="width:80px;"/><br>' + 
                    '<label>身份证：</label><input type="text" name="baoxianidcard"  placeholder="身份证" id="baoxianidcard1" value="" style="width:140px;" />' +  
                    '</td>' +
                '</tr> ');
          }
           
           //服务类型是票务，需要添加预约日期
           if(<%=pro_servertype %>==1){
                var curentDate=CurentDate(); 
                $("#datetime").text(curentDate);  

                if(<%=isSetVisitDate%>==1){
                    $("#selDate").parent().show();
                    //団期日历
                                                                                                                                     $("#calDiv").datepicker({
                    minDate: curentDate,
                    onSelect: function (dateText) {
                        $("#datetime").val(dateText);
                        $("#Span1").html(dateText);
                        $("#nowdate").val(dateText);
                     

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
          
                    //日历
                    $("#selDate").click(function () {
                        scrollTo(0, 1);
                        thisMonth("traveldate");
                        $("#orderFome").removeClass().addClass("translate3d");
                        $("#calDiv").fadeIn(); $("#inner").hide();
                        $("footer").hide();
                    });
                }
           }
            
            //积分
            $("#Integral").click(function () {
                if ($("#hid_In").val() == 0) {
                    $("#Integral").removeClass("qb_icon icon_checkbox");
                    $("#Integral").addClass("qb_icon icon_checkbox checked");

                    $("#hid_In").val(1);
                }
                else {
                    $("#Integral").removeClass("qb_icon icon_checkbox checked");
                    $("#Integral").addClass("qb_icon icon_checkbox");

                    $("#hid_In").val(0);
                }
            })
            //预付款
            $("#Imprest").click(function () {
                if ($("#Im").val() == 0) {
                    $("#Imprest").removeClass("qb_icon icon_checkbox");
                    $("#Imprest").addClass("qb_icon icon_checkbox checked");

                    $("#Im").val(1);
                }
                else {
                    $("#Imprest").removeClass("qb_icon icon_checkbox checked");
                    $("#Imprest").addClass("qb_icon icon_checkbox");

                    $("#Im").val(0);
                }
            })

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

            var b = $("#maxpeapalMin").val(),
	        a = <%=num %>;
            b = b == 0 ? 1 : b;

             $("#orderNum").val(b);


            
            $("#reduceP").bind("click", function () {
            
                var e = $("#amount").val();
                var d = parseInt($("#maxpeapalMin").val(), 10);
                var c = $("#orderNum").val();
                if (/^\d+$/g.test(c) && d < c) {
                    c = parseInt(c, 10) - 1;
                    $("#orderNum").val(c);
                    ticketsChange();
                   
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
                              '<label>姓  名：</label><input type="text"  placeholder="姓名" id="travelname' + j + '" value="' + $("#travelname" + j).trimVal() + '"  style="width:60px;"  />' +
                              '<label>联系电话：</label><input type="tel"  placeholder="电话" id="travelphone' + j + '" value="' + $("#travelphone" + j).trimVal() + '"  style="width:100px;"  />' +
                              '<br><label>身份证</label><input type="text"  placeholder="身份证" id="travelidcard' + j + '" value="' + $("#travelidcard" + j).trimVal() + '"   style="width:180px;" />' +
                                '<br><input name="chkItem" type="checkbox" value="1" id="chkItem" style="width: auto;margin-right: auto;height: auto;margin-left: 15px;"/>与预定人相同' + 
                    '</div></td>' +
                '</tr> ';
                                }
                                $("#travel_tbody").html(ihtml);

                            }

                    //如果是保险产品，则需要增加被保险人信息
                    if ($("#hid_server_type").val() == 14) {

                        var ihtml = "";
                        for (var j = 1; j <= c; j++) {
                              

                            ihtml += '<tr>' +
                '<td class="tdHead"  valign="top" colspan="2">' +
                        '<label>姓名：</label><input type="text" name="baoxianname"  placeholder="姓名" id="baoxianname' + j + '" value="' + $("#travelname" + j).trimVal() + '" style="width:60px;"/>' +
                            '<label>拼音/英文名：</label><input type="text" name="baoxianpinyinname"  placeholder="拼音/英文名" id="baoxianpinyinname' + j + '" value="' + $("#travelphone" + j).trimVal() + '"  style="width:80px;" /><br>' +
                        '<label>身份证：</label><input type="text" name="baoxianidcard"   placeholder="身份证" id="baoxianidcard' + j + '" value="' + $("#travelidcard" + j).trimVal() + '" style="width:140px;"  />';
                                
                            ihtml += '</td>' +
            '</tr> ';

                        }
                        $("#baoxian_tbody").html(ihtml);

                    }


                     var f = $("#orderNum").val();
                    
                    $("#priceTotal").html('￥<b style=" font-size:18px;"> ' +fmoney(e * f,2) + '</b>');
                }
            });
            $("#plusP").bind("click", function () {

                var e = $("#amount").val();

                var c = parseInt($("#maxpeapalMax").val(), 10);
                var d = $("#orderNum").val();
                if (/^\d+$/g.test(d) && c > d) {
                    d = parseInt(d, 10) + 1;
                    $("#orderNum").val(d);
                    ticketsChange();
                 
                    //如果是旅游大巴产品，则需要增加乘车人信息
                    if ($("#hid_server_type").val() == 10) {
                             //旅游大巴产品，限制提单数量最高60
                            if (parseInt(d) > 60) {
                                alert("本产品限制每笔订单最高预订数量为60人");
                                $("#orderNum").val(60);
                                return;
                            }

                            $("#travel_tbody").append('<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2"><div class="w-item">' +
                            '<label>姓  名：</label><input type="text"  placeholder="姓名" id="travelname' + d + '" value=""  style="width:60px;" />' +
                            '<br><label>联系电话：</label><input type="tel"  placeholder="电话" id="travelphone' + d + '" value=""  style="width:100px;" />' +
                            '<br><label>身份证：</label><input type="text"   placeholder="身份证" id="travelidcard' + d + '" value=""  style="width:180px;" />' +
                    '</div></td>' +
                '</tr> ');
                        }

                      //如果是保险产品，则需要增加被保险人信息
                    if ($("#hid_server_type").val() == 14) {
                        //保险产品，限制提单数量最高50
                        if (parseInt(d) > 50) {
                            alert("保险产品限制预订数量最高为50人");
                            $("#u_num").val(50);
                            return;
                        }

                        $("#baoxian_tbody").append(
                 '<tr>' +
                    '<td class="tdHead"  valign="top" colspan="2">' +
                   '<label>姓名：</label><input type="text" name="baoxianname"  placeholder="姓名" id="baoxianname' + d + '" value=""  style="width:60px;"/>' + '<label>拼音/英文名：</label><input type="text" name="baoxianpinyinname"   id="baoxianpinyinname' + d + '" value=""  style="width:80px;" /><br>' +
                  '<label>身份证：</label><input type="text" name="baoxianidcard"  placeholder="身份证" id="baoxianidcard' + d + '" value="" style="width:140px;" />' + 
                    '</td>' +
                '</tr> ');

                    }

                    var f = $("#orderNum").val();
                    $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + fmoney(e * f,2) + '</b>');
                }
            });

        $("#submitBtn1").click(function () {
          
                <% 
                        // ispanicbuy 抢购
                        // panic_begintime  开始时间
                        // panicbuy_endtime 抢购结束
                        // limitbuytotalnum 抢购总数
               if (ispanicbuy ==1){
              %>
                var dinggou = $("#hid_dinggou").val();
               
                

                if (dinggou == "0") {
                    return false
                }
                <%} %>
                 var hid_pro_num = $("#hid_pro_num").val();
                  var number = $("#orderNum").val();
                  if(parseInt(hid_pro_num)<parseInt(number)){
                    if(parseInt(hid_pro_num) !=0){
                    alert("预定数量大于限购数量！"+hid_pro_num+number);
                    return false;
                    }

                }

                $("#loading").hide();
                //$(this).css("background", "#FF5500");
                //$(this).css("height", "44px"); 
                if ($(this).hasClass("disabled")) {
                    $("#loading").hide();
                      alert("订单按钮不可用");
                    return false;
                }
                var g = $("#orderNum").val();
                if (!/^[1-9]?[0-9]*$/.test(g) || g == "") {
                    showErr("\u8bf7\u9009\u62e9\u9884\u8ba2\u6570\u91cf\uff01");
                    $("#loading").hide();
                    return false;
                }
                if (g > Number($("#maxpeapalMax").val()) || g < 1) {
                    showErr("\u8bf7\u91cd\u65b0\u9009\u62e9\u9884\u8ba2\u6570\u91cf\uff01<br /><strong class='f60'>1</strong> \u5f20\u8d77\u8ba2\uff0c\u6700\u591a\u53ef\u9884\u8ba2 <strong class='f60'>" + $("#maxpeapalMax").val() + "</strong> \u5f20\u3002");
                    $("#loading").hide();
                    return false;
                }

                if (!checkPerName("getName_1")) {
                    $("#loading").hide();
                    return false;
                }
                if (!checkPerPhone("getPhone_1")) {
                    $("#loading").hide();
                    //alert("预订人电话格式不正确");
                    return false;
                }
                var f = Number($("#orderNum").val()),
		        h = true,
		        d = true;
                if ($("#isRealName").val() == "1" && f > 1) {
                    for (var c = 2; c <= f; c++) {
                        if (!checkPerName("getName_" + c)) {
                            $("#loading").hide();
                            return false;
                        }
                        if (!checkPerPhone("getPhone_" + c)) {
                            $("#loading").hide();
                            return false;
                        }
                    }
                }
//                $(this).addClass("disabled");

                var comid = $("#hid_com").val();
                var id = $("#wxid").val();
                var ordertype = $("#hid_ordertype").val();
                var payprice = $("#amount").val();
                var Integral = 0;
                var Imprest = 0;
                if ($("#hid_In").val() == 1) {
                    Integral = $("#hid_Integral").val();
                }
                if ($("#Im").val() == 1) {
                    Imprest = $("#hid_Imprest").val();
                }


               
                var name = $("#getName_1").val();
                var phone = $("#getPhone_1").val();
                var datetime = $("#datetime").val();

                var buyuid = $("#buyuid").val();
                var tocomid = $("#tocomid").val();

                 var travelnames = ""; //乘车人姓名列表
                var travelidcards = ""; //乘车人身份证列表
                var travelnations = ""; //乘车人民族列表
                 var travelphones = ""; //乘车人联系电话列表
                  var travelremarks = ""; //乘车人备注列表
           
                var errid = "0"; // 输入格式错误的控件id

                


                if ($("#hid_server_type").trimVal() == 10) {

                    if(datetime==""){
                       $("#loading").hide();
                       document.getElementById("datetime").focus();
                        alert("请选择出游日期.");
                            return false;
                    }



                    //判断乘车人信息不可为空
                    for (var i = 1; i <= parseInt(number)  ; i++) {
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
                            errid = "travelname" + i;
                            break;
                        }
                        if(travel_idcard != "")
                        {
                             if (!IdCardValidate(travel_idcard)) {
                                alert("乘车人" + travel_name + "身份证格式错误");
                                errid = "travelidcard" + i;
                                break;
                            }
                        }else{
//                         alert("乘车人" + travel_name + "身份证不可为空");
//                            errid = "travelidcard" + i;
//                            break;
                        }
                        travelnames += travel_name + ",";
                        travelidcards += travel_idcard + ",";
                        travelnations += travel_nation + ",";
                        travelphones += travel_phone + ",";
                        travelremarks += travel_remark + ",";

                          <%if (Iuse ==1){ //针对本人使用的 限制预订人与乘车人相同%>
                          if(i==1){
                             if(travel_name!=name){
                                alert("此卡限制本人使用，预订人和乘车人请填写同一人");
                                errid = "travelname1";
                                break;
                             }
                          }
                          <%} %>

 
                    }
                    if (errid != "0") {
                        $(errid).focus();
                         
                        return;
                    }
                }

                  var manyspeci = $("#hid_manyspeci").val();
                    var speciid = $("#hid_speciid").val();

                    if(manyspeci==1){
                        if(speciid==0){
                            alert("请选择具体规格");
                            
                            return;
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
                        var u_num=$("#orderNum").val();
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
                                    alert("身份证号(" + baoxian_idcard + ")格式错误，可不填写");
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

                $("#submitBtn1").val("提交中..");
                $("#submitBtn1").attr("disabled", "disabled");




                //提交预订
                $.post("/JsonFactory/OrderHandler.ashx?oper=createorder", { baoxiannames:baoxiannames,baoxianpinyinnames:baoxianpinyinnames,baoxianidcards:baoxianidcards,speciid:speciid,Integral: Integral, Imprest: Imprest, openid: $("#hid_openid").val(), proid: id, ordertype: ordertype, payprice: payprice, u_num: number, u_name: name, u_phone: phone, u_traveldate: datetime, comid: comid, buyuid: buyuid, tocomid: tocomid , travelnames: travelnames, travelidcards: travelidcards, travelnations: travelnations,travelphones: travelphones,travelremarks: travelremarks, travel_pickuppoints: $("#pointuppoint").trimVal(), travel_dropoffpoints: $("#dropoffpoint").trimVal(), order_remark: $("#order_remark").trimVal(),pno:'<%=pno %>'  }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#loading").hide();
                        alert(data.msg);
                        $('#submitBtn1').removeAttr("disabled");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.dikou != "") {
                            location.href = "backUrl.aspx?orderid=" + data.msg + "&comid=" + comid + " &dikou=" + data.dikou;
                            return;
                        }
                        <%if(pno !=""){ %>
                           alert("预约成功！");
                           location.href = "/yy/success.aspx";
                        <%}else{ %>
                            location.href = "pay.aspx?orderid=" + data.msg + "&comid=" + comid;
                        <%} %>
                        $("#loading").hide();
                        return;
                    }

                })

            })
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
        function isWeiXin() {
            var ua = window.navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == 'micromessenger' && parseFloat(navigator.appVersion) >= 5) {
                return true;
            } else {
                return false;
            }
        }
        function checkPerName(e) {
            $("#" + e).val($("#" + e).val().replace(/\s/g, ""));
            var c = true,
	b = e.replace("getName_", ""),
	a = $("#isRealName").val();
            if ($("#" + e).val() != "" && $("#" + e).val() != "\u8bf7\u586b\u5199\u53d6\u7968\u4eba\u59d3\u540d") {
                if (/^(.*?)+[\d~!@#$%^&*()_\-+\={}\[\];:'"\|,.<>?\uff01\uffe5\u2026\u2026\uff08\uff09\u2014\u2014\uff5b\uff5d\u3010\u3011\uff1b\uff1a\u2018\u201c\u2019\u201d\u3001\u300a\u300b\uff0c\u3002\u3001\uff1f]/.test($("#" + e).val())) {
                    if (a == "1") {
                        showErr("\u53d6\u7968\u4eba" + b + "\u7684\u59d3\u540d\u4e0d\u5f97\u5305\u542b\u6570\u5b57\u6216\u7279\u6b8a\u5b57\u7b26\uff01")
                    } else {
                        showErr("\u53d6\u7968\u4eba\u59d3\u540d\u4e0d\u5f97\u5305\u542b\u6570\u5b57\u6216\u7279\u6b8a\u5b57\u7b26\uff01")
                    }
                    c = false
                } else {
                    var f = "";
                    var d = Number($("#orderNum").val());
                    for (i = 1; i < d; i++) {
                        if (b != i) {
                            f += "@" + $("#getName_" + i).val() + ","
                        }
                    }
                    if (f.indexOf("@" + $("#" + e).val() + ",") > -1) {
                        showErr("\u53d6\u7968\u4eba\u59d3\u540d\u6709\u91cd\u590d\uff0c\u8bf7\u91cd\u65b0\u586b\u5199\uff01");
                        c = false
                    }
                }
            } else {
                if (a == "1") {
                    showErr("\u8bf7\u586b\u5199\u53d6\u7968\u4eba" + b + "\u7684\u59d3\u540d\uff01")
                } else {
                    showErr("\u8bf7\u586b\u5199\u53d6\u7968\u4eba\u59d3\u540d\uff01")
                }
                c = false
            }
            return c
        }
        function checkPerPhone(f) {
            var c = true,
	b = f.replace("getPhone_", ""),
	a = $("#isRealName").val();
            if ($("#" + f).val() != "" && $("#" + f).val() != "\u514d\u8d39\u63a5\u6536\u8ba2\u5355\u786e\u8ba4\u77ed\u4fe1") {
                if (!/^1\d{10}$/i.test($("#" + f).val())) {
                    if (a == "1") {
                        showErr("\u8bf7\u6b63\u786e\u586b\u5199\u53d6\u7968\u4eba" + b + "\u7684\u624b\u673a\u53f7\u7801\uff01")
                    } else {
                        showErr("\u8bf7\u6b63\u786e\u586b\u5199\u53d6\u7968\u4eba\u624b\u673a\u53f7\u7801\uff01")
                    }
                    c = false
                } else {
                    var e = "";
                    var d = Number($("#orderNum").val());
                    for (i = 1; i < d; i++) {
                        if (b != i) {
                            e += "@" + $("#getPhone_" + i).val() + ","
                        }
                    }
                    if (e.indexOf("@" + $("#" + f).val() + ",") > -1) {
                        showErr("\u53d6\u7968\u4eba\u624b\u673a\u53f7\u7801\u6709\u91cd\u590d\uff0c\u8bf7\u91cd\u65b0\u586b\u5199\uff01");
                        c = false
                    }
                }
            } else {
                if (a == "1") {
                    showErr("\u8bf7\u586b\u5199\u53d6\u7968\u4eba" + b + "\u7684\u624b\u673a\u53f7\u7801\uff01")
                } else {
                    showErr("\u8bf7\u53d6\u7968\u4eba\u624b\u673a\u53f7\u7801\uff01")
                }
                c = false
            }
            return c
        }
        function ticketsChange() {
            var f = parseInt($("#maxpeapalMin").val(), 10);
            var d = parseInt($("#maxpeapalMax").val(), 10);
            if (f >= $("#orderNum").val()) {
                $("#reduceP").addClass("enabled")
            } else {
                $("#reduceP").removeClass("enabled")
            }
            if (d <= $("#orderNum").val()) {
                $("#plusP").addClass("enabled")
            } else {
                $("#plusP").removeClass("enabled")
            }
            $("#count").html($("#orderNum").val() + "\u5f20");
            if ($("#isRealName").val() == "1") {
                var a = "",
		    e = $("#perList .w-item").length / 2,
		    c = $("#orderNum").val();
                if (c == 1) {
                    $("#perList").html("")
                } else {
                    if (e + 2 > c) {
                        if (e == 1) {
                            e = 2
                        }
                        for (var b = e * 2; b > c * 2 - 3; b--) {
                            $("#perList .w-item:eq(" + b + ")").remove()
                        }
                    } else {
                        for (var b = e + 2; b <= c; b++) {
                            a += '<div class="w-item"><dl class="in-item fn-clear"><dt>\u53d6\u7968\u4eba' + b + '</dt><dd><input type="text" id="getName_' + b + '" name="GName" placeholder="\u8bf7\u586b\u5199\u53d6\u7968\u4eba\u59d3\u540d" value=""  /></dd></dl></div><div class="w-item"><dl class="in-item fn-clear"><dt>\u624b\u673a\u53f7</dt><dd><input type="tel" id="getPhone_' + b + '" name="GMobile" maxlength="11" placeholder="\u514d\u8d39\u63a5\u6536\u8ba2\u5355\u786e\u8ba4\u77ed\u4fe1" value="" /></dd></dl></div>'
                        }
                        $("#perList").append(a)
                    }
                }
            }
        };
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
            $('<div id="showMsg"><div class="msg-title">\u6e29\u99a8\u63d0\u793a</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr()">\u77e5\u9053\u4e86</a></div></div>').appendTo("body");
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
        function hideErr() {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #showMsg").hide()
        }
    </script>
    <script type="text/javascript">
             $(function () {
 <% 
        // ispanicbuy 抢购
        // panic_begintime  开始时间
        // panicbuy_endtime 抢购结束
        // limitbuytotalnum 抢购总数
        if (ispanicbuy ==1){
        %>
             <%
                if (panic_begintime <= nowtoday && nowtoday<panicbuy_endtime){//抢购在有效期范围内
             %>
               $('#qianggou').html('<div style="font-size:18px;line-height:20px; padding-top:5px;" id="qianggoujishi">火热抢购中..</div>');
               $("#hid_dinggou").val("1");
             <%
             }else if(panic_begintime>nowtoday){         
             %>

                   $("#suborder").addClass("order_un"); 
                   
                   $("#hid_dinggou").val("0");
                  
                   $('#qianggou').html('<div style="font-size:18px;line-height: 20px; padding-top:5px;" id="qianggoujishi">距开始时间还剩  <span id="day_show"></span><span id="hour_show"></span><span id="minute_show"></span><span id="second_show"></span></div>')
                   var jishicount=$('#hid_jishicount').val();

                   var intDiff = parseInt(jishicount); //倒计时
                   timer(intDiff);
             <%
              }else{
             %>
                $('#qianggou').html('<div style="color: #666;;" id="qianggoujishi">抢购已结束</div>')

                $("#suborder").addClass("order_un"); 
                $("#hid_dinggou").val("0");
              <%} 
              %>
        <%} 
        %>

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
             $("#qianggou").html("火热抢购中..");
             $("#suborder").removeClass("order_un"); 
             $("#hid_dinggou").val("1");
         }


    </script>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- CSS -->
    <link rel="stylesheet" href="/h5/order/css/css4.css" onerror="_cdnFallback(this)" />
    <link rel="stylesheet" href="/h5/order/css/css1.css" onerror="_cdnFallback(this)" />
    <style>
    .loading
    {
        top: 40% !important;
        width: 200px !important;
        left: 40% !important;
    }
    </style>
</head>
<body class=" ">
    <!-- container -->
    <div class="container js-page-content wap-page-order">
        <div class="content confirm-container"  id="orderFome">
            <div class="app app-order">
                <div class="app-inner inner-order" id="js-page-content">
                    <!-- 通知 -->
                    <!-- 商品列表 -->
                    <div class="block-order block-border-top-none">
                        <div class="">
                            <!-- ▼顶部通栏 -->
                            <div class="js-mp-info share-mp-info">
                                <a class="page-mp-info" href="/h5/order/Default.aspx">
                                    <img class="mp-image" width="24" height="24" src="<%=logoimg%>" />
                                    <i class="mp-nickname">
                                        <%=title %>
                                    </i></a>
                                <div class="links">
                                </div>
                            </div>
                            <!-- ▲顶部通栏 -->
                        </div>
                        <hr class="margin-0 left-10">
                        <div class="block block-list block-border-top-none block-border-bottom-none">
                            <div class="block-item name-card name-card-3col clearfix" id="prolist">
                                <a href="javascript:;" class="thumb">
                                    <img class="js-view-image" src="<%=imgurl %>" alt="<%=pro_name %>">
                                </a>
                                <div class="detail">
                                    <a href="/h5/order/pro.aspx?id=<%=id %>">
                                        <h3>
                                            <%=pro_name %></h3>
                                    </a>
                                    <p class="c-gray ellipsis">
                                    </p>
                                </div>
                                <div class="right-col">
                                    <div class="price">
                                        ¥&nbsp;<span><%=price %></span></div>
                                </div>
                            </div>
                        </div>

                       <div class="order-message clearfix block " id="divnum">
                        <dl class="in-item-number fn-clear">
                        <dd>
                                <span id="reduceP" class="btn" style="padding: 0px;"></span>
                                <span><input id="orderNum" placeholder="数量.." class="ipt-numbers notice" maxlength="3" name="ticketnumber" type="tel" readonly="readonly"></span>
                                <span id="plusP" class="btn" style="padding: 0px;"></span>
                       </dd>
                       </dl>
                       </div>
 
                       <div class="order-message clearfix block " style="" id="selDate">
                         日期: <input value="" placeholder="选择出行日期..." readonly="readonly" class="js-msg-container font-size-12" id="datetime" name="datetime" placeholder="请选择出行日期" style="border:0px;width:80%;height:28px;">
                        </div>
                        <div class="order-message clearfix block " style="">
                         姓名: <input id="getName_1" name="TravelerName" placeholder="请填写预订人姓名" value="" class="js-msg-container font-size-12" type="text" style="border:0px;width:80%;height:28px;">
                        </div>
                        <div class="order-message clearfix block " style="">
                          手机: <input id="getPhone_1" name="TravelerMobile" maxlength="11" placeholder="免费接收订单确认短信" value="" class="js-msg-container font-size-12" type="tel" style="border:0px;width:80%;height:28px;">
                        </div>
                        <div class="order-message clearfix block banche " style="">
                           上车: <select id="pointuppoint" class="mi-input">
                            </select>
                        </div>
                        <div class="order-message clearfix block banche " style="">
                           下车: <select id="dropoffpoint" class="mi-input">
                            </select>
                        </div>

                        <hr class="margin-0 left-10 none" />
                        <div class="order-message clearfix none" id="js-order-message" >
                            <textarea id="message" class="js-msg-container font-size-12" placeholder="给卖家留言..."></textarea>
                        </div>

                        <!-- 名单 -->
                    <div class="block-order block-border-top-none" style="display: none; margin-top: 20px;" id="div_travelbus">
                    <dl class="in-item fn-clear">
                        <dd>
                            出行人信息:</dd>
                        <dd>
                            <table width="100%" class="grid" id="travel_tb" style="display: none;">
                                <tbody id="travel_tbody">
                                </tbody>
                            </table>
                        </dd>
                        <dd>
                            <span class="banche"></span></dd>
                    </dl>
                </div>
                <div class="block-order block-border-top-none w-item" style="display: none; margin-top: 50px;" id="div_baoxian">
                    <dl class="in-item fn-clear">
                        <dd>
                            乘车人信息</dd>
                        <dd>
                            <table width="100%" class="grid" id="baoxian_tb"  >
                                <tbody id="baoxian_tbody">
                                </tbody>
                            </table>
                        </dd>  
                    </dl>
                </div> 


                        <div class="order-message clearfix block">
                            商品总价：</div>
                    </div>


                    


                    <!-- 支付 -->
                    <div class="js-step-topay ">
                        <div class="js-used-coupon block pg_upgrade_title" style="display: none;">
                        <div class="action-container" >
                          <% if (Integral > 0)
                           { %>
                        <i class="qb_icon icon_checkbox" id="Integral"></i> 积分余额<%=Integral%>
                        <%} %>
                        <% if (Imprest > 0)
                           { %>
                        <i class="qb_icon icon_checkbox" id="Imprest"></i> 预付款余额<%=Imprest%>
                        <%} %>
                        </div>
                        </div>
                        <div class="">
                            <div class="js-order-total block-item order-total">
                                <strong class="js-real-pay c-orange" id="total_price">合计： <span id="priceTotal">¥&nbsp;</span>
                                </strong>
                            </div>
                        </div>
                        <div class="action-container" id="Div1">
                           
                            <div style="margin-bottom: 10px;">
                                    <button id="submitBtn1" type="button" data-pay-type="baiduwap" class="btn-pay btn btn-block btn-large btn-baiduwap  btn-green">
                                    立即预订</button>
                            </div>
                           
                        </div>

                        <div id="resvalues">
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div id="js-self-fetch-modal" class="modal order-modal">
        </div>

        <div class="footer">
            <!-- 商家公众微信号 -->
            <div class="copyright">
                <div class="ft-copyright">
                   服务热线：<a href="tel:<%=phone %>"><%=phone %></a>
                </div>
            </div>
        </div>
    </div>
    <div id="loading" class="loading" style="display: none; z-index:10000;">
        正在加载...
    </div>
    <script src="../Scripts/mCal.js" type="text/javascript"></script>
    <link href="../Scripts/mCal.css" rel="stylesheet" type="text/css" />
    <!--Baidu-->
    <script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "//hm.baidu.com/hm.js?8fcd06cc927f3554397ca18509561b69";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>
    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "",
                desc: "<%=pro_explain%>",
                title: '<%=title%>'
            });
        });
    </script>
    <div id="calDiv" style="display: none; margin-top: -40px">
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

     <input type="hidden" id="hid_com" name="hid_com" value="<%=comid %>" />
    <input type="hidden" id="wxid" value="<%=id %>" />
    <input type="hidden" id="nowdate" value="<%=nowdate %>" />
    <input type="hidden" id="amount" name="amount" value="<%=price %>">
    <input type="hidden" id="" value="<%=ordertype %>" />
    <input type="hidden" id="maxpeapalMax" name="maxpeapalMax" value="<%=pro_max %>" />
    <input type="hidden" id="maxpeapalMin" name="maxpeapalMin" value="<%=pro_min %>" />
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_Integral" value="<%=Integral %>" />
    <input type="hidden" id="hid_Imprest" value="<%=Imprest %>" />
    <input type="hidden" id="hid_In" value="0" />
    <input type="hidden" id="Im" value="0" />
    <input type="hidden" id="buyuid" value="<%=buyuid %>" />
    <input type="hidden" id="tocomid" value="<%=tocomid %>" />
    <input type="hidden" id="hid_pno" value="<%=pno %>" />

    <input type="hidden" id="hidLeavingDate" value="" />
    <input type="hidden" id="hidMinLeavingDate" value="" />
    <input type="hidden" id="hidLinePrice" value="" />
    <input id="hidEmptyNum" type="hidden" value="" />
    <input type="hidden" id="hidchildreduce" value="<%=childreduce %>" />
    <input type="hidden" id="hid_dinggou" value="1" />
    <input type="hidden" id="hid_jishicount" value="<%=shijiacha %>" />
    <input type="hidden" id="hid_server_type" value="<%=pro_servertype %>" />
    <!--上车地点，下车地点-->
    <input id="hid_pickuppoint" type="hidden" value="<%=pickuppoint %>" />
    <input id="hid_dropoffpoint" type="hidden" value="<%=dropoffpoint %>" />

    <input id="hid_pro_num" type="hidden" value="<%=pro_num %>" />
    
    <!--多规格-->
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
                         //装载产品规格
         <%if (manyspeci==1){ %>
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Selectguigelist",
                    data: { comid:  $("#hid_com").val(),proid: <%=id %>,agentid: 0 },
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

                $("#sPrice").html(_price);
                $("#amount").val(_price); 
                $("#priceTotal").html('￥<b style=" font-size:18px;"> ' + fmoney(_price * $("#orderNum").val(), 2) + '</b>');

            }
        })

    </script>

</body>
</html>