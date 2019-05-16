<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pro.aspx.cs" Inherits="ETS2.WebApp.H5.order.Pro" %>

<!DOCTYPE html>
<html class="no-js " lang="zh-CN" >

<head>
    <meta charset="utf-8">
    <meta name="keywords" content="<%=title %>" />
    <meta name="description" content="" />
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">

    <title><%=pro_name %>    - <%=title %></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script> 
    <script src="/Scripts/jquery.cookie.2.2.0.min.js" type="text/javascript"></script> 
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/swipe.js"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>



    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?type=quick&ak=mKeOlGW2zgs8LAVf7ihHzSTD&v=1.0"></script>
<script type="text/javascript">
    $(function () {
            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker({
                    minDate: new Date(),
                    beforeShowDay: nationalDays ,
                    maxDate: new Date(<%=pro_end_str %>)
                });
            });
            
            var natDays = [ <%=nowork %>];  
  
            function nationalDays(date) {  
                for (i = 0; i < natDays.length; i++) {  
                   if (date.getFullYear() == natDays[i][2] && date.getMonth() == natDays[i][0] - 1 && date.getDate() == natDays[i][1]) {  
                        return [false, natDays[i][2] + '_day'];  
                   }  
                }  
                return [true, ''];  
            }  
            

        var comid = $("#hid_comid").val();
        var proid = $("#hid_proid").val();

        getmenubutton(comid, 'js-navmenu');

        
        var server_type=$("#hid_server_type").val();
        var bookpro_ispay=$("#hid_bookpro_ispay").val();

        if(server_type==12 ||server_type==13){
            $("#buy1").html("立即预约");
            $("#buy2").html("立即预约");
            $(".js-confirm-it").html("提交预约信息");
            $("#linkproject").removeClass("hide");
            $("#linkindex").addClass("hide");
            $("#right-icon").addClass("hide");

            $("#travlenum_view").addClass("hide");
            
            $("#travledate_view").removeClass("hide");
            $("#travlename_view").removeClass("hide");
            $("#travlephone_view").removeClass("hide");
            $("#travletime_view").removeClass("hide");
            

            $("#allmap").removeClass("hide");
                
        }



        //根据公司id获得关注作者信息
        $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
            dat = eval("(" + data + ")");
            if (dat.type == 1) {

            }
            if (dat.type == 100) {
                $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage  btn btn-follow\">关注我们</a>");
            }
        });

                //查询购物车数量
                $.post("/JsonFactory/OrderHandler.ashx?oper=agentsearchcart", { userid: $("#hid_userid").val(), comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                    }
                    if (data.type == 100) {
                        $("#right-icon").removeClass("hide");
                        $("#Num").html(data.msg);
                    }
                });


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
                $('.day_show').html(day + "天");
                $('.hour_show').html('<s id="h"></s>' + hour + '时');
                $('.minute_show').html('<s></s>' + minute + '分');
                $('.second_show').html('<s></s>' + second + '秒');
                intDiff--;
            }, 1000);
        }
        function qinggou() {
            $("#buy1").removeClass("btn-grey-dark");
            $("#buy2").removeClass("btn-grey-dark");
            $("#buy1").addClass("butn-qrcode");
            $("#buy2").addClass("btn-orange-dark");
            $("#buy1").html("火热抢购中..");
            $("#buy2").html("火热抢购中..");
            $("#hid_dinggou").val("1");
        }

        <%if (Ispanicbuy==1 ){ %>
         <%if (Limitbuytotalnum<=0){%>
            $("#buy1").removeClass("butn-qrcode");
            $("#buy2").removeClass("btn-orange-dark");
            $("#buy1").addClass("btn-grey-dark");
            $("#buy2").addClass("btn-grey-dark");
            $("#buy1").html("抢购已结束");
            $("#buy2").html("抢购已结束");

            $("#hid_dinggou").val("0");

         <% }else{%>
         
            <%
                if (panic_begintime <= nowtoday && nowtoday<panicbuy_endtime){//抢购在有效期范围内
             %>
                $("#buy1").html("火热抢购中");
                $("#buy2").html("火热抢购中");
                $("#hid_dinggou").val("1");
             <%
             }else if(panic_begintime>nowtoday){         
             %>

                   $("#hid_dinggou").val("0");
                   $("#buy1").removeClass("butn-qrcode");
                   $("#buy2").removeClass("btn-orange-dark");
                   $("#buy1").addClass("btn-grey-dark");
                   $("#buy2").addClass("btn-grey-dark");
                   $('#buy2').html('抢购 距开始还剩 <span id="day_show"></span><span class="hour_show"></span><span class="minute_show"></span><span class="second_show"></span>')
                   $('#buy1').html('抢购 距开始还剩 <span id="day_show"></span><span class="hour_show"></span><span class="minute_show"></span><span class="second_show"></span>')
                   var jishicount=$('#hid_jishicount').val();

                   var intDiff = parseInt(jishicount); //倒计时
                   timer(intDiff);
             <%
              }else{
             %>
                $("#buy1").removeClass("butn-qrcode");
                $("#buy2").removeClass("btn-orange-dark");
                $("#buy1").addClass("btn-grey-dark");
                $("#buy2").addClass("btn-grey-dark");
                $("#buy1").html("抢购已结束");
                $("#buy2").html("抢购已结束");
                $("#hid_dinggou").val("0");
              <%} 

              %>


        $(".js-buy-it").click(function () {
           if( server_type==10 || server_type==14){
                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val();
                var action = $("#hid_action").val();
                var speciid = $("#hid_speciid").val();
                var manyspeci = $("#hid_manyspeci").val();

                 location.href = "/h5/OrderEnter.aspx?id="+ proid+"&num=" + num+"&speciid="+speciid;
                 return;
            }else{
                var qianggou= $("#hid_dinggou").val();
                if(qianggou !=0){
                    $("#QJwxuFqolZ").show();
                    $(".js-confirm-it").html("下一步");
                    $("#hid_action").val("1");
                    $("#zhegai").show();
                }
            }
        });

        $(".js-qrcode-buy").click(function () {
           if( server_type==10 || server_type==14){
                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val();
                var action = $("#hid_action").val();
                var speciid = $("#hid_speciid").val();
                var manyspeci = $("#hid_manyspeci").val();

                 location.href = "/h5/OrderEnter.aspx?id="+ proid+"&num=" + num+"&speciid="+speciid;
                 return;
            }else{
                var qianggou= $("#hid_dinggou").val();
                if(qianggou !=0){
                    $("#QJwxuFqolZ").show();
                    $("#zhegai").show();
                }
            }
        });

        <%
        }
        }else{%>
         $(".js-buy-it").click(function () {
            if( server_type==10 || server_type==14){
                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val();
                var action = $("#hid_action").val();
                var speciid = $("#hid_speciid").val();
                var manyspeci = $("#hid_manyspeci").val();

                 location.href = "/h5/OrderEnter.aspx?id="+ proid+"&num=" + num+"&speciid="+speciid;
                 return;
            }else{
                $("#QJwxuFqolZ").show();
                $("#zhegai").show();
            }
        });

        $(".js-qrcode-buy").click(function () {
            if( server_type==10 || server_type==14){
                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val();
                var action = $("#hid_action").val();
                var speciid = $("#hid_speciid").val();
                var manyspeci = $("#hid_manyspeci").val();

                 location.href = "/h5/OrderEnter.aspx?id="+ proid+"&num=" + num+"&speciid="+speciid;
                 return;
            }else{
                $("#QJwxuFqolZ").show();
                $("#zhegai").show();
            }
        });
        <%} %>

        $("#zhegai").click(function () {
            if( server_type==10 || server_type==14){
                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val();
                var action = $("#hid_action").val();
                var speciid = $("#hid_speciid").val();
                var manyspeci = $("#hid_manyspeci").val();

                 location.href = "/h5/OrderEnter.aspx?id="+ proid+"&num=" + num+"&speciid="+speciid;
                 return;
            }else{
                $("#QJwxuFqolZ").hide();
                $("#zhegai").hide();
            }
        });




        $(".response-area-minus").click(function () {
            var num = parseInt($(".txt").val());
            if (num <= 1) {
                $(".txt").val(1);
            } else {
                $(".txt").val(num - 1);
            }
        });

        $(".response-area-plus").click(function () {
            var num = parseInt($(".txt").val());
            $(".txt").val(num + 1);
        });

        $(".js-confirm-it").click(function () {
            $("#loading").show();
            //预订产品 走独立的流程
            if(server_type==12 || server_type==13){
                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val();
                var action = $("#hid_action").val();

                var traveldate = $("#traveldate").val();
                var travelday = $("#travelday").val();
                var speciid = $("#hid_speciid").val();
                var manyspeci = $("#hid_manyspeci").val();
                var channelcoachid = $("#hid_channelcoachid").val();

                traveldate = traveldate +" "+ travelday +":00:00";

                var travelname = $("#travelname").val();
                var travelphone = $("#travelphone").val();

                if(traveldate ==""){
                    alert("请选择日期");
                    $("#loading").hide();
                    return;
                }
                if(travelname ==""){
                    alert("请填写预约人姓名");
                    $("#loading").hide();
                    return;
                }

                if(travelphone ==""){
                    alert("请填写预约人手机");
                    $("#loading").hide();
                    return;
                }
                
                //直接订购提交预订
                 $.post("/JsonFactory/OrderHandler.ashx?oper=createorder", {Integral: 0, Imprest: 0, openid: $("#hid_openid").val(), proid: proid,speciid:speciid, payprice: <%=price %>, u_num: num, u_name: travelname, u_phone: travelphone, u_traveldate: traveldate, comid: comid,channelcoachid:channelcoachid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#loading").hide();
                             alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            if(server_type==13){//教练产品
                                //跳转到一个 支付等待页面 需要
                                location.href = "/wxpay/micromart_orderpay.aspx?id=" + data.msg + "&comid=" + comid+"&md5="+data.md5;

                            }else{//预约产品
                                if(bookpro_ispay==1){//如需支付进入支付页面
                                    location.href = "/wxpay/micromart_orderpay.aspx?id=" + data.msg + "&comid=" + comid;
                                 }else{
                                    location.href = "yuyuesuc.aspx?id=" + data.msg + "&comid=" + comid+"&md5="+data.md5;
                                 }
                             }
                            $("#loading").hide();
                            return;
                        }

                })

            }
            else if( server_type==10 || server_type==14){
                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val();
                var action = $("#hid_action").val();
                var speciid = $("#hid_speciid").val();
                var manyspeci = $("#hid_manyspeci").val();

                 location.href = "/h5/OrderEnter.aspx?id="+ proid+"&num=" + num+"&speciid="+speciid;
                 return;
            }
            else if( server_type==9){
                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val(); 
                var action = $("#hid_action").val();
                var speciid = $("#hid_speciid").val();
                var manyspeci = $("#hid_manyspeci").val();

                location.href = "/h5/hotel/Hotelshow.aspx?proid="+ proid+"&num=" + num+"&speciid="+speciid+"&id="+comid;
                return;
            }
            else{ 
                

                var num = parseInt($(".txt").val());
                var proid = $("#hid_proid").val();
                var action = $("#hid_action").val();
                var speciid = $("#hid_speciid").val();
                var manyspeci = $("#hid_manyspeci").val();

                var serverid= $("#hid_serverid").val();

                //如果服务不为空，重新读取此，防止后退后提交
                if(serverid !=""){
                    serverid="";
                    $(".sys_item_spec .sys_item_specpara_server ul li").each(function () {
                    var selveri = $(this);
                    if($(this).hasClass("selected")){
						 serverid += $(this).attr("data-aid")+"A";
					    }
                    })

                }


                if(manyspeci==1){
                    if(speciid==0){
                      alert("请选择具体规格");
                        $("#loading").hide();
                        return;
                    }
                }



                //            window.location.href = "pay.aspx?num=" + num + "&id=" + proid;
                //            window.location.href = "/wxpay/micromart_pay_" + num + "_" + proid + ".aspx";
                var issetfinancepaytype = false;
                if ('<%=issetfinancepaytype %>' == 'True' || '<%=issetfinancepaytype %>' == 'true') {
                    issetfinancepaytype = true;
                }
                if(action=="1"){//直接订购
                    
                    
                    if (isWeiXin() == true && issetfinancepaytype == true) {
                        $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenshiLinkAboutPay", { comid: '<%=comid %>', redirect_uri: "http://shop" + comid + ".etown.cn/wxpay/micromart_pay_" + num + "_" + proid+"g"+speciid +"s"+serverid+ ".aspx" }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                //                        $.prompt("获取链接出错");
                                window.location.href = "/wxpay/micromart_pay_" + num + "_" + proid+"g"+speciid+"s"+serverid+ ".aspx";
                                return;
                            }
                            if (data.type == 100) {
                                window.location.href = data.msg;
                            }
                        })
                    } else {
                        window.location.href = "/wxpay/micromart_pay_" + num + "_" + proid +"g"+ speciid +"s" + serverid + ".aspx";
                    }
                }else{//加入购物车
                    $("#QJwxuFqolZ").hide();
                    $("#zhegai").hide();
                    $('#cartloading').show();; 
                     $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { userid: $("#hid_userid").val(), comid: comid, proid: proid,speciid:speciid, u_num:num }, function (data) {
                      data = eval("(" + data + ")");
                      $("#zhegai").hide();
                      if (data.type == 1) {
                      }
                      if (data.type == 100) {
                          $("#right-icon").removeClass("hide");
                      
                           //查询购物车数量
                      $.post("/JsonFactory/OrderHandler.ashx?oper=agentsearchcart", { userid: $("#hid_userid").val(), comid: comid }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                $('#cartloading').html("添加到购物车失败");
                                $('#cartloading').fadeOut(3000);
                            }
                            if (data.type == 100) {
                                $('#cartloading').html("成功添加到购物车");
                                $('#cartloading').fadeOut(3000);
                                $("#Num").html(data.msg);
                            }
                        })
                      }
                     })
              }
          }
        });

        $(".js-add-cart").click(function () {

            $("#QJwxuFqolZ").show();
            $(".js-confirm-it").html("加入购物车");
            $("#hid_action").val("0");
            $("#zhegai").show();

        });
        

        $(".js-cancel").click(function () {
            $("#QJwxuFqolZ").hide();
            $("#zhegai").hide();
        });

        //加载导航 ;
        $.ajax({
            type: "post",
            url: "/JsonFactory/ProductHandler.ashx?oper=getprochildimglist",
            data: { comid: comid, proid: proid },
            async: false,
            success: function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //$.prompt("查询错误");
                    //return;
                }
                if (data.type == 100) {
                    if (data.msg.length > 0) {
                        var daohanglist = "";
                        var tablist = "";
                        for (var i = 0; i < data.msg.length; i++) {
                            daohanglist = daohanglist + "<li><a  onclick=\"return false;\"><div><img class=\"goods-main-photo\" src=\"" + data.msg[i].imgurl + "\" style=\"max-height: 320px;\"/></div></a> </li>";
                            tablist += "<li></li>"
                        }
                        $(".list_font").append(daohanglist);
                        $(".list_tab").append(tablist);

                    }
                }
            }
        })

         <%if (Ispanicbuy==1 || Ispanicbuy==2){ %>
           <%if (Limitbuytotalnum<=0){ %>
                   $("#buy1").removeClass("js-qrcode-buy ");
                   $("#buy1").removeClass("butn-qrcode");
                   $("#buy1").removeClass("btn-orange-dark");
                   $("#buy2").removeClass("js-buy-it");
                   $("#buy2").removeClass("btn-orange-dark");
                   

                   $("#buy1").addClass("btn-orange-dark-shouqing");
                   $("#buy2").addClass("butn-qrcode-shouqing");

                   $("#buy1").html("商品已售罄");
                   $("#buy2").html("商品已售罄");

            <%}%>
         <%}%>


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
                               //$(".sys_item_spec").empty();

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

         $("#traveldate").change(function(){ 
            getworktime($(this).val());
         }); 

    })


    function getworktime(date){
         //查询日期
          $("#loading").show();
               $.post("/JsonFactory/UserHandle.ashx?oper=Getjiaolianworktime", {id:<%=MasterId %>,date:date }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#loading").hide();
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            $("#travelday").empty();


                            if(data.msg !=""){
                                var workh=data.msg;
                                var workh_arr=workh.split(",");
                                for(var i=0;i<workh_arr.length ;i++ ){
                                    if(workh_arr[i] !=""){
                                        $("#travelday").append("<option value='"+workh_arr[i]+"'>"+workh_arr[i]+"点</option>");
                                    }
                                }
                            }
                             return;
                        }
                })
    }


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
     <!-- 页面样式表 -->
    <style type="text/css">

        .ui-datepicker  
        {
             background-color: #ffffff !important;
            margin-left: -20px;
            width:200px !important;
            }
        .ui-datepicker-other-month
        {
             background-color: #ffffff !important;
            }
            
            
            .avatar {
    width: 70px;
    height: 70px;
    margin: 0px auto 5px !important;
}
.avatar span {
    border: 1px solid #C6BFB9 !important;
}
.block.block-list + .block.block-list {
    margin-top: 2px !important;
}
.sku-layout .layout-content .goods-models > dl {
    padding: 2px 0px 2px !important;
}
    </style>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    
    <!-- CSS -->
    <link onerror="_cdnFallback(this)" href="css/css1.css" rel="stylesheet">    
    <link onerror="_cdnFallback(this)" href="css/css3.css" rel="stylesheet">
	<link onerror="_cdnFallback(this)" href="css/css.css" rel="stylesheet">    
    <link onerror="_cdnFallback(this)" href="css/base.css" rel="stylesheet">   
        <link  href="css/bottommenu.css" rel="stylesheet">  
