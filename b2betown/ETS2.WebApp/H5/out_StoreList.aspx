<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="out_StoreList.aspx.cs" Inherits="ETS2.WebApp.H5.out_StoreList" %>

<html lang="zh-CN" class="no-js ">
<head>
    <meta charset="utf-8">
    <meta content="True" name="HandheldFriendly">
    <meta content="320" name="MobileOptimized">
    <meta content="telephone=no" name="format-detection">
    <meta content="on" http-equiv="cleartype">


    <link href="image/yz_fc.ico" rel="icon">
    <title><%=title %></title>

    <!-- meta viewport -->
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport">
    
    <!-- CSS -->
    <link onerror="_cdnFallback(this)" href="order/css/css1.css" rel="stylesheet">    
	<link onerror="_cdnFallback(this)" href="order/css/css.css" rel="stylesheet">    
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script> 
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
 <script src="/Scripts/Order.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;
        var pageSize = 16;
        $(function () {


            var comid = $("#hid_comid").val();
            getmenubutton(comid, 'js-navmenu');
            var projectid = $("#hid_projectid").val();
            //根据公司id获得关注作者信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage btn btn-follow \">关注我们</a>");
                }
            });

            SearchList(pageindex, 16);

            var stop = true;
            $(window).scroll(function () {

                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#pageindex").val()) + 1;

                        stop = false;
                        $("#loading").show();
                        SearchList(pageindex, 16);
                    }
                }
            });

            //装载列表
            function SearchList(pageindex, pagesize, key) {

                $("#loading").show();

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ChannelHandler.ashx?oper=ChannelcompanyOrderlocation",
                    data: { channelcompanyid: $("#hid_channelcompanyid").val(), comid: $("#hid_comid").val(), pageindex: pageindex, pagesize: pagesize, key: key, channelcompanytype:"1" },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#list").hide();
                            $("#black_top").hide();
                            $("#page").hide();
                            $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                            return;
                        }
                        if (data.type == 100) {
                            if (data.totalcount == 0) {
                                location.href = "OrderinfoTitle.aspx?id=" + $("#hid_comid").val();
                            }
                            $("#loading").hide();

                            $("#ProductItemEdit").tmpl(data.msg).appendTo(".content-body");
                            $("#hid_pageindex").val(pageindex);

                        }
                    }
                })
            }

        });

        function SubstrDome(s, num) {
            var ss;
            if (s.length > num) {
                ss = s.substr(0, num) + "..";
                return (ss);
            }
            else {
                return (s);
            }

        }
        function jucom(id) {
            location.href = "StoreDefault.aspx?menshiid=" + id;
        }
        function juaddress(id) {
            location.href = "StoreInfo.aspx?menshiid=" + id + "&type=3";
        }
        function juQrcodeurl(url) {
            location.href = url;
        }


    </script>
          <script type="text/javascript">
              //判断微信版本,微信版本5.0以上
              function isWeiXin() {
                  var ua = window.navigator.userAgent.toLowerCase();
                  if (ua.match(/MicroMessenger/i) == 'micromessenger' && parseFloat(navigator.appVersion) >= 5) {
                      return true;
                  } else {
                      return false;
                  }
              }
    </script>
 
	</head>

<body class=" " >
        <!-- container -->
    <div class="container ">
        <div class="header">
        <!-- ▼顶部通栏 -->
                                
            <div class="js-mp-info share-mp-info">
            <a href="/h5/order/default.aspx" class="page-mp-info">
                <img width="24" height="24" src="<%=comlogo%>" class="mp-image">
                <i class="mp-nickname">
                    <%=title %>                </i>
            </a>
            <div class="links ">
                
                                                                </div>
        </div>
        <!-- ▲顶部通栏 -->
</div>        <div class="content ">
        <div class="content-body ">
    
                    
          
    
    
        </div>                       		
	    <div class="content-sidebar">
		            <a href="/h5/Default.aspx" class="link">
		                <div class="sidebar-section shop-card">
		                    <div class="table-cell">
		                       <img src="<%=comlogo %>" class="shop-img" alt="公众号头像" height="60" width="60">
		                    </div>
		                    <div class="table-cell">
		                        <p class="shop-name">
		                        	<%=title %>		                        			                       
                                </p>
		                    </div>
		                </div>
		            </a>
		               <div class="sidebar-section shop-info">
		                    <div class="section-detail">
		                         <p class="shop-detail"><%=Scenic_intro %></p>
		                        	<p class="text-center weixin-title">微信“扫一扫”立即关注</p>
			                        <div class="js-follow-qrcode text-center qr-code">
			                        <img width="158" height="158" src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>">
                                    </div>
			                        <p class="text-center weixin-no">微信号：<%=weixinname %></p>
			                </div>
		               </div>		    		        		    	    
          </div>
    
               
			   </div>        <div style="min-height: 1px;" class="js-footer">
            
    <div class="footer">
        <div class="copyright">
                            <div class="ft-links">
                        <span id="copydaohang"></span>
                    <span class="links"></span>
                                                        </div>
                        <div class="ft-copyright">
<a href="#">易城商户平台技术支持</a>
</div>
        </div>
    </div>
        </div>      </div>

    <!-- JS -->
   
   <div class="motify"><div class="motify-inner"></div></div>
   
      <div id="right-icon" class="icon-hide   no-border hide" data-count = "1">
	<div class="right-icon-container clearfix" style="width: 50px">
		
		<a id="global-cart" href="Cart.aspx" class="no-text" style="background-image: url(image/s0.png);
							background-size: 50px 50px;
							background-position: center;">
			<p class="right-icon-img"></p>
			<p class="right-icon-txt">购物车</p>
		</a>
	</div>
</div>



<script type="text/x-jquery-tmpl" id="ProductItemEdit"> 


        <div class="block block-order" >
                <div class="store-header header">
                    <span onclick="jucom('${Id}');">店铺：${Companyname} </span>
                </div>
                <hr class="margin-0 left-10">
                <div class="name-card name-card-store clearfix">
                   


                    <a href="javascript:;" class="thumb js-view-image-list"  {{if (m !=null)}}onclick="juQrcodeurl('${m.Qrcodeurl}');" {{/if}}>
                    <img class="js-view-image-item" src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>">
                    </a>
                    <a href="tel:${Companyphone}"><div class="phone"></div></a>
                    <a class="detail" href="javascript:;" onclick="juaddress('${Id}');">
                        <h3>
                            ${Companyaddress}               </h3>
                                        <p class="c-gray-dark ellipsis" style="margin-top: 5px">
                                        ${ViewDistance(Distance)}
                                         </p>
                                    </a>
                </div>
   
                    <hr>
                <div class="name-card-bottom c-gray-dark" > <a href="tel:${Companyphone}">商家电话：${Companyphone}</a></div>
        </div>  

            </script>

</div>


<div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>

 <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_channelcompanyid" type="hidden" value="0" />
    <input id="hid_channelcompanytype" type="hidden" value="<%=channelcompanytype %>" />
    <input id="hid_pageindex" type="hidden" value="1" />
    <input id="num" type="hidden" value="10" />


        <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {
            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=comlogo %>",
                desc: "<%=Scenic_intro %>",
                title: ' <%=title %>'
            });
        });
    </script>

</body></html>