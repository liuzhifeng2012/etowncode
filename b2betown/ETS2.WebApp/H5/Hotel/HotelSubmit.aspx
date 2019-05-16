<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Hotel/hotel.Master" AutoEventWireup="true" CodeBehind="HotelSubmit.aspx.cs" Inherits="ETS2.WebApp.H5.Hotel.HotelSubmit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../../Styles/Hotel2/base.2.0.css" rel="stylesheet" type="text/css" />
<link href="/Styles/Hotel/orderWrite_com.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var roomtypeid = $("#hid_roomtypeid").val();
            var checkindate = $("#hid_checkindate").val();
            var checkoutdate = $("#hid_checkoutdate").val();
            var bookdaynum = $("#hid_bookdaynum").val();
            var comid = $("#hid_comid").val();

            if (roomtypeid != '0') {
                $.post("/JsonFactory/ProductHandler.ashx?oper=GetRoomType", { roomtypeid: roomtypeid, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        //                        $.prompt("获取房型有误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#lblname").text(data.msg.Name);
                        $.post("/JsonFactory/ProductHandler.ashx?oper=GetRoomTypeDayList", { roomtypeid: roomtypeid }, function (data1) {
                            data1 = eval("(" + data1 + ")");
                            if (data1.type == 1) {
                                //                                $.prompt("获取房型每日房况有误");
                                return;
                            }
                            if (data1.type == 100) {
                                var totalprice = 0;
                                for (var j = 0; j < data1.msg.length; j++) {
                                    var dayprice = parseInt(data1.msg[j].Dayprice);
                                    totalprice += dayprice;
                                }
                                $("#totalPrice").text(totalprice);
                            }
                        })
                    }
                })
            }

            $("#roomcount").change(function () {
                var roomcount = $("#roomcount").val();
                $("#booknum").text(roomcount + "间");

                $("#totalPrice").text(parseInt(roomcount) * parseInt($("#totalPrice").text()));
            })

            $("#cometime").change(function () {

                $("#cometime1").text($("#cometime").find("option:selected").text());
            })
            $("#submitBtn").click(function () {

                var name = $("#name").val();
                var phone = $("#phone").val();
                if (name == null || name == "") {
                    //                    $(".nameTip").html("请填写预订人");
                    //                    $(".nameTip").css('color', 'red');
                    alert("请填写预订人");
                    return;
                }
                if (phone == null || phone == "") {
                    //                    $(".phoneTip").html("请填写手机号");
                    //                    $(".phoneTip").css('color', 'red');
                    alert("请填写手机号");
                    return;
                }
                if (isMobel(phone) == false) {
                    //                    $(".phoneTip").html("手机格式错误");
                    //                    $(".phoneTip").css('color', 'red');
                    alert("手机格式错误");
                    return;
                }


                var title = $("#lblname").text();
                var number = $("#roomcount").val();
                var resdate = checkindate;
                var totalprice = 0;
                $.post("/JsonFactory/OrderHandler.ashx?oper=Reservation_edit", { comid: comid, name: name, phone: phone, title: title, number: number, wxmaterialid: 0, checkindate: checkindate, checkoutdate: checkoutdate, roomtypeid: roomtypeid, totalprice: totalprice, lastercheckintime: $("#cometime").find("option:selected").text() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        return;
                    }
                    else {
                        alert("预定成功，稍后酒店客服人员会联系你");
                        return;
                    }
                })

            })
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <header class="header">
      <h1>填写订单</h1>
      <div class="left-head">
              <a id="goBack" href="javascript:history.go(-1);" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
      </div>
      <div class="right-head">
        <a href="" class="head-btn fn-hide"><span class="inset_shadow"><span class="head-home"></span></span></a>
      </div>
    </header>
   <div class="wrap write">
        <div class="t-head">
            <div class="t-date">
                <span>入住<%=checkindate%></span> <span>离店<%=checkoutdate%></span> <span class="t-right">
                    共<%=bookdaynum%>晚</span>
            </div>
            <div class="order-price" id="priceInfo" style="display: none;">
                订单总额：¥<span id="totalPrice"></span>
            </div>
        </div>
        <div class="order-write">
            <dl class="fn-clear" id="roomCount">
                <dt><span class="o-icon room-icon"></span>
                    <label id="lblname">
                    </label>
                </dt>
                <dd>
                    <span class="o-select" id="booknum">1间</span> <span class="arrow-icon"></span>
                </dd>
                <select id="roomcount">
                    <option value="1" selected="selected">1间</option>
                    <option value="2">2间</option>
                    <option value="3">3间</option>
                    <option value="4">4间</option>
                    <option value="5">5间</option>
                    <option value="6">6间</option>
                    <option value="7">7间</option>
                    <option value="8">8间</option>
                    <option value="9">9间</option>
                    <option value="10">10间</option>
                </select>
            </dl>
            <dl class="fn-clear" id="Retention">
                <dt><span class="o-icon click-icon"></span>最晚到店时间</dt>
                <dd>
                    <span class="o-select" id="cometime1">次日00:00之前</span> <span class="arrow-icon">
                    </span>
                </dd>
                <select id="cometime">
                    <option value="1">次日00:00之前</option>
                    <option value="2">次日00:00之后(需担保)</option>
                </select>
            </dl>
            <dl class="fn-clear">
                <dt><span class="o-icon per-icon"></span>入住人</dt>
                <dd>
                    <input id="name" placeholder="姓名" value="" type="text"><span class="nameTip"></span>
                </dd>
            </dl>
            <dl class="fn-clear">
                <dt><span class="o-icon mob-icon"></span>手机号</dt>
                <dd>
                    <input id="phone" placeholder="用于接收确认短信" value="" maxlength="11" type="tel"><span
                        class="phoneTip"></span></dd>
            </dl>
        </div>
        <div style="display: block;" class="o-tip fn-hide" id="isDanBao">
            <dl class="fn-clear">
                <dt></dt>
                <dd>
                    如果<%=checkindate %>次日0:00之前不能到酒店，请您及时联系酒店，以免房间被过时取消。酒店通常下午2点开始办理入住，早到可能需要稍作等待。</dd>
            </dl>
        </div>
        <div class="o-append" id="return">
            <span class="o-bg"></span>
        </div>
    </div>
    <div class="wrap qtips_div" id="tips">
        <p>
        </p>
        <span class="attr"></span>
    </div>
    <div class="order-btn">
        <input id="submitBtn" value="提交订单" type="button"></div>
    <input type="hidden" id="hid_checkindate" value="<%=checkindate %>" />
    <input type="hidden" id="hid_checkoutdate" value="<%=checkoutdate %>" />
    <input type="hidden" id="hid_bookdaynum" value="<%=bookdaynum %>" />
    <input type="hidden" id="hid_roomtypeid" value="<%=roomtypeid %>" />
    <input type="hidden" id="hid_comid" value="<%=comid %>" />
</asp:Content>
