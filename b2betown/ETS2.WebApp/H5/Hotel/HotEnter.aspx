<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true"
    CodeBehind="HotEnter.aspx.cs" Inherits="ETS2.WebApp.H5.HotEnter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/Hotel/orderWrite_com.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <style>
        #box1 {
 position: absolute;
 top: 20px;
 left: 1px;
 width: 50px;
}
img
 {
     max-width: 100%;
     }

    #box2 {
position:absolute;
 top: 1px;
 left: 1px;
 width: 100%;
 background-color: #ffffff;
 filter:alpha(Opacity=60);-moz-opacity:0.6;opacity: 0.6;
 text-align:center;
}
    </style>
    <script type="text/javascript">
        $(function () {
            var proid = $("#hid_proid").val();
            var checkindate = $("#hid_checkindate").val();
            var checkoutdate = $("#hid_checkoutdate").val();
            var bookdaynum = $("#hid_bookdaynum").val();
            var comid = $("#hid_comid").val();

            if (proid != '0') {
                $.post("/JsonFactory/ProductHandler.ashx?oper=GetHouseType", { proid: proid, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        //                        $.prompt("获取房型有误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#lblname").text(data.msg.Proname);
                        $("#hid_ReserveType").val(data.msg.ReserveType);
                        $.post("/JsonFactory/ProductHandler.ashx?oper=GetHouseTypeDayList", {proid:data.msg.Proid,startdate:checkindate,enddate:checkoutdate}, function (data1) {
                            data1 = eval("(" + data1 + ")");
                            if (data1.type == 1) {
                                //                                $.prompt("获取房型每日房况有误");
                                return;
                            }
                            if (data1.type == 100) {
                                var totalprice = 0;
                                for (var j = 0; j < data1.msg.length; j++) {
                                    var dayprice = parseInt(data1.msg[j].Menprice);
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
             $("#loading").show();
                var name = $("#name").val();
                var phone = $("#phone").val();
                var number = $("#roomcount").val();

                if( parseInt($("#MinEmptynum").val()) < parseInt(number)){
                    alert("您预定数量大于剩余房间数量，现有房间"+ $("#MinEmptynum").val()+ "间");
                     $("#loading").hide();
                    return;
                }


                if (name == null || name == "") {
                    //                    $(".nameTip").html("请填写预订人");
                    //                    $(".nameTip").css('color', 'red');
                    alert("请填写预订人");
                     $("#loading").hide();
                    return;
                }
                if (phone == null || phone == "") {
                    //                    $(".phoneTip").html("请填写手机号");
                    //                    $(".phoneTip").css('color', 'red');
                    alert("请填写手机号");
                     $("#loading").hide();
                    return;
                }
                if (isMobel(phone) == false) {
                    //                    $(".phoneTip").html("手机格式错误");
                    //                    $(".phoneTip").css('color', 'red');
                    alert("手机格式错误");
                     $("#loading").hide();
                    return;
                }


                var title = $("#lblname").text();
                

                var resdate = checkindate;
                var totalprice = 0;

                 //提交预订
                $.post("/JsonFactory/OrderHandler.ashx?oper=createorder", { openid: '<%=openid %>', proid: proid, ordertype: <%=ordertype %>, payprice: '<%=singleroom_totalprice %>', u_num:number , u_name: name, u_phone: phone, u_traveldate: '<%=DateTime.Now.ToString("yyyy-MM-dd") %>', comid: comid, buyuid: <%=buyuid %>,tocomid:<%=tocomid %>,start_date:checkindate,end_date:checkoutdate,bookdaynum:<%=bookdaynum %>,lastarrivaltime:$("#cometime").find("option:selected").text(),fangtai:'<%=fangtai %>'}, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#loading").hide();
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                          $("#loading").hide();
                        if (data.dikou != "") {
                            location.href = "backUrl.aspx?orderid=" + data.msg + " &comid=" + comid + " &dikou=" + data.dikou;
                            return;
                        }
                     
                       location.href = "/h5/pay.aspx?orderid=" + data.msg + "&comid=" + comid;
                       return;

                    }

                })

            })

             SearchList(1, 3, '<%=proid %>', "<%=checkindate %>", "<%=checkoutdate %>");
        })


         //装载产品列表
        function SearchList(pageindex, pageSize, proid, indate, outdate) {

            var comid = $("#hid_comid").val();
           
            $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=Hotelpagelist",
                data: {projectid:$("#hid_projectid").val(), comid: comid, pageindex: pageindex, pagesize: pageSize,startdate:indate,enddate:outdate,proid:proid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        alert("您预定的日期已满房，请选择其他日期或房型！");
                        history.go(-1);
                    }
                    if (data.type == 100) {

                       $("#MinEmptynum").val(data.msg[0].MinEmptynum);




                        if(data.msg[0].MinEmptynum>0 && data.msg[0].allprice>0){
                        
                        }else{
                          alert("您预定的日期已满房，请选择其他日期或房型！");
                          history.go(-1);
                        }
                    }
                }
            })
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div class="body">
       <div id="box1" onclick="javascript:history.go(-1)" ><span class="head-return"></span></div>
          <img src="<%=projectimgurl %>" alt="<%=projectname %>" title="<%=projectname %>" />
       </div>
       <div id="box2"><%=projectname %></div>
    <div class="wrap write">
        <div class="t-head">
            <div class="t-date">
                <span>入住<%=checkindate%></span> <span>离店<%=checkoutdate%></span> <span class="t-right">
                    共<%=bookdaynum%>晚</span>
            </div>
            <div class="order-price" id="priceInfo" style="display: none">
                订单总额：¥<span id="totalPrice"></span>
            </div>
        </div>
        <div class="order-write">
            <dl class="fn-clear">
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
                    <span class="o-select" id="cometime1">当日20:00之前</span> <span class="arrow-icon">
                    </span>
                </dd>
                <select id="cometime">
                    <option value="1">当日20:00之前</option>
                    <option value="2">当日20:00之后(需担保)</option>
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
                    如果<%=checkindate %>当日20:00之前不能到酒店，请您及时联系酒店，以免房间被过时取消。酒店通常下午2点开始办理入住，早到可能需要稍作等待。</dd>
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
    <input type="hidden" id="MinEmptynum" value="0" />
    
    <input type="hidden" id="hid_proid" value="<%=proid %>" />
    <input type="hidden" id="hid_comid" value="<%=comid %>" />
    <input type="hidden" id="hid_ReserveType" value="" />
</asp:Content>
