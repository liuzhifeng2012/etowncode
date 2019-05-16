<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="micromart_pay.aspx.cs"
    Inherits="ETS2.WebApp.wxpay.Mico" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html class="no-js " lang="zh-CN">
<head>
    <meta charset="utf-8" />
    <meta name="keywords" content="<%=title %>" />
    <meta name="description" content="" />
    <meta name="HandheldFriendly" content="True" />
    <meta name="MobileOptimized" content="320" />
    <meta name="format-detection" content="telephone=no" />
    <meta http-equiv="cleartype" content="on" />
    <title>
        <%=pro_name %>待付款的订单</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.cookie.2.2.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/weixin/CryptoJS/components/core-min.js"></script>
    <script type="text/javascript" src="/Scripts/weixin/CryptoJS/rollups/sha1.js"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link type="text/css" href="/Scripts/timepicker/css/jquery-ui-1.8.17.custom.css"
        rel="stylesheet" />
    <link type="text/css" href="/Scripts/timepicker/css/jquery-ui-timepicker-addon.css"
        rel="stylesheet" />
    <script type="text/javascript" src="/Scripts/timepicker/js/jquery-ui-1.8.17.custom.min.js"></script>
    <script type="text/javascript" src="/Scripts/timepicker/js/jquery-ui-timepicker-addon.js"></script>
    <script type="text/javascript" src="/Scripts/timepicker/js/jquery-ui-timepicker-zh-CN.js"></script>
	
    <script type="text/javascript">
        $(function () {

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                 $($(this)).datepicker({

                    <%if(isSetVisitDate==0){ %>
                    numberOfMonths: 1,
                    minDate: 0,
                    defaultDate: +4,
                    maxDate: '+8m +1w'

                    <%}else{ %>
                    minDate:'<%=pro_start.ToString("yyyy-MM-dd") %>',
                    maxDate:'<%=pro_end.ToString("yyyy-MM-dd") %>'
                      

                    <%} %>
                });
            });

            var comid = $("#hid_comid").val();

            if (isWeiXin() == true) {}else{
                $("#weixinpay").hide();
            }
           
            //根据公司id获得关注作者信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {
                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage btn btn-follow\">关注我们</a>");
                }
            });

            <%if (pro_servertype !=11 ){ %>
            //非实物产品隐藏城市及详细地址
            $(".No-Eticket").hide();
            $("#upaddress").html("编辑收货人及手机");
            <%} %>

            var deliverytype = $("input:radio[name='deliverytype']:checked").val();

            //判断浏览器，微信5.0以上版本浏览器的话调用微信 共享收货地址接口；否则 利用用户端cookie保存的收货地址
                if ($.cookie('cookie_name') == null) {
                   if ('<%=code %>'=='' &&'<%=pro_servertype %>'=='11') {
                    $("#sku-message-poppage").show();
                    // $("#in_province").append("<option value='' selected >请选择配送地区</option>");
                    }
                } else {
                    var name = $.cookie('cookie_name');
                    var phone = $.cookie('cookie_phone');
                    var idcard = $.cookie('cookie_idcard');
                    var province = $.cookie('cookie_province');
                    var city = $.cookie('cookie_city');
                    var address = $.cookie('cookie_address');
                    var code = $.cookie('cookie_code');

                    if (name== undefined){
                            name="";
                         }
                         if (phone== undefined){
                            phone="";
                         }
                         if (province== undefined || province== "省份"){
                            province="";
                         }
                         if (city== undefined || city== "城市"){
                            city="";
                         }
                         if (address== undefined){
                            address="";
                         }
                         if (code== undefined){
                            code="";
                         }

                    $("#hid_name").val(name);
                    $("#hid_phone").val(phone);
                    $("#hid_idcard").val(idcard);
                    var proid = $("#hid_proid").val();
                    var num = $("#hid_num").val();
                    var price = $("#hid_price").val();



                    <%if (pro_servertype ==11 ){ %>
                    $("#hid_province").val(province);
                    $("#hid_city").val(city);
                    $("#hid_address").val(address);
                    $("#hid_code").val(code);
                   

                    <%}else{ %>
                      province = "";
                      city = "";
                      address = "";
                      code = "";

                    <%} %>

                     $("#divaddress").html(name + " , " + phone + " , " + idcard + " <div>" + province + city + " </div><div>" + address + " " + code + "</div>");


                    var cart = $("#hid_cart").val();
                    var cart_num = $("#hid_cart_num").val();
                    var cart_id = $("#hid_cart_id").val();
                    
                    var proid_temp = proid;
                    var num_temp=num;

                    if (cart=="1"){
                      proid_temp = cart_id;
                      num_temp=cart_num;
                    }


                    //计算运费
                    $.post("/JsonFactory/OrderHandler.ashx?oper=getshopcartexpressfee", { proidstr: proid_temp, citystr: city, numstr: num_temp }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            //alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            deliverytype = $("input:radio[name='deliverytype']:checked").val();
                            if(deliverytype==4){
                            $("#expressfee").html("￥0 运费(自提)");
                            $("#total_price").html(" 合计：￥" + ((price * num) * 100) / 100);
                            }else{
                            $("#expressfee").html("￥" + data.msg + "运费");
                            $("#total_price").html(" 合计：￥" + ((price * num) * 100 + parseFloat(data.msg) * 100) / 100);
                            }
                            return;
                        }
                    })

                }
           


               //拼接url传参字符串
                function perapara(objvalues, isencode) {
                  var parastring = "";
                  for (var key in objvalues) {
                    isencode = isencode || false;
                    if (isencode) {
                      parastring += (key + "=" + encodeURIComponent(objvalues[key]) + "&");
                    }
                    else {
                      parastring += (key + "=" + objvalues[key] + "&");
                    }
                  }
                  parastring = parastring.substr(0, parastring.length - 1);
                  return parastring;
                }
                function getSign(beforesingstring) {
                  sign = CryptoJS.SHA1(beforesingstring).toString();
                  return sign;
                }


            $(".selectdeliverytype").click(function () {

                    var city = $("#hid_city").val();
                    var proid = $("#hid_proid").val();
                    var num = $("#hid_num").val();
                    var price = $("#hid_price").val();


                //计算运费
                    $.post("/JsonFactory/OrderHandler.ashx?oper=getshopcartexpressfee", { proidstr: proid_temp, citystr: city, numstr: num_temp }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            //alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            deliverytype = $("input:radio[name='deliverytype']:checked").val();
                            if(deliverytype==4){
                            $("#expressfee").html("￥0 运费(自提)");
                            $("#total_price").html(" 合计：￥" + ((price * num) * 100) / 100);
                            }else{
                            $("#expressfee").html("￥" + data.msg + "运费");
                            $("#total_price").html(" 合计：￥" + ((price * num) * 100 + parseFloat(data.msg) * 100) / 100);
                            }
                            return;
                        }
                    })

             })

            $("#upaddress").click(function () {

            if('<%=pro_servertype %>'!='11'){
                        $("#sku-message-poppage").show();
                        var name = $("#hid_name").val();
                        var phone = $("#hid_phone").val();
                        var idcard = $("#hid_idcard").val();
                        var province = $("#hid_province").val();
                        var city = $("#hid_city").val();
                        var address = $("#hid_address").val();
                        var code = $("#hid_code").val();

                        if (name== undefined){
                            name="";
                         }
                         if (phone== undefined){
                            phone="";
                         }
                         if (province== undefined || province== "省份"){
                            province="";
                         }
                         if (city== undefined || city== "城市"){
                            city="";
                         }
                         if (address== undefined){
                            address="";
                         }
                         if (code== undefined){
                            code="";
                         }


                        $("#in_name").val(name);
                        $("#in_phone").val(phone);
                        $("#in_idcard").val(idcard);
                        <%if (pro_servertype ==11 ){ %>
                        $("#in_province").val(province);
                        $("#in_city").append("<option value='" + city + "' selected >" + city + "</option>");
                        $("#in_address").val(address);
                        $("#in_code").val(code);
                        <%} %>

                        $("#divaddress").html(name + " , " + phone );
            }else{
               if ('<%=code %>'=='') {
                        $("#sku-message-poppage").show();
                        var name = $("#hid_name").val();
                        var phone = $("#hid_phone").val();
                        var idcard = $("#hid_idcard").val();
                        var province = $("#hid_province").val();
                        var city = $("#hid_city").val();
                        var address = $("#hid_address").val();
                        var code = $("#hid_code").val();


                         if (name== undefined){
                            name="";
                         }
                         if (phone== undefined){
                            phone="";
                         }
                         if (province== undefined || province== "省份"){
                            province="";
                         }
                         if (city== undefined || city== "城市"){
                            city="";
                         }
                         if (address== undefined){
                            address="";
                         }
                         if (code== undefined){
                            code="";
                         }

                        $("#in_name").val(name);
                        $("#in_phone").val(phone);
                         $("#in_idcard").val(idcard);
                      
                        $("#in_province").val(province);
                        $("#in_city").append("<option value='" + city + "' selected >" + city + "</option>");
                        $("#in_address").val(address);
                        $("#in_code").val(code);
                        

                        $("#divaddress").html(name + " , " + phone + " <div>" + province + city + " </div><div>" + address + " " + code + "</div>");
                } else { //判断微信版本,微信版本5.0以上才支持 收货地址共享接口

                           
                            //签名
                              var signparasobj = {
                                  "accesstoken": "",
                                  "appid": '<%=appId %>',
                                  "noncestr": "",
                                  "timestamp": "",
                                  "url": ""
                                };
                            var signparas = $.extend(signparasobj, {
                              "accesstoken": '<%=access_tokenstring %>',
                              "noncestr": '<%=nonceStr %>',
                              "timestamp": '<%=timeStamp %>',
                              "url": window.location.href
                            });
                             //签名
                            var signstring = getSign(perapara(signparas));

                            WeixinJSBridge.invoke('editAddress',{
                                "appId" : '<%=appId %>',
                                "scope" : "jsapi_address",
                                "signType" : "sha1",
                                "addrSign" :  signstring,
                                "timeStamp" : '<%=timeStamp %>',
                                "nonceStr" : '<%=nonceStr %>',
                            },function(res){
                               var ff = ''; 
//                               var obj = resvalues != null ? resvalues : document.getElementById('resvalues');
                                if (res == null) { 
//                                   obj.innerText = '测试返回为空'; 
                                }
                                else {
//                                        for (var key in res)
//                                        { 
//                                            var js = 'res.' + key + ' = ' + res[key].toString(); ff = ff + js;
//                                        }
//                                        obj.innerText = ff;

//                                      alert(ff);
                                         
                                    var name=res["userName"].toString();
                                    var phone= res["telNumber"].toString();
                                    var province=res["proviceFirstStageName"].toString() ;
                                    var city=res["addressCitySecondStageName"].toString();
                                    var address=res["addressDetailInfo"].toString()  ;
                                    var code=res["addressPostalCode"].toString()  ;
                                      
                                    if (name== undefined){
                                        name="";
                                     }
                                     if (phone== undefined){
                                        phone="";
                                     }
                                     if (province== undefined || province== "省份"){
                                        province="";
                                     }
                                     if (city== undefined || city== "城市"){
                                        city="";
                                     }
                                     if (address== undefined){
                                        address="";
                                     }
                                     if (code== undefined){
                                        code="";
                                     }
                                    
                                    //得到返回的数据 
                                    $("#sku-message-poppage").hide();
                                    $("#hid_name").val(name);
                                    $("#hid_phone").val(phone);
                                    $("#hid_province").val(province);
                                    $("#hid_city").val(city);
                                    $("#hid_address").val(address);
                                    $("#hid_code").val(code);

                                    
                                    $("#divaddress").html(name + " , " + phone + " <div>" + province + city + " </div><div>" + address + " " + code + "</div>");

                                    $.cookie('cookie_name', name, { expires: 365 });
                                    $.cookie('cookie_phone', phone, { expires: 365 });
                                   
                                    if(province !=""){
                                     $.cookie('cookie_province', province, { expires: 365 });
                                    }
                                    if(city !=""){
                                    $.cookie('cookie_city', city, { expires: 365 });
                                    }
                                    if(address !=""){
                                    $.cookie('cookie_address', address, { expires: 365 });
                                    }
                                    if(code !=""){
                                    $.cookie('cookie_code', code, { expires: 365 });
                                    }


                                    var proid = $("#hid_proid").val();
                                    var num = $("#hid_num").val();
                                    var price = $("#hid_price").val();


                                    var cart = $("#hid_cart").val();
                                    var cart_num = $("#hid_cart_num").val();
                                    var cart_id = $("#hid_cart_id").val();
                    
                                    var proid_temp = proid;
                                    var num_temp=num;

                                    if (cart=="1"){
                                      proid_temp = cart_id;
                                      num_temp=cart_num;
                                    }


                                    //计算运费
                                   $.post("/JsonFactory/OrderHandler.ashx?oper=getshopcartexpressfee", { proidstr: proid_temp, citystr: city, numstr: num_temp }, function (data) {
                                        data = eval("(" + data + ")");
                                        if (data.type == 1) {

                                            //alert(data.msg);
                                            return;
                                        }
                                        if (data.type == 100) {
                                            deliverytype = $("input:radio[name='deliverytype']:checked").val();
                                            if(deliverytype =4){
                                                $("#expressfee").html("￥ 0 运费(自提)");
                                                $("#total_price").html(" 合计：￥" + ((price * num) * 100) / 100);
                                            }else{
                                                $("#expressfee").html("￥" + data.msg + "运费");
                                                $("#total_price").html(" 合计：￥" + ((price * num) * 100 + parseFloat(data.msg) * 100) / 100);
                                            }

                                            return;
                                        }
                                    })
                                      
                                }

                            });
                        }  
            }
              
            });

            //判断购物车产品，进行加载
            var cart=$("#hid_cart").val();
            var userid=$("#hid_userid").val();
            if(cart == "1"){
            //装载产品列表
            $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=usercartlist",
                    data: { userid: userid, comid: comid,proid:$("#hid_cart_id").val(),speciid:$("#hid_id_speciid").val() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //$.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            var prolisthtml = "";
                            var prosum = 0;
                            for (var i = 0; i < data.msg.length; i++) {
                                prolisthtml +='<div class="block block-list block-border-top-none block-border-bottom-none" id="prolist4095">';
                                prolisthtml +='<div class="block-item name-card name-card-3col clearfix">';
                                prolisthtml +='<a href="Pro.aspx?id=4095" class="thumb">';
                                prolisthtml +='<img src="'+data.msg[i].Imgurl+'" height="60">';
                                prolisthtml +='</a>';
                                prolisthtml +='<div class="detail">';
                                prolisthtml +='<h3>' + data.msg[i].Pro_name + '</h3>';
                                prolisthtml +='<p class="sku-detail ellipsis js-toggle-more">';
                                prolisthtml +='<span class="c-gray">';
                                prolisthtml +='</span>';
                                prolisthtml +='</p>';
                                prolisthtml +='</div>';
                                prolisthtml +='<div class="right-col">';
                                prolisthtml +='<div class="price">￥<span>' + data.msg[i].Advise_price + '</span></div>';
                                prolisthtml +='<div class="num">';
                                prolisthtml +='<div class="clearfix">';
                                prolisthtml +='<div class="quantity">';
                                prolisthtml +='             <span>×' + data.msg[i].U_num + '</span>';                                                                                                                                   
                                prolisthtml +='<div class="txtCover"></div> ';                                
                                prolisthtml +='</div>';                       
                                prolisthtml +='</div>';
                                prolisthtml +='</div>';
                                prolisthtml +='</div>';
                                prolisthtml +='</div>';
                                prolisthtml +='</div><hr class="margin-0 left-10">';
                                prosum += data.msg[i].Agent_price * data.msg[i].U_num;
                            }
                            $("#prolist").html(prolisthtml);
                            $("#prolist").removeClass("name-card-3col");

                        }
                    }
                })
                
            }



            $("#submitsave").click(function () {
                var name = $("#in_name").val();
                var phone = $("#in_phone").val();
                var idcard= $("#in_idcard").val();
                var province = $("#in_province").val();
                var city = $("#in_city").val();
                var address = $("#in_address").val();
                var code = $("#in_code").val();
                deliverytype = $("input:radio[name='deliverytype']:checked").val();
                if (name == "") {
                    alert("请填写收货人");
                    $("#loading").hide();
                    return;
                }
                if (phone == "") {
                    alert("请填写联系人电话");
                    $("#loading").hide();
                    return;
                }
                <%if (issetidcard==1){%>
                        if(idcard==""){
                              alert("请填写身份证号");
                              $("#loading").hide();
                            return;
                        }else{
                            if (!IdCardValidate(idcard)) {
                                alert("身份证格式错误");
                                $("#loading").hide();
                                return;
                            }
                        }
                
                <%} %>

                <%if (pro_servertype ==11 ){ %>
                if(deliverytype !=4){//非自取
                    if (province == "" || province == null) {
                        alert("请选择地区");
                        $("#loading").hide();
                        return;
                    }
                    if (city == "" || city == null) {
                        alert("请选择所属城市或区县");
                        $("#loading").hide();
                        return;
                    }
                    if (address == "") {
                        alert("请详细填写地址");
                        $("#loading").hide();
                        return;
                    }

                }

                <%} %>
                $("#sku-message-poppage").hide();
                $("#hid_name").val(name);
                $("#hid_phone").val(phone);
                $("#hid_idcard").val(idcard);
                $("#hid_province").val(province);
                $("#hid_city").val(city);
                $("#hid_address").val(address);
                $("#hid_code").val(code);

              $("#divaddress").html(name + " , " + phone + ", " + idcard + " <div>" + province + city + " </div><div>" + address + " " + code + "</div>");

                $.cookie('cookie_name', name, { expires: 365 });
                $.cookie('cookie_phone', phone, { expires: 365 });
                $.cookie('cookie_idcard', idcard, { expires: 365 });
                if(province !=""){
                    $.cookie('cookie_province', province, { expires: 365 });
                }
                if(city !=""){
                    $.cookie('cookie_city', city, { expires: 365 });
                }
                if(address !=""){
                    $.cookie('cookie_address', address, { expires: 365 });
                }
                if(code !=""){
                   $.cookie('cookie_code', code, { expires: 365 });
                }



                var proid = $("#hid_proid").val();
                var num = $("#hid_num").val();
                var price = $("#hid_price").val();

                var cart = $("#hid_cart").val();
                var cart_num = $("#hid_cart_num").val();
                var cart_id = $("#hid_cart_id").val();
                    
                var proid_temp = proid;
                var num_temp=num;

                if (cart=="1"){
                  proid_temp = cart_id;
                  num_temp=cart_num;
                }

                //计算运费
                $.post("/JsonFactory/OrderHandler.ashx?oper=getshopcartexpressfee", { proidstr: proid_temp, citystr: city, numstr: num_temp }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {

                        //alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        if(deliverytype =4){
                            $("#expressfee").html("￥ 0 运费(自提)");
                            $("#total_price").html(" 合计：￥" + ((price * num) * 100) / 100);
                        }else{
                            $("#expressfee").html("￥" + data.msg + "运费");
                            $("#total_price").html(" 合计：￥" + ((price * num) * 100 + parseFloat(data.msg) * 100) / 100);
                        }
                        return;
                    }
                })
            });

            $("#submitcannel").click(function () {
                $("#sku-message-poppage").hide();
            });
            $("#wxpay").click(function () {
                $("#loading").show();
                createorder('wx');
            });

            $("#alipay").click(function () {
                $("#loading").show();
                createorder('alipay');
            });
            $("#webalipay").click(function () {
                $("#loading").show();
                createorder('webalipay');
            });



            $(".js-used-coupon").show();

            
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
                if ($("#hid_Im").val() == 0) {
                    $("#Imprest").removeClass("qb_icon icon_checkbox");
                    $("#Imprest").addClass("qb_icon icon_checkbox checked");

                    $("#hid_Im").val(1);
                }
                else {
                    $("#Imprest").removeClass("qb_icon icon_checkbox checked");
                    $("#Imprest").addClass("qb_icon icon_checkbox");

                    $("#hid_Im").val(0);
                }
            })


            function createorder(paytype) {
                $("#loading").show();
                var comid = $("#hid_comid").val();
                var proid = $("#hid_proid").val();

                var id_speciid =$("#hid_id_speciid").val();
                var serverid =$("#hid_serverid").val();
                var num = $("#hid_num").val();
                var price = $("#hid_price").val();
                var express = $("#hid_express").val();
                var name = $("#hid_name").val();
                var phone = $("#hid_phone").val();
                var idcard = $("#hid_idcard").val();
                var province = $("#hid_province").val();
                var city = $("#hid_city").val();
                var address = $("#hid_address").val();
                var code = $("#hid_code").val();
                
                 var u_traveldate = $("#u_traveldate").val();
                var message = $("#message").val();
                var cart = $("#hid_cart").val();
                var cart_num = $("#hid_cart_num").val();
                var cart_id = $("#hid_cart_id").val();
                deliverytype = $("input:radio[name='deliverytype']:checked").val();

                <%if (issetidcard==1){%>
                    if (idcard == "") {
                        alert("此票需要填写身份证信息");
                        $("#loading").hide();
                        return;
                    }
                <%} %>

                 <%if (isSetVisitDate==1){%>
                    if (u_traveldate == "") {
                        alert("请选择出游日期");
                        $("#loading").hide();
                        return;
                    }
                <%} %>

                var Integral = 0;
                var Imprest = 0;
                if ($("#hid_In").val() == 1) {
                    Integral = $("#hid_Integral").val();
                }
                if ($("#hid_Im").val() == 1) {
                    Imprest = $("#hid_Imprest").val();
                }

                if (proid == 0 || name == "" || phone == "" ) {
                    alert("请填写收货人及电话");
                    $("#upaddress").click();
                    $("#loading").hide();
                    return;
                }
                <%if (pro_servertype ==11 ){ %>
                if(deliverytype ==4){ }else{
                    if (province == "" || city == "" || city == "城市"|| address == "" ) {
                        alert("请选择地区并详细填写配送地址");
                        $("#upaddress").click();
                        $("#loading").hide();
                        return;
                    }
                }
                <%} %>

                if(cart =="0"){
                    //直接订购提交预订
                    $.post("/JsonFactory/OrderHandler.ashx?oper=createorder", {Integral: Integral, Imprest: Imprest, openid: $("#hid_openid").val(), proid: proid,speciid:id_speciid, payprice: price, u_num: num, u_name: name, u_phone: phone,u_idcard:idcard, u_traveldate:u_traveldate, comid: comid, buyuid: 0, tocomid: 0, province: province, city: city, address: address, code: code, order_remark: message, express: express, deliverytype: deliverytype,sid:serverid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#loading").hide();
                             alert(data.msg);
                             if(data.msg=="请完成你的未支付订单"){
                                 location.href="/h5/order/order.aspx";
                             }
                            return;
                        }
                        if (data.type == 100) {

                            if (paytype == "alipay") {

                                location.href = "/h5/pay_by/WebForm1.aspx?out_trade_no=" + data.msg + "&comid=" + comid;
                            } else if(paytype == "wx"){
                                $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenshiLinkAboutPay", { comid: comid, redirect_uri: "http://shop" + comid + ".etown.cn/wxpay/payment_" + data.msg + "_1.aspx" }, function (data1) {
                                    data1 = eval("(" + data1 + ")");
                                    if (data1.type == 1) {
                                        alert(data1.msg);
                                       // location.href="/h5/order/order.aspx";
                                        return;
                                    }
                                    if (data1.type == 100) {
                                        location.href = data1.msg;
                                    }
                                })
                            }else if(paytype == "webalipay"){
                             location.href = "/ui/vasui/pay.aspx?orderid=" + data.msg + "&comid=" + comid;
                            }
                            $("#loading").hide();
                            return;
                        }

                })
                
                }else{
                    //购物车提交订单
                    $.post("/JsonFactory/OrderHandler.ashx?oper=cartcreateorder", {Integral: Integral, Imprest: Imprest, openid: $("#hid_openid").val(), proid: cart_id,speciid:id_speciid, payprice: price, u_num: cart_num, u_name: name, u_phone: phone, u_traveldate: '<%=nowtoday %>', comid: comid, buyuid: 0, tocomid: 0, province: province, city: city, address: address, code: code, order_remark: message, express: express, deliverytype: deliverytype,userid:$("#hid_userid").val() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#loading").hide();
                            alert(data.msg);
                            //location.href="/h5/order/order.aspx";
                            return;
                        }
                        if (data.type == 100) {

                            if (paytype == "alipay") {

                                location.href = "/h5/pay_by/WebForm1.aspx?out_trade_no=" + data.msg + "&comid=" + comid;
                            } else if(paytype == "wx"){
                                $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenshiLinkAboutPay", { comid: comid, redirect_uri: "http://shop" + comid + ".etown.cn/wxpay/payment_" + data.msg + "_1.aspx" }, function (data1) {
                                    data1 = eval("(" + data1 + ")");
                                    if (data1.type == 1) {
                                        alert(data1.msg);
                                        //location.href="/h5/order/order.aspx";
                                        return;
                                    }
                                    if (data1.type == 100) {
                                        location.href = data1.msg;
                                    }
                                })
                            }else if(paytype == "webalipay"){
                             location.href = "/ui/vasui/pay.aspx?orderid=" + data.msg + "&comid=" + comid;
                            }
                            $("#loading").hide();
                            return;
                        }

                })
                
                }
            };


        });
    </script>
    <script type="text/javascript">
        //判断微信版本,微信版本5.0以上
        function isWeiXin() {
            var ua = window.navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == 'micromessenger' && parseFloat(navigator.appVersion) >= 5) {
                return true;
            } else {
                return false;
            }
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
        <div class="content confirm-container">
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
                                    <a href="/h5/order/pro.aspx?id=<%=pro_id %>">
                                        <h3>
                                            <%=pro_name %></h3>
                                    </a>
                                    <p class="c-gray ellipsis">
                                    </p>
                                </div>
                                <div class="right-col">
                                    <div class="price">
                                        ¥&nbsp;<span><%=price %></span></div>
                                    <div class="num">
                                        ×<span class="num-txt"><%=num %></span></div>
                                </div>
                            </div>
                        </div>
                        <hr class="margin-0 left-10" />
                        <div class="bottom" style=" padding-top:10px; padding-left:10px;">
                            商品总价：<span>¥&nbsp;<%=price * num%></span></div>
                        </div>
                        <div class="order-message clearfix" id="js-order-message">
                            <%if (isSetVisitDate == 1)
                              { %>
                             游玩日期：<input value="" class="dataNum dataIcon" readonly="readonly" isdate="yes" id="u_traveldate">

                            <%} %>
                        </div>
                        
                    <!-- 取货类型 -->
                    <div class="block No-Eticket" id="deliverytype">
                        <div class="" style="height: 20px;">
                                <label>
                                    <input name="deliverytype" class="selectdeliverytype" value="2" checked="checked" type="radio">
                                    快递配送</label>
                                <label>
                                    <input name="deliverytype" class="selectdeliverytype" value="4" type="radio">
                                    自提(请咨询商家)</label></div>
                        <div class="action-container">
                        </div>
                    </div>

                    <!-- 物流 -->
                    <div class="block express" id="js-logistics-container">
                        <div class="opt-wrapper" style="height: 20px;">
                            <a id="upaddress" href="#" class="btn btn-xxsmall btn-grayeee butn-edit-address js-edit-address">
                                编辑收货地址</a></div>
                        <div class="action-container" id="divaddress">
                        </div>
                    </div>
                    <!-- 支付 -->
                    <div class="js-step-topay ">
                        <div class="js-used-coupon  pg_upgrade_title" style="display: none;">
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
                        <div class="block">
                            <div class="js-order-total block-item order-total">
                                ￥<%=price * num%>
                                + <span id="expressfee">￥0.00运费</span><br>
                                <strong class="js-real-pay c-orange" id="total_price">合计：￥<%=price*num %>
                                </strong>
                            </div>
                        </div>
                        <div class="action-container" id="Div1">
                            <%if (bo == true)
                              {
                                  if (iswxsubscribenum == false)
                                  {%>
                            <div id="weixinpay" style="margin-bottom: 10px;">
                                <button id="wxpay" type="button" data-pay-type="umpay" class="btn-pay btn btn-block btn-large btn-umpay  btn-green">
                                    微信支付</button>
                            </div>
                                <%if (comid != 112 && comid != 1194 && comid != 2607)
                                  { %>
                                <div style="margin-bottom: 10px;">
                                    <button id="alipay" type="button" data-pay-type="baiduwap" class="btn-pay btn btn-block btn-large btn-baiduwap  btn-white">
                                        支付宝支付</button>
                                </div>
                                <div class="center action-tip js-pay-tip">
                                 如遇到屏蔽支付宝，请点击右上角 “在浏览器中打开”进行支付。单笔限额 50000元</div>
                                 <%} %>

                            <%}
                                  else { 
                                  %>
                               <div id="weixinpay" style="margin-bottom: 10px;">
                                <button id="wxpay" type="button" data-pay-type="umpay" class="btn-pay btn btn-block btn-large btn-umpay  btn-green">
                                    微信支付</button>
                                </div>
                                    <%if (comid != 112 && comid != 1194 && comid != 2607)
                                    { %>
                            <div style="margin-bottom: 10px;">
                                <button id="alipay" type="button" data-pay-type="baiduwap" class="btn-pay btn btn-block btn-large btn-baiduwap  btn-white">
                                    支付宝支付</button>
                            </div>
                            <div class="center action-tip js-pay-tip">
                             如遇到屏蔽支付宝，请点击右上角 “在浏览器中打开”进行支付。单笔限额 50000元</div>
                                     <%}else{ %>
                                     <div class="center action-tip js-pay-tip">
                                     请在微信购买，使用微信进行支付</div>

                                    <%} %>

                                  <%
                                  }
                                  }
                              else
                              { %>
                                    <%if (comid != 112 && comid != 1194 && comid != 2607)
                                    { %>
                            <div id="viewwebalipay" style="margin-bottom: 10px; ">
                                <button id="webalipay" type="button" data-pay-type="baiduwap" class="btn-pay btn btn-block btn-large btn-baiduwap  btn-green">
                                    支付宝支付</button>
                            </div>
                                     <%}else{ %>
                                     <div class="center action-tip js-pay-tip">
                                     请在微信购买，使用微信进行支付</div>
                                      <%} %>
                            <%} %>
                        </div>
                        <div class="center action-tip js-pay-tip">
                            支付完成后，如需退换货请及时联系卖家</div>
                        <div id="resvalues">
                        </div>
                    </div>
                </div>
                <div class="app-inner inner-order peerpay-gift address-fm" style=" display:none;
                    height: 200%;" id="sku-message-poppage">
                    <div class="js-list block block-list">
                    </div>
                    <div class="block" style="margin-bottom: 10px;">
                        <div class="block-item">
                            <label class="form-row form-text-row">
                                <em class="form-text-label">收货人</em> <span class="input-wrapper">
                                    <input id="in_name" name="in_name" class="form-text-input" value="" placeholder="名字"
                                        type="text"></span>
                            </label>
                        </div>
                        <div class="block-item">
                            <label class="form-row form-text-row">
                                <em class="form-text-label">联系电话</em> <span class="input-wrapper">
                                    <input id="in_phone" name="in_phone" class="form-text-input" value="" placeholder="手机或固话"
                                        type="tel"></span>
                            </label>
                        </div>
                        <div class="block-item" style=" display:<%if (issetidcard==0){%>none<%} %>">
                            <label class="form-row form-text-row">
                                <em class="form-text-label">身份证号码</em> <span class="input-wrapper">
                                    <input id="in_idcard" name="in_idcard" class="form-text-input" value="" placeholder="身份证号码"
                                        type="tel"></span>
                            </label>
                        </div>
                        <div class="block-item No-Eticket">
                            <div class="form-row form-text-row">
                                <em class="form-text-label">选择地区</em>
                                <div class="input-wrapper input-region js-area-select">
                                    <span>
                                        <select id="in_province" name="in_province" class="address-province" data-next-type="城市"
                                            data-next="city">
                                            <option data-code="" value="省份">省份</option>
                                        </select>
                                    </span><span>
                                        <select id="in_city" name="in_city" class="address-city" data-next-type="区县" data-next="county">
                                            <option data-code="0" value="城市">城市</option>
                                        </select>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="block-item No-Eticket">
                            <label class="form-row form-text-row">
                                <em class="form-text-label">详细地址</em> <span class="input-wrapper">
                                    <input id="in_address" name="in_address" class="form-text-input" value="" placeholder="街道门牌信息"
                                        type="text"></span>
                            </label>
                        </div>
                        <div class="block-item No-Eticket">
                            <label class="form-row form-text-row">
                                <em class="form-text-label">邮政编码</em> <span class="input-wrapper">
                                    <input id="in_code" maxlength="6" name="in_code" class="form-text-input" value=""
                                        placeholder="邮政编码" type="tel"></span>
                            </label>
                        </div>
                    </div>
                    <div>
                        <div class="action-container">
                            <a id="submitsave" class="js-address-save btn btn-block btn-blue">保存</a> <a id="submitcannel"
                                class="js-address-cancel btn btn-block">取消</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="js-self-fetch-modal" class="modal order-modal">
        </div>
        <div class="js-modal modal order-modal">
            <div class="js-scene-address-list scene">
                <div class="address-ui address-list">
                    <h4 class="list-title text-right">
                        <a class="js-cancel-address-list" href="javascript:;">取消</a></h4>
                    <div class="block">
                        <div class="js-address-container address-container">
                            <div style="min-height: 80px;" class="loading">
                            </div>
                        </div>
                        <div class="block-item">
                            <h4 class="js-add-address add-address">
                                增加收货地址</h4>
                        </div>
                    </div>
                </div>
            </div>
            <div class="js-scene-address-fm scene">
            </div>
        </div>
        <div class="footer">
            <!-- 商家公众微信号 -->
            <div class="copyright">
                <div class="ft-copyright">
                    <a href="#">易城商户平台技术支持</a>
                </div>
            </div>
        </div>
    </div>
    <div id="loading" class="loading" style="display: none; z-index:10000;">
        正在加载...
    </div>
    <script type="text/javascript">
        var province = document.getElementById('in_province');
        var city = document.getElementById('in_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_proid" type="hidden" value="<%=id %>" />
    <input id="hid_num" type="hidden" value="<%=num %>" />
    <input id="hid_price" value="<%=price %>" type="hidden" />
    <input id="hid_express" value="0" type="hidden" />
    <input id="hid_openid" value="<%=openid %>" type="hidden" />
    <input id="hid_name" value="" type="hidden" />
    <input id="hid_phone" value="" type="hidden" />
    <input id="hid_idcard" value="" type="hidden" />
    <input id="hid_province" value="" type="hidden" />
    <input id="hid_city" value="" type="hidden" />
    <input id="hid_address" value="" type="hidden" />
    <input id="hid_code" value="" type="hidden" />

    <input type="hidden" id="hid_Integral" value="<%=Integral %>" />
    <input type="hidden" id="hid_Imprest" value="<%=Imprest %>" />
    <input type="hidden" id="hid_In" value="0" />
    <input type="hidden" id="hid_Im" value="0" />
    <input type="hidden" id="hid_cart" value="<%=cart %>" />
    <input id="hid_cart_num" type="hidden" value="<%=cart_num %>" />
    <input id="hid_cart_id" type="hidden" value="<%=cart_id %>" />
    <input id="hid_id_speciid" type="hidden" value="<%=id_speciid %>" />
    <input id="hid_serverid" type="hidden" value="<%=serverid %>" />
    <input id="hid_issetidcard" type="hidden" value="<%=issetidcard %>" />

    <input id="hid_userid" type="hidden" value="<%=userid %>" />
</body>
</html>