<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ETS2.WebApp.byts._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>会员中心 - 北京青年旅行社股份有限公司 - 总社 BYTS.cn</title>
    <link href="style/byts2013.css" rel="stylesheet" type="text/css" />
    <link rel="icon" href="/favicon.ico" type="image/x-icon" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/Styles/style.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/Impromptu.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <style type="text/css">
        .top20disp-col2
        {
            padding-left: 20px;
        }
        .top20disp-col2 a
        {
            border-bottom: 1px solid #aaa;
        }
        .top20disp-col2 a:hover
        {
            border: none;
        }
        .byts_menu_1000 a
        {
            color: #FFFDFE;
            display: block;
            font-size: 14px;
            height: 30px;
            padding: 0 17px 0 15px;
            text-align: center;
        }
        h2
        {
            margin: 0;
            padding: 0;
            font-size: 1.083em;
            font-weight: 500;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var comid = $("#comid").trimVal();
            //            var Scenic_intro = $("#hid_Scenic_intro").trimVal();
            //            var Scenic_name = $("#hid_Scenic_name").trimVal();

            //            $("#comremark").html(Scenic_intro);
            //            $(".mod-left h3").html(Scenic_name + "介绍");


            //活动
            var pageSize = 10; //每页显示条数
            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                $("#tblist").empty();
                $("#divPage").empty();

                //获取未领取活动
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=UnActlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.totalCount > 0) {
                                var tab_i = 1;
                                //填充列表数据
                                $.each(data.msg, function (i, item) {
                                    $("#ulreply").append('<li class="cl" onclick="javascript:tab_select(2,' + tab_i + ');">'
                                        + '<div class="pro1 titlefont">'
                                           + item.Title
                                            + '<span id="tab_sun_2_' + tab_i + '_biao"> ▼</span>'
                                        + '</div>'
                                        + '<div class="pro2">'
                                            + item.Money
                                        + '</div>'
                                        + '<div class="pro3 pricefont">'
                                             + ChangeDateFormat(item.Actend)
                                        + '</div>'
                                        + '<div class="pro4" style="color:#FF6600;" align="right">'
                                        + '<input type="button" onclick="ClaimCoupon(' + item.Id + ')" value=" 点击领取 " style="width: 80px; height: 25px;background-color: #ff6600;color:#ffffff;font-weight: 700;" />'
                                        + '</div>'
                                        + '<div class="pro10"  id="tab_sun_2_' + tab_i + '" style="display: none;">'
                                        + '<p style="font-size:12px">优惠活动说明 : <br />'
                                        + item.Remark
						                + '</div>'
                                        + '</li>');

                                    tab_i = tab_i + 1;

                                })

                            }
                        }
                    }
                })

                //获取已有活动列表
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=Actlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("活动列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            if (data.totalCount == 0) {
                                $("#ulreply").append('<li class="cl" onclick="javascript:tab_select(1,' + tab_i + ');">'
                                        + '<div class="pro1 titlefont">'
                                        + '暂时无专享项目'
						                + '</div>'
                                        + '</li>');
                            } else {
                                var tab_i = 1;
                                $.each(data.msg, function (i, item) {

                                    if (item.Usestate == "未使用") {
                                        $("#ulreply").append('<li class="cl" onclick="javascript:tab_select(1,' + tab_i + ');">'
                                        + '<div class="pro1 titlefont">'
                                           + item.Title
                                        + '<span id="tab_sun_1_' + tab_i + '_biao"> ▼</span></div>'
                                        + '<div class="pro2">'
                                            + item.Money
                                        + '</div>'
                                        + '<div class="pro3 pricefont">'
                                             + ChangeDateFormat(item.Actend)
                                        + '</div>'
                                        + '<div class="pro4" style="color:#FF6600;" align="right">'
                                        + item.Usestate
                                        + '</div>'
                                        + '<div class="pro10"  id="tab_sun_1_' + tab_i + '" style="display: none;">'
                                        + '<p style="font-size:12px">优惠活动说明 : <br />'
                                        + item.Remark
						                + '</div>'
                                        + '</li>');
                                    } else {
                                        $("#ulreply").append('<li class="cl" onclick="javascript:tab_select(1,' + tab_i + ');">'
                                        + '<div class="pro1 titlefont" >'
                                           + item.Title
                                        + '<span id="tab_sun_1_' + tab_i + '_biao"> ▼</span></div>'
                                        + '</div>'
                                        + '<div class="pro2">'
                                            + item.Money
                                        + '</div>'
                                        + '<div class="pro3 pricefont">'
                                             + ChangeDateFormat(item.Actend)
                                        + '</div>'
                                        + '<div class="pro4" align="right">'
                                        + item.Usestate
                                        + '</div>'
                                        + '<div class="pro10"  id="tab_sun_1_' + tab_i + '" style="display: none;">'
                                        + '<p style="font-size:12px">优惠活动说明 : <br />'
                                        + item.Remark
						                + '</div>'
                                        + '</li>');

                                    }
                                    tab_i = tab_i + 1;
                                })
                                //setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })

            }

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

            //tab切换
            $("ul.tab li").click(function () {
                var c = $("ul.tab li");
                var div_name = "bd_" + c.index(this);

                $("div.bd").hide();
                $("#" + div_name).show();
                c.eq(c.index(this)).addClass("on").siblings().removeClass("on");
                //设置字体
                c.eq(c.index(this)).css("font-weight", "bold").siblings().css("font-weight", "normal");
            })

            //取消修改密码
            $("#passcal").click(function () {
                $("#Dupdatepass").hide();
            })

            //修改电子邮箱
            $("#updatemail").click(function () {
                $("#Dudatemail").show();
            })

            $("#btmail").click(function () {
                var mial = $("#newmail").val();
                var comid = $("#hid_comid").trimVal();
                var id = '<%=AccountId %>';
                if (mial == null || mial == "") {
                    $("#mailspan").html("填写邮箱");
                    $("#mailspan").css("color", "red");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=updatemail",
                    data: { comid: comid, Id: id, mial: mial },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == 1) {
                                $.prompt("参数传递出错，请重新操作");
                                return;
                            } else if (data.msg == "OK") {
                                $.prompt("修改邮箱成功");
                                $("#Dudatemail").hide();
                                return
                            }
                        }
                        else {
                            $.prompt("修改邮箱错误");
                            return;
                        }
                    }
                });
            })

            //修改绑定手机号
            $("#updatephone").click(function () {
                $("#Dupdatephone").show();
            })
            $("#btphone").click(function () {
                var phone = $("#newphone").val();
                var comid = $("#hid_comid").trimVal();
                var id = '<%=AccountId %>';

                if (phone == null || phone == "") {
                    $("#phonespan").html("填写手机号");
                    $("#phonespan").css("color", "red");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=updatephone",
                    data: { comid: comid, Id: id, phone: phone },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == 1) {
                                $.prompt("参数传递出错，请重新操作");
                                return;
                            } else if (data.msg == "OK") {
                                $.prompt("修改手机成功");
                                $("#Dupdatephone").hide();
                                return
                            }
                        }
                        else {
                            $.prompt("修改手机错误");
                            return;
                        }
                    }
                });
            })

            $("#upweixin").click(function () {
                $("#showeixin").show();
            })
            $("#down").click(function () {
                $("#showeixin").hide();
            })


            //修改密码
            $("#updatepass").click(function () {
                $("#Dupdatepass").show();
            })
            $("#btpass").click(function () {
                var comid = $("#hid_comid").trimVal();
                var oldpass = $("#oldpass").val();
                var newpass = $("#newpass").val();
                var Tnewpas = $("#Tnewpass").val();
                var id = '<%=AccountId %>';
                var pas = '<%=AccountPass %>';
                if (oldpass == null || oldpass == "" || oldpass != pas) {
                    $("#oldspan").html("原密码错误");
                    $("#oldspan").css("color", "red");
                    return;
                }
                if (newpass == null || newpass == "") {
                    $("#newspan").html("输入密码错误");
                    $("#newspan").css("color", "red");
                    return;
                }
                if (Tnewpas == null || Tnewpas == "") {
                    $("#Tnewspan").html("确认输入密码错误");
                    $("#Tnewspan").css("color", "red");
                    return;
                }
                if (newpass != Tnewpas) {
                    $("#Tnewspan").html("两次输入不同，请从新输入");
                    $("#Tnewspan").css("color", "red");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=updatepass",
                    data: { comid: comid, Id: id, password1: Tnewpas },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == 1) {
                                $.prompt("参数传递出错，请重新操作");
                                return;
                            } else if (data.msg == "OK") {
                                $.prompt("修改成功");
                                $("#Dupdatepass").hide();
                                return
                            }
                        }
                        else {
                            $.prompt("修改密码错误");
                            return;
                        }
                    }
                });
            })

        })


        //打开页面后自动加载
        jQuery(document).ready(function () {
            $("div.bd").hide();
            $("#bd_0").show();
        })


        //领取活动
        function ClaimCoupon(aid) {
            var comid = $("#hid_comid").trimVal();
            $.ajax({
                type: "post",
                url: "/JsonFactory/CrmMemberHandler.ashx?oper=ClaimActlist",
                data: { comid: comid, aid: aid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("领取活动列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        location.reload();
                    }
                }
            });
        }

        //产品详情展示
        function tab_select(li, im) {
            if ($("#tab_sun_" + li + "_" + im + "").css("display") == "none") {
                $("#tab_sun_" + li + "_" + im + "").css('display', 'block');
                $("#tab_sun_" + li + "_" + im + "_biao").text(' ▲');
            } else {
                $("#tab_sun_" + li + "_" + im + "").css('display', 'none');
                $("#tab_sun_" + li + "_" + im + "_biao").text(' ▼');
            }
        }
    </script>
    <style type="text/css">
        input
        {
            border: 1px solid #5984bb;
        }
    </style>
    <script type="text/javascript">

        var pageSize = 10; //每页显示条数
        $(function () {

            var userid = $("#channelid").val();
            var comid = $("#comid").val();

            //  SearchListuser(1);

            //验票明细列表
            function SearchListuser(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/BusinessCustomersHandler.ashx?oper=Fuwupagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, user: userid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#tblist").html("查询会员数据出现错误。");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("没有查到会员信息。");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpageuser(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })


            }
            //分页
            function setpageuser(newcount, newpagesize, curpage) {
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

                        SearchListuser(page);

                        return false;
                    }
                });
            }
        })
    </script>
