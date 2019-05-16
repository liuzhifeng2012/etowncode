﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Project_Yanpiaolist.aspx.cs" Inherits="ETS2.WebApp.Agent.m.Project_Yanpiaolist" MasterPageFile="/Agent/m/Site2.Master"  %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
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

            //日历
            var nowdate = '<%=startime %>';
            var enddate = '<%=endtime %>';

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();

                $("#startime").val(nowdate);
                $("#endtime").val(enddate);

            });

            SearchList(1);

            var stop = true;

            //装载列表
            function SearchList(pageindex) {
                var startime = $("#startime").val();
                var endtime = $("#endtime").val();
                var Id = $("#hid_id").val();
                stop = true;
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=pnolistbyproid",
                    data: { pageindex: pageindex, pagesize: pageSize, Id: Id, startime: startime, endtime: endtime },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data == "err") {
                            $("#loading").hide();
                            $.prompt("查询错误");
                            return;
                        }

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
          <h1>验证数据</h1>
         <div class="left-head"> 
              <a href="javascript:history.go(-1)" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="home-10"></span></span>
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
            <dl class="">
                <div>
                  起始 <input name="startime" type="text" id="startime" style=" width:80px;"  isdate="yes"/> 截止 <input name="endtime" style=" width:80px;"  type="text" id="endtime" isdate="yes"/>
                </div>
                <dd>
                    <s id="search_botton"></s>
                </dd>
            </dl>
        </div>

        <div id="tblist">
        </div>
        <div style=" height:20px;"></div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit">
         <div class="layout-box">
        <ul class="list-a">
             <li>名称： ${ProName}</li>
             <li>订单号：${Orderid}</li>
             <li>验证数量：${UseNum}</li>
             <li>门市价：${FacePrice}</li>
             <li>电子码：${Pno}</li>
             <li>验证机器号：{{if PosId !="0"}}
                                ${PosId} 
                                {{/if}}
                                {{if Pcaccount !=""}}
                                ${Pcaccount}
                                {{/if}}</li>
             <li>验证日期：${ConfirmDate}</li>
        </ul>
    </div> 

    </script>

    </div>
        <div id="loading" class="loading" style="display: none;">
        正在加载...
    </div>
    <input id="hid_id" type="hidden" value="<%=Id %>" />
    <input id="hid_pageindex" type="hidden" value="1" />
    
</asp:Content>
