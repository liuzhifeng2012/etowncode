<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterSuc.aspx.cs" Inherits="ETS2.WebApp.Account.RegisterSuc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>易城商户平台注册</title>
    <link charset="utf-8" rel="stylesheet" href="/Styles/style-4.css">
    <link charset="utf-8" rel="stylesheet" href="/Styles/reg.css">
    <script type="text/javascript" src="../Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-impromptu.4.0.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../Scripts/Impromptu.css" />
    <script type="text/javascript" src="../Scripts/common.js"></script>
    <style type="text/css">
        #banner
        {
            position: relative;
            margin: 0 auto;
            min-width: 990px;
            overflow: hidden;
            width: 100%;
            height: 500px;
            z-index: 100;
            border-top: 4px solid #ff9900;
            background-color: #f9f9f9;
        }
        .ui-form-title strong
        {
            float: left;
            margin-left: 80px;
            text-align: right;
            width: 330px;
            font-size: 16px;
        }
    </style>
</head>
<body>
     <div class="topbar">
        <div class="grid-990 topbar-wrap fn-clear">
            <ul class="topmenu">
             <a href="/">返回首页</a>
            </ul>
        </div>
    </div>
    <div class="header" coor="header">
        <div class="grid-780 grid-780-pd fn-hidden fn-clear">
            <div id="et-img-logo1" class="fn-left">
              <a href="http://vctrip.etown.cn/business.html"> <img id="imglogo" height="75" alt="" src="/manage/images/v-logo-1.jpg"></img></a>
            </div>
        </div>
    </div>
    <div id="banner" class="slide-1" coor-rate="0.1" coor="default-banner" role="banner"
        data-banner="false">
        <div class="grid-780 grid-780-pd fn-hidden fn-clear">
            <div class="navi-container">
                <ol class="ui-step    ui-step-4 ">
                    <li>
                        <div class="ui-step-line">
                            -</div>
                        <div class="ui-step-icon">
                            <i class="ui-step-number">第一步： </i><span class="ui-step-text">提交注册信息</span>
                        </div>
                    </li>
                    <li class="ui-step-start ui-step-done ui-step-active">
                        <div class="ui-step-line">
                            -</div>
                        <div class="ui-step-icon">
                            <i class="ui-step-number">第二步：</i> <span class="ui-step-text">恭喜成功注册</span>
                        </div>
                    </li>
                </ol>
            </div>
        </div>
        <form name="Regi" method="post" action="" target="_parent">
        <div class="grid-780 grid-780-border fn-clear">
            <p class="ui-tiptext">
                审核期一般为2个工作日，开通成功通知发送到您预留电子邮箱,请注意接收，谢谢！
            </p>
            <div class="ui-form-dashed">
            </div>
            <h3 class="ui-form-title">
                <strong>恭喜你注册成功，请等待管理员确认通知</strong>
            </h3>
            <div class="ui-form-dashed">
            </div>
        </div>
        </form>
    </div>
    <div class="ui-form-dashed">
    </div>
        <div class="footer" coor="footer">
        <div class="grid-780 sitelink fn-clear" coor="sitelink" role="contentinfo">
            <ul class="ui-link fn-clear">
                <li class="ui-link-item">我们专注为中国旅游同业提供电子化营销整合方案。
           包括景区、酒店、旅行社的电子预订与验证、微信营销、分销渠道管理。 </li>

                <li class="ui-link-item">Copyright Vctrip@2014. All Right Reserved    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;北京微旅程科技有限公司</li>
                <li class="ui-link-item">北京市朝阳区劲松华腾大厦4层401   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;4006361919 010-59022350</li></ul>
        </div>
    </div>
</body>
</html>
