<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="ETS2.WebApp.H5.order.Cart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html class="no-js " lang="zh-CN"><head>
    <meta charset="utf-8">
    <meta name="keywords" content="<%=title %>">
    <meta name="description" content="">
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">

    <title><%=title %></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
     <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var userid = $("#hid_userid").val();
            var comid = $("#hid_comid").val();

            SearchList(1);


            //装载产品列表
            function SearchList(pageindex) {
                var key = $("#key").val();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.post("/JsonFactory/OrderHandler.ashx?oper=usercartlist", { userid: userid, comid: comid }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        // $.prompt("查询渠道列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#content");
                            //setpage(data.totalCount, pageSize, pageindex);
                        }
                        summoney();
                    }
                })
            }



            $("#all").click(function () {

                if ($(this).attr("checked")) {
                    $("[name='Id']").attr("checked", 'true'); //全选
                }
                else {

                    $("[name='Id']").removeAttr("checked"); //取消全选
                }
            })

            $("#confirmButton").click(function () {
                var cartid = "";

                $("[name='Id']").each(function () {
                    if ($(this).attr("checked")) {
                        cartid += $(this).val() + "_";
                    }
                })

                if (cartid == "") {
                    alert("请选择结算的产品");
                    return;
                }

                window.location.href = "/wxpay/micromart_pay_1_" + cartid + ".aspx";
            })

            $(".js-cancel-order").click(function () {
                var name = $(".js-cancel-order").html();

                if (name == "编辑") {
                    $(".js-cancel-order").html("完成");
                    $(".js-go-pay").addClass("hide");
                    $(".j-delete-goods").removeClass("hide");

                } else {
                    $(".js-cancel-order").html("编辑");
                    $(".js-go-pay").removeClass("hide");
                    $(".j-delete-goods").addClass("hide");

                }
            })

            $(".j-delete-goods").click(function () {
                var cartid = "";
                $("[name='Id']").each(function () {
                    if ($(this).attr("checked")) {
                        cartid += $(this).val() + ",";
                    }
                })

                if (proid == "") {
                    alert("请选择需要移除购物车的产品");
                    return;
                }



                if (confirm("请确认是否产品移除购物车?")) {
                    $("[name='Id']").each(function () {
                        if ($(this).attr("checked")) {
                            var pro_id = $(this).val();
                            $.post("/JsonFactory/OrderHandler.ashx?oper=agentdelcart", { userid: $("#hid_userid").val(), comid: $("#hid_comid").val(), cartid: cartid }, function (data) {
                                data = eval("(" + data + ")");
                                if (data.type == 1) {
                                }
                                if (data.type == 100) {
                                    $("#prolist" + pro_id).remove();
                                    return;
                                }
                            })
                            i = i + 1;
                        }
                    })
                    summoney();
                }



            })




        })

        function addnum(proid, speciid, cartid, money) {
            var num = $("#u_num" + cartid).val();
            num++;
            $("#u_num" + cartid).val(num);
            addcart(proid, speciid, cartid);

            $("#sum" + cartid).html(num * money);

        }

        function reducenum(proid, speciid, cartid, money) {
            var num = $("#u_num" + cartid).val();
            num--;
            if (num == 0) {
                if (confirm("是否产品移除购物车?")) {

                    $.post("/JsonFactory/OrderHandler.ashx?oper=agentdelcart", { userid: $("#hid_userid").val(), comid: $("#hid_comid").val(), proid: proid, cartid: cartid, speciid: speciid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                        }
                        if (data.type == 100) {
                            $("#prolist" + proid).remove();
                            summoney();
                            return;
                        }
                    })
                }
            }

            if (num <= 0) {
                num = 1
            }
            $("#u_num" + cartid).val(num);
            reducecart(proid, speciid, cartid, num);
            $("#sum" + cartid).html(num * money);
          

        }

        function addcart(proid, speciid, cartid) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { userid: $("#hid_userid").val(), comid: $("#hid_comid").val(), proid: proid, cartid: cartid, speciid: speciid, u_num: 1 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    summoney();
                }
                if (data.type == 100) {
                    //$("#cart").show();
                   // summoney();
                }
            })
           // summoney();
        }
        function reducecart(proid, speciid, cartid, num) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentreducecart", { userid: $("#hid_userid").val(), comid: $("#hid_comid").val(), proid: proid, speciid: speciid, cartid: cartid, u_num: num }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    summoney();
                }
                if (data.type == 100) {
                    //$("#cart").show();
                    summoney();
                }
            })
        }


        function sub() {
            $("[name='Id']").each(function () {
                if ($(this).attr("checked")) {
                }
                else {
                    $("#all").removeAttr("checked");
                }

            })
            summoney();
        }


        function summoney() {
            var cartid = "";
            var speciid = "";
            $("[name='Id']").each(function () {
                if ($(this).attr("checked")) {
                    cartid += $(this).val() + ",";
                }
            })

            if (cartid == "") {
               // alert("请选择结算的产品");
                $(".c-orange").html("￥ 0 ");
                return;
            }

            $.post("/JsonFactory/OrderHandler.ashx?oper=sumprice", { userid: $("#hid_userid").val(), comid: $("#hid_comid").val(), cartid: cartid}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    $(".c-orange").html("￥ " + data.msg);
                }
            })
            
        }

    </script>

    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    
    <!-- CSS -->
    <link rel="stylesheet" href="/Styles/cart.css" onerror="_cdnFallback(this)">  
        <link  href="css/bottommenu.css" rel="stylesheet">  
  </head>

