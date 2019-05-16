<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="h5_b2bcrmtequan.aspx.cs"
    Inherits="ETS2.WebApp.M.h5_b2bcrmtequan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta charset="utf-8"></meta>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"
        name="viewport"></meta>
    <meta content="telephone=no" name="format-detection"></meta>
    <title>
        <%=title %></title>
    <link href="/Styles/weixin/StyleBinding.css" rel="stylesheet" type="text/css" />
    <style>
        #page_intro article span {
           background: url('') no-repeat top left;
        }
        abbr, article, aside, audio, canvas, datalist, details, dialog, eventsource, figure, figcaption, footer, header, hgroup, mark, menu, meter, nav, output, progress, section, small, time, video, legend
        {
            display: block;
        }
        ul li
        {
            text-align: left;
            text-decoration: none;
            list-style: none;
        }
        ul li ul li
        {
            text-decoration: none;
            list-style: none;
        }
    </style>
</head>
<body id="page_intro" class="">
    <div id="mappContainer">
        <div class="inner root" style="height: 650px; margin-bottom: 50px;">
            <article class="pdo" style="width: 300px;">
            <div class="inn">
            <span class="pdo tl"></span>
            <span class="pdo tr"></span>
            <span class="pdo bl"></span>
            <span class="pdo br"></span>
             <ul>
               <li>
               <h2 class="type_header_商家简介"> 
            <%=title %></h2>
                   <ul>
            <li style="color:#797979;font-size:12px;">
                <%=tishi %>
            </li>
            <li style="color:#797979;font-size:12px;">
                <%=tequan %>
            </li>
            </ul>
               </li>
               <li>
                      <h2 class="type_header_商家简介">
                        
                      </h2>
               </li>
            </ul>
           

            </div>
            </article>
        </div>
    </div>
    <script>
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            WeixinJSBridge.call('hideToolbar');
        });
    </script>
</body>
</html>
