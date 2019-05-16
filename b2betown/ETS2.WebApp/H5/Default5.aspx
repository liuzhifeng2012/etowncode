<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default5.aspx.cs" Inherits="ETS2.WebApp.H5.Default5" %>


<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
<meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />

<title><%=companyname%></title>
<link rel="stylesheet" type="text/css" href="/Styles/fontcss/font-awesome.min.css">
<!--[if IE 7]>
		  <link rel="stylesheet" href="/Styles/fontcss/font-awesome-ie7.min.css" />
<![endif]-->
<link rel="stylesheet" type="text/css" href="/Styles/H5/mh5-5.css" />
<script type="text/javascript" src="/Scripts/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="/Scripts/swipe.js"></script>
	<meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport">
        <meta name="Keywords" content="" />
        <meta name="Description" content="" />
        <!-- Mobile Devices Support @begin -->
            <meta content="application/xhtml+xml;charset=UTF-8" http-equiv="Content-Type">
            <meta content="no-cache,must-revalidate" http-equiv="Cache-Control">
            <meta content="no-cache" http-equiv="pragma">
            <meta content="0" http-equiv="expires">
            <meta content="telephone=no, address=no" name="format-detection">
            <meta name="apple-mobile-web-app-capable" content="yes" /> <!-- apple devices fullscreen -->
            <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
        <!-- Mobile Devices Support @end -->
<script type="text/javascript">
    var pageSize = 10; //只显示条数

    $(function () {
        var comid = $("#hid_comid").val();
        var bannerlist = "";
        var daohanglist = "";

            //加载导航 ;
            $.ajax({
                type: "post",
                url: "/JsonFactory/DirectSellHandler.ashx?oper=getmenulist",
                data: { comid: comid, pageindex: 1, pagesize: 20, typeid: 0 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("查询错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.totalCount == 0) {
                        }
                        else {

                            for (i = 0; i < data.totalCount; i++) {
                                daohanglist = daohanglist + "<li><a href=\"" + data.msg[i].Linkurl + "\"><div><span class=\"" + data.msg[i].Fonticon + "\"></span></div><div>                                <p>" + data.msg[i].Name + "</p>                            </div></a> </li>";
                            }
                            $(".list_font").html(daohanglist);
                        }
                    }
                }
            })

    })

    </script>
</head>
 <body onselectstart="return true;" ondragstart="return false;">
<div class="body" style="padding-bottom:52px;">
	<!-- 首页焦点图开始 -->
    <div style="-webkit-transform:translate3d(0,0,0);">
    <div style="visibility: visible;" id="banner_box" class="box_swipe">
		<%=bannerstr%>
	</div>
    </div>
	<!-- 首页焦点图结束 -->
	<br/><header>
        <div class="snower">
        </div>
    </header> 	        <section>
            <a href="tel:<%=tel %>" class="link_tel icon-phone"><%=tel%></a>
        </section>
        		<!--
		用户分类管理
		-->
		<section>
            <ul class="list_font">
            
           </ul>
        </section>
</div>

<!-- 底部开始 -->
        			<footer style="overflow:visible;">
				<div class="weimob-copyright" style="padding-bottom:10px;">
											<a href="#">©<%=companyname%></a>
									</div>
			</footer>
<!-- js在最后 -->
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
<input type="hidden" id="hid_comid" value="<%=comid %>" />
</body>
</html>