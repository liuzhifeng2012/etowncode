<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/H5/Order.Master" CodeBehind="Orderserver.aspx.cs" Inherits="ETS2.WebApp.H5.Orderserver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 页面样式表 -->
    <style type="text/css">
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
    </style>
    <!-- 页面样式表 -->
    <link href="../Styles/H5/Order.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-orderrili.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link onerror="_cdnFallback(this)" href="/h5/order/css/base.css" rel="stylesheet">

    
    <script type="text/javascript">
        $(function () {
        var comid = $("#hid_comid").val();
        <%if(err==1){ %>
                alert("<%=errtext %>>");
                window.location.href = "/yy/buyserver.aspx";
        <%}else{ %>
            $.ajax({
                        type: "post",
                        url: "/JsonFactory/ProductHandler.ashx?oper=SelectServerlist",
                        data: { comid: comid,proid: $("#hid_proid").val(),pno:"<%=pno %>"  },
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
        
        $("#submitBtn1").click(function () {
          
                $("#loading").hide();
               
//               $(this).addClass("disabled");

                var comid = $("#hid_comid").val();
                var id = $("#wxid").val();
                var ordertype = 2;
                var payprice = $("#amount").val();

                var serverid= $("#hid_serverid").val();
                var name= $("#hid_name").val();
                var phone= $("#hid_phone").val();

                //如果服务不为空，重新读取此，防止后退后提交
                if(serverid !=""){
                    serverid="";
                    $(".sys_item_spec .sys_item_specpara_server ul li").each(function () {
                    var selveri = $(this);
                    if($(this).hasClass("selected")){
						 serverid += $(this).attr("data-aid")+",";
					    }
                    })

                }
                

                $("#submitBtn1").val("提交中..");
                //$("#submitBtn1").attr("disabled", "disabled");


                //提交预订
                $.post("/JsonFactory/OrderHandler.ashx?oper=Recharge", { u_name: name, u_phone: phone, comid: comid,pno:'<%=pno %>',sid:serverid  }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#loading").hide();
                        alert(data.msg);
                        $('#submitBtn1').removeAttr("disabled");
                        return;
                    }
                    if (data.type == 100) {
                        location.href = "pay.aspx?orderid=" + data.msg + "&comid=" + comid;
                        $("#loading").hide();
                        return;
                    }

                })

            })
            
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
                if (!/^1[3,4,5,8]\d{9}$/i.test($("#" + f).val())) {
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

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <script type="text/javascript">
        var _tcopentime = new Date().getTime();
        var _hmt = _hmt || [];
    </script>
    <script type="application/x-javascript">
        addEventListener('DOMContentLoaded',function(){setTimeout(function(){scrollTo(0,1);},0);},false);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="inner">
        <!-- 公共页头  -->
        <header class="header" style="background-color: #3CAFDC;">

      <h1 style=" text-align:center;">在线自助购买押金与服务</h1>
      <div class="left-head">
          <a href="/YY/buyserver.aspx" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>


    </header>
        <!-- 页面内容块 -->
        <div id="orderFome" class="order-form">
            <div class="t-head">
                <div class="order-price" id="priceInfo">
                    在线自助购买押金与服务
                </div>
            </div>
            <div class="container">
                <div class="w-item conlist">
                    <dl class="fn-clear">
                        <dt></dt>
                        <dd id="sName">
                            <%=title%></dd>
                    </dl>
                </div>
                <div class="w-item conlist">
                    <dl class="fn-clear">
                        <dt>总金额：</dt>
                        <dd id="price" class="sys_item_price">
                            0</dd>
                    </dl>
                </div>
                <div class="w-item conlist">
                    <dl class="fn-clear">
                        <dt>小件费用：</dt>
                        <dd id="sprice" class="sys_item_price">
                            0</dd>
                    </dl>
                </div>
                <div class="w-item conlist">
                    <dl class="fn-clear">
                        <dt>押金金额：</dt>
                        <dd id="dprice" class="sys_item_price">
                            0</dd>
                    </dl>
                </div>
                <div class="w-item">
                    <dl class="in-item-number fn-clear" style="background: #f7f7f7;">
                        <dt>请选择服务</dt>
                        <dd>
                            <div class="" style="border: 0 solid #CCC; margin: 10px;">
                                <div class="iteminfo_buying">
                                    <!--规格属性-->
                                    <div class="sys_item_spec">

                                    <dl class="clearfix iteminfo_parameter sys_item_specpara_server" data-sid="" style=" display:none;">
			                        <dt>请选择需要购买的服务(此服务包含押金)</dt>
			                            <dd>
				                            <ul class="sys_spec_text" id="serverlist">

                            
               
                                            </ul>

			                            </dd>
		                            </dl>

                                    </div>
                                    <!--规格属性-->
                                </div>
                            </div>
                        </dd>
                    </dl>
                </div>
                
                <div id="perList">
                </div>
            </div>
            <div id="qianggou" class="p-tips" style="text-align: center; font-size: 14px; color: #FF6600;">
            </div>

            <div class="order-btn fn-clear" id="suborder">
                <div class="submit-btn">
                    
                    <input type="button" class="btn" id="submitBtn1" value="确认购买">
                    
                </div>
            </div>

        </div>
    </div>
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
    <input type="hidden" id="hid_comid" name="hid_com" value="<%=comid %>" />
    <input type="hidden" id="hid_openid" value="" />
    <input type="hidden" id="hid_Integral" value="0" />
    <input type="hidden" id="hid_Imprest" value="0" />
    <input type="hidden" id="hid_In" value="0" />
    <input type="hidden" id="Im" value="0" />
    <input type="hidden" id="buyuid" value="0" />
    <input type="hidden" id="tocomid" value="0" />
    <input type="hidden" id="hid_pno" value="<%=pno %>" />


      
<script type="text/x-jquery-tmpl" id="ServerItemEdit"> 
{{if Fserver==0}}
    {{if mustselect==0}}
        <li data-aid="${id}" data-dprice="${serverDepositprice}" data-sprice="${saleprice}"><a href="javascript:;" title="${servername}">${servername}</a><i></i></li>
    {{else}}
        <li class="xuanzhong" data-aid="${id}" data-dprice="${serverDepositprice}" data-sprice="${saleprice}"><a href="javascript:;" title="${servername}">${servername}</a><i></i></li>
    {{/if}}
{{/if}}
</script>


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



    <input type="hidden" id="hidLeavingDate" value="" />
    <input type="hidden" id="hidMinLeavingDate" value="" />
    <input type="hidden" id="hidLinePrice" value="" />
    <input id="hidEmptyNum" type="hidden" value="" />

    <input id="hid_phone" value="<%=u_phone %>" type="hidden" />
    <input id="hid_name" value="<%=name %>" type="hidden" />
    <input id="hid_proid" value="<%=proid %>" type="hidden" />
    <input id="hid_serverid" value="" type="hidden">
	<input id="hid_server_price" value="0" type="hidden">
<script>
//加载后自动点击
    $(document).ready(function () {
        setTimeout('autoclick()', 1500);
    })
    function autoclick() {
        $(".xuanzhong").trigger("click");  //让系统自动执行单击事件
    }

  //价格json


         var sys_item = {
            "mktprice": "0",
            "price": "0",
            "sys_attrprice": {              }
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

                $("#sprice").text(Number(sprice / 100).toFixed(2));
                $("#dprice").text(Number(dprice / 100).toFixed(2));
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


     

</asp:Content>
