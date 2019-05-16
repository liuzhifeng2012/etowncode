<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="OrderinfoTitle.aspx.cs"
    Inherits="ETS2.WebApp.H5.OrderinfoTitle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;" />
    <meta name="format-detection" content="telephone=no" />
    <!-- Mobile Devices Support @begin -->
            <meta content="application/xhtml+xml;charset=UTF-8" http-equiv="Content-Type">
            <meta content="no-cache,must-revalidate" http-equiv="Cache-Control">
            <meta content="no-cache" http-equiv="pragma">
            <meta content="0" http-equiv="expires">
            <meta content="telephone=no, address=no" name="format-detection">
            <meta name="apple-mobile-web-app-capable" content="yes" /> <!-- apple devices fullscreen -->
            <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
        <!-- Mobile Devices Support @end -->
    <title>
        <%=title %></title>
    <style>
        body
        {
            padding: 0px;
            font-family: "宋体";
            background: none repeat scroll 0% 0% #FFF;
            color: #222;
            height: 100%;
            position: relative;

            margin: 0 auto !important;
            
        }
        body ,article, section, h1, h2, hgroup, p, a, ul, li, em, div, small, span, footer, canvas, figure, figcaption, input
        {
            margin: 0;
            padding: 0;
        }
        a
        {
            text-decoration: none;
            cursor: pointer;
        }
        a.autotel
        {
            text-decoration: none;
            color: inherit;
        }
        
        .inner
        {

            padding: 10px 10px;
            margin: 0 auto;
            max-width: 640px;
        }
        h1
        {
            font-size: 22px;
            font-weight: normal;
            line-height: 26px;
            margin-bottom: 18px;
        }
        img
        {
            max-width:100%;
            border: none;
            margin-bottom: 8px;
            clear: both;
            margin:auto; 
        }
        .old_message
        {
            line-height: 20px;
            text-indent: 2em;
            font-size: 14px;
            color: #565752;
            text-align: left;
            margin-bottom: 10px;
            word-wrap: break-word;
        }
        p
        {
            color: #565752;
            text-align: left;
            margin-bottom: 10px;
            word-wrap: break-word;
            line-height: 25px;
        }
        .phone
        {
        }
        .phone a
        {
            font-size: 16px;
            font-weight: bold;
            color: #fff;
            background: #fe932b;
            padding: 5px 10px;
            text-align: center;
            vertical-align: middle;
        }
        .tel
        {
            width: 134px;
            height: 33px;
            line-height: 33px;
            display: inline-block;
            text-align: center;
            font-size: 14px;
            border: 1px solid #4a4a4a;
            margin: 0 7px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            text-shadow: 0 1px #2daf35;
            color: #fff;
            box-shadow: inset 0 0 5px #8ee392;
            background-color: #29a832;
            background-image: -webkit-gradient(linear,0% 0,0% 100%,from(#4cbd51),to(#079414));
        }
        .yuding
        {
            width: 134px;
            height: 33px;
            line-height: 33px;
            display: inline-block;
            text-align: center;
            font-size: 14px;
            border: 1px solid #4a4a4a;
            margin: 0 7px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            text-shadow: 0 1px #2daf35;
            color: #fff;
            box-shadow: inset 0 0 5px #8ee392;
            background-color: #29a832;
            background-image: -webkit-gradient(linear,0% 0,0% 100%,from(#4cbd51),to(#079414));
        }
        #mcover
        {
            position: fixed;
            top: 0px;
            left: 0px;
            width: 100%;
            height: 100%;
            background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.7);
            display: none;
            z-index: 20000;
        }
        #mcover img
        {
            position: fixed;
            right: 18px;
            top: 5px;
            width: 260px !important;
            height: 180px !important;
            z-index: 20001;
        }
        .text
        {
            margin: 15px 0px;
            font-size: 14px;
            word-wrap: break-word;
            color: #727272;
        }
        #mess_share
        {
            margin: 15px 0px;
            display: block;
        }
        #share_1
        {
            float: left;
            width: 49%;
            display: block;
        }
        .button2
        {
            font-size: 16px;
            padding: 8px 0px 0px 0px;
            border: 1px solid #ADADAB;
            color: #000;
            background-color: #E8E8E8;
            background-image: linear-gradient(to top, #DBDBDB, #F4F4F4);
            box-shadow: 0px 1px 1px rgba(0, 0, 0, 0.45), 0px 1px 1px #EFEFEF inset;
            text-shadow: 0.5px 0.5px 1px #FFF;
            text-align: center;
            border-radius: 3px;
            width: 100%;
            vertical-align: middle;
        }
        #share_2
        {
            float: right;
            width: 49%;
            display: block;
        }
        #mess_share img
        {
            width: 22px !important;
            height: 22px !important;
            vertical-align: top;
            border: 0px none;
        }
        
        
        
        
.share-mp-info {
    position: relative;
    background: none repeat scroll 0% 0% #EEE;
    color: #FFF;
    font-size: 0px;
    line-height: 0;
    padding: 1px 105px 1px 1px;
}
 .page-mp-info {
    display: block;
    padding: 4px 10px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}
 .share-mp-info .links {
    font-size: 14px;
    line-height: 24px;
    color: #888;
}

.share-mp-info .links {
    font-size: 14px;
    line-height: 24px;
    color: #888;
}
.share-mp-info i {
    color: #888;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}
.share-mp-info em, .share-mp-info i {
    vertical-align: middle;
    font-style: normal;
}
.share-mp-info .links {
    position: absolute;
    top: 6px;
    right: 10px;
    display: inline-block;
}
.share-mp-info .page-mp-info, .share-mp-info .links {
    font-size: 14px;
    line-height: 24px;
    color: #888;
}
.share-mp-info .links a {
    margin-left: 8px;
}
.share-mp-info a {
    color: #888;
}
.btn-follow {
    padding: 5px;
    color: #0C3;
    border: 1px solid #0C3;
    margin-right: 5px;
    font-size: 12px;
    line-height: 12px;
}
.share-mp-info img.mp-image {
    vertical-align: middle;
    margin-right: 3px;
    width: 24px;
    height: 24px;
    border-radius: 100%;
    box-shadow: 0px 0px 3px rgba(0, 0, 0, 0.25);
}
.btn {
    display: inline-block;
    background-color: #FFF;
    border: 1px solid #E5E5E5;
    border-radius: 2px;
    padding: 4px;
    text-align: center;
    margin-right: 2px;
    color: #999;
    font-size: 12px;
    cursor: pointer;
    line-height: 18px;
}
.btn-follow {
    padding: 5px;
    color: #0C3;
    border: 1px solid #0C3;
    margin-right: 5px;
    font-size: 12px;
    line-height: 12px;
}

    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/MenuButton.js" type="text/javascript"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

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

        });

    </script>


    <script type="text/javascript">
        $(function () {
            $("#divinner").click(function () {
                var articleurl = $("#hid_articleurl").val();
                if (articleurl != "") {
                    window.open(articleurl, target = "_self");
                }
            })
        })
    </script>
</head>
<body>
 <div class="header">
        <!-- ▼顶部通栏 -->
                                
            <div class="js-mp-info share-mp-info">
            <a href="/h5/order/default.aspx" class="page-mp-info">
                <img width="24" height="24" src="<%=logoimg%>" class="mp-image">
                <i class="mp-nickname">
                    <%=title%>                </i>
            </a>
            <div class="links ">
                
                                                                </div>
        </div>
        <!-- ▲顶部通栏 -->
</div> 
    <div class="inner" id="divinner">
        <div>
                <h2>
                    商家介绍：</h2>
                 <%if (article != ""){%>
                 <p>
                    <%=article%></p>
                 <%} %>

                 <%if (serviceinfo != "")
                      { %>
                <p>
                    <%=serviceinfo%></p>
                 <%} %>
                 
                 
                 <%if (Scenic_Takebus != "")
                      { %>
                <h2>
                    公交路线：</h2>
                <p>
                    <%=Scenic_Takebus%></p>
                <%} %>

                <%if (Scenic_Drivingcar != "")
                      { %>
                <h2>
                    开车路线：</h2>
                <p>
                    <%=Scenic_Drivingcar%></p>
                <%} %>
                    </div>




         <input id="hid_comid" type="hidden" value="<%=comid %>" />

    </div>

<style>
    .copyrightbox{ padding: 30px 0px; font-size:14px; background:#f0f0f0; color:#666666;   text-align:center;}
    .copyrightbox .copyrightbtn{ color:#0066cc; padding-right:15px;}
</style>

<div class="copyrightbox fn-clear">
<span id="copydaohang"></span>
    <span class="links"></span>
<div style=" padding-top:15px;font-size:12px; display:none;">
    易城商户平台技术支持
   </div>
</div> 

   
    <script type="text/javascript" src="/Scripts/ppkextend.js"></script>
    <script type="text/javascript">
        $(function () {

            //分享  
            $.ppkWeiShare({
                path: location.href,
                image: "<%=logoimg %>",
                desc: "<%=article %>",
                title: ' <%=title %>'
            });
        });
    </script>
</body>
</html>
