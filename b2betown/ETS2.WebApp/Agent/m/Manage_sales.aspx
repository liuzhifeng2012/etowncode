<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manage_sales.aspx.cs" Inherits="ETS2.WebApp.Agent.m.Manage_sales"
    MasterPageFile="/Agent/m/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <!-- 页面样式表 -->
    <link href="/Styles/H5/scenery.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/H5/mh5pro.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/cart.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comidtemp").trimVal();
            var projectid = $("#hid_projectid").trimVal();

            $("#h_comname").text($("#hid_company").trimVal());
            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=warrantprolist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid, comid: $("#hid_comidtemp").trimVal(), key: '', projectid: projectid, viewmethod: "1,2" },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {

                            return;
                        }
                        if (data.type == 100) {

                            if (data.totalCount == 0) {
                                if (pageindex == 1) {
                                    $("#list").html("查询数据为空");
                                }
                            } else {
                                if (pageindex == 1) {
                                    $("#list").empty();
                                }

                                stop = true;
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                $("#hid_pageindex").val(pageindex);
                            }

                        }
                    }
                })
            }


            var stop = true;
            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#hid_pageindex").val()) + 1;

                        stop = false;
                        SearchList(pageindex);
                    }
                }
            });



        })
        function SubstrDome(s, num) {
            var ss;
            if (s.length > num) {
                ss = s.substr(0, num) + "..";
                return (ss);
            }
            else {
                return (s);
            }

        }
        function openlink(id) {
            var comid = $("#hid_comid_temp").trimVal();
            location.href = "pro_sales.aspx?id=" + id + "&comid=" + $("#hid_comidtemp").trimVal();
        }
        function goshopproject() {
            location.href = "/agent/m/Manage_sales.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goshopcart() {
            location.href = "/agent/m/ShopCart.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function goorder() {
            location.href = "/agent/m/order.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
        function gofinane() {
            location.href = "/agent/m/Finane.aspx?comid=" + $("#hid_comidtemp").trimVal();
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <header class="header">
        <h1 id="h_comname"></h1>
        <div class="left-head"> 
              <a href="/agent/m/default.aspx" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="home-10"></span></span>
              </a> 
            </div>
        <div class="right-head"> 
                <a href="loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>  
        </div>
        </header>
    <div class="container ">
        <div class="tabber  tabber-n3 tabber-double-11 clearfix">
             <a href="javascript:goshopproject()" class="active">
                产品列表</a> <a class="" href="javascript:goorder()">订单列表</a> <a href="javascript:gofinane()">
                    财务记录</a><a href="javascript:goshopcart()">购物车</a>
        </div>
    </div>
    <span id="tickets-url" class="fn-hide"></span><span id="sceneryId" class="fn-hide">
    </span>
    <%if (projectid != 0)
      { %>
    <div class="content-wrap">
        <article class="scenerys">
      <div class="body">
        <div class="pic">
          <a href="projectinfoTitle.aspx?id=<%=projectid %>">
          <img src="<%=projectimgurl %>" alt="<%=projectname %>" title="<%=projectname %>" /></a>
          <div class="pic-info">
      
          </div>  
        </div>
        <section class="scenerys-cont">
            <a href="projectinfoTitle.aspx?id=<%=projectid %>">
               <h3><%=projectname%></h3> 
            </a>
            <p class="scenerys-level"></p>
          <p class="scenery-conment">
              <a href="projectinfoTitle.aspx?id=<%=projectid %>" style="color: #434e5a; ">
                 <% if (projectbrief.Length > 45)
                    { 
                    %>
                    <%=projectbrief.Substring(0, 45)%>..
                    
                    <%}
                    else
                    { %>
                    <%=projectbrief%>
                    <%
                        }%>
                  <%--  <s class="ico-right">
                            </s>--%>
              </a>
          </p>
        </section>
         <div class="clear"></div>
       </div>
      
      </article>
    </div>
    <%} %>
    <div id="page1">
        <div id="list">
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
       
                 <div  class="list-item fn-clear" onclick="openlink('${Id}');this.style.background = '#e9eff5';">
                        <div class="pic">
                            <img src="${ProImg}">
                        </div>
                        <div class="info">
                            <h5>${SubstrDome(Pro_name,21)}&nbsp;&nbsp;
                            {{if IsViewStockNum==1}}
                            <span style="color: #1a9ed9;font-size:12px;">库存:${StockNum}</span>
                            {{/if}}
                               </h5>
                            <div class="i-note">
                                <p style="color: #1a9ed9;margin-right:70px;"><span>
                                      ${SubstrDome(Pro_explain,11)} 
                                </span>
                                </p>
                            </div>
                        </div>
                        <div class="price">
                            {{if Manyspeci==1}}
                                立即购买
                            {{else}}
                             {{if Agent_price>0}} ¥${Agent_price}<em>起</em>{{else}}已售完{{/if}}
                             {{/if}}
                        </div> 
                        <span class="arrow-right"></span>
                    </div>
                    <!--{{if Manyspeci==1}}
                        {{each(i,user) GuigeList}}
                        <div  class="list-item fn-clear" style="background:#cccccc; padding-left:50px;">
                            <div class="info"> ${Pro_name}${user.speci_name}  门市价:¥${user.speci_face_price}  销售价：¥${user.speci_advise_price} 结算价：¥${user.speci_agentsettle_price} </div>
                        </div> 
                         {{/each}}
                     {{/if}}--!>
       
        </script>
    </div>
    <div id='cart' style="display:none ; position: absolute; bottom: 6em; right: 2em; width: 55px;
        height: 55px; background-color: #FFFAFA; margin: 10px; border-radius: 60px; border: solid rgb(55,55,55)  #FF0000   1px;
        cursor: pointer;" class="float-buy-bar">
        <div class="cart">
            <a href="ShopCart.aspx?comid=<%=comid %>"></a>
        </div>
    </div>
    <input id="hid_pageindex" type="hidden" value="1" />
    <input id="hid_projectid" type="hidden" value="<%=projectid %>" />
</asp:Content>
