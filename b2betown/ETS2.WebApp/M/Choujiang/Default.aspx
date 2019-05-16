<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ETS2.WebApp.M.Choujiang.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
<meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
<meta http-equiv="Pragma" content="no-cache" />
<meta http-equiv="Expires" content="0" />

<meta name="viewport" content="width=device-width,height=device-height,inital-scale=1.0,maximum-scale=1.0,user-scalable=no;"/>
<meta name="apple-mobile-web-app-capable" content="yes"/>
<meta name="apple-mobile-web-app-status-bar-style" content="black"/>
<meta name="format-detection" content="telephone=no"/>
<link href="css/bootstrap.min.css?<%=shijianchuo %>" rel="stylesheet"/>
<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
<script src="js/jquery.alerts.js" type="text/javascript"></script>
<link href="css/jquery.alerts.css" rel="stylesheet" type="text/css" media="screen" />
<link rel="stylesheet" href="css/jquery.mobile-1.3.2.min.css" />
<script src="js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>
<link href="css/huodong.css?<%=shijianchuo %>" rel="stylesheet" type="text/css" media="screen" />
<link href="css/zf.css?<%=shijianchuo %>" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {

        //获取已有活动列表
        $.ajax({
            type: "post",
            url: "../../JsonFactory/Choujiang.ashx?oper=huojiang&openid=<%=openid%>&t=<%=shijianchuo %>",
            data: {id: <%=actid %> },
            async: false,
            success: function (data) {
                data = eval("(" + data + ")");

                if (data.type == 100) {
                    if (data.totalCount == 0) {
                    } else {
                       var htmlstr = "";
	                       htmlstr = htmlstr + '<div class=\"yj mg_top mg_down\"><p class=\"yj hd_bt\">中奖名单：</p>'
						   htmlstr = htmlstr + '<div style=\"padding:0 10px;\">'

                           $.each(data.msg, function (i, item) {
                               htmlstr = htmlstr + '<p>'+ ' ' + data.msg[i].Name + ' ' + data.msg[i].phone  + ' ' + data.msg[i].Award_class + '</p>'
        				   })
                           htmlstr = htmlstr + '</div></div>'
                           $("#zhongjiangmingdan").append(htmlstr);
                    }


                }
            }
        })
    })
</script>


 <script type="text/javascript">
     function cli_hide2() {
         $("#layer_mask2").hide();
         $('#forward2').hide();
     }

     function cli_zhuanfa2() {
         $("#layer_mask2").show();
         $('#forward2').show();
     }



    </script>
<style>
.cover {
	width: 100%;
	max-width: 480px;
	margin: 0 auto;
	position: relative;
}

.cover img {
	width: 100%;
}

#outer-cont {
	position: absolute;
	width: 100%;
	top: 50px;
}

#inner-cont {
	position: absolute;
	width: 100%;
	top: 96px;
}

#outer {
	width: 220px;
	max-width: 220px;
	height: 220px;
	margin: 0 auto;
}

#inner {
	width: 86px;
	max-width: 86px;
	height: 114px;
	margin: 0 auto;
	cursor: pointer;
}

#outer img,#inner img {
	display: block;
	margin: 0 auto;
}

.content {
	margin-top: 150px;
	padding: 0 15px;
}

.content .desc {
	font-weight: bold;
	border-bottom: 1px dashed #000;
	padding: 12px 0px;
}

