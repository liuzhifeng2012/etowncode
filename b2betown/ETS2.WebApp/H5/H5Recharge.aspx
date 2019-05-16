<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true" CodeBehind="H5Recharge.aspx.cs" Inherits="ETS2.WebApp.H5.H5Recharge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 页面样式表 -->
    <style type="text/css">
        .none
        {
            display: none;
        }
        input:-moz-placeholder {
        color:#DDE;
        }
        input::-webkit-input-placeholder {
         color:#DDE;
        }
    </style>
    <script type="text/jscript">
        $(function () {
            $("#submitBtn1").click(function () {
                $("#loading").show();
                var comid = $("#hid_com").val();
                var price = $("#Recharge").val();
                if (price > 500) {
                    showErr("金额小于等于500元");
                    return;
                }
                var openid = $("#hid_openid").val();
                //提交
                $.post("/JsonFactory/OrderHandler.ashx?oper=H5Recharge", { comid: comid, openid: openid, price: price }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#loading").hide();
                        showErr("充值 Error");
                        return;
                    }
                    if (data.type == 100) {
                        location.href = "pay.aspx?orderid=" + data.msg + " &comid=" + comid;
                        return;
                    }

                })
            })
        });
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
    <!-- 页面样式表 -->
    <link href="../Styles/H5/Order.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 公共页头  -->
        <a id="goBack" href="javascript:history.go(-1);">
        <header class="header">
                    <h1>预付款充值</h1>
        <div class="left-head">
          <a  href="javascript:history.go(-1);" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
    </a>
    <div class="container" style=" margin-top:30px;">
    <div class="w-item marginTop">
                <dl class="in-item fn-clear">
                    <dt>预付款</dt>
                    <dd>
                        <input type="tel" id="Recharge" name="Recharge" maxlength="11" placeholder="输入充值预付款金额"value="" class="writeok"/></dd>
                </dl>
            </div>
    </div>
    <div class="order-btn fn-clear">
            <div class="submit-btn">
                <input type="button" class="btn" id="submitBtn1" value="在线充值"/>
            </div>
        </div>
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_com" value="<%=comid %>" />
</asp:Content>
