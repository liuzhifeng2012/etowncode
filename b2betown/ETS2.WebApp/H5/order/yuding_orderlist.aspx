<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="yuding_orderlist.aspx.cs" Inherits="ETS2.WebApp.H5.order.yuding_orderlist" %>

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
    <script src="/Scripts/common.js" type="text/javascript"></script>

    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var userid = $("#hid_userid").val();
            var comid = $("#hid_comid").val();


            SearchList(1);

            //根据公司id获得关注作者信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage  btn btn-follow\">关注我们</a>");
                }
            });
            //装载产品列表
            function SearchList(pageindex) {
                $("#loading").show();
                var key = $("#key").val();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                <%if (state==1){ %>
                $("#pageindex").val(pageindex);
                $.post("/JsonFactory/OrderHandler.ashx?oper=yudingOrderPageList", { pageindex: pageindex, openid: $("#hid_openid").val(), AccountId: $("#hid_AccountId").val() }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $("#loading").hide();
                        $("#content").html("<div class=\"empty-list\" style=\"margin-top:30px;\"> 请 <a href=\"Login.aspx?come=/h5/order/yuding_orderlist.aspx\">登录</a> 后查看我的订单！</div>");
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();

                        if (data.totalCount == 0) {
                            $("#content").html("<div class=\"empty-list\" style=\"margin-top:30px;\"> <h4>哎呀，没有查到未使用的预订订单？</h4></div>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#content");
                            setpage(data.totalCount, pageSize, pageindex);
                        }
                    }
                })
                <%}else{ %>
                 $("#content").html("<div class=\"empty-list\" style=\"margin-top:30px;\"> <h4>验证页面失效，请重新扫码进行验证！</h4></div>");
                <%} %>

            }



            var stop = true;


            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#pageindex").val()) + 1;
                        var pageSize = parseInt($("#num").val()) + 10;

                        stop = false;

                        SearchList(pageindex);

                    }
                }
            });


            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchList(page);

                        return false;
                    }
                });
            }

            $(".js-cancel").click(function () {
                $("#QJwxuFqolZ").hide();

            });

            $(".response-area-minus").click(function () {
                var num = parseInt($(".txt").val());
                if (num <= 1) {
                    $(".number").val(1);
                } else {
                    $(".number").val(num - 1);
                }
            });

            $(".response-area-plus").click(function () {
                var num = parseInt($(".number").val());
                var allnum = parseInt($("#hid_num").val());

                if (num < allnum) {
                    $(".number").val(num + 1);
                } else {
                    $(".number").val(allnum);
                }
            });

            $("#number").hover(function () {
                var num = parseInt($(".number").val());
                var allnum = parseInt($("#hid_num").val());
                if (num > allnum) {
                    $(".number").val(allnum);
                }
            });

            $(".js-confirm-it").click(function () {
                $("#loading").show();
                //预订产品 走独立的流程


                var num = parseInt($(".txt").val());
                var id = $("#hid_id").val();
                var pno = $("#hid_pno").val();
                //直接订购提交预订

               
                    $.post("/JsonFactory/EticketHandler.ashx?oper=zizhueconfirm", { id: id, pno: pno, usenum: num, comid: $("#hid_comid").val() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#loading").hide();
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            alert("验证成功！");
                            location.href = "order.aspx";
                            return;
                        }
                    })
                    //alert("确定");
                    return true;

            });


        })

        function querenyanzheng(id,pno,proname,num,imgurl,phone,name,date) {
            $("#QJwxuFqolZ").show();
            $("#number").val(num);
            $("#hid_num").val(num);
            $("#hid_id").val(id);
            $(".title").html(proname);
            $("#hid_pno").val(pno);
            $("#qrimg").attr("src", imgurl);
            $(".yuyue_phone").html(phone);
            $(".yuyue_name").html(name);
            $(".yuyue_date").html(date);
        }



    </script>

    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    
    <!-- CSS -->
    <link rel="stylesheet" href="/Styles/cart.css" onerror="_cdnFallback(this)">  
    <link rel="stylesheet" href="/h5/order/css/css.css" onerror="_cdnFallback(this)">  
    <link rel="stylesheet" href="/h5/order/css/css1.css" onerror="_cdnFallback(this)">  
        <link  href="css/bottommenu.css" rel="stylesheet">  
  </head>

<body class=" ">

  <div class="header">
        <!-- ▼顶部通栏 -->
                                
            <div class="js-mp-info share-mp-info">
            <a class="page-mp-info" href="default.aspx">
                <img class="mp-image" width="24" height="24" src="<%=logoimg %>"/>
                <i class="mp-nickname">
                    <%=title %>                </i>
            </a>
           <div class="links">
                
                                                                </div>
        </div>
        <!-- ▲顶部通栏 -->
