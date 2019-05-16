<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EticketList.aspx.cs" Inherits="ETS2.WebApp.M.EticketList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <meta name="format-detection" content="telephone=no">
    <link rel="stylesheet" type="text/css" href="/Styles/css4.css" />
    <script type="text/javascript" src="/Scripts/js4.js"></script>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/Impromptu.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function goBack() {
            window.location.href = 'Default.aspx';
        }      
    </script>
    <script type="text/javascript">
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            WeixinJSBridge.call('hideToolbar');
        });
    </script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 100; //每页显示条数
        $(function () {

            SearchList(1);

            //装载订单列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=ConsumerOrderPageList",
                    data: { openid: $("#hid_openid").val(), AccountId: $("#hid_AccountId").val() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#ulreply").append("<section  class='section1'><header></header><article>还没有成功订单</article><time></time></section>");

                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#ulreply").append("<section  class='section1'><header></header><article>还没有成功订单</article><time></time></section>");

                            } else {
                                //                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#ulreply");
                                //                                setpage(data.totalCount, pageSize, pageindex);
                                //填充列表数据
                                var j = 1;
                                $.each(data.msg, function (i, item) {


                                    var htmlstr = "";
                                    if (j == 1) {
                                        htmlstr = htmlstr + ' <section onclick="GetOrderDetail(' + item.Id + ')" class="section1">'
                                    } else if (j == 2) {
                                        htmlstr = htmlstr + ' <section onclick="GetOrderDetail(' + item.Id + ')" class="section2">'
                                    } else if (j == 3) {
                                        htmlstr = htmlstr + ' <section onclick="GetOrderDetail(' + item.Id + ')" class="section3">'
                                    }
                                    var orderstate_view = "";
                                    if (item.Order_state == 2) {
                                        orderstate_view = "订单处理中";
                                    }
                                    if (item.Order_state == 4) {
                                        orderstate_view = "发货中 " ;
                                    }
                                    if (item.Order_state == 8) {
                                        orderstate_view = "已发货 ";
                                    }
                                    if (item.Order_state == 16) {
                                        orderstate_view = "退款";
                                    }

                                    htmlstr = htmlstr + '<header>	'
                                    htmlstr = htmlstr + '<span class="status_list_000"> </span>	'
                                    htmlstr = htmlstr + '<img src="' + item.ConsumerLogo + '" class="card_logo" alt=""   height="29px">' + item.ConsumerCompanyName
                                    htmlstr = htmlstr + '</header>	'
                                    htmlstr = htmlstr + '<article>	   '
                                    htmlstr = htmlstr + '<p><f effect="4c000000,0,-1" size="21" rowspace="5" maxrows="2" face"din-regular"="">' + item.Proname + '(' + item.U_num + '张)</f></p>	'
                                    htmlstr = htmlstr + '</article>'
                                    if (item.Server_type == 11) {
                                        htmlstr = htmlstr + '<time>实物订单：  <span style="width: 120px; height: 25px;">'+orderstate_view+'</span></time><br>'
                                    } else {
                                        htmlstr = htmlstr + '<time>有效期截止到：  <span style="width: 120px; height: 25px;">' + item.ProValid + '</span></time><br>'
                                    }
                                    htmlstr = htmlstr + '<time>  </time>'
                                    htmlstr = htmlstr + '</section>'




                                    j = j + 1;
                                    if (j == 4) {
                                        j = 1;
                                    }
                                    $("#ulreply").append(htmlstr);

                                })


                            }


                        }
                    }
                })
            }
            //            //分页
            //            function setpage(newcount, newpagesize, curpage) {
            //                $("#divPage").paginate({
            //                    count: Math.ceil(newcount / newpagesize),
            //                    start: curpage,
            //                    display: 5,
            //                    border: false,
            //                    text_color: '#888',
            //                    background_color: '#EEE',
            //                    text_hover_color: 'black',
            //                    images: false,
            //                    rotate: false,
            //                    mouse: 'press',
            //                    onChange: function (page) {

            //                        SearchList(page);

            //                        return false;
            //                    }
            //                });
            //            }
        })
        function GetOrderDetail(orderid) {
            window.open("EticketDetail.aspx?orderid=" + orderid + "&openid=" + $("#hid_openid").trimVal(), target = "_self");
        }
        
    </script>
</head>
<body>
    <div id="ulreply">
    </div>
    <!--<div class="footFix" id="footReturn">
        <a href="indexcard.aspx?openid=<%=openid %>"><span>返回会员卡首页</span></a>
    </div>-->
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_AccountId" value="<%=AccountId %>" />
    <%--   <script type="text/x-jquery-tmpl" id="ProductItemEdit">
          <section onclick="GetOrderDetail(${Id})" class="section1">
        <header>	
        	<span class="status_list_000"> </span>	
            		<img src="${ConsumerLogo}" class="card_logo" alt="">${ConsumerCompanyName}
                    </header>	
                    <article>	   
                    <p><f effect="4c000000,0,-1" size="21" rowspace="5" maxrows="2" face"din-regular"="">${Proname} (${U_num}张)</f></p>	
                    </article>
                    	<time>有效期截止到：  <span style="width: 120px; height: 25px;">${ProValid}</span></time><br>
                        <time>  </time>
                        </section>
                    
    </script>--%>
</body>
</html>
