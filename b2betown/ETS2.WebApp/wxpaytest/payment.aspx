<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payment.aspx.cs" Inherits="ETS2.WebApp.wxpaytest.payment"
    MasterPageFile="/H5/Order.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <meta id="Meta1" name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1; user-scalable=no;" />
    <script language="javascript" src="http://res.mail.qq.com/mmr/static/lib/js/lazyloadv3.js"
        type="text/javascript"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1" />
     <link href="/Styles/H5/pay.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        // 当微信内置浏览器完成内部初始化后会触发WeixinJSBridgeReady事件。

        //公众号支付
        function sub() {
            WeixinJSBridge.invoke('getBrandWCPayRequest', {
                "appId": "<%= appId %>", //公众号名称，由商户传入
                "timeStamp": "<%= timeStamp %>", //时间戳
                "nonceStr": "<%= nonceStr %>", //随机串
                "package": "<%= package %>", //扩展包
                "signType": "MD5", //微信签名方式:1.sha1
                "paySign": "<%= paySign %>" //微信签名
            }, function (res) {
                if (res.err_msg == "get_brand_wcpay_request:ok") {
                    //                        alert("微信支付成功!");
                    window.location.href = "/wxpaytest/wxpaysuc.aspx"; //跳转到支付成功页面
                } else if (res.err_msg == "get_brand_wcpay_request:cancel") {
                    alert("用户取消支付!");
                } else {
                    alert(res.err_msg);
                    alert("支付失败!");
                }
                // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回ok，但并不保证它绝对可靠。
                //因此微信团队建议，当收到ok返回时，向商户后台询问是否收到交易成功的通知，若收到通知，前端展示交易成功的界面；若此时未收到通知，商户后台主动调用查询订单接口，查询订单的当前状态，并反馈给前端展示相应的界面。
            });
        }
    
    </script>
    <style type="text/css">
        .order-btn input
        {
            width: 100%;
            height: 40px;
            border: 0px;
            border-image-source: initial;
            border-image-slice: initial;
            border-image-width: initial;
            border-image-outset: initial;
            border-image-repeat: initial;
            background: 0px 50%;
            color: rgb(255, 255, 255);
        }
        .order-btn
        {
            line-height: 44px;
            margin: 10px 10px 20px;
            font-size: 18px;
            text-align: center;
            color: rgb(255, 255, 255);
            background: rgb(254, 147, 43);
            border-radius: 4px;
            border: 1px solid rgb(243, 119, 61);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 公共页头  -->
    <a id="goBack" href="javascript:history.go(-1);">
        <header class="header">
                    <h1>确认支付</h1>
        <div class="left-head">
          <a href="javascript:history.go(-1);" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
    </a>
    <!-- 页面内容块 -->
    <div class="body">
        <div class="body-content">
            <dl>
                <dt>产品名称：</dt>
                <dd>
                    <%=proname%></dd>
            </dl>
            <dl>
                <dt>商家：</dt>
                <dd>
                    <%=comName%></dd>
            </dl>
            <dl>
                <dt>手机号：</dt>
                <dd>
                    <%=u_mobile %></dd>
            </dl>
            <%if (servertype == 9)//酒店
              {
            %>
            <dl>
                <dt>预订间数：</dt>
                <dd>
                    <%=buy_num%>(单间总价<%=price%>)</dd>
            </dl>
            <%
                }
              else if (servertype == 2 || servertype == 8)//当地游；跟团游
              { 
            %>
            <dl>
                <dt>出游人数：</dt>
                <dd>
                    <%=pricedetail%>
                </dd>
            </dl>
            <%
                }
              else//票务
              {
            %>
            <dl>
                <dt>数量：</dt>
                <dd>
                    <%=buy_num%>(单价<%=price%>)</dd>
            </dl>
            <%
                }%>
            <dl>
                <dt>订单金额：</dt>
                <dd class="font-orange">
                    <%=p_totalprice%>元</dd>
            </dl>
            <%if (order_status != "等待对方付款")
              {%>
            <dl>
                <dt>订单状态：</dt>
                <dd style="font-size: 16px; font-weight: bold;">
                    <%=order_status%></dd>
            </dl>
            <%  } %>
        </div>
        <%if (order_status == "等待对方付款")
          {%>
        <div class="order-btn fn-clear" style="border: 1px solid rgb(0, 170, 0);">
            <div class="submit-btn">
                <input type="button" class="btn" id="submitBtn1" value="微信支付" onclick="sub()" style="background-color: #00AA00;">
            </div>
        </div>
        <%}
          else
          { 
        %>
        <div class="order-btn fn-clear" style="border: 1px solid #AAAAAA;">
            <div class="submit-btn">
                <input type="button" class="btn" id="Button1" value="微信支付" disabled="disabled" style="background-color: #AAAAAA;">
            </div>
        </div>
        <%
            } %>
    </div>
    <footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                  <span style="display:block; padding-bottom:5px;  line-height:20px;">服务热线：<a href="tel:<%=phone %>"><%=phone %></a></span>
          </div>
      </footer>
    <script type="text/javascript">
        // 两秒后模拟点击
        setTimeout(function () {
            // IE
            if (document.all) {
                document.getElementById("submitBtn1").click();
            }
            // 其它浏览器
            else {
                var e = document.createEvent("MouseEvents");
                e.initEvent("click", true, true);
                document.getElementById("submitBtn1").dispatchEvent(e);
            }
        }, 1500);
    </script>
</asp:Content>