</head>
<body style=" margin-bottom:40px;" >
        <!-- container -->
    <div class="container wap-goods internal-purchase">
        <div class="header">
        <!-- ▼顶部通栏 -->
                                
            <div class="js-mp-info share-mp-info">
            <a class="page-mp-info" href="default.aspx">
                <img class="mp-image" width="24" height="24" src="<%=logoimg %>"/>
                <i class="mp-nickname">
                    <%=title %>                </i>
            </a>
           <div class="links">
                
                                                                </div>
        </div>
        <!-- ▲顶部通栏 -->
</div>        <div class="content ">
        <div class="content-body">
    <!-- 分享文案 -->
    <span id="wxdesc" class="hide"></span>
    
    <div class="js-image-swiper custom-image-swiper custom-goods-swiper custom-image-swiper-single">
        <div class="swiper-container-img" style="height: 320px;">
        <div class="swiper-wrapper" style="-webkit-transform:translate3d(0,0,0);">
                        <div class="swiper-slide box_swipe" id="banner_box" style="visibility: visible;">
                         <%if (Ispanicbuy==1 || Ispanicbuy==2){ %>
                            <%if (Limitbuytotalnum<=0){ %>
                        <div class="Soldout"><img  src="image/5337034_100415061309_2.gif"  style="display: block;width:120px;padding-top:5px;padding-left:5px;"></div>
                             <%}%>
                        <%}%>

                           <ul class="list_font">
                                <li> <a class="js-no-follow"  onclick="return false;" style="height: 320px;"><img class="goods-main-photo" src="<%=imgurl %>" style="" /></a></li>
                           </ul>
                           <ol class="list_tab">
                           <li class="on"></li>
                           </ol>
            </div>
                    </div>
    </div>

    </div>                    <div class="goods-header">
            <h2 class="title"><%=pro_name %></h2>
                        <div class="goods-price ">
                                    <div class="current-price">
                        <span>￥&nbsp;</span><i class="js-goods-price price"><script  type="text/javascript">                                                                                ViewPrice_html(<%=price %>);</script></i>
                    </div>
                                    <div class="original-price">
                                    <%if (face_price != 0)
                                      { %>
                                     &nbsp;<span>￥&nbsp;</span><i class="js-goods-price price"><script  type="text/javascript">                                                                                                   ViewPrice_html(<%=face_price%>);</script></i>
                                     <%} %>
                                            </div>
                            </div>
        </div>
                                
                                    
        <hr class="with-margin" />
    <div class="sku-detail adv-opts" style="border-top:none;">
        <div class="sku-detail-inner adv-opts-inner">
        <%if (Server_type == 1)
          { %>
            <dl>
                <dt>有效期：</dt>
                <dd><%=pro_youxiaoqi%> (<%=youxianshiduan %>)</dd>
            </dl>
            <%} %>
        </div>
        <div class="qrcode-buy">
         <%if (view_phone == 1 )
             {//预订产品都显示电话预订 %>
             <a href="tel:<%=bindphone %>" id="tel2" class="btn btn-block btn-orange-dark butn-qrcode""><img alt="" src="/Images/phone-flat.png" style="height: 20px;vertical-align:bottom;">电话咨询</a>
           <%} %>

           <%if (view_phone == 0 || view_phone == 2 || view_phone == 3)
             {//等于0或等于2 则判断为 非预订产品或预订产品绑定人与渠道相同 %>
            <a  id="buy1" href="javascript:;" class="js-qrcode-buy btn btn-block btn-orange-dark butn-qrcode">立即购买</a>
           <%} %>


            


        </div>
    </div>
        <div class="js-components-container components-container">
        
	<div id="linkindex" class="custom-store">
        <a class="custom-store-link clearfix" href="Default.aspx">
            <div class="custom-store-img"> <img class="mp-image" width="24" height="24" src="<%=logoimg %>"/></div>
            <div class="custom-store-name"><%=title %></div>
            <span class="custom-store-enter">进入店铺</span>
        </a>
    </div>
    <div id="linkproject" class="custom-store hide">
        <a class="custom-store-link clearfix" href="list.aspx?projectid=<%=projectid %>">
            <div class="custom-store-img"> <img class="mp-image" width="24" height="24" src="<%=logoimg %>"/></div>
            <div class="custom-store-name">查看项目信息：<%=projectname %></div>

        </a>
    </div>
    <%if (Coordinate != "")
      { %>
    <div id="mapdaohang" class="custom-store">
        <a class="custom-store-link clearfix" href="javascript:;" onclick="DRIVINGdaohang();">
            <div class="custom-store-img"> <img class="mp-image" src="/images/red_marker.jpg"  width="18" ></div>
            <div class="custom-store-name">地址：<%=Address%></div>
            <span class="custom-store-enter"> 进入导航 </span>
        </a>
    </div>
    <div id="allmap" class="" style=" width:100%; height:130px; display:none;"></div>
    <script type="text/javascript">
        // 百度地图API功能
        var map = new BMap.Map("allmap");
        map.centerAndZoom(new BMap.Point(<%=Coordinate %>), 13);

        map.addControl(new BMap.ZoomControl());  //添加地图缩放控件
        var marker1 = new BMap.Marker(new BMap.Point(<%=Coordinate %>));  //创建标注
        map.addOverlay(marker1);                 // 将标注添加到地图中
        map.disableDragging();


        //创建信息窗口
//        var infoWindow1 = new BMap.InfoWindow("<%=projectname %> ");
//        marker1.addEventListener("click", function () { this.openInfoWindow(infoWindow1); });

         function DRIVINGdaohang() {
                            var start = {
	                             name:""
	                        }
	                        var end = {
	                           name:"<%=Address%>",latlng:new BMap.Point(<%=Coordinate %>)
	                        }
	                        var opts = {
	                            mode:BMAP_MODE_DRIVING,
	                            region:""
	                        }
	                        var ss = new BMap.RouteSearch();
	                        ss.routeCall(start,end,opts);
        }

      </script>

      <%} %>
 <div class="custom-recommend-goods js-custom-recommend-goods hide clearfix">
    <div class="custom-recommend-goods-title">
        <a class="js-custom-recommend-goods-link" href="">推荐商品</a>
    </div>
     <ul class="custom-recommend-goods-list js-custom-recommend-goods-list clearfix">
    </ul>
