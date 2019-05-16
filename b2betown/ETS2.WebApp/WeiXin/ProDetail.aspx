<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProDetail.aspx.cs" Inherits="ETS2.WebApp.WeiXin.ProDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;" />
    <meta name="format-detection" content="telephone=no" />
    <title>
        <%=title %></title>
    <style>
        body
        {
            background-color: #F7F8F3;
            text-align: center;
            font-family: 'Helvetica Neue' ,sans-serif;
            overflow-x: hidden;
        }
        body, article, section, h1, h2, hgroup, p, a, ul, li, em, div, small, span, footer, canvas, figure, figcaption, input
        {
            margin: 0;
            padding: 0;
        }
        a
        {
            text-decoration: none;
            cursor: pointer;
        }
        a.autotel
        {
            text-decoration: none;
            color: inherit;
        }
        .tel{
         width:134px;height:33px;
         line-height:33px;
         display:inline-block;
         text-align:center;
         font-size:14px;
         border:1px solid #4a4a4a;
         margin:0 7px;
         -webkit-border-radius:6px;
         border-radius:6px;
         text-shadow:0 1px #2daf35;
         color:#fff;
         box-shadow:inset 0 0 5px #8ee392;
         background-color:#29a832;
         background-image:-webkit-gradient(linear,0% 0,0% 100%,from(#4cbd51),to(#079414));
         }
         .yuding{
         width:134px;height:33px;
         line-height:33px;
         display:inline-block;
         text-align:center;
         font-size:14px;
         border:1px solid #4a4a4a;
         margin:0 7px;
         -webkit-border-radius:6px;
         border-radius:6px;
         text-shadow:0 1px #2daf35;
         color:#fff;
         box-shadow:inset 0 0 5px #8ee392;
         background-color:#29a832;
         background-image:-webkit-gradient(linear,0% 0,0% 100%,from(#4cbd51),to(#079414));
         }
        .inner
        {
            /*width: 270px;*/
            min-width: 280px;
            padding: 10px 25px;
            margin: 0 auto;
        }
        h1
        {
            font-size: 22px;
            font-weight: normal;
            line-height: 26px;
            margin-bottom: 18px;
        }
        img
        {
            width: 280px;
            border: none;
            margin-bottom: 8px;
        }
        .old_message
        {
            line-height: 20px;
            text-indent: 2em;
            font-size: 14px;
            color: #565752;
            text-align: left;
            margin-bottom: 10px;
            word-wrap: break-word;
        }
        p
        {
            color: #565752;
            text-align: left;
            margin-bottom: 10px;
            word-wrap: break-word;
            line-height:25px;
        }
        .header{height:44px;line-height:44px;text-align:center;background-color:#0065cd}
        .header h1.logo{width:97px;height:26px;display:block;margin:5px auto 0 auto;;background-size:97px 26px}
        .header h1{width:170px;display:block;margin:0 auto;font-family:microsoft yahei;font-size:18px;color:#fff;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;font-weight:700}
        .header h1.small{padding-top:5px;font-size:12px;line-height:16px;white-space:normal;overflow:visible}
        .header h1.single{font-size:12px;line-height:40px}
        .left-head{position:absolute;left:0;top:0;height:40px}
        .head-return{width:11px;height:17px;margin:5px 15px 0;overflow:hidden;display:inline-block;background:url(../Images/icon_back.png) no-repeat;background-size:11px 17px}
        .w-item {
padding: 0 10px;
background: #eee;
margin-bottom: 10px;
line-height: 1.5em;
}
.in-item {
line-height: 44px;
}
.fn-clear {
zoom: 1;
}
.fn-clear:after {
visibility: hidden;
display: block;
font-size: 0;
content: " ";
clear: both;
height: 0;
}
dl {
display: block;
-webkit-margin-before: 1em;
-webkit-margin-after: 1em;
-webkit-margin-start: 0px;
-webkit-margin-end: 0px;
}
.in-item dt {
float: left;
width: 28%;
}
.in-item dd {
position: relative;
overflow: hidden;
padding: 0 15px 0 10%;
}
dd {
display: block;
-webkit-margin-start: 40px;
}
.writeok, .in-item dd input.writeok {
color: #005bb5;
}

.in-item dd input {
width: 100%;
margin-right: -15px;
height: 44px;
border: 0;
background: 0;
color: #005bb5;
outline: 0;
-webkit-box-shadow: none;
border-radius: 0;
}
.fn-clear:after {
visibility: hidden;
display: block;
font-size: 0;
content: " ";
clear: both;
height: 0;
}
.in-item-number dt {
line-height: 44px;
float: left;
width: 25%;
}
.remind {
height: 18px;
line-height: 18px;
overflow: hidden;
font-size: 12px;
padding-left: 25px;
position: relative;
}
.qminu {
position: absolute;
left: 8px;
top: 1px;
background: #7eafc7;
color: #fff;
border-radius: 5px;
display: inline-block;
padding: 0 2px;
font-size: 10px;
line-height: 16px;
}
.f60 {
color: #f60;
}
.order-btn {
height: 44px;
line-height: 44px;
margin: 0 20px 20px;
font-size: 18px;
text-align: center;
color: #fff;
background: #fe932b;
}
.order-btn input {
width: 100%;
height: 40px;
border: 0;
background: 0;
color: #fff;
}
input, select, textarea {
font-size: 100%;
}
.phone{}
.phone a
{
    font-size: 16px;
    font-weight:bold;
    color: #fff;
   background: #fe932b;
   padding:5px 10px;
   text-align: center;
   vertical-align:middle;
    }
    #mcover {
    position: fixed;
    top: 0px;
    left: 0px;
    width: 100%;
    height: 100%;
    background: none repeat scroll 0% 0% rgba(0, 0, 0, 0.7);
    display: none;
    z-index: 20000;
}
#mcover img {
    position: fixed;
    right: 18px;
    top: 5px;
    width: 260px !important;
    height: 180px !important;
    z-index: 20001;
}
.text {
    margin: 15px 0px;
    font-size: 14px;
    word-wrap: break-word;
    color: #727272;
}
#mess_share {
    margin: 15px 0px;
    display: block;
}
#share_1 {
    float: left;
    width: 49%;
    display: block;
}
.button2 {
    font-size: 16px;
    padding: 8px 0px 0px 0px;
    border: 1px solid #ADADAB;
    color: #000;
    background-color: #E8E8E8;
    background-image: linear-gradient(to top, #DBDBDB, #F4F4F4);
    box-shadow: 0px 1px 1px rgba(0, 0, 0, 0.45), 0px 1px 1px #EFEFEF inset;
    text-shadow: 0.5px 0.5px 1px #FFF;
    text-align: center;
    border-radius: 3px;
    width: 100%;
    vertical-align:middle
}
#share_2 {
    float: right;
    width: 49%;
    display: block;
}
#mess_share img {
    width: 22px !important;
    height: 22px !important;
    vertical-align: top;
    border: 0px none;
}
    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //日历
            var nowdate = $("#nowdate").val();
            $("#datetime").html(nowdate);
            $("#selDate").click(function () {
                scrollTo(0, 1);
                thisMonth("traveldate");
                $("#orderFome").removeClass().addClass("translate3d");
                $("#calDiv").fadeIn(); $("#inner").hide()
            });
            $("#goBack").click(function () {
                $("#calDiv").hide();
                $("#inner").show();
                $("#datetime").val("请选择时间");
            })
        })

        //验证手机号
        function isMobel(value) {
            if (/^13\d{9}$/g.test(value) || /^15\d{9}$/g.test(value) || /^14\d{9}$/g.test(value) || /^18\d{9}$/g.test(value)) {
                return true;
            } else {
                return false;
            }
        }
        function submitbtn() {
            $("#num_span").html("");
            $("#name_span").html("");
            $("#phone_span").html("");
            $("#date_span").html("");
            var comid = 101;
            var id = $("#wxid").val();
            var number = $("#getNum").val();
            var name = $("#getName").val();
            var phone = $("#getPhone").val();
            var datetime = $("#datetime").html();

            if (number == null || number == 0) {
                $("#num_span").html("请填写预订数量");
                $("#num_span").css('color', 'red');
                return;
            }
            if (datetime == null || datetime == "请选择时间" || datetime == "") {
                $("#date_span").html("请填写时间");
                $("#date_span").css('color', 'red');
                return;
            }
            if (name == null || name == "") {
                $("#name_span").html("请填写预订人");
                $("#name_span").css('color', 'red');
                return;
            }
            if (phone == null || name == "") {
                $("#phone_span").html("请填写手机号");
                $("#phone_span").css('color', 'red');
                return;
            }
            if (isMobel(phone) == false) {
                $("#phone_span").html("手机格式错误");
                $("#phone_span").css('color', 'red');
                return;
            }

            //提交预订
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Reservation_insert", { id: id, comid: comid, number: number, name: name, phone: phone, datetime: datetime }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 0) {
                    $("#num_span").html("预订出现错误");
                    $("#num_span").css('color', 'red');
                    return;
                }
                if (data.type == 10) {
                    if (data.msg != 0) {
                        alert("预订提交成功");
                        $("#getNum").val("");
                        $("#getName").val("");
                        $("#getPhone").val("");
                        return;
                    }
                    else {
                        alert("预订提交成功" + data.msg + "条");
                        $("#getNum").val("");
                        $("#getName").val("");
                        $("#getPhone").val("");
                        return;
                    }
                }
            })
        }

        function clickbook(bookurl) {
            window.open(bookurl, target = "_self");
        }
  
    </script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <script type="text/javascript">
        var _tcopentime = new Date().getTime();
        var _hmt = _hmt || [];
    </script>
    <script type="application/x-javascript">
        addEventListener('DOMContentLoaded',function(){setTimeout(function(){scrollTo(0,1);},0);},false);
    </script>
