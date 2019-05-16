<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userinfo.aspx.cs" Inherits="ETS2.WebApp.M.userinfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="text/html;charset=utf-8" http-equiv="Content-Type"></meta>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;" name="viewport"></meta>
    <meta content="telephone=no" name="format-detection"></meta>

    <style type="text/css">
        abbr, article, aside, audio, canvas, datalist, details, dialog, eventsource, figure, figcaption, footer, header, hgroup, mark, menu, meter, nav, output, progress, section, small, time, video, legend
        {
            display: block;
        }
        .footFix{width:100%;
                    text-align:center;
                    position:fixed;
                    left:0;
                    bottom:0;
                    z-index:99;}
        #footReturn,
        #footReturn2
        {
            z-index:89;
            display:inline-block;
            text-align:center;
            text-decoration:none;
            vertical-align:middle;
            cursor:pointer;
            width:100%;
            outline:0 none;
            overflow:visible;
            -moz-box-sizing:border-box;
            box-sizing:border-box;
            padding:0;height:41px;
            opacity:.95;
            border-top:1px solid #181818;
            box-shadow:inset 0 1px 2px #b6b6b6;
            background-color:#515151;
            background-image:-webkit-gradient(linear,0% 0,0% 100%,from(#838383),to(#202020));
        }
        #footReturn:hover,
        #footReturn:active,
        #footReturn2:hover,
        #footReturn2:active
        {
            background-color:#525252;background-image:-webkit-gradient(linear,0% 0,0% 100%,from(#838383),to(#222));
        }
        #footReturn a,
        #footReturn2 a
        {
            display:block;
            line-height:41px;
            color:#fff;
            text-shadow:1px 1px #282828;
            font-size:14px;
            font-weight:bold;}
    
        #footReturn a span,
        #footReturn2 a span
        {
            line-height:41px;
            padding-left:28px;
            background:url('/Images/arrow1.png') no-repeat 0 50%;
            -webkit-background-size:12px 15.5px;
            background-size:12px 15.5px;}
        #footReturn[hidden],#footReturn2[hidden]{display:none;}
    </style>
    <link href="../Styles/weixin/wei_bind.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>

    <script src="/Scripts/common.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").val();
            var id = $("#hid_id").val();
            //$("input:radio[name='Sex'][value='" + $("#hid_Sex").val() + "']").attr("checked", true)
            $("#inName").val($("#hid_Name").trimVal());
            $("#Phone").val($("#hid_phone").trimVal());
            $("#inBirthYear").val($("#hid_year").trimVal());
            $("#inBirthMonth").val($("#hid_month").trimVal());
            $("#inBirthDay").val($("#hid_day").trimVal());
            $("#recaptBtn").html("获取验证码");

            var sex = $("#hid_sex").val();
            if (sex == "男") {
                $("#gradenameboy").addClass("qb_icon icon_checkbox checked");
            }
            if (sex == "女") {
                $("#gradenamegirl").addClass("qb_icon icon_checkbox checked");
            }
            //hid_sex
            $("#gradenameboy").click(function () {
                $("#gradenamegirl").removeClass("qb_icon icon_checkbox checked");
                $("#gradenamegirl").addClass("qb_icon icon_checkbox");

                $("#gradenameboy").removeClass("qb_icon icon_checkbox");
                $("#gradenameboy").addClass("qb_icon icon_checkbox checked");

                $("#hid_sex").val("男");
            })

            $("#gradenamegirl").click(function () {
                $("#gradenameboy").removeClass("qb_icon icon_checkbox checked");
                $("#gradenameboy").addClass("qb_icon icon_checkbox");

                $("#gradenamegirl").removeClass("qb_icon icon_checkbox");
                $("#gradenamegirl").addClass("qb_icon icon_checkbox checked");

                $("#hid_sex").val("女");
            })

            var inBirthday = $("#inBirthYear").val() + '-' + $("#inBirthMonth").val() + '-' + $("#inBirthDay").val();
            if ($("#Phone").val() != "" && $("#inName").val() != "" && $("#hid_Sex").val() != "") {
                $("input").attr("disabled", "disabled");
                $("select").attr("disabled", "disabled");
                //$("#inName").attr("readOnly", true);
                $("#confirmBtn").css("display", "none");
                $("#divCode").css("display", "none");
                $("#recaptBtn").css("display", "none");
                $("#spanBirthday").css("text-align", "center");
                $("#spanBirthday").html("您的信息已完善请返回会员卡首页！");

            }
            if ($("#Phone").val() != "") {
                $("#Phone").attr("disabled", "disabled");
            }
            if ($("#inName").val() != "") {
                $("#inName").attr("disabled", "disabled");
            }


            //判断验证码
            $("#inCode").blur(function () {
                $("#spanMsg_inCode").html(""); //离开后先清空备注
                var Phone = $("#Phone").val();
                var code = $("#inCode").val();
                //判断手机不为空
                if (Phone != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getCode", { Phone: Phone, code: code, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {

                                return;

                            } else {

                                return;
                            }

                        }
                    })
                }
            })

            //判断手机
            $("#Phone").blur(function () {
                $("#PhoneVer").html(""); //离开后先清空备注
                var Phone = $("#Phone").val();
                //Phone
                if (Phone != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getPhone", { Phone: Phone, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {

                                return;
                            }
                            else if (data.msg == "此手机已经注册") {
                                $("#hid_ture").val(1);
                                $("#PhoneVer").html("此手机已开通会员账户，再次输入请刷新！");//，本次验证后将为您转到原有账户。
                                $("#PhoneVer").css("color", "red");
                                //$("#confirmBtn").html("验证绑定");
                                $("#divName").css("display", "none");
                                $("#divSex").css("display", "none");
                                $("#divBirthYear").css("display", "none");
                                $("#divCode").css("display", "none");
                                $("#confirmBtn").css("display", "none");
                                return;
                            }
                            else {
                                $("#PhoneVer").html(data.msg);
                                return;
                            }
                        }
                    })
                }
            })

            $("#cal").click(function () {
                $("#helptelshow").hide();
            })
            //提交按钮
            $("#confirmBtn").click("click", function () {
                var Cardcode = $("#hid_Card").val();
                var Name = $("#inName").val();
                var Phone = $("#Phone").val();
                var code = $("#inCode").val();
                var openid = $("#hid_openid").val();

                var Sex = $("#hid_sex").val();
                var inBirthday = $("#inBirthYear").val() + '-' + $("#inBirthMonth").val() + '-' + $("#inBirthDay").val();

                if (Phone == "") {
                    $("#PhoneVer").html("请填写手机");
                    $("#PhoneVer").css("color", "red");
                    $("#loading").hide();
                    return;
                }

                if (code == "") {
                    $("#spanMsg_inCode").html("请填写验证码");
                    $("#spanMsg_inCode").css("color", "red");
                    $("#loading").hide();
                    return;
                }

                if ($("#hid_ture").val() == 1) {
                    if (openid != "") {
                        $.post("/JsonFactory/CrmMemberHandler.ashx?oper=weiBtnopenid", { openid: openid, comid: comid, code: code, Phone: Phone }, function (wdata) {
                            wdata = eval("(" + wdata + ")");

                            if (wdata.type == 100) {
                                if (wdata.msg == "OK") {
                                    //$("#loading").hide();
                                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=codedelete", { openid: openid, comid: comid, code: code, Phone: Phone }, function (data) {
                                        data = eval("(" + data + ")");
                                        if (data.type == 100) {
                                            if (data.msg == "OK") {
                                                location.reload();
                                                return;
                                            }
                                            else {
                                                $("#spanMsg_inCode").html("操作验证码错误");
                                                $("#spanMsg_inCode").css("color", "red");
                                                return;
                                            }
                                        }
                                    })
                                    //location.reload();
                                    return;
                                } else {
                                    $("#loading").hide();
                                    $("#PhoneVer").html(wdata.msg);
                                    $("#PhoneVer").css("color", "red");
                                    return;
                                }
                            } else {
                                $("#loading").hide();
                                $("#PhoneVer").html(wdata.msg);
                                $("#PhoneVer").css("color", "red");
                                return;
                            }
                        })
                    }
                    else {
                        $("#PhoneVer").html("微信号为空");
                        $("#PhoneVer").css("color", "red");
                    }
                }
                else {
                    if (Name == "") {
                        $("#span_inName").html("请填写姓名");
                        $("#span_inName").css("color", "red");
                        $("#loading").hide();
                        return;
                    }
                    if (Sex == "") {
                        $("#span_sex").html("请填写性别");
                        $("#span_sex").css("color", "red");
                        $("#loading").hide();
                        return;
                    }


                    //创建订单
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=weiupmember", { comid: comid, Cardcode: Cardcode, Name: Name, Phone: Phone, Sex: Sex, inBirthday: inBirthday, code: code }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            $("#loading").hide();

                            if (data.msg == "1") {
                                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=codedelete", { openid: openid, comid: comid, code: code, Phone: Phone }, function (data) {
                                    data = eval("(" + data + ")");
                                    if (data.type == 100) {
                                        if (data.msg == "OK") {
                                            location.reload();
                                            return;
                                        }
                                        else {
                                            $("#spanMsg_inCode").html("操作验证码错误");
                                            $("#spanMsg_inCode").css("color", "red");
                                            return;
                                        }
                                    }
                                })

                                //location.href = "indexcard.aspx";
                                return;
                            }
                            else if (data.msg == "验证码错误") {
                                $("#spanMsg_inCode").html("验证码错误");
                                $("#spanMsg_inCode").css("color", "red");
                                $("#inCode").val(" ");
                                return;
                            }
                            else {

                                $("#spanBirthday").html(data.msg);
                                $("#spanBirthday").css("color", "red");
                                $("#inCode").val(" ");
                                return;
                            }
                        }
                        else {
                            $("#spanBirthday").html("错误，请检查后再提交");
                            $("#spanBirthday").css("color", "red");
                            $("#inCode").val(" ");
                            return;
                        }
                    })

                }


            })


            $("#recaptBtn").click(function () {
                var phone = $("#Phone").val();
                $("#recaptBtn").html("获取验证码");
                if (phone != "") {

                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=UpCode", { Phone: phone, comid: comid, id: id }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#recaptBtn").html("验证信息已发送，请注意查收");
                                //$("#confirmBtn").attr("disabled", "disabled");
                                $("#recaptBtn").removeClass("btn_code");
                                $("#recaptBtn").addClass("btn_code  disabled");
                                $("#Hid_js").html(60);
                                run();
                                return;
                            }
                            if (data.msg == "No") {
                                $("#recaptBtn").html("点累了吧，请休息一会");
                                $("#recaptBtn").removeClass("btn_code");
                                $("#recaptBtn").addClass("btn_code disabled");
                                $("#Hid_js").html(60);
                                run();
                                return;
                            }

                        }
                        else {
                            if (data.msg == "Notime") {
                                $("#recaptBtn").html("验证信息已发送，请稍后……");
                                $("#recaptBtn").removeClass("btn_code");
                                $("#recaptBtn").addClass("btn_code disabled");
                                return;
                            }
                            else {

                                $("#spanMsg_inCode").html("验证信息发送错误！");
                                return;
                            }

                        }

                    })
                } else {

                    $("#PhoneVer").html("请填写手机");
                    $("#PhoneVer").css("color", "red");
                    $("#loading").hide();
                    return;
                }
            })
        })
    </script>

    
