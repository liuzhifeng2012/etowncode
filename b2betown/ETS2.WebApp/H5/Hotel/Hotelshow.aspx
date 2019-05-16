<%@ Page Title="" Language="C#" MasterPageFile="~/H5/Order.Master" AutoEventWireup="true"
    CodeBehind="Hotelshow.aspx.cs" Inherits="ETS2.WebApp.H5.Hotelshow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Hotel/StyleSheet1.css" rel="stylesheet" type="text/css" />
    <!-- 页面样式表 -->
    <style type="text/css">
        .none
        {
            display: none;
        }
        .block
        {
            display:block;
        }
        .margin
        {
            margin-top: 50px;
        }
        .scenery-conment a, a:hover
        {
            color: #434e5a;
        }
        input:-moz-placeholder
        {
            color: #DDE;
        }
        input::-webkit-input-placeholder
        {
            color: #DDE;
        }.qb_icon{display:inline-block;
              background-repeat:no-repeat;
              background-size:100%}
              .qb_clearfix:after
              {
                  clear:both;content:".";
                  display:block;height:0;
                  visibility:hidden}
                  .qb_mb10{margin-bottom:10px}.qb_mb20{margin-bottom:20px}
                  .qb_mr10{margin-right:10px}.qb_pt10{padding-top:10px}
                  .qb_fs_s{font-size:12px}.qb_fs_m,body{font-size:14px}
                  .qb_fs_l{font-size:15px}.qb_fs_xl{font-size:17px}
                  .mod_btn{font-size:34px}.qb_fr{float:right}
                  .qb_gap{padding-left:10px;padding-right:10px;margin-bottom:10px}
                  .qb_none{display:none!important}.qb_tac{text-align:center}
                  .qb_tar{text-align:right}.qb_flex{display:-webkit-box}
                  .qb_flex .flex_box{-webkit-box-flex:1;display:block}
                  .qb_hr{border:0;border-top:1px solid #eeeff0;
                         border-bottom:1px solid #FFF;margin:20px 0;clear:both}
                         .qb_quick_tip{position:fixed;height:23px;padding:3px 5px;background:rgba(0,0,0,.8);
                                       color:#FFF;border-radius:5px;text-align:center;
                                       z-index:202;top:5px;left:10px;right:10px}
       .icon_checkbox,.pg_upgrade_title li,.mod_input .x
       {
       background-image:url('/Images/sprite_upgrade.png');
       background-size:25px;background-repeat:no-repeat}
       .pg_upgrade_title li{background-position:-8px -169px;padding-left:30px}
       .pg_upgrade_content .icon_checkbox
       {
           background-position:0 -40px;
           width:25px;height:25px;
           vertical-align:-8px;margin-right:5px}
           .pg_upgrade_content .icon_checkbox.checked{background-position:0 0
    
       }
       .mod_input
          {
              background-color:#FFF;border:1px solid #c5c5c5;
              border-radius:5px;padding:2px 10px;line-height:37px;
              box-shadow:0 0 2px rgba(0,0,0,.2) inset;position:relative}
       .qb_mb10{margin-bottom:10px}
       .pg_upgrade_content .icon_checkbox
       {
           background-position:0 -40px;
           width:25px;height:25px;
           vertical-align:-8px;margin-right:5px}
           .pg_upgrade_content .icon_checkbox.checked{background-position:0 0
    
       }
       .qb_gap{padding-left:10px;padding-right:10px;margin-bottom:10px}
       .ico-right {
    width: 9px;
    height: 13px;
    overflow: hidden;
    position: absolute;
    bottom: 0px;
    right: 2px;
    background: url('../../Images/icon_you_com.png') no-repeat scroll 50% 50% / 9px 13px transparent;
    transition: transform 0.1s ease-in 0s;
    transform: rotate(90deg);
    transform-origin: 50% 50% 0px;
    
}
.roomitem dt {
    padding-top: 15px;
    font-weight:bold:
}
.price
{
     padding-top: 15px;
    }
    
    #box1 {
 position: absolute;
 top: 20px;
 left: 1px;
 width: 50px;
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
    <!-- 页面样式表 -->
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="../../Styles/Hotel/wabapp.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/H5/list.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="../../Styles/H5/Order.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1;
        $(function () {

            //            $("#list .list-item").clock(function () {
            //                $("#list").css("background", "#e9eff5");
            //            })

            var comid = $("#hid_comid").val();
            //获得有效的酒店房期
            $.post("/JsonFactory/ProductHandler.ashx?oper=GetMinValidByProid", { comid: comid, proid: $("#hid_proid").val() }, function (data) {
                data = eval("(" + data + ")");
                $("#indate").html(data.mindate);
                $("#outdate").html(data.mindate_next);
            })



            $("#sInDate").click(function () {
                scrollTo(0, 1);
                thisMonth();
                $("#orderFome").removeClass().addClass("translate3d");
                $("#calDiv").fadeIn();
                $("footer").hide();
            });
            $("#sOutDate").click(function () {
                scrollTo(0, 1);
                thisMonth1();
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


           

            //显示更多，则是显示全部
            $("#showmore").click(function () {
                $("#loading").show();
                //                var pageSize = parseInt($("#num").val()) + 3;

                if ($("#showmore_span").html() == "收起更多优惠房型") {
                    SearchList(pageindex, 0, $("#hid_proid").val(), $("#indate").html(), $("#outdate").html());
                } else {
                    SearchList(pageindex, 0, $("#hid_proid").val(), $("#indate").html(), $("#outdate").html());
                }

                //                $("#num").val(pageSize);
            })

            SearchList(pageindex, 0, $("#hid_proid").val(), "<%=mindate %>", "<%=mindate_out %>");
        });

        


        //装载产品列表
        function SearchList(pageindex, pageSize, proid, indate, outdate) {

            var comid = $("#hid_comid").val();
            $("#loading").show();
            $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=Hotelpagelist",
                data: {projectid:$("#hid_projectid").val(), comid: comid, pageindex: pageindex, pagesize: pageSize,startdate:indate,enddate:outdate,proid:proid },
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
//                                               }
                            if (pageSize==0)//0展开操作；大于0 收起操作
                            {
                                $("#showmore_span").html("收起更多优惠房型");
                            }else{
                                $("#showmore_span").html("展开更多优惠房型");
                            }

                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#roomList");
                            setpage(data.totalCount, pageSize, pageindex,proid,indate,outdate);
                        }


                    }
                }
            })
        }
        //分页
        function setpage(newcount, newpagesize, curpage,proid,indate,outdate) {
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

                    SearchList(page, newpagesize,proid,indate,outdate);

                    return false;
                }
            });
        }
        function SubstrDome(s, num) {
            var ss;
            if (s.length > num) {
                ss = s.substr(0, num) + "..";
                return (ss);
            }
            else {
                return (s);
            }

        }
        function csss(id) {
            $("#loading").show();
            location.href = "OrderEnter.aspx?id=" + id;
        }

        function showErr(a) {
            $("html").css({
                "overflow-y": "hidden"
            });
            if ($("#bgDiv").html() == null) {
                $('<div id="bgDiv"></div>').appendTo("body")
            }
            if ($("#showMsg").html() != null) {
                $("#showMsg").remove()
            }
            $('<div id="showMsg"><div class="msg-title">\u6e29\u99a8\u63d0\u793a</div><div class="msg-content">' + a + '</div><div class="msg-btn"><a href="javascript:;" onclick="hideErr()">\u77e5\u9053\u4e86</a></div></div>').appendTo("body");
            var b = $(window).height();
            var d = $(window).scrollTop();
            var c = $("#showMsg").height();
            $("#bgDiv").css({
                height: $(document).innerHeight()
            }).show();
            $("#showMsg").css({
                top: (b - c) / 2
            }).show()
        }
        function hideErr() {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #showMsg").hide()
        }
        function getdatetime(id) {
           var indate=$("#indate").html();
           var outdate = $("#outdate").html();
           var comid = $("#hid_comid").val();

           
           if (DateDiff(outdate,indate)>0) {

           } else {
               alert("离店日期必须大于入住日期");
               return;
           }
           location.href = "HotEnter.aspx?id=" + id + "&indate=" + indate + "&outdate=" + outdate + "&comid=" + comid+"&uid=<%=uid %>";
       }
       function getproshow(id) {
           if ($("#roomitem" + id + "").css("display") == "none") {
               $("#roomitem" + id + "").css('display', 'block');
           } else {
               $("#roomitem" + id + "").css('display', 'none');
           }
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="inner">
    <div id="orderFome" class="order-form">
   


      <div class="body">
       <div id="box1" onclick="javascript:history.go(-1)" ><span class="head-return"></span></div>
          <img src="<%=projectimgurl %>" alt="<%=projectname %>" title="<%=projectname %>" />
       </div>
       <div id="box2"><%=projectname %></div>
       
        <%--<div class="bottomBox">
          <a href="#"><span class="map"></span>地图查询地址<em class="gray-ico"></em></a>   </div> --%>


    <!-- 页面内容块 -->
    <div class="wrap roomdetail">
        <ul class="hoteldate fn-clear">
            <li   id="sInDate">
                <label id="indate"><%=mindate%></label></li>
            <li id="sOutDate">
                <label id="outdate"><%=mindate_out%></label></li>
        </ul>

        <div id="selectroomList">
            
        </div>
        <div id="roomList">
            
        </div><%--<em>¥118</em><span class="gift">礼</span>--%>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit">
            <dl class="roomitem fn-clear">
                <dt onclick="javascript:getproshow('${ID}')">
                    <h5>
                        <strong>${Name}</strong></h5>
                    <div class="append">
                        
                    </div>
                </dt>
                 {{if NowdayPrice!=0}}
                     <dd onclick="javascript:getdatetime('${ID}')">
                  {{else}}
                  <dd>
                  {{/if}}
                
                    <div class="price">
                    {{if NowdayPrice!=0}}
                       <strong>¥${NowdayPrice}</strong>
                        {{else}}
                        <strong>暂无报价</strong>
                        {{/if}}
                         <span>>></span>
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
                       
        <div style="display: block;" class="showmore fn-hide" id="showmore">
            <span id="showmore_span" style="display: none;" ></span></div>
    </div>
    
    <div class="wrap nearhotel">
        <%--<a href="#" onclick="_hmt.push(['_trackEvent', 'new-hotelnearby', 'click', 'touch']);">
            周边酒店</a>--%>
    </div>
    <footer>
          <div class="footer_link c_right" style=" margin:0px 0 0 0; text-align:center">
                 <span style="display:block; padding-bottom:5px;  line-height:20px;">服务热线：<a href="tel:<%=phone %>"><%=phone %></a></span>
          </div>
    </footer>
    </div>
    <div id="calDiv" style="display: none; margin-top: -40px">
        <header class="header"  style=" background-color: #3CAFDC;">
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
    </div>
    <div id="calDiv1" style="display: none; margin-top:0px">
        <header class="header" style=" background-color: #3CAFDC;">
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
        <%--<div class="sel-tip">
            <h4 style="text-align: left">
                温馨提示：</h4>
            <p>
                1、<span class="f60">如需预订，您最晚要在游玩当天20:30前下单，请尽早预订。每人最多限购5张。</span><br />
                2、查看游玩日期及景点门票的价格，若需预订，请直接选中对应的日期即可。</p>
        </div>--%>
        <ul id="holidayList1">
        </ul>
    </div>

    <div style="height: 565px; display: none;" id="bgDiv">
    </div>
    <div id="showMsg" style="top: 352px; display: none;">
        <div class="msg-title">
            温馨提示</div>
        <div class="msg-content">
            请填写取票人姓名！</div>
        <div class="msg-btn">
            <a href="javascript:;" onclick="hideErr()">知道了</a></div>
    </div>
    <input id="room" type="hidden" value="0"/>
    <input id="zhan" type="hidden" value="0"/>
    <input id="hid_comid" type="hidden" value="<%=comid %>" />
    <input id="num" type="hidden" value="3" />
    <input id="Hidden1" type="hidden" value="<%=comidture %>" />
    <input id="hid_openid" type="hidden" value="www" />
    <input type="hidden" id="nowdate" value="<%=nowdate %>" />
    <input type="hidden" id="enddate" value="<%=enddate %>" />

    <input type="hidden" id="hid_proid" value="<%=proid %>" />

    <input type="hidden" id="hid_projectid" value="<%=projectid %>" />

    <script src="/Scripts/JSmCal.js" type="text/javascript"></script>
    <link href="../../Scripts/mCal.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JSmCal1.js" type="text/javascript"></script>
    <link href="../../Scripts/mCal1.css" rel="stylesheet" type="text/css" />
    <!--Baidu-->
    <%--<script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "//hm.baidu.com/hm.js?8fcd06cc927f3554397ca18509561b69";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>--%>
</asp:Content>
