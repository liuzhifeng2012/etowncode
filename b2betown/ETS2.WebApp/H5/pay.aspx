<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true"
    CodeBehind="pay.aspx.cs" Inherits="ETS2.WebApp.H5.pay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/H5/pay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            if (isWeiXin()) {//微信浏览器
                $(".wxpay").show();
            } else {//其他浏览器
                $(".wxpay").hide();
                $(".payshouming").show();
            }
        })
        function isWeiXin() {
            var ua = window.navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == 'micromessenger' && parseFloat(navigator.appVersion) >= 5) {
                return true;
            } else {
                return false;
            }
        }
        function goalipay() {
            location.href = "pay_by/WebForm1.aspx?out_trade_no=<%=orderid%>"
        }
        function gowxpay() {
            location.href = "<%=wxpaylinkurl %>"
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <!-- 公共页头  -->
    <a id="goBack" href="javascript:history.go(-1);">
        <header class="header"  style=" background-color: #3CAFDC;">
         <h1>选择支付方式</h1>
        <div class="left-head">
          <a href="javascript:history.go(-1);" class="tc_back head-btn">
             <span class="inset_shadow"><span class="head-return"></span></span>
          </a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none">
              <span class="inset_shadow"><span class="head-home"></span></span>
          </a>
        </div>
    </header>
    </a>
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

    <!-- 页面内容块 -->
    <div class="body">



        <div class="body-content">
            <%if (order_type == 1)//正常订单
              {
                  if (servertype == 2 || servertype == 8)//当地游；跟团游
                  {
                      if (travel_proid > 0)
                      {
            %>
            <dl>
                <dt>产品编号：</dt>
                <dd>
                    <%=travel_proid%></dd>
            </dl>
            <%
}
                  }
            %>
            <dl>
                <dt>产品名称：</dt>
                <dd>
                    <%=proname%></dd>
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
              else if (servertype == 2 || servertype == 8 || servertype == 10)//当地游；跟团游；旅游大巴
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
            <%} %>
            <dl>
                <dt>订单金额：</dt>
                <dd class="font-orange">
                    <%=p_totalpricedesc%></dd>
            </dl>
            <dl>
                <dt>联系手机：</dt>
                <dd>
                    <%=u_mobile%></dd>
            </dl>
            <dl>
                <dt>商家：</dt>
                <dd>
                    <%=comName%></dd>
            </dl>
            <%
                if (paystatus == 2)//已支付
                {
            %>
            <dl>
                <dt>支付状态：</dt>
                <dd>
                    已支付</dd>
            </dl>
            <%
                }
                if (orderstatus != "等待对方付款")
                {
            %>
            <dl>
                <dt>订单状态：</dt>
                <dd style="font-size: 16px; font-weight: bold;">
                    <%=orderstatus%>
                </dd>
            </dl>
            <%
                }

              }
              if (order_type == 2)//充值订单
              {
            %>
            <dl>
                <dt style="font-size: 18px;"><%=proname %> </dt>
                <dd>
                    &nbsp;&nbsp;
                </dd>
            </dl>
            <dl>
                <dt></dt>
                <dd style="font-size: 16px; text-align: right;">
                    合计(需在线支付):￥<%=p_totalpricedesc %>
                </dd>
            </dl>
            <%
                }
            %>
        </div>
        <%
            if (orderstatus == "等待对方付款")//订单状态：等待对方付款
            {
                if (comid == 1148 || comid == 112 || comid == 1194 || comid == 2607)//易游只是显示微信支付
                {
        %>
        <div class="body-content marginTop">
            <button id="Button2" onclick="gowxpay();" type="button" data-pay-type="baiduwap"
                class=" wxpay btn-pay btn btn-block btn-large btn-baiduwap  btn-green">
                微信支付</button>
                <p style="margin-top: 10px; display:none;" class="payshouming">
                请使用微信扫码进行支付</p>
        </div>
        <%
}
                else//其他商户都显示
                {
        %>
        <div class="body-content">
            <button id="Button1" onclick="gowxpay();" type="button" data-pay-type="baiduwap"
                class="btn-pay btn btn-block btn-large btn-baiduwap  btn-green wxpay">
                微信支付</button>

            <button id="webalipay" onclick="goalipay();" type="button" data-pay-type="baiduwap"
                class="btn-pay btn btn-block btn-large btn-baiduwap ">
                支付宝支付</button>
            <p style="margin-top: 10px;">
                如遇到屏蔽支付宝，请点击右上角 “在浏览器中打开”进行支付。单笔限额 50000元</p>

        </div>
        <%
            }
            }
        %>
        <%--<div class="body-content" onclick="this.style.background = '#e9eff5';">
            <a href="/tenpay/payRequest.aspx?order_no=<%=orderid%>&product_name=<%=proname %>&order_price=<%=p_totalprice %>&remarkexplain=<%=proname %>"
                class="font-gray">
                <h3>
                    财付通快捷支付</h3>
                <p style="margin-top: 10px;">
                    单笔限额 50000元</p>
                <em class="ico-right"></em></a>
        </div>--%>
    </div>
    <footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                  <span style="display:block; padding-bottom:5px;  line-height:20px;">服务热线：<a href="tel:<%=phone %>"><%=phone %></a></span>
          </div> 
      </footer>
       <input type="hidden" id="hid_comid" name="hid_comid" value="<%=comid %>" />
</asp:Content>