</div>


<!-- 富文本内容区域 -->
<div class="custom-richtext">
<%=sumaryend%></div>

    
                                
    </div>
        <div class="js-bottom-opts bottom-opts">
                                    
<!-- 购买链接 -->
            
           <%if (view_phone == 1 || view_phone == 2 || view_phone == 3)
             {//预订产品都显示电话预订 %>
                 <%if (channleimg != "")//检测如果有顾问 头像 则显示
                 { %>
                <div class="avatar">

                    <span style="background-image: url(<%=channleimg%>)"></span>
                </div>
                <%} %>

                <%if (view_phone != 3)
                  { %>
                 <a href="tel:<%=bindphone %>" id="tel" class="btn btn-orange-dark btn-2-1"><img alt="" src="/Images/phone-flat.png" style="height: 20px;vertical-align:bottom;">电话咨询</a>
                <%} %>

           <%} %>

           <%if (view_phone == 0 || view_phone == 2 || view_phone == 3)
             {//等于0或等于2 则判断为 非预订产品或预订产品绑定人与渠道相同 %>
            <a href="javascript:;" id="buy2" class="js-buy-it btn btn-orange-dark btn-2-1">立即购买</a>
           <%} %>


    
            <%if (pro_servertype == 11 && Ispanicbuy !=1)//实物并且不是抢购产品，抢购只能单买
              { %>
            <!-- 购物车按钮 -->
            <a href="javascript:;" id="cartadd" style=" color:#333;" class="js-add-cart btn btn-2-1">加入购物车</a>
                <!--<a href="Cart.aspx" id="cartNum" style="vertical-align:middle;">
                <span style=" padding:0 0 5px 0; " ><img src="/Images/cart.jpg" alt="" width="26" style="vertical-align: bottom; padding-right:1px;" /><em id="Num"></em></span></a>-->
            <%} %>
           </div>
											

