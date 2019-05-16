<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ETS2.WebApp.UI.Frame.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=proname %></title>
    <!-- meta信息，可维护 -->
    <%-- <meta charset="UTF-8" />--%>
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta content="telephone=no" name="format-detection" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <!-- 页面样式表 -->
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-sohu.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/Styles/common.css" />
    <script type="text/javascript">
        $(function () {


            //加载日历
            $(document).ready(function () {


                $("#datepicker").datepicker({
                    minDate: $("#hidMinLeavingDate").val(),
                    onSelect: function (dateText) {
                        //$(".tip-yellowsimple").remove();

                        $("#u_traveldate").val(dateText);

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

                        var dayprice = '';

                        var result = false;
                        $(data).each(function (i, n) {
                            if (n == dt) {
                                result = true;
                                dayprice = price[i];
                            }
                        });

                        if (result) {
                            return [true, "hasCourse", "￥" + dayprice + "起"];

                        } else {
                            return [false, "noCourse", '无团期'];
                        }
                    }
                });
            });


            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#btn-submit").click();    //这里添加要处理的逻辑  
                }
            });

            //日历
            var nowdate = '<%=today %>';
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                //$(this).val(nowdate);
                $($(this)).datepicker();
            });

            $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })


            $("#btn-submit").click(function () {
                var name = $.trim($("#Name").val());
                var num = $.trim($("#Num").val());
                var phone = $.trim($("#Phone").val());
                var getcode = $.trim($("#Getcode").val());
                var proid = $.trim($("#hid_proid").val());
                var u_traveldate = $.trim($("#u_traveldate").val());
                var ordertype = $.trim($("#hid_ordertype").val());
                $("#loading").show();

                if (u_traveldate == "" || u_traveldate == "请左侧日历选择出行日期") {
                    $("#error-box").html("请左侧日历选择出行日期");
                    $("#error-box").show();
                    $("#loading").hide();
                    return;
                }

                if (num == '' || num == "请填写出行人数") {
                    $("#error-box").html("请填写出行人数");
                    $("#error-box").show();
                    $("#loading").hide();
                    return;
                } else {
                    var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                    if (!patrn.exec(num)) {
                        $("#error-box").html("请正确填写出行人数");
                        $("#error-box").show();
                        $("#loading").hide();
                        return;
                    }
                }

                if (name == "" || name == "请填写姓名") {
                    $("#error-box").html("请填写联系人姓名");
                    $("#error-box").show();
                    $("#loading").hide();
                    return;
                }
                if (phone == '' || phone == "联系人手机") {
                    $("#error-box").html("请填写联系人手机");
                    $("#error-box").show();
                    $("#loading").hide();
                    return;
                }

                if (isMobel(phone)) {
                } else {
                    $("#error-box").html("填写的手机号有误，此手机为接收抵扣券短信");
                    $("#error-box").show();
                    $("#loading").hide();
                    return;
                }


                if (getcode == '' || getcode == "验证码") {
                    $("#error-box").html("请填写验证码");
                    $("#error-box").show();
                    $("#loading").hide();
                    return;
                }


                $.post("/JsonFactory/OrderHandler.ashx?oper=sohuorder", { proid: proid, ordertype: ordertype, u_num: num, u_name: name, u_phone: phone, u_traveldate: u_traveldate, getcode: getcode }, function (data) {
                    data = eval("(" + data + ")");
                    if (parseInt(data.type) == 1) {
                        $("#error-box").html(data.msg);
                        $("#error-box").show();
                        $("#loading").hide();


                    }
                    if (parseInt(data.type) == 100) {
                        $("#loading").hide();
                        //alert("订单提交成功，优惠券将发送到您提交的手机上！");
                        Operatingstar();
                        $("#validateCode").src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
                        $("#Getcode").val("");
                        return;
                    }
                })
                function callbackfunc(e, v, m, f) {
                    if (v == true)
                        location.reload();
                }
            })

            $("#cancel").click(function () {
                $("#Operating").hide();
                location.reload();
            })
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

        function Operatingstar() {

            $("#Operating").show();
            $("#OperatingTitle").html("订单提交成功，抵用券将发送到您提交的手机上");
        }


        //验证手机号
        function isMobel(value) {
            if (/^13\d{9}$/g.test(value) || /^15\d{9}$/g.test(value) || /^14\d{9}$/g.test(value) ||
	/^18\d{9}$/g.test(value)) {
                return true;
            } else {
                return false;
            }
        }  
 


    </script>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link type="text/css" rel="stylesheet" href="http://css.sohu.com/upload/global1.3.css" />
    <style type="text/css" >
        .pickerselected a.ui-state-default
        {
            background: #3BAAE3;
            border: 1px solid #74B2E2;
        }
        .pickerspanTime a.ui-state-default
        {
            background: #E4F1FB;
            color: #0074A3;
        }
        .ui-datepicker td span, .ui-datepicker td a
        {
            display: block;
            text-align: center;
            text-decoration: none;
            color: #333333;
        }
        
        
        body
        {
            font-family: '\5FAE\8F6F\96C5\9ED1';
            color: #333;
        }
        #channelNav
        {
            margin: 0 auto;
        }
        #foot
        {
            background-color: #00cc99;
            width: 100%;
            border: 0;
            color: #FFF;
            height: 36px;
            line-height: 36px;
            padding: 0;
            margin: 0;
            margin-top: 40px;
        }
        #foot a
        {
            color: #FFF;
        }
        .header
        {
            overflow: hidden;
            height: 438px;
            position: relative;
            text-align: left;
            color: #FFF;
        }
        .header .wenzi
        {
            position: absolute;
            top: 50px;
            right: 0;
            width: 396px;
            height: 255px;
            background: url(http://i0.itc.cn/20140220/121_a411069d_8f08_effb_3753_3593362aaa23_1.png) 50% 0 no-repeat;
            padding: 45px 32px 0;
        }
        .header .wenzi h1
        {
            font-size: 36px;
            line-height: 44px;
            color: #ffffcc;
        }
        .header .wenzi h3
        {
            font-size: 24px;
            line-height: 56px;
            padding-bottom: 10px;
        }
        .header .wenzi h3 span
        {
            text-decoration: line-through;
        }
        .header .wenzi h4
        {
            font-size: 14px;
            line-height: 24px;
        }
        .header .wenzi .t1
        {
            font-weight: 700;
            background: url(http://i2.itc.cn/20140220/121_8a4f2998_c088_4ff2_90a8_740a4939b36b_1.png) 0 50% no-repeat;
            padding-left: 20px;
        }
        .header .wenzi .t2
        {
            background: url(http://i2.itc.cn/20140220/121_8a4f2998_c088_4ff2_90a8_740a4939b36b_2.png) 0 5px no-repeat;
            padding-left: 20px;
        }
        
        .fcue_lvyouluxian_01
        {
            padding: 28px 0;
        }
        .fcue_lvyouluxian_01 .left
        {
            width: 561px;
            overflow: hidden;
        }
        .fcue_lvyouluxian_01 .left .examples_body
        {
            clear: both;
            width: 561px;
            margin: 0px auto;
            height: 630px;
            overflow: hidden;
        }
        .fcue_lvyouluxian_01 .left .examples_body .bx_wrap
        {
            width: 561px;
            margin: 0 auto;
            position: relative;
            overflow: hidden;
        }
        .fcue_lvyouluxian_01 .left a.prev
        {
            width: 36px;
            height: 23px;
            position: absolute;
            top: 0;
            right: 170px;
            text-indent: -9999px;
            background: url(http://i2.itc.cn/20140221/121_7802de8c_0f6d_1654_5dba_1b5a7879c345_2.jpg) no-repeat;
            z-index: 300;
            _filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='http://i2.itc.cn/20140221/121_7802de8c_0f6d_1654_5dba_1b5a7879c345_2.jpg',sizingMethod='scale');
            _background: none;
        }
        .fcue_lvyouluxian_01 .left a.prev:hover
        {
            width: 36px;
            height: 23px;
            position: absolute;
            top: 0;
            right: 170px;
            text-indent: -9999px;
            background: url(http://i1.itc.cn/20140221/121_7802de8c_0f6d_1654_5dba_1b5a7879c345_1.jpg) no-repeat;
            z-index: 300;
            _filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='http://i1.itc.cn/20140221/121_7802de8c_0f6d_1654_5dba_1b5a7879c345_1.jpg',sizingMethod='scale');
            _background: none;
        }
        .fcue_lvyouluxian_01 .left a.next
        {
            width: 36px;
            height: 23px;
            position: absolute;
            top: 0;
            left: 170px;
            text-indent: -9999px;
            background: url(http://i2.itc.cn/20140221/121_7802de8c_0f6d_1654_5dba_1b5a7879c345_4.jpg) no-repeat;
            z-index: 300;
            _filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='http://i2.itc.cn/20140221/121_7802de8c_0f6d_1654_5dba_1b5a7879c345_4.jpg',sizingMethod='scale');
            _background: none;
        }
        .fcue_lvyouluxian_01 .left a.next:hover
        {
            width: 36px;
            height: 23px;
            position: absolute;
            top: 0;
            left: 170px;
            text-indent: -9999px;
            background: url(http://i0.itc.cn/20140221/121_7802de8c_0f6d_1654_5dba_1b5a7879c345_3.jpg) no-repeat;
            z-index: 300;
            _filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='http://i0.itc.cn/20140221/121_7802de8c_0f6d_1654_5dba_1b5a7879c345_3.jpg',sizingMethod='scale');
            _background: none;
        }
        .fcue_lvyouluxian_01 .left a:active
        {
            outline: none;
        }
        .fcue_lvyouluxian_01 .left p
        {
            font-size: 16px;
            line-height: 20px;
            font-weight: 700;
            margin-bottom: 18px;
        }
        .fcue_lvyouluxian_01 .right
        {
            width: 320px;
        }
        .fcue_lvyouluxian_01 .right .tit
        {
            margin-bottom: 40px;
        }
        .fcue_lvyouluxian_01 .right .but
        {
            margin: 40px auto 10px auto;
            width: 320px;
        }
        .fcue_lvyouluxian_01 .right .but img
        {
            margin: 0 auto;
            cursor: pointer;
        }
        
        .fcue_lvyouluxian_02
        {
            margin-bottom: 35px;
        }
        .fcue_lvyouluxian_02 .block01
        {
            overflow: hidden;
        }
        .fcue_lvyouluxian_02 .block01 ul
        {
            width: 950px;
        }
        .fcue_lvyouluxian_02 .block01 .htbar
        {
            background: url(http://i2.itc.cn/20140220/121_2d83e41b_bbb1_e5d2_46e1_dbd1ceab5365_1.gif) 0 100% repeat-x;
            width: 950px;
            overflow: hidden;
        }
        .fcue_lvyouluxian_02 .block01 .htbar li
        {
            width: 111px;
            height: 42px;
            float: left;
            line-height: 42px;
            background: url(http://i1.itc.cn/20140220/121_b700eb45_052b_0cc3_7909_1e2bb68ee24a_1.gif) 100% 50% no-repeat;
            cursor: pointer;
            font-size: 14px;
        }
        .fcue_lvyouluxian_02 .block01 .htbar li a
        {
            color: 000;
            display: block;
            text-decoration: none;
            width: 111px;
        }
        .fcue_lvyouluxian_02 .block01 .htbar li a:hover
        {
            background: url(http://i3.itc.cn/20140220/121_f8754b0d_243a_37bf_cb30_a20c50d805ee_1.png) 0 100% repeat-x;
            display: block;
            width: 111px;
            color: #000;
        }
        .fcue_lvyouluxian_02 .block01 .htbar .sa
        {
            background: url(http://i2.itc.cn/20140220/121_45afdc16_c126_9636_c9e0_82d892924528_1.png) 50% 0 no-repeat;
            width: 111px;
            height: 42px;
        }
        .fcue_lvyouluxian_02 .block01 .htbar .sa a
        {
            color: #FFF;
        }
        .fcue_lvyouluxian_02 .block01 .htbar .sa a:hover
        {
            color: #FFF;
        }
        .fcue_lvyouluxian_02 .block01 .htbar .none
        {
            background: none;
        }
        .fcue_lvyouluxian_02 .block01 .htbar .r
        {
            float: right;
            background: url(http://i1.itc.cn/20140220/121_0d217f77_8890_1d22_427d_9bdaddbb9ba1_1.jpg) 100% 50% no-repeat;
            padding-right: 15px;
            width: 130px;
        }
        .fcue_lvyouluxian_02 .block01 .htbar .r a
        {
            width: 130px;
        }
        .fcue_lvyouluxian_02 .block01 .htbar .r a:hover
        {
            background: none;
            color: #00cc99;
            width: 130px;
        }
        
        .fcue_lvyouluxian_02 .block01 .flist01 .pt01
        {
            width: 920px;
            overflow: hidden;
            text-align: left;
            margin: 25px auto;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt01 h1
        {
            font-size: 16px;
            line-height: 26px;
            color: #000;
            margin: 10px auto 0;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt01 h1 span
        {
            color: #00cc99;
            font-weight: 700;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt01 h1 a
        {
            color: #00cc99;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt01 h3
        {
            color: #666;
            line-height: 30px;
            text-indent: 48px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .blockA
        {
            width: 920px;
            overflow: hidden;
            text-align: left;
            margin: 0 auto;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 h3
        {
            font-size: 16px;
            line-height: 42px;
            color: #000;
            margin: 10px 0 0;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 h3 span
        {
            color: #00cc99;
            font-weight: 700;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02
        {
            overflow: hidden;
            padding: 15px 0 10px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .l
        {
            width: 51px;
            font-size: 16px;
            line-height: 26px;
            color: #00cc99;
            background: url(http://i1.itc.cn/20140221/121_b21fd67a_c681_8909_0e28_9145ad513b35_1.jpg) 100% 50% no-repeat;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .f1
        {
            margin: 0;
            line-height: 18px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .r
        {
            width: 840px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .r p
        {
            font-size: 16px;
            line-height: 26px;
            color: #000;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .r span
        {
            color: #666;
            line-height: 24px;
            text-indent: 48px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .r p a
        {
            color: #00cc99;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .r dl
        {
            overflow: hidden;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .r dd
        {
            width: 250px;
            float: left;
            margin-right: 45px;
            font-size: 14px;
            color: #00cc99;
            line-height: 22px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .r dd img
        {
            margin-bottom: 5px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .pt02 .r .none
        {
            margin-right: 0;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .lineA
        {
            width: 170px;
            background: url(http://i2.itc.cn/20140221/121_25cc4748_e28c_5792_06e7_fa5fbc71ed6c_1.jpg) 0 100% repeat-x;
            overflow: hidden;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .lineB
        {
            width: 100%;
            background: url(http://i3.itc.cn/20140221/121_f0344076_0a85_8e36_5b18_53cd38000176_1.jpg) repeat-x;
            height: 1px;
            overflow: hidden;
            margin: 60px auto;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .bg
        {
            background: url(http://i2.itc.cn/20140221/121_25cc4748_e28c_5792_06e7_fa5fbc71ed6c_1.jpg) 45px 42px repeat-y;
            height: 100%;
            width: 920px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .b1
        {
            padding-bottom: 0;
        }
        
        .fcue_lvyouluxian_02 .block01 .flist01 ul
        {
            width: 920px;
            overflow: hidden;
            margin: 15px auto;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 li
        {
            width: 180px;
            float: left;
            margin: 0 5px 20px 0;
            display: inline;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 li p
        {
            font-size: 14px;
            line-height: 18px;
            text-align: left;
            padding: 0 10px 5px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 li img
        {
            margin: 5px 5px 10px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 li a
        {
            display: block;
            width: 180px;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 li a:hover
        {
            background-color: #2db5b5;
            color: #FFF;
            text-decoration: none;
        }
        .fcue_lvyouluxian_02 .block01 .flist01 .none
        {
            margin-right: 0;
        }
        
        .fcue_lvyouluxian_03 .block
        {
            overflow: hidden;
            margin-top: 25px;
        }
        .fcue_lvyouluxian_03 .block .l
        {
            width: 630px;
        }
        .fcue_lvyouluxian_03 .block .r
        {
            width: 290px;
        }
        .fcue_lvyouluxian_03 .block .l .box
        {
            overflow: hidden;
            padding-top: 20px;
            height: 100px;
        }
        .fcue_lvyouluxian_03 .block .l .box .left
        {
            width: 135px;
        }
        .fcue_lvyouluxian_03 .block .l .box .center
        {
            width: 300px;
            margin-left: 30px;
            display: inline;
            text-align: left;
        }
        .fcue_lvyouluxian_03 .block .l .box .center h1
        {
            font-size: 16px;
            line-height: 26px;
            text-align: center;
            padding-bottom: 5px;
        }
        .fcue_lvyouluxian_03 .block .l .box .center p
        {
            line-height: 22px;
        }
        .fcue_lvyouluxian_03 .block .l .box .center p a
        {
            color: #00cc99;
        }
        .fcue_lvyouluxian_03 .block .l .box .right
        {
            width: 125px;
            text-align: right;
        }
        .fcue_lvyouluxian_03 .block .l .box .right h3
        {
            font-size: 14px;
            line-height: 26px;
            color: #ff6600;
        }
        .fcue_lvyouluxian_03 .block .l .box .right h3 span
        {
            font-size: 26px;
            font-weight: 700;
        }
        .fcue_lvyouluxian_03 .block .l .box .right h4
        {
            line-height: 20px;
            color: #00cc99;
        }
        .fcue_lvyouluxian_03 .block .l .box .right h5
        {
            line-height: 18px;
            color: #999;
        }
        
        .art_bar
        {
            padding: 10px 10px 0;
            position: fixed;
            bottom: 100px;
            left: 50%;
            margin-left: 475px;
            _height: 100px;
            _position: absolute;
            _top: expression(eval(document.documentElement.scrollTop-90+document.documentElement.clientHeight-this.offsetHeight-(parseInt(this.currentStyle.marginTop,10)||0)-(parseInt(this.currentStyle.marginBottom,10)||0)));
            width: 36px;
        }
        .art_bar li
        {
            width: 36px;
            height: 36px;
            margin-bottom: 1px;
            text-align: left;
            float: left;
        }
        .art_bar li .ab3, .art_bar li .ab4, .art_bar li .ab5
        {
            display: block;
            width: 36px;
            height: 36px;
        }
        .art_bar li .ab3
        {
            position: relative;
            text-indent: -9999px;
            background: url(http://i2.itc.cn/20140221/121_1a27c55b_8a7a_9ab6_50d0_769f63ccc13c_6.jpg) no-repeat;
        }
        .art_bar li .ab3:hover, .art_bar .on .ab3:hover
        {
            background: url(http://i2.itc.cn/20140221/121_1a27c55b_8a7a_9ab6_50d0_769f63ccc13c_3.jpg) no-repeat;
        }
        .art_bar li .ab4
        {
            text-indent: -9999px;
            background: url(http://i2.itc.cn/20140221/121_1a27c55b_8a7a_9ab6_50d0_769f63ccc13c_5.jpg) no-repeat;
        }
        .art_bar li .ab4:hover, .art_bar .on .ab4:hover
        {
            background: url(http://i1.itc.cn/20140221/121_1a27c55b_8a7a_9ab6_50d0_769f63ccc13c_2.jpg) no-repeat;
        }
        .art_bar li .ab5
        {
            text-indent: -9999px;
            background: url(http://i1.itc.cn/20140221/121_1a27c55b_8a7a_9ab6_50d0_769f63ccc13c_4.jpg) no-repeat;
        }
        .art_bar li .ab5:hover, .art_bar .on .ab5:hover
        {
            background: url(http://i3.itc.cn/20140221/121_1a27c55b_8a7a_9ab6_50d0_769f63ccc13c_1.jpg) no-repeat;
        }
        
        .ab4wrap-con
        {
            position: absolute;
            top: 47px;
            left: 46px;
            width: 300px;
            line-height: 28px;
            height: 30px;
            z-index: 1000;
        }
        .ab4wrap-con li
        {
            width: 26px;
            height: 30px;
            background: none;
            cursor: pointer;
            float: left;
        }
        .ab4wrap-con li:hover
        {
            text-decoration: none;
        }
        
        .input-style
        {
            height: 38px;
            border: #B8B8B8 solid 1px;
            color: #333;
            font-size: 16px;
            font-family: Microsoft YaHei;
            padding-left: 16px;
            margin-bottom: 20px;
            line-height: 180%;
        }
        .input-text
        {
            width: 300px;
        }
        
        .input-yanzheng
        {
            width: 200px;
            margin-left: 0;
            float: left;
        }
        
        .input-yanzheng
        {
            width: 200px;
            margin-left: 0;
            float: left;
        }
        
        .ValiCode
        {
            margin-top: 8px;
            width: 87px;
            height: 24px;
        }
        #error-box
        {
            color: #FF0000;
            font-size: 14px;
            display: none;
        }       #loading
        {
            position: absolute;
            left: 50%;
            top: 60px;
            z-index: 99;
        }
        #loading, #loading .lbk, #loading .lcont
        {
            width: 146px;
            height: 146px;
        }
        #loading .lbk, #loading .lcont
        {
            position: relative;
        }
        #loading .lbk
        {
            background-color: #000;
            opacity: .5;
            border-radius: 10px;
            margin: -73px 0 0 -73px;
            z-index: 0;
        }
        #loading .lcont
        {
            margin: -146px 0 0 -73px;
            text-align: center;
            color: #f5f5f5;
            font-size: 14px;
            line-height: 35px;
            z-index: 1;
        }
        #loading img
        {
            width: 35px;
            height: 35px;
            margin: 30px auto;
            display: block;
        }
        #OperatingName p
        {
             padding:0 10px 2px 10px;
            }
            
         .hasCourse
         {
            cursor:pointer;
          }
    </style>
</head>
<body>
    <div class="fcue_lvyouluxian_01 area">
        <div class="left">
            <div class="examples_body">
                <div type="text" id="datepicker">
                </div>
            </div>
        </div>
        <div class="right">
            <img src="baomingtitle.jpg"
                width="320" height="36" class="tit">
            <div class="list">
                <span id="error-box"></span>
                <input name="Name" type="text" class="input-text input-style" id="u_traveldate" onfocus="if (value =='请左侧日历选择出行日期'){value =''}"
                    onblur="if (value ==''){value='请左侧日历选择出行日期'}" value="请左侧日历选择出行日期" placeholder="      出行日期"  autocomplete="off" disableautocomplete=""
                    readonly />
                <input name="Num" type="text" class="input-text input-style" id="Num" onfocus="if (value =='请填写出行人数'){value =''}"
                    onblur="if (value ==''){value='请填写出行人数'}" value="请填写出行人数" placeholder="      人" autocomplete="off"
                    disableautocomplete="" />
                <input name="Name" type="text" class="input-text input-style" id="Name" onfocus="if (value =='请填写姓名'){value =''}"
                    onblur="if (value ==''){value='请填写姓名'}" value="请填写姓名"  placeholder="      姓名" autocomplete="off" disableautocomplete="" />
                
                <input name="Phone" type="text" class="input-text input-style" id="Phone" onfocus="if (value =='联系人手机'){value =''}"
                    onblur="if (value ==''){value='联系人手机'}" value="联系人手机" placeholder="      手机"  autocomplete="off" disableautocomplete="" />
                <input name="Code" type="text" class="input-style input-yanzheng" id="Getcode" onfocus="if (value =='验证码'){value =''}"
                    onblur="if (value ==''){value='验证码'}" value="验证码" placeholder="      验证码"  autocomplete="off" disableautocomplete="" />
                <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="验证码"
                    class="ValiCode" title="点击图片刷新验证码">
            </div>
            <div class="but">
                <img src="sub.jpg"
                    width="180" height="50" id="btn-submit"></div>
            <img src="weilvxingcode.jpg"
                width="320" height="140" class="erweima">
        </div>
    </div>

        <div id="loading" style="top: 150px; display: none;">
            <div class="lbk">
            </div>
            <div class="lcont">
                <img src="/Images/loading.gif" alt="loading..." />提交中...</div>
        </div>

        <div id="shiyongshuoming" style="top: 150px; display: ;">
            <div class="lbk">
            </div>
            <div class="lcont">
            </div>
        </div>

        <div id="Operating" style="background-color:#BFFFEF;border:1px solid #005E46;  margin:0px auto;width:520px; height:340px;display:none;left:20%; position:absolute; top:25%;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td height="40"  bgcolor="#00cc99" ><span style="font-size:14px;" id="OperatingTitle"></span></td>
                  </tr>
                  <tr>
                    <td  bgcolor="#BFFFEF" align="left"><span style=" padding:10px;font-size:14px;" id="OperatingName"><%= pro_Remark %></span></td>
                  </tr>
                  <tr>
                    <td height="38"  align="center"  class="tdHead">
					<input name="cancel" id="cancel" type="button" class="formButton" value="  确  定  " /></td>
                  </tr>
                </table>
        </div>	
    <input type="hidden" id="hid_ordertype" value="1" />
    <input type="hidden" id="hid_proid" value="<%=lineid%>" />
    <input type="hidden" id="hid_proname" value="<%=proname%>" />
    <input type="hidden" id="hidLeavingDate" value="" runat="server" />
    <input type="hidden" id="hidMinLeavingDate" value="" runat="server" />
    <input type="hidden" id="hidLinePrice" value="" runat="server" />
</body>
</html>
