<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true" CodeBehind="call_back_url.aspx.cs" Inherits="ETS2.WebApp.H5.call_back_url" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.head-return {
width: 11px;
height: 17px;
margin: 20px 15px 0;
overflow: hidden;
display: inline-block;
background: url(/Images/return.png) no-repeat;
background-size: 11px 17px;
}
</style>
    <script type="text/javascript">
        function skippage() {
            if ('<%=bo %>' == "true" || '<%=bo %>' == "True") {
                window.open("http://shop.etown.cn/agent/m", target = "_self");
            } else {
                window.open("http://shop.etown.cn/agent", target = "_self");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <!-- 公共页头  -->
      <header class="header" style="height: 80px;line-height: 80px;background-color: #3CAFDC;">
       <%if (order_type == 1)
                  {
                %>
                <dl>
                    <dd>
                       <h1><%=title %></h1></dd>
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
                  if (order_type == 2)
                  { 
                %>
                <dl>
                    <dt></dt>
                    <dd>
                        <h1><%=title %></h1>
                    </dd>
                </dl>
                 <dl>
                    <dt></dt>
                    <dd>
                        请点击 <a href="javascript:void(0)" onclick="skippage()">此处</a> 跳转
                    </dd>
                </dl>
                <%
                    } 
                %>


                    
        <div class="left-head">
        <%if (order_type == 1)
                  {
                %>
         <a id="A1" href="/h5/order" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a> 
          <%
                    }
                  if (order_type == 2)
                  {
                %>
                 <a id="A2" href="javascript:void(0)"  onclick="skippage()" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a> 
          <%
                    } 
                %>

                 </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
    <footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                 <span style="display:block; padding-bottom:5px;  line-height:20px;"><%=comname %> 服务热线：<a href="tel:<%=phone %>"><%=phone %></a></span>
          </div>
      </footer>
</asp:Content>