</div> 
        <!-- container -->
    <div class="container ">
       <div class="content"   style="min-height: 388px;">
          <div id="backs-list-container">
			<li class="block block-order animated">
		        <div id="content"></div>
   
	        </li>
		
		

        </div>        
        <div class="footer">
                    <!-- 商家公众微信号 -->
            <div class="copyright">
            <div class="ft-copyright">
               <a href="#">易城商户平台技术支持</a>
            </div>
            </div>    
       </div>           
   </div>
  </div>

  <div id="QJwxuFqolZ" class="sku-layout sku-box-shadow" style="overflow: hidden; visibility: visible; opacity: 1; bottom: 0px; left: 0px; right: 0px; transform: translate3d(0px, 0px, 0px); position: fixed; z-index: 1100; transition: all 300ms ease 0s; display: none;">
        <div class="layout-title sku-box-shadow name-card sku-name-card">
            <div class="thumb"><img id="qrimg" src="/Images/defaultThumb.png" alt=""></div>
            <div class="detail goods-base-info clearfix">
                <p class="title c-black ellipsis"></p>
                
            </div> 
            <div class="js-cancel sku-cancel">
                <div class="cancel-img"></div>
            </div>
    </div>
    
    <div style="height:; " class="adv-opts layout-content">
        <div id="travlenum_view" class="goods-models js-sku-views block block-list block-border-top-none">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>使用数量</label>
		</dt>
		<dd>
		<dl class="clearfix">
				<div class="quantity">
				<div class="response-area response-area-minus"></div>            
				<button disabled="disabled" class="minus " type="button"></button>            
				<input id="number" class="txt" value="1" type="number">            
				<button class="plus" type="button"></button>            
				<div class="response-area response-area-plus"></div>            
				<div class="txtCover"></div>        </div>
				<div class="stock pull-right font-size-12">
			</div>
		</dl>
		</dd></dl>

		</div>
        <div id="travlephone_view" class="goods-models js-sku-views block block-list block-border-top-none">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>预约人</label>
		</dt>

        <dl class="clearfix">
				<div class="yuyue_name" style="float: right;margin-top: 10px;">
		        </div>
		</dl>
		</dd>
		</dl>
		</div>

        <div id="Div1" class="goods-models js-sku-views block block-list block-border-top-none">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>预约人手机</label>
		</dt>

        <dl class="clearfix">
				<div class="yuyue_phone" style="float: right;margin-top: 10px;" >
		        </div>
		</dl>
		</dd>
		</dl>
		</div>

        <div id="Div2" class="goods-models js-sku-views block block-list block-border-top-none">
		<dl class="clearfix block-item">
		<dt class="model-title sku-num pull-left">                    
		<label>预约日期</label>
		</dt>

        <dl class="clearfix">
				<div class=" yuyue_date" style="float: right;margin-top: 10px;">
		        </div>
		</dl>
		</dd>
		</dl>
		</div>

        <div class="confirm-action content-foot">
    <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark">请在与预约联系人确认后，立即使用</a>
	</div>
    </div>
</div>


 <script type="text/x-jquery-tmpl" id="ProductItemEdit">
<div class="header">
				<span class="font-size-12">订单号:${Id}</span>
		</div>
 <hr class="margin-0 left-10">
		<div class="block block-list block-border-top-none block-border-bottom-none" id="prolist${Id}">
    		<div class="block-item name-card name-card-3col clearfix">
                <a href="#" class="thumb">
                            <img src="${Imgurl}" height="60">
                </a>
        		<div class="detail">
            		<h3>${Proname}</h3>
                <p class="sku-detail ellipsis js-toggle-more">
                    <span class="c-gray">
                    </span>
                </p>
				</div>
				<div class="right-col">
					<div class="price"><span><a href="javascript:;" class="btn btn-in-order-list" onclick="querenyanzheng(${Eticket.Id},'${Pno}','${Proname}',${Eticket.Use_pnum},'${Imgurl}','${U_phone}','${U_name}','${jsonDateFormatKaler(U_traveldate)}')" >立即使用</a></span></div>
					<div class="num">
						<span>
                          {{if Eticket !=null}}
                                {{if Eticket.Use_pnum>0}}
                             可用数量：${Eticket.Use_pnum}
                                {{else}}
                                已验证
                                {{/if}}
                            {{else}}
                                已验证
                            {{/if}}
                        </span>
					</div>
				</div>
   		 </div>
   
	</div>

    </script>

    <div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_AccountId" value="<%=AccountId %>" />
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_num" value="0" />
    <input type="hidden" id="hid_pno" value="0" />
    <input type="hidden" id="pageindex" value="1" />
    
</body></html>
