<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ETS2.WebApp.QRCode._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" name="viewport" content="width=device-width, initial-scale=1">
    <title>易城二维码</title>
    <style type="text/css">
        .container-fluid
        {
            margin-top: 20px;
        }
        .container-fluid
        {
            padding-right: 20px;
            padding-left: 20px;
        }
        
        div[Attributes Style]
        {
            text-align: -webkit-center;
        }
        user agent stylesheetdiv
        {
            display: block;
        }
        body
        {
            margin: 0;
            font-family: "Helvetica Neue" ,Helvetica,Arial,sans-serif;
            font-size: 14px;
            line-height: 20px;
            color: #333333;
            background-color: #ffffff;
        }
        .container-fluid:before, .container-fluid:after
        {
            display: table;
            content: "";
            line-height: 0;
        }
        .container-fluid:after
        {
            clear: both;
        }
        
        .container-fluid:before, .container-fluid:after
        {
            display: table;
            content: "";
            line-height: 0;
        }
        h4
        {
            font-size: 17.5px;
        }
        h1, h2, h3, h4, h5, h6
        {
            margin: 10px 0;
            font-family: inherit;
            font-weight: bold;
            line-height: 20px;
            color: inherit;
            text-rendering: optimizelegibility;
        }
        h4[Attributes Style]
        {
            text-align: center;
        }
        user agent stylesheeth4
        {
            display: block;
            -webkit-margin-before: 1.33em;
            -webkit-margin-after: 1.33em;
            -webkit-margin-start: 0px;
            -webkit-margin-end: 0px;
            font-weight: bold;
        }
        img
        {
            max-width: 100%;
            width: auto\9;
            height: auto;
            vertical-align: middle;
            border: 0;
            -ms-interpolation-mode: bicubic;
        }
        
        Inherited from body body
        {
            margin: 0;
            font-family: "Helvetica Neue" ,Helvetica,Arial,sans-serif;
            font-size: 14px;
            line-height: 20px;
            color: #333333;
            background-color: #ffffff;
        }
        p
        {
            margin: 0 0 10px;
        }
        p[Attributes Style]
        {
            text-align: -webkit-center;
        }
        user agent stylesheetp
        {
            display: block;
            -webkit-margin-before: 1em;
            -webkit-margin-after: 1em;
            -webkit-margin-start: 0px;
            -webkit-margin-end: 0px;
        }
    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#qrcode").attr("src","/ui/pmui/eticket/showtcode.aspx?pno=<%=pno %>");
            $("#qrcode").next().text("辅助码 ：<%=pno %>");
        })
    </script>
    <meta name="viewport" content="width=device-width" />
</head>
<body> 
    <div align="center" class="container-fluid" style="margin-bottom: 30px;">
        <h4 align="center" class="center-block">
            易城二维码</h4>
        <img class="center-block" src="/Images/defaultThumb.png" id="qrcode" width="280px" height="280px" />
        <p align="center" class="center-block">
            辅助码 ： </p>
    </div> 
</body>
</html>
