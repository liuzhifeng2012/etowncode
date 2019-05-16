<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="micromart_orderpay.aspx.cs" Inherits="ETS2.WebApp.wxpay.micromart_orderpay" %>

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
        待付款的订单</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.cookie.2.2.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/weixin/CryptoJS/components/core-min.js"></script>
    <script type="text/javascript" src="/Scripts/weixin/CryptoJS/rollups/sha1.js"></script>
    <script type="text/javascript">
        $(function () {
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


            //判断购物车产品，进行加载
                var cart = $("#hid_cart").val();
                var cartid = $("#hid_cartid").val();
                
            var userid=$("#hid_userid").val();
            var proid_temp = $("#hid_proid").val();;

            if(cart == "1"){
            //装载产品列表
            $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=userorderprolist",
                    data: { cartid: cartid, comid: comid, proid: proid_temp },
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

            
             

             

            function createorder(paytype) {
                $("#loading").show();
                var comid = $("#hid_comid").val();
                var id = $("#hid_id").val();
              

              <%if(Server_type==13){ //如果是教练订单，需要 商家确认 %>
                 searchpaystate();

                 if($("#hid_paystate").val()==0){
                    $("#jiaolianloading").html("等待商家确认，稍等..！");
                    $("#jiaolianloading").show();
                     $("#loading").hide();
                    return ;
                 }

                 if($("#hid_paystate").val()==2){
                    $("#jiaolianloading").html("订单已支付！");
                    $("#jiaolianloading").show();
                      $("#loading").hide();
                    return ;
                 }

              <%} %>

                            if (paytype == "alipay") {

                                location.href = "/h5/pay_by/WebForm1.aspx?out_trade_no=" + id + "&comid=" + comid;
                            } else if(paytype == "wx"){
                                $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetMenshiLinkAboutPay", { comid: comid, redirect_uri: "http://shop" + comid + ".etown.cn/wxpay/payment_" + id + "_1.aspx" }, function (data1) {
                                    data1 = eval("(" + data1 + ")");
                                    if (data1.type == 1) {
                                        alert(data1.msg);
                                        return;
                                    }
                                    if (data1.type == 100) {
                                        location.href = data1.msg;
                                    }
                                })
                            }else if(paytype == "webalipay"){
                             location.href = "/ui/vasui/pay.aspx?orderid=" + id + "&comid=" + comid;
                            }
                            $("#loading").hide();
                            return;
            };

             <%if(Server_type==13){ //如果是教练订单，需要 商家确认 %>
                 if($("#hid_paystate").val()==0){
                     searchpaystate();
                     setInterval("searchpaystate()",10000); 
                 }
            <%} %>

            <%if (paystate==2){ %>
                $("#jiaolianloading").html("订单已支付");
                $("#jiaolianloading").show();
                
            <%} %>

        });


        function searchpaystate() {

              if($("#hid_paystate").val() !=0){
                          $("#jiaolianloading").html("教练已经确认，您可以进行支付了！");
                          $("#jiaolianloading").hide();
                           $("#loading").hide();
                          return;
              }

             $.post("/JsonFactory/OrderHandler.ashx?oper=GetorderPaystate", { comid: $("#hid_comid").val(),id:$("#hid_id").val() }, function (data1) {
                      data1 = eval("(" + data1 + ")");
                      if (data1.type == 1) {
                           if(data1.msg==-1){
                              alert("您预订的时间教练忙，此订单被商家取消，请重新提交");
                              location.href = "/h5/peoplelist.aspx";
                           }else{
                              $("#jiaolianloading").html("等待教练为您确认，请稍等..！");
                              $("#jiaolianloading").show();
                               $("#loading").hide();
                              return;
                          }
                      }
                      if (data1.type == 100) {
                          $("#hid_paystate").val(data1.msg);
                          $("#jiaolianloading").hide();    
                          alert("教练已经确认，您可以进行支付了");
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
        <style>
.jiaolianloading {
    height: 100px;
    line-height: 80px;
    position: fixed;
    left: 10% ;
    bottom: 50px;
    width: 80% ;
    background: rgba(0, 0, 0, 0.7) none repeat scroll 0% 0%;
    text-align: center;
    font-size: 16px;
    color: #FFF;
    border-radius: 5px;
    z-index: 99;
    display: none;
    top: 40%;
}

    </style>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- CSS -->
    <link rel="stylesheet" href="/h5/order/css/css4.css" onerror="_cdnFallback(this)" />
    <link rel="stylesheet" href="/h5/order/css/css1.css" onerror="_cdnFallback(this)" />
</head>
<body class=" ">
    <!-- container -->
    <div class="container js-page-content wap-page-order">
        <div class="content confirm-container">
            <div class="app app-order">
                <div class="app-inner inner-order" id="js-page-content">
                    <!-- 通知 -->
                    <!-- 商品列表 -->
                    <div class=" block-order block-border-top-none">
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
                            <%if (order_state == 1)
      { %>
    <div class="important-message important-message-order">
			<!-- 客户看 -->
			<h3>订单状态：<%= orderstatus%>							</h3>
			<p>您于<%=subtime%>下的订单</p>
            <p><%if (Ispanicbuy == 0)
                   {%>请在24小时<%}
                   else
                   { %>抢购产品请在30分钟<%} %>付款，超时订单将自动取消。</p>

	</div><%} %>
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
                                        ¥&nbsp;<span><%=pay_price%></span></div>
                                    <div class="num">
                                        ×<span class="num-txt"><%=num %></span></div>
                                </div>
                            </div>
                        </div>

                       
                    </div>
                    <!-- 物流 -->
                    <div class="block express" id="js-logistics-container">
                        <div class="opt-wrapper" style="height: 20px;">
                            <!-- <a id="upaddress" href="javascript:;" class="btn btn-xxsmall btn-grayeee butn-edit-address js-edit-address">
                                编辑收货地址</a></div>-->
                        <div class="action-container" >
                        
                        </div>
                    </div>
                    <!-- 支付 -->
                    <div class="js-step-topay ">
                        <div class="js-used-coupon block pg_upgrade_title" style="display: none;">
                        <div class="action-container" >
                        <%=address%>
                        </div>
                        </div>
                        <div class="block">
                            <div class="js-order-total block-item order-total">
                                ￥<%=pro_price%>
                                + <span id="expressfee">￥<%=Express %>运费</span>
                                
                                <br>
                                <strong class="js-real-pay c-orange" id="total_price">支付金额：￥<%=price%>
                                </strong>
                            </div>
                        </div>
                        <div class="action-container" id="Div1">
                            <%if (bo == true)
                              { %>
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
                              else
                              { %>
                               <%if (comid != 112 && comid != 1194 && comid != 2607)
                                 { %>
                            <div id="viewwebalipay" style="margin-bottom: 10px; ">
                                <button id="webalipay" type="button" data-pay-type="baiduwap" class="btn-pay btn btn-block btn-large btn-baiduwap  btn-green">
                                    支付宝支付</button>
                            </div>
                            <%}
                                 else
                                 { %>
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
                <div class="app-inner inner-order peerpay-gift address-fm" style="display: none;
                    height: 100%;" id="sku-message-poppage">
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

    <div id="jiaolianloading" class="jiaolianloading" style="display: none; z-index:10000;">
        正在加载...
    </div>

    
    <script type="text/javascript">
        var province = document.getElementById('in_province');
        var city = document.getElementById('in_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_num" type="hidden" value="<%=num %>" />
    <input id="hid_price" value="<%=price %>" type="hidden" />
    <input id="hid_express" value="0" type="hidden" />
    <input id="hid_openid" value="<%=openid %>" type="hidden" />
    <input id="hid_name" value="" type="hidden" />
    <input id="hid_phone" value="" type="hidden" />
    <input id="hid_province" value="" type="hidden" />
    <input id="hid_city" value="" type="hidden" />
    <input id="hid_address" value="" type="hidden" />
    <input id="hid_code" value="" type="hidden" />
    <input type="hidden" id="hid_In" value="0" />
    <input type="hidden" id="hid_Im" value="0" />
    <input id="hid_id" type="hidden" value="<%=id %>" />
    <input id="hid_cartid" type="hidden" value="<%=cart_id %>" />
    
    <input id="hid_proid" type="hidden" value="<%=proid %>" />
    <input id="hid_cart" type="hidden" value="<%=cart %>" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
    <input id="hid_paystate" type="hidden" value="<%=paystate %>" />
</body>
</html>