<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lineTrip.aspx.cs" Inherits="ETS2.WebApp.H5.lineTrip" %>


<!DOCTYPE HTML>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<link rel="stylesheet" type="text/css" href="/Styles/h5/travel.css" />
<link rel="stylesheet" type="text/css" href="/Styles/h5/xingcheng.css" />
<link rel="stylesheet" type="text/css" href="/Styles/fontcss/font-awesome.min.css" />
 <!--[if IE 7]>
		  <link rel="stylesheet" href="source/module/admin/template/assets/css/font-awesome-ie7.min.css" />
<![endif]-->
<title><%=pro_name%></title>
<meta name="keywords" content="" />
<meta name="description" content="" />
<meta name="HandheldFriendly" content="true" />
<meta name="MobileOptimized" content="width" />
<meta id="viewport" content="width=device-width, user-scalable=yes,initial-scale=1" name="viewport" />
<meta name="apple-mobile-web-app-capable" content="yes">
<meta content="black" name="apple-mobile-web-app-status-bar-style" />
<meta content="telephone=no" name="format-detection" />
<script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>

<script type="text/javascript">
    var pageSize = 10; //只显示条数

    $(function () {
        var comid = $("#hid_comid").val();
        var lineid = $("#hid_lineid").val();
        //加载 ;
        $.ajax({
            type: "post",
            url: "/JsonFactory/ProductHandler.ashx?oper=getLinetripById",
            data: { lineid: lineid },
            async: false,
            success: function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("查询错误");
                    return;
                }
                if (data.type == 100) {

                    //行程处理
                    var linestr = "";
                    var linecenter = "";
                    var linetop = "";
                    var linebotton = "";
                    if (data.msg != null) {

                        for (var i = 0; i < data.msg.length; i++) {

                            if (i == 0) {
                                linetop = " <p class=\"date_plan_top\">      第" + (i + 1) + "天: " + data.msg[i].ActivityArea + "&nbsp;" + data.msg[i].Traffic + "       </p>";
                            } else {
                                linetop = " <p class=\"date_plan\">        第" + (i+1) + "天: " + data.msg[i].ActivityArea + "&nbsp;" + data.msg[i].Traffic + "     </p>";
                            }
                            linecenter = "    <div class=\"plan_box\">      " + data.msg[i].Description + " <br>酒店:" + data.msg[i].Hotel + "<br>用餐:" + data.msg[i].Dining + "  </div>";

                            if (i == 0) {//第一天不加，第二天开始头部加
                                linebotton = "";
                            } else {
                                linebotton = "<div class=\"explain_back\">                <div class=\"explain\">                                    <p class=\"explain_cont_bottom\"></p>                </div>                </div>";
                            }
                            linestr += linebotton + linetop + linecenter
                        }

                        var linedefautop = "<p class=\"mc_lists\"></p>";
                        var linedefaubot = "<div class=\"slide_block\" id=\"slideBlock\">                <div class=\"slide_store\" id=\"slideStore\" >下载到手机上查看</div>                <div class=\"slide_cancel\" id=\"slideCancel\">取消</div>            </div>";
                        $(".main_cont").html(linedefautop + linestr + linedefaubot);

                    }

                }
            }
        })

    })

    </script>
<script type="text/javascript">
    var viewPortScale;
    var dpr = window.devicePixelRatio;
    viewPortScale = 0.5;
    //
    var detectBrowser = function (name) {
        if (navigator.userAgent.toLowerCase().indexOf(name) > -1) {
            return true;
        } else {
            return false;
        }
    };
    if (detectBrowser('ipad')) {
        document.getElementById('viewport').setAttribute(
        'content', 'width=device-width, user-scalable=no,initial-scale=1');
    } else if (detectBrowser('ucbrowser')) {
        document.getElementById('viewport').setAttribute(
        'content', 'user-scalable=no, width=device-width, minimum-scale=0.5, initial-scale=' + viewPortScale);
    } else if (detectBrowser('360browser')) {
        document.getElementById('viewport').setAttribute(
        'content', 'target-densitydpi=320,user-scalable=no, width=640, minimum-scale=1, initial-scale=1');
    } else {
        document.getElementById('viewport').setAttribute(
        'content', 'target-densitydpi=320, user-scalable=no,width=640, minimum-scale=0.5, initial-scale=' + viewPortScale);
    }

    
</script>
<style type="text/css">
img{
max-width:580px;
}
</style>
</head>
<body>
<div class="header-box" id="header">
    <div class="header clearfix" id="topHeader">
                <div class="back_icon_control" id="mainBack">
            <span class="icon-arrow-left"></span><a href="javascript:self.location=document.referrer;" class="back_icon_info">返回</a>
        </div>
                <div class="title with-desc"> 
            <div class="name">线路行程</div>
                        <div class="desc">线路编号：<%=travelproductid%></div>
                    </div>
    </div>
</div><div class="wrapper" id="wrapper">
	<div id="index">	
    <div class="main_cont">
   
        </div>

        <div class="back_top">
            <a onClick="window.scrollTo(0, 0);">回到顶部</a>
        </div>
            <p class="ps">电话咨询，旅游顾问将给您更详尽的解答。</p>


        <div class="grey_mask" id="greyMask"></div>

        <div class="first_tips" id="firstTips">
            <span class="tips_info">长按图片可下载到手机上哦！</span>
            <span class="cancel" id="tipsCancel"></span>        </div>

	</div>
	</div>
    <input type="hidden" id="hid_lineid" value="<%=lineid %>" /> 
</body>
</html>

