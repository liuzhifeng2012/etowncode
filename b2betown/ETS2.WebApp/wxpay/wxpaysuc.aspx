<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxpaysuc.aspx.cs" Inherits="ETS2.WebApp.wxpay.wxpaysuc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=comname%></title>
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
    <script type="text/javascript">
        function skippage() { 
           if('<%=bo %>'=="true"||'<%=bo %>'=="True"){
               window.open("http://shop.etown.cn/agent/m",target="_self");
           }else{
               window.open("http://shop.etown.cn/agent",target="_self");
           }
       }



    </script>
    <style type="text/css">
        .none
        {
            display: none;
        }
    </style>
    <!-- 页面样式表 -->
    <link href="../Styles/H5/scenery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/H5/list.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <div>
        <!-- 公共页头  -->
        <header class="header" style="height: 80px; line-height: 80px;background-color: #3CAFDC;">
                    <h1>支付完成页面</h1>
        <div class="left-head">
          <a href="javascript:history.go(-1);" class="tc_back head-btn">
          <span class="inset_shadow"><span  ></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
        <!-- 页面内容块 -->
        <div class="body">
            <div class="body-content" style="text-align: center; font-size: 25px; padding: 30px 0;">
                <%if (order_type == "1")
                  {
                %>
                <dl>
                    <dd>
                        恭喜你，支付成功!</dd>
                </dl>
                 <dl>
                    <dt></dt>
                    <dd>
                    <%if (servertype == 13)
                      { %>
                       请点击 <a href="http://shop<%=comid %>.etown.cn/h5/order/Coachyuyuesuc.aspx?id=<%=orderid %>&md5=<%=md5 %>">此处</a> 跳转
                     <script type="text/javascript">
                         window.setTimeout("window.location='http://shop<%=comid %>.etown.cn/h5/order/Coachyuyuesuc.aspx?id=<%=orderid %>&md5=<%=md5 %>'", 3000); 
                    </script> 
                    <%}
                      else if (servertype == 12)
                      {
                          %>

                          请点击 <a href="http://shop<%=comid %>.etown.cn/h5/order/yuyuesuc.aspx?id=<%=orderid %>&md5=<%=md5 %>">此处</a> 跳转
                     <script type="text/javascript">
                         window.setTimeout("window.location='http://shop<%=comid %>.etown.cn/h5/order/yuyuesuc.aspx?id=<%=orderid %>&md5=<%=md5 %>'", 2000); 
                    </script> 

                          <%
                        }
                      else
                      { %>

                        请点击 <a href="http://shop<%=comid %>.etown.cn/h5/order">此处</a> 跳转
                        <%} %>

                    </dd>
                </dl>
                <%
                    }
                  if (order_type == "2")
                  { 
                %>
                <dl>
                    <dt></dt>
                    <dd>
                        恭喜你，充值成功!
                    </dd>
                </dl>
                 <dl>
                    <dt></dt>
                    <dd>
                        请点击 <a href="http://shop<%=comid %>.etown.cn/h5/order">此处</a> 跳转
                    </dd>
                </dl>
                <%
                    } 
                %>
                
            </div>
        </div>
        <footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                  <span style="display:block; padding-bottom:5px;  line-height:20px;">服务热线：<a href="tel:<%=phone %>"><%=phone %></a></span>
          </div>
      </footer>
    </div>
</body>
</html>
