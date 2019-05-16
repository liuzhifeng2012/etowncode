<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="order.aspx.cs" Inherits="ETS2.WebApp.Agent.m.order"
    MasterPageFile="/Agent/m/Site1.Master" %>

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
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <!-- CSS -->
    <link rel="stylesheet" href="/agent/m/css/cart.css" onerror="_cdnFallback(this)">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/morder.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
      
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comidtemp").trimVal();

            $("#div_comname").html("商户:" + $("#hid_company").trimVal());
            $("#h_comname").text($("#hid_company").trimVal());

            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    showErr("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getagentorderlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid, account: $("#hid_account").trimVal(), comid: comid, key: $("#key").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $(".empty-list").show();
                            $("#backs-list-container").find("li").hide();
                            return;
                        }
                        if (data.type == 100) {

                            if (data.totalCount == 0) {
                                if (pageindex == 1) {
                                    //                                    $("#list").html("查询数据为空");
                                    $(".empty-list").show();
                                    $("#backs-list-container").find("li").hide();
                                }
                            } else {
                                if (pageindex == 1) {
                                    $("#list").empty();
                                }

                                stop = true;
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                $("#hid_pageindex").val(pageindex);

                                $(".empty-list").hide();
                                $("#backs-list-container").find("li").show();

                                var oidstr = ""; //订单号列表
                                $.each($("input[name='hid_hzinsorderid']"), function (i, item) {
                                    oidstr += $(item).val() + ",";
                                })
                                if (oidstr != "") {
                                    oidstr = oidstr.substr(0, oidstr.length - 1);
                                    //查询慧择网保单列表
                                    $.post("/JsonFactory/OrderHandler.ashx?oper=GethzinsorderSearch", { oidstr: oidstr }, function (datat) {
                                        datat = eval("(" + datat + ")");
                                        if (datat.type == 1) {
//                                            alert("查询慧择网保单状态出错");
                                            return;
                                        }
                                        if (datat.type == 100) {
                                            for (var ii = 0; ii < datat.msg.length; ii++) {
                                                //                                                alert(datat.msg[ii].insureNum);
                                                $("#span_" + datat.msg[ii].insureNum).text(datat.msg[ii].effectiveStateStr);
                                                if (datat.msg[ii].effectiveState == 1)//未生效之前可以退保
                                                {
                                                    $("#back_" + datat.msg[ii].insureNum).show();
                                                }

                                            }
                                        }
                                    })
                                }
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
                        SearchList(pageindex);
                    }
                }
            });
        })
        function gobackprolist() {
            location.href = "/agent/m/Manage_sales.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goshopproject() {
            location.href = "/agent/m/Manage_sales.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goshopcart() {
            location.href = "/agent/m/ShopCart.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goorder() {
            location.href = "/agent/m/order.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function gofinane() {
            location.href = "/agent/m/Finane.aspx?comid=" + $("#hid_comidtemp").trimVal();
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
            $('<div id="showMsg"><div class="msg-title">温馨提示</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr()">知道了</a></div></div>').appendTo("body");
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
            $("#bgDiv, #showMsg").hide();

        }
        function openlink(id) {
            var comid = $("#hid_comid_temp").trimVal();
            location.href = "pro_sales.aspx?id=" + id + "&comid=" + $("#hid_comidtemp").trimVal();
        }
        function agentorderpay(oid) {
            var comid = $("#hid_comidtemp").trimVal();
            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=agentorderpay",
                data: { comid: comid, id: oid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        showErr(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        alert("结算成功");
                        location.href = "Order.aspx?comid=" + comid;
                    }
                }
            })

        }

        function restticketsms(oid, comid) {

            if (oid == '') {
                showErr("参数传递错误");
                return;
            }
            if (comid == '' || comid == '0') {
                alert("公司id传递错误");
                return;
            }
//            if (confirm("确认重发短信吗")) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=restticketsms",
                    data: { comid: comid, oid: oid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            showErr("重发短信失败");
                            return;
                        }
                        if (data.type == 100) {
                            showErr("重发成功");
                            return;
                        }
                    }
                })

//            }
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <header class="header" style=" background-color: #3CAFDC;">
          <h1 id="h_comname"></h1>
        <div class="left-head"> 
             <a href="/agent/m/default.aspx" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="home-10"></span></span>
              </a> 
            </div>
        <div class="right-head"> 
                <a href="loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>  
        </div>
        </header>
    <!-- container -->
    <div class="container ">
        <div class="tabber  tabber-n3 tabber-double-11 clearfix">
             <a href="javascript:goshopproject()"   >产品列表</a> <a class="active" href="javascript:goorder()">
                订单列表</a> <a href="javascript:gofinane()">财务记录</a><a href="javascript:goshopcart()">购物车</a>
        </div>
        <div class="layout-full-box pr mt10" id="div_comname">
        </div>
        <div id="backs-list-container" class="block">
            <li class="block block-order animated">
                <div class="block block-list block-border-top-none block-border-bottom-none" id="list">
                </div>
                <hr class="margin-0 left-10">
            </li>
            <div class="empty-list" style="margin-top: 30px;">
                <!-- 文本 -->
                <div>
                    <h4>
                        哎呀，暂无记录？</h4>
                </div>
                <!-- 自定义html，和上面的可以并存 -->
                <div>
                    <a href="#" onclick="javascript:gobackprolist();" class="tag tag-big tag-orange"
                        style="padding: 8px 30px; color: #F15A0C; text-decoration: underline;">去逛逛</a></div>
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
    <div class="sn-block mt15">
        <div class="cart-title wbox">
            <div class="wbox-flex">
                <p class="of ft-15">
                    订单号：<span class="gray">${Id}</span></p>
            </div>
            <div class="gray  ft-15">
                ${jsonDateFormat(U_subdate)}
            </div>
        </div>
        <ul class="sn-list cart-list cart-list-gt">
            <li>
                <div class="wbox-flex">
                    <a href="javascript:void(0);" class="pro-list" style="display: -webkit-box;
