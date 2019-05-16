<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="buyserver.aspx.cs" Inherits="ETS2.WebApp.YY.buyserver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>
        在线自助购买押金与服务
</title>
    <script type="text/javascript">
        var _tcopentime = new Date().getTime();
        var _hmt = _hmt || [];
    </script>
    <!-- meta信息，可维护 -->
    <meta charset="UTF-8" /><meta name="apple-mobile-web-app-capable" content="yes" /><meta name="apple-mobile-web-app-status-bar-style" content="black" /><meta content="telephone=no" name="format-detection" /><meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <!-- 页面样式表 -->
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .none
        {
            display: none;
        }
        
    </style>
    <!-- 页面样式表 -->
    <link href="../Styles/H5/scenery.css" rel="stylesheet" type="text/css" /><link href="../Styles/H5/list.css" rel="stylesheet" type="text/css" /><meta name="description" content="北京景点门票，北京景点门票预订，北京旅游景点大全" /><meta name="keywords" content="是您旅行的好伙伴。" />
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
        .in-item dt {
    text-align: right !important;
}
        .in-item {

            text-align: center!important;
        }
        .in-item dd span {
    display: block;
    text-align: right;
    color: #9EA5AC;
    float: left;
    padding: 10px 30px;
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

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
     <script type="text/javascript">
         $(function () {
             $("#pno").focus();
             $("#duanxintemp").show();
             $("#cardtemp").hide();
             document.onkeydown = function (e) {
                 var ev = document.all ? window.event : e;
                 if (ev.keyCode == 13) {// 如（ev.ctrlKey && ev.keyCode==13）为ctrl+Center 触发 
                     //要处理的事件 
                     $("#sub").click();
                 }
             }





             //提交按钮
             $("#sub").click(function () {
                 $("#loading").html("正在提交，请稍后...");
                 $("#loading").show();
                 var pno = $("#pno").val();
                 var getcode = $("#getcode").val();
                 var pno1 = $("#pno1").val();
                 var buytype = 0;

                 if (buytype == 0) {
                     if (pno == "") {
                         alert("请填写短信码!");
                         $("#loading").hide();
                         return;
                     };
                 }

                 if (buytype == 1) {
                     if (pno1 == "") {
                         alert("请填写卡号!");
                         $("#loading").hide();
                         return;
                     };
                 }
                 if (getcode == "") {
                     alert("请填写验证码!");
                     $("#loading").hide();
                     return;
                 };

                 //判断验证码输入是否正确
                 $.post("/JsonFactory/OrderHandler.ashx?oper=buyserver", { pno: pno, getcode: getcode,buytype:buytype,pno1:pno1 }, function (data) {
                     data = eval("(" + data + ")");
                     if (data.type == 1) {
                         $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                         alert(data.msg);
                         $("#loading").hide();
                         return;
                     }
                     if (data.type == 100) {
                         $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                         location.href = "/h5/orderserver.aspx?pno=" + data.msg
                     }
                 })

             })

         })

         function selectbuytype() {
             var buytype = $("input:radio[name='buytype']:checked").val();
             if (buytype == 0) {
                 $("#duanxintemp").show();
                 $("#cardtemp").hide();

             } else {
                 $("#duanxintemp").hide();
                 $("#cardtemp").show();
             }
         }
    </script>
</head>
<body>
    <div>
    <div id="loading" class="loading" style="display: none;">
       正在加载..
    </div>
        
    <div id="inner">
        <!-- 公共页头  -->
        <header class="header" style="background-color: #3CAFDC;">
                    <h1 style=" text-align:center;">自助购买押金与服务</h1>
                    
    </header>
        <!-- 页面内容块 -->
        <div id="orderFome" class="order-form" style=" padding-top:50px;">
            <div class="t-head">
                
                 <div  style=" display:none;">
                                <label>
                                    <input name="buytype" class="selectdeliverytype" value="0" type="radio" onclick="selectbuytype();">
                                    短信码购买</label>
                               <label>
                                
                                    <input name="buytype" class="selectdeliverytype" value="1" type="radio" checked="checked"   onclick="selectbuytype();">
                                    输入卡号购买</label></div>
                

            </div>
            <div class="container">
                
                <div class="w-item" id="duanxintemp" style="display:none;">
                    <dl class="in-item fn-clear">
                        <dt>短信码：</dt>
                        <dd>
                            <input type="tel" id="pno" name="pno" placeholder="请输入短信码" value="" class="writeok">
                        </dd>
                    </dl>
                </div>
                <div class="w-item" id="cardtemp" >
                    <dl class="in-item fn-clear">
                        <dt>卡  号：</dt>
                        <dd>
                            <input type="tel" id="pno1" name="pno1" placeholder="请输入卡号" value="" class="writeok">
                        </dd>
                    </dl>
                </div>
                <div class="w-item">
                    <dl class="in-item fn-clear borderTop">
                        <dt>验证码：</dt>
                        <dd>
                        <input id="getcode" name="getcode" type="tel" class="writeok"  placeholder="验证码" style="float: left; width: 50px; padding-left: 10px;" size="6"  /><span id="imgcode"><img id="validateCode" src="/Account/GetValidateCode.ashx" alt="验证码" onclick="this.src='/Account/GetValidateCode.ashx?rnd=' + Math.random();"/></span><div class="tips c8" id="codemsg"></div>
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
                    <input type="button" class="btn" id="sub" value="确认，下一步">
                </div>
            </div>
            
        </div>


    </div>

    <input id="hid_comid" value="0" type="hidden" />
   
    </div>
</body>
</html>
