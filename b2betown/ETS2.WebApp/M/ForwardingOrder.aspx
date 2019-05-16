<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true"
    CodeBehind="ForwardingOrder.aspx.cs" Inherits="ETS2.WebApp.M.ForwardingOrder" %>

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
        <link href="../Styles/css4.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;
        $(function () {

            var comid = $("#hid_comid").val();
            SearchList(pageindex, 20);

            //装载排名列表
            function SearchList(pageindex, pageSize) {
                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=forwardingpagelist",
                    data: { comid: comid, wxid: <%=aid%>, pageindex: pageindex, pagesize: pageSize },
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

                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 公共页头  -->

          <header class="header">
          <h1>转发排行</h1>
        <div class="left-head">
        </div>
        <div class="right-head">
          <a href="#" class=""><span class="inset_shadow"><span class=""></span></span></a>
        </div>
        </header>
    <span id="tickets-url" class="fn-hide"></span><span id="sceneryId" class="fn-hide">
    </span>
    <div class="content-wrap">
        <article class="scenerys">
      <div class="body">

        <section class="scenerys-cont">
               <h3><%=wxTitle%></h3>
            <p class="scenerys-level"></p>
          <p class="scenery-conment">
                 <%=wxsummary%>
          </p>
        </section>
         <div class="clear"></div>
       </div>
      </article>
    </div>
    <!-- 页面内容块 -->
    <div id="page1" style="min-height:450px">
        <table width="100%" border="0" >
                    <tr>
                        <td width="30%">
                              <p align="center"> 姓名</p>
                        </td>
                        <td width="40%">
                              <p align="center">卡号</p>
                        </td>
                        <td width="30%">
                            <p align="center">转发访问量</p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                    <script type="text/x-jquery-tmpl" id="ProductItemEdit">  
                     <tr> 
                        <td height="26" class="sender item">
                            <p align="center">
                               ${Name}
                            </p>
                        </td>
                        <td height="26" class="sender item">
                            <p align="center">
                             ${Idcard}  </p>
                        </td>

                        <td height="26" class="sender item">
                            <p align="center">
                               ${Fornum} </p>
                        </td>
                    </tr>
                    </script>
                </table>
        <div class="footFix" id="footReturn">
            <a href="indexcard.aspx"><span>返回会员卡首页</span></a>
        </div>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="num" type="hidden" value="20" />
</asp:Content>
