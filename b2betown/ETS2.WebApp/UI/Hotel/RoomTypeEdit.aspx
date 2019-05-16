<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomTypeEdit.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.UI.HotelUI.RoomTypeEdit" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            //首先加载数据
            var roomtypeid = $("#hid_roomtypeid").trimVal();
            var comid = $("#hid_comid").trimVal();
            bindViewImg();
            //日历
            var nowdate = '<%=DateTime.Now.ToString("yyyy-MM-dd") %>';
            var dateinput = $("input[isdate=yes]");
            if (roomtypeid == '0') {
                $("#pro_start").val(new Date().format("yyyy-MM-dd"));
                $("#pro_end").val(DateAdd(new Date(), 1).format("yyyy-MM-dd"));
            }
            $.each(dateinput, function (i) {
                $($(this)).datepicker();

            });


            if (roomtypeid != '0') {
                $.post("/JsonFactory/ProductHandler.ashx?oper=GetRoomType", { roomtypeid: roomtypeid, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取房型有误");
                        return;
                    }
                    if (data.type == 100) {

                        $("#name").val(data.msg.Name);
                        $("#bedtype").val(data.msg.Bedtype);
                        $("#bedwidth").val(data.msg.Bedwidth);
                        $("input[name='whetherextrabed'][value='" + data.msg.Whetherextrabed + "']").attr("checked", true);
                        $("#extrabedprice").val(data.msg.Extrabedprice);
                        $("#ReserveType").val(data.msg.ReserveType);
                        $("#wifi").val(data.msg.Wifi);
                        $("#Breakfast").val(data.msg.Breakfast);
                        $("#server_type").val(data.msg.Server_type);
                        $("#pro_type").val(data.msg.Pro_type);
                        $("#source_type").val(data.msg.Source_type);
                        $("#Sms").val(data.msg.Sms);
                        $("#Builtuparea").val(data.msg.Builtuparea);
                        $("#floor").val(data.msg.Floor);
                        $("#largestguestnum").val(data.msg.Largestguestnum);

                        $("input[name='whethernon-smoking'][value='" + data.msg.Whethernonsmoking + "']").attr("checked", true);
                        $("#amenities").val(data.msg.Amenities);
                        $("#Mediatechnology").val(data.msg.Mediatechnology);
                        $("#Foodanddrink").val(data.msg.Foodanddrink);
                        $("#ShowerRoom").val(data.msg.ShowerRoom);
                        $("input[name='whetheravailabel'][value='" + data.msg.Whetheravailabel + "']").attr("checked", true);

                        $("#roomtyperemark").val(data.msg.Roomtyperemark);
                        $("#hid_imgurl").val(data.msg.Roomtyperemark);

                        $("#RecerceSMSName").val(data.msg.RecerceSMSName);
                        $("#RecerceSMSPhone").val(data.msg.RecerceSMSPhone);

                        if (data.msg.Sms == "" || data.msg.Sms == null) {
                            //新添加产品，自动设定电子票内容
                            $("#Sms").val("您订购的 $产品名称$ ，$数量$张，有效期到：$有效期$，电子码：$票号$,请在有效期内使用！");
                        } else {
                            $("#Sms").val(data.msg.Sms);
                        }

                        $.post("/JsonFactory/ProductHandler.ashx?oper=GetRoomTypeDayList", { roomtypeid: roomtypeid }, function (data1) {
                            data1 = eval("(" + data1 + ")");
                            if (data1.type == 1) {
                                $.prompt("获取房型每日房况有误");
                                return;
                            }
                            if (data1.type == 100) {
                                $("#tbody1").html("");
                                var appendhtml = "";
                                for (var j = 0; j < data1.msg.length; j++) {
                                    appendhtml += '<tr>' +
                                        '<td colspan="2">' +
                                            new Date(ChangeDateFormat(data1.msg[j].Daydate)).format("yyyy-MM-dd") + ' 当日价格:<input type="text" id="dayprice' + new Date(ChangeDateFormat(data1.msg[j].Daydate)).format("yyyyMMdd") + '" value="' + data1.msg[j].Dayprice + '" />' +
                                            '当日空房数量:<input type="text" id="availablenum' + new Date(ChangeDateFormat(data1.msg[j].Daydate)).format("yyyyMMdd") + '" value="' + data1.msg[j].Dayavailablenum + '" />' +
                                            '预定类型:<select id="reservetype' + new Date(ChangeDateFormat(data1.msg[j].Daydate)).format("yyyyMMdd") + '">';

                                    if (data1.msg[j].ReserveType == 1) {
                                        appendhtml += '<option value="1" selected="selected">前台现付</option>' +
                                                '<option value="2">预付电子凭证</option>';
                                    } else {
                                        appendhtml += '<option value="1">前台现付</option>' +
                                                '<option value="2" selected="selected">预付电子凭证</option>';
                                    }
                                    appendhtml += '</select>' +
                                        '</td>' +
                                    '</tr>';


                                }

                                $("#tbody1").html(appendhtml);

                                $("#pro_start").val(data1.prostart);
                                $("#pro_end").val(data1.proend);

                            }

                        })
                    }

                })
            }

            //确认发布按钮
            $("#GoProAddNext").click(function () {


                var name = $("#name").trimVal();
                if (name == "") {
                    $.prompt("房型名称不可为空!");
                    return;
                }
                var bedtype = $("#bedtype").trimVal();
                if (bedtype == "") {
                    $.prompt("床型不可为空!");
                    return;
                }
                var bedwidth = $("#bedwidth").trimVal();
                if (bedwidth == "") {
                    $.prompt("床宽不可为空!");
                    return;
                }
                var whetherextrabed = $("input:radio[name='whetherextrabed']:checked").val();


                var extrabedprice = $("#extrabedprice").trimVal();
                if (whetherextrabed == "true") {
                    if (extrabedprice == "") {
                        $.prompt("请填写加床费用");
                        return;
                    } else {
                        if (isNaN($("#extrabedprice").trimVal())) {
                            $.prompt("加床费用请输入数字");
                            return;
                        }
                    }
                }

                var ReserveType = $("#ReserveType").val();

                var wifi = $("#wifi").trimVal();
                if (wifi == "") {
                    $.prompt("请填写wifi情况");
                    return;
                }
                var Breakfast = $("#Breakfast").val();

                var server_type = $("#server_type").val();
                var pro_type = $("#pro_type").val();
                var source_type = $("#source_type").val();
                var Sms = $("#Sms").trimVal();
                //                if (Sms == "") {
                //                    $.prompt("短信不能为空，产生的电子票则按此短信内容发送");
                //                    return;
                //                }
                var Builtuparea = $("#Builtuparea").trimVal();

                var floor = $("#floor").trimVal();
                var largestguestnum = $("#largestguestnum").trimVal();
                if (largestguestnum == "") {
                    $.prompt("请填写最多入住人数");
                    return;
                }
                else {
                    if (isNaN($("#largestguestnum").trimVal())) {
                        $.prompt("最多入住人数请输入数字");
                        return;
                    }
                }


                var whethernonsmoking = $('input:radio[name="whethernon-smoking"]:checked').val();
                var amenities = $("#amenities").trimVal();
                var Mediatechnology = $("#Mediatechnology").trimVal();
                var Foodanddrink = $("#Foodanddrink").trimVal();
                var ShowerRoom = $("#ShowerRoom").trimVal();

                var imgurl = $("#<%=headPortrait.FileUploadId_ClientId %>").val();

                if (imgurl == "") {
                    imgurl = $("#hid_imgurl").trimVal();
                }
                var whetheravailabel = $("input:radio[name='whetheravailabel']:checked").val();
                var roomtyperemark = $("#roomtyperemark").trimVal();

                if (isMobel($("#RecerceSMSPhone").trimVal()) == false) {
                    alert("接收预定短信人员电话格式不正确");
                    return;
                }

                //房型每日的房间情况
                var prostart = $("#pro_start").trimVal();
                var proend = $("#pro_end").trimVal();
                var diff = DateDiff(proend, prostart); //两个日期差值
                var roomtypepara = "";
                for (var i = 0; i <= diff; i++) {
                    var ndate = DateAdd(prostart, i).format("yyyyMMdd");

                    var daypricepara = "dayprice" + ndate;
                    var availablenumpara = "availablenum" + ndate;
                    var reservetypepara = "reservetype" + ndate;

                    var dayprice = $("#" + daypricepara).val();
                    if (dayprice == "" || isNaN($("#" + daypricepara).val())) {
                        alert("当日价格格式输入有误");
                        return;
                    }
                    var availablenum = $("#" + availablenumpara).val();
                    if (availablenum == "" || isNaN($("#" + availablenumpara).val())) {
                        alert("当日可用空房数量格式有误");
                        return;
                    }
                    roomtypepara += daypricepara + "=" + dayprice + "&" + availablenumpara + "=" + availablenum + "&" + reservetypepara + "=" + $("#" + reservetypepara).val() + "&";

                }



                $.post("/JsonFactory/ProductHandler.ashx?oper=editroomtype", { RecerceSMSName: $("#RecerceSMSName").trimVal(), RecerceSMSPhone: $("#RecerceSMSPhone").trimVal(), prostart: prostart, proend: proend, roomtypepara: roomtypepara, createruserid: $("#hid_userid").trimVal(), comid: $("#hid_comid").trimVal(), id: roomtypeid, name: name, bedtype: bedtype, bedwidth: bedwidth, whetherextrabed: whetherextrabed, extrabedprice: extrabedprice, ReserveType: ReserveType, wifi: wifi, Breakfast: Breakfast, server_type: server_type, pro_type: pro_type, source_type: source_type, Sms: Sms, Builtuparea: Builtuparea, floor: floor, largestguestnum: largestguestnum, whethernonsmoking: whethernonsmoking, amenities: amenities, Mediatechnology: Mediatechnology, Foodanddrink: Foodanddrink, ShowerRoom: ShowerRoom, imgurl: imgurl, whetheravailabel: whetheravailabel, roomtyperemark: roomtyperemark }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        $.prompt("房型发布成功", {
                            buttons: [
                                                 { title: '确定', value: true }
                                                ],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "RoomTypeList.aspx";
                            }
                        });
                        return;
                    } else {
                        $.prompt("房型信息编辑出错");
                        return;
                    }
                })

            })


            $("#bb").click(function () {
                var prostart = $("#pro_start").trimVal();
                var proend = $("#pro_end").trimVal();

                if (new Date(prostart) < new Date(proend)) {

                    if (new Date(new Date().format("yyyy-MM-dd")) > new Date(prostart)) {
                        alert("开始日期不可小于当前日期")
                        return;
                    } else {
                        var diff = DateDiff(proend, prostart); //两个日期差值
                        if (diff == false) {
                            alert("请检查开始截止日期输入是否有误");
                            return;
                        } else {
                            $("#tbody1").html("");
                            var appendhtml = ""; //发布的日期区间内字符串
                            //alert(DateAdd(prostart, 2).format("yyyyMMdd"));
                            //alert(diff);
                            for (var i = 0; i <= diff; i++) {
                                var ndate = DateAdd(prostart, i).format("yyyyMMdd");
                                appendhtml += '<tr>' +
                                        '<td colspan="2">' +
                                            DateAdd(prostart, i).format("yyyy-MM-dd") + ' 当日价格:<input type="text" id="dayprice' + ndate + '" value="1" />' +
                                            '当日空房数量:<input type="text" id="availablenum' + ndate + '" value="1" />' +
                                            '预定类型:<select id="reservetype' + ndate + '">' +
                                                '<option value="1">前台现付</option>' +
                                                '<option value="2">预付电子凭证</option>' +
                                            '</select>' +
                                        '</td>' +
                                    '</tr>';
                            }
                            $("#tbody1").append(appendhtml);
                        }
                    }
                }
                else {
                    alert("开始日期需要小于截止日期");
                    return;
                }

            })

        })

        function bindViewImg() {
            var defaultPath = "";
            var imgSrc = '<%=headPortraitImgSrc %>';
            if (imgSrc == "") {
                //                $("#headPortraitImg").attr("src", defaultPath);
            } else {
                var filePath = '<%=headPortrait.fileUrl %>';
                var headlogoImgSrc = filePath + imgSrc;
                $("#headPortraitImg").attr("src", headlogoImgSrc);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()" target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductAdd.aspx" onfocus="this.blur()" target=""><span>添加产品</span></a></li>
                <li><a href="/UI/hotel/RoomTypeList.aspx" onfocus="this.blur()" target=""><span>客房管理</span></a></li>
                <li class="on"><a href="/UI/hotel/RoomTypeEdit.aspx" onfocus="this.blur()" target="">
                    <span>添加房型 </span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    添加酒店房型
                </h3>
                <table class="grid">
                    <tr>
                        <td colspan="2">
                            <h3>
                                房型基本信息</h3>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            房型名称：
                        </td>
                        <td>
                            <input name="name" type="text" id="name" value="" size="50" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            床型
                        </td>
                        <td>
                            <input name="bedtype" type="text" id="bedtype" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            床宽：
                        </td>
                        <td>
                            <input name="bedwidth" type="text" id="bedwidth" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            可否加床：
                        </td>
                        <td>
                            <label>
                                <input name="whetherextrabed" type="radio" value="true" checked>
                                可以</label>
                            <label>
                                <input name="whetherextrabed" type="radio" value="false">
                                不可以</label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            加床费用：
                        </td>
                        <td>
                            <input name="extrabedprice" type="text" id="extrabedprice" value="" />(单位：“/床/间夜”)
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdHead">
                            预订类型：
                        </td>
                        <td>
                            <select name="ReserveType" id="ReserveType">
                                <option value="1">前台现付</option>
                                <option value="2">预付电子凭证</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            wifi：
                        </td>
                        <td>
                            <input name="wifi" cols="50" rows="1" id="wifi">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            早餐情况：
                        </td>
                        <td>
                            <select name="Breakfast" id="Breakfast">
                                <option value="1">无早</option>
                                <option value="2">单早</option>
                                <option value="3">双早</option>
                            </select>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdHead">
                            服务类型：
                        </td>
                        <td>
                            <select name="server_type" id="server_type">
                                <option value="1">电子凭证票</option>
                            </select>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdHead">
                            票类型：
                        </td>
                        <td>
                            <select name="pro_type" id="pro_type">
                                <option value="1">电子票</option>
                            </select>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdHead">
                            出票来源：
                        </td>
                        <td>
                            <select name="source_type" id="source_type">
                                <option value="1">系统自动生成</option>
                                <option value="2">倒码</option>
                            </select>
                            <br />
                            系统自动生成：是采用本系统出票，验票。
                            <br />
                            倒码：只通过本系统出票，电子码由其供应商提供。
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdHead">
                            短信格式：
                        </td>
                        <td>
                            <textarea name="Sms" cols="50" rows="5" id="Sms"></textarea>
                            <br />
                            $票号$ $姓名$ $数量$ $有效期$ $产品名称$ 进行替换为相应的值。
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            建筑面积(选填)：
                        </td>
                        <td>
                            <input name="Builtuparea" type="text" id="Builtuparea" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            楼层(选填)：
                        </td>
                        <td>
                            <input name="floor" type="text" id="floor" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            最多入住人数(数字格式,必填)：
                        </td>
                        <td>
                            <input type="text" name="largestguestnum" id="largestguestnum">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            可否安排无烟楼层：
                        </td>
                        <td>
                            <label>
                                <input name="whethernon-smoking" type="radio" value="true">
                                可以</label>
                            <label>
                                <input name="whethernon-smoking" type="radio" value="false" checked>
                                不可以</label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            便利设施(选填)：
                        </td>
                        <td>
                            <textarea name="amenities" cols="50" rows="2" id="amenities"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            媒体/科技(选填)：
                        </td>
                        <td>
                            <textarea name="Mediatechnology" cols="50" rows="2" id="Mediatechnology"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            食品和饮品(选填)：
                        </td>
                        <td>
                            <textarea name="Foodanddrink" cols="50" rows="2" id="Foodanddrink"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            浴室(选填)：
                        </td>
                        <td>
                            <textarea name="ShowerRoom" cols="50" rows="2" id="ShowerRoom"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            房型图片 ：
                        </td>
                        <td>
                            <div class="C_head">
                                <dl>
                                    <dt>
                                        <input type="hidden" id="Hidden1" value="" />
                                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" /></dt>
                                    <dd>
                                    </dd>
                                </dl>
                                <div class="cl">
                                </div>
                            </div>
                            <div class="C_head_no">
                                <div class="C_head_1">
                                    <ul>
                                        <li style="height: 20px; margin-left: 40px">
                                            <div class="C_verify">
                                                <label>
                                                    更换图片：</label>
                                                <span>
                                                    <uc1:uploadFile ID="headPortrait" runat="server" />
                                                </span>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            上线/下线：
                        </td>
                        <td>
                            <label>
                                <input name="whetheravailabel" type="radio" value="true" checked>
                                上线</label>
                            <label>
                                <input name="whetheravailabel" type="radio" value="false">
                                下线</label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            接收预定短信人员姓名：
                        </td>
                        <td>
                            <input type="text" name="RecerceSMSName" id="RecerceSMSName" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            接收预定短信人员电话：
                        </td>
                        <td>
                            <input type="text" name="RecerceSMSPhone" id="RecerceSMSPhone" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            房型备注(选填)：
                        </td>
                        <td>
                            <textarea name="roomtyperemark" cols="50" rows="5" id="roomtyperemark"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            当前日期
                        </td>
                        <td>
                            <%=DateTime.Now.ToString("yyyy-MM-dd") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            发布的日期区间
                        </td>
                        <td>
                            开始
                            <input name="pro_start" type="text" id="pro_start" value="" size="12" isdate="yes" />
                            截止
                            <input name="pro_end" type="text" id="pro_end" value="" size="12" isdate="yes" />
                            <input type="button" id="bb" value="确 定" />
                        </td>
                    </tr>
                    <tbody id="tbody1" style="background-color: rgb(239, 239, 239);">
                    </tbody>
                </table>
                <table border="0">
                    <tr>
                        <td width="600" height="80" align="center">
                            <input type="button" name="GoProAddNext" id="GoProAddNext" value="  确认发布该房型  " />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_roomtypeid" value="<%=roomtypeid %>" />
    <input type="hidden" id="hid_imgurl" value="" />
</asp:Content>
