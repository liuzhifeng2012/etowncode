<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TwoCodeDetail.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ETicket.TwoCodeDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" type="text/css" href="/Styles/weixin/css4.css" />
    <title>易城O2O-电子凭证验证服务</title>
</head>
<body>
    <div id="header">
        <span class="left btn_back" onclick="goBack();"></span>
        <!-- <span class="left btn_set"></span> -->
        返回易城卡列表<!-- <span class="right btn_more"></span> -->
    </div>
    <div class="card_info">
        <section style="background: url('http://www.etown.cn/web-test/weixin/img/sys_ticket2_blue.png') no-repeat;
            background-size: 100% 100%; font-family: fontNumber; margin-left: auto; margin-right: auto;
            width: 300px; height: 170px;">

        	                
        <img name="realLogo" id="entLogo" src="http://www.etown.cn/web-test/weixin/img/51.png" height="29px" style="position:absolute;margin-top:8px;margin-left:20px;" alt="公司logo">
						
											
												<span name="logo" style="white-space: nowrap;position:absolute;width:0px;line-height:29px;color:#ffffff;margin-left:80px;margin-top:8px;vertical-align:middle;text-align:left;">
							
															<span style="font-family:DIN-Regular;font-size:15px;text-shadow:0px -1px 2px rgba(0,0,0,0.30);">
									<f effect="4c000000,0,-1" size="15" face="DIN-Regular" id="fScenicName"> 
								</f></span>
											
						</span>
						
																	
												
																	
												<span name="" style="position:absolute;height:29px;color:#;margin-left:20px;margin-top:8px;vertical-align:;text-align:;">
							
											
						</span>
																	
												<span name="" style="position:absolute;width:165px;height:12px;color:#999999;margin-left:100px;margin-top:118px;vertical-align:;text-align:left;">
															<span style="font-family:;font-size:8px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1" size="8">地点
								</f></span>
													</span>
																	
												<span name="" style="position:absolute;width:165px;height:35px;color:#333333;margin-left:100px;margin-top:133px;vertical-align:;text-align:left;">
															<span style="font-family:DIN-Regular;font-size:12px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1" size="12" face="DIN-Regular" rowspace="3" maxrows="2" id="fComAddress">
								</f></span>
													</span>
																	
												
																	
												<span name="" style="position:absolute;width:72px;height:35px;color:#333333;margin-left:20px;margin-top:133px;vertical-align:;text-align:left;">
															<span style="font-family:DIN-Regular;font-size:12px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1" size="12" face="DIN-Regular" id="fPro_end"> 
								</f></span>
													</span>
																	
												
																	
												<span name="" style="position:absolute;width:240px;height:57px;color:#ffffff;margin-left:20px;margin-top:53px;vertical-align:;text-align:left;">
															<span style="font-family:DIN-Regular;font-size:21px;text-shadow:0px -1px 2px rgba(0,0,0,0.30);">
									<f effect="4c000000,0,-1" size="21" rowspace="5" maxrows="2" face="DIN-Regular" id="fProductName"> 
								</f></span>
													</span>
																	
												
																	
												<span name="" style="position:absolute;width:72px;height:12px;color:#999999;margin-left:20px;margin-top:118px;vertical-align:;text-align:left;">
															<span style="font-family:;font-size:8px;text-shadow:0px 1px 2px rgba(255,255,255,0.40);">
									<f effect="66ffffff,0,1" size="8">有效期
								</f></span>
													</span>
																	
												
																	
												<span name="" style="position:absolute;width:300px;height:170px;color:#;margin-left:0px;margin-top:0px;vertical-align:;text-align:;">
													</span>
																	
												
																	
												
											
    </section>
        <!--根据状态了添加我要使用按钮-->
        <p id="iUsed">
            <a href="javascript:useCardBtn();" id="useCardBtn" class="btn_big btn_green">我要使用</a></p>
        <p id="more">
            <a href="javascript:showShareDiv();" class="btn_big ">更多操作</a></p>
        <p id="getDetail">
            <a href="javascript:cardDetailBtn();" id="cardDetailBtn" class="link"><i class="ico_info">
            </i>查看详情</a>|<a href="javascript:logsDetailBtn();" id="logsDetailBtn" class="link"><i
                class="ico_money"></i>消费记录</a></p>
    </div>
    <!--详细信息-->
    <div id="useDetailDiv" class="pop_up_box" style="display: none;">
        <div class="header">
            <a href="javascript:useDetailCancelBtn();" id="useDetailCancelBtn" class="btn_small btn_black">
                取消</a> 详情
        </div>
        <div class="pop_up_box_cont">
            <header>使用条款</header>
            <section class="records_1">
	            <!-- <header>使用条款</header> -->
				<p> 服务包含</p> 
                <p id="pservice_Contain"></p>
                <p>服务不含</p>
                <p id="pservice_NotContain"></p>
                <p>注意事项</p>
                <p id="pprecautions"></p>
				 
        </section>
        </div>
    </div>
    <!--消费记录-->
    <div id="logsDetailDiv" class="pop_up_box" style="display: none; bottom: -320px;">
        <div class="header">
            <a href="javascript:logsDetailCancelBtn();" id="logsDetailCancelBtn" class="btn_small btn_black">
                取消</a> 记录
        </div>
        <div class="pop_up_box_cont">
            <section class="records">
	            <!-- <header>消费记录</header> -->
	            <p></p>
				<p>2013-07-05 11:00 购买成功,购买张数12张</p>
				<p>2013-07-06 9:10 验票成功,使用4张,剩余8张</p>
				<p>2013-07-12 13:25 验票成功,使用8张,剩余0张</p>
				<p></p>
	        </section>
        </div>
    </div>
    <!--选择更多-->
    <div id="shareDiv" class="pop_up_box" style="display: none;">
        <div class="header">
            <a href="javascript:cancelShare();" class="btn_small btn_black">取消</a> 更多
        </div>
        <div class="pop_up_box_cont share">
            <p id="shareP" style="display: none">
                <a href="javascript:passShare();" class="btn_big">分享</a></p>
            <!-- <p id="giveP" style="display:none"><a href="javascript:passGive();" class="btn_big" >转赠</a></p> -->
            <p style="min-height: 50px">
                <a href="javascript:deletePass();" class="btn_big">删除</a></p>
        </div>
    </div>
    <!--加载声音动画-->
    <div id="soundLoad" class="pop_up_box" style="display: none; bottom: -320px;">
        <div class="header">
            <a href="javascript:cancelUseCard();" id="checkCancelBtn" class="btn_small btn_black">
                取消</a>扫描验证</div>
        <div class="pop_up_box_cont">
            <div class="code_proving">
                <span id="msg" runat="server" style="color: Red"></span>
                <img src="showtcode.aspx?pno=<%=pno %>" /></div>
            <p>
                <a href="javascript:qrCheck();" id="checkCodeBtn" class="btn_big btn_green">辅助消费码：<b>
                    <%=pno %>
                </b></a>
            </p>
        </div>
    </div>
    <!-- 播放声音div -详细信息只有一条记录，只能循环一次-->
    <div id="audioDiv" align="right" style="display: none">
        <!-- <audio id="audioId" src='/public/media/${id}.mp3'>浏览器不支持audio标签</audio> -->
        <!-- <input id="controlAudioBtn" type="button" value="Play" onclick="audioControl()"/> -->
        <audio id="audioId" src=""></audio>
    </div>
    <input type="hidden" id="hid_pno" value="<%=pno %>" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        (function ($) {
            $.post("/JsonFactory/EticketHandler.ashx?oper=GetPnoDetail", { pno: $("#hid_pno").val() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("获取数据出现错误");
                    return;
                }
                if (data.type == 100) {

                    $("#fScenicName").text(data.msg[0].ScenicName);
                    $("#fComAddress").text(data.msg[0].ComAddress);
                    $("#fPro_end").text(ChangeDateFormat(data.msg[0].Pro_end));
                    $("#fProductName").text(data.msg[0].ProductName + "(" + data.msg[0].UserNum + "张)");
                    $("#pprecautions").text(data.msg[0].Precautions);
                    $("#pservice_NotContain").text(data.msg[0].ServiceNotContain);
                    $("#pservice_Contain").text(data.msg[0].ServiceContain);
                }
            })
        })(jQuery)
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
       
    </script>
</body>
</html>