</head>
<body>
    <div class="inner" id="inner">
        <h1>
            <span style="color: #fe932b; font-weight: bold">
                <%=price %>
            </span>&nbsp;
            <%=title %></h1>
        <div style="height: 20px; line-height: 15px; font-size: 12px; text-align: left; margin-top: -7px;">
            <%
                if (authorpayurl != "")
                {
            %>
            <table>
                <tr>
                    <td sytle="white-space: nowrap">
                        <a href="<%=authorpayurl %>">
                            <%=datetime%>&nbsp;
                            <%=Author%>
                        </a>
                    </td>
                    <td>
                        <div style="width: 25px; float: left;">
                            <img src="/Images/jiantou.png" style="width: 14px; margin: 3px 1px 3px 5px" />
                        </div>
                        <div style="width: 60px; float: left; color: #565752;">
                            点击加关注</div>
                    </td>
                </tr>
            </table>
            <%
                }
                else
                { 
            %>
            <%=datetime%>&nbsp;
            <%=Author%>
            <%
                }
            %>
        </div>
        <%
            if (headPortraitImgSrc != "")
            {
        %>
        <p>
            <center>
                <strong><span style="color: #ff0000;">
                    <img title="" src="<%=headPortraitImgSrc %>" /></span></strong></center>
        </p>
        <%
            }
        %>
        <p>
            <%=summary %>
        </p>
        <p>
            <%=article %>
        </p>
        <%
            if (phone_tel != "")
            {
        %>
        <p>
            <%=phone %><a href="tel:<%=phone_tel %>"><%=phone_tel %></a>
        </p>
        <%
            }
        %>
        <%-- <p>
            <strong><span style="color: #ff0000;"></span></strong>
            <br />
        </p>--%>
        <% 
            if (comid == 101)
            {
        %>
        <div style="display: none">
            <p class="p-tips">
                微预订：（提交预订信息后微旅行客服将与您联系确认）</p>
            <span id="num_span"></span>
            <div class="w-item">
                <dl class="in-item fn-clear">
                    <dt>预订数量 </dt>
                    <dd>
                        <input type="number" id="getNum" name="TravelerMobile" maxlength="5" placeholder="人数"
                            value="" class="writeok" /></dd>
                </dl>
            </div>
            <span id="date_span"></span>
            <div class="w-item">
                <dl class="in-item fn-clear" id="selDate">
                    <dt>日期</dt>
                    <dd>
                        <span id="datetime" class="writeok" size="12" value=""></span>
                    </dd>
                </dl>
            </div>
            <span id="name_span"></span>
            <div class="w-item">
                <dl class="in-item fn-clear">
                    <dt>您的姓名</dt>
                    <dd>
                        <input type="text" id="getName" name="TravelerName" placeholder="姓名" value="" class="writeok" />
                    </dd>
                </dl>
            </div>
            <span id="phone_span"></span>
            <div class="w-item">
                <dl class="in-item fn-clear">
                    <dt>手机号</dt>
                    <dd>
                        <input type="tel" id="getPhone" name="TravelerMobile" maxlength="11" placeholder="免费接收确认短信"
                            value="" class="writeok" /></dd>
                </dl>
            </div>
            <div id="perList">
            </div>
            <div class="order-btn fn-clear">
                <div class="submit-btn">
                    <input type="submit" class="btn" onclick="javascript:submitbtn()" id="submitBtn"
                        value="提交预定" /></div>
            </div>
        </div>
        <div class="w-item">
            <span style="background-color: rgb(255, 0, 0); color: rgb(255, 255, 255);"><strong></strong>
            </span></p>
            <p style="margin-top: 0px; margin-bottom: 0px; white-space: normal; padding: 0px;
                min-height: 1.5em;">
                <span style="font-size: 14px; color: rgb(0, 176, 80);">阅读以后不要忘记了点击手机屏幕右上角的按钮，分享到朋友圈！</span>
                <span style="font-size: 14px; font-family: Arial, sans-serif;">
                    <br />
                    <span style="font-size: 14px; font-family: Arial, sans-serif; color: rgb(0, 176, 80);">
                        ————</span></p>
            <p style="margin-top: 0px; margin-bottom: 0px; white-space: normal; padding: 0px;
                min-height: 1.5em;">
                <span style="font-size: 14px; color: rgb(0, 176, 80);"><span style="color: #fe932b;
                    font-weight: bold">关注有礼!</span>赠送</span> <span style="font-size: 14px; font-family: Arial, sans-serif;
                        color: rgb(0, 176, 80);">100</span> <span style="font-size: 14px; color: rgb(0, 176, 80);">
                            元积分哟，邀请好友关注微旅行，好友也可以领取哟。</span> <span style="font-size: 14px; font-family: Arial, sans-serif;
                                color: rgb(0, 176, 80);">
                                <br />
                                ————<br />
                            </span><span style="font-size: 14px; color: rgb(0, 176, 80);">回复</span>
                <span style="font-size: 14px; font-family: Arial, sans-serif; color: rgb(0, 176, 80);">
                    “韩国”，“泰国”，“云南”，“台湾”，“香港”，“九寨沟“ 查看线路介绍。</span></span><span style="font-size: 14px;
                        font-family: Arial, sans-serif; color: rgb(0, 176, 80);"><br />
                        ————<br />
                    </span><span style="font-size: 14px; color: rgb(0, 176, 80);">回复</span> <span style="font-size: 14px;
                        font-family: Arial, sans-serif; color: rgb(0, 176, 80);">“特价”查看特价路线</span></p>
            <br />
            <span style="margin-left: -10px;">
                <img src="../Images/weilxwx.jpg" /></span>
        </div>
        <% 
            }            
        %>
    </div>
    <input type="hidden" id="wxid" value="<%=id %>" />
    <input type="hidden" id="nowdate" value="<%=nowdate %>" />
    <div id="calDiv" style="display: none">
        <header class="header">
                    <h1>选择日期</h1>
        <div class="left-head">
          <a id="goBack" href="#" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
    </header>
        <div class="low_calendar">
            <div class="low_calendar_top">
                <a class="top_left" href="javascript:;" id="last_mon"><span class="last_mon"></span>
                </a><span class="top_middle"><span id="dateInfo"></span></span><a class="top_right"
                    href="javascript:;" id="next_mon"><span class="next_mon"></span></a>
            </div>
            <div id="calendar">
            </div>
        </div>
        <div class="sel-tip">
            <h4 style="text-align: left">
                温馨提示：</h4>
            <p>
                1、<span class="f60">如需预订，您最晚要在游玩当天20:30前下单，请尽早预订。每人最多限购5张。</span><br />
                2、查看游玩日期及景点门票的价格，若需预订，请直接选中对应的日期即可。</p>
        </div>
        <ul id="holidayList">
        </ul>
    </div>
    <%
        if (Articleurl != "")
        {
    %>
    <div class="order-btn fn-clear">
        <div class="submit-btn">
            <input type="button" class="btn" id="submitBtn1" value="我要预订" onclick="clickbook('<%=Articleurl %>')" />
        </div>
    </div>
    <%
        }
        else
        { 
    %>
    <section style="width: 95%; margin: 0px auto;">
	<div id="mcover" onclick="document.getElementById('mcover').style.display='';" style="display:none;">
		<img src="/images/guide.png">
	</div>
	<div class="text" id="content">
		<div id="mess_share">
			<div id="share_1">
				<button class="button2" onclick="document.getElementById('mcover').style.display='block';">
					<img src="/images/icon_msg.png">&nbsp;发送给朋友
				</button>
			</div>
			<div id="share_2">
				<button class="button2" onclick="document.getElementById('mcover').style.display='block';">
					<img src="/images/icon_timeline.png">&nbsp;分享到朋友圈
				</button>
			</div>
			<div class="clr"></div>
		</div>
	</div>
</section>
    <%
        if (authorpayurl != "")
        {
    %>
    <div style="width: 50%; float: left; text-align: center;">
        <a href="<%=authorpayurl %>" style="font-size: 12px; height: 30px; display: block;
            color: #000000; text-align: center; line-height: 35px; margin-bottom: 40px; margin-top: 20px;">
            关注我们</a>
    </div>
    <div style="padding-bottom: 0!important; width: 50%; float: left; text-align: center;">
        <a style="font-size: 12px; height: 30px; display: block; color: #000000; text-align: center;
            line-height: 35px; margin-bottom: 40px; margin-top: 20px;" href="javascript:window.scrollTo(0,0);">
            返回顶部</a>
    </div>
    <%}
        else
        { %>
    <div style="padding-bottom: 0!important;">
        <a style="font-size: 12px; margin: 80px auto; display: block; color: #000000; text-align: center;
            line-height: 35px; margin-bottom: 0px;" href="javascript:window.scrollTo(0,0);">
            返回顶部</a>
    </div>
    <%}%>
    <%
        }
    %>
    <!-- 公共页脚  -->
    <script src="../Scripts/mCal.js?v=7" type="text/javascript"></script>
    <link href="../Scripts/mCal.css" rel="stylesheet" type="text/css" />
    <!--Baidu-->
    <script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "//hm.baidu.com/hm.js?8fcd06cc927f3554397ca18509561b69";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>
</body>
</html>
