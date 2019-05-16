<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FriendlyRemind.aspx.cs"
    Inherits="ETS2.WebApp.FriendlyRemind" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>对不起,出错了 </title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type">
    <link charset="utf-8" rel="stylesheet" href="/styles/style-2.css">
    <style type="text/css">
        .header
        {
            height: 80px;
        }
        .txt-1
        {
            height: 129px;
            -webkit-transition: none;
            -moz-transition: none;
            -ms-transition: none;
            -o-transition: none;
            transition: none;
        }
        .slide-number
        {
            margin-left: -45px;
        }
        .about-item .ui-link-item a
        {
            color: #666;
        }
    </style>
</head>
<body role="document">
    <!--[if IE 6]>

  <style>
      .kie-bar {
          height: 24px;
          line-height: 1.8;
          font-weight:normal;
          text-align: center;
          border-bottom:1px solid #fce4b5;
          background-color:#FFFF9B;
          color:#e27839;
          position: relative;
          font-size: 14px;
          text-shadow: 0px 0px 1px #efefef;
          padding: 5px 0;
      }
      .kie-bar a {
          color:#08c;
          text-decoration: none;
      }
  </style>
  <![endif]-->

    <div data-banner="false">
        <ul id="J-slide" style="position: relative; background-color: #ffffff;">
            <li class="slide" data-bg="" data-pic="" data-txt="" data-load="false" data-iebg=""
                style="z-index: 1; position: absolute; left: 0px; top: 0px; opacity: 1;">
                <div class="txt-wrap" align="center" style="margin-top: 60px;">
                    <div class="zhuce_layout bg_2" style="height: 240px; margin-top: 60px;">
                        <div class="w500 ovl mauto mt30 mb30">
                            <img src="/images/icon_14.jpg" class="fl mr30">
                            <p class="mb10 a_ms_yahei f24 mt20" style=" font-size:20px; color:#333333">
                                对不起,出错啦!</p>
                            <p class="f12" style="color: black;">
                                请您重新尝试刚才的操作，或者<a href="/default.aspx" class="col06c">返回首页面</a>
                            </p>
                        </div>
                    </div>
                </div>
            </li>
        </ul>
    </div>

</body>
</html>
