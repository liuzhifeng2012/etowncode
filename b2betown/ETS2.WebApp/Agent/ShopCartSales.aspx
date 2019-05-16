<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="ShopCartSales.aspx.cs" Inherits="ETS2.WebApp.Agent.ShopCartSales" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-rili.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>

    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script src="/Scripts/convertToPinyinLower.js" type="text/javascript"></script>
        <script src="/Scripts/shopcart.js" type="text/javascript"></script>
        <style type="text/css">

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

    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        Math.formatFloat = function (f, digit) {
            var m = Math.pow(10, digit);
            return parseInt(f * m, 10) / m;
        }



        $(function () {

            var agentid = $("#hid_agentid").trimVal();
            var proid = $("#hid_proid").trimVal();
            var speciid = $("#hid_speciid").trimVal();
            var cartid = $("#hid_cartid").trimVal();
            var num = $("#hid_num").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                var pro_youxiaoqi = "";
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=agentcartlist",
                    data: { proid: proid, speciid: speciid, cartid: cartid, agentid: agentid, comid: comid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            var prolisthtml = "";
                            var prosum = 0;

                            for (var i = 0; i < data.msg.length; i++) {
                                prolisthtml += '<tr><td width="550" style="border-bottom: 1px solid #CCC;"><p align="left">' + data.msg[i].Pro_name + '</p></td><td width="80" style="border-bottom: 1px solid #CCC;"><p align="right">' + data.msg[i].Agent_price + '</p></td><td width="70" style="border-bottom: 1px solid #CCC;"><p align="right">' + data.msg[i].U_num + '</p></td></tr>';
                                prosum += data.msg[i].Agent_price * data.msg[i].U_num;
                            }
                            $("#prolist").append(prolisthtml);

                            //如果是实物产品显示出运费模块


                            $("#prosum").html("总商品金额：￥" + prosum);
                            $("#hid_payprice").val(prosum);
                            getDeliveryFee(proid, num, $("#com_city").trimVal());
                            $("#tbody_address").show();

                        }
                    }
                })

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

                $("#confirmButton").click(function () {
                    var proid = $("#hid_proid").trimVal();
                    var speciid = $("#hid_speciid").trimVal();
                    var cartid = $("#hid_cartid").trimVal();
                    var u_num = $("#hid_num").trimVal();
                    var u_name = $("#u_name").trimVal();
                    var u_phone = $("#u_phone").trimVal();
                    var u_traveldate = $("#u_traveldate").trimVal();

                    var deliverytype = $("input:radio[name='deliverytype']:checked").trimVal();
                    var province = $("#com_province").trimVal();
                    var city = $("#com_city").trimVal();
                    var address = $("#txtaddress").trimVal();
                    var txtcode = $("#txtcode").trimVal();

                    var saveaddress = $("input:checkbox[name='saveaddress']:checked").trimVal(); ;
                    var payprice = $("#hid_payprice").trimVal();
                    



                    if (u_name == "") {
                        $.prompt("请填写接收人姓名");
                        return;
                    }
                    if (u_phone == "") {
                        $.prompt("请填写接收人手机号，来接收电子票短信");
                        return;
                    } else {
                        if (!isMobel(u_phone)) {
                            $.prompt("请正确填写接收人手机号");
                            return;
                        }
                    }


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
                            if (travel_idcard == "") {
                                alert("乘车人" + travel_name + "身份证号不可为空");
                                errid = "travelidcard" + i;
                                break;

                            } else {
                                if (!IdCardValidate(travel_idcard)) {
                                    alert("乘车人" + travel_name + "身份证格式错误");
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


                    //创建订单
                    $('#confirmButton').hide().after('<span id="spLoginLoading" style="margin-left:10px;color:#f39800; font-size:16px;">提交中……</span>');
                    $.post("/JsonFactory/OrderHandler.ashx?oper=agentcartorder", { deliverytype: deliverytype, province: province, city: city, address: address, txtcode: txtcode, agentid: agentid, comid: comid, proid: proid, speciid: speciid, cartid: cartid, u_num: u_num, u_name: u_name, u_phone: u_phone, u_traveldate: u_traveldate, travelnames: travelnames, travelidcards: travelidcards, travelnations: travelnations, travelphones: travelphones, travelremarks: travelremarks, travel_pickuppoints: $("#pointuppoint").trimVal(), travel_dropoffpoints: $("#dropoffpoint").trimVal(), order_remark: $("#order_remark").trimVal(), saveaddress: saveaddress, payprice: payprice }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            if (data.msg == "yfkbz") {
                                alert("您的预付款不足，先充足预付款后，再进购物车提交订单");

                                //创建支付订单
                                $.post("/JsonFactory/OrderHandler.ashx?oper=agentRecharge", { agentid: agentid, comid: comid, payprice: data.money, u_name: "分销客户:" + u_name, u_phone: u_phone }, function (data) {
                                    data = eval("(" + data + ")");
                                    if (data.type == 1) {
                                        $.prompt(data.msg);
                                        return;
                                    }
                                    if (data.type == 100) {
                                        location.href = "pay.aspx?orderid=" + data.msg + "&comid=" + comid+"&act=cart" ;
                                        return;
                                    }
                                })
                                //$('#confirmButton').show();
                                $('#spLoginLoading').hide();
                            } else {
                                $.prompt(data.msg);
                                $('#confirmButton').show();
                                $('#spLoginLoading').hide();
                            }

                            return;
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
                    $.prompt("请填写正确地宝贝数量");
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
                   '<label>民族：</label><input type="text" id="travelnation' + i + '" value="汉族"  style="width:40px;"/>' +
                   '<label>备注：</label><input type="text" id="travelremark' + i + '" value=""  style="width:100px;" />' +

                    '</td>' +
                '</tr> ');

                    }
                    var totalcount = $("#u_num").trimVal() * $("#hid_payprice").trimVal();
                    $("#heji").text("￥" + fmoney(totalcount, 2));
                }
            })
            //人数减
            $(".btn-reduce").click(function () {
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
                            '<label>民族：</label><input type="text" id="travelnation' + j + '" value="' + minzu + '" style="width:40px;"/>' +
                            '<label>备注：</label><input type="text" id="travelremark' + j + '" value="' + $("#travelremark" + j).trimVal() + '"  style="width:100px;" />';
                                if (j == 1) {
                                    ihtml += '<input name="chkItem" type="checkbox" value="1" id="chkItem" />与预定人相同';
                                }
                                ihtml += '</td>' +
                '</tr> ';

                            }
                            $("#travel_tbody").html(ihtml);

                        }

                        var totalcount = $("#u_num").trimVal() * $("#hid_payprice").trimVal();
                        $("#heji").text("￥" + fmoney(totalcount, 2));
                    } else {

                        return false;
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
                            i = 60;
                            //                            return;
                        }
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
                            '<label>民族：</label><input type="text" id="travelnation' + j + '" value="' + minzu + '" style="width:40px;"/>' +
                             '<label>备注：</label><input type="text" id="travelremark' + j + '" value="' + $("#travelremark" + j).trimVal() + '"  style="width:100px;" />';
                            if (j == 1) {
                                ihtml += '<input name="chkItem" type="checkbox" value="1" id="chkItem" />与预定人相同';
                            }
                            ihtml += '</td>' +
                '</tr> ';
                        }
                        $("#travel_tbody").html(ihtml);
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
            //日历
            $("#u_traveldate").click(function () {
                scrollTo(0, 1);
                $("#calDiv").fadeIn();
                $("#setting-home").hide();
                $("#secondary-tabs").hide();
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

                getDeliveryFee("", "", "");
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

            
        })

        function getDeliveryFee(proid, num, city) {
            var yunfei = 0;
           var chked = $("input:radio[name='deliverytype']:checked").val();
           $.post("/JsonFactory/OrderHandler.ashx?oper=getshopcartexpressfee", { proidstr: $("#hid_proid").val(), numstr: $("#hid_num").val(), citystr: $("#com_city").val() }, function (data) {
               data = eval("(" + data + ")");
               if (data.type == 100) {

                   if (chked == 4) {
                       $("#yunfei").html(" 运费：￥0");
                       yunfei = 0;
                       $("#heji").html("应付总额：￥" + Math.formatFloat(parseFloat($("#hid_payprice").trimVal()), 2));
                       
                   } else {
                       $("#yunfei").html(" 运费：￥" + data.msg);
                       yunfei = data.msg;
                       $("#heji").html("应付总额：￥" + Math.formatFloat(parseFloat(data.msg) + parseFloat($("#hid_payprice").trimVal()), 2));
                   }


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
            var num = $("#hid_num").val();
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
        
        .wrap-input {
          float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <table>
            <tr>
                <td class="tdHead" style="font-size: 14px; height: 26px;">
                    商户名称：
                    <%=company %>
                    (授权类型：
                    <%=Warrant_type_str%>)
                </td>
            </tr>
            <tr>
                <td class="tdHead" style="font-size: 14px; height: 26px;">
                    <%=yufukuan%>
                     <a class="a_anniu" href="Recharge.aspx?comid=<%=comid_temp %>" target="_blank">在线充值</a> 
                </td>
            </tr>
        </table>
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="Manage_sales.aspx?comid=<%=comid_temp %>" onfocus="this.blur()">
                    <span>产品列表</span></a></li>
                <li><a href="Order.aspx?comid=<%=comid_temp %>" onfocus="this.blur()"><span>订单记录</span></a></li>
                <%if (Warrant_type == 2)
                  { %>
                <li><a href=" EticketCount.aspx?comid=<%=comid_temp %>" onfocus="this.blur()"><span>
                    验码统计</span></a></li>
                <li><a href="Verification.aspx?comid=<%=comid_temp %>" onfocus="this.blur()"><span>验码记录</span></a></li>
                <%} %>
                <li><a href="Finane.aspx?comid=<%=comid_temp %>" onfocus="this.blur()"><span>财务列表</span></a></li>
                <li><a href="EticketBack.aspx?comid=<%=comid_temp %>" onfocus="this.blur()"><span>退订/订单状态</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3 style="width: 900px;">
                    产品基本信息</h3>
                <table width="900px" class="grid">
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
                <table width="900px" id="prolist" class="grid">
                    <tr style="background: none repeat scroll 0% 0% #F3F3F3;">                        
                        <td width="750" style="border-bottom: 1px solid #CCC;">
                            <p align="left">
                                商品</p>
                        </td>
                        <td width="80" style=" border-bottom: 1px solid #CCC;">
                             <p align="right">分销价(元)</p>
                        </td>
                        <td width="70" style="border-bottom: 1px solid #CCC;">
                            <p align="right"> 数量</p>
                        </td>
                    </tr>
                </table>
               <table  width="900px" class="grid">
                   <tr>
                        <td  style=" height:10px;">
                          
                        </td>
                    </tr>
                    </table>

                <h3 style="width: 900px;">
                    <%if (Warrant_type == 1)
                      {%>
                    接收人信息
                    <%} %>
                    <%if (Warrant_type == 2)
                      { %>
                    联系人（此订单不发送电子码）
                    <%} %>
                </h3>
                <table width="900px" class="grid">
                    <tr>
                        <td   style="width:85px;">
                            姓 名
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="u_name" value="" /><span style="color:Red;">*</span> <label><input name="saveaddress" value="1" type="checkbox" /> 保存到常用地址</label>  <input type="button" name="uesoldaddress" id="uesoldaddress" value="使用常用地址" />
                        </td>
                    </tr>
                    <tr>
                        <td   style="width:85px;">
                            接收手机
                        </td>
                        <td>
                            <input name="u_phone" class="dataNum dataIcon" id="u_phone" value="" /><span style="color:Red;">*</span>
                        </td>
                    </tr>
                    <tbody id="tbody_address" style="display: none;  ">
                        <!--实物产品收货人地址信息-->
                        <tr>
                            <td   style="width:85px;">
                                运送方式
                            </td>
                            <td  >
                                <label>
                                    <input name="deliverytype" type="radio" value="2" checked>
                                    快递(需缴纳运费)</label>
                                <label>
                                    <input name="deliverytype" type="radio" value="4"  >
                                    自提(免运费)</label>
                            </td>
                        </tr>
                        <tr id="delivery_tr1">
                            <td   style="width:85px;">
                                收货地址
                            </td>
                            <td>
                                <select name="com_province" id="com_province" class="ui-input">
                                    <option value="省份" selected="selected">省份</option>
                                </select>
                                <select name="com_city" id="com_city" class="ui-input">
                                    <option value="城市" selected="selected">城市</option>
                                </select><span style="color:Red;">*</span>
                            </td>
                        </tr>
                        <tr  id="delivery_tr2">
                            <td   style="width:85px;">
                                详细地址
                            </td>
                            <td>
                                <input name="txtaddress" class="dataNum dataIcon" id="txtaddress" value="" style="width: 350px;" /><span style="color:Red;">*</span>
                            </td>
                        </tr>
                        <tr  id="delivery_tr3">
                            <td   style="width:85px;">
                                邮编
                            </td>
                            <td>
                                <input name="txtcode" class="dataNum dataIcon" id="txtcode" value="" style="width: 122px;" />
                            </td>
                        </tr>

                    </tbody>

                  
                    <tbody id="tbody_busplus" style="display: none;">
                        <!--旅游大巴附加信息-->
                        <tr >
                            <td   style="width:85px;">
                                上车地点
                            </td>
                            <td>
                                <select id="pointuppoint">
                                </select>
                            </td>
                        </tr>
                        <tr id="tr_dropoffpoint" style="display: none;">
                            <td   style="width:85px;">
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
                <table width="900px" class="grid" id="travel_tb" style="display: none;">
                    <tr>
                        <td class="tdHead" valign="top" colspan="2">
                            <h3>
                                乘车人信息(请务必填写用于保险):</h3>
                        </td>
                    </tr>
                    <tbody id="travel_tbody">
                    </tbody>
                </table>
                <table  width="900px" class="grid">
                   <tr>
                        <td  style="width:85px;">
                            备注
                        </td>
                        <td>
                            <input  class="dataNum dataIcon" id="order_remark" value="" style="width:350px;"/> 
                        </td>
                    </tr>
                    </table>
                <hr style="width: 900px; margin:10px 0" />
                 <table width="900px" class="grid">
                    <tr>                        
                        <td >
                            <p align="right" id="prosum">
                                </p>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <p align="right" id="yunfei">
                                </p>
                        </td>
                    </tr>
                    <tr>                        
                        <td>
                            <p align="right" id="heji">
                                </p>
                        </td>
                    </tr>
                </table>
                <table width="900px" class="grid">
                    <tr>
                        <td class="tdHead" style="text-align: right;">
                            <button id="confirmButton" name="confirmButton" type="button" class="btSubOrder1 noOrderSubmit1">
                                提交订单</button>    
                                
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="calDiv" style="display: none; margin-top: -40px">
    </div>
    <div class="data">
    </div>

    <div id="bindingagent" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 760px; height: 450px; display: none; z-index: 10; left: 20%;
        top: 20%;" class="dialog">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" 
            style="padding: 10px;">
            <tr>
                <td height="100%">
                    <div id="agentlist" style=" margin:20px;">
                        <table width="720" border="0">
                            <tr style="background-color:#999999;border-width: 1px;border-style: solid;border-color: #A6A6A6 #CCC #CCC;">
                                <td width="50"  height="30">
                                    <p align="left" style=" padding-left:5px;">
                                        姓名
                                    </p>
                                </td>
                                <td width="50">
                                    <p align="left">
                                        手机
                                    </p>
                                </td>
                                <td width="120">
                                    <p align="left">
                                        城市
                                    </p>
                                </td>
                                <td width="380">
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

    <div id='cartt' style=" display:none;position: absolute; bottom: 6em; right: 2em; width: 55px; height:55px; background-color: #FFFAFA; margin:10px; border-radius:60px; border: solid rgb(55,55,55)  #FF0000   1px;cursor:pointer;"><a href="ShopCart.aspx?comid=<%=comid_temp %>"><img src="/images/cart.gif" width="39" style="padding:8px 0 0 9px;"/></a></div>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <input id="hid_proid" type="hidden" value="<%=id %>" />
     <input id="hid_num" type="hidden" value="<%=num %>" />
    <input id="hid_speciid" type="hidden" value="<%=id_speciid %>" />
    <input id="hid_cartid" type="hidden" value="<%=cartid %>" />
    

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
    <script type="text/javascript">
        var province = document.getElementById('com_province');
        var city = document.getElementById('com_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</asp:Content>
