<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="h5info.aspx.cs" Inherits="ETS2.WebApp.M.h5info" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"></meta>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"
        name="viewport"></meta>
    <meta content="telephone=no" name="format-detection"></meta>
    <title>
        <%=title %></title>
    <link href="../Styles/weixin/StyleBinding.css" rel="stylesheet" type="text/css" />
    <style>
        abbr, article, aside, audio, canvas, datalist, details, dialog, eventsource, figure, figcaption, footer, header, hgroup, mark, menu, meter, nav, output, progress, section, small, time, video, legend
        {
            display: block;
        }
        ul li
        {
            text-align:center;
            text-decoration:none;
            list-style:none;
            }
        ul li ul li
        {
            text-decoration:none;
            list-style:none;
        }
        #page_intro article span {
           background: url('') no-repeat top left;
        }
        
    </style>
</head>
<body id="page_intro" class="">
    <div id="mappContainer">
        <div class="inner root" style="height: 650px; margin-bottom:50px;">
            <article class="pdo" style="width: 350px;">
            <div class="inn">
            <span class="pdo tl"></span>
            <span class="pdo tr"></span>
            <span class="pdo bl"></span>
            <span class="pdo br"></span>
            <ul>
               <li>
               <h2 class="type_header_商家简介"><%=price %>
            <%=title %></h2>
                   <ul>
            <li style="color:#797979;font-size:12px;">
                <%=summary %>
            </li>
            <li style="color:#797979;font-size:12px;">
                <%=article %>
            </li>
            </ul>
               </li>
               <li>
                      <h2 class="type_header_商家简介">
                        <%=phone %><a href="tel:<%=phone_tel %>"><%=phone_tel %></a>
                      </h2>
               </li>
            </ul>
           

            </div>
            </article>
        </div>
        <!--<div class="footFix" id="footReturn">
            <a href="indexcard.aspx"><span>返回会员卡首页</span></a>
        </div>-->
    </div>
    <script>
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            WeixinJSBridge.call('hideToolbar');
        });
    </script>
</body>
</html>
