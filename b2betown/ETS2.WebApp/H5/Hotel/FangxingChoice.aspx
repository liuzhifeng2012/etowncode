<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Hotel/hotel.Master" AutoEventWireup="true"
    CodeBehind="FangxingChoice.aspx.cs" Inherits="ETS2.WebApp.H5.Hotel.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--页面样式--%>
    <link href="../../Styles/Hotel2/base.2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Hotel2/hotelnewdeatil.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var pageindex = 1;
        $(function () {

            $("#indate").html($("#nowdate").val());
            $("#outdate").html($("#enddate").val());

            $("#sInDate").click(function () {
                scrollTo(0, 1);
                thisMonth("traveldate");
                $("#orderFome").removeClass().addClass("translate3d");
                $("#calDiv").fadeIn(); $("#inner").css("display", "none");
                $("footer").hide();
            });
            $("#sOutDate").click(function () {
                scrollTo(0, 1);
                thisMonth1("traveldate");
                $("#orderFome").removeClass().addClass("translate3d");
                $("#calDiv1").fadeIn(); $("#inner").css("display", "none");
                $("footer").hide();
            });


            $("#goBacktime").click(function () {
                $("#orderFome").removeClass("translate3d");
                $("#calDiv").hide();
                $("#bgDiv").css("display", "none");
                $("#inner").show();
                $("footer").show();
                //$("#sInDate label").html("请选择时间");
            })

            $("#goBacktime1").click(function () {
                $("#orderFome").removeClass("translate3d");
                $("#calDiv1").hide();
                $("#bgDiv1").css("display", "none");
                $("#inner").show();
                $("footer").show();
                //$("#sOutDate label").html("请选择时间");
            })

            var comid = $("#hid_comid").val();
            SearchList(pageindex, 3);
            //装载产品列表
            function SearchList(pageindex, pageSize) {
                if (pageindex == '') {
                    $("#more").html("");
                    return;
                }
                $("#loading").show();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Hotelpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#list").hide();
                            $("#black_top").hide();
                            $("#page").hide();
                            $("#page1").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">努力加载中……</div>");
                            return;
                        }
                        if (data.type == 100) {
                            $("#loading").hide();
                            $("#roomList").empty();
                            if (data.totalCount == 0) {
                                $("#roomList").html("<div style=\" font-size:16px; color:#fff; text-align:center; vertical-align:middle; font-weight:bold\">查询数据为空</div>");
                            } else {
                                if (data.totalCount - pageindex * pageSize > 0) {
                                    $("#more").removeClass("return");
                                    $("#more").html("展开更多优惠房型"); //(" + (data.totalCount - pageindex * pageSize) + ")
                                } else {
                                    $("#more").html("收起更多优惠房型");
                                    $("#more").addClass("return");
                                    $("#hidnum").val(0);
                                }

                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#roomList");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })
            }

            $("#more").click(function () {
                $("#loading").show();
                if ($("#hidnum").val() == 0) {
                    SearchList(1, 3);
                    $("#hidnum").val(1)
                    return;
                }
                var pageSize = parseInt($("#num").val()) + 3;
                SearchList(pageindex, pageSize);
                $("#num").val(pageSize);
            })

        });
        //分页
        function setpage(newcount, newpagesize, curpage) {
            $("#divPage").paginate({
                count: Math.ceil(newcount / newpagesize),
                start: curpage,
                display: 5,
                border: false,
                text_color: '#888',
                background_color: '#EEE',
                text_hover_color: 'black',
                images: false,
                rotate: false,
                mouse: 'press',
                onChange: function (page) {

                    SearchList(page, newpagesize);

                    return false;
                }
            });
        }

        function getdatetime(id) {
            var indate = $("#indate").html();
            var outdate = $("#outdate").html();
            var comid = $("#hid_comid").val();
            location.href = "HotelSubmit.aspx?id=" + id + "&indate=" + indate + "&outdate=" + outdate + "&comid=" + comid;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="inner">
    <header class="header">
      <h1><%=comName%></h1>
      <div class="right-head">
        <a href="" class="head-btn fn-hide"><span class="inset_shadow"><span class="head-home"></span></span></a>
      </div>
    </header>
    
    <div id="page1">
        <div class="wrap detail">
            <dl class="fn-clear">
                <dt>
                 <a href="HotelIntroduction.aspx?id=<%=comid %>">
                  <img src="<%=headPortraitImgSrc %>" alt="<%=comName %>" title="<%=comName %>" />
                 </a>
                </dt>
                <dd>
                    <a　style="color:#000;" href="HotelIntroduction.aspx?id=<%=comid %>">
                        <label>
                           <h2><%=comName%></h2>
                        </label>
                    </a>
                </dd>
                <dd class="common">
                    <a style=" color:#000; font-size:13px;" href="HotelIntroduction.aspx?id=<%=comid %>">
                        <%=remark%>
                    </a>
                 </dd>
            </dl>
            <div class="map" onclick="">
               <%-- 地址：  <span><%=address%></span>--%>
               <label>
                   <%=address%>
               </label>
            </div>
        </div>
        <div class="tip fn-hide" id="tip">
            
        </div>
        <div class="wrap roomdetail">
            <ul class="hoteldate fn-clear">
            <li   id="sInDate">
                <label id="indate">
                    </label></li>
            <li id="sOutDate">
                <label id="outdate">
                    </label></li>
        </ul>
            <div id="roomList">
                   
            </div>
            <div class="showmore fn-hide return"id="more" style="display: block;">
                <span id="showmore_span"></span>
            </div>
            <script type="text/x-jquery-tmpl" id="ProductItemEdit">
            <dl class="roomitem fn-clear">
                <dt>
                    <h5>
                        <strong>${Name}</strong></h5>
                    <div class="append">
                        
                    </div>
                </dt>
                <dd>
                    <div class="price">
                        <strong>¥${NowdayPrice}</strong> <span></span>
                    </div>
                    <div class="btn">
                    {{if dayavailablenum!=0}} <a onclick="javascript:getdatetime('${ID}')">预订</a>{{/if}}
                    {{if dayavailablenum==0}} <a id="no-room">预订</a>{{/if}}
                    </div>
                </dd>
            </dl>
            <div class="roominfo fn-hide" style=" display:none" id="roomitem${ID}">
                <dl class="detailmore fn-clear">
                    <dt>
                        <img src="${Roomtypeimge}" /></dt>
                    <dd class="fn-clear">
                        <span class="half">早餐：{{if Breakfast==1}}无早{{/if}}{{if Breakfast==2}}单早{{/if}}{{if Breakfast==3}}双早{{/if}}</span>
                        <span class="half">加床:{{if Whetherextrabed==true}}可以{{/if}}{{if Whetherextrabed==false}}不可{{/if}}(加床费用：${extrabedprice}/床/间夜)</span>
                        <span class="half">最大入住人数：${largestguestnum}人</span>
                        <span class="half">床型：${bedwidth}</span>
                        <span class="half">宽带：${wifi}</span>
                        
                    </dd>
                </dl>
            </div>
    </script>
        </div>
    </div>
    <div id="page2" class="fn-hide">
        <div id="calendarDiv">
        </div>
        <p class="calendar-info"></p>
    </div>
    </div>
    <div id="calDiv" style="display: none; margin-top: 0px">
        <header class="header">
         <h1>选择入住日期</h1>
        <div class="left-head">
          <a id="goBacktime" href="javascript:history.go(-1);" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
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
        
        <ul id="holidayList"></ul>
    </div>
    <div id="calDiv1" style="display: none; margin-top:0px">
        <header class="header">
                    <h1>选择离店日期</h1>
        <div class="left-head">
          <a id="goBacktime1" href="javascript:history.go(-1);" class="tc_back head-btn"><span class="inset_shadow"><span class="head-return"></span></span></a>
        </div>
    </header>
        <div class="low_calendar">
            <div class="low_calendar_top">
                <a class="top_left" href="javascript:;" id="last_mon1"><span class="last_mon"></span>
                </a><span class="top_middle"><span id="dateInfo1"></span></span><a class="top_right"
                    href="javascript:;" id="next_mon1"><span class="next_mon"></span></a>
            </div>
            <div id="calendar1">
            </div>
        </div>
        <ul id="holidayList1">
        </ul>
    </div>

    <div style="height: 565px; display: none;" id="bgDiv">
    </div>

    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="num" type="hidden" value="3" />
    <input id="hid_openid" type="hidden" value="www" />
    <input id="hidnum" type="hidden" value="1" />
    <input type="hidden" id="nowdate" value="<%=nowdate %>" />
    <input type="hidden" id="enddate" value="<%=enddate %>" />

    <script src="/Scripts/JSmCal.js" type="text/javascript"></script>
    <link href="../../Scripts/mCal.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JSmCal1.js" type="text/javascript"></script>
    <link href="../../Scripts/mCal1.css" rel="stylesheet" type="text/css" />
</asp:Content>
