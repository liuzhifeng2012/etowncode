<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mTravelbusOrderCount.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.mTravelbusOrderCount" MasterPageFile="/UI/pmui/ETicket/mEtown.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8">
    <meta name="keywords" content="微商城">
    <meta name="description" content="">
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="cleartype" content="on">
    <title></title>
    <!-- meta viewport -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!-- CSS -->
    <link rel="stylesheet" href="/agent/m/css/cart.css">
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <link href="/agent/m/css/morder.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <!--电脑端手机端兼容日历 js css-->
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.core-2.5.2.js"
        type="text/javascript"></script>
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.core-2.5.2-zh.js"
        type="text/javascript"></script>
    <link href="http://shop.etown.cn/Scripts/mobiscroll/dev/css/mobiscroll.core-2.5.2.css"
        rel="stylesheet" type="text/css" />
    <link href="http://shop.etown.cn/Scripts/mobiscroll/dev/css/mobiscroll.animation-2.5.2.css"
        rel="stylesheet" type="text/css" />
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.datetime-2.5.1.js"
        type="text/javascript"></script>
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.datetime-2.5.1-zh.js"
        type="text/javascript"></script>
    <!-- S 可根据自己喜好引入样式风格文件 -->
    <script src="http://shop.etown.cn/Scripts/mobiscroll/dev/js/mobiscroll.android-ics-2.5.2.js"
        type="text/javascript"></script>
    <link href="http://shop.etown.cn/Scripts/mobiscroll/dev/css/mobiscroll.android-ics-2.5.2.css"
        rel="stylesheet" type="text/css" />
    <!-- E 可根据自己喜好引入样式风格文件 -->
    <script type="text/javascript">
        $(function () {
			var currYear = (new Date()).getFullYear();	
			var opt={};
			opt.date = {preset : 'date'};
			//opt.datetime = { preset : 'datetime', minDate: new Date(2012,3,10,9,22), maxDate: new Date(2014,7,30,15,44), stepMinute: 5  };
			opt.datetime = {preset : 'datetime'};
			opt.time = {preset : 'time'};
			opt.default = {
				theme: 'android-ics light', //皮肤样式
		        display: 'modal', //显示方式 
		        mode: 'scroller', //日期选择模式
				lang:'zh',
		        startYear:currYear - 10, //开始年份
		        endYear:currYear + 10 //结束年份
			};

			$("#datetime").val('').scroller('destroy').scroller($.extend(opt['date'], opt['default']));
            $("#datetime").click();

        });
    </script>
    <!--电脑端手机端兼容日历 js css-->
    <script type="text/javascript">
        $(function () {
            $("#h_comname").text($("#hid_comname").trimVal());

            $("#search_botton").click(function () {

                var startdate = $("#datetime").trimVal();
                var enddate = $("#datetime").trimVal();

                SearchList(startdate, enddate, 10, $("#hid_comid").trimVal());
            })

            var startdate = '<%=this.nowdate %>';
            var enddate = '<%=this.nextdate %>';

            SearchList(startdate, enddate, 10, $("#hid_comid").trimVal());
        })

        function SearchList(startdate, enddate, servertype, comid) {
            var day = DateDiff(enddate, startdate);
            if (parseInt(day) < 0) {
                alert("起始时间不可大于结束时间");
                return;
            }

            if (parseInt(day) > 6) {
                alert("查询区间不可大于1星期");
                return;
            }

            $.post("/jsonfactory/orderhandler.ashx?oper=travelbusordercountbyday", { startdate: startdate, enddate: enddate, servertype: servertype, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#list").html("");
                    var str1 = "";
                    for (var i = 0; i < data.msg.length; i++) {

                        if (parseInt(data.msg[i].paysucnum) > 0) {
                            str1 += '<div class="layout-box" >' +
                        '<ul class="list-a">' +
                            '<li id="db_li' + i + '"  onclick="viewdetail(' + i + ',\'' + data.msg[i].daydate + '\')">' + data.msg[i].daydate + '(支付:' + data.msg[i].paysucnum + '人)' +
                              '<label style="float:right;" id="db_label' + i + '">点击展开</label>' +
                              '<input type="hidden" id="tr_hid_' + i + '" value="0" />' +
                            '</li>' +
                        '</ul>' +
                    '</div>';
                        } else {
                            str1 += '<div class="layout-box" >' +
                        '<ul class="list-a">' +
                            '<li>' + data.msg[i].daydate + '(支付:' + data.msg[i].paysucnum + '人)' +
                              '<label style="float:right;" id="db_label' + i + '">无需处理</label>' +
                              '<input type="hidden" id="tr_hid_' + i + '" value="0" />' +
                            '</li>' +
                        '</ul>' +
                    '</div>';
                        }
                    }
                    $("#list").html(str1);

                    if (parseInt(data.msg[0].paysucnum) > 0) {
                        viewdetail(0, startdate);
                    }
                }
            })
        }
        function viewdetail(index, daydate) {
            if ($("#tr_hid_" + index).trimVal() == 0) {
                $("#tr_hid_" + index).val(1);
                $("#db_label" + index).text("点击关闭");

                $.post("/jsonfactory/orderhandler.ashx?oper=Getb2bcomprobytraveldate", { daydate: daydate, comid: $("#hid_comid").trimVal(), servertype: 10 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1)
                    { }
                    if (data.type == 100) {
                        var contentstr = '';
                        for (var i = 0; i < data.msg.length; i++) {
                            if (data.msg[i].paysucbooknum > 0) {
                                contentstr += '<li id="db_childer_li' + i + '" style="background-color: #eee; padding:10px;"><a  href="mTravelbusOrderVisitorlist.aspx?daydate=' + daydate + '&proid=' + data.msg[i].Proid + '&oper=jietuansuc">' +
                                             data.msg[i].Proname + '(支付:' + data.msg[i].paysucbooknum + '人)';
                                if (data.msg[i].ishasplanbus == 1) {
                                    if (data.msg[i].paysucbooknum == 0) {
                                        contentstr += '<label style="float:right;">无需处理</label>';
                                    }
                                    else {
                                        // contentstr += '';
                                    }
                                }
                                else {

                                    if (data.msg[i].paysucbooknum == 0) {
                                        contentstr += '<label  style="float:right;">无需处理</label>';
                                    }
                                    else {
                                        //contentstr += '<a  style="float:right;" href="mTravelbusOrderVisitorlist.aspx?daydate=' + daydate + '&proid=' + data.msg[i].Proid + '&oper=paysuc">查看名单</a>';
                                    }
                                }
                                contentstr += '</a></li>';
                            } else {
                                contentstr += '<li id="db_childer_li' + i + '" name="db_childer_li_nobuy_' + daydate + '" style="background-color: #3CAFDC; padding:10px; display:none;"><a  href="mTravelbusOrderVisitorlist.aspx?daydate=' + daydate + '&proid=' + data.msg[i].Proid + '&oper=jietuansuc">' +
                                             data.msg[i].Proname + '(暂时无人预定)';
                                contentstr += '</a></li>';
                            }
                        }
                       
//                        if ($("[name='db_childer_li_nobuy_" + daydate + "']").length > 0) {
                        contentstr += '<li id="db_childer_li' + i + '"  style="background-color: #eee; padding:10px;cursor:pointer;" onclick="openMore(this,\'' + daydate + '\')">展开暂时没人预定的车辆</li>';
//                        }

                        var titlehtml = $("#db_li" + index).html();

                        var tbhtml = $("#list").html();
                        tbhtml = tbhtml.replace(titlehtml, titlehtml + contentstr);
                        $("#list").html(tbhtml);
                    }
                })
            }
            else {
                $("#tr_hid_" + index).val(0);
                $("#db_label" + index).text("点击展开");

                var ulhtml = $("#db_li" + index).parent(".list-a").html();
                //                alert(ulhtml);
                var firstlihtml = $("#db_li" + index).prop("outerHTML");
                //                alert(firstlihtml);
                var contenthtml = ulhtml.replace(firstlihtml, "");
                //                alert(contenthtml);


                var tbhtml = $("#list").html();
                tbhtml = tbhtml.replace(contenthtml, "");
                $("#list").html(tbhtml);
            }
        }

        function goyanzheng() {
            location.href = "/ui/pmui/ETicket/mETicketIndex.aspx";
        }
        function goyanzhenglist() {
            location.href = "/ui/pmui/ETicket/mETicketList.aspx";
        }
        function golvyoudaba() {
            location.href = "/ui/MemberUI/mTravelbusOrderCount.aspx";
        }

        function openMore(obj, daydate) {
            
//            $("[name='db_childer_li_nobuy_" + daydate + "']").each(function (i, item) {
//                $(item).show();
//            });
//            $(obj).text("关闭");
            if ($("[name='db_childer_li_nobuy_" + daydate + "']").eq(0).css("display").toLowerCase() == "none") {
                $("[name='db_childer_li_nobuy_" + daydate + "']").each(function (i, item) {
                    $(item).show();
                });
                $(obj).text("关闭暂时没人预定的车辆");
            }
            else {
                $("[name='db_childer_li_nobuy_" + daydate + "']").each(function (i, item) {
                    $(item).hide();
                });
                $(obj).text("展开暂时没人预定的车辆");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <header class="header" style="background-color: #3CAFDC;">
          <h1 id="h_comname"></h1>
        <div class="left-head"> 
                 <a href="/ui/pmui/ETicket/mETicketIndex.aspx" class="tc_back head-btn">
                  <span class="inset_shadow"><span class="home-10"></span></span>
              </a> 
            </div>
        <div class="right-head"> 
                <a href="/yanzheng/loginout.aspx" style=" font-size:12px; color:#ffffff;"><span class="inset_shadow"><span style="padding-right:10px;font-size:14px;">退出</span></span></a>    
        </div>
        </header>
    <!-- container -->
    <div class="container ">
        <div class="tabber  tabber-n3 tabber-double-11 clearfix">
            <a href="javascript:goyanzheng()" style="width: 33%;">验证电子凭证</a> <a href="javascript:goyanzhenglist()"
                style="width: 33%;">验证列表</a> <a class="active" href="javascript:golvyoudaba()" style="width: 33%;">
                    旅游大巴查询</a>
        </div>
        <div class="list-search" style="height: 32px; padding: 5px 10px 7px; background: #eee;">
            <dl class="fn-clear" style="height: 32px; background: #fff; border-radius: 5px; border: 1px solid #c9c9c9;
                position: relative;">
                <dt style="position: relative; overflow: hidden; padding-left: 5px; margin-right: 40px;">
                    <input id="datetime" placeholder="选择出车日期" style="height: 25px; margin-top: 4px; border: 0;
                        outline: 0; background: 0; width: 100%;" />
                </dt>
                <dd style="float: left; width: 30px; height: 25px; margin-top: 4px; position: absolute;
                    top: 0; right: 0;">
                    <s id="search_botton" style="width: 17px; height: 17px; display: block; vertical-align: middle;
                        margin: 4px auto 0; background: url(/Images/public_com.png) no-repeat -44px 0;
                        background-size: 64px 17.5px;"></s>
                </dd>
            </dl>
        </div>
        <div id="backs-list-container" class="block">
            <li class="block block-order animated">
                <div class="block block-list block-border-top-none block-border-bottom-none" id="list"
                    style="padding-left: 0;">
                    <div class="layout-box">
                        <ul class="list-a">
                            <%--   <li>2015-03-03(支付成功:0人;截团:0人;) </li>
                            <li style="background-color: #eee; padding-left: 20px;">39座旅游大巴(支付成功:2人;截团:0人) </li>--%>
                        </ul>
                    </div>
                </div>
            </li>
            <div class="empty-list" style="margin-top: 30px; display: none;">
                <!-- 文本 -->
                <div>
                    <h4>
                        哎呀，暂不记录？</h4>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