</head>
<body>
<div id="loading" style="top: 150px; display:none;">
        <div class="lbk">
        </div>
        <div class="lcont">
            <img src="../Images/loading.gif" alt="loading...">正在加载...</div>
    </div>
    <div style="position: absolute; top: 50px; left: 431.5px; display:none;" class="footFix confirm shown helpTel" id="helptelshow" data-ffix-top="60">
             <article>
                 <h1 id="h1"></h1>
                 <a href="javascript:void(0)" id="cal" class="no">返回</a>
             </article>
    </div>
		<div class="qb_gap pg_upgrade_content">
			<div class="mod_color_weak qb_fs_s qb_gap qb_pt10">
				完善会员卡资料
			</div>

            <!-- 手机号 -->
			<div class="mod_input qb_mb10 qb_flex" id="divTel">
				<label for="_tmp_4">手　　机：</label>
				<input value="" class="flex_box" placeholder="请输入手机号" id="Phone" type="tel">
			</div>
			<span id="PhoneVer"></span>

						<!-- 姓名 -->
			<div class="mod_input qb_mb10 qb_flex" id="divName">
                                <label for="_tmp_4">姓　　名：</label>
                				<input value="" class="flex_box" placeholder="请输入姓名" id="inName" type="text">
			</div>
                                <span id="span_inName"></span>
			
						<!-- 性别 -->

            <div class="mod_input qb_mb10" id="divSex">
				性　　别：<label class="qb_mr10">
                <i class="qb_icon icon_checkbox" id="gradenameboy"></i>男 
                <i class="qb_icon icon_checkbox" id="gradenamegirl"></i>女 
                </label>
                <span id="span_sex"></span>
			</div>
            <div class="mod_input qb_mb10 qb_flex"id="divBirthYear" style=" display:none;">
				<div>
					生　　日：
				</div>
				<input value="" id="inBirth" style="display:none">

				<select class="mod_input qb_mb10 qb_flex" id="inBirthYear" style="padding:0px;margin:2px 2px 0 0;float:left;height: 31px;line-height:31px;font-size: 14px;outline: none;">
					<option value="0">年</option>
				<option value="2013">2013</option><option value="2012">2012</option><option value="2011">2011</option><option value="2010">2010</option><option value="2009">2009</option><option value="2008">2008</option><option value="2007">2007</option><option value="2006">2006</option><option value="2005">2005</option><option value="2004">2004</option><option value="2003">2003</option><option value="2002">2002</option><option value="2001">2001</option><option value="2000">2000</option><option value="1999">1999</option><option value="1998">1998</option><option value="1997">1997</option><option value="1996">1996</option><option value="1995">1995</option><option value="1994">1994</option><option value="1993">1993</option><option value="1992">1992</option><option value="1991">1991</option><option value="1990">1990</option><option value="1989">1989</option><option value="1988">1988</option><option value="1987">1987</option><option value="1986">1986</option><option value="1985">1985</option><option value="1984">1984</option><option value="1983">1983</option><option value="1982">1982</option><option value="1981">1981</option><option value="1980">1980</option><option value="1979">1979</option><option value="1978">1978</option><option value="1977">1977</option><option value="1976">1976</option><option value="1975">1975</option><option value="1974">1974</option><option value="1973">1973</option><option value="1972">1972</option><option value="1971">1971</option><option value="1970">1970</option><option value="1969">1969</option><option value="1968">1968</option><option value="1967">1967</option><option value="1966">1966</option><option value="1965">1965</option><option value="1964">1964</option><option value="1963">1963</option><option value="1962">1962</option><option value="1961">1961</option><option value="1960">1960</option><option value="1959">1959</option><option value="1958">1958</option><option value="1957">1957</option><option value="1956">1956</option><option value="1955">1955</option><option value="1954">1954</option><option value="1953">1953</option><option value="1952">1952</option><option value="1951">1951</option><option value="1950">1950</option><option value="1949">1949</option><option value="1948">1948</option><option value="1947">1947</option><option value="1946">1946</option><option value="1945">1945</option><option value="1944">1944</option><option value="1943">1943</option><option value="1942">1942</option><option value="1941">1941</option><option value="1940">1940</option><option value="1939">1939</option><option value="1938">1938</option><option value="1937">1937</option><option value="1936">1936</option><option value="1935">1935</option><option value="1934">1934</option><option value="1933">1933</option><option value="1932">1932</option><option value="1931">1931</option><option value="1930">1930</option><option value="1929">1929</option><option value="1928">1928</option><option value="1927">1927</option><option value="1926">1926</option><option value="1925">1925</option><option value="1924">1924</option><option value="1923">1923</option><option value="1922">1922</option><option value="1921">1921</option><option value="1920">1920</option><option value="1919">1919</option><option value="1918">1918</option><option value="1917">1917</option><option value="1916">1916</option><option value="1915">1915</option><option value="1914">1914</option></select>

				<!--<input  name ="upgradeinput" type="date" id="birthtxt" class="flex_box" placeholder="年" maxlength="4" />-->
				<select class="mod_input qb_mb10 qb_flex" id="inBirthMonth" style="padding:0px;margin:2px 2px 0 0;line-height:31px;float:left;height: 31px;font-size: 14px;outline: none;">
					<option value="0">月</option><option value="1">1</option><option value="2">2</option>
					<option value="3">3</option><option value="4">4</option><option value="5">5</option>
					<option value="6">6</option><option value="7">7</option><option value="8">8</option>
					<option value="9">9</option><option value="10">10</option><option value="11">11</option>
					<option value="12">12</option>
				</select>

				<!--<input  name ="upgradeinput" type="date" id="birthtxt" class="flex_box" placeholder="年" maxlength="4" />-->
				<select class="mod_input qb_mb10 qb_flex" id="inBirthDay" style="padding:0px;margin:2px 2px 0 0;line-height:31px;float:left;height: 31px;font-size: 14px;outline: none;">
					<option value="0">日</option>
					<option value="1">1</option>
					<option value="2">2</option>
					<option value="3">3</option>
					<option value="4">4</option>
					<option value="5">5</option>
					<option value="6">6</option>
					<option value="7">7</option>
					<option value="8">8</option>
					<option value="9">9</option>
					<option value="10">10</option>
					<option value="11">11</option>
					<option value="12">12</option>
					<option value="13">13</option>
					<option value="14">14</option>
					<option value="15">15</option>
					<option value="16">16</option>
					<option value="17">17</option>
					<option value="18">18</option>
					<option value="19">19</option>
					<option value="20">20</option>
					<option value="21">21</option>
					<option value="21">22</option>
					<option value="23">23</option>
					<option value="24">24</option>
					<option value="25">25</option>
					<option value="26">26</option>
					<option value="27">27</option>
					<option value="28">28</option>
					<option value="29">29</option>
					<option value="30">30</option>
					<option value="31">31</option>
				</select>
			</div>
			<span id="spanBirthday"></span>
			
						<!-- 短信验证 -->
			<div class="qb_flex qb_mb10"  id="divCode">
				<div class="mod_input flex_box qb_mr10" id="recaptchaCon">
					<div class="qb_flex">
						<label for="_tmp_5" style="display: block; width: 70px">验&nbsp;证&nbsp;码：</label>
						<input id="inCode" type="tel" class="flex_box" placeholder="请输入验证码">
					</div>
				</div>
                <a class="btn_code" href="#" id="recaptBtn"></a>
			</div>
            <span id="spanMsg_inCode"></span>
			
			<a id="confirmBtn" href="#" class="mod_btn btn_block qb_mb10">提&nbsp;交</a>
		</div>
	<input type="hidden" id="hid_id" value="<%=AccountId %>" />
    <input type="hidden" id="hid_Card" value="<%=AccountCard %>" />
    <input type="hidden" id="hid_comid" value="<%=comid %>" />
    <input type="hidden" id="hid_sex" value="<%=Accountsex %>" />
    <input type="hidden" id="hid_Name" value="<%=AccountName %>" />
    <input type="hidden" id="hid_phone" value="<%=Accountphone %>" />
    <input type="hidden" id="hid_year" value="<%=year %>" />
    <input type="hidden" id="hid_month" value="<%=month %>" />
    <input type="hidden" id="hid_day" value="<%=day %>" />
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <span style=" display:none;"  id="Hid_js">0</span>
    
    <input type="hidden" id="hid_ture" value="0" />

    <script type="text/javascript">
        function run() {
            var s = document.getElementById("Hid_js");
            if (s.innerHTML == 0) {
                $("#recaptBtn").html("获取验证码");
                $("#recaptBtn").removeClass("btn_code  disabled");
                $("#recaptBtn").addClass("btn_code");
                return false;
            }
            s.innerHTML = s.innerHTML * 1 - 1;
            $("#recaptBtn").html(s.innerHTML + "秒后获取验证码");
        }
        window.setInterval("run();", 1000);
</script>

    <!--<div class="footFix" id="footReturn">
            <a href="indexcard.aspx"><span>返回会员卡首页</span></a>
    </div>-->
    <script>
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            WeixinJSBridge.call('hideToolbar');
        });
    </script>
</body>
</html>
