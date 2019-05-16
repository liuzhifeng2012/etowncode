<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="ETS2.WebApp.byts.register" %>

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
    <script type="text/javascript">
        $(function () {
            var comid =<%=comid %>;

            //账号名
            $("#Email").blur(function () {
                $("#EmailVer").html(""); //离开后先清空备注
                var Email = $("#Email").val();
                //判断邮箱不为空
                if (Email != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getEmail", { Email: Email, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#EmailVer").html("√");
                                $("#EmailVer").css("color", "green");
                                $("#VEmail").val(1);
                            } else {
                                $("#EmailVer").html(data.msg);
                                $("#EmailVer").css("color", "red");
                                $("#VEmail").val(0);
                                return;
                            }

                        }
                    })
                }
            })

            //判断密码
            $("#QPassword1").blur(function () {
                $("#QPassword1Ver").html(""); //离开后先清空备注
                var QPassword1 = $("#QPassword1").val();
                var Password1 = $("#Password1").val();
                //Phone
                if (QPassword1 == "" || QPassword1 != Password1) {
                    $("#QPassword1Ver").html("再次填写密码错误");
                    $("#QPassword1Ver").css("color", "red");
                    return false;
                } else {
                    $("#VQPassword1").val(1);
                }
            })

            //判断手机
            $("#Phone").blur(function () {
                $("#PhoneVer").html(""); //离开后先清空备注
                var Phone = $("#Phone").val();
                //Phone
                if (Phone != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getPhone", { Phone: Phone, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#PhoneVer").html("√");
                                $("#PhoneVer").css("color", "green");
                                $("#VPhone").val(1);
                            } else {
                                $("#PhoneVer").html(data.msg);
                                $("#PhoneVer").css("color", "red");
                                $("#VPhone").val(0);
                                return;
                            }

                        }
                    })
                }
            })


            //提交按钮
            $("#btn-submit").click(function () {
                var Email = $("#Email").val();
                var Password1 = $("#Password1").val();
                var QPassword1 = $("#QPassword1").val();
                var Name = $("#Name").val();
                var Phone = $("#Phone").val();
                //var Sex = $('input:radio[name="Sex"]:checked').val();
                var Sex = "";

                if (Email == "") {
                    $("#EmailVer").html("请填电子邮件");
                    $("#EmailVer").css("color", "red");
                    return;
                }
                if ($("#VEmail").val() == 0) {
                    $("#EmailVer").html("电子邮箱有误");
                    $("#EmailVer").css("color", "red");
                    return;
                };
                if (Password1 == "") {
                    $("#Password1Ver").html("请填写密码");
                    $("#Password1Ver").css("color", "red");
                    return;
                }
                if (QPassword1 == "") {
                    $("#QPassword1Ver").html("再次填写密码错误");
                    $("#QPassword1Ver").css("color", "red");
                    return;
                }
                if ($("#VQPassword1").val() == 0) {
                    $("#QPassword1Ver").html("密码有误");
                    $("#QPassword1Ver").css("color", "red");
                    return;
                };

                if (Name == "") {
                    $("#NameVer").html("请填写姓名");
                    $("#NameVer").css("color", "red");
                    return;
                }

                if (Phone == "") {
                    $("#PhoneVer").html("请填写手机");
                    $("#PhoneVer").css("color", "red");
                    return;
                }
                if ($("#VPhone").val() == 0) {
                    $("#PhoneVer").html("手机号码有误");
                    $("#PhoneVer").css("color", "red");
                    return;
                };


                $("#loading").html("正在提交开卡信息，请稍后...")

                //创建订单
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Useregcard", { comid: comid, Email: Email, Password1: Password1, Name: Name, Phone: Phone, Sex: Sex }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("注册出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            location.href = "CardSuccess.aspx?Name=" + Name + "&Email=" + Email + "&Phone=" + Phone
                            return;
                        }
                        else {
                            $.prompt("参数传递出错，请重新操作");
                            return;
                        }
                    }
                })

            })
        })
    </script>
</head>
<body>
   <%-- <div class="user_top">
        <div class="nr">
            <img src="i/reg_logo.gif" alt="" />
        </div>
    </div>--%>
    <div class="user_reg">
        <div class="lnr">
            <h1 class="yh">
                注册新会员</h1>
            <div class="bdnr">
                <p class="p1">
                    邮箱：</p>
                <input type="text" class="inp1" maxlength="100" id="Email" name="Email">
                <p class="p2" id="EmailVer">
                </p>
            </div>
            <div class="bdnr">
                <p class="p1">
                    密码：</p>
                <input type="password" class="inp1" maxlength="100" id="Password1" name="Password1">
                <p class="p2" id="Password1Ver">
                </p>
            </div>
            <div class="bdnr">
                <p class="p1">
                    确认密码：</p>
                <input type="password" class="inp1" maxlength="100" id="QPassword1" name="QPassword1">
                <p class="p2" id="QPassword1Ver">
                </p>
            </div>
            <div class="bdnr">
                <p class="p1">
                    用户姓名：</p>
                <input type="text" class="inp1" maxlength="100" id="Name" name="Name">
                <p class="p2" id="NameVer">
                </p>
            </div>
            <div class="bdnr">
                <p class="p1">
                    手机号：</p>
                <input type="text" class="inp1" maxlength="100" id="Phone" name="Phone">
                <p class="p2" id="PhoneVer">
                </p>
            </div>
            <%--<div class="bdnr">
                <p class="p1">
                    验证码：</p>
                <input type="text" class="inp2" maxlength="100" id="" name="">
                <p class="p2">
                    <img src="i/reg_yzm.gif" alt="" /></p>
                <p class="p2">
                    <a href="#">换一张</a></p>
            </div>
            <div class="bdnr">
                <p class="p1">
                    &nbsp;</p>
                <input type="checkbox" class="inp3">
                <p class="p3">
                    我已经阅读并同意遵守<a href="#">《北青旅用户协议》</a></p>
            </div>--%>
            <div class="bdnr" style="height: 100px;">
                <p class="p1">
                    &nbsp;</p>
                <p class="p4">
                    <a href="#" id="btn-submit">
                        <img src="i/reg_btn.gif" alt="注册" /></a></p>
                <span id="loading"></span>
            </div>
        </div>
        <div class="rnr">
            <div class="txtreg">
                <p>
                    已经有北青账号？ 您可以请直接</p>
                <p>
                    <a href="/byts/login.aspx">
                        <img src="i/reg_btn_txt.jpg" alt="登录" /></a></p>
            </div>
            <img src="i/reg_img.jpg" alt="" />
        </div>
    </div>
    <input type="hidden" name="VEmail" id="VEmail" value="0" />
    <input type="hidden" name="VQPassword1" id="VQPassword1" value="0" />
    <input type="hidden" name="VPhone" id="VPhone" value="0">
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