<body class=" ">
        <!-- container -->
    <div class="container ">
                <div class="content"  style="min-height: 288px;">
					<div class="tabber  tabber-n3 tabber-double-11 clearfix">
                        <a class="" href="Default.aspx"><< 继续购物</a>
						<a class="active" class="" href="Cart.aspx">购物车</a>
						<a class="" href="Order.aspx">订单记录</a>
					</div>
        <div id="backs-list-container" style="margin-top: 20px;">
			<li class="block block-order animated">
            <div class="header">
				<span class="font-size-12"> <a href="Default.aspx" class="page-mp-info">店铺: <%=title %></a></span>
                <a class="js-cancel-order pull-right font-size-12 c-blue" href="javascript:;">编辑</a>
		</div>
		<div id="content"></div>
   
	</li>
		

        <div class="js-bottom-opts bottom-fix" style="padding:0;">
			<div class="bottom-cart clear-fix">
				<div class="select-all"><label> <input id="all"  type="checkbox" checked="checked" onclick="summoney();"  />全选</label></div>
				<div class="total-price">合计：<span class="js-total-price c-orange">0</span>元</div>
				<button href="javascript:;" id="confirmButton" class="js-go-pay btn btn-orange-dark font-size-14" >结算</button>
                <button href="javascript:;" class="j-delete-goods btn font-size-14 btn-red hide">删除</button>
				
			</div>
		</div>
		
		<div class="empty-list" style="margin-top:30px; display:none;">
    <!-- 文本 -->
    <div>
        <h4>哎呀，购物车空了 去逛逛？</h4>
        <p class="font-size-12">不落单一起团</p>
    </div>
    </div>
    </div>        <div class="footer">
        <!-- 商家公众微信号 -->
<div class="copyright">
<div class="ft-copyright">
   <a href="#">易城商户平台技术支持</a>
</div>
</div>    </div>            </div>


 <script type="text/x-jquery-tmpl" id="ProductItemEdit">

 <hr class="margin-0 left-10">
		<div class="block block-list block-border-top-none block-border-bottom-none" id="prolist${Id}">
    		<div class="block-item name-card name-card-3col clearfix">
                <span style="float:left;" >
				<input name="Id" value="${Cartid}" data-id="${Speciid}" data-cart="${Cartid}" type="checkbox" checked="checked" onclick="summoney();"  />

                </span>
                <a href="Pro.aspx?id=${Id}" class="thumb">
                    <img src="${Imgurl}" height="60">
                </a>
        		<div class="detail">
            		<h3>${Pro_name}</h3>
                <p class="sku-detail ellipsis js-toggle-more">
                    <span class="c-gray">
                    </span>
                </p>
				</div>
				<div class="right-col">
					<div class="price">￥<span>${Advise_price}</span></div>
					<div class="num">
						<div class="clearfix">
                            <div class="quantity">
				                <div class="response-area response-area-minus"  onclick="reducenum('${Id}','${Speciid}','${Cartid}','${Advise_price}')"></div>            
				                <button disabled="disabled" class="minus " type="button"></button>            
				                <input id="u_num${Cartid}" class="txt" value="${U_num}" type="number">     
                                <input id="price${Cartid}" name="price" type="hidden" value="${Advise_price*U_num}" />       
				                <button class="plus" type="button"  ></button>            
				                <div class="response-area response-area-plus" onclick="addnum('${Id}','${Speciid}','${Cartid}','${Advise_price}')"  ></div>            
				                <div class="txtCover"></div>        
                                </div>

                                
						</div>
					</div>
				</div>
   		 </div>
    
	</div>

    </script>


    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
    <input id="hid_sumprice" type="hidden" value="0" />
    <input id="hid_editstate" type="hidden" value="1" />
</body></html>
