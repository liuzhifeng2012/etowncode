<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EticketDetail.aspx.cs"
    Inherits="ETS2.WebApp.M.EticketDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <meta name="format-detection" content="telephone=no">
    <link rel="stylesheet" type="text/css" href="/Styles/css4.css">
    <script type="text/javascript" src="/Scripts/js4.js"></script>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/Impromptu.css" rel="stylesheet" type="text/css">
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
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
    <title></title>
    <script type="text/javascript">


        $(function () {
            //根据订单id得到产品信息
            var orderid = $("#hid_orderid").trimVal();
            var openid = $("#hid_openid").trimVal();
            $.post("/JsonFactory/OrderHandler.ashx?oper=GetProductByOrderId", { orderid: orderid, openid: openid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //$.prompt("获取产品信息出错");
                    return;
                }
                if (data.type == 100) {
                    $("#entLogo").attr("src", data.msg[0].ConsumerLogo);
                    $("#CouponTitle1").html(data.msg[0].Proname);
                    $("#CouponTitle2").html("(" + data.msg[0].U_num + "张)");
                    $("#CouponEndDate").html(data.msg[0].ProValid);
                    $("#div_prodetail").html('<section class="records_1">' +
				        '<p> <b>服务包含</b></p> ' +
				        '<p>' + data.msg[0].ServiceContain + '</p>' +
                        '</section>' +
                        '<section class="records_1">' +
				        '<p> <b>服务不包含</b></p> ' +
				        '<p>' + data.msg[0].ServiceNotContain + '</p>' +
                        '</section>' +
                         '<section class="records_1">' +
				        '<p> <b>注意事项</b></p> ' +
				        '<p>' + data.msg[0].Precautions + '</p>' +
                        '</section>');

                    $("#fcomname").html(data.msg[0].ConsumerCompanyName);
                    $("#hid_etiketsendstate").val(data.msg[0].Send_state);

                    $("#companyaddress").html(data.msg[0].CompanyAddress);


                    <%if(servertype==11){ //实物 
                    %>

                    var orderstate_view="";
                    if(data.msg[0].Deliverytype==4){
                        if(data.msg[0].Order_state==2){
                            orderstate_view="订单处理中";
                        }
                        if(data.msg[0].Order_state==4){
                            orderstate_view="自提,已发码";
                        }
                        if(data.msg[0].Order_state==8){
                            orderstate_view="自提,已提货";
                        }
                        if(data.msg[0].Order_state==16){
                            orderstate_view="退款";
                        }

                    }else{

                        if(data.msg[0].Order_state==2){
                            orderstate_view="订单处理中";
                        }
                        if(data.msg[0].Order_state==4){
                            orderstate_view="发货中 "+ data.msg[0].Expresscom+":"+data.msg[0].Expresscode;
                        }
                        if(data.msg[0].Order_state==8){
                            orderstate_view="已发货 "+data.msg[0].Expresscom+":"+data.msg[0].Expresscode;
                        }
                        if(data.msg[0].Order_state==16){
                            orderstate_view="退款";
                        }
                    }

                     $("#CouponTitle2").html("(数量：" + data.msg[0].U_num + ")");

                    $("#companyaddress_title").html("状态");
                    $("#companyaddress").html(orderstate_view);
                    $("#CouponEndDate_title").html("订单日期");
                    $("#CouponEndDate").html(ChangeDateFormat(data.msg[0].U_subdate));


                     $("#div_prodetail").html('<section class="records_1">' +
				        '<p>' + data.msg[0].ServiceContain + '</p>' +
                        '</section>');


                    $("#iUsed").hide();
                    //$("#logsDetailBtn").hide();
                    $("#logsDetailBtn").html("处理状态");
                    
                     sttr ='<table><tbody><tr><td>' + jsonDateFormatKaler(data.msg[0].U_subdate) + '</td><td>提交订单 </td><td> 数量：' + data.msg[0].U_num + '</td></tr></tbody></table>';
                     sttr += '<table><tbody><tr><td>' + jsonDateFormatKaler(data.msg[0].Backtickettime) + '</td><td>' + orderstate_view + '</td><td></td></tr></tbody></table>';
 
                     $("#actlog").html(sttr);

                    <%}%>

                }
            })




            if ($("#hid_pno").val() != 0) {
                if ($("#hid_sourcetype").trimVal() == 1)//电子码来源为本系统生成的，可以查询验票日志；倒码过来的不可查询
                {
                    //根据电子码得到电子码验票日志
                    $.post("/JsonFactory/EticketHandler.ashx?oper=GetPnoConsumeLogList", { pno: $("#hid_pno").trimVal() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            //$.prompt("查询电子票日志出错");
                            return;
                        }
                        if (data.type == 100) {
                            var sttr = "";
                            if (data.totalcount != 0) {
                                for (var i = 0; i < data.msg.length; i++) {
                                    sttr += '<table><tbody><tr><td>' + data.msg[i].Actiondate + '</td><td>' + data.msg[i].A_remark + '</td><td>' + data.msg[i].Use_pnum + '张</td></tr></tbody></table>';
                                }
                            }

                            $("#actlog").html(sttr);
                        }
                    })

                } else {//电子码来源为倒码
                    $("#actlog").html("");
                }

            } else {
                $("#actlog").html("");
            }




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

        function goList() {
            window.location.href = 'EticketList.aspx?openid=' + $("#hid_openid").trimVal();
        }
        function goWeixin() {
            if ($("#hid_sourcetype").trimVal() == 1) {//本系统生成的
                //var sendstate = $("#hid_etiketsendstate").val();
                //if (sendstate == 1)//未发码
                //{
                //  showPop("DivNotSendEticket");

                // $("#carl2").click(function () {
                //     hidePop("DivNotSendEticket");
                // })
                // } else {//已发码
                showPop("soundLoad");
                $(".code_proving").show();
                $("#enter").click(function () {
                    window.location.href = 'weixin.aspx'; //indexcard.aspx
                })
                $("#carl").click(function () {
                    hidePop("soundLoad");
                })
                //}
            }
            else {//倒码过来的

                showPop("soundLoad");
                $(".code_proving").hide();
                $("#carl").click(function () {
                    hidePop("soundLoad");
                })
            }



        }

    </script>
