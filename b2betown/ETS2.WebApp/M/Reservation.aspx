<%@ Page Title="" Language="C#" MasterPageFile="~/M/MemberH5.Master" AutoEventWireup="true" CodeBehind="Reservation.aspx.cs" Inherits="ETS2.WebApp.M.Reservation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //日历
            var nowdate = '<%=this.nowdate %>';
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {

                $(this).val(nowdate);
                $($(this)).datepicker({
                    numberOfMonths: 1,
                    minDate: 0,
                    defaultDate: +4,
                    maxDate: '+8m +1w'
                });
            });
        })


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
            var datetime = $("#datetime").val();

            if (number == null || number == 0) {
                $("#num_span").html("请填写预订数量");
                $("#num_span").css('color', 'red');
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
            if (datetime == null || datetime == "") {
                $("#date_span").html("请填写时间");
                $("#date_span").css('color', 'red');
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
                    if (data.msg == 1) {
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
    </script>
    <style type="text/css">
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
        
        .inner
        {
            width: 270px;
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
            width: 270px;
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
        }
		
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
margin: 0 10px 20px;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   <div class="inner">
        <h1>
            <%=price %>
            <%=title %></h1>
        <%
            if (headPortraitImgSrc != "")
            {
        %>
        <p>
            <strong><span style="color: #ff0000;">
                <img title="" src="<%=headPortraitImgSrc %>" /></span></strong></p>
        <%
            }
        %>
        <p>
            <%=summary %>
        </p>
        <p>
            <%=article %>
        </p>
        <p>
            <%=phone %>
        </p>
        <p>
            <strong><span style="color: #ff0000;"></span></strong>
            <br />
        </p>
    <% if (comid==101) { %>
    <p class="p-tips">微预订：（提交预订信息后微旅行客服将与您联系确认）</p> 
    <div class="w-item"> 
        <dl class="in-item fn-clear"> 
            <dt>预订数量 </dt> 
          <dd><input type="number" id="getNum" name="TravelerMobile" maxlength="11" placeholder="请填写预定人数" value="" class="writeok" /><span id="num_span"></span></dd> 
        </dl> 
    </div> 
    <span id="date_span"></span>
    <div class="w-item"> 
        <dl class="in-item fn-clear"> 
            <dt>预订时间</dt> 
            <dd>
                <input name="datetime" type="datetime" id="datetime" size="12" isdate="yes"/>
            </dd>
        </dl> 
    </div>
    <div class="w-item"> 
        <dl class="in-item fn-clear"> 
            <dt>您的姓名</dt> 
            <dd><input type="text" id="getName" name="TravelerName" placeholder="请填写预定人姓名" value="" class="writeok" /> <span id="name_span"></span> </dd>
        </dl> 
    </div> 
    <div class="w-item"> 
        <dl class="in-item fn-clear"> 
            <dt>手机号</dt> 
            <dd><input type="tel" id="getPhone" name="TravelerMobile" maxlength="11" placeholder="免费接收确认短信" value="" class="writeok" /><span id="phone_span"></span></dd> 
        </dl> 
    </div> 
    <div id="perList"> 
    </div> 
      <div class="order-btn fn-clear"> 
          <div class="submit-btn"> 
              <input type="submit" class="btn" onclick="javascript:submitbtn()" id="submitBtn" value="提交预定" /></div> 
        </div>
 </div>
  <% } %>
    <input type="hidden" id="wxid" value="<%=id %>" />
</asp:Content>
