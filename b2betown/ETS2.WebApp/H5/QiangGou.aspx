<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/H5/Order.Master" CodeBehind="QiangGou.aspx.cs" Inherits="ETS2.WebApp.H5.QiangGou" %>

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
    <link href="/Styles/H5/mh5pro.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var pageindex = 1;
        $(function () {

            //            $("#list .list-item").clock(function () {
            //                $("#list").css("background", "#e9eff5");
            //            })

            var comid = $("#hid_comid").val();
            var pro_class = $("#hid_proclass").val();

            SearchList(pageindex, 12);

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
                        data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: '抢购'},
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $("#list").hide();
                                $("#black_top").hide();
                                $("#page").hide();
                                $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
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
                                    $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">查询数据为空</div>");
                                } else {
                                    if (data.totalCount - pageindex * pageSize > 0) {
                                        $("#pagea").html("查看更多(" + (data.totalCount - pageindex * pageSize) + ")");
                                    } else {
                                        $("#pagea").html("查看更多(0)");
                                    }
                                    $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                    setpage(data.totalCount, pageSize, pageindex);
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
                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Selectpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key:'抢购' },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#list").hide();
                            $("#black_top").hide();
                            $("#page").hide();
                            $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
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
                                $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">查询数据为空</div>");
                            } else {
                                if (data.totalCount - pageindex * pageSize > 0) {
                                    $("#pagea").html("查看更多(" + (data.totalCount - pageindex * pageSize) + ")");
                                } else {
                                    $("#pagea").html("查看更多(0)");
                                }

                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                                setpage(data.totalCount, pageSize, pageindex);
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
            $("#loading").show();
            //根据服务类型不同进入不同页面
            if (type == 1) {//票务
                location.href = "/h5/Order/pro.aspx?id=" + id + "&projectid=" + $("#hid_projectid").val() + "&buyuid=<%=buyuid %>&tocomid=<%=tocomid %>";
            } else if (type == 2 || type == 8) {//旅游
                location.href = "linedetail.aspx?lineid=" + id + "&projectid=" + $("#hid_projectid").val() + "&buyuid=<%=buyuid %>&tocomid=<%=tocomid %>";
            } else if (type == 9) {//酒店客房
                location.href = "hotel/hotelshow.aspx?proid=" + id + "&id=<%=comid %>&uid=<%=uid %>";
            } else {
                location.href = "/h5/Order/pro.aspx?id=" + id + "&projectid=" + $("#hid_projectid").val() + "&buyuid=<%=buyuid %>&tocomid=<%=tocomid %>";
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
    <a id="goBack" href="List.aspx">
        <header class="header">
                    <h1>抢购</h1>
        <div class="left-head">
          <a  href="javascript:self.location=document.referrer;" class="tc_back head-btn">
          <span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head">
           <%if (Wxfocus_url != "")
           { %>
                <%if (comid != 106 || tocomid == 0 || tocomid == 106)
                      { %>
                <a href="<%=Wxfocus_url%>" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;"><%=Wxfocus_author%></span></span></a>
                <%}
           } %>


        </div>
    </header>
    </a>
    <% 
        }
        else
        {
    %>
    <header class="header">
          <h1>抢购</h1>
        <div class="left-head">
        </div>
        <div class="right-head">
           <%if (Wxfocus_url != "")
           { %>
                <%if (comid != 106 || tocomid == 0 || tocomid == 106)
                      { %>
                <a href="<%=Wxfocus_url%>" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;"><%=Wxfocus_author%></span></span></a>
                <%}
           } %>
        </div>
        </header>
    <% 
        }
    %>
    <span id="tickets-url" class="fn-hide"></span><span id="sceneryId" class="fn-hide">
    </span>
    <!-- 页面内容块 -->
    <div id="page1">

        <div id="list">
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit"> 
                 <div  class="list-item fn-clear" onclick="csss('${Id}','${Server_type}');this.style.background = '#e9eff5';">
                        <div class="pic">
                        <span class="status_qianggou">状态：抢购</span>
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
                    </div>
        </script>
        <div id="page" class="pagelist" style="display: block">
            <a class="pageList-more" id="pagea" href="javascript:;"></a>
        </div>
        <div class="topdiv">
            <a id="black_top" href="#" style="display: inline;">返回顶部</a></div>
      
    </div>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="num" type="hidden" value="10" />

</asp:Content>
