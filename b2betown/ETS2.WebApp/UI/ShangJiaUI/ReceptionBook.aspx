<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceptionBook.aspx.cs"
    Inherits="ETS2.WebApp.UI.ShangJiaUI.ReceptionBook" %>

<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title id="js-meta-title">
        <%=Com_name%>
    </title>
    <!-- ▼ Common CSS -->
    <link rel="stylesheet" href="/Styles/pc/pc_Bootstrap.css">
    <link rel="stylesheet" href="/Styles/pc/pc_man.css">
    <!-- ▲ Common CSS -->
    <!-- ▼ App CSS -->
    <link rel="stylesheet" href="/Styles/pc/list_css.css">
    <link rel="stylesheet" href="/Styles/pc/list_css2.css">
    <link onerror="_cdnFallback(this)" href="/h5/order/css/css1.css" rel="stylesheet">
    <link onerror="_cdnFallback(this)" href="/h5/order/css/css3.css" rel="stylesheet">
    <link onerror="_cdnFallback(this)" href="/h5/order/css/css.css" rel="stylesheet">
    <link onerror="_cdnFallback(this)" href="/h5/order/css/base.css" rel="stylesheet">
    <link href="/h5/order/css/bottommenu.css" rel="stylesheet">
    <style>
        .content
        {
            margin: 0px auto;
        }
        .ui-datepicker {
    background-color: #FFF !important;
    margin-left: -20px;
    width: 200px !important;
}
.container {
    background-color: #ffffff;
}
.quantity {
    margin-top: 10px;
    padding-left:20px;
    float: left !important;
}
.block.block-list {
    margin-top: 0px  !important;
}
.sku-layout .model-title {
    width: 100px;
}
.iteminfo_parameter dd {
    float: left;
    display: inline;
    white-space: nowrap;
    text-align: left;
    color: #555;
    padding-right: 10px;
}
    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.cookie.2.2.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/swipe.js"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    $(function () {

      //日历
        var dateinput = $("input[isdate=yes]");
        $.each(dateinput, function (i) {
            $($(this)).datepicker();
        });
            


        var comid = $("#hid_comid").val();
        var proid = $("#hid_proid").val();



        $(".shop-info").hover(function () {
            $(".shop-info").addClass("hidestate");
            $(".shop-info").removeClass("hide");
        }, function () {
           $(".shop-info").addClass("hide");
           $(".shop-info").removeClass("hidestate");
        });
        $(".btn-fx-buy").click(function () {
            $(".popover-goods").show();
            shake('popover-goods');
        });

        $(".popover-goods").hover(function () {
            $(".popover-goods").show().jshaker();
        }, function () {
            $(".popover-goods").delay(10).hide(0);
        });

        $(".swiper-pagination-switch").click(function () {
            var index = $(this).attr("data-index");
            var removepx = -(index * 400);
            $(".swiper-wrapper").css({ "left": removepx + "px" });
        });



        $("#traveldate").change(function(){ 
            getworktime($(this).val());
         }); 




        $(".js-cancel").click(function () {
            $("#suborder").hide();
            $("#hid_proid").val(0);
        });




        $(".js-confirm-it").click(function () {

            $("#loading").show();
            $(this).val("提交中...").attr("disabled", "disabled");
            //预订产品 走独立的流程
            var server_type=$("#hid_server_type").val();
            var num = parseInt($(".txt").val());
            var proid = $("#hid_proid").val();
            var action = $("#hid_action").val();
            var traveldate = $("#traveldate").val();
            var travelday = $("#travelday").val();
            var speciid = $("#hid_speciid").val();
            var manyspeci = $("#hid_manyspeci").val();
            var channelcoachid = $("#hid_channelcoachid").val();
             var Order_remark = $("#Order_remark").val();
                

            var travelname = $("#travelname").val();
            var travelphone = $("#travelphone").val();

                if(manyspeci==1){
                     if(speciid ==0){
                        alert("请选择规格");
                        $("#loading").hide();
                        $(this).val("提交订单").removeAttr("disabled");
                        return;
                     }
                }

                if(server_type==13){
                      if(channelcoachid ==0){
                        alert("请选择教练");
                        $("#loading").hide();
                        $(this).val("提交订单").removeAttr("disabled");
                        return;
                     }
                }

                if(server_type==12 || server_type==13){
                    if(traveldate ==""){
                        alert("请选择日期");
                        $("#loading").hide();
                        $(this).val("提交订单").removeAttr("disabled");
                        return;
                    }else{

                        traveldate = traveldate +" "+ travelday +":00:00";
                    }
                }

                

                if(travelname ==""){
                        alert("请填写预约人姓名");
                        $("#loading").hide();
                        $(this).val("提交订单").removeAttr("disabled");
                        return;
                }

                if(travelphone ==""){
                        alert("请填写预约人手机");
                        $("#loading").hide();
                        $(this).val("提交订单").removeAttr("disabled");
                        return;
                }

                if(Order_remark==""){
                    alert("请选择付款方式");
                        $("#loading").hide();
                        $(this).val("提交订单").removeAttr("disabled");
                        return;
                }

                //直接订购提交预订
                 $.post("/JsonFactory/OrderHandler.ashx?oper=createorder", {Integral: 0, Imprest: 0, openid: $("#hid_openid").val(), proid: proid,speciid:speciid, payprice: 1.00, u_num: num, u_name: travelname, u_phone: travelphone, u_traveldate: traveldate, comid: comid,channelcoachid:channelcoachid,autosuccess:1,Order_remark:Order_remark}, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#loading").hide();
                            $(this).val("提交订单").removeAttr("disabled");
                             alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $(this).val("提交订单").removeAttr("disabled");
                            $("#suborder").hide();
                            $("#suborder_proname").html("");
                            $("#suborder_price").html(0);
                            $("#hid_proid").val(0);
                            $("#hid_server_type").val(0);
                            $("#hid_manyspeci").val(0);
                            $("#hid_speciid").val(0);
                            $("#hid_channelcoachid").val(0);
                            $("#traveldate").val("");
                            $("#travelname").val("");
                            $("#travelphone").val("");

                            $("#loading").hide();
                            
                            alert("提交成功");
                            return;
                        }

                })

            
        });


        
    

    });

    //装载产品列表
            function SearchPorList(pageindex, pageSize,menuid,projectid,viewlist) {
                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=SelectMenupagelist",
                    data: { comid: <%=comid %>, pageindex: pageindex, pagesize: pageSize,menuid:menuid,projectid:projectid,allview:1 },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                           // $("#list").hide();
                            //$("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            $("#"+viewlist).empty();
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#"+viewlist);

                        }
                    }
                })
            }
    
            function getworktime(date){
                 //查询日期
                  $("#loading").show();
                       $.post("/JsonFactory/UserHandle.ashx?oper=Getjiaolianworktime", {id:2394,date:date }, function (data) {
                                data = eval("(" + data + ")");
                                if (data.type == 1) {
                                    $("#loading").hide();
                                    return;
                                }
                                if (data.type == 100) {
                                    $("#loading").hide();
                                    $("#travelday").empty();


                                    if(data.msg !=""){
                                        var workh=data.msg;
                                        var workh_arr=workh.split(",");
                                        for(var i=0;i<workh_arr.length ;i++ ){
                                            if(workh_arr[i] !=""){
                                                $("#travelday").append("<option value='"+workh_arr[i]+"'>"+workh_arr[i]+"点</option>");
                                            }
                                        }
                                    }
                                     return;
                                }
                        })
            }

         
    </script>