display: box;">
                        <div class="pro-img">
                            <img src="${ProImg}"
                                alt=""  >
                        </div>
                        <div class="pro-info wbox-flex">
                            <div class="pro-name" style=" margin-bottom:10px;">
                                ${Proname}
                            </div>
                            <div class="pr gray"  >
                                价格:<span  style="color:#FC7C25;">¥${Pay_price}<span> <span class="list-opra gray "  >数量：${U_num}</span>
                            </div>
                            <div class="gray" >
                                运费:<span   style="color:#FC7C25;">{{if Express==0}}包邮{{else}}¥${Express}{{/if}}</span>
                            </div>
                            <input type="hidden" value="${U_num}" name="partNumber">
                            <input type="hidden" value="${Pro_id}" name="productId">
                        </div>
                    </a> 
                </div>
            </li>
        </ul>
        <div class="cart-bottom wbox">
            <div class="wbox-flex">
                <p class="of" style="font-size:100%;">
                    合计： <span class="snPrice" style="color:#FC7C25;">¥{{if Order_state>1}} ${Paymoney} {{if Imprest1>=1}}+ ${Imprest1}{{/if}}{{if childreduce*Child_u_num>0}}-${childreduce*Child_u_num}{{/if}}{{else}}${Paymoney}{{/if}}</span>
                </p>
            </div>
            <div class="gray">
           {{if OrderType_Hzins==0}}<!--非慧择网订单-->
                {{if Order_state==1}}
                    {{if Order_type==1}}
                         未付款 <input type="button" onclick="agentorderpay('${Id}')"  value="立即结算"/> 
                    {{else}}
                         未付款，充值失败
                    {{/if}}
                {{else}}
                 ${Order_state_str}

                 {{if Server_type==9}}
                    {{if Hotelinfo != null}}
                                                 <input type="button" onclick="alert('入住日期：${ChangeDateFormat(Hotelinfo.Start_date)}-离店日期：${ChangeDateFormat(Hotelinfo.End_date)}')"  value="详情"/> 
                     {{/if}}
                 {{/if}}
                  {{if Server_type==11}}
                      {{if Order_state==4}}
                            {{if Deliverytype==4}} 
                                   <a class="btn btn-orange btn-in-order-list" onclick="restticketsms('${Id}','${Comid}')"  href="#" >重 发</a>
                            {{else}}
                                <!--订单已发货，${Expresscom}：${Expresscode}-->
                            {{/if}}
                      {{/if}}
                  {{else}}
                      {{if Server_type==2||Server_type==8}}
                          {{if Order_state==4}}
                              <a class="btn btn-orange btn-in-order-list" onclick="restticketsms('${Id}','${Comid}')"  href="#" >重 发</a>
                          {{/if}}
                      {{else}}
                          {{if Order_state==4}}
                             {{if Warrant_type=="1"}}
                              <a class="btn btn-orange btn-in-order-list" onclick="restticketsms('${Id}','${Comid}')"  href="#" >重 发</a>
                             {{/if}}
                          {{/if}}
                      {{/if}} 
                  {{/if}} 
                {{/if}}

                 {{/if}}
                 {{if  OrderType_Hzins==1}}<!--慧择网订单 但没有生成真实保险订单-->
                                 <span id="span_hzinsorderstaus">${Order_state_str}</span>

                                <!--  {{if Pay_state==2}}
                                   {{if Order_state==2||Order_state==20}} 
                                 <input type="button" id="back_hzinsorderstaus${Id}" onclick="backticket_ch('退保申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退保"/> 
                                   {{/if}}
                                 {{/if}} 

                                 {{if Order_remark!=''}}
                                      <input type="button" onclick="showremark('${Order_remark}')"  value="备注"/>                   {{/if}}-->
                            {{/if}}

                            {{if  OrderType_Hzins==2}}<!--慧择网订单 并且已经生成真实保险订单-->
                                 <span id="span_${InsureNum}"></span>
                                  
                                  <!--{{if Order_state==4}}
                                 <input type="button" id="back_${InsureNum}" onclick="backticket_ch('退保申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${U_num}')"  value="退保" style="display:none;"/>            
                                 {{/if}}-->
                                 
                                  <a href="/agent/Hzins_detail.aspx?orderid=${Id}" target="_blank" style="text-decoration: underline">保单详情</a>   
                                  <!--保存生成真实保险订单的 本系统订单号-->
                                  <input type="hidden" name="hid_hzinsorderid" value="${Id}">
                            {{/if}}
            </div>
        </div>
    </div>
     
    </script>
    <input id="hid_pageindex" type="hidden" value="1" />
</asp:Content>
