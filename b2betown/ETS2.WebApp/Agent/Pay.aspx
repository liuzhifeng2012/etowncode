<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"  CodeBehind="Pay.aspx.cs" Inherits="ETS2.WebApp.Agent.Pay" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            $(function () {

                $("ul.paybank li").click(function () {
                    var c = $("ul.paybank li");
                    var div_name = "bankinfo_con_" + c.index(this);

                    $("div.bankinfo_con").hide();
                    $("#" + div_name).show();
                    c.eq(c.index(this)).addClass("paybank_on").siblings().removeClass("paybank_on");
                    //设置字体
                    c.eq(c.index(this)).css("font-weight", "bold").siblings().css("font-weight", "normal");
                })



                  $(".pay").click(function () {
                      $("#fullbg,#paylog").show();
                  })

            })


            //关闭灰色 jQuery 遮罩
            function closeBg() {
                $("#fullbg,#paylog").hide();
            }

            //支付成功
            function payover() {

                var orderd = $("#hid_orderid").val();
                var agentorderid = $("#hid_agentorderid").val();
                var comid = $("#hid_comid_temp").val();
                var act = $("#hid_act").val();
                if (agentorderid != 0) {

                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/OrderHandler.ashx?oper=agentorderpay",
                        data: { comid: comid, id: agentorderid },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $.prompt(data.msg);
                                return;
                            }
                            if (data.type == 100) {
                                location.href = "Order.aspx?comid=" + comid;
                            }
                        }
                    })

                } else if (act == "cart") {

                    location.href = "ShopCart.aspx?comid=" + comid;
                } else {

                    location.href = "Manage_sales.aspx?comid=" + comid;

                };

                $("#fullbg,#paylog").hide();
            }
            //支付失败
            function payerr() {

                alert("如果支付出异常，您可以更换其他方式进行支付！");
                $("#fullbg,#paylog").hide();
            }
	


    </script>
    <link href="/Styles/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .main {
            width: auto; 
          }
          
          
#fullbg {
    background-color:Gray;
    left:0px;
    opacity:0.5;
    position:absolute;
    top:0px;
    z-index:3;
    filter:alpha(opacity=50); /* IE6 */
    -moz-opacity:0.5; /* Mozilla */
    -khtml-opacity:0.5; /* Safari */
}
 #paylog {
    background-color:#FFF;
    border:1px solid #888;
    display:none;
    height:200px;
    left:50%;
    margin:-50px 0 0 -300px;
    position:fixed !important; /* 浮动对话框 */
    position:absolute;
    top:50%;
    width:350px;
    z-index:15;
}
#paylog p {
    margin:0 0 12px;
}
#paylog p.close {
    text-align:right;
}
.payover {
    height: 35px;
    width: 111px;
    background: url("/images/wanchengzhifu.jpg") no-repeat scroll 0% 0% transparent;
    display: block;
    float: left;
    text-indent: -9999em;
    border: 0px none;
    cursor: pointer;
}
.paycannel {
    height: 35px;
    width: 111px;
    background: url("/images/zhifuyudaowenti.jpg") no-repeat scroll 0% 0% transparent;
    display: block;
    float: left;
    text-indent: -9999em;
    border: 0px none;
    cursor: pointer;
}
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
      
         <div id="setting-home" class="vis-zone">
            <div class="inner">
            <div class="main">
             <div class="bottomLineBanner">
                <span class="bottomLineName"><%=proname%></span>
            </div>
        <div class="separationBox">
                <div class="bookOrderDetails countprice">
                合计(<span class="pricefont">需在线支付</span>):<span class="pricefont" >￥<%=p_totalprice%></span></div>
            </div>
            