</head>
<body class="theme theme--blue" style="">

<!-- ▼ Main header -->
<header class="ui-header">
    <div class="ui-header-inner clearfix">
        <div class="ui-header-logo">
            <a href="javascript:;" class="js-hover logo" data-target="js-shop-info">
        <%=Com_name%>              <span class="smaller-title hide"><%=Scenic_name %></span>
      </a>
        </div>
        <nav class="ui-header-nav">
            
    </div>
</header>
<!-- ▲ Main header -->

    <!-- ▼ Main container -->
    <div id="Maintop">
    </div>
    <div class="container goods-detail-main clearfix">
        <div class="content">
            <%  for (int i = 0; i < projectlist.Count; i++)
                {  %>
            <!-- 图片广告 -->
            <div class="shop_module_floor custom-image-swiper custom-image-swiper-single custom-image-swiper-single2">
                <div class="swiper-container">
                    <div class="swiper-wrapper">
                        <div class="swiper-slide">
                            <!-- 标题    -->
                            <div class="shop_module_hd">
                                <h4>
                                    <%=projectlist[i].Projectname%></h4>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- 商品区域 -->
            <ul id="list<%=projectlist[i].Id%>" class="sc-goods-list pic clearfix size-1 ">
                <script>
     $(function () {
        SearchPorList(1,8,0,<%=projectlist[i].Id%>,"list<%=projectlist[i].Id%>");
     })
                </script>
            </ul>
            <%} %>
        </div>
        <div class="popover popover-goods js-popover-goods" style="left: 632px; top: 44px;
            display: none;">
            <div class="popover-inner">
                <h4 class="title clearfix">
                    <span class="icon-weixin pull-left"></span>手机启动微信<br>
                    扫一扫购买</h4>
                <div class="ui-goods-qrcode">
                    <img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码"
                        class="qrcode-img">
                </div>
            </div>
        </div>
        <div class="popover popover-weixin js-popover-weixin" style="left: 971.5px; top: 161px;
            display: none;">
            <div class="popover-inner">
                <h4 class="title">
                    打开微信，扫一扫</h4>
                <h5 class="sub-title">
                    或搜索微信号：<%=weixinname %></h5>
                <div class="ui-goods-qrcode">
                    <img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码"
                        class="qrcode-img">
                </div>
            </div>
        </div>
        <div class="shop-info shop-info-fixed js-shop-info hide">
            <div class="container clearfix">
                <div class="js-async shop-qrcode pull-left">
                    <img src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>" alt="二维码"></div>
                <div class="shop-desc pull-left">
                    <dl>
                        <dt>
                            <%=Com_name %>
                        </dt>
                        <dd>
                        </dd>
                        <dt>微信扫描二维码，访问我们的微信店铺</dt>
                    </dl>
                </div>
                <span class="arrow"></span>
            </div>
        </div>
        <div class="js-notifications notifications">
        </div>
        <div class="back-to-top hide">
            <a href="#" class="js-back-to-top"><i class="icon-chevron-up"></i>返回顶部</a>
        </div>
        <div class="popover-logistics popover" style="display: none; top: 272px; left: 488px;">
            <div class="arrow">
            </div>
            <div class="popover-inner">
                <div class="popover-content">
                    <div class="logistics-content js-logistics-region" style="min-height: 30px;">
                    </div>
                </div>
            </div>
            <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
        <li class="goods-card goods-list small-pic card" data-pid="${Id}" data-name="${Pro_name}" data-price="${Advise_price}" data-type="${Server_type}" data-manyspeci="${Manyspeci}">
                    <a href="javascript:;" class="link js-goods clearfix"  data-goods-id="6655060" title="${Pro_name}">
					<div class="photo-block">
                        {{if (Ispanicbuy==1 || Ispanicbuy==2)}}
                            {{if (Limitbuytotalnum<=0)}}
                        <div class="Soldout"><img  src="image/5337034_100415061309_2.gif"  style="display: block;width:80px;padding-top:5px;padding-left:5px;"></div>
                            {{/if}}
                        {{/if}}
						<img class="goods-photo js-goods-lazy" data-width="640" data-height="640" data-src="${Imgurl}" src="${Imgurl}" style="display: block;">
					</div>
					<div class="info clearfix info-title info-price btn4">
						<p class="goods-title">${Pro_name}</p>
						<p class="goods-sub-title c-black hide">  ${Pro_explain}</p>
						<p class="goods-price">
                        {{if (Server_type==9)}}
                        <em>￥${HousetypeNowdayprice}</em>
                           {{else}}
                           <em>￥${Advise_price}</em>
                        {{/if}}    
						</p>
						<p class="goods-price-taobao">特价：￥${Advise_price}</p>   
                        {{if (Server_type==11)}}
                          <div class="goods-buy btn1"></div>
                        {{/if}}
					</div>
                    </a>
			</li>
            </script>
            <script type="text/x-jquery-tmpl" id="WxItemp"> 
               <li class="goods-cardwx  normalwx"> 
                 <div class="custom-messages single">
                    <a href="Article.aspx?id=${MaterialId}" class="clearfix">
                        <div class="custom-messages-image">
                                                <div class="image">
                                <img src="${Imgpath}" style="display: inline;" class="js-lazy" data-src="${Imgpath}">
                            </div>
                        </div>
                        <div class="custom-messages-content">
                            <h4 class="title">
                                ${Title}                    </h4>
                                                <div class="summary">
                                ${cutstr(Summary,240) }                 
                                 {{if Price!=0}}
                                    ¥${Price}<em>起</em>
                                {{/if}}
                        
                                 </div>
                    
                        </div>
                    </a>
                </div>
               </li>
            </script>
        </div>
    </div>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_userid" type="hidden" value="<%=userid %>" />
    <input id="hid_action" type="hidden" value="1" />
    <input id="hid_proid" type="hidden" value="0" />
    <input id="pageindex" type="hidden" value="1" />
    <input id="hid_speciid" value="0" type="hidden" />
    <input id="hid_channelcoachid" value="0" type="hidden" />
    <input id="hid_server_type" value="0" type="hidden" />
    <input id="hid_manyspeci" value="0" type="hidden" />
    

    <div id="loading" class="loading" style="display: none;">
        正在加载...
    </div>
    <div id="suborder" class="sku-layout sku-box-shadow" style="display: none; overflow: hidden;
        visibility: visible; opacity: 1; bottom: 0px; left: 0px; right: 0px; transform: translate3d(0px, 0px, 0px);
        position: fixed; z-index: 98; transition: all 300ms ease 0s;">
        <div class="layout-title sku-box-shadow name-card sku-name-card">
            <div class="thumb">
                <img src="/Images/defaultThumb.png" alt="">
            </div>
            <div class="detail goods-base-info clearfix">
                <p class="title c-black ellipsis" id="suborder_proname">
                </p>
                <div class="goods-price clearfix">
                    <div class="current-price c-black pull-left">
                        <span class="price-name pull-left font-size-14 c-orangef60">￥</span> <i class="js-goods-price price font-size-18 c-orangef60 vertical-middle sys_item_price"
                            id="suborder_price"></i><i style="display: none;" class="sys_item_mktprice">
                        </i>
                    </div>
                </div>
            </div>
            <div class="js-cancel sku-cancel">
                <div class="cancel-img">
                </div>
            </div>
        </div>
        <div style="height: ;" class="adv-opts layout-content">
            <div id="travlenum_view" class="goods-models js-sku-views block block-list block-border-top-none hide">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            数量</label>
                    </dt>
                    <dd>
                        <dl class="clearfix">
                            <div class="quantity">
                                <div class="response-area response-area-minus">
                                </div>
                                <button disabled="disabled" class="minus " type="button">
                                </button>
                                <input id="number" class="txt" value="1" type="number">
                                <button class="plus" type="button">
                                </button>
                                <div class="response-area response-area-plus">
                                </div>
                                <div class="txtCover">
                                </div>
                            </div>
                            <div class="stock pull-right font-size-12">
                            </div>
                        </dl>
                    </dd>
                </dl>
            </div>
            <div class="iteminfo_buying">
                <!--规格属性-->
                <div class="sys_item_spec"  style=" display:none;">
                </div>
                <!--规格属性-->
                                <!--教练-->
                <div class="sys_item_jiaolian" style=" display:none;">
                <dl class="clearfix iteminfo_parameter ">
			        <dd>教练</dd>
			        <dd>
				        <ul class="sys_spec_text sys_item_jiaolian_list">
                              
               
                        </ul>

			        </dd>
		        </dl>
                </div>
                <!--教练-->
            </div>
            <div id="travledate_view" class="goods-models js-sku-views block block-list block-border-top-none">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            预约日期:</label>
                    </dt>
                    <dl class="clearfix">
                        <div class="quantity">
                            <input name="traveldate" id="traveldate" class="txtinputsub " value=""  placeholder="日期"  readonly="readonly"  isdate="yes" type="text"/>
                        </div>
                    </dl>
                </dl>
            </div>
            <div id="travletime_view" class="goods-models js-sku-views block block-list block-border-top-none">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            预约时间:</label>
                    </dt>
                    <dl class="clearfix">
                        <div class="quantity">
                            <select id="travelday" class="txtinputsub" style="height: 30px; width: 123px;">
                                <option value="9">9点</option>
                                <option value="10">10点</option>
                                <option value="11">11点</option>
                                <option value="12">12点</option>
                                <option value="13">13点</option>
                                <option value="14">14点</option>
                                <option value="15">15点</option>
                                <option value="16">16点</option>
                            </select>
                        </div>
                    </dl>
                </dl>
            </div>
            <div id="travlename_view" class="goods-models js-sku-views block block-list block-border-top-none">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            预约人姓名:</label>
                    </dt>
                    <dl class="clearfix">
                        <div class="quantity">
                            <input id="travelname" class="txtinputsub" value="" placeholder="姓名" type="text">
                            
                        </div>
                    </dl>
                </dl>
            </div>
            <div id="travlephone_view" class="goods-models js-sku-views block block-list block-border-top-none">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            预约人手机:</label>
                    </dt>
                    <dl class="clearfix">
                        <div class="quantity">
                            <input id="travelphone" class="txtinputsub" value="" placeholder="手机" type="tel">
                        </div>
                    </dl>
                </dl>
            </div>

            <div id="Div1" class="goods-models js-sku-views block block-list block-border-top-none">
                <dl class="clearfix block-item">
                    <dt class="model-title sku-num pull-left">
                        <label>
                            支付方式:</label>
                    </dt>
                    <dl class="clearfix">
                        <div class="quantity">
                            <select id="Order_remark" class="txtinputsub" style="height: 30px; width: 123px;">
                                <option value="">请选择付款方式</option>
                                <option value="现金">现金</option>
                                <option value="刷卡">刷卡</option>
                                <option value="微信支付">微信支付</option>
                                <option value="支付宝">支付宝</option>
                             </select>
                        </div>
                    </dl>
                </dl>
            </div>
            
            <div class="confirm-action content-foot">
                <a href="javascript:;" class="js-confirm-it btn btn-block btn-orange-dark">提交订单</a>
            </div>
        </div>
    </div>
    <script>
        $(function () {
            new Swipe(document.getElementById('banner_box'), {
                speed: 500,
                auto: 3000,
                callback: function () {
                    var lis = $(this.element).next("ol").children();
                    lis.removeClass("on").eq(this.index).addClass("on");
                }
            });
        });
    </script>
    <script type="text/javascript" src="/Scripts/hilove.js"></script>
    <script type="text/x-jquery-tmpl" id="guige"> 

                <dl class="clearfix iteminfo_parameter sys_item_specpara" name="fatherzxguige" data-sid="${GuigeNum}">
			        <dd>${GuigeTitle}</dd>
			        <dd>
				        <ul class="sys_spec_text">
                
                            {{each GuigeValues}}  
                                <li data-aid="${$value.id}"  name="zxguige"><a href="javascript:;" title="${$value.Name}">${$value.Name}</a><i></i></li>
                            {{/each}}
               
                        </ul>

			        </dd>
		        </dl>
    </script>

    <script type="text/x-jquery-tmpl" id="jiaolian"> 
                
          <li data-aid="${channelid}"  name="zxjiaolian"><a href="javascript:;" title="${Name}">${Name}</a><i></i></li>
               
    </script>


    <script>


        //商品规格选择
        $(function () {

            var sys_item = {
                "mktprice": "", //face_price
                "price": "", //Advise_price
                "sys_attrprice": ""//规格价格json

            };

            $(".goods-card").live("click", function () {
                $(".js-confirm-it").val("提交订单").removeAttr("disabled");

                var id = $(this).attr("data-pid");
                var clickname = $(this).attr("data-name");
                var price = $(this).attr("data-price");
                var servertype = $(this).attr("data-type");
                var manyspeci = $(this).attr("data-manyspeci");

                $("#suborder").show();
                $("#suborder_proname").html(clickname);
                $("#suborder_price").html(price);
                $("#hid_proid").val(id);
                $("#hid_server_type").val(servertype);
                $("#hid_manyspeci").val(manyspeci);


                if (servertype == 13 || servertype == 12) {//预约/教练产品 显示出预约日期及时间
                    $("#travledate_view").show();
                    $("#travletime_view").show();
                } else {
                    $("#travledate_view").hide();
                    $("#travletime_view").hide();
                }
                if (servertype == 14) {
                    $("#travledate_view").show();
                    $("#travletime_view").hide();
                }





                if (servertype == 13) { //读取教练
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/CrmMemberHandler.ashx?oper=peopleList",
                        data: { comid: $("#hid_comid").val(), pageindex: 1, pagesize: 30 },

                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $(".sys_item_jiaolian").hide();
                                $(".sys_item_jiaolian_list").empty();
                                return;
                            }
                            if (data.type == 100) {

                                $(".sys_item_jiaolian").show();
                                $(".sys_item_jiaolian_list").empty();
                                $("#jiaolian").tmpl(data.msg).appendTo(".sys_item_jiaolian_list");

                            }


                        }
                    })
                } else {
                    $(".sys_item_jiaolian").hide();
                }



                //装载产品规格
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Selectguigelist",
                    data: { comid: $("#hid_comid").val(), proid: id },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $(".iteminfo_buying").hide();
                            $(".sys_item_spec").hide();
                            $(".sys_item_spec").empty();
                            return;
                        }
                        if (data.type == 100) {
                            $(".iteminfo_buying").show();

                            $(".sys_item_spec").show();
                            $(".sys_item_spec").empty();
                            $("#guige").tmpl(data.msg).appendTo(".sys_item_spec");

                        }
                    }
                })

                //获取产品规格的价格信息
                $.post("/JsonFactory/ProductHandler.ashx?oper=getguigepricearr", { proid: id }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        if (data.msg != "" && data.msg != "") {
                            sys_item.mktprice = data.face_price;
                            sys_item.price = data.advise_price;
                            sys_item.sys_attrprice = eval("(" + data.msg + ")");

                        }
                    }
                })
            })



            $("[name='zxjiaolian']").live("click", function () {
                var i = $(this);
                if (!!$(this).hasClass("selected")) {
                    $(this).removeClass("selected");
                    $("#hid_channelcoachid").val(0);
                } else {
                    $(this).addClass("selected").siblings("li").removeClass("selected");
                    $("#hid_channelcoachid").val($(this).attr("data-aid"));
                }


            })



            $("[name='zxguige']").live("click", function () {
                var i = $(this);
                if (!!$(this).hasClass("selected")) {
                    $(this).removeClass("selected");
                    i.removeAttr("data-attrval");
                } else {
                    $(this).addClass("selected").siblings("li").removeClass("selected");
                    i.parents('.clearfix').attr("data-attrval", $(this).attr("data-aid"))
                }
                getattrprice(); //输出价格

            })

            //获取对应属性的价格
            function getattrprice() {
                var defaultstats = true;
                var _val = '';
                var _resp = {
                    mktprice: ".sys_item_mktprice",
                    price: ".sys_item_price"
                }  //输出对应的class

                $("[name='fatherzxguige']").each(function () {

                    var i = $(this);
                    var v = i.attr("data-attrval");
                    if (!v) {
                        defaultstats = false;
                    } else {
                        _val += _val != "" ? "_" : "";
                        _val += v;
                    }
                })




                if (!!defaultstats) {
                    _mktprice = sys_item['sys_attrprice'][_val]['mktprice'];
                    _price = sys_item['sys_attrprice'][_val]['price'];
                    $("#hid_speciid").val(sys_item['sys_attrprice'][_val]['speciid'])

                } else {
                    _mktprice = sys_item['mktprice'];
                    _price = sys_item['price'];
                    $("#hid_speciid").val(0);

                }
                //输出价格
                $(_resp.mktprice).text(_mktprice);  ///其中的math.round为截取小数点位数
                $(_resp.price).text(_price);
                $("#suborder_price").text(_price);
            }
        })
    </script>
</body>
</html>
