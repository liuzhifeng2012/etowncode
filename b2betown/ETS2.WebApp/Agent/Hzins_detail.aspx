<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hzins_detail.aspx.cs" Inherits="ETS2.WebApp.Agent.Hzins_detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <meta http-equiv="x-ua-compatible" content="IE=edge">
    <title>分销后台</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.post("/JsonFactory/OrderHandler.ashx?oper=Hzins_detail", { orderid: $("#hid_orderid").val() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.msg != null && data.msg != "") {
                        if (data.msg.data != null && data.msg.data != "") {
                            $("#bx_title").find("b").text(data.msg.data.prodName);
                            if (data.msg.data.policyDetailInfo != null && data.msg.data.policyDetailInfo != "") {//保单明细
                                $("#bx_policyDetailInfo").find("li").eq(0).append(data.msg.data.policyDetailInfo.totalNum);
                                $("#bx_policyDetailInfo").find("li").eq(1).append(data.msg.data.policyDetailInfo.buySinglePrice);
                                $("#bx_policyDetailInfo").find("li").eq(2).append(data.msg.data.policyDetailInfo.settlementPrice);
                                $("#bx_policyDetailInfo").find("li").eq(3).append(data.msg.data.policyDetailInfo.deallineText);
                                $("#bx_policyDetailInfo").find("li").eq(4).append(data.msg.data.insureNum);
                            }
                            if (data.msg.data.applicantInfo != null && data.msg.data.applicantInfo != "") { //投保人信息
                                $("#bx_applicantInfo").find("li").eq(0).append(data.msg.data.applicantInfo.cName);
                                $("#bx_applicantInfo").find("li").eq(1).append(data.msg.data.applicantInfo.eName);
                                $("#bx_applicantInfo").find("li").eq(2).append("身份证");
                                $("#bx_applicantInfo").find("li").eq(3).append(data.msg.data.applicantInfo.cardCode);
                                $("#bx_applicantInfo").find("li").eq(4).append(data.msg.data.applicantInfo.birthday);
                                var toubaosex = "男";
                                if (data.msg.data.applicantInfo.sex == 0) {
                                    toubaosex = "女";
                                }
                                $("#bx_applicantInfo").find("li").eq(5).append(toubaosex);
                                $("#bx_applicantInfo").find("li").eq(6).append(data.msg.data.applicantInfo.mobile);
                                $("#bx_applicantInfo").find("li").eq(7).append(data.msg.data.applicantInfo.email);

                            }
                            if (data.msg.data.insurantInfos != null && data.msg.data.insurantInfos != "") {//被保人列表
                                var istr = '';
                                for (var i = 1; i <= data.msg.data.insurantInfos.length; i++) {
                                    var relationId = "其他";
                                    if (data.msg.data.insurantInfos[i - 1].relationId == 1) {
                                        relationId = "本人";
                                    }
                                    var sex = "女";
                                    if (data.msg.data.insurantInfos[i - 1].sex == 1) {
                                        sex = "男";
                                    }

                                    istr += '<div class="sf_preview mart10">' +
                                                    '<p>' +
                                                        '第' + i + '被保险人</p>' +
                                                    '<ul>' +
                                                        '<li><span>与被保险人关系：</span>' + relationId + ' </li>' +
                                                        '<li><span>被保险人姓名：</span>' + data.msg.data.insurantInfos[i - 1].cName + ' </li>' +
                                                        '<li><span>证件类型：</span>身份证 </li>' +
                                                        '<li><span>证件号码：</span>' + data.msg.data.insurantInfos[i - 1].cardCode + ' </li>' +
                                                        '<li><span>出生日期：</span>' + data.msg.data.insurantInfos[i - 1].birthday + '</li>' +
                                                        '<li><span>性别：</span>' + sex + ' </li>' +
                                                        '<li><span>手机号码：</span>' + data.msg.data.insurantInfos[i - 1].mobile + ' </li>' +
                                                        '<li><span>购买份数：</span>' + data.msg.data.insurantInfos[i - 1].count + '</li>' +
                                                    '</ul>' +
                                                '</div>' +
                                                '<div class="clear">' +
                                                '</div>';
                                }
                                $("#bx_InsurantInfo").html(istr);
                            }
                        }
                    }
                }
            })
        })
    </script>
    <style type="text/css">
        ul, dl, ol
        {
            list-style: none;
            margin: 0;
            padding: 0;
            display: block;
            list-style-type: disc;
            -webkit-margin-before: 1em;
            -webkit-margin-after: 1em;
            -webkit-margin-start: 0px;
            -webkit-margin-end: 0px;
            -webkit-padding-start: 0;
        }
        
        #page, #header, #content, #footer
        {
            margin: 0 auto;
            zoom: 1;
        }
        .w950
        {
            width: 950px;
        }
        .float_l
        {
            float: left;
        }
        .sf_reaervationt
        {
            width: 950px;
            height: 45px;
            text-align: center;
        }
        .mart15
        {
            margin-top: 15px;
        }
        .sf_reaervationt b
        {
            padding-right: 15px;
            line-height: 35px;
            font-size: 20px;
            font-weight: 500;
            color: #000;
            font-family: \5FAE\8F6F\96C5\9ED1,\9ED1\4F53;
        }
        .sf_preview
        {
            float: left;
            width: 950px;
        }
        .mart10
        {
            margin-top: 10px;
        }
        
        .sf_preview p
        {
            float: left;
            width: 950px;
            line-height: 32px;
            font-size: 12px;
            font-weight: bold;
            color: #333;
            text-indent: 5px;
        }
        
        .sf_preview ul
        {
            float: left;
            width: 950px;
            border-top: 1px solid #d8d8d8;
        }
        .sf_preview ul li
        {
            float: left;
            width: 950px;
            line-height: 30px;
            font-size: 12px;
            color: #555;
            border-bottom: 1px solid #e7e7e7;
            text-indent: 15px;
            list-style-type: none;
        }
        .sf_preview ul li span
        {
            float: left;
            width: 220px;
            padding-right: 5px;
            background: #f7f7f7;
            color: #333;
            text-align: right;
        }
        
        #detail-page .right-nav
        {
            display: none;
        }
        .sf_preview ul.second-ul
        {
            float: right;
            width: 725px;
            display: inline-block;
            border-top: none;
        }
        
        .sf_preview ul.second-ul li
        {
            width: 725px;
            line-height: 30px;
            font-size: 12px;
            color: #555;
            border-bottom: 1px solid #e7e7e7;
            text-indent: 15px;
        }
        
        .sf_preview ul.second-ul li:last-child
        {
            border-bottom: none;
        }
        
        .sf_preview ul.second-ul li span
        {
            float: left;
            width: 220px;
            padding-right: 5px;
            background: #ffffff;
            color: #333;
            text-align: right;
        }
    </style>