<div class="separationBox">
            <div class="bottomLineBanner">
                <span class="bottomLineName">支付方式</span>
            </div>
            <div>
                <div id="bankselect">
                    <ul class="paybank">
                        <li id="paybank_1" class="" style="font-weight: normal;">信用卡</li>
                        <li id="paybank_2" class="paybank_on" style="font-weight: bold;">储蓄卡</li>
                        <li id="paybank_3" style="font-weight: normal;">支付平台</li>
                    </ul>
                </div>
                <div class="bankinfo_con" id="bankinfo_con_0" style="display: none;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="2">
                        <tbody>
                            <tr>
                                <td height="50" colspan="3" nowrap="nowrap">
                                    <p>
                                        &nbsp;&nbsp;&nbsp; <strong>快捷支付</strong> 一步验证，无需开通网银！
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=BOC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhongguo.gif" alt="中国银行" width="154" height="33" border="0"></a></strong>
                                </td>
                                <td width="32%" height="-1" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=CMB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhaohang.gif" alt="招商银行" width="154" height="33" border="0"></a></strong>
                                </td>
                                <td width="35%" height="-1" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=CCB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/jianshe.gif" alt="建设银行" width="154" height="33" border="0"></a></strong>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=ICBC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/gongshang.gif" alt="工商银行" width="154" height="33" border="0"></a></strong>
                                </td>
                                <td height="1" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=ABC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/nongye.gif" alt="中国农业银行" width="154" height="33" border="0"></a></strong>
                                </td>
                                <td height="1" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=CEB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/guangda.gif" alt="光大银行" width="154" height="33" border="0"></a></strong>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=SPABANK"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/pingan.gif" alt="平安银行" width="154" height="33" border="0"></a></strong>
                                </td>
                                <td height="4" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                </td>
                                <td height="4" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                </td>
                            </tr>
                            <tr>
                                <td height="50" colspan="3" nowrap="nowrap">
                                    &nbsp;&nbsp;&nbsp; <strong>网银支付</strong> （需开通网银）
                                </td>
                            </tr>
                            <tr>
                                <td width="33%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=BOCB2C"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhongguo.gif" alt="中国银行" width="154" height="33" border="0"></a>
                                </td>
                                <td width="32%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=ICBCB2C"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/gongshang.gif" alt="工商银行" width="154" height="33" border="0"></a>
                                </td>
                                <td width="35%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CCB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/jianshe.gif" alt="建设银行" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CMB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhaohang.gif" alt="招商银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=BJBANK"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/beijing.gif" alt="北京银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=PSBC-DEBIT"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/youzheng.gif" alt="邮政储蓄" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=COMM"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/jiaotong.gif" alt="交通银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CEBBANK"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/guangda.gif" alt="光大银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=SPDB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/shangpufa.gif" alt="上海浦东发展银行" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CIB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/xingye.gif" alt="兴业银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CITIC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhongxin.gif" alt="中信银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=ABC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/nongye.gif" alt="中国农业银行" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=SDB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/shenfa.gif" alt="深圳发展银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CMBC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/minsheng.gif" alt="民生银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=GDB"
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
                <div class="bankinfo_con" id="bankinfo_con_1" style="display: block;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="2">
                        <tbody>
                            <tr>
                                <td height="50" colspan="3" nowrap="nowrap">
                                    <p>
                                        &nbsp;&nbsp;&nbsp;<strong> 快捷支付</strong> 一步验证，无需开通网银！
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=BOC&amp;Payt=d"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhongguo.gif" alt="中国银行" width="154" height="33" border="0"></a></strong>
                                </td>
                                <td width="32%" height="-1" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=ABC&amp;Payt=d"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/nongye.gif" alt="中国农业银行" width="154" height="33" border="0"></a></strong>
                                </td>
                                <td width="35%" height="-1" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=CCB&amp;Payt=d"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/jianshe.gif" alt="建设银行" width="154" height="33" border="0"></a></strong>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=ICBC&amp;Payt=d"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/gongshang.gif" alt="工商银行" width="154" height="33" border="0"></a></strong>
                                </td>
                                <td height="1" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong><a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=motoPay&defaultbank=CEB&amp;Payt=d"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/guangda.gif" alt="光大银行" width="154" height="33" border="0"></a></strong>
                                </td>
                                <td height="1" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <strong></strong>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" colspan="3" nowrap="nowrap">
                                    &nbsp;&nbsp;&nbsp; <strong>网银支付</strong> （需开通网银）
                                </td>
                            </tr>
                            <tr>
                                <td width="33%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=BOCB2C"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhongguo.gif" alt="中国银行" width="154" height="33" border="0"></a>
                                </td>
                                <td width="32%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=ICBCB2C"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/gongshang.gif" alt="工商银行" width="154" height="33" border="0"></a>
                                </td>
                                <td width="35%" height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CCB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/jianshe.gif" alt="建设银行" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CMB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhaohang.gif" alt="招商银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=BJBANK"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/beijing.gif" alt="北京银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=PSBC-DEBIT"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/youzheng.gif" alt="邮政储蓄" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=COMM"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/jiaotong.gif" alt="交通银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CEBBANK"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/guangda.gif" alt="光大银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=SPDB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/shangpufa.gif" alt="上海浦东发展银行" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CIB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/xingye.gif" alt="兴业银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CITIC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/zhongxin.gif" alt="中信银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=ABC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/nongye.gif" alt="中国农业银行" width="154" height="33" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=SDB"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/shenfa.gif" alt="深圳发展银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=CMBC"
                                        target="_blank" class="pay">
                                        <img src="/images/banklogo/minsheng.gif" alt="民生银行" width="154" height="33" border="0"></a>
                                </td>
                                <td height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=bankPay&defaultbank=GDB"
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
                                <td  height="50" align="center" nowrap="nowrap" bgcolor="#FFFFFF">
                                    <a href="/ui/VASUI/alipay/subpay.aspx?out_trade_no=<%=orderid%>&paymethod=&defaultbank="
                                        target="_blank" class="pay">
                                        <img src="/images/taobaologo123x40.jpg" alt="支付宝" width="123" height="40" border="0"></a><br>
                                </td>
                                
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        </div>
             </div>
        </div>

    </div>
    <div class="data">
    </div>

    <div style="height: 100%; width: 100%; display: none;" id="fullbg"></div>
    <div id="paylog">
<div id="dialog_content"  style="background-color:#efefef;text-align:left; padding-left:5px; padding-top:5px;height:30px; font-size:16px;">支付</div> 
<div style="text-align:center;padding-top:30px; padding:12px;">

		<div style="padding-left:20px; font-size:12px;">请您在新打开的网上银行页面进行支付，支付完成前请不要关闭该窗口。
		</div>
		<div style="padding-top:20px;padding-left:60px;">
			<a href="#" onClick="payover();" class="payover">已完成付款</a>
			<a href="#" onClick="payerr();"class="paycannel">付款遇到问题</a>
		</div>
		<div style="text-align:left; padding-top:20px;font-size:12px;">
			<br><a href="#"  onclick="closeBg();">&lt;&lt; 选择其他支付方式</a>
		</div>


</div> 
</div> 

    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <input id="hid_payprice" type="hidden" value="0" />
    <input id="hid_agentorderid" type="hidden" value="<%=agentorderid %>" />
    <input id="hid_orderid" type="hidden" value="<%=orderid %>" />
    <input id="hid_act" type="hidden" value="<%=act %>" /
    </asp:Content>