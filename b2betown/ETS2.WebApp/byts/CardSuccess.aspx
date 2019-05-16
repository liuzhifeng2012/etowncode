<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardSuccess.aspx.cs" Inherits="ETS2.WebApp.byts.CardSuccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="robots" content="all" />
    <title>注册新用户 - 北京青年旅行社股份有限公司 - 总社 BYTS.cn</title>
    <meta name="description" content="介绍" />
    <meta name="keywords" content="关键词" />
    <link href="style/byts2013.css" rel="stylesheet" type="text/css" />
    <link rel="icon" href="/favicon.ico" type="image/x-icon" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/Impromptu.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <style type="text/css">
        .user_reg .lnr .bdnr .p1
        {
            float: left;
            font-size: 14px;
            padding-right: 5px;
            text-align: left;
            width: 500px;
        }
    </style>
</head>
<body>
    <%--  <div class="user_top">
        <div class="nr">
            <img src="i/reg_logo.gif" alt="" />
        </div>
    </div>--%>
    <div class="user_reg">
        <div class="lnr">
            <h1 class="yh">
                注册成功</h1>
            <div class="bdnr">
                <p class="p1">
                    持卡人信息：</p>
            </div>
            <div class="bdnr">
                <p class="p1">
                    您的姓名
                    <%=Name_temp%>
                </p>
            </div>
            <div class="bdnr">
                <p class="p1">
                    手机号
                    <%=Phone%></p>
            </div>
            <div class="bdnr">
                <p class="p1">
                    电子邮箱
                    <%=Email%></p>
            </div>
            <div class="bdnr">
                <p class="p1">
                    祝贺您卡开成功！您是我们尊贵的会员，请 <b><a href="login.aspx" style="color: #FF6600">登 录 </a></b>查询更多会员专享服务</p>
            </div>
            <div class="bdnr">
                <p class="p1">
                </p>
            </div>
        </div>
        <div class="rnr">
            <div class="txtreg">
                <p>
                    已经有北青账号？ 您可以请直接</p>
                <p>
                    <a href="#">
                        <img src="i/reg_btn_txt.jpg" alt="登录" /></a></p>
            </div>
            <img src="i/reg_img.jpg" alt="" />
        </div>
    </div>
    <%--<div class="foot_1000">
        <div class="foot_1000_nr">
            <div class="txtnr">
                <h1>
                    旅游常见问题</h1>
                <p>
                    <a href="http://www.byts.cn/news/2011-05-31/1216524959.htm" target="_blank">青旅独立成团的优势</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-07/1943377638.htm" target="_blank">纯玩是什么意思？</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-07/1943377638.htm" target="_blank">单房差是什么意思？</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-07/1943377638.htm" target="_blank">双飞、双卧都是什么意思？</a></p>
            </div>
            <div class="txtnr">
                <h1>
                    付款和发票</h1>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-07/1060948981.htm" target="_blank">签约可以刷卡吗？</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-07/1060948981.htm" target="_blank">付款方式有哪些？</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-07/1060948981.htm" target="_blank">怎么网上支付？</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-07/1060948981.htm" target="_blank">如何获取发票？</a></p>
            </div>
            <div class="txtnr">
                <h1>
                    签署旅游合同</h1>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/136269755.htm" target="_blank">有旅游合同范本下载吗？</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/136269755.htm" target="_blank">门市地址在哪里？</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/136269755.htm" target="_blank">能传真签合同吗？</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/136269755.htm" target="_blank">可以不签合同吗？</a></p>
            </div>
            <div class="txtnr">
                <h1>
                    会员功能</h1>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/1577521790.htm">会员独享功能</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/1577521790.htm">如何成为会员</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/1577521790.htm">会员忘记密码怎么办</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/1577521790.htm">非会员可以预定产品吗？</a></p>
            </div>
            <div class="txtnr" style="border-right: 0;">
                <h1>
                    旅游其他事项</h1>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/915221303.htm" target="_blank">签证相关问题解答</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/149137005.htm" target="_blank">旅游保险问题解答</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/382950416.htm" target="_blank">退款问题解答</a></p>
                <p>
                    <a href="http://www.byts.cn/news/2011-11-08/2071078637.htm" target="_blank">旅途中的问题</a></p>
            </div>
        </div>
    </div>
    <div class="foot_1000_a">
        <p>
            <a href="#">关于青旅</a><span>|</span><a href="#">青旅招聘</a><span>|</span><a href="#">进入T3系统</a><span>|</span><a
                href="#">营业网点分布</a></p>
        <p>
            中国旅游协会理事单位<span>|</span>北京市旅游协会理事单位<span>|</span>中国国家旅游局特许经营中国公民出境旅游组团社</p>
        <p>
            版权所有 © 1997-2013 北京青年旅行社股份有限公司总社 www.byts.cn 经营许可证 L-BJ-GJ00060 京ICP证041363号 声明：本站内容未经许可不得转载!</p>
        <div class="foot_1000_a_img">
            <img src="i/index_foot.gif" alt="" />
        </div>
    </div>--%>
</body>
</html>
