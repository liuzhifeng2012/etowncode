<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/V/Member.Master" CodeBehind="Pay.aspx.cs"
    Inherits="ETS2.WebApp.UI.VASUI.Pay" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            //            var browsertype = fBrowserRedirect();
            ////            alert(browsertype);
            //            if (browsertype == "mobile" || browsertype == "ipad") {
            //                window.open("/h5/pay.aspx?orderid=<%=orderid%>", target = "_self");
            //                return;
            //            }
            //            else { }

            $("ul.paybank li").click(function () {
                var c = $("ul.paybank li");
                var div_name = "bankinfo_con_" + c.index(this);

                $("div.bankinfo_con").hide();
                $("#" + div_name).show();
                c.eq(c.index(this)).addClass("paybank_on").siblings().removeClass("paybank_on");
                //设置字体
                c.eq(c.index(this)).css("font-weight", "bold").siblings().css("font-weight", "normal");
            })

            //定时刷新 判断订单支付状态
            setInterval(checkPaystate, 3000); // 注意函数名没有引号和括弧！ 

            function checkPaystate() {
                $.post("/JsonFactory/OrderHandler.ashx?oper=checkPaystate", {orderid:<%=orderid%>}, function (data) { 
                 data=eval("("+data+")");
                 if(data.type==1){}
                 if(data.type==100){
                    if(data.msg==2){
                       window.open("PaySuc.aspx?out_trade_no="+<%=orderid%>,target="_self");
                    } 
                 }
                })
            }
        })
         
    </script>
    <link rel="stylesheet" type="text/css" href="http://union.tenpay.com/bankList/css_col4.css" />
    <style type="text/css">
        .p-w-bd
        {
            padding-left: 50px;
            margin-bottom: 30px;
        }
        .p-w-bd:after, .pay-weixin:after
        {
            display: table;
            content: "";
            clear: both;
        }
        .p-w-hd
        {
            margin-bottom: 20px;
            font-size: 18px;
            font-family: "Microsoft Yahei";
        }
        .p-w-box
        {
            float: left;
            width: 300px;
        }
        .pw-box-hd
        {
            margin-bottom: 20px;
        }
        .pw-box-hd img
        {
            border: 1px solid #ddd;
        }
        .pw-box-ft
        {
            height: 44px;
            padding: 8px 0 8px 125px;
            background: url() 50px 8px no-repeat #ff7674;
        }
        .pw-box-ft p
        {
            margin: 0;
            font-size: 14px;
            line-height: 22px;
            color: #fff;
            font-weight: 700;
        }
        .p-w-sidebar
        {
            float: left;
            width: 379px;
            height: 421px;
            padding-left: 50px;
            margin-top: -20px;
            background: url(http://shop.etown.cn/images/weixin/phone-bg.png) 50px 0 no-repeat;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="site-nav">
    </div>
    <div class="main">
        <h3 class="titlepng">
            <%=proname%>
        </h3>
        <div class="separationBox">
            <div class="bottomLineBanner">
                <span class="bottomLineName">预订信息</span>
            </div>
            <div class="bookOrderDetails">
                <div class="bookOrderDetailsRight dataMarginBox" name="choseTime_34009" vtype="rq"
                    data-err-rq="" data-err-fn="" vfn="calendarVerify">
                    <span>姓 名 ：</span><%=u_name%></div>
                <span class="bookLimit">(实名认证，请填写出游人姓名)</span>
            </div>
            <div class="bookOrderDetails">
                <span class="bookOrderDetailsLeft" style="margin-left: -10px;">手机号码：</span>
                <div class="bookOrderDetailsRight dataMarginBox" name="choseTime_34009" vtype="rq"
                    data-err-rq="" data-err-fn="" vfn="calendarVerify">
                    <%=u_mobile %>
                </div>
                <span class="bookLimit">(免费接收订单确认短信，请务必正确填写)</span>
            </div>
            <% if (ordertype != 2)//判断不是充值订单
               { %>
            <%if (Server_type == 9)
              {%>
            <div class="bookOrderDetails">
                <span class="bookOrderDetailsLeft" style="margin-left: -10px;">日 期 ： </span>
                <div class="bookOrderDetailsRight dataMarginBox" vfn="calendarVerify">
                    入住:
                    <%=stardate%>
                    离店:<%=enddate%>
                </div>
            </div>
            <div class="bookOrderDetails" style="margin-left: -10px;">
                <span class="bookOrderDetailsLeft">房间数量：</span>
                <%=buy_num%>
            </div>
            <%}
              else
              { %>
            <%if (cart == 0)
              { %>
            <%if (Server_type != 11)
              { %>
            <div class="bookOrderDetails" style="margin-left: -10px;">
                <span class="bookOrderDetailsLeft">游玩日期：</span>
                <div class="bookOrderDetailsRight dataMarginBox" vfn="calendarVerify">
                    <%=travel_date%>
                </div>
            </div>
            <%} %>
            <div class="bookOrderDetails" style="margin-left: -10px;">
                <span class="bookOrderDetailsLeft">数 量 ： </span>
                <%=buy_num%>
            </div>
            <%if (Server_type != 11)
              { %>
            <div class="bookOrderDetails " style="margin-left: -10px;">
                <span class="bookOrderDetailsLeft">有 效 期 ：</span><%=u_youxiaoqi%></div>
            <%} %>
            <%} %>
            <%} %>
            <%} %>
            <div class="bookOrderDetails countprice">
                合计(<span class="pricefont">需在线支付</span>):<span class="pricefont">￥<%=p_totalprice%></span></div>
        </div>
        <div class="separationBox">
            <div class="bottomLineBanner">
                <span class="bottomLineName">支付方式</span>
            </div>
            <div>
                <div id="bankselect">
                    <ul class="paybank">
                        <li id="paybank_1" class="paybank_on" style="font-weight: normal; display: ;">微信支付</li>
                        <li id="paybank_1" style="font-weight: normal;">银行卡支付</li>
                        <li id="paybank_1" class="" style="font-weight: bold;">平台支付</li>
                    </ul>
                </div>
                <%if (comid != 112 && comid != 1194)
                  {%>
                <div class="bankinfo_con" id="bankinfo_con_0">
                    <div class="pay-weixin">
                        <div class="p-w-hd" style="padding: 12px 30px 0">
                            微信支付</div>
                        <div class="p-w-bd" style="position: relative">
                            <div class="p-w-box">
                                <div class="pw-box-hd">
                                    <img src="<%=nativePayImgurl %>" width="298">
                                </div>
                                <div class="pw-box-ft">
                                    <p>
                                        请使用微信扫一扫</p>
                                    <p>
                                        扫描二维码支付</p>
                                </div>
                            </div>
                            <div class="p-w-sidebar">
                            </div>
                        </div>
                    </div>
                    <%--   <table width="100%" border="0" cellpadding="5" cellspacing="2">
                        <tbody>
                            <tr>
                                <td height="50" colspan="2" nowrap="nowrap">
                                    <td align="center">
                                        <img src="<%=nativePayImgurl %>" id="img3" height="135" width="135" />
                                    </td>
                                </td>
                            </tr>
                        </tbody>
                    </table>--%>
                </div>
                <div class="bankinfo_con" id="bankinfo_con_1" style="display: none;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="2">
                        <tbody>
                            <tr>
                                <td height="50" colspan="3" nowrap="nowrap">
                                    <p>
                                        &nbsp;&nbsp;&nbsp; <strong>支付宝快捷支付</strong> 一步验证，无需开通网银！
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" height="50" align="left" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=BOC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/kuaijeizhifu.png" alt="快捷支付" width="355" height="68" border="0"
                                            style="padding-left: 40px;"></a></strong>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" colspan="3" nowrap="nowrap">
                                    &nbsp;&nbsp;&nbsp; <strong>网银支付</strong> （通过银行支付）
                                </td>
                            </tr>
                            <tr>
                                <td width="33%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=BOCB2C"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhongguo.gif" alt="中国银行" width="154" height="33" border="0"></a>
                                </td>
                                <td width="32%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=ICBCB2C"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/gongshang.gif" alt="工商银行" width="154" height="33" border="0"></a>
                                </td>
                                <td width="35%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CCB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/jianshe.gif" alt="建设银行" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CMB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhaohang.gif" alt="招商银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=BJBANK"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/beijing.gif" alt="北京银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=PSBC-DEBIT"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/youzheng.gif" alt="邮政储蓄" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=COMM"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/jiaotong.gif" alt="交通银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CEBBANK"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/guangda.gif" alt="光大银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=SPDB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/shangpufa.gif" alt="上海浦东发展银行" width="154" height="33"
                                            border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CIB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/xingye.gif" alt="兴业银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CITIC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhongxin.gif" alt="中信银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=ABC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/nongye.gif" alt="中国农业银行" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=SDB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/shenfa.gif" alt="深圳发展银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CMBC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/minsheng.gif" alt="民生银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=GDB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/guangfa.gif" alt="广东发展银行" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="30" colspan="3" nowrap="nowrap" bgcolor="#FFFFFF">
                                    &nbsp;&nbsp;&nbsp; 点击上面银行图标 进入银行支付页面
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="bankinfo_con" id="bankinfo_con_2" style="display: none;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="2">
                        <tbody>
                            <tr>
                                <td height="50" colspan="2" nowrap="nowrap">
                                    &nbsp;&nbsp;&nbsp; 点击下列第三方支付平台图标 进入支付页面
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=&defaultbank=" target="_blank"
                                        class="pay">
                                        <img src="/images/taobaologo123x40.jpg" alt="支付宝" width="123" height="40" border="0"></a>
                                    &nbsp;&nbsp; <a href="<%=tenpay_url %>" target="_blank" class="pay">
                                        <img src="/images/caifutong.jpg" alt="财付通" width="123" height="40" border="0" style="display: none;"></a><br>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <%}%>
            </div>
        </div>
    </div>
</asp:Content>
