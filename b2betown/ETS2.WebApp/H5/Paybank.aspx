<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Paybank.aspx.cs" Inherits="ETS2.WebApp.H5.Paybank" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript">
        var _tcopentime = new Date().getTime();
        var _hmt = _hmt || [];
    </script>
    <title>储蓄卡支付</title>
    <!-- meta信息，可维护 -->
    <meta charset="UTF-8">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta content="telephone=no" name="format-detection">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <!-- 页面样式表 -->    
    <link href="../Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .none{ display:none;}
    </style>
    <!-- 页面样式表 -->
    <link href="../Styles/H5/paymode.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <!-- 公共页头  -->
    <a href="javascript:history.go(-1);">
      <header class="header">
                    <h1><span id="span_title"></span></h1>
        <div class="left-head">
          <a id="goBack" href="javascript:history.go(-1);" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
        <div class="right-head">
          <a href="#" class="head-btn none"><span class="inset_shadow"><span class="head-home"></span></span></a>
        </div>
    </header>
    </a>
    <!-- 页面内容块 -->
    
<div class="body">

  <form method="post" id="form">
      <div id="chu" style=" display:none">
      <dl class="banklist-wrap">
        <dt class="bankhot">快捷支付</dt>
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="ICBC">中国工商银行储蓄卡</a>
        </dd>  
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="CCB">中国建设银行储蓄卡</a>
        </dd> 
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="ABC">中国农业银行储蓄卡</a>
        </dd> 
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="CEB">中国光大银行储蓄卡</a>
        </dd> 
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="BOC">中国银行储蓄卡</a>
        </dd> 
        <dt></dt>
      </dl>
      </div>
      <div id="xin"style=" display:none">
      <dl class="banklist-wrap">
        <dt class="bankhot">快捷支付</dt>
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="ICBC">中国工商银行信用卡</a>
        </dd> 
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="CMB">招商银行信用卡</a>
        </dd> 
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="CCB">中国建设银行信用卡</a>
        </dd> 
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="ABC">中国农业银行信用卡</a>
        </dd> 
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="CEB">中国光大银行信用卡</a>
        </dd> 
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="SPABANK">平安信用卡</a>
        </dd>  
        <dd class="banklist-content">
          <a href="javascript:void(0)" class="cashiercode" data-id="BOC">中国银行信用卡</a>
        </dd> 
        <dt></dt>
      </dl>
      </div>
       <%--<dl class="banklist-wrap">
          <dt class="banklist-title">A</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ARCU">安徽省农村信用社储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_AYCB">安阳银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">B</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BSB">包商银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BJBANK">北京银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOHAIB">渤海银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">C</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CABANK">长安银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CZCCB">长治市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOCY">朝阳银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOCD">承德银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CDRCB">成都农商银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CDCB">成都银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CRCBANK">重庆农村商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CCQTGB">重庆三峡银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CQBANK">重庆银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">D</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BODD">丹东银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_DAQINGB">大庆市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_DYCB">德阳商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_DZBANK">德州银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_DYCCB">东营银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_DRCBCL">东莞农村商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOD">东莞银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">E</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ORBANK">鄂尔多斯银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">F</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_FDB">富滇银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_FJHXBC">福建海峡银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_FJNX">福建省农村信用社联合社储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_FSCB">抚顺银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_FXCB">阜新银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">G</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_GZB">赣州银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_GHB">广东华兴银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_NYBANK">广东南粤银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_GDRCC">广东省农村信用社联合社储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_GDB">广发银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BGB">广西北部湾银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_GRCB">广州农村商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_GCB">广州银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_GLBANK">桂林银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_GYCB">贵阳银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">H</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_HDBANK">邯郸银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_HZCB">杭州银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_HKB">汉口银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_HANABANK">韩亚银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BHB">河北银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOHB">鹤壁银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_EGBANK">恒丰银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_HSBK">衡水市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_HXBANK">华夏银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_HBC">湖北银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_H3CB">呼和浩特市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_HSBC">汇丰银行(中国)储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_HSBANK">徽商银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">J</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CZRCB">江南农村商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_JRCB">江苏江阴农村商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_TCRCB">江苏太仓农村商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_JSBANK">江苏银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_COMM">交通银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_JXBANK">嘉兴银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_JINCHB">晋城银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_JHBANK">金华银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_JNBANK">济宁银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_JSB">晋商银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_JZBANK">晋中市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOJZ">锦州银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_JJBANK">九江银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">K</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CBKF">开封市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_KORLABANK">库尔勒市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_KLB">昆仑银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">L</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_LSBANK">莱商银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_LZYH">兰州银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_LSCCB">乐山市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_LSBC">临商银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_LZCCB">柳州银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">N</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_NCB">南昌银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CGNB">南充市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_NJCB">南京银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_NYCB">南阳村镇银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_NDHB">宁波东海银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_NBBANK">宁波银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">P</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_PZHCCB">攀枝花市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOP">平顶山银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">Q</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_QLBANK">齐鲁银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_QDCCB">青岛市商业银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">R</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_RZB">日照银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">S</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SCCB">三门峡银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SDRCU">山东省农村信用社联合社储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SHBANK">上海银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SRBANK">上饶银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SXCB">绍兴市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SJBANK">盛京银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SRCB">深圳农村商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SPABANK">深圳平安银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SZSBK">石嘴山银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SDEB">顺德农村信用联社储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SCRCU">四川省农村信用社联合社储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOSZ">苏州银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">T</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_TACCB">泰安市商业银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">W</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BANKWF">潍坊银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_WHCCB">威海市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_WHBANK">乌海银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_WJRCB">吴江农村商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_WRCB">无锡农村商业银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">X</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_XMBANK">厦门银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_XABANK">西安银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_XLBANK">小榄银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_XTB">邢台银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CIB">兴业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_DBSCN">星展银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOSH">新韩银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_XXBANK">新乡银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_XYBANK">信阳银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_XCYH">许昌银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title return">Y</dt>
          <dd class="banklist-content none" style="display: block;">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_YQCCB">阳泉市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none" style="display: block;">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_YBCCB">宜宾市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none" style="display: block;">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOYK">营口银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none" style="display: block;">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_YNRCC">云南省农村信用社储蓄卡</a>
          </dd> 
          <dd class="banklist-content none" style="display: block;">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_YXCCB">玉溪市商业银行储蓄卡</a>
          </dd> 
       </dl> 
       <dl class="banklist-wrap">
          <dt class="banklist-title">Z</dt>
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_SCB">渣打银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ZRCBANK">张家港农村商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ZJKCCB">张家口市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CMB">招商银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ZJNX">浙江省农村信用社联合社储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ZZBANK">郑州银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ICBC">中国工商银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CEB">中国光大银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CCB">中国建设银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CMBC">中国民生银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ABC">中国农业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOC">中国银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_PSBC">中国邮政储蓄银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_CITIC">中信银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BOZK">周口银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_RBOZ">珠海华润银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_BZMD">驻马店银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ZGCCB">自贡市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_ZYCBANK">遵义市商业银行储蓄卡</a>
          </dd> 
          <dd class="banklist-content none">
            <a href="javascript:void(0)" class="cashiercode" data-id="DEBITCARD_NBYZ">鄞州银行储蓄卡</a>
          </dd> 
       </dl> --%>
    <input type="hidden" id="orderid" name="orderid" value="<%=orderid %>"/> 
    <input type="hidden" id="bank" name="orderid" value="<%=bank %>"/>
  </form>
