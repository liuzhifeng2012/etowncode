<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="subevaluate.aspx.cs" Inherits="ETS2.WebApp.H5.subevaluate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="text/html;charset=utf-8" http-equiv="Content-Type"></meta>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;" name="viewport"></meta>
    <meta content="telephone=no" name="format-detection"></meta>
	<link onerror="_cdnFallback(this)" href="order/css/css.css" rel="stylesheet"> 
    <link onerror="_cdnFallback(this)" href="order/css/star.css" rel="stylesheet"> 
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
        
        .area {
    width: 100%;
    height: 88px;
}
.mod_color_weak
{
    font-size:16px;
    }
    body {
    background-color: #ffffff !important;
    font-size: 14px;
}
.mod_input {

    border: 0px none  !important;
   
}

    </style>
    <link href="../Styles/weixin/wei_bind.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>

    <script src="/Scripts/common.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").val();
            var uid = $("#hid_uid").val();
            var evatype = $("#hid_evatype").val();
            var oid = $("#hid_oid").val();



            $.post("/JsonFactory/OrderHandler.ashx?oper=evaluatePageinfo", { id: oid,evatype:evatype }, function (wdata) {
                wdata = eval("(" + wdata + ")");

                if (wdata.type == 100) {
                    if (wdata.msg != "") {
                        $("#xinde").val(wdata.msg[0].text);
                        $(".star" + wdata.msg[0].starnum).addClass("active");
                    } else {
                        $("#loading").hide();
                        $("#StarVer").html(wdata.msg);
                        $("#StarVer").css("color", "red");
                        return;
                    }
                }
            })

            //提交按钮
            $("#confirmBtn").click("click", function () {
                var area = $(".area").val();
                var star = $("#hid_star").val();
                var openid = $("#hid_openid").val();
                $("#StarVer").html("");
                $("#AreaVer").html("");

                if (star == "0") {
                    $("#StarVer").html("请选择平分");
                    $("#StarVer").css("color", "red");
                    $("#loading").hide();
                    return;
                }

                if (area == "" || area == "请输入对服务的满意度评价~") {
                    $("#AreaVer").html("请填写验留言，最少5个字");
                    $("#AreaVer").css("color", "red");
                    $("#loading").hide();
                    return;
                }

                $.post("/JsonFactory/OrderHandler.ashx?oper=subevaluate", { openid: openid, comid: comid, uid: uid, star: star, evatype: evatype, oid: oid, area: area }, function (wdata) {
                    wdata = eval("(" + wdata + ")");

                    if (wdata.type == 100) {
                        alert("评价完成");
                        window.location.href = "/h5/order/order.aspx";
                    } else {
                        $("#loading").hide();
                        $("#StarVer").html(wdata.msg);
                        $("#StarVer").css("color", "red");
                        return;
                    }
                })
            })


            $(".star1").click(function () {
                $(".star1").addClass("active");
                $(".star2").removeClass("active");
                $(".star3").removeClass("active");
                $(".star4").removeClass("active");
                $(".star5").removeClass("active");
                $("#hid_star").val(1);
            })
            $(".star2").click(function () {
                $(".star1").removeClass("active");
                $(".star2").addClass("active");
                $(".star3").removeClass("active");
                $(".star4").removeClass("active");
                $(".star5").removeClass("active");
                $("#hid_star").val(2);
            })
            $(".star3").click(function () {
                $(".star1").removeClass("active");
                $(".star2").removeClass("active");
                $(".star3").addClass("active");
                $(".star4").removeClass("active");
                $(".star5").removeClass("active");
                $("#hid_star").val(3);
            })
            $(".star4").click(function () {
                $(".star1").removeClass("active");
                $(".star2").removeClass("active");
                $(".star3").removeClass("active");
                $(".star4").addClass("active");
                $(".star5").removeClass("active");
                $("#hid_star").val(4);
            })
            $(".star5").click(function () {
                $(".star1").removeClass("active");
                $(".star2").removeClass("active");
                $(".star3").removeClass("active");
                $(".star4").removeClass("active");
                $(".star5").addClass("active");
                $("#hid_star").val(5);
            })

            $(".area").click(function () {
                var area_temp = $(".area").val();
                if (area_temp == "请输入对服务的满意度评价~") {
                    $(".area").val("");
                    $(".area").removeClass("area01");
                }

            })

        })

        function inputarea() {

            var area_temp = $(".area").val();
            if (area_temp == "") {
                $(".area").addClass("area01");
                $(".area").val("请输入对服务的满意度评价~");
                
            }
        
         }
    </script>

    
</head>
<body>
<div id="loading" style="top: 150px; display:none;">
        <div class="lbk">
        </div>
        <div class="lcont">
            <img src="../Images/loading.gif" alt="loading...">正在加载...</div>
    </div>
      <div class="header">
        <!-- ▼顶部通栏 -->
                                
            <div class="js-mp-info share-mp-info">
				<a href="/h5/order/Default.aspx" class="page-mp-info">
					<img width="24" height="24" src="<%=logoimg %>" class="mp-image">
					<i class="mp-nickname">
						<%=title %>                </i>
				</a>
            <div class="links">
                
                                                                </div>
        </div>
        <!-- ▲顶部通栏 -->
</div>   
		<div class="qb_gap pg_upgrade_content">
			<div class="mod_color_weak qb_gap qb_pt10" id="proname">
				<%=Pro_name%>
			</div>

            <span id="StarVer"></span>
			<div class="mod_input qb_mb10 qb_flex" id="star">
				<label for="_tmp_4">评分：</label>
				<span class="commstar"><a href="javascript:;" class="star1"></a><a href="javascript:;" class="star2"></a><a href="javascript:;" class="star3 "></a><a href="javascript:;" class="star4" ></a><a href="javascript:;" class="star5"></a></span>
			</div>
			

			 <span id="AreaVer"></span>
			<div class="mod_input qb_mb10 qb_flex" id="divName">
                                <label for="_tmp_4">心得：</label>

                                <textarea id="xinde" name="" cols="" rows="" class="area area01" style=" margin-bottom:6px;border: 0px none;width: 100%; min-width:250px;" onBlur="inputarea();">请输入对服务的满意度评价~</textarea>
			</div>
                               


			<a id="confirmBtn" href="#" class="mod_btn btn_block qb_mb10">提&nbsp;交</a>
		</div>
	<input type="hidden" id="hid_uid" value="<%=AccountId %>" />
    <input type="hidden" id="hid_comid" value="<%=comid %>" />
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_evatype" value="<%=evatype %>" />
    <input type="hidden" id="hid_oid" value="<%=oid %>" />
    <input type="hidden" id="hid_star" value="0" />
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