</div>
<div class="modal modal-login js-modal-login">
    <div class="account-form login-form">
    <form class="js-login-form " method="GET" action="login.aspx">
                <h2 class="js-login-title form-title big">
            <span>
                            请先登录账号
            </span>
        </h2>
        <!-- 表单主体 -->
        <ul class="block block-form margin-bottom-normal">
            <li class="block-item">
                <label for="phone">手机号码</label>
                <input id="phone" type="tel" name="phone" maxlength="11" class="js-login-phone" autocomplete="off" placeholder="请输入您的手机号" value=""/>
            </li>
            <li class="block-item">
                <label for="password">登录密码</label>
                <input id="password" type="password" name="password" autocomplete="off" maxlength="20" class="js-login-pwd" placeholder="请填写您的密码" />
            </li>
            <li class="relative block-item js-auth-code-li auth-hide">
                <label for="code">验证码</label>
                <input id="code" type="tel" name="code" class="js-auth-code" placeholder="请输入短信验证码" maxlength="6"/>
                <button type="button" class="btn btn-green btn-auth-code font-size-12 js-auth-code-btn" data-text="获取验证码">
                    获取验证码
                </button>
            </li>
        </ul>
        <div class="action-container">
            <button type="submit" class="js-submit btn btn-green btn-block" disabled>
                            登录
                        </button>
                        <button type="button" class="js-login-cancel btn btn-block btn-white" >返回</button>
                    </div>
        <div class="action-links">
            <p class="center">
                                    <a class="js-login-mode c-blue" data-login-mode="signup" href="javascript:;">注册账号</a>
                                <span class="division-span"></span>
                <a href="#changepassword" class="js-forget-password forget-password">忘记密码</a>
            </p>
        </div>
    </form>