.loading-mask {
	width: 100%;
	height: 100%;
	position: fixed;
	background: rgba(0, 0, 0, 0.6);
	z-index: 100;
	left: 0px;
	top: 0px;
}
</style>
<title><%=Title %></title> 
<script	type="text/javascript">
                        function loading(canvas, options) {
                            this.canvas = canvas;
                            if (options) {
                                this.radius = options.radius || 12;
                                this.circleLineWidth = options.circleLineWidth || 4;
                                this.circleColor = options.circleColor || 'lightgray';
                                this.moveArcColor = options.moveArcColor || 'gray';
                            } else {
                                this.radius = 12;
                                this.circelLineWidth = 4;
                                this.circleColor = 'lightgray';
                                this.moveArcColor = 'gray';
                            }
                        }
                        loading.prototype = {
                            show: function () {
                                var canvas = this.canvas;
                                if (!canvas.getContext) return;
                                if (canvas.__loading) return;
                                canvas.__loading = this;
                                var ctx = canvas.getContext('2d');
                                var radius = this.radius;
                                var me = this;
                                var rotatorAngle = Math.PI * 1.5;
                                var step = Math.PI / 6;
                                canvas.loadingInterval = setInterval(function () {
                                    ctx.clearRect(0, 0, canvas.width, canvas.height);
                                    var lineWidth = me.circleLineWidth;
                                    var center = { x: canvas.width / 2, y: canvas.height / 2 };
                                    ctx.beginPath();
                                    ctx.lineWidth = lineWidth;
                                    ctx.strokeStyle = me.circleColor;
                                    ctx.arc(center.x, center.y + 20, radius, 0, Math.PI * 2);
                                    ctx.closePath();
                                    ctx.stroke();
                                    ctx.beginPath();
                                    ctx.strokeStyle = me.moveArcColor;
                                    ctx.arc(center.x, center.y + 20, radius, rotatorAngle, rotatorAngle + Math.PI * .45);
                                    ctx.stroke();
                                    rotatorAngle += step;
                                }, 100);
                            },
                            hide: function () {

                                var canvas = this.canvas;
                                canvas.__loading = false;
                                if (canvas.loadingInterval) {
                                    window.clearInterval(canvas.loadingInterval);
                                }
                                var ctx = canvas.getContext('2d');
                                if (ctx) ctx.clearRect(0, 0, canvas.width, canvas.height);
                            }
                        };  
</script>

</head>

<body style="overflow-x: hidden;">
     <div class="layer-mask" id="layer_mask2" style="z-index: 10002;display:none;  position: absolute;    position: fixed;    width: 100%;    height: 100%;    background-color: #1b1b1b;    opacity: 0.9;    top: 0;left: 0;" onClick="cli_hide2()"></div>
      <div class="helper forward" id="forward2"></div>
	<div data-role="page" id="main">
		<div data-role="content" style="padding: 0;">
			<div class="loading-mask">
				<div
					style="position: absolute; left: 50%; margin-left: -150px; text-align: center;">
					<canvas id="loading" style="width: 240px;height: 150px;">loading</canvas>
					<div style="color: white; font-size: 20px;">正在加载中...</div>
				</div>
			</div>
			<script type="text/javascript">
			    var loadingObj = new loading(document.getElementById('loading'), { radius: 20, circleLineWidth: 8 });
			    loadingObj.show();   
	        </script>
			<div>
				<div>
					<div style="min-width: 450px;">
						<div class="cover">
							<img src="images/bj.jpg" />
						</div>
						<div id="outer-cont">
							<div id="outer">
								<img src="images/zp3.png" />
							</div>
						</div>
						<div id="inner-cont">
							<div id="inner">
								<img src="images/zhuanzhen.png" />
							</div>
						</div>
					</div>
					<div class="yj mg_top" id="duijiang" style="display:none;">
						<p class="yj hd_bt">中奖结果：</p>
						<div style="padding:0 10px;">
							<h3 style="color:Red" id="zhongjiang_desc"></h3>
							<p><b>您的中奖码：<span id="zhongjiang_sn"></span></b></p>
							<p>请按照活动说明，联系商家兑现奖品!</p>
                    		<a href="#dj_dialog" data-rel="popup" data-position-to="window" data-role="button" data-theme="b" data-transition="pop">我要兑奖</a>
						</div>
					</div>
                    <%if (actstate == 0)
                      { %>
                       <div class="yj mg_top" style=" padding:10px 0 0 10px;">
                        <p>暂无抽奖活动，敬请继续关注</p>
                    </div>

                    <%}else{ %>


                    <div class="yj mg_top" style=" padding:10px 0 0 10px;">
                        <p>今天还有<strong> <span style='color:red;' id="choujiangcishu"><%=choujiangcishu%></span> </strong> 次抽奖机会</p>
                    </div>
					<div class="yj mg_top">
						<p class="yj hd_bt">活动奖项：</p>
						<div style="padding:0 10px;">
                        
						<p><strong>一等奖：</strong><%=Award_title1%><span style='color:red;'><%=Award_num1%></span>名</p>
                        <%if (Award_title2 != "")
                          {  %>
                        <p><strong>二等奖：</strong><%=Award_title2%> <span style='color:red;'><%=Award_num2%></span>名</p>
                        <%} %>
                        <%if (Award_title3 != "")
                          {  %>
                        <p><strong>三等奖：</strong><%=Award_title3%> <span style='color:red;'><%=Award_num3%></span>名</p>						
                        <%} %>
                        </div>
					</div>
					<div class="yj mg_top mg_down">
						<p class="yj hd_bt">活动说明：</p>
						<div style="padding:0 10px;">
							<p><strong><%=Title%></strong></p>
							<p>活动时间：<%=ERNIE_star%> ~ <%=ERNIE_end%></p>
							<p>活动说明：<%=Remark%></p>
						</div>
					</div>
                    <div id="zhongjiangmingdan">
                    
                    </div>
                    <%} %>
				</div>
			</div>
		</div>
        <div style="max-width:90%;margin:0 auto 50px;padding-bottom: 20px">
        <!--<button onclick="cli_zhuanfa2()" type="button">     <span>转发分享到朋友圈</span></button> -->
        </div>
   			
 <style type="text/css">
