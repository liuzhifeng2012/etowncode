<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommonAddrlist.aspx.cs"
    Inherits="ETS2.WebApp.Agent.m.CommonAddrlist" MasterPageFile="/Agent/m/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <title>常用地址编辑</title>
    <!-- CSS -->
    <link rel="stylesheet" href="/h5/order/css/css4.css" />
    <link rel="stylesheet" href="/h5/order/css/css1.css" />
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            SearchAddressList(1);
            function SearchAddressList(pageindex) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=addresspagelist",
                    data: { pageindex: pageindex, pagesize: 10, agentid: $("#hid_agentid").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            return;
                        }
                        if (data.type == 100) {

                            if (data.totalCount == 0) {
                                if (pageindex == 1) {
                                    //                                    $("#tblist").html("查询数据为空");
                                    window.open("CommonAddrEdit.aspx?isshopcart=" + $("#hid_isshopcart").trimVal() + "&proid=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self");
                                }
                            } else {
                                if (pageindex == 1) {
                                    $("#tblist").empty();
                                }

                                stop = true;
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                $("#hid_pageindex").val(pageindex);
                            }
                        }
                    }
                })
            }

            var stop = true;
            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#hid_pageindex").val()) + 1;

                        stop = false;
                        SearchAddressList(pageindex);
                    }
                }
            });

            $("#submitsave").click(function () { 
                    window.open("CommonAddrEdit.aspx?isshopcart=" + $("#hid_isshopcart").trimVal() + "&proid=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self"); 
                
            })
        })

        function chooseclick(addrid) {
           var isshopcart = $("#hid_isshopcart").trimVal();
            if (isshopcart == 1) { 
            window.open("ShopCartSales.aspx?addrid=" + addrid + "&agentid="+$("#hid_agentid").trimVal()+"&proid=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self");
            }else{ 
             window.open("pro_sales.aspx?addrid=" + addrid + "&id=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self");
            }
        }
        function editclick(addrid) {
            window.open("CommonAddrEdit.aspx?isshopcart=" + $("#hid_isshopcart").trimVal() + "&addrid=" + addrid + "&proid=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal(), target = "_self");
        }
        function delclick(addrid) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=deladdr", { addrid: addrid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    showErr("删除成功");
                }
            })
        }

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
            $('<div id="showMsg"><div class="msg-title">温馨提示</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr(\'' + a + '\')">知道了</a></div></div>').appendTo("body");
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
        function hideErr(a) {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #showMsg").hide();
            if (a == '删除成功') {
                //alert("g--" + $("#hid_isshopcart").trimVal());
                location.href = "CommonAddrlist.aspx?isshopcart=" + $("#hid_isshopcart").trimVal() + "&proid=" + $("#hid_proid").trimVal() + "&unum=" + $("#hid_unum").trimVal() + "&comid=" + $("#hid_comidtemp").trimVal();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div style="background-color: #FFFFFF;">
        <div class="action-container" style="width: 100%; text-align: right; padding-bottom: 10px;">
            <a id="submitsave" class="js-address-save btn btn-block btn-blue" style="width: 30%;
                margin-right: 20px;"  >新增</a>
            <%--  <a id="submitcannel" class="js-address-cancel btn btn-block"  style="width: 20%; float:right;">取消</a>--%>
        </div>
        <div class="app-inner inner-order peerpay-gift address-fm" style="display: ; height: 100%;"
            id="sku-message-poppage">
            <div class="block" id="tblist">
            </div>
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
     <div class="block-item" >
                <span class="form-row form-text-row" style="width:100%;">
                    <em class="form-text-label" style="width:50%; 
                        font-size: 16px;">${U_name} ${U_phone}</em> 
                       
                </span>
                <span class="form-row form-text-row"  style="width:100%;">
                    <em class="form-text-label" style=" color: #9ea5ac;width:100%;">${Province} ${City} ${Address} ${Code}</em>
                </span>
                 <span class="form-row form-text-row"  style="width:100%;padding-left:0; ">
                   <a id="A5" class="js-address-save btn btn-block"  style="width: 30%;" onclick="chooseclick('${Id}')">选定</a>
                   <a id="A1" class="js-address-save btn btn-block" style="width: 30%;"  onclick="editclick('${Id}')">编辑</a>                                
                   <a id="A2" class="js-address-save btn btn-block"    style="width: 30%;"  onclick="delclick('${Id}')">删除</a>  
                 </span>
            </div>
        </script>
        <input id="hid_pageindex" type="hidden" value="1" />
        <input type="hidden" id="hid_proid" value="<%=proid %>" />
        <input type="hidden" id="hid_unum" value="<%=unum %>" />
        <input type="hidden" id="hid_isshopcart" value="<%=isshopcart %>"/>
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
</asp:Content>