</head>
<body>
    <div class="card_info">
        <section style="background: url('/images/card_2.gif') no-repeat; background-size: 100% 100%;
            font-family: fontNumber; margin-left: auto; margin-right: auto; width: 300px;
            height: 170px;">

        	                
        <img name="realLogo" id="entLogo" src="" height="29px" style="position:absolute;margin-top:8px;margin-left:20px;" alt="">
	                   
						<span name="logo" style="white-space: nowrap;position:absolute;width:0px;line-height:29px;color:#247BFA;margin-left:185px;margin-top:15px;vertical-align:middle;text-align:left;">
							
						<span style="font-family:DIN-Regular;font-size:13px;text-shadow:0px -1px 2px rgba(0,0,0,0.30);  ">
							<f effect="4c000000,0,-1" size="13" face="DIN-Regular" id="fcomname"></f></span>
		
						</span>

						<span name="" style="position:absolute;height:29px;color:#;margin-left:20px;margin-top:8px;vertical-align:;text-align:;">	
						</span>
																	
						 <span name="" style="position:absolute;width:165px;height:12px;color:#999999;margin-left:100px;margin-top:128px;vertical-align:;text-align:left;">
						    <span style="font-family:;font-size:8px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
						    <f effect="66ffffff,0,1" size="8" id="companyaddress_title">地点</f>
                            </span>
						</span> 

						 <span name="" style="position:absolute;width:165px;height:35px;color:#333333;margin-left:100px;margin-top:143px;vertical-align:;text-align:left;">
								<span style="font-family:DIN-Regular;font-size:12px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1" size="12" face="DIN-Regular" rowspace="3" maxrows="2" id="companyaddress">
 
								</f></span>
						</span> 
							
						<span name="" style="position:absolute;width:172px;height:35px;color:#333333;margin-left:20px;margin-top:143px;vertical-align:;text-align:left;">
								<span style="font-family:DIN-Regular;font-size:12px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1"   face="DIN-Regular" id="CouponEndDate"> 
								</f></span>
						</span>
									
						<span name="" style="position:absolute;width:270px;height:57px;color:#ffffff;margin-left:20px;margin-top:63px;vertical-align:;text-align:left;">
								<span style="font-family:DIN-Regular;font-size:21px;text-shadow:0px -1px 2px rgba(0,0,0,0.30);">
									<f effect="4c000000,0,-1" size="21" rowspace="5" maxrows="2" face="DIN-Regular" id="CouponTitle1"> 
								</f></span>
                                <span style="font-family:DIN-Regular;font-size:21px;text-shadow:0px -1px 2px rgba(0,0,0,0.30);">
									 <f effect="4c000000,0,-1" size="13" rowspace="5" maxrows="2" face="DIN-Regular" id="CouponTitle2"> 
								</f> </span>
						</span>
																	
												
						<span name="" style="position:absolute;width:72px;height:12px;color:#999999;margin-left:20px;margin-top:128px;vertical-align:;text-align:left;">
								<span style="font-family:;font-size:8px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1" size="8" id="CouponEndDate_title">有效期截止到
								</f></span>
						</span>
																	
						<span name="" style="position:absolute;width:300px;height:170px;color:#;margin-left:0px;margin-top:0px;vertical-align:;text-align:;">
						</span>

                        
                          <%--<span class="status_info_04">状态：未领取</span>--%>
                         

					
        </section>
        <!--根据状态了添加我要使用按钮-->
        <p id="iUsed">
            <a href="javascript:goWeixin();" id="useCardBtn" class="btn_big btn_green" style="color: White;">
                我要使用</a></p>
        <p id="getDetail">
            <a href="javascript:cardDetailBtn();" id="cardDetailBtn" class="link"><i class="ico_info">
            </i>查看详情</a>| <a href="javascript:logDetailBtn();" id="logsDetailBtn" class="link"><i
                class="ico_money"></i>消费记录</a></p>
    </div>
    <!--详细信息-->
    <div id="useDetailDiv" class="pop_up_box" style="display: none;">
        <div class="header">
            <a href="javascript:useDetailCancelBtn();" id="useDetailCancelBtn" class="btn_small btn_black">
                取消</a> 详情
        </div>
        <div class="pop_up_box_cont" id="div_prodetail">
            <%--   <header>详细信息</header>
            <section class="records_1">
				<p> <b>活动内容说明</b></p> 
				<p> 家庭出境游500元优惠券 </p>
        </section>--%>
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
                <span id="body_msg" style="color: Red"></span>
                <img src="/ui/pmui/eticket/showtcode.aspx?pno=<%=pno %>"></div>
            <p>
                <a href="#" id="checkCodeBtn" class="btn_big btn_green" style="color: #ffff">电子码：<b><%=pno %>
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
	        <%--<table>
            <tbody>
            <tr>	
            <td>2013-10-14	</td>
            <td>[家庭出境游500元优惠券]	</td>
            <td>出境,美洲,跟团旅游</td>
            </tr>
            </tbody>
            </table>     --%>              
           </section>
        </div>
    </div>
    <input id="aid" type="hidden" value="5">
    <!--未发码-->
    <div id="DivNotSendEticket" class="pop_up_box" style="display: none; bottom: -320px;">
        <div class="header">
            <a href="#" id="carl2" class="btn_small btn_black">取消</a></div>
        <div class="pop_up_box_cont">
            <p>
                <a href="javascript:void(0)" id="enter2" class="btn_big btn_green" style="color: #ffffff;">
                    尚未发码！</a>
            </p>
        </div>
    </div>
    <!--<div class="footFix" id="footReturn">
        <a href="javascript:void(0)" onclick="goList()"><span>返回电子凭证列表</span></a>
    </div>-->
    <input type="hidden" id="hid_orderid" value="<%=orderid %>" />
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_etiketsendstate" value="1" />
    <input type="hidden" id="hid_pno" value="<%=pno %>" />
    <input type="hidden" id="hid_sourcetype" value="<%=sourcetype %>" />
</body>
</html>