#dj_dialog a:hover,.smDiv a,.smDiv a:hover{ 
	text-decoration:none;
}
</style>

<script type="text/javascript">
    var lastuserid = 0;
    isUptel = false;
    function up_hdtel() {
        if (isUptel) {
            jAlert("您的手机号码正在提交，请稍等！", "数据处理中");
        }

        hy_tel = $("#hy_tel").val();
        hy_tel = $.trim(hy_tel);
        if (hy_tel == "") {
            jAlert("请填写您的手机号码", "填写错误");
            return false;
        }
        if (hy_tel.length != 11) {
            jAlert("请输入正确的手机号码", "填写错误");
            return false;
        }

        hduname = $("#hduname").val();
        if (hduname.length > 10) {
            jAlert("您的姓名过长！", "填写错误");
            return false;
        }

        isUptel = true;
        $.ajax({
            url: "../../JsonFactory/Choujiang.ashx?ygj=huodong&t=<%=shijianchuo %>&openid=<%=openid %>&id=<%=actid %>&udid=" + lastuserid + "&oper=huodong_tel",
            data: "hdtel=" + hy_tel + "&hduname=" + hduname,
            dataType: "json",
            success: function (data) {

                isUptel = false;
                rt = data.rt;
                if (rt == -1) {
                    jAlert("提交失败，请重新尝试！", "错误信息");
                }
                else if (rt == 1) {
                    $.mobile.changePage("#main");
                    jAlert(data.msg, "保存成功");
                    setTimeout(function () {

                        window.location.href = "Default.aspx?t=" +Math.random(); 
                    }, 2000);
                   
                }
            }
        });

        return false;
    }
</script>

<div data-role="popup" id="dj_dialog" data-overlay-theme="a" data-theme="c">
    <div data-role="header" data-theme="a">
        <h1>我要兑奖</h1>
    </div>
    <div data-role="content" data-theme="d">
        <p>请输入您的手机号码和姓名，商家可以联系您进行兑奖。</p>
        <input name="hy_tel" id="hy_tel" placeholder="请输入正确的手机号" value="<%=Accountphone %>" type="text">
        <input name="hduname" id="hduname" placeholder="请输入姓名" value="<%=Accountname %>" type="text">
        <a href="#" data-role="button" data-inline="true" data-rel="back" data-theme="c">取消</a>
        <a href="#" data-role="button" data-inline="true" data-theme="b" onclick="return up_hdtel()">马上提交</a>
    </div>
