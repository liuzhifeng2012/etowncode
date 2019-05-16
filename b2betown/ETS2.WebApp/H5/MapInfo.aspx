<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapInfo.aspx.cs" Inherits="ETS2.WebApp.H5.MapInfo" %>


<html lang="zh-CN" class="no-js ">
<head>
    <meta charset="utf-8">
    <meta content="True" name="HandheldFriendly">
    <meta content="320" name="MobileOptimized">
    <meta content="telephone=no" name="format-detection">
    <meta content="on" http-equiv="cleartype">


    <link href="image/yz_fc.ico" rel="icon">
    <title> <%=Com_name%></title>

    <!-- meta viewport -->
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport">
    
    <!-- CSS -->
    <link onerror="_cdnFallback(this)" href="order/css/css1.css" rel="stylesheet">    
	<link onerror="_cdnFallback(this)" href="order/css/css.css" rel="stylesheet">    
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script> 
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
        <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <!-- 页面样式表 -->

    <script type="text/javascript" src="http://api.map.baidu.com/api?type=quick&ak=mKeOlGW2zgs8LAVf7ihHzSTD&v=1.0"></script>


    <script type="text/javascript">
        var pageindex = 1;
        var pageSize = 16;
        $(function () {

            var comid = $("#hid_comid").val();
            getmenubutton(comid, 'js-navmenu');

            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompanyguanzhu", { comid: comid }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $(".links").html("<a href=\"" + dat.msg + "\" class=\"mp-homepage btn btn-follow\">关注我们</a>");
                }
            });


            var height = $(window).height() - 100;
            if (height < 280) {
                height = 280;
            }
            $("#allmap").height(height);

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
                <link  href="css/bottommenu.css" rel="stylesheet"> 
	</head>

<body class=" " >
        <!-- container -->
    <div class="container ">

      <div class="content ">
        <div class="content-body ">

        <!-- 页面内容块 -->
        <div class="intro">


                    <div id="allmap" style=" width:100%; height:400px;"></div>
                    <script type="text/javascript">
                        // 百度地图API功能
                        var map = new BMap.Map("allmap");
                        var point = new BMap.Point(<%=Coordinate%>);
                        map.centerAndZoom(point, 13);
                       
                        map.addControl(new BMap.ScaleControl());                    // 添加比例尺控件
                        //map.addControl(new BMap.OverviewMapControl());              //添加缩略地图控件
                        
                        var marker1 = new BMap.Marker(point);  // 创建标注
                        map.addOverlay(marker1);              // 将标注添加到地图中

                        var opts = {
                            width: 220,     // 信息窗口宽度
                            height: 100,     // 信息窗口高度
                            title: "<%=Com_name%>" // 信息窗口标题
                        }

                        var infoWindow = new BMap.InfoWindow("地址：<%=Com_add%><br><a href=\"javascript:;\"  onclick='DRIVINGdaohang()'  class=\"daohang_anniu\">进入导航</a> ", opts);  // 创建信息窗口对象
                        marker1.openInfoWindow(infoWindow);

                        marker1.addEventListener("click", function(){          
                           this.openInfoWindow(infoWindow);
                      });

                      

                      function DRIVINGdaohang() {
                            var start = {
	                             name:""
	                        }
	                        var end = {
	                            name:"<%=Com_add %>",latlng:new BMap.Point(<%=Coordinate%>)
	                        }
	                        var opts = {
	                            mode:BMAP_MODE_DRIVING,
	                            region:"<%=Coordinate %>"
	                        }
	                        var ss = new BMap.RouteSearch();
	                        ss.routeCall(start,end,opts);
                        }

                     </script>



        </div>
                    
          
          <div class="block block-order" id="shop-detail-container"><div class="name-card name-card-3col name-card-store clearfix">
            <a href="javascript:;" class="thumb js-view-image-list">
        
                    <img class="js-view-image-item" src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>">
        
            </a>
            <a href="tel:<%=Tel%>"><div class="phone"></div></a>
            <div class="detail">
            <h3>
            <%=Com_name%>
            </h3>

                <h3>
                     <%=Com_add %>
                </h3>

                <p class="c-gray-dark ellipsis" style="margin-top: 1px">
        
        
                    电话：<%=Tel%>
                </p>

            </div>
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
        </div>        <!-- fuck taobao -->
    <div class="fullscreen-guide fuck-taobao hide" id="js-fuck-taobao">
        <span class="js-close-taobao guide-close">×</span>
        <span class="guide-arrow"></span>
        <div class="guide-inner">
            <div class="step step-1"></div>
            <div class="js-step-2 step"></div>
        </div>
    </div>    </div>

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
                    <span>店铺：${Companyname}</span>
                </div>
                <hr class="margin-0 left-10">
                <div class="name-card name-card-3col name-card-store clearfix">
                    <a href="javascript:;" class="thumb js-view-image-list">
                    <img class="js-view-image-item" src="http://open.weixin.qq.com/qr/code/?username=<%=weixinname %>">
                                                        </a>
                    <a href="tel:${Companyphone}"><div class="phone"></div></a>
                    <a class="detail" href="javascript:;" onclick="csss('${Id}');">
                        <h3>
                            ${Companyaddress}               </h3>
                                        <p class="c-gray-dark ellipsis" style="margin-top: 5px">
                                         </p>
                                    </a>

            
                </div>
   
                    <hr>
                <div class="name-card-bottom c-gray-dark" > <a href="tel:${Companyphone}">商家推荐：电话：${Companyphone}</a></div>
        </div>  
</script>

</div>


<div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>

 <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="hid_pageindex" type="hidden" value="1" />
    <input id="num" type="hidden" value="10" />
        <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {
            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=comlogo %>",
                desc: "",
                title: ' <%=Com_name %>'
            });
        });
    </script>
</body></html>