<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXmessage.aspx.cs" Inherits="ETS2.WebApp.H5.WXmessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
       <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
       <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    
     <script type="text/javascript">
         $(function () {
             var pagee = 1;
             var pageSize = 5; //每页显示条数
             var html_temp = "";
             var name_temp = "";
             var content_temp = "";
             $.ajax({
                 type: "post",
                 url: "/JsonFactory/WeiXinHandler.ashx?oper=wxsendmsglistbytop5",
                 data: { comid: 106 },
                 async: false,
                 success: function (data) {
                     data = eval("(" + data + ")");

                     if (data.type == 1) {
                         $.prompt("查询微信交互信息列表错误");
                         return;
                     }
                     if (data.type == 100) {
                         if (data.msg.length > 0) {

                             for (i = 0; i < data.msg.length; i++) {
                                 if (data.msg[i].KeRenName != "") {
                                     name_temp = data.msg[i].Nickname
                                 } else {
                                     name_temp = data.msg[i].KeRenName
                                 }
                                 if (data.msg[i].MsgType != "text") {
                                     content_temp = "语音咨询";
                                 } else {
                                     //content_temp = data.msg[i].Content;
                                     content_temp = "微信咨询";
                                 }

                                 html_temp = "<li> <a>" + name_temp + " ( " + data.msg[i].CreateTime + ") : " + content_temp + " </a></li>"
                                 $("#wxmessage").append(html_temp);
                             }
                            
                         }
                     }
                 }
             })

             var $this = $(".renav");
             var scrollTimer;
             $this.hover(function () {
                 clearInterval(scrollTimer);
             }, function () {
                 scrollTimer = setInterval(function () {
                     scrollNews($this);
                 }, 2000);
             }).trigger("mouseout");
         })


         function scrollNews(obj) {
             var $self = obj.find("ul:first");
             var lineHeight = $self.find("li:first").height();
             $self.animate({ "margin-top": -lineHeight + "px" }, 600, function () {
                 $self.css({ "margin-top": "0px" }).find("li:first").appendTo($self);
             })
         } 
    </script>
        <style type="text/css">
.renav{
width:400px;
height:21px;
line-height:21px;
overflow:hidden;

font-size:13px;
}
.renav li{
height:21px;
}
</style>
</head>
<body style=" background:#B9D5D7;">
    <form id="form1" runat="server">

<div class="renav">
<ul style="margin-top: 0px;" id="wxmessage">
</ul>
</div> 
    </form>
</body>
</html>