</head>
<body>
    <%--<div class="byts_top_1000">
        <div class="byts_top_nr_1000">
            <div class="logo">
                <a href="#">
                    <img src="i/logo.gif" alt="" /></a>
            </div>
            <div class="top1">
                嗨，欢迎来到北青旅总社！ <a href="#">请登录</a> <span><a href="#">免费注册</a></span> <span><a href="#">
                    青旅动态</a></span> <span><a href="#">北青品牌</a></span> <span class="shoucang"><a href="#">
                        收藏网站</a></span> <a href="#">网站导航</a>
            </div>
            <div class="mall_top_so_1000">
                <div class="mall_top_so1" onmouseout="this.className='mall_top_so2_1000'" onmouseover="this.className='mall_top_so2_1000'">
                    <label for="mq" style="visibility: visible;">
                        搜索 产品名称/品牌</label>
                    <input value="" type="text" maxlength="40" tabindex="9" id="mq" name="searchName">
                    <button type="submit" id="J_MallSearchBtn">
                        搜索<s></s></button>
                </div>
            </div>
        </div>
    </div>
    <div class="byts_menu_1000">
        <div class="byts_menu_1000_nr">
            <ul>
                <li><a target="_blank" href="http://www.byts.cn/">首页</a></li>
                <li><a target="_blank" href="http://www.byts.cn/byts_project.jsp">青旅成团</a></li>
                <li><a target="_blank" href="http://www.byts.cn/channel.jsp?aspect_type=0">国内旅游</a></li>
                <li><a target="_blank" href="http://www.byts.cn/channel.jsp?aspect_type=1">出境旅游</a></li>
                <li><a target="_blank" href="http://www.byts.cn/channel.jsp?aspect_type=2">周边旅游</a></li>
                <li><a target="_blank" href="http://www.byts.cn/ziyouxing/">自由行</a></li>
                <li><a target="_blank" href="http://www.byts.cn/zhuanti/youlun/">邮轮游</a></li>
                <li><a target="_blank" href="http://www.byts.cn/zhuanti/gongsi">公司旅游</a></li>
            </ul>
            <p class="fenbu">
                <a target="_blank" href="http://www.byts.cn/store_all_list.jsp">北青门市分布</a></p>
        </div>
    </div>--%>
    <div class="grid-780 grid-780-border fn-clear">
        <%-- <div style="text-align: right">
            <a href="/byts/Out.aspx" seed="topmenuItem-linkT1k" smartracker="on">退出</a>
        </div>--%>
        <div class="mod-bottom " style="padding-top: 3px">
            <div class="slideTxtBox">
                <div class="hd cl">
                    <ul class="tab">
                        <li class="on">会员专享区</li>
                        <%--<li class="">会员卡使用说明</li>
                        <li class="">商户介绍</li>--%>
                        <li class="">我的账户</li>
                        <li class="">特惠推荐</li>
                        <%if (channeltype == 1)
                          {
                              Response.Write("<li class=\"\">渠道返佣</li>");
                              Response.Write("<li class=\"\">服务会员查询</li>");
                          } %>
                        <%
                            if (mcMaterilList.Count > 0)
                            {
                                for (int i = 0; i < mcMaterilList.Count; i++)
                                {
                                    Response.Write("<li class=\"\">" + mcMaterilList[i].Title + "</li>");
                                }
                            }
                        %>
                        <% if (Servercard != 0)
                           { %>
                        <div align="right">
                            您的旅游顾问：<%= Servername %>
                            (
                            <%=Servermobile %>
                            )</div>
                        <%} %>
                    </ul>
                    <a href="/byts/Out.aspx?comid=<%=comid %>" seed="topmenuItem-linkT1k" style="float: right;"
                        smartracker="on">退出</a>
                </div>
                <div class="clearfix">
                </div>
                <div class="tempWrap" style="overflow: hidden; position: relative; width: 980px">
                    <div class="bd" id="bd_0" style="display: block;">
                        <div class="row mediumTopMargin">
                            <div class="leftCol">
                                <h1>
                                    尊敬的会员
                                    <%=AccountName%>
                                    您好！</h1>
                                <span style="font-size: 12px">当前为您准备了会员专享服务如下：<sup></sup>
                                    <br>
                                </span>
                            </div>
                        </div>
                        <ul id="ulreply" class="infoList bj" style="float: left; width: 860px;">
                            <li class="cl">
                                <div class="pro1">
                                    会员专享项目
                                </div>
                                <div class="pro2">
                                    优惠金额
                                </div>
                                <div class="pro3">
                                    活动截止到
                                </div>
                                <div class="pro4">
                                </div>
                                <div class="pro5">
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div class="bd" id="bd_1" style="display: none;">
                        <div class="row mediumTopMargin">
                            <div class="leftCol">
                                <h1>
                                    尊敬的会员
                                    <%=AccountName%>
                                </h1>
                                <span style="font-size: 12px">祝贺您成为 尊贵会员，请在这里管理和完善你的会员信息<sup></sup>
                                    <br />
                                </span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="leftCol mediumTopMargin" style="width: 100%">
                                <div class="hr gray">
                                    &nbsp;</div>
                                <table cellspacing="0" cellpadding="0" class="top20disp-table" data-taid="Top20PageTop20MainTable">
                                    <tbody>
                                        <tr class="top20disp-row row0; text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 150px;">
                                                <h2>
                                                    您的姓名：</h2>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <a href="#" id="Deals__ctl1_lnkDeal" style="font-size: 1.083em">
                                                    <%=AccountName %>
                                                </a>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <small></small>
                                            </td>
                                        </tr>
                                        <tr class="top20disp-row row0;text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 30%;">
                                                <h2>
                                                    您的会员卡号：</h2>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <a href="#" id="Deals__ctl2_lnkDeal" style="font-size: 1.083em">
                                                    <%=AccountCard %></a>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <small></small>
                                            </td>
                                        </tr>
                                        <tr class="top20disp-row row0;text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 30%;">
                                                <h2>
                                                    您的会员密码：</h2>
                                            </td>
                                            <td class="top20disp-col2 mediumPadding">
                                                <div id="Dupdatepassshow">
                                                    ******
                                                </div>
                                                <div id="Dupdatepass" style="background-color: #ffffff; border: 2px solid #5984bb;
                                                    margin: 0 atuo; display: none; left: 20%; position: absolute; top: 20%; display: none;
                                                    width: 360px; height: 160px; line-height: 25px;">
                                                    <div style="color: #fff; background: #5984bb; text-align: left; line-height: 25px;
                                                        width: 360px;">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;修改会员密码</div>
                                                    <div style="margin: 20px 0px 0 30px">
                                                        原密码 ：<input id="oldpass" type="text" /><span id="oldspan"></span><br />
                                                        新密码 ：<input id="newpass" type="text" /><span id="newspan"></span><br />
                                                        新密码 ：<input id="Tnewpass" type="text" /><span id="Tnewspan">（确认输入）</span><br />
                                                        <input id="btpass" style="margin-left: 60px; width: 90px; height: 25px; color: #5984bb"
                                                            type="button" value="提交密码" />
                                                        <input id="passcal" type="button" style="width: 90px; height: 25px; color: #5984bb"
                                                            value="取消" />
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="top20disp-col2 mediumPadding">
                                                <small><a href="#" id="updatepass" style="font-size: 1.083em">修改会员密码</a> </small>
                                            </td>
                                        </tr>
                                        <tr class="top20disp-row row0;text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 30%;">
                                                <h2>
                                                    微信关注：</h2>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <a href="#" id="A7" style="font-size: 1.083em">
                                                    <%=weixin%></a>
                                            </td>
                                            <td class="top20disp-col2 mediumPadding">
                                                <small>
                                                    <% if (weixin == "未关注")
                                                       {%>
                                                    <a href="#" id="upweixin" style="font-size: 1.083em">关注微信</a>
                                                    <%} %>
                                                </small>
                                            </td>
                                        </tr>
                                        <tr class="top20disp-row row0;text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 30%;">
                                                <h2>
                                                    手机号：</h2>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <a href="#" id="Deals__ctl3_lnkDeal" style="font-size: 1.083em">
                                                    <%=Accountphone %></a>
                                                <div id="Dupdatephone" style="background-color: #ffffff; border: 2px solid #5984bb;
                                                    margin: 0 atuo; display: none; left: 20%; position: absolute; top: 20%; display: none;
                                                    line-height: 25px;">
                                                    填写新绑定手机 ：<input id="newphone" type="text" /><span id="phonespan"></span>
                                                    <input id="btphone" style="margin-left: 60px; width: 90px; height: 25px; color: #5984bb"
                                                        type="button" value="修改绑定手机" />
                                                </div>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <small>
                                                    <%--<a href="#"  id="updatephone" style="font-size:1.083em">修改绑定手机号</a> --%>
                                                </small>
                                            </td>
                                        </tr>
                                        <tr class="top20disp-row row0;text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 30%;">
                                                <h2>
                                                    电子邮箱：</h2>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <a href="#" id="Deals__ctl4_lnkDeal" style="font-size: 1.083em;">
                                                    <%=AccountEmail %></a>
                                                <div id="Dudatemail" style="background-color: #ffffff; border: 2px solid #5984bb;
                                                    margin: 0 atuo; display: none; left: 20%; position: absolute; top: 20%; display: none;
                                                    line-height: 25px;">
                                                    填写电子邮箱 ：<input id="newmail" type="text" /><span id="mailspan"></span>
                                                    <input id="btmail" style="margin-left: 60px; width: 90px; height: 25px; color: #5984bb"
                                                        type="button" value="修改电子邮箱" />
                                                </div>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <small>
                                                    <%--<a href="#" id="updatemail" style="font-size:1.083em"> 修改电子邮箱 </a>--%>
                                                </small>
                                            </td>
                                        </tr>
                                        <tr class="top20disp-row row0;text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 30%;">
                                                <h2>
                                                    预付款：</h2>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <a href="#" id="Imprest" style="font-size: 1.083em">
                                                    <%=Imprest %></a>
                                            </td>
                                            <td class="top20disp-col2 mediumPadding">
                                                <small>
                                                    <%if (comid == 101 || comid == 106)
                                                      {%>
                                                    <a href="RechargeWeb.aspx" id="addRecharge" style="font-size: 1.083em">充值预付款 </a>
                                                    <%} %>
                                                </small>
                                            </td>
                                        </tr>
                                        <tr class="top20disp-row row0;text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 30%;">
                                                <h2>
                                                    积分：</h2>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <a href="#" id="A6" style="font-size: 1.083em">
                                                    <%=Integral %></a>
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <small></small>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <%--<div class="bd" id="bd_1" style="display: ;">
                        <div class="row mediumTopMargin">
                            <div class="leftCol">
                                <h1>
                                    微旅行会员卡使用说明:
                                </h1>
                                <span style="font-size: 12px">请仔细阅读下面信息，方便您持会员卡享受各项专享优惠活动
                                    <br>
                                </span>
                            </div>
                        </div>
                        <br>
                        <p>
                            1、 成为微旅行会员可享受会员专享优惠服务，本会员卡需开卡激活24小时后生效使用。<br />
                            2、 在指定旅行社门市咨询或报名时需提前出示本卡或电话中告知您的会员卡号,且不能与商户/门店其他优惠活动同时使用。<br />
                            3、 使用本卡参团报名时需按所选会员优惠活动中标明的限定内容享受优惠，且每笔订单只能享受一次会员优惠活动。<br />
                            4、 本卡是实名制会员卡，每卡只绑定一位专属会员。开卡后不能转让使用，如有丢失请联系微旅行客服补办。<br />
                            5、 本卡具有预存消费功能，会员账户中的预付款可累计使用但不能透支，不能提现，不退余额，不计利息。<br />
                            6、 请您妥善保管账户密码信息，如因自身原因导致会员账户信息泄漏而造成损失由会员自行承担。<br />
                            <br />
                            北京微旅程科技有限公司 微旅行会员服务部
                            <br />
                            2013-9-11
                            <br />
                        </p>
                        <br>
                    </div>
                    <div class="bd" id="bd_2" style="display: ;">
                        <div class="row mediumTopMargin">
                            <div class="leftCol">
                                <h1>
                                    微旅行会员卡优惠报名旅行社门店:
                                </h1>
                                <span style="font-size: 12px">持微旅行卡在下面门市部报名即可享受会员活动优惠
                                    <br>
                                </span>
                            </div>
                        </div>
                        <br>
                        <table cellspacing="0" cellpadding="0" class="top20disp-table" data-taid="Top20PageTop20MainTable">
                            <tbody>
                                <tr class="top20disp-row row0; text-align:left;">
                                    <td class="top20disp-col1 tzred mediumPadding" style="width: 40%;">
                                        <h2>
                                            北京青年旅行社国贸营业部</h2>
                                    </td>
                                    <td class="top20disp-col2 mediumPadding">
                                        <a href="" id="A1" style="font-size: 1.083em">北京市朝阳区建外大街12号5层516室（贵友对面双子座东边） »</a>
                                    </td>
                                </tr>
                                <tr class="top20disp-row row0;text-align:left;">
                                    <td class="top20disp-col1 tzred mediumPadding" style="width: 40%;">
                                        <h2>
                                            神舟国旅光明桥营业部</h2>
                                    </td>
                                    <td class="top20disp-col2 mediumPadding">
                                        <a href="" id="A2" style="font-size: 1.083em">北京朝阳区劲松三区甲302号华腾大厦703室 »</a>
                                    </td>
                                </tr>
                                <tr class="top20disp-row row0;text-align:left;">
                                    <td class="top20disp-col1 tzred mediumPadding" style="width: 40%;">
                                        <h2>
                                            北京中国国际旅行社北三环营业部</h2>
                                    </td>
                                    <td class="top20disp-col2 mediumPadding">
                                        <a href="" id="A3" style="font-size: 1.083em">北京市海淀区中关村南大街2号数码大厦A座717室 »</a>
                                    </td>
                                </tr>
                                <tr class="top20disp-row row0;text-align:left;">
                                    <td class="top20disp-col1 tzred mediumPadding" style="width: 40%;">
                                        <h2>
                                            北京中国国际旅行社呼家楼营业部</h2>
                                    </td>
                                    <td class="top20disp-col2 mediumPadding">
                                        <a href="" id="A4" style="font-size: 1.083em">北京市朝阳区呼家楼北街7号楼1层底商 »</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="row mediumTopMargin">
                            <div class="leftCol">
                                <br>
                                <h2>
                                    微旅行会员报名咨询: 4006-888-210
                                </h2>
                            </div>
                        </div>
                    </div>--%>
                    <%if (channeltype == 1)
                      { %>
                    <div class="bd" id="bd_3" style="display: none;">
                        <div class="row mediumTopMargin">
                            <div class="leftCol">
                                <h1>
                                    尊敬的会员
                                    <%=AccountName%>
                                </h1>
                                <span style="font-size: 12px">您的返佣信息<sup></sup>
                                    <br>
                                </span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="leftCol mediumTopMargin" style="width: 100%">
                                <div class="hr gray">
                                    &nbsp;</div>
                                <table cellspacing="0" cellpadding="0" class="top20disp-table" data-taid="Top20PageTop20MainTable">
                                    <tbody>
                                        <tr class="top20disp-row row0; text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 150px;">
                                                <h2>
                                                    开卡数量：</h2>
                                            </td>
                                            <td class="top20disp-col2 mediumPadding">
                                                <%=Opencardnum%>
                                                张 *
                                                <%=RebateOpen%>
                                                元
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <small></small>
                                            </td>
                                        </tr>
                                        <tr class="top20disp-row row0;text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 30%;">
                                                <h2>
                                                    消费数量：</h2>
                                            </td>
                                            <td class="top20disp-col2 mediumPadding">
                                                <%=Firstdealnum%>
                                                数 *
                                                <%=RebateConsume%>
                                                元
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <small></small>
                                            </td>
                                        </tr>
                                        <tr class="top20disp-row row0;text-align:left;">
                                            <td class="top20disp-col1 tzred mediumPadding" style="width: 30%;">
                                                <h2>
                                                    合 计：</h2>
                                            </td>
                                            <td class="top20disp-col2 mediumPadding">
                                                <%=Summoney%>
                                                元
                                            </td>
                                            <td class="top20disp-col3 mediumPadding">
                                                <small></small>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="bd" id="bd_4" style="display: none;">
                        <div id="setting-home" class="vis-zone mail-list">
                            <div class="inner">
                                <h3>
                                    会员 列表</h3>
                                <div style="text-align: center;">
                                    <%--<label>
                        请输入(姓名，手机，卡号，邮箱)
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询会员" style="width: 120px;
                            height: 26px;">
                    </label>--%>
                                </div>
                                <table width="950" border="0">
                                    <tr>
                                        <td width="16%">
                                            卡号
                                        </td>
                                        <td width="10%">
                                            用户姓名
                                        </td>
                                        <td width="8%">
                                            手机
                                        </td>
                                        <td width="8%">
                                            微信情况
                                        </td>
                                        <td width="12%">
                                            邮箱
                                        </td>
                                        <td width="6%">
                                            预付款
                                        </td>
                                        <td width="6%">
                                            积分
                                        </td>
                                        <td width="14%">
                                            注册时间&nbsp;
                                        </td>
                                    </tr>
                                    <tbody id="tblist">
                                    </tbody>
                                    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td>
                            ${idcard} 
                        </td>
                        <td>
                                ${customername}
                        </td>
                        <td>
                            ${phone} 
                        </td>
                          <td>
                            ${winxin} 
                        </td>
                        <td>
                            ${email} 
                        </td>
                        <td >
                                ${imprest}
                        </td>
                        <td >
                               ${integral}
                        </td>
                        <td >

                                ${jsonDateFormatKaler(registerdate)}
                            
                        </td>
                    </tr>
                                    </script>
                                </table>
                                <div id="divPage">
                                </div>
                            </div>
                        </div>
                        <div class="data">
                        </div>
                    </div>
                    <%} %>
                    <div class="bd" id="bd_2" style="display: ;">
                        <div class="main">
                            <div class="leftCol rightBodyColumn">
                                <div class="row mediumTopMargin">
                                    <div class="leftCol">
                                        <h1>
                                            精选推荐</h1>
                                        <span style="font-size: 12px">发布日期<%=Listtime%>
                                            <sup></sup>
                                            <br>
                                        </span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="leftCol mediumTopMargin" style="width: 100%">
                                        <div class="hr gray">
                                            &nbsp;</div>
                                        <%--<table cellspacing="0" cellpadding="0" class="top20disp-table" data-taid="Top20PageTop20MainTable">
                            
                                <tbody>
                                 <asp:Repeater ID="Rplist" runat="server">
                                    <ItemTemplate>
                                       <tr class="top20disp-row row0; text-align:left;">
                                    <td class="top20disp-col1 tzred mediumPadding" style="width:20%;">
                                        <h2>￥<%#Eval("Price")%>起</h2>
                                    </td>
                                    <td class="top20disp-col2" width="500px">
                                        <a href="info.aspx?id=<%#Eval("MaterialId") %>" target="_blank" id="Deals__ctl1_lnkDeal" style="font-size:1.083em"><%#Eval("Title")%></a>
                                    </td>
                                </tr>
                                    </ItemTemplate>
                                 </asp:Repeater>
                            </tbody>
                               </table>--%>
                                    </div>
                                </div>
                                <div>
                                    <asp:Repeater ID="menu" runat="server" OnItemDataBound="menu_ItemDataBound">
                                        <ItemTemplate>
                                            <dl>
                                                <dt>
                                                    <h3>
                                                        <%#this.strsub(Eval("typename").ToString())%>
                                                        &nbsp;&nbsp;&nbsp;<%# this.pernum(int.Parse(Eval("id").ToString()), comid)%></h3>
                                                </dt>
                                                <dd>
                                                    <table cellspacing="0" cellpadding="0" class="top20disp-table" data-taid="Top20PageTop20MainTable">
                                                        <asp:Repeater ID="Rplist" runat="server">
                                                            <ItemTemplate>
                                                                <tr class="top20disp-row row0; text-align:left;">
                                                                    <td class="top20disp-col1 tzred mediumPadding" style="width: 20%;">
                                                                        <h2>
                                                                            ￥<%#Eval("Price")%>起</h2>
                                                                    </td>
                                                                    <td class="top20disp-col2" width="500px">
                                                                        <a href="info.aspx?id=<%#Eval("MaterialId") %>" target="_blank" id="Deals__ctl1_lnkDeal"
                                                                            style="font-size: 1.083em">
                                                                            <%#Eval("Title")%></a>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </dd>
                                            </dl>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblEmpty" Text="无推荐线路" runat="server" Visible='<%#bool.Parse((menu.Items.Count==0).ToString())%>'>     
     
                                            </asp:Label>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%
                        if (mcMaterilList.Count > 0)
                        {
                            for (int i = 0; i < mcMaterilList.Count; i++)
                            {
                                var mcid = 3 + i;
                                if (channeltype == 1)
                                {
                                    mcid = 5 + i;
                                }
                    %>
                    <div class="bd" id="bd_<%=mcid %>" style="display: ;">
                        <div class="row mediumTopMargin">
                            <div class="leftCol">
                                <h1>
                                    <%=mcMaterilList[i].Title %>
                                </h1>
                                <span style="font-size: 12px">
                                    <%=mcMaterilList[i].Summary %>
                                    <br>
                                </span>
                            </div>
                        </div>
                        <br>
                        <p>
                            <%=mcMaterilList[i].Article %>
                        </p>
                        <br>
                    </div>
                    <%
                        }
                        }
                    %>
                    <%-- <div class="ui-form-item">
	        <div id="submitBtn" class="ui-button  ui-button-morange "></div>
	       </div>--%>
                </div>
            </div>
        </div>
    </div>
    <input id="channelid" type="hidden" value="<%=channelid %>" />
    <input id="comid" type="hidden" value="<%=comid %>" />
    <div id="showeixin" style="display: none; position: absolute; top: 270px; left: 150px;
        z-index: 999; background: #FFF;">
        <a id="down" href="#">关闭</a>
        <p>
            <img src="/UploadFile/2013100014210957773.gif" /></p>
        <p>
            扫描关注【微旅行】，享受更多优惠</p>
    </div>
    <%-- <div class="foot_1000">
        <div class="foot_1000_nr">
            <div class="txtnr">
                <h1>
                    旅游常见问题</h1>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=1">青旅独立成团产品的优势</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=2">纯玩是什么意思？</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=3">单房差是什么意思？</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=4">双飞、双卧都是什么意思？</a></p>
            </div>
            <div class="txtnr">
                <h1>
                    付款和发票</h1>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=5 ">签约可以刷卡吗？</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=6 ">付款方式有哪些？</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=7 ">怎么网上支付？</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=8 ">如何获取发票？</a></p>
            </div>
            <div class="txtnr">
                <h1>
                    签署旅游合同</h1>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=9">有旅游合同范本下载吗？</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=10">门市地址在哪里？</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=11">能传真签合同吗？</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=12">可以不签合同吗？</a></p>
            </div>
            <div class="txtnr">
                <h1>
                    会员功能</h1>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=13">会员独享权利</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=14">如何成为会员</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=15">会员积分兑换</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=16">会员如何获取积分</a></p>
            </div>
            <div style="border-right: 0;" class="txtnr">
                <h1>
                    旅游其他事项</h1>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=17">签证相关问题解答</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=18">旅游保险相关问题</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=19">退款问题</a></p>
                <p>
                    <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=20">旅途中紧急状况处理</a></p>
            </div>
        </div>
    </div>
    <div class="foot_1000_a">
        <p>
            <a target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=25">关于青旅</a><span>|</span><a
                target="_blank" href="http://www.byts.cn/article_show.jsp?article_act_id=26">青旅招聘</a><span>|</span><a
                    target="_blank" href="http://nt3.uedoc.com/T3_work/login.jsp">进入T3系统</a><span>|</span><a
                        target="_blank" href="http://www.byts.cn/store_all_list.jsp">营业网点分布</a></p>
        <p>
            中国旅游协会理事单位<span>|</span>北京市旅游协会理事单位<span>|</span>中国国家旅游局特许经营中国公民出境旅游组团社</p>
        <p>
            版权所有 &copy; 1997-2013 北京青年旅行社股份有限公司总社 www.byts.cn 经营许可证 L-BJ-GJ00060 京ICP证041363号
            声明：本站内容未经许可不得转载!</p>
        <div class="foot_1000_a_img">
            <img alt="" src="http://www.byts.cn/i/index_foot.gif">
        </div>
    </div>--%>
