<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ETS2.WebApp.Agent.m._default"
    MasterPageFile="/Agent/m/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <!-- 页面样式表 -->
    <link href="/Styles/H5/scenery.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/H5/mh5pro.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();

            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                if (pageindex == '') {

                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=warrantlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid, key: $("#search_name").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询渠道列表错误");
                            return;
                        }
                        if (data.type == 100) {

                            if (data.totalCount == 0) {
                                if(pageindex==1){
                                  $("#list").html("授权商户列表为空，请联系商户授权！");
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
        function openlink(comid) {
            location.href = "ProjectList.aspx?comid=" + comid;
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <header class="header">
          <h1>在线预订</h1>
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
                 <div  class="list-item fn-clear" onclick="openlink('${Comid}');this.style.background = '#e9eff5';">
                        <div class="pic">
                            {{if ComPhoneLogo != "/Images/defaultThumb.png" && ComPhoneLogo !="http://image.etown.cn/UploadFile/2014030010543114093.jpg"}}
                            <img src="${ComPhoneLogo}">
                            {{else}}
                            <img src="${Topimage}">
                            {{/if}}
                        </div>
                        <div class="info">
                            <h5>${SubstrDome(Comname,11)}   </h5>
                            <div class="i-note">
                                <p style="color: #1a9ed9;margin-right:70px;"><span>
                                    <em><a href="/agent/m/Recharge.aspx?comid=${Comid}">在线充值</a></em>
                                </span>
                                </p>
                            </div>
                        </div>
                        <div class="price">
                               <em>余额:${Imprest}元</em>
                                <em><a  onclick="openlink('${Comid}');this.style.background = '#e9eff5';"  >进入商户</a></em>
                        </div> 
                        <span class="arrow-right"></span>
                    </div>
        </script>
    </div>
    <input id="hid_pageindex" type="hidden" value="1" />
</asp:Content>
