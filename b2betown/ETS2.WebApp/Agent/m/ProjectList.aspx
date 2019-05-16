<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectList.aspx.cs" Inherits="ETS2.WebApp.Agent.m.ProjectList"
    MasterPageFile="/Agent/m/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <!-- 页面样式表 -->
    <link href="/Styles/H5/scenery.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/H5/mh5pro.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/agent/m/css/cart.css">
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            $("#h_comname").text($("#hid_company").trimVal());
            $("#div_comname").html("商户:" + $("#hid_company").trimVal());
            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=warrantprojectlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid, comid: $("#hid_comidtemp").trimVal(), key: $("#search_name").trimVal() },
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
            $("#search_botton").click(function () {

                SearchList(1);
            })

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
            location.href = "Manage_sales.aspx?projectid=" + id + "&comid=" + $("#hid_comidtemp").trimVal();

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
              <a href="javascript:history.go(-1)" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="head-return"></span></span>
              </a> 
            </div>
        <div class="right-head"> 
                <a href="loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>  
        </div>
        </header>
    <div class="container ">
        <div class="tabber  tabber-n3 tabber-double-11 clearfix">
           <a href="javascript:goshopproject()" class="active">
                产品列表</a><a class="" href="javascript:goorder()">订单列表</a> <a href="javascript:gofinane()">
                    财务记录</a> <a href="javascript:goshopcart()">购物车</a> 
        </div>
    </div>
    <span id="tickets-url" class="fn-hide"></span><span id="sceneryId" class="fn-hide">
    </span>
    <!-- 页面内容块 -->
    <div id="page1">
        <div class="list-search">
            <dl class="fn-clear">
                <dt>
                    <input type="text" id="search_name" />
                </dt>
                <dd>
                    <s id="search_botton"></s>
                </dd>
            </dl>
        </div>
        <div id="list">
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
                 <div  class="list-item fn-clear" onclick="openlink('${Id}');this.style.background = '#e9eff5';">
                        <div class="pic">
                            <img src="${Projectimg}">
                        </div>
                        <div class="info">
                            <h5>${SubstrDome(Projectname,11)}   </h5>
                            <div class="i-note">
                                <p style="color: #1a9ed9;margin-right:70px;"><span>
                                      ${SubstrDome(Briefintroduce,11)} 
                                </span>
                                </p>
                            </div>
                        </div>
                        <div class="price">
                               <em>进入购买</em>
                        </div> 
                        <span class="arrow-right"></span>
                    </div>
        </script>
    </div>
    <input id="hid_pageindex" type="hidden" value="1" />
</asp:Content>
