<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ETS2.WebApp.byts.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="robots" content="all" />
    <title>登录 - 北京青年旅行社股份有限公司 - 总社 BYTS.cn</title>
    <meta name="description" content="介绍" />
    <meta name="keywords" content="关键词" />
    <link href="style/byts2013.css" rel="stylesheet" type="text/css" />
    <link rel="icon" href="/favicon.ico" type="image/x-icon" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/Impromptu.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid=<%=comid %>;
            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#btn-submit").click();    //这里添加要处理的逻辑  
                }
            });

            $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })

            $("body").keydown(function (e) {
                if (e.keyCode == 13) {
                    $(".btn_login").click();
                }
            });


            //判断密码
            $("#Account").blur(function () {
                $("#AccountVer").html(""); //离开后先清空备注
                var Account = $("#Account").val();
                if (Account == "") {
                    $("#error-box").html("请填写账户");
                    $("#error-box").css("color", "red");
                    return;
                }
            })

            //判断密码
            $("#Pwd").blur(function () {
                $("#PwdVer").html(""); //离开后先清空备注
                var Pwd = $("#Pwd").val();
                if (Pwd == "") {
                    $("#error-box").html("填写密码");
                    $("#error-box").css("color", "red");
                    return;
                } 
            })

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#btn-submit").click();    //这里添加要处理的逻辑  
                }
            });


            //提交按钮
            $("#btn-submit").click("click", function () {
            
                var Account = $("#Account").val();
                var Pwd = $("#Pwd").val();
                var getcode = $.trim($("#getcode").val());
                var come =$("#hid_come").val();

                if (Account == "") {
                    $("#error-box").html("请填写账户");
                    $("#error-box").show();
                    return;
                }

                if (Pwd == "") {
                    $("#error-box").html("填写密码");
                    $("#error-box").show();
                    return;
                } 
                 if (getcode == '') {
                    $("#error-box").html("验证码不可为空");
                    $("#error-box").show();

                    return;
                }
                $(".loading-text").html("正在登陆信息，请稍后...")

                //登陆
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Login2", { comid:comid, Account: Account, Pwd: Pwd,getcode:getcode }, function (data) {
                    if (data == "OK") {
//                       if(come=="")
//                       {
                         window.open("/byts/Default.aspx", target = "_self"); 
//                       }
//                       else
//                       {
//                          window.open(come, target = "_parent"); 
//                       }
                    } else {
                        $("#error-box").html(data);
                        $("#error-box").show();
                        return;
                    }


                })

            })

        })
        function refreshimg(){
         $("#validateCode").attr("src",'/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime());
//        this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
        }
    </script>
    <style type="text/css">
        .error-box
        {
            background: none no-repeat scroll -381px 5px #FFFFD0;
            line-height: 1.3;
            margin: 10px auto 0;
            padding: 5px 5px 5px 25px;
            width: 202px;
        }
        .error-box
        {
            background: url("#") no-repeat scroll -381px 8px #FFFFD0;
            border: 1px solid #FFAD77;
            color: #FF0000;
            line-height: 1.6;
            margin: 0 auto 10px;
            padding: 5px 10px 5px 25px;
            width: 200px;
        }
    </style>
</head>
<body>
    <%--<div class="user_top">
        <div class="nr">
            <img src="i/login_logo.gif" alt="" />
        </div>
    </div>--%>
    <div class="user_login">
        <div class="user_login_nr">
            <h1 class="yh">
                北青会员登录</h1>
            <div class="error-box " id="error-box" style="display: none;">
            </div>
            <div class="user_name">
                <input type="text" class="inp1" maxlength="100" id="Account" name="Account">
                <div class="txt" onmouseover="this.className='txtnone'">
                    邮箱/手机号/用户名
                </div>
            </div>
            <div class="user_name">
                <input type="password" class="inp1" maxlength="100" id="Pwd" name="Pwd">
                <div class="txt" onmouseover="this.className='txtnone'">
                    请输入密码
                </div>
            </div>
            <div class="user_name" style="height: 55px;">
                <input type="text" class="inp2" maxlength="100" id="getcode">
                <p class="p2">
                    <img id="validateCode" src="/account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码"
                        onclick="refreshimg()" /></p>
                <p class="p2">
                    <a href="#" onclick="refreshimg()">换一张</a></p>
            </div>
            <div class="user_name" style="height: 40px;">
                <%--<input type="checkbox" class="inp3">
              <p class="p3">
                    下次自动登录</p>--%>
            </div>
            <div class="user_name1">
                <a href="#" id="btn-submit">
                    <img src="i/login_btn.jpg" alt="登录" /></a>
                <%--<p class="p2">
                    <a href="#">忘记密码？</a></p>--%>
            </div>
            <div class="user_name2">
                还不是北青会员？ <a href="/byts/register.aspx">免费注册</a>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_come" value="<%=comeurl %>" />
    <%-- <div class="foot_1000">
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
