<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/M/MemberH5.Master"
    CodeBehind="Default.aspx.cs" Inherits="ETS2.WebApp.M.Default" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <title>
        <%=comname %></title>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();
            var channelcompanyid = $("#hid_channelcompanyid").trimVal();
            
            //            if (comid != 101) {
            //                $("#footReturn").hide();
            //            }
            //活动
            var pageSize = 200; //每页显示条数
            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                $("#tblist").empty();
                $("#divPage").empty();

                //获取未领取活动
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=UnActlist",
                    data: { comid: comid, channelcompanyid: channelcompanyid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.totalCount == 0) {
                                //                                $("#ulreply").append("<section  class='section1'><header></header><article>无可领取的活动</article><time></time></section>");
                            }
                            else {
                                $("#hid_notlingqunum").val(data.totalCount);
                                //填充列表数据
                                var j = 1;
                                $.each(data.msg, function (i, item) {
                                    var htmlstr = "";
                                    if (j == 1) {
                                        htmlstr = htmlstr + '<section onclick="getUnCoupon(' + item.Id + ')" class="section1" >'
                                    } else if (j == 2) {
                                        htmlstr = htmlstr + '<section onclick="getUnCoupon(' + item.Id + ')" class="section2" >'
                                    } else if (j == 3) {
                                        htmlstr = htmlstr + '<section onclick="getUnCoupon(' + item.Id + ')" class="section3" >'
                                    }
                                    htmlstr = htmlstr + '<header>'
                                    htmlstr = htmlstr + '		<span class="status_list_04">状态：未领取</span>'
                                    htmlstr = htmlstr + '			<img  src="' + $("#hid_comlogo").trimVal() + '" class="card_logo" alt="">' + $("#hid_comname").trimVal()
                                    htmlstr = htmlstr + '	</header>'
                                    htmlstr = htmlstr + '	<article>'
                                    htmlstr = htmlstr + '	   <p><f effect="4c000000,0,-1"  size="21" rowSpace="5" maxRows="2" face"DIN-Regular">' + item.Title + '</p>'
                                    htmlstr = htmlstr + '	</article>'
                                    htmlstr = htmlstr + '	<time>'
                                    htmlstr = htmlstr + '  有效期：  <span style="width: 120px; height: 25px;">' + ChangeDateFormat(item.Actend) + '</span>'
                                    //htmlstr = htmlstr + '<input type="button" onclick="ClaimCoupon(' + item.Id + ')" value=" 领取优惠 " style="width: 120px; height: 25px;" />'
                                    htmlstr = htmlstr + '</time>'
                                    htmlstr = htmlstr + '</section>';

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

                //获取已有活动列表
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=Actlist",
                    data: { comid: comid, channelcompanyid: channelcompanyid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("活动列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            if (data.totalCount == 0) {
                                //                                $("#ulreply").append("<section  class='section1'><header></header><article>无已领取活动</article><time></time></section>");
                            } else {
                                $("#hid_haslingqunum").val(data.totalCount);
                                var j = 1;
                                $.each(data.msg, function (i, item) {

                                    var htmlstr = "";
                                    if (j == 1) {
                                        htmlstr = htmlstr + '<section onclick="getCoupon(' + item.Id + ')" class="section1" >'
                                    } else if (j == 2) {
                                        htmlstr = htmlstr + '<section onclick="getCoupon(' + item.Id + ')" class="section2" >'
                                    } else if (j == 3) {
                                        htmlstr = htmlstr + '<section onclick="getCoupon(' + item.Id + ')" class="section3" >'
                                    }

                                    htmlstr = htmlstr + '<header>'
                                    if (item.Usestate == "已使用") {
                                        htmlstr = htmlstr + '		<span class="status_list_03">状态：已使用</span>'
                                    }
                                    htmlstr = htmlstr + '			<img  src="' + $("#hid_comlogo").trimVal() + '" class="card_logo" alt="">' + $("#hid_comname").trimVal()
                                    htmlstr = htmlstr + '	</header>'
                                    htmlstr = htmlstr + '	<article>'
                                    htmlstr = htmlstr + '	   <p><f effect="4c000000,0,-1"  size="21" rowSpace="5" maxRows="2" face"DIN-Regular">' + item.Title + '</p>'
                                    htmlstr = htmlstr + '	</article>'
                                    htmlstr = htmlstr + '	<time><f effect="66ffffff,0,1" size="12" face="DIN-Regular">' + ChangeDateFormat(item.Actstar) + ' 至 ' + ChangeDateFormat(item.Actend) + '</time>'
                                    htmlstr = htmlstr + '</section>';

                                    j = j + 1;
                                    if (j == 4) {
                                        j = 1;
                                    }

                                    $("#ulreply").append(htmlstr);

                                })
                                //setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })

            }


        })
        //打开页面后自动加载
        jQuery(document).ready(function () {
            $("div.bd").hide();
            $("#bd_0").show();
        })




        function getCoupon(aid) {
            window.location.href = 'Coupon.aspx?aid=' + aid + "&act=A";
        }

        function getUnCoupon(aid) {
            window.location.href = 'Coupon.aspx?aid=' + aid + "&act=N";
        }


        function goBangding(Openid) {
            window.location.href = 'indexcard.aspx'; //weixin.aspx
        }

        function Popbox(info, href, options) {
            options = $.extend(this.options, options || {});
            new Boxy("<div style='padding-left:50px;padding-right:50px;text-align:center;font-size:14px;'>" + info + "</div>", $.extend(
            {
                behaviours: function () {
                    location.href = href;
                }
            }, options));
        }

        function goWeixin(Openid) {
            window.location.href = 'weixin.aspx'; //indexcard.aspx
        }

        $(function () {
            //如果商家未添加活动
            if ($("#hid_notlingqunum").trimVal() == 0 && $("#hid_haslingqunum").trimVal() == 0) {
                $("#ulreply").append("<section  class='section1'><header></header><article>商家还未添加活动</article><time></time></section>");
            }
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="ulreply">
    </div>
    <%--<div id="header">
	<span id="Span1" class="left btn_set" onclick="goBangding('<%=openid %>');"></span>我的微旅行
</div>
    --%>
    <!--<div class="footFix" id="footReturn">
        <a href="indexcard.aspx"><span>返回会员卡首页</span></a>
    </div>-->
    <input type="hidden" id="hid_notlingqunum" value="0" />
    <input type="hidden" id="hid_haslingqunum" value="0" />
    <input type="hidden" id="hid_comlogo" value="<%=comlogo %>" />
    <input type="hidden" id="hid_channelcompanyid" value="<%=channelcompanyid %>" />

</asp:Content>
