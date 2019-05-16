<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mETicketIndex.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ETicket.mETicketIndex" MasterPageFile="/UI/pmui/ETicket/mEtown.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8">
    <meta name="keywords" content="微商城">
    <meta name="description" content="">
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">
    <title></title>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!-- CSS -->
    <link href="/Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/agent/m/css/cart.css">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/morder.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 200; //每页显示条数
        $(function () {
            $("#pno").focus();
            $("#pno").val("");
            $("#tblist").empty();
            $("#li_tblist").hide();
            $("#empty-list").hide();

            var comid = $("#hid_comid").trimVal();

            $("#h_comname").text($("#hid_comname").trimVal());

            $("#search_botton").click(function () {
                var pno = $("#pno").trimVal();
                if (pno == "") {
                    alert("请输入电子码!");
                    return;
                }
                searchpno(pno);
            })

            $("#loading-example-btn").click(function () {
                var usenum = $("#pnonum").trimVal();
                var pno = $("#pno").trimVal();
                if (usenum == "") {
                    alert("请选择使用数量!");
                    return;
                }
                if (pno == "") {
                    alert("请输入电子码！");
                    return;
                }


                $.post("/JsonFactory/EticketHandler.ashx?oper=econfirm", { pno: pno, usenum: usenum, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        location.reload();
                        return;
                    }
                    if (data.type == 100) {

                        //电子票使用详细信息
                        $.post("/JsonFactory/EticketHandler.ashx?oper=getedetail", { validateticketlogid: data.msg, pno: $("#pno").trimVal(), comid: comid }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                alert(data.msg);
                                return;
                            }
                            if (data.type == 100) {
                                $("#tblist").empty();
                                $("#li_tblist").show();
                                $("#empty-list").hide();

                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                //电子票号清空
                                $("#pno").val("");
                            }
                        })
                    }
                })
            })

            //非微信浏览器则隐藏扫码
            if (isWeiXin() == true) { } else {
                $("#saomahref").hide();
            }
        })
        function searchpno(pno) {
            $("#tblist").empty();
            $("#li_tblist").hide();
            $("#pno").val(pno);

            //判断电子码是否存在
            $.post("/JsonFactory/EticketHandler.ashx?oper=ValidateEticket", { pno: pno, comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    $("#empty-list").show();
                    $("#pnonum").empty();

                    //给电子码使用数量赋值
                    var use_pnum = data.msg.Use_pnum;
                    var selstr = "";
                    for (var i = use_pnum; i >= 1; i--) {
                        selstr += "<option value='" + i + "'>" + i + "</option>";
                    }
                    $("#pnonum").append(selstr);

                    $("#ticketname").text(data.msg.E_proname);
                }
            })
        }
        function goyanzheng() {
            location.href = "/ui/pmui/ETicket/mETicketIndex.aspx";
        }
        function goyanzhenglist() {
            location.href = "/ui/pmui/ETicket/mETicketList.aspx";
        }
        function golvyoudaba() {
            location.href = "/ui/MemberUI/mTravelbusOrderCount.aspx";
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
        .btn.btn-orange-dark
        {
            color: #FFF;
            border-color: rgb(28, 189, 241);
            background-color: rgb(60, 175, 220);
            font-size: 14px;
            width: 120px;
            margin-top: 10px;
            padding: 7px 80px;
        }
        a:link, a:visited
        {
            color: #666;
            text-decoration: none;
            height: 25px;
            line-height: 18px;
            size: 14px;
        }
        .btn
        {
            margin-bottom: 15px;
        }
        .btn
        {
            display: inline !important;
            margin-left: auto;
            margin-right: auto;
            padding-left: 14px;
            padding-right: 14px;
            font-size: 18px;
            text-align: center;
            text-decoration: none;
            overflow: visible;
            height: 42px;
            border-radius: 5px;
            box-sizing: border-box;
            color: #FFF;
            line-height: 42px;
        }
        .list-search dl
        {
            height: 38px !important;
        }
        .list-search
        {
            padding: 5px 10px 15px !important;
        }
    </style>
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=0">
    <link rel="stylesheet" href="http://demo.open.weixin.qq.com/jssdk/css/style.css?ts=1420774989">
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"> </script>
    <script>
        wx.config({
            debug: false,
            appId: '<%=appId %>',
            timestamp: '<%=timestamp %>',
            nonceStr: '<%=nonceStr %>',
            signature: '<%=signature %>',
            jsApiList: [
        'checkJsApi',
        'onMenuShareTimeline',
        'onMenuShareAppMessage',
        'onMenuShareQQ',
        'onMenuShareWeibo',
        'hideMenuItems',
        'showMenuItems',
        'hideAllNonBaseMenuItem',
        'showAllNonBaseMenuItem',
        'translateVoice',
        'startRecord',
        'stopRecord',
        'onRecordEnd',
        'playVoice',
        'pauseVoice',
        'stopVoice',
        'uploadVoice',
        'downloadVoice',
        'chooseImage',
        'previewImage',
        'uploadImage',
        'downloadImage',
        'getNetworkType',
        'openLocation',
        'getLocation',
        'hideOptionMenu',
        'showOptionMenu',
        'closeWindow',
        'scanQRCode',
        'chooseWXPay',
        'openProductSpecificView',
        'addCard',
        'chooseCard',
        'openCard'
      ]
        });
    </script>
    <script src="http://demo.open.weixin.qq.com/jssdk/js/api-6.1.js?ts=1420774989"> </script>
    <script type="text/javascript">
        //        // 9 微信原生接口
        //        // 9.1.1 扫描二维码并返回结果
        //        document.querySelector('#scanQRCode0').onclick = function () {
        //            wx.scanQRCode({
        //                desc: 'scanQRCode desc'
        //            });
        //        };
        //        // 9.1.2 扫描二维码并返回结果
        //        document.querySelector('#scanQRCode1').onclick = function () {
        //            wx.scanQRCode({
        //                needResult: 1,
        //                desc: 'scanQRCode desc',
        //                success: function (res) {
        //                    alert(JSON.stringify(res));
        //                }
        //            });
        //        };
        function scanclick() {
            wx.scanQRCode({
                needResult: 1,
                desc: 'scanQRCode desc',
                success: function (res) {
                    //                    alert(JSON.stringify(res));
                    var data = JSON.stringify(res);
                    data = eval("(" + data + ")");
                    //                    alert(data.resultStr);
                    searchpno(data.resultStr);
                }
            });
        }
        wx.error(function (res) {
            //            alert(res.errMsg);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <header class="header" style="background-color: #3CAFDC;">
          <h1 id="h_comname"> </h1>
        <div class="left-head"> 
              <a href="/ui/pmui/ETicket/mETicketIndex.aspx" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="home-10"></span></span>
              </a> 
            </div>
        <div class="right-head"> 
                <a href="/yanzheng/loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>  
        </div>
        </header>
    <!-- container -->
    <div class="container " style="text-align: center;">
        <div class="tabber  tabber-n3 tabber-double-11 clearfix">
            <a href="javascript:goyanzheng()" class="active" style="width: 33%;">验证电子凭证</a>
            <a class="" href="javascript:goyanzhenglist()" style="width: 33%;">验证列表</a> <a href="javascript:golvyoudaba()"
                style="width: 33%;">旅游大巴查询</a>
        </div>
        <div class="list-search" style="margin-bottom: 10px;">
            <dl class="fn-clear">
                <dt>
                    <input type="tel" id="pno" placeholder="请输入电子码" />
                </dt>
                <dd>
                    <%--<s ></s> --%>
                </dd>
            </dl>
        </div>
        <a href="javascript:void(0)" id="search_botton" class="js-buy-it btn btn-orange-dark btn-2-1">
            验证</a>
        <%if (isrightwxset == 1)
          { %>
        <a href="javascript:void(0)" onclick="scanclick()" style="padding-left: 20px;" id="saomahref">
            扫码验证</a>
        <%} %>
        <div id="backs-list-container" class="block">
            <li class="block block-order animated" id="li_tblist" style="display: none;">
                <div class="block block-list block-border-top-none block-border-bottom-none" id="tblist"
                    style="padding-left: 0;">
                    <%--  <div class="layout-box">
                      <ul class="list-a">
                            <li><span>流水：a</span><span style="float: right;">b</span> </li>
                            <li>内容： c </li>
                            <li>收入：<strong class="r1">&nbsp; d</strong></li>
                            <li>支出：<strong class="r1">&nbsp;e</strong></li>
                            <li>余额：<strong class="r1">&nbsp;f</strong></li>
                        </ul>
                    </div>--%>
                </div>
            </li>
            <div class="empty-list" id="empty-list" style="text-align: left; display: none;">
                <!-- 文本 -->
                <div>
                    <h4 id="ticketname">
                    </h4>
                    <h4>
                        <select id="pnonum" class="mi-input" style="width: 100%;">
                        </select><input type="button" style="border-radius: 2px; width: 100%; line-height: 40px;
                            height: 40px; color: #fff; font-size: 14px; margin-top: 10px; border: 1px solid #3CAFDC;
                            border-bottom-color: #26922C; background-color: #3CAFDC; box-shadow: 0 1px 0 rgba(255,255,255,.2) inset, 1px 1px 2px rgba(0,0,0,.2);
                            background: -webkit-gradient(linear,0 0,0 100%,color-stop(0,#3CAFDC),color-stop(100%,#3CAFDC));
                            background: -moz-linear-gradient(top,#3CAFDC,#3CAFDC); background: -o-linear-gradient(top,#3CAFDC,#3CAFDC);
                            background: -ms-linear-gradient(top,#3CAFDC,#3CAFDC); background: linear-gradient(top,#3CAFDC,#3CAFDC);"
                            value="验 证" id="loading-example-btn">
                    </h4>
                </div>
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                      <div class="layout-box">
                      <ul class="list-a">
                            <li><h2>
                                            <strong></strong>电子凭证验证成功！</h2></li>
                            <li> 服务内容：<strong>${ProductName} </li>
                            <li> 使用数量： <span class="STYLE9">${ThisUseNum}</span> 张&nbsp; 电子票号：${Pno}</li>
                            <li>服务单价：${fmoney(OnePrice,2)} 元</li>
                            <li>有 效 期： ${ChangeDateFormat(Pro_Start)}--${ChangeDateFormat(Pro_end)}</li>
                        </ul>
                    </div> 
    </script>
</asp:Content>
