<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Project.aspx.cs" Inherits="ETS2.WebApp.Agent.m.Project1" MasterPageFile="/Agent/m/Site2.Master" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <!-- 页面样式表 -->
    <link href="/Styles/H5/scenery.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/H5/mh5pro.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/morder.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/agent/m/css/cart.css" >
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();

            SearchList(1);
            var stop = true;
            //装载列表
            function SearchList(pageindex) {
                stop = true;
                var key = $("#key").val();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=projectlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#loading").hide();
                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {

                            if (data.totalCount == 0) {
                                $("#loading").hide();
                                $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                            } else {
                                $("#loading").hide();
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })

            }

            //查询
            $("#Search").click(function () {
                $("#tblist").empty();
                SearchList(1);
            })


           
            $(window).scroll(function () {

                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                if ($(document).height() <= totalheight) {
                    if (stop == true) {
                        var pageindex = parseInt($("#hid_pageindex").val()) + 1;

                        stop = false;
                        $("#loading").show();
                        SearchList(pageindex);
                    }
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

                        SearchList(page);

                        return false;
                    }
                });
            }
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <header class="header">
          <h1>项目列表</h1>
         <div class="left-head"> 
              <a href="#javascript:history.go(-1)" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="head-return1"></span></span>
              </a> 
            </div>
        <div class="right-head"> 
                <a href="loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>  
        </div>
        </header>
    <span id="tickets-url" class="fn-hide"></span><span id="sceneryId" class="fn-hide">
    </span>
    <!-- 页面内容块 -->
    <div id="page1">
        <div class="list-search">
            <dl class="fn-clear">
                <dt>
                    <input type="text" id="key" />
                </dt>
                <dd>
                    <s id="Search"></s>
                </dd>
            </dl>
        </div>
        <div id="tblist">
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit">
        <div class="layout-box">
        <a href="Project_Count.aspx?id=${Id}"><ul class="list-a">
             <li>名称： ${Projectname}</li>
             <li>全部：${All_Use_pnum}</li>
             <li>上月：${YoM_Use_pnum}</li>
             <li>本月：${ToM_Use_pnum}</li>
             <li>昨天：${Yday_Use_pnum}</li>
             <li>今天：${Today_Use_pnum}</li>
            <li><a href="Project_Count.aspx?id=${Id}">查看此项目</a></li>   
        </ul></a>
    </div> 
    </script>

    </div>
    <div id="loading" class="loading" style="display: none;">
        正在加载...
    </div>
    <input id="hid_pageindex" type="hidden" value="1" />
</asp:Content>