</div>    </div>


	<script type="text/javascript">
	    function showprize(prize, sncode, show) {
	        var prize_desc = '';
	        switch (prize) {
	            case 1:
	                prize_desc = "恭喜您获得一等奖";
	                break;
	            case 2:
	                prize_desc = "恭喜您获得二等奖";
	                break;
	            case 3:
	                prize_desc = "恭喜您获得三等奖";
	                break;
	            case 4:
	                prize_desc = "恭喜您获得四等奖";
	                break;
	            case 5:
	                prize_desc = "恭喜您获得五等奖";
	                break;
	            case 6:
	                prize_desc = "恭喜您获得六等奖";
	                break;
	            case 7:
	                prize_desc = "恭喜您获得七等奖";
	                break;
	            case 8:
	                prize_desc = "恭喜您获得八等奖";
	                break;
	        }
	        $("#zhongjiang_desc").html(prize_desc);
	        $("#zhongjiang_sn").html(sncode);
	        if (show)
	            $("#duijiang").fadeIn("normal");
	    }

	    $(function () {
	        var prize = parseInt('0');
	        var sncode = '';
	        if (prize > 0) {
	            sncode = '';
	            showprize(prize, sncode, true);

	        }
	        else {
	            prize = null;
	        }
	        {
	            window.requestAnimFrame = (function () {
	                return window.requestAnimationFrame ||
    				window.webkitRequestAnimationFrame ||
    				window.mozRequestAnimationFrame ||
    				window.oRequestAnimationFrame ||
    				window.msRequestAnimationFrame ||
    				function (callback) {
    				    window.setTimeout(callback, 20);
    				};
	            })();

	            var totalDeg = 360 * 3 + 0; //总距离,两个部分，第一部分360*10为至少转多少圈，第二部分15是最终要停住的位置(单位都是度)
	            var steps = []; //步值
	            // 不中奖的位置
	            var lostDeg = [15, 75, 195, 315];
	            // 中奖的位置
	            var prizeDeg = [345, 225, 105, 45, 285, 165, 135, 255, ];
	            // 奖品

	            var count = 0;
	            var now = 0;
	            var a = 0.02; //减速度
	            var outter, inner, timer, running = false;
	            function countSteps() {
	                var t = Math.sqrt(2 * totalDeg / a);
	                var v = a * t;
	                for (var i = 0; i < t; i++) {
	                    steps.push((2 * v * i - a * i * i) / 2);
	                }
	                steps.push(totalDeg);
	            }
	            function step() {
	                outter.style.webkitTransform = 'rotate(' + steps[now++] + 'deg)';
	                outter.style.MozTransform = 'rotate(' + steps[now++] + 'deg)';
	                //console.log(steps[now]);
	                if (now < steps.length) {
	                    requestAnimFrame(step);
	                } else {
	                    setTimeout(function () {
	                        if (prize != null) {
	                            $("#duijiang").fadeIn("normal");
	                        } else {
	                            jAlert("感谢您的参与，请下次努力！", "谢谢参与");
	                        }
	                        running = false;
	                    }, 200);
	                }
	            }

	            // 不输入deg则默认抽不中奖
	            function start(deg) {

	                deg = deg || lostDeg[parseInt(lostDeg.length * Math.random())];
	                running = true;
	                clearInterval(timer);
	                totalDeg = 360 * 5 + deg;
	                steps = [];
	                now = 0;
	                countSteps();
	                requestAnimFrame(step);
	            }
	            window.start = start;
	            outter = document.getElementById('outer');
	            inner = document.getElementById('inner');
	            i = 10;
	            $("#inner").click(function () {
	                if ($("#choujiangcishu").text() < 1) {
	                    jAlert("您已经没有抽奖次数了，请关注更多抽奖活动！", "错误信息");
	                    clearInterval(timer);
	                    running = false;
	                    return;
                    }

	                if (running) return;
	                if (prize != null) { jAlert("亲，您已中奖，先去找商家领奖哦！", "您已中奖"); return; }
	                running = true;

	                //没点击一次 -1抽奖次数
	                $("#choujiangcishu").text($("#choujiangcishu").text() - 1);

	                $.ajax({
	                    url: "../../JsonFactory/Choujiang.ashx?ygj=huodong&openid=<%=openid%>&id=<%=actid %>&oper=dzp_choujiang&t="+Math.random(),
	                    dataType: "json",
	                    beforeSend: function () {

	                        timer = setInterval(function () {
	                            i += 10;

	                            //outter.style.webkitTransform = 'rotate(' + i + 'deg)';
	                            //outter.style.MozTransform = 'rotate(' + i + 'deg)';
	                        }, 5);
	                    },
	                    success: function (data) {

	                        if (data.error != "") {
	                            jAlert(data.error, "错误信息");
	                            clearInterval(timer);
	                            running = false;
	                            return;
	                        }
	                        if (data.success) {
	                            prize = data.prizetype;
	                            sncode = data.sn;
	                            start(prizeDeg[data.prizetype - 1]);
	                            showprize(prize, sncode, false);

	                            //更新lastuserid,用来保存电话号码的那个数据用
	                            lastuserid = data.insert_id;
	                        } else {
	                            prize = null;
	                            start();
	                        }
	                        count++;
	                    },
	                    error: function () {

	                        prize = null;
	                        start();
	                        count++;
	                    },
	                    timeout: 4000
	                });
	            });
	        }

	    });

	    loadingObj.hide();
	    $(".loading-mask").remove();
</script>
</body>
</html>
