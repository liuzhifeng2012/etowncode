<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/M/MemberH5.Master"
    CodeBehind="Coupon.aspx.cs" Inherits="ETS2.WebApp.M.Coupon" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <title>
        <%=comname %></title>
    <script type="text/javascript">


        $(function () {

            var aid = $("#aid").val();
            var comid = $("#hid_comid").trimVal();
            var comname = $("#hid_comname").trimVal();
            var comlogo = $("#hid_comlogo").trimVal();

            $("#entLogo").attr("src", comlogo);
//            $("#fcomname").html(comname);

            //活动日志
            $.ajax({
                type: "post",
                url: "/JsonFactory/CrmMemberHandler.ashx?oper=Loadingactlog",
                data: { aid: aid, comid: comid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        //$("#records").html("查询会员数据出现错误。");
                        return;
                    }

                    if (data.type == 100) {
                        if (data.totalCount == 0) {
                            $("#actlog").html("没有查到消费信息。");
                        } else {
                            $.each(data.msg, function (i, item) {
                                var htmlstr = "";
                                htmlstr = htmlstr + '<table>'
                                htmlstr = htmlstr + '<tr>'
                                htmlstr = htmlstr + '	<td>'
                                htmlstr = htmlstr + ChangeDateFormat(item.Usesubdate)
                                htmlstr = htmlstr + '	</td>'
                                htmlstr = htmlstr + '	<td>'
                                htmlstr = htmlstr + '[' + item.ACTID + ']'
                                htmlstr = htmlstr + '	</td>'
                                htmlstr = htmlstr + '	<td>'
                                htmlstr = htmlstr + item.ServerName
                                htmlstr = htmlstr + '	</td>'
                                htmlstr = htmlstr + '</tr>'
                                htmlstr = htmlstr + '</section>';
                                $("#actlog").append(htmlstr);

                            })
                        }
                    }
                }
            })


            //领取活动
            $.ajax({
                type: "post",
                url: "/JsonFactory/CrmMemberHandler.ashx?oper=ClaimActlist",
                data: { comid: comid, aid: aid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {

                    }
                }
            });


        })

    </script>
    <script type="text/javascript">
        //查看详情按钮事件
        function cardDetailBtn() {
            showPop("useDetailDiv");
        }

        //取消详情按钮事件
        function useDetailCancelBtn() {
            hidePop("useDetailDiv");
        }


        //弹出框
        var oldContent;
        function showPop(a) {
            var _screenWidth = window.screen.width;
            var _isoFlag = /iP(ad|hone|od)/.test(navigator.userAgent);
            //320 的ios和非详情，日志div增加样式
            if (_isoFlag && _screenWidth <= 320) {
                iospBoxStyle(a);
            }
            if (oldContent != null) {

                if (_screenWidth > 320 || _isoFlag) {

                    $("#" + oldContent).css("bottom", "-320px");
                }

                $("#" + oldContent).css("display", "none");
            }

            $("#" + a).css("display", "block");
            setTimeout(function () {
                if (_screenWidth > 320 || _isoFlag) {

                    $("#" + a).css("bottom", "0px");
                } else {

                    $("#" + a).css("bottom", "auto");
                }
            }, 100);
            oldContent = a;
        }

        function hidePop(a) {
            var _screenWidth = window.screen.width;
            var _isoFlag = /iP(ad|hone|od)/.test(navigator.userAgent);
            if (_screenWidth > 320 || _isoFlag) {

                $("#" + a).css("bottom", "-320px");
            }
            setTimeout(function () {

                $("#" + a).css("display", "none");
            }, 500);
        }
        //针对320的ios做预处理
        function iospBoxStyle(id) {
            var _st = document.getElementById(id);
            _st.style.position = "fixed";
            _st.style.zIndex = "99999";
            _st.style.left = "0";
            _st.style.bottom = "-320px";
            _st.style.height = "270px";
        }
        //我要使用增加click事件
        function useCardBtn() {
            showPop("soundLoad");

        }
        /***
        * 取消card使用按钮事件
        */
        function cancelUseCard() {

            //隐藏扫描图片
            hidePop("soundLoad");
        }

        //查看消费日志
        function logDetailBtn() {
            showPop("logsDetailDiv");
        }
        //取消消费日志
        function cancelUselog() {

            //隐藏扫描图片
            hidePop("logsDetailDiv");
        }
       
    </script>
    <script type="text/javascript">

        function goDefault(Openid) {
            window.location.href = 'Default.aspx';
        }
        function goWeixin() {
            showPop("goWeixin");
            $("#enter").click(function () {
                window.location.href = 'weixin.aspx'; //indexcard.aspx
            })
            $("#carl").click(function () {
                hidePop("goWeixin");
            })

        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div class="card_info">
        <section style="background: url('/images/card_2.gif') no-repeat; background-size: 100% 100%;
            font-family: fontNumber; margin-left: auto; margin-right: auto; width: 300px;
            height: 170px;">

        	                
        <img name="realLogo" id="entLogo" src="" height="29px" style="position:absolute;margin-top:8px;margin-left:20px;" alt="公司logo">
	                   
						<span name="logo" style="white-space: nowrap;position:absolute;width:0px;line-height:29px;color:#247BFA;margin-left:85px;margin-top:15px;vertical-align:middle;text-align:left;">
							
						<span style="font-family:DIN-Regular;font-size:13px;text-shadow:0px -1px 2px rgba(0,0,0,0.30);  ">
							<f effect="4c000000,0,-1" size="13" face="DIN-Regular" id="fcomname"></f></span>
		
						</span>

						<span name="" style="position:absolute;height:29px;color:#;margin-left:20px;margin-top:8px;vertical-align:;text-align:;">	
						</span>
																	
						<span name="" style="position:absolute;width:165px;height:12px;color:#999999;margin-left:100px;margin-top:128px;vertical-align:;text-align:left;">
						    <span style="font-family:;font-size:8px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
						    <f effect="66ffffff,0,1" size="8">地点</f>
                            </span>
						</span>

						<span name="" style="position:absolute;width:165px;height:35px;color:#333333;margin-left:100px;margin-top:143px;vertical-align:;text-align:left;">
								<span style="font-family:DIN-Regular;font-size:12px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1" size="12" face="DIN-Regular" rowspace="3" maxrows="2"><%=Usetitle%> 
								</f></span>
						</span>
							
						<span name="" style="position:absolute;width:72px;height:35px;color:#333333;margin-left:20px;margin-top:143px;vertical-align:;text-align:left;">
								<span style="font-family:DIN-Regular;font-size:12px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1" size="12" face="DIN-Regular"  id="CouponEndDate"><%=Actend %>
								</f></span>
						</span>
									
						<span name="" style="position:absolute;width:270px;height:57px;color:#ffffff;margin-left:20px;margin-top:63px;vertical-align:;text-align:left;">
								<span style="font-family:DIN-Regular;font-size:21px;text-shadow:0px -1px 2px rgba(0,0,0,0.30);">
									<f effect="4c000000,0,-1" size="21" rowspace="5" maxrows="2" face="DIN-Regular" id="CouponTitle"><%=Title %>
								</f></span>
                                <br><span style="font-family:DIN-Regular;font-size:13px;text-shadow:0px -1px 2px rgba(0,0,0,0.30);">
									<f effect="4c000000,0,-1" size="13" rowspace="5" maxrows="2" face="DIN-Regular" id="CouponTitle"><%=Atitle %>
								</f></span>
						</span>
																	
												
						<span name="" style="position:absolute;width:72px;height:12px;color:#999999;margin-left:20px;margin-top:128px;vertical-align:;text-align:left;">
								<span style="font-family:;font-size:8px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1" size="8">有效期
								</f></span>
						</span>
																	
						<span name="" style="position:absolute;width:300px;height:170px;color:#;margin-left:0px;margin-top:0px;vertical-align:;text-align:;">
						</span>

                        <%if (usestate == 2 || usestate == 3)
                          { %>

                         <span class="status_info_03">状态：已使用</span>
                         <%}
                          else if (usestate == 5)//|| usestate == 6
                          { %>
                          <span class="status_info_04">状态：未领取</span>
                         <%} %>

					
        </section>
        <!--根据状态了添加我要使用按钮-->
        <%if (usestate == 5)
          { %>
        <p id="iUsed">
            <a href="javascript:goWeixin();" id="useCardBtn" class="btn_big btn_green" style="color: White;">
                我要使用<%--设置我的微旅行会员账户--%></a></p>
        <%}
          else if (usestate == 2 || usestate == 3)
          { %>
        <p id="iUsed">
            <a href="javascript:;" id="useCardBtn" class="btn_big btn_green" style="color: White;">
                优惠活动已经使用</a></p>
        <%}
          else if (usestate == 4)
          { %>
        <p id="P1">
            <a href="javascript:;" id="A1" class="btn_big btn_green" style="color: White;">优惠活动已过期</a></p>
        <%--   <%}else if (usestate == 6) { %>
             <p id="P2"><a href="javascript:goDefault('<%=openid %>');" id="A2" class="btn_big btn_green" style=" color: White;">优惠活动尚未领取</a></p>--%>
        <%}
          else
          { %>
        <p id="iUsed">
            <a href="javascript:useCardBtn();" id="useCardBtn" class="btn_big btn_green" style="color: White;">
                我要使用</a></p>
        <%} %>
        <p id="getDetail">
            <a href="javascript:cardDetailBtn();" id="cardDetailBtn" class="link"><i class="ico_info">
            </i>查看详情</a>| <a href="javascript:logDetailBtn();" id="logsDetailBtn" class="link"><i
                class="ico_money"></i>消费记录</a></p>
    </div>
    <%--    <div id="header"> 
    <span class="left btn_back" onClick="goDefault('<%=openid %>');"></span> 
    微旅行优惠券
    </div> --%>
    <!--详细信息-->
    <div id="useDetailDiv" class="pop_up_box" style="display: none;">
        <div class="header">
            <a href="javascript:useDetailCancelBtn();" id="useDetailCancelBtn" class="btn_small btn_black">
                取消</a> 详情
        </div>
        <div class="pop_up_box_cont">
            <header>报名与使用说明</header>
            <section class="records_1">
				<p> <b>活动内容说明</b></p> 
				<p> <%=Remark%> </p>
                <p> <b>承兑商家及地址</b></p> 
				<p>  <%=Useremark%></p>

        </section>
        </div>
    </div>
    <!--选择更多-->
    <div id="shareDiv" class="pop_up_box" style="display: none;">
        <div class="header">
            <a href="javascript:cancelShare();" class="btn_small btn_black">取消</a> 更多
        </div>
        <div class="pop_up_box_cont share">
        </div>
    </div>
    <!--加载二维码-->
    <div id="soundLoad" class="pop_up_box" style="display: none; bottom: -320px;">
        <div class="header">
            <a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black">
                取消</a></div>
        <div class="pop_up_box_cont">
            <div class="code_proving">
                <span id="msg" runat="server" style="color: Red"></span>
                <img src="/ui/pmui/eticket/showtcode.aspx?pno=<%=cardcode %>" /></div>
            <p>
                <a href="javascript:qrCheck();" id="checkCodeBtn" class="btn_big btn_green" style="color: #ffffff;">
                    辅码：<b>
                        <%=cardcode%>
                    </b></a>
            </p>
        </div>
    </div>
    <!--消费记录-->
    <div id="logsDetailDiv" class="pop_up_box" style="display: none; bottom: -320px;">
        <div class="header">
            <a href="javascript:cancelUselog();" id="logsDetailCancelBtn" class="btn_small btn_black">
                取消</a> 消费记录
        </div>
        <div class="pop_up_box_cont">
            <section id="actlog" class="records_1">
	        </section>
        </div>
    </div>
    <input id="aid" type="hidden" value="<%=aid %>" />
     <input type="hidden" id="hid_comlogo" value="<%=comlogo %>" />
    <!--设置账户-->
    <div id="goWeixin" class="pop_up_box" style="display: none; bottom: -320px;">
        <div class="header">
            <a href="#" id="carl" class="btn_small btn_black">取消</a></div>
        <div class="pop_up_box_cont">
            <p>
                <a href="#" id="enter" class="btn_big btn_green" style="color: #ffffff;">请先设置我的微旅行会员账户！</a>
            </p>
        </div>
    </div>
    <!--<div class="footFix" id="footReturn">
        <a href="indexcard.aspx"><span>返回会员卡首页</span></a>
    </div>-->
</asp:Content>
