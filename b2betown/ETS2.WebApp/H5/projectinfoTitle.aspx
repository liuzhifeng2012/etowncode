<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projectinfoTitle.aspx.cs" Inherits="ETS2.WebApp.H5.projectinfoTitle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var _tcopentime = new Date().getTime();
        var _hmt = _hmt || [];
    </script>
    <!-- meta信息，可维护 -->
    <meta charset="UTF-8" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta content="telephone=no" name="format-detection" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <!-- 页面样式表 -->
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .none
        {
            display: none;
        }
        #loading
        {
            position: absolute;
            left: 50%;
            top: 60px;
            z-index: 99;
        }
        #loading, #loading .lbk, #loading .lcont
        {
            width: 146px;
            height: 146px;
        }
        #loading .lbk, #loading .lcont
        {
            position: relative;
        }
        #loading .lbk
        {
            background-color: #000;
            opacity: .5;
            border-radius: 10px;
            margin: -73px 0 0 -73px;
            z-index: 0;
        }
        #loading .lcont
        {
            margin: -146px 0 0 -73px;
            text-align: center;
            color: #f5f5f5;
            font-size: 14px;
            line-height: 35px;
            z-index: 1;
        }
        #loading img
        {
            width: 35px;
            height: 35px;
            margin: 30px auto;
            display: block;
        }
    </style>
    <!-- 页面样式表 -->
    <link href="../Styles/H5/scenery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <meta name="description" content="北京景点门票，北京景点门票预订，北京旅游景点大全" />
    <meta name="keywords" content="是您旅行的好伙伴。" />
    <style type="text/css">
        .intro
        {
            padding: 10px 0;
        }
        .introTop
        {
            background-color: white;
            line-height: 20px;
            margin: 0 10px 0 10px;
            padding: 10px;
        }
        .introTop h2
        {
            font-weight: bold;
        }
    </style>
    <link href="../Styles/H5/Order.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("#submitBtn1").click(function () {
                //                window.history.go(-1);
                location.href = "http://shop<%=comid %>.etown.cn" ;
            })
        })
    </script>
</head>
<body>
    <div>
        <div id="loading" style="top: 150px; display: none;">
            <div class="lbk">
            </div>
            <div class="lcont">
                <img src="../Images/loading.gif" alt="loading..." />正在加载...</div>
        </div>
        <a id="goBack" href="javascript:history.go(-1);">
            <header class="header"  style=" background-color: #3CAFDC;">
                    <h1><%=projectname%></h1>
        <div class="left-head">
          <a  href="javascript:history.go(-1);" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
        </a>
        <!-- 页面内容块 -->
        <div class="intro">
            <div class="introTop">
                <h2>
                    商家介绍：</h2>
                <p>
                    <%=projectbrief%></p>
                </br><p>
                    <%=projectserviceintroduce%></p>
            </div>
            <div class="order-btn fn-clear">
                <div class="submit-btn">
                    <input type="button" class="btn" id="submitBtn1" value="在线预订" />
                </div>
            </div>
        </div>
        <input id="com_id" type="hidden" value="<%=comid %>" />
    </div>
</body>
</html>
