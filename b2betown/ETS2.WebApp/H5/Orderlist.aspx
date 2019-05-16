<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true"
    CodeBehind="Orderlist.aspx.cs" Inherits="ETS2.WebApp.H5.Orderlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 页面样式表 -->
    <style type="text/css">
        .none
        {
            display: none;
        }
        .margin
        {
            margin-top: 50px;
        }
        .scenery-conment a, a:hover
        {
            color: #434e5a;
        }
    </style>
    <!-- 页面样式表 -->
    <link href="../Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;
        $(function () {

            //            $("#list .list-item").clock(function () {
            //                $("#list").css("background", "#e9eff5");
            //            })

            var comid = $("#hid_comid").val();
            var pro_class = $("#hid_proclass").val();
            var pno = $("#hid_pno").val();
            var pnum =  $("#hid_pnum").val();
            if(pno !="" && pnum<=0){
                $("#list").html("<div style=\" font-size:16px; padding-top:20px; text-align:center; vertical-align:middle; font-weight:bold\">此预约码 可使用数量为 0 !</div>");
            }else{

                SearchList(pageindex, 12);
            }



            $("#search_botton").click(function () {
                $("#loading").show();
                var key = $("#search_name").val();
                if (key == "") {
                    $("#search_name").css("border", "1px solid #FF0000");
                    return;
                }
                else {
                    $("#search_name").css("border", "0px solid #fff");
                }
                var pageSize = 12;
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Selectpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, projectid: $("#hid_projectid").val(), proclass: pro_class, pno: pno },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#list").hide();
                            $("#black_top").hide();
                            $("#page").hide();
//                            $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            $("#list").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#list").hide();
                                $("#black_top").hide();
                                $("#page").hide();
                                $("#list").html("<div style=\" font-size:16px; padding-top:20px; text-align:center; vertical-align:middle; font-weight:bold\">查询数据为空</div>");
                            } else {
                                if (data.totalCount - pageindex * pageSize > 0) {
                                    $("#pagea").html("查看更多(" + (data.totalCount - pageindex * pageSize) + ")");
                                } else {
                                    $("#pagea").html("查看更多(0)");
                                }
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                //setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })

            })

            var stop = true;


            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());

                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#pageindex").val()) + 1;
                        var pageSize = parseInt($("#num").val()) + 12;
                        var key = $("#search_name").val();
                      
                        stop = false;
                        $("#loading").show();
                        $.ajax({
                            type: "post",
                            url: "/JsonFactory/ProductHandler.ashx?oper=Selectpagelist",
                            data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, projectid: $("#hid_projectid").val(), proclass: pro_class,pno:pno },
                            async: false,
                            success: function (data) {
                                data = eval("(" + data + ")");
                                stop = true;
                                $("#loading").hide();
                                if (data.type == 1) {
                                    //$("#line-list").hide();
                                    return;
                                }
                                if (data.type == 100) {
                                    $("#loading").hide();
                                    //$("#line-list").empty();
                                    $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                    $("#pageindex").val(pageindex);

                                }
                            }
                        })


                    }
                }
            });


            $("#pagea").click(function () {
                $("#loading").show();
                var pageSize = parseInt($("#num").val()) + 12;
                var key = $("#search_name").val();
                if (key == "") {
                    SearchList(pageindex, pageSize);
                    $("#num").val(pageSize);
                }
                else {
                    pageSize = parseInt($("#num").val()) + 12;
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/ProductHandler.ashx?oper=Selectpagelist",
                        data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, projectid: $("#hid_projectid").val(), proclass: pro_class, pno: pno },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $("#list").hide();
                                $("#black_top").hide();
                                $("#page").hide();
//                                $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                                return;
                            }
                            if (data.type == 100) {
                                $("#loading").hide();
                                $("#list").empty();
                                $("#divPage").empty();
                                if (data.totalCount == 0) {
                                    $("#list").hide();
                                    $("#black_top").hide();
                                    $("#page").hide();
                                    $("#page1").html("<div style=\" font-size:16px;padding-top:20px; text-align:center; vertical-align:middle; font-weight:bold\">查询数据为空</div>");
                                } else {
                                    if (data.totalCount - pageindex * pageSize > 0) {
                                        $("#pagea").html("还有(" + (data.totalCount - pageindex * pageSize) + ")");
                                    } else {
                                        $("#pagea").html("");
                                    }
                                    $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                    //setpage(data.totalCount, pageSize, pageindex);
                                }


                            }
                        }
                    })
                    $("#num").val(pageSize);
                }
            })

            //装载产品列表
            function SearchList(pageindex, pageSize) {
                if (pageindex == '') {
                    $("#list").html("点击查看更多");
                    return;
                }
                //$("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Selectpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, projectid: $("#hid_projectid").val(), proclass: pro_class, viewmethod: "1,3,5", pno: $("#hid_pno").val() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#list").hide();
                            $("#black_top").hide();
                            $("#page").hide();
//                            $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            $("#list").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#list").hide();
                                $("#black_top").hide();
                                $("#page").hide();
                                $("#page1").html("<div style=\" font-size:16px; padding-top:20px; text-align:center; vertical-align:middle; font-weight:bold\">查询数据为空</div>");
                            } else {
                                if (data.totalCount - pageindex * pageSize > 0) {
                                    $("#pagea").html("还有(" + (data.totalCount - pageindex * pageSize) + ")");
                                } else {
                                    $("#pagea").html("");
                                }

                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                //setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })
            }
        });
        //分页
        function setpage(newcount, newpagesize, curpage) {
            $("#divPage").paginate({
                count: Math.ceil(newcount / newpagesize),
                start: curpage,
                display: 5,
                border: false,
                text_color: '#888',
                background_color: '#EEE',
                text_hover_color: 'black',
                images: false,
                rotate: false,
                mouse: 'press',
                onChange: function (page) {

                    SearchList(page, newpagesize);

                    return false;
                }
            });
        }
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
        function csss(id, type) {
            //根据服务类型不同进入不同页面
            if (type == 1 || type == 10) {//票务 或者旅游大巴
                location.href = "OrderEnter.aspx?proclass=<%=proclass %>&id=" + id + "&projectid=" + $("#hid_projectid").val() + "&buyuid=<%=buyuid %>&tocomid=<%=tocomid %>&pno=<%=pno %>";
            } else if (type == 2 || type == 8) {//旅游
                location.href = "http://shop<%=comid%>.etown.cn/h5/linedetail.aspx?lineid=" + id + "&projectid=" + $("#hid_projectid").val() + "&buyuid=<%=buyuid %>&tocomid=<%=tocomid %>&pno=<%=pno %>";
            } else if (type == 9) {//酒店客房
                location.href = "http://shop<%=comid%>.etown.cn/h5/hotel/hotelshow.aspx?proid=" + id + "&id=<%=comid %>&uid=<%=uid %>&pno=<%=pno %>";
            } else if(type == 11){

                location.href = "http://shop<%=comid%>.etown.cn/h5/order/pro.aspx?id=" + id;
            }
            else if (type == 14) {
                location.href = "http://shop<%=comid%>.etown.cn/h5/OrderEnter.aspx?proclass=<%=proclass %>&id=" + id + "&projectid=" + $("#hid_projectid").val() + "&buyuid=<%=buyuid %>&tocomid=<%=tocomid %>&pno=<%=pno %>";
            } 
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 公共页头  -->
    <%
        if (comidture == 1)
        { 
    %>
    <a id="goBack" href="list.aspx?class=<%=proclass %>&buyuid=<%=buyuid%>&tocomid=<%=tocomid %>">
        <header class="header"  style=" background-color: #3CAFDC;">

<%if (pno == "")
  { %>
                    <h1 style=" text-align:center;">在线预订</h1>
        <div class="left-head">
          <a  href="list.aspx?class=<%=proclass %>&buyuid=<%=buyuid%>&tocomid=<%=tocomid %>" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head"> 
        <a href="/yy/" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">在线预约</span></span></a>  
        </div>
           <%}
  else
  { %>
      <h1 style=" text-align:center;">在线预约 - 选择产品</h1>
        <div class="left-head">
          <a href="/YY/" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head" style=" display:none;"> 
         <a href="http://yunding.etown.cn/h5/Order/" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">在线购买</span></span></a>  
         </div>

           <%} %>

        </div>
    </header>
    </a>
    <% 
        }
        else
        {
    %>
    <header class="header"  style=" background-color: #3CAFDC;">
    <%if (pno == "")
  { %>

          <h1 style=" text-align:center;">在线预订</h1>
        <div class="left-head">
        </div>
         <div class="right-head"> 
        <a href="/yy/" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">在线预约</span></span></a>  
        </div>

        <%}
  else
  { %>
      <h1 style=" text-align:center;">在线预约 - 选择产品</h1>
      <div class="left-head">
          <a href="/YY/" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head" style=" display:none;"> 
         <a href="http://yunding.etown.cn/h5/Orderlist.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">在线购买</span></span></a>  
         </div>
           <%} %>
        </header>
    <% 
        }
    %>
    <span id="tickets-url" class="fn-hide"></span><span id="sceneryId" class="fn-hide">
    </span>
    <% if (projectid > 0)
       {
    %>
    <div class="content-wrap">
        <article class="scenerys">
      <div class="body">
        <div class="pic">
          <a href="projectinfoTitle.aspx?id=<%=projectid %>">
          <img src="<%=projectimgurl %>" alt="<%=projectname %>" title="<%=projectname %>" /></a>
          <div class="pic-info">
            <%--10张--%>
          </div> 
          <%--<div class="info-bg">
          </div>--%>
        </div>
        <section class="scenerys-cont">
            <a href="projectinfoTitle.aspx?id=<%=projectid %>">
               <h3><%=projectname%></h3><%--<em style="position: absolute;right: 30px;bottom: 45px;color: #1a9ed9;"></em>--%>
            </a>
            <p class="scenerys-level"></p>
          <p class="scenery-conment">
              <a href="projectinfoTitle.aspx?id=<%=projectid %>">
                 <% if (projectbrief.Length > 45)
                    { 
                    %>
                    <%=projectbrief.Substring(0,45)%>..
                    
                    <%}else{ %>
                    <%=projectbrief%>
                    <%
                                                     }%>
                    <s class="ico-right">
                            </s>
              </a>
          </p>
        </section>
         <div class="clear"></div>
       </div>
        <%--<div class="bottomBox">
          <a href="#"><span class="map"></span>地图查询地址<em class="gray-ico"></em></a>   </div> --%>
      </article>
    </div>
    <%
        } %>
    <!-- 页面内容块 -->
    <div id="page1">
        <%--<div class="list-search">
            <dl class="fn-clear">
                <dt>
                    <input type="text" placeholder="请输入景点或名称" id="search_name"/>
                </dt>
                <dd>
                    <s  id="search_botton"></s>
                </dd>
            </dl>
        </div>--%>
        <div id="list">
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
                
  <div  class="list-item fn-clear" onclick="csss('${Id}','${Server_type}')">
                        <div class="pic">
                         {{if Ispanicbuy==1}}<span class="status_qianggou">状态：抢购</span>{{/if}}

                            {{if Imgurl!=""}}<img src="${Imgurl}">{{/if}}
                        </div>
                        <div class="info">
                            <h5><!--${SubstrDome(Projectname,17)}--> ${SubstrDome(Pro_name,22)}</h5>
                            {{if Server_type==1}}
                            <div class="i-note">
                                <p style="color: #1a9ed9;"><span>
                                     ${Pro_explain}
                                     
                                      
                                     
                                </span>
                                </p>
                            </div>
                            {{/if}}
                        </div>
                        <div class="price">
                         {{if Server_type==9}}
                                  {{if HousetypeNowdayprice==0||HousetypeNowdayprice==0.00}}
                                  暂无报价
                                  {{else}}
                                  ¥${HousetypeNowdayprice}<em>起</em>
                                  {{/if}}
                        {{else}}
                                  {{if Advise_price==0||Advise_price==0.00}}
                                    暂无报价
                                    {{else}}
                                      ¥${Advise_price}<em>起</em>  
                                   {{/if}}
                         {{/if}}


                           
                           
                        </div>
                        <span class="arrow-right"></span>
                        <span class="jifen-right">{{if Pro_Integral >0}}会员积分 ${Pro_Integral}{{/if}}</span>
                    </div>
                     
        </script><%if (pno == "")
                   { %>
        <div id="page" class="pagelist" style="display: block">
            <a class="pageList-more" id="pagea" href="javascript:;"></a>
        </div>
        <div class="topdiv">
            <a id="black_top" href="#" style="display: inline;">返回顶部</a></div>
            <%} %>
    </div>

    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="num" type="hidden" value="10" />
    <input id="hid_pno" type="hidden" value="<%=pno %>" />
    <input id="hid_pnum" type="hidden" value="<%=usepnonum %>" />
    <input id="hid_openid" type="hidden" value="" />
    <input id="hid_projectid" type="hidden" value="<%=projectid %>" />
    <input id="hid_proclass" type="hidden" value="<%=proclass %>" />
    <input id="pageindex" type="hidden" value="1" />
</asp:Content>
