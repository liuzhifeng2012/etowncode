

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head><title>

</title><meta content="text/html;charset=utf-8" http-equiv="Content-Type" /></meta>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;" name="viewport" /></meta>
    <meta content="telephone=no" name="format-detection" /></meta>
    <meta id="viewport" content="width=device-width, user-scalable=yes,initial-scale=0.2" name="viewport" /><link href="/Styles/weixin/wei_yuyue.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-rili.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })
            $("#validateCodetext").click(function () {
                $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })


            //查询按钮
            $("#selBtn").click(function () {
                var pno = $("#txtpno").trimVal();
                var getcode = $.trim($("#getcode").val());
                if (pno == "") {
                    $("#PnoVer").text("请输入电子码!");
                    return;
                }
                if (getcode == '') {
                    $("#PnoVer").text("验证码不可为空!");
                    return;
                }

                $.post("/JsonFactory/ProductHandler.ashx?oper=getprobypno", { pno: pno, getcode: getcode }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                        $("#PnoVer").text(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#hid_pno").val(pno);
                        $("#hid_orderid").val(data.msg[0].OrderId);
                        $("#hid_comid").val(data.msg[0].Com_id);
                        $("#hid_proid").val(data.msg[0].Id);
                        $("#hid_proname").val(data.msg[0].Pro_name);

                        $("#view_proname").text(data.msg[0].Pro_name);
                        $("#view_canbooknum").text("可预约人数:" + data.msg[0].Canbooknum + "人");
                        if (data.msg[0].Canbooknum == 0) {
                            $("#PnoVer").text("当前电子码的可预约人数为0，不可预约");
                        } else {
                            //                            var dstr="";//可预约人数下拉框
                            //                            for(var i=parseInt(data.msg[0].Canbooknum);i>0;i--)
                            //                            {
                            //                               dstr+='<option value="'+i+'">'+i+'人</option>';
                            //                            }
                            //                            $("#selbooknum").html(dstr);
                            $("#bespeaknum").val(data.msg[0].Canbooknum);

                            $("#calDiv").hide();
                            $("#seldiv").hide();
                            $("#viewdiv").show();
                            $("#viewresultdiv").hide();

                        }
                    }
                })
            })
            //提交预约按钮
            $("#subBtn").click(function () {
                var bespeakname = $("#bespeakname").trimVal();
                var phone = $("#phone").trimVal();
                var idcard = $("#idcard").trimVal();
                var bespeakdate = $("#bespeakdate").trimVal();
                var bespeaknum = $("#bespeaknum").trimVal();
                var remark = $("#remark").trimVal();
                var orderid = $("#hid_orderid").trimVal();
                var pno = $("#hid_pno").trimVal();

                var comid = $("#hid_comid").trimVal();
                var proid = $("#hid_proid").trimVal();
                var proname = $("#hid_proname").trimVal();
                if (bespeakname == "") {
                    $("#Span1").text("请填写预约人姓名");
                    return;
                }

                if (phone == "") {
                    $("#Span1").text("请正确填写手机号");
                    return;
                } else {
                    if (!isMobel(phone)) {
                        $("#Span1").text("请正确填写手机号");
                        return;
                    }

                }
                if (idcard == "") {
                    $("#Span1").text("身份证号不可为空");
                    return;
                } else {
                    if (!IdCardValidate(idcard)) {
                        $("#Span1").text("身份证格式错误");
                        return;
                    }
                }


                $.post("/JsonFactory/ProductHandler.ashx?oper=subautobespeak", { bespeakname: bespeakname, phone: phone, idcard: idcard, bespeakdate: bespeakdate, bespeaknum: bespeaknum, remark: remark, orderid: orderid, pno: pno, comid: comid, proid: proid, proname: proname }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#Span1").text(data.msg);
                        //                        window.location.reload();
                        return;
                    }
                    if (data.type == 100) {

                        $("#seldiv").hide();
                        $("#viewdiv").hide();
                        $("#viewresultdiv").show();
                        $("#calDiv").hide();
                    }
                })

            })

            //日历可预约日期列表
            $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=Getbespeakdatelist",
                data: {},
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        //団期处理
                        var datestr = "";
                        var datestr_zhehang = "<br>";
                        var firstdate = "";
                        var firstdate_price = "0";
                        var listdate = "";
                        var listprice = "";
                        if (data.linedate != null) {
                            for (var i = 0; i < data.linedate.length; i++) {
                                if (((i + 1) % 4) == 0) {
                                    datestr_zhehang = "<br>";
                                } else {
                                    datestr_zhehang = "";
                                }
                                datestr += data.linedate[i].Daydate + "," + datestr_zhehang;

                                if (i == 0) {
                                    firstdate = data.linedate[i].Daydate;
                                    firstdate_price = data.linedate[i].Menprice;
                                }

                                listdate += data.linedate[i].Daydate + ",";

                                listprice += data.linedate[i].Menprice + ","

                            }
                            $("#hidLeavingDate").val(listdate); //日期列表
                            $("#hidMinLeavingDate").val(firstdate); //第一天
                            $("#hidLinePrice").val(listprice); //价格列表

                            $("#bespeakdate").val(firstdate);
                        }
                    }
                }
            })

            //日历
            $("#calDiv").datepicker({
                minDate: $("#hidMinLeavingDate").val(),
                onSelect: function (dateText) {
                    $("#bespeakdate").val(dateText);
                    var datearr = $("#hidLeavingDate").val().split(",");
                    var pricearr = $("#hidLinePrice").val().split(",");
                    for (var i = 0; i < datearr.length; i++) {
                        if (datearr[i] == dateText) {
                            //                                $("#sPrice").html(pricearr[i]);
                            $("#" + dateText).addClass("selected"); //对选中的增加
                        } else {
                            ////$("#" + datearr[i]).removeClass("selected"); //对未选中的日期移除
                        }
                    }

                    $("#calDiv").hide();
                    $("#seldiv").hide();
                    $("#viewdiv").show();
                    $("#viewresultdiv").hide();
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
                        return [true, "hasCourse", dayprice];

                    } else {
                        return [false, "noCourse", '不可预约'];
                    }
                }
            });

            //日历
            $("#bespeakdate").click(function () {
                scrollTo(0, 1);


                $("#calDiv").fadeIn();
                $("#seldiv").hide();
                $("#viewdiv").hide();
                $("#viewresultdiv").hide();
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
    <style type="text/css">
        .mi-input
        {
            border-width: 1px;
            border-style: solid;
            border-color: #A6A6A6 #CCC #CCC;
            -moz-border-top-colors: none;
            -moz-border-right-colors: none;
            -moz-border-bottom-colors: none;
            -moz-border-left-colors: none;
            border-image: none;
            color: #4D4D4D;
            font: 14px tahoma,arial,Hiragino Sans GB,宋体;
            padding: 6px 4px;
            vertical-align: top;
            width: 180px;
            height: 38px;
        }
        .bustitle
        {
            width: 160px;
            float: left;
            display: block;
        }
        abbr, article, aside, audio, canvas, datalist, details, dialog, eventsource, figure, figcaption, footer, header, hgroup, mark, menu, meter, nav, output, progress, section, small, time, video, legend
        {
            display: block;
        }
        .footFix
        {
            width: 100%;
            text-align: center;
            position: fixed;
            left: 0;
            bottom: 0;
            z-index: 99;
        }
        #footReturn, #footReturn2
        {
            z-index: 89;
            display: inline-block;
            text-align: center;
            text-decoration: none;
            vertical-align: middle;
            cursor: pointer;
            width: 100%;
            outline: 0 none;
            overflow: visible;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
            padding: 0;
            height: 80px;
            opacity: .95;
            border-top: 1px solid #181818;
            box-shadow: inset 0 1px 2px #b6b6b6;
            background-color: #515151;
            background-image: -webkit-gradient(linear,0% 0,0% 100%,from(#838383),to(#202020));
        }
        #footReturn:hover, #footReturn:active, #footReturn2:hover, #footReturn2:active
        {
            background-color: #525252;
            background-image: -webkit-gradient(linear,0% 0,0% 100%,from(#838383),to(#222));
        }
        #footReturn a, #footReturn2 a
        {
            display: block;
            line-height: 41px;
            color: #fff;
            text-shadow: 1px 1px #282828;
            font-size: 18px;
            font-weight: bold;
        }
        
        #footReturn a span, #footReturn2 a span
        {
            line-height: 41px;
            padding-left: 28px;
            background: url('/Images/arrow1.png') no-repeat 0 50%;
            -webkit-background-size: 12px 15.5px;
            background-size: 12px 15.5px;
        }
        #footReturn[hidden], #footReturn2[hidden]
        {
            display: none;
        }
        
        body {
background-color: #fff;
line-height: 1.5;
color: #000;
text-align: left;
}
    </style>
</head>
<body id="body">
    <div class="qb_gap pg_upgrade_content" id="seldiv">
        <div class="mod_color_weak qb_fs_s qb_gap qb_pt10">
            自助预约
        </div>
        <!-- 电子码 -->
        <div class="mod_input qb_mb10 qb_flex" id="divTel">
            <label for="_tmp_4">
                电子码：</label>
            <input value="" class="flex_box" placeholder="请输入电子码" id="txtpno" autocomplete="off">
        </div>
         <div class="mod_input qb_mb10 qb_flex"  >
            <label for="_tmp_4">
                验证码：</label>
            <input name="getcode" type="text" placeholder="验证码" id="getcode" size="10" class="i-text i-text-authcode ui-input sl-checkcode-input" />
                                <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码" />
                                <a href="javascript:;" id="validateCodetext">更换</a>
        </div>

        

        <span id="PnoVer"></span><a id="selBtn" href="javascript:void(0)" class="mod_btn btn_block qb_mb10">
            查&nbsp;询</a>
    </div>
    <div class="qb_gap pg_upgrade_content" id="viewdiv" style="display: none;">
        <div class="mod_color_weak qb_fs_s qb_gap qb_pt10">
            自助预约
        </div>
        <div id="mappContainer">
            <div class="inner root">
                <ul>
                    <li>
                        <h2 id="view_proname">
                        </h2>
                    </li>
                    <li style="color: #797979; font-size: 24px;">
                        <h3 id="view_canbooknum">
                        </h3>
                    </li>
                    <li style="color: #797979; font-size: 24px;">请在下面提交客人预约信息: </li>
                    <li>
                        <table class="grid" id="travel_tb">
                            <tr>
                                <td class="tdHead" valign="top" colspan="2">
                                    <label class="bustitle">
                                        姓 名：</label><input type="text" id="bespeakname" value="" style="width: 160px;"  autocomplete="off"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="bustitle">
                                        手机号：</label><input type="text" id="phone" value="" style="width: 160px;"  autocomplete="off"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="bustitle">
                                        身份证号：</label><input type="text" id="idcard" value="" style="width: 360px;"  autocomplete="off"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="bustitle">
                                        预约日期：</label><input type="text" id="bespeakdate" value="" style="width: 160px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="bustitle">
                                        预约人数：</label>
                                    
                                    <input type="text" id="bespeaknum" value="" style="width: 160px;" readonly="readonly" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="bustitle">
                                        备注：</label><input type="text" id="remark" value="" style="width: 360px;"  autocomplete="off"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span1"></span><a id="subBtn" href="javascript:void(0)" class="mod_btn btn_block qb_mb10"  >
                                        提&nbsp;交</a>
                                </td>
                            </tr>
                        </table>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="qb_gap pg_upgrade_content" id="viewresultdiv" style="display: none;">
        <div class="mod_color_weak qb_fs_s qb_gap qb_pt10">
            自助预约
        </div>
        <div>
            <div class="inner root">
                <ul>
                    <li>
                        <h2>
                            你的预约请求已经提交，请等待管理员确认后短信通知。</h2>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div id="calDiv" style="display: none; margin-top: 40px;max-width: 640px;">
    </div>
    <input type="hidden" id="hidLeavingDate" value="" />
    <input type="hidden" id="hidMinLeavingDate" value="" />
    <input type="hidden" id="hidLinePrice" value="" />
    <input type="hidden" id="hid_orderid" value="0" />
    <input type="hidden" id="hid_pno" value="0" />
    <input type="hidden" id="hid_comid" value="" />
    <input type="hidden" id="hid_proid" value="" />
    <input type="hidden" id="hid_proname" value="" />
</body>
</html>
   <input type="hidden" id="hid_orderid" value="0" />
    <input type="hidden" id="hid_pno" value="0" />
    <input type="hidden" id="hid_comid" value="" />
    <input type="hidden" id="hid_proid" value="" />
    <input type="hidden" id="hid_proname" value="" />
</body>
</html>
