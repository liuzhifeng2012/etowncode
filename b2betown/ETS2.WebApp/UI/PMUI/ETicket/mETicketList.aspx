<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mETicketList.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.ETicket.mETicketList"
    MasterPageFile="/UI/pmui/ETicket/mEtown.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8">
    <meta name="keywords" content="微商城">
    <meta name="description" content="">
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">
    <title></title>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!-- CSS -->
    <link rel="stylesheet" href="/agent/m/css/cart.css">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/morder.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            $("#h_comname").text($("#hid_comname").trimVal());

            $("#search_botton").click(function () {
                SearchList(1);
            })

            SearchList(1);

            function SearchList(pageindex) {
                $.post("/JsonFactory/EticketHandler.ashx?oper=pagelist", { comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pageSize, key: $("#pno").trimVal(), eclass: -1 }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $(".empty-list").show();
                        $("#backs-list-container").find("li").hide();
                        return;
                    }
                    if (data.type == 100) {

                        if (data.totalCount == 0) {
                            if (pageindex == 1) {
                                //                                    $("#list").html("查询数据为空");
                                $(".empty-list").show();
                                $("#backs-list-container").find("li").hide();
                            }
                        } else {
                            if (pageindex == 1) {
                                $("#list").empty();
                            }

                            stop = true;
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#list");
                            $("#hid_pageindex").val(pageindex);

                            $(".empty-list").hide();
                            $("#backs-list-container").find("li").show();
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


        function goyanzheng() {
            location.href = "/ui/pmui/ETicket/mETicketIndex.aspx";
        }
        function goyanzhenglist() {
            location.href = "/ui/pmui/ETicket/mETicketList.aspx";
        }
        function golvyoudaba() {
            location.href = "/ui/MemberUI/mTravelbusOrderCount.aspx";
        }
 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <header class="header" style="background-color: #3CAFDC;">
          <h1 id="h_comname"></h1>
        <div class="left-head"> 
                 <a href="/ui/pmui/ETicket/mETicketIndex.aspx" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="home-10"></span></span>
              </a> 
            </div>
        <div class="right-head"> 
                <a href="/yanzheng/loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>    
        </div>
        </header>
    <!-- container -->
    <div class="container ">
        <div class="tabber  tabber-n3 tabber-double-11 clearfix">
            <a href="javascript:goyanzheng()" style="width: 33%;">验证电子凭证</a> <a class="active"
                href="javascript:goyanzhenglist()" style="width: 33%;">验证列表</a> <a href="javascript:golvyoudaba()"
                    style="width: 33%;">旅游大巴查询</a>
        </div>
        <div class="list-search" style="height: 32px; padding: 5px 10px 7px; background: #eee;">
            <dl class="fn-clear" style="height: 32px; background: #fff; border-radius: 5px; border: 1px solid #c9c9c9;
                position: relative;">
                <dt style="position: relative; overflow: hidden; padding-left: 5px; margin-right: 40px;">
                    <input type="tel" id="pno" placeholder="请输入电子码" style="height: 25px; margin-top: 4px;
                        border: 0; outline: 0; background: 0; width: 100%;" />
                </dt>
                <dd style="float: left; width: 30px; height: 25px; margin-top: 4px; position: absolute;
                    top: 0; right: 0;">
                    <s id="search_botton" style="width: 17px; height: 17px; display: block; vertical-align: middle;
                        margin: 4px auto 0; background: url(/Images/public_com.png) no-repeat -44px 0;
                        background-size: 64px 17.5px;"></s>
                </dd>
            </dl>
        </div>
        <div id="backs-list-container" class="block">
            <li class="block block-order animated">
                <div class="block block-list block-border-top-none block-border-bottom-none" id="list"
                    style="padding-left: 0;">
                </div>
            </li>
            <div class="empty-list" style="margin-top: 30px;">
                <!-- 文本 -->
                <div>
                    <h4>
                        哎呀，暂不记录？</h4>
                </div>
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
     <div class="layout-box">
        <ul class="list-a"> 
            <li>电子凭证： ${Pno} </li>
            <li>使用数量：<strong class="r1">&nbsp; ${UseNum}张</strong></li>
            <li>验证日期:${ConfirmDate} </li>
            <li>验票账户: 
             {{if PosId !="0"&&PosId!="999999999"}} ${PosId} {{/if}}
             {{if Pcaccount !=""}} ${Pcaccount} {{/if}}
            </li>

            <li>产品名称：<strong class="r1">&nbsp; ${ProName}</strong></li>
            <!--<li>出票单位：<strong class="r1">&nbsp;{{if Outcompany==""}} -- {{else}} ${Outcompany} {{/if}}</strong></li>
            <li>有效期： ${ChangeDateFormat(ProEnd)} </li> 
            <li>订单号: ${Orderid}</li>-->
           
        </ul>
    </div>
    </script>
    <input id="hid_pageindex" type="hidden" value="1" />
</asp:Content>