</head>
<body id="detail-page">
    <div id="page" class="w950">
        <!--content-->
        <div id="content">
            <div class="float_l w950">
                <div class="float_l sf_reaervationt mart15" id="bx_title">
                    <b>“畅游华夏”境内旅行保险</b>
                </div>
                <div class="sf_preview mart10" id="bx_policyDetailInfo">
                    <p>
                        保单明细</p>
                    <ul>
                        <li><span>投保份数：</span></li>
                        <li><span>单价：</span></li>
                        <li><span>保险费：</span></li>
                        <li><span>保险期间：</span></li>
                        <li><span>投保单号：</span></li>
                    </ul>
                </div>
                <div class="clear">
                </div>
                <div class="sf_preview mart10" id="bx_applicantInfo">
                    <p>
                        投保人信息</p>
                    <ul>
                        <li><span>投保人姓名：</span> </li>
                        <li><span>拼音/英文名：</span> </li>
                        <li><span>证件类型：</span> </li>
                        <li><span>证件号码：</span> </li>
                        <li><span>出生日期：</span> </li>
                        <li><span>性别：</span> </li>
                        <li><span>手机号码：</span> </li>
                        <li><span>电子邮箱：</span> </li>
                    </ul>
                </div>
                <div class="clear">
                </div>
                <div id="bx_InsurantInfo">
                   <!-- <div class="sf_preview mart10">
                        <p>
                            第1被保险人</p>
                        <ul>
                            <li><span>与被保险人关系：</span>其他 </li>
                            <li><span>被保险人姓名：</span>李四 </li>
                            <li><span>证件类型：</span>身份证 </li>
                            <li><span>证件号码：</span>110101199609019894 </li>
                            <li><span>出生日期：</span>1996-09-01 </li>
                            <li><span>性别：</span>男 </li>
                            <li><span>手机号码：</span> </li>
                            <li><span>购买份数：</span>1 </li>
                        </ul>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="sf_preview mart10">
                        <p>
                            第2被保险人</p>
                        <ul>
                            <li><span>与被保险人关系：</span>其他 </li>
                            <li><span>被保险人姓名：</span>张三 </li>
                            <li><span>证件类型：</span>身份证 </li>
                            <li><span>证件号码：</span>110101199609013337 </li>
                            <li><span>出生日期：</span>1996-09-01 </li>
                            <li><span>性别：</span>男 </li>
                            <li><span>手机号码：</span> </li>
                            <li><span>购买份数：</span>1 </li>
                        </ul>
                    </div>
                    <div class="clear">
                    </div>-->
                </div>
                <div class="sf_preview mart10">
                    <p>
                        第受益人信息</p>
                    <ul>
                        <li><span>受益人：</span>法定继承人 </li>
                    </ul>
                </div>
                <div class="clear">
                </div>
                <div class="sf_preview mart10">
                    <p>
                        紧急联系人</p>
                    <ul>
                        <li><span>姓名：</span> </li>
                        <li><span>电话：</span> </li>
                    </ul>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!--content end-->
    </div>
    <input type="hidden" id="hid_orderid" value="<%=orderid %>" />
</body>
</html>