</div>
</div>                       		
	    <div class="content-sidebar">
		            <a href="Default.aspx" class="link">
		                <div class="sidebar-section shop-card">
		                    <div class="table-cell">
		                       <img src="<%=logoimg %>" class="shop-img" alt="公众号头像" height="60" width="60">
		                    </div>
		                    <div class="table-cell">
		                        <p class="shop-name">
		                        	<%=title %>		                        			                       
                                </p>
		                    </div>
		                </div>
		            </a>
		               <div class="sidebar-section shop-info">
		                    <div class="section-detail">
		                         <p class="shop-detail"><%=Scenic_intro %></p>
		                        	<p class="text-center weixin-title">微信“扫一扫”立即关注</p>
			                        <div class="js-follow-qrcode text-center qr-code">
			                        <img width="158" height="158" src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>">
                                    </div>
			                        <p class="text-center weixin-no">微信号：<%=weixinname %></p>
			                </div>
		               </div>		    		        		    	    
          </div>
   	
    
                    </div>        <div class="js-footer" style="min-height: 1px;">
            
    <div class="footer">
        <div class="copyright">
                            <div class="ft-links">
                    <span id="copydaohang"></span>
                    <span class="links"></span>
                                                        </div>
                        <div class="ft-copyright">
<a href="#">易城商户平台技术支持</a>
</div>
 
        </div>
    </div>
        </div> 
     </div>

               
    <div id="zhegai" style="z-index: 1009;position:absolute;left:0;top:0; bottom:0;  width:100%; height:1000%; filter:alpha(Opacity=80);-moz-opacity:0.9;opacity: 0.9; display:none; background:#000000;" ></div>      
    <div id="QJwxuFqolZ" class="sku-layout sku-box-shadow" style="overflow: hidden; visibility: visible; opacity: 1; bottom: 0px; left: 0px; right: 0px; transform: translate3d(0px, 0px, 0px); position: fixed; z-index: 1100; transition: all 300ms ease 0s;display:none;">
        <div class="layout-title sku-box-shadow name-card sku-name-card">
            <div class="thumb">
            <%if (Server_type == 13 && channleimg !="")
              {%>
              <img src="<%=channleimg %>" alt="<%=bindname %>">
              <%}else{ %>
            <img src="<%=imgurl %>" alt="">
            <%} %>
            
            </div>
            <div class="detail goods-base-info clearfix">
                <p class="title c-black ellipsis"><%=pro_name %> 
                 <%if (Server_type == 13)
                   {%>
                   教练：<%=bindname %>
                   
                   <%} %>
                
                </p>
                <div class="goods-price clearfix">
                    <div class="current-price c-black pull-left">
                        <span class="price-name pull-left font-size-14 c-orangef60">￥</span>
                        <i class="js-goods-price price font-size-18 c-orangef60 vertical-middle sys_item_price">
                        <script type="text/javascript">
                            ViewPrice_html('<%=price %>');</script>
                        </i>
                        <i style=" display:none;" class="sys_item_mktprice"></i>
                        <i style=" display:none; color: #555; font-size:12px;font-weight: inherit;" class="serverfuwu" ></i>
                    </div>
                </div>
            </div> 
            <div class="js-cancel sku-cancel">
                <div class="cancel-img"></div>
            </div>
    </div>
    
    <div   style="height:; " class="adv-opts layout-content">
        <div id="travlenum_view" class="goods-models js-sku-views block block-list block-border-top-none">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>数量</label>
		</dt>
		<dd>
		<dl class="clearfix">
				<div class="quantity">
				<div class="response-area response-area-minus"></div>            
				<button disabled="disabled" class="minus " type="button"></button>            
				<input id="number" class="txt" value="1" type="number" />            
				<button class="plus" type="button"></button>            
				<div class="response-area response-area-plus"></div>            
				<div class="txtCover"></div>        </div>
				<div class="stock pull-right font-size-12" />
				<!--<dt class="model-title stock-label pull-left">
			        <label>剩余: </label>
			        </dt>
			        <dd class="stock-num">
				        10
			     </dd>-->
			
			</div>
		</dl>
		</dd></dl>


		</div>
        <%if (manyspeci == 1 || Server_type == 13 || Wrentserver==1)//多规格，教练预约，绑定服务 都显示出多规格选项
          {//当多规格，显示规格 %>

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
        <div id="travledate_view" class="goods-models js-sku-views block block-list block-border-top-none hide">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>预约日期</label>
		</dt>

        <dl class="clearfix">
				<div class="quantity">

				<input name="traveldate" id="traveldate" class="txtinputsub" value=""  readonly="readonly" placeholder="日期" isdate="yes" type="text" /> 
			</div>
		</dl>
		</dd>

		</dl>


		</div>

        <div id="travletime_view" class="goods-models js-sku-views block block-list block-border-top-none hide">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>预约时间</label>
		</dt>

        <dl class="clearfix">
				<div class="quantity" >
				         <select id="travelday" class="txtinputsub" style="height:30px; width:123px; " >
                         <%if (workh != "")
                           {
                           %>
                           <%=workh %>
                           <%}
                           else
                           { %>
                            <option value="7">7点</option>
                            <option value="8">8点</option> 
                            <option value="9" selected>9点</option> 
                            <option value="10">10点</option> 
                            <option value="11">11点</option> 
                            <option value="12">12点</option> 
                            <option value="13">13点</option> 
                            <option value="14">14点</option> 
                            <option value="15">15点</option> 
                            <option value="16">16点</option> 
                            <option value="17">17点</option> 
                            <option value="18">18点</option> 
                            <option value="19">19点</option> 
                            <option value="20">20点</option> 
                            <option value="21">21点</option> 
                            <%} %>
                 </select>         
			</div>
		</dl>
		</dd>

		</dl>


		</div>

        <div id="travlename_view" class="goods-models js-sku-views block block-list block-border-top-none hide">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>预约人姓名</label>
		</dt>

        <dl class="clearfix">
				<div class="quantity">

				<input id="travelname" class="txtinputsub" value="" type="text" placeholder="姓名" />            
			</div>
		</dl>
		</dd>

		</dl>


		</div>

        <div id="travlephone_view" class="goods-models js-sku-views block block-list block-border-top-none hide">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>预约人手机</label>
		</dt>

        <dl class="clearfix">
				<div class="quantity">

				<input id="travelphone" class="txtinputsub" value="" type="tel"  placeholder="手机" />            
			</div>
		</dl>
		</dd>

		</dl>


		</div>


        <div class="confirm-action content-foot">
    <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark">下一步</a>
	</div>
    </div>
</div>  
                
                


   <div id="right-icon" class="icon-hide   no-border hide" data-count = "1">
	<div class="right-icon-container clearfix" style="width: 50px">
		
		<a id="global-cart" href="Cart.aspx" class="no-text" style="background-image: url(image/s0.png);
							background-size: 50px 50px;
							background-position: center;">
			<p class="right-icon-img"></p>
			<p class="right-icon-txt">购物车</p>
		</a>
	</div>
</div>
<div id='cart' style=" display:none;position: absolute; bottom: 6em; right: 2em; width: 55px; height:55px; background-color: #FFFAFA; margin:10px; border-radius:60px; border: solid rgb(55,55,55)  #FF0000   1px;cursor:pointer;"><a href="Cart.aspx"><img src="/images/cart.gif" width="39" style="padding:8px 0 0 9px;"/></a></div>
   

<div id="loading" class="loading" style="display: none; z-index:1111">
            正在加载...
        </div>
<div id="cartloading" class="loading" style="display:;bottom: 220px;">
            已成功添加到购物车
        </div>

        
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

        <!-- js在最后 -->
<script>
    $(function () {
        new Swipe(document.getElementById('banner_box'), {
            speed: 500,
            auto: 3000,
            callback: function () {
                var lis = $(this.element).next("ol").children();
                lis.removeClass("on").eq(this.index).addClass("on");
            }
        });
    });
	</script>
    <script type="text/javascript" src="/Scripts/hilove.js"></script>
<input id="hid_proid" value="<%=id %>" type="hidden"  />
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
    
    <input id="hid_price" value="<%=price %>" type="hidden"  />
    <input id="hid_action" value="1" type="hidden"  />

    <input id="hid_jishicount" value="<%=shijiacha %>" type="hidden"  />
    <input id="hid_dinggou" value="0" type="hidden"  />
    
    <input id="hid_server_type" value="<%=Server_type %>" type="hidden"  />
    <input id="hid_bookpro_ispay" value="<%=bookpro_ispay %>" type="hidden"  />
    <input id="hid_manyspeci" value="<%=manyspeci %>" type="hidden"  />
    <input id="hid_channelcoachid" value="<%=channelcoachid %>" type="hidden"  />
    <input id="hid_speciid" value="0" type="hidden"  />
	<input id="hid_serverid" value="" type="hidden">
	<input id="hid_server_price" value="0" type="hidden">
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
                var _mktprice=0;
                var _price=0;
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
                $(_resp.price).text(Number(_price).toFixed(2));
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



    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

//            //分享  
//            $.ppkWeiShare({
//                path: location.href,
//                image: "<%=imgurl %>",
//                desc: "<%=remark %>",
//                title: ' <%=pro_name %>'
//            });
        });
    </script>
    </body>
</html>