</div>


    <!-- 公共页脚  -->
  <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
  <script type="text/javascript">
      $(function () {
          var bank = $("#bank").val();
          if (bank == 1) {
              $("#chu").show();
              $("#xin").hide();
              $("#span_title").html("储蓄卡支付");
          }
          if (bank == 2) {
              $("#chu").hide();
              $("#xin").show();
              $("#span_title").html("信用卡支付");
          }
          $(".banklist-title").click(function () {
              if ($(this).hasClass("return")) {
                  $(this).removeClass("return");
                  $(this).siblings(".banklist-content").hide();
              }
              else {
                  $(this).addClass("return");
                  $(this).siblings(".banklist-content").show();
              }
          })

          $(".cashiercode").click(function () {
              var bankname = $(this).attr("data-id");
              var orderid = $("#orderid").val();
              if (bank == 1) {
                  //location.href = "Payment.aspx?out_trade_no=" + orderid + "&paymethod=expressGatewayDebit&defaultbank=" + bank + "&amp;Payt=d";
                  location.href = "/UI/VASUI/alipay/subpay.aspx?out_trade_no=" + orderid + "&paymethod=motoPay&defaultbank=" + bankname + "&amp;Payt=d";
              }
              if (bank == 2) {
                  location.href = "/UI/VASUI/alipay/subpay.aspx?out_trade_no=" + orderid + "&paymethod=motoPay&defaultbank=" + bankname + "&amp;Payt=c";
              }
          })
      })
  </script>
  
</body>
</html>
