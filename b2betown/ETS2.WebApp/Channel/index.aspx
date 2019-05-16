<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ETS2.WebApp.Channel.index" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <meta charset="utf-8">
    <title>大好河山旅游微商城</title>

    <link charset="utf-8" rel="stylesheet" href="css/business.css">
    <link charset="utf-8" rel="stylesheet" href="css/demo.css">
<link charset="utf-8" rel="stylesheet" href="/Styles/reg.css" />
 <style type="text/css">

.modal {
position: fixed;
top: 10%;
left: 50%;
z-index: 1050;
width: 560px;
margin-left: -280px;
background-color: white;
border: 1px solid #999;
-webkit-border-radius: 6px;
-moz-border-radius: 6px;
border-radius: 6px;
-webkit-box-shadow: 0 3px 7px rgba(0, 0, 0, 0.3);
-moz-box-shadow: 0 3px 7px rgba(0, 0, 0, 0.3);
box-shadow: 0 3px 7px rgba(0, 0, 0, 0.3);
-webkit-background-clip: padding-box;
-moz-background-clip: padding-box;
background-clip: padding-box;
outline: none;
}
.modal-header {
padding: 5px 15px;
}
button.close {
padding: 0;
cursor: pointer;
background: transparent;
border: 0;
-webkit-appearance: none;
float:right;
}
.modal-header h3 {
margin: 0;
line-height: 30px;
}

</style>
<style type="text/css">

</style>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/jquery-impromptu.4.0.min.js"></script>
<link rel="stylesheet" type="text/css" href="n/Scripts/Impromptu.css" />
<script type="text/javascript" src="/Scripts/common.js"></script>
<script src="/Scripts/hoverdelay.js" type="text/javascript"></script>
<link charset="utf-8" rel="stylesheet" href="reg.css" />
<link rel="stylesheet" href="/Styles/pc/pc_swiper.css">


<script type="text/javascript">
    $(function () {
        var comid = 0;

        //图形验证码
        $("#validateCodetext").click(function () {
            $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
        })
        $("#validateCode").click(function () {
            this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
        })



        $(".icon-preview").hoverDelay({
            hoverEvent: function () {
                $(".popover-goods").removeClass("hide");
                $(".popover-goods").removeClass("hidestate");
                //shake('popover-goods');
            },
            outEvent: function () {
                if ($(".popover-goods").hasClass("hidestate")) {
                } else {
                    $(".popover-goods").addClass("hide");
                }

            }
        });

        $(".popover-goods").hoverDelay({
            outEvent: function () {
                if ($(".popover-goods").hasClass("hidestate")) {
                } else {
                    $(".popover-goods").addClass("hide");
                }
            }
        });



        $(".popover-goods").hover(function () {
            $(".popover-goods").removeClass("hide");
            $(".popover-goods").addClass("hidestate");
        }, function () {
            $(".popover-goods").addClass("hide");
            $(".popover-goods").removeClass("hidestate");
        });


        //--------------------------商户注册-------------------------------

        $("#regis").click(function () {
            $("#regi").show();
            // $("#zhegaiceng").show();

        });
        $("#Button1").click(function () {
            $("#regi").hide();
            // $("#zhegaiceng").hide();

        });


        $("#closec").click(function () {
            $("#regic").hide();
            // $("#zhegaiceng").hide();
        });

        //商户行业类型
        $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetIndustryList", {}, function (data) {
            data = eval("(" + data + ")");
            if (data.type == 100) {
                $("#sel_hangye").empty();
                for (var i = 0; i < data.msg.length; i++) {
                    $("#com_type").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Industryname + '</option>');
                }

            }
        })


        //加载一个通用行行业，已方便注册
        $("#com_type").append('<option value="0">其他行业</option>');


        $("#regisub").click(function () {
            var Account = $("#Account").trimVal();
            var passwords = $("#passwords").trimVal();
            var qpasswords = $("#qpasswords").trimVal();

            var com_name = $("#com_name").trimVal();
            var Scenic_name = $("#Scenic_name").trimVal();
            var com_type = $("#com_type").trimVal();


            var Contact = $("#Contact").trimVal(); //联系人姓名
            var tel = $("#tel").trimVal();

            var province = $("#province").trimVal();
            var city = $("#city").trimVal();



            if (Account == "") {
                alert("登录账户不可为空");
                return;
            }

            if (passwords == "") {
                alert("密码不可为空！");
                return;
            }
            if (qpasswords == "") {
                alert("确认密码不可为空！");
                return;
            }
            if (passwords != qpasswords) {
                alert("密码和确认密码不符！");
                return;
            }
            if (com_name == "") {

                alert("单位名称不可为空！");
                return;
            }

            if (province == "" || province == "省份") {
                alert("请选择所在省份");
                return;
            }
            if (city == "" || city == "城市") {
                alert("请选择所属城市");
                return;
            }
            if (com_type == "") {
                alert("请选择所属行业");
                return;
            }

            if (Contact == "") {
                alert("联系人姓名不可为空！");
                return;
            }

            if (tel == "") {
                alert("联系电话不可为空！");
                return;
            }

            //                $('#regi').hide().after('<span id="spLoginLoading" style="margin-left:105px;height:30px;color:#f39800; ">登录中……</span>');
            $.ajax({
                type: "POST",
                url: "/JsonFactory/RegisterUser.ashx?oper=edit",
                data: { Account: Account, passwords: passwords, com_name: com_name, Scenic_name: Scenic_name, Contact: Contact, tel: tel, province: province, city: city, com_type: com_type },
                success: function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == 1) {
                        alert("账户注册提示:" + data.msg, { buttons: [{ title: "确定", value: true}], submit: function (e, v, m, f) { if (v == true) { } } });
                        return;
                    }
                    if (data.type == 100) {
                        $("#regi").hide();
                        $("#zhegaiceng").hide();
                        $("#regic").show();
                        $("#hid_regiid").val(data.result);


                    }
                }
            });
        });



        $("#upregi").click(function () {

            var scenic_intro = $("#scenic_intro").trimVal();
            var domainname = $("#domainname").trimVal();
            var comextid = $("#hid_regiid").trimVal();
            var com_code = $("#com_code").trimVal();
            var weixinname = $("#weixinname").trimVal();



            if (comextid == "") {
                alert("商家信息不可为空");
                return;
            }

            $.post("/JsonFactory/AccountInfo.ashx?oper=editcomzizhi", { comextid: comextid, scenic_intro: scenic_intro, domainname: domainname, com_code: com_code, weixinname: weixinname }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("提交出错");
                    return;
                }
                if (data.type == 100) {
                    alert("提交成功");
                    $("#regic").hide();
                    return;
                }
            })

        })
        //商户注册结束-------------------------------------------


        //--------------------分销注册---------------------------

        $("#regia").click(function () {
            //$("#regiagent").show();
            //location.href = "/Agent/Regi.aspx";
        })
        $("#closeregiagent").click(function () {
            $("#regiagent").hide();
        })
        //账号名
        $("#RAgent_Email").blur(function () {
            $("#EmailVer").html(""); //离开后先清空备注
            var Email = $("#RAgent_Email").val();
            //判断邮箱不为空
            if (Email != "") {
                $.post("/JsonFactory/AgentHandler.ashx?oper=getEmail", { Email: Email, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            //                                 $("#EmailVer").html("√");
                            //                                 $("#EmailVer").css("color", "green");
                            //                                 $("#VEmail").val(1);
                        } else {
                            alert(data.msg);
                            $("#RAgent_Email").val("");
                            return;
                        }

                    }
                })
            }
        })

        //账号名
        $("#RAgent_Phone").blur(function () {
            $("#PhoneVer").html(""); //离开后先清空备注
            var Phone = $("#RAgent_Phone").val();
            //判断手机不为空
            if (Phone != "") {
                $.post("/JsonFactory/AgentHandler.ashx?oper=getPhone", { Phone: Phone, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                        } else {
                            alert(data.msg);
                            $("#RAgent_Phone").val("");
                            return;
                        }

                    }
                })
            }
        })

        //判断密码
        $("#RAgent_QPassword1").blur(function () {
            $("#QPassword1Ver").html(""); //离开后先清空备注
            var QPassword1 = $("#RAgent_QPassword1").val();
            var Password1 = $("#RAgent_Password1").val();
            if (QPassword1 == "" || QPassword1 != Password1) {
                alert("再次填写密码错误");
                $("#RAgent_QPassword1").val("");
                return false;
            }
        })


        //提交按钮
        $("#RAgent_regisub").click(function () {
            var Email = $("#RAgent_Email").val();
            var Password1 = $("#RAgent_passwords").val();
            var QPassword1 = $("#RAgent_qpasswords").val();
            var Name = $("#RAgent_Name").val();
            var Phone = $("#RAgent_Phone").val();
            var phonecode = $("#RAgent_phonecode").val();

            var Company = $("#RAgent_Email").val();

            if (Email == "") {
                alert("请填账户");
                return;
            }
            if ($("#RAgent_VEmail").val() == 0) {
                alert("账户有误!");
                return;
            }
            if (Password1 == "") {
                alert("请填写密码!");
                return;
            }
            if (QPassword1 == "") {
                alert("请再次确认密码!");
                return;
            }

            if (QPassword1 != Password1) {
                alert("两次密码输入不符！");
                return;
            }

            if ($("#RAgent_VQPassword1").val() == 0) {
                alert("密码有误!");
                return;
            };

            if (Name == "") {
                alert("请填写姓名!");
                return;
            };

            if (Phone == "") {
                alert("请填写手机!");
                return;
            };
            if ($("#RAgent_VPhone").val() == 0) {
                alert("手机有误!");
                return;
            };
            if (phonecode == "") {
                alert("请填写短信验证码!");
                return;
            };

            //判断验证码输入是否正确
            $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms", { mobile: $("#RAgent_Phone").val(), smscode: $("#RAgent_phonecode").val(), source: "分销注册验证码" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("验证码不相符");
                    return;
                }
                if (data.type == 100) {


                    $("#loading").html("正在提交注册信息，请稍后...")

                    //创建订单
                    $.post("/JsonFactory/AgentHandler.ashx?oper=Agentregi", { agentsourcesort: 0, Email: Email, Password1: Password1, Name: Name, Tel: Phone, Phone: Phone, Company: Company }, function (data1) {
                        data1 = eval("(" + data1 + ")");
                        if (data1.type == 1) {
                            alert("注册出现错误，请刷新重新提交！");
                            return;
                        }
                        if (data1.type == 100) {
                            if (data1.msg == "OK") {
                                alert(" 您已注册成功，请等待商家为您授权！");
                                location.href = "/Agent/Login.aspx?Email=" + Email
                                return;
                            }
                            else {
                                alert("参数传递出错，请重新操作");
                                return;
                            }
                        }
                    })

                }
            })

        })


        //获取手机验证码
        $("#RAgent_getphonecode").click(function () {
            var imgcode = $("#getcode").trimVal();
            if (imgcode == "") {
                alert("请输入图形验证码!");
                return;
            }
            //判断图形验证码是否正确
            $.post("/JsonFactory/RegisterUser.ashx?oper=verifyimgcode", { imgcode: imgcode }, function (dd) {
                dd = eval("(" + dd + ")");
                if (dd.type == 1) {
                    alert("图形验证码输入不正确");
                    $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                    return;
                } else {

                    if ($.trim($("#RAgent_Phone").val()) == "") {
                        alert("请输入手机号码!");
                        return;
                    }

                    if ($.trim($("#RAgent_getphonecode").html()) == "获取短信验证码") {
                        $("#RAgent_getphonecode").attr("disabled", "disabled").css("background-color", "#f4f4f4");
                        _callSmsApi();

                    }
                }
            })

        })

    })

    function checkMobile(tel) {
        tel = $.trim(tel);
        if (tel.length != 11 || tel.substr(0, 1) != 1) {
            return false;
        }
        return true;
    }
    function _sendSmsCD() {
        var sec = parseInt($("#RAgent_getphonecode").html());
        if (sec > 1) {
            $("#RAgent_getphonecode").html((sec - 1) + "秒后可再次发送短信");
            window.setTimeout(_sendSmsCD, 1000);
        } else {
            $("#RAgent_getphonecode").html("获取短信验证码");
            $("#RAgent_getphonecode").removeAttr("disabled").css("background-color", "#FFFFFF");
        }
    }

    function _callSmsApi() {
        var imgcode = $("#getcode").trimVal();
        if (imgcode == "") {
            alert("请输入图形验证码!");
            return;
        }
        $.post("/JsonFactory/AgentHandler.ashx?oper=callvalidsms", { imgcode: imgcode, mobile: $("#RAgent_Phone").val(), comid: $("#hid_comid").trimVal(), source: "分销注册验证码" }, function (data) {
            data = eval("(" + data + ")");
            if (data.type == 1) {
                alert(data.msg);
            }
            if (data.type == 100) {
                $("#RAgent_getphonecode").html("30秒后可再次发送短信");
                window.setTimeout(_sendSmsCD, 1000);
            }
        })

    }
		
		
		
</script>
</head>
   



<body role="document" >    
   <align="center">
  
<header class="ui-header">
    <div class="ui-header-inner clearfix">
        <div class="ui-header-logo">
            <a href="javascript:;" class="js-hover logo" data-target="js-shop-info">
        大好河山旅游微商城       
      </a>
        </div>
        <div class="ui-header-nav">
            <ul class="clearfix">
     <li><a href="/">首页</a></li>  
     <li class="divide">|</li>                      
     <li><a href="/ui/shangjiaui/ProductList.aspx" target="_blank">商城</a></li>
	 <li class="divide">|</li>
     <li><a href="/ui/shangjiaui/PJList.aspx" target="_blank">全部产品</a></li>
     <li class="divide">|</li>
     <li><a href="/ui/shangjiaui/Article.aspx" target="_blank">最新文章</a></li>           </ul>
        </div>
		<div class="ui-header-nav headcenter">
		<span class="headfuwudianhua">服务电话：</span><span class="headtel">0313-4725366</span>
		 <a target="_blank" href="http://wpa.qq.com/msgrd?v=3&amp;uin=2633221350&amp;site=qq&amp;menu=yes"><img src="http:///shop.etown.cn/images/qq.png" alt="2633221350" title="2633221350" border="0"></a> 
		 <a target="_blank" href="http://wpa.qq.com/msgrd?v=3&amp;uin=200847480&amp;site=qq&amp;menu=yes"><img src="http:///shop.etown.cn/images/qq.png" alt="200847480" title="200847480" border="0"></a>
		</div>

    </div>
</header>

      <div>
	  <div class="" style=" height:50px;width: 930px;">
      <div style=" float:left;padding-top: 6px;"><img src="images/logo_1.jpg" alt="大好河山旅游微商城"  border="0"></div>
<div style=" float:right;padding:6px 0 2px 0px;">
		<span class="regiservice" id="regia"><a href="/Agent/Regi.aspx" target="_blank">注册分销商</a></span><span class="regiservice" id="regis">成为供应商</span>
		<span class="regiservice" ><i class="icon-preview" data-position="top" data-target="js-popover-goods"></i></span>
		</div>
</div>
        <div id="glume" >
          <ul class="Limg">
            <div id="banner2" class="slide-1" coor-rate="0.1" coor="default-banner" role="banner" data-banner="false">
              <div class="login-box">
                <iframe class="fn-hide" allowtransparency="true" src="/agent/Login.html" width="286" height="367" id="loginIframe" frameborder="0" scrolling="no" coor="aside-login" style="display: block;"></iframe>
                <div id="loginLoading" style="display: none;"><img width="50" height="50" alt="加载中" src="#" seed="loginLoading-iE2012062qz2UvlUVF" smartracker="on" /></div>
              </div>
              <div id="glume2" >
                <ul class="Limg">
                  <li class="bg_1" onClick="javascript:;"><a href="javascript:;">&nbsp;</a></li>
                  <li class="bg_2" onClick="javascript:;"><a href="javascript:;">&nbsp;</a></li>
                  <li class="bg_3" onClick="javascript:;"><a href="javascript:;">&nbsp;</a></li>
                  <li class="bg_4" onClick="javascript:;"><a href="javascript:;">&nbsp;</a></li>
                </ul>
				<cite class="Nubbt"> 
					<span class="on" >1</span> 
					<span >2</span> 
					<span >3</span> 
					<span >4</span>   
			    </cite>              </div>
              <script language="javascript" type="text/javascript">
                  (function () {
                      if (!Function.prototype.bind) {
                          Function.prototype.bind = function (obj) {
                              var owner = this, args = Array.prototype.slice.call(arguments), callobj = Array.prototype.shift.call(args);
                              return function (e) { e = e || top.window.event || window.event; owner.apply(callobj, args.concat([e])); };
                          };
                      }
                  })();
                  var glume = function (id) {
                      this.ctn = document.getElementById(id);
                      this.adLis = null;
                      this.btns = null;
                      this.animStep = 0.1; //动画速度0.1～0.9
                      this.switchSpeed = 5; //自动播放间隔(s)
                      this.defOpacity = 1;
                      this.tmpOpacity = 1;
                      this.crtIndex = 0;
                      this.crtLi = null;
                      this.adLength = 0;
                      this.timerAnim = null;
                      this.timerSwitch = null;
                      this.init();
                  };
                  glume.prototype = {
                      fnAnim: function (toIndex) {
                          if (this.timerAnim) { window.clearTimeout(this.timerAnim); }
                          if (this.tmpOpacity <= 0) {
                              this.crtLi.style.opacity = this.tmpOpacity = this.defOpacity;
                              this.crtLi.style.filter = 'Alpha(Opacity=' + this.defOpacity * 100 + ')';
                              this.crtLi.style.zIndex = 0;
                              this.crtIndex = toIndex;
                              return;
                          }
                          this.crtLi.style.opacity = this.tmpOpacity = this.tmpOpacity - this.animStep;
                          this.crtLi.style.filter = 'Alpha(Opacity=' + this.tmpOpacity * 100 + ')';
                          this.timerAnim = window.setTimeout(this.fnAnim.bind(this, toIndex), 50);
                      },
                      fnNextIndex: function () {
                          return (this.crtIndex >= this.adLength - 1) ? 0 : this.crtIndex + 1;
                      },
                      fnSwitch: function (toIndex) {
                          if (this.crtIndex == toIndex) { return; }
                          this.crtLi = this.adLis[this.crtIndex];
                          for (var i = 0; i < this.adLength; i++) {
                              this.adLis[i].style.zIndex = 0;
                          }
                          this.crtLi.style.zIndex = 2;
                          this.adLis[toIndex].style.zIndex = 1;
                          for (var i = 0; i < this.adLength; i++) {
                              this.btns[i].className = '';
                          }
                          this.btns[toIndex].className = 'on'
                          this.fnAnim(toIndex);
                      },
                      fnAutoPlay: function () {
                          this.fnSwitch(this.fnNextIndex());
                      },
                      fnPlay: function () {
                          this.timerSwitch = window.setInterval(this.fnAutoPlay.bind(this), this.switchSpeed * 1000);
                      },
                      fnStopPlay: function () {
                          window.clearTimeout(this.timerSwitch);
                      },
                      init: function () {
                          this.adLis = this.ctn.getElementsByTagName('li');
                          this.btns = this.ctn.getElementsByTagName('cite')[0].getElementsByTagName('span');
                          this.adLength = this.adLis.length;
                          for (var i = 0, l = this.btns.length; i < l; i++) {
                              with ({ i: i }) {
                                  this.btns[i].index = i;
                                  this.btns[i].onclick = this.fnSwitch.bind(this, i);
                                  this.btns[i].onclick = this.fnSwitch.bind(this, i);
                              }
                          }
                          this.adLis[this.crtIndex].style.zIndex = 2;
                          this.fnPlay();

                      }
                  };
                  var player1 = new glume('glume');
            </script>
            </div>
          </ul>
        </div>
	</div>
	
	<div class="nm_advertising">
            <span><img src="http://shop.etown.cn/agent/images/laba.png"></span>  <a>公告：大好河山票务分销平台上线啦！ 3分钟注册分销账户，在线下单更方便！</a>
    </div>

    <div class="footer" coor="footer">
         <div class="grid-990 sitelink fn-clear" coor="sitelink" role="contentinfo">
			<br>
             <ul class="ui-link fn-clear">
			   <li class="ui-link-item">   大好河山票务分销平台（张家口大好河山旅行社）</li>	
         	   <li class="ui-link-item"> 分销合作咨询：0313-4725366   崇礼旅游接待：张家口崇礼县雪绒花酒店大堂内</li>		
             </ul>	
         </div>
		  <div class="grid-990 sitelink fn-clear bottomcom" coor="sitelink" role="contentinfo">
		 易城商户 平台技术支持
		 </div>
    </div>

	
<div id="regi" class="modal hide  in"  style="  position: absolute;height: 650px; width: 782px; left: 45%; border-top-left-radius: 12px; border-top-right-radius: 12px; border-bottom-right-radius: 12px; border-bottom-left-radius: 12px; z-index: 901; display: none; " >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true" id="Button1" style=" font-size:18px;">
                    ×</button>
            </div>
            <div id="contentinfo" style="padding: 0; line-height: 25px; width: 650px; height: 355px; overflow-y: auto; margin: 10px auto 0 auto;">
            <div id="showli" style="height: 430px; width: 350px; left: 0; top: 30px; z-index: 99999; position: absolute; display: ">

                        <h3 class="ui-form-title" style="padding-left:430px;">
                        <strong>商户注册</strong></h3>

                <form name="Regi" method="post" action="" target="_parent">
                <div class="grid-780  fn-clear" style=" padding-top:15px;">
                    <h3 class="ui-form-title">
                        <strong>填写登录信息</strong><span class="explain">请填写您的账户登录名和密码</span></h3>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                                登陆账户</label>
                            <input name="Account" type="text" id="Account" maxlength="250" size="20" value=""
                                class="ui-input" autocomplete="off">
                            <label id="lblaccountmsg">
                            </label>
                        </div>
                        <div class="ui-form-item">
                            <label for="payPwdConfirm" class="ui-label">
                                登录密码</label>
                            <input name="passwords" type="password" id="passwords" size="12" maxlength="50" class="ui-input"
                                data-explain="请输入登录密码">
                        </div>
                        <div class="ui-form-item">
                            <label for="payPwdConfirm" class="ui-label">
                                再次输入密码</label>
                            <input name="qpasswords" type="password" id="qpasswords" size="12" maxlength="50"
                                class="ui-input" data-explain="请再次输入登录密码。">
                        </div>
                    </div>
                    <div class="ui-form-dashed">
                    </div>
                    <h3 class="ui-form-title">
                        <strong>填写商家信息</strong><span class="explain">请准确填写商家单位名称和相关信息</span></h3>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                                单位名称</label>
                            <input name="com_name" type="text" id="com_name" size="40" maxlength="250" class="ui-input2"
                                autocomplete="off">
                            <label id="lblcompanyname">
                            </label>
                        </div>
                    </div>
                    <div class="ui-form-group" style=" display:none;">
                        <div class="ui-form-item">
                            <label for="payPwdConfirm" class="ui-label">
                                景区名称/简称</label>
                            <input name="Scenic_name" type="text" id="Scenic_name" size="20" maxlength="250"
                                class="ui-input2" autocomplete="off">
                        </div>
                    </div>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                             所在城市</label>
                            <select name="province" id="province" class="ui-input">  
                                <option value="省份" selected="selected" >省份</option>  
                            </select>  
                            <select name="city" id="city"  class="ui-input">  
                                <option value="城市" selected="selected">市县</option>  
                            </select>  

                        </div>
                    </div>
                   <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                             所属行业</label>
                            <select name="com_type" id="com_type" class="ui-input">  
                                <option value="" selected="selected" >请选择所属行业</option>  
                            </select>  
                        </div>
                    </div>
            
                    <h3 class="ui-form-title">
                        <strong>联系人信息</strong><span class="explain"><span class="ft-orange">请准确填写联系人信息</span></span></h3>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="realName" class="ui-label">
                                联系人姓名</label>
                            <input name="Contact" type="text" id="Contact" maxlength="250" size="20" class="ui-input"
                                autocomplete="off">
                        </div>
                        <div class="ui-form-item">
                            <label for="IDCardNo" class="ui-label">
                                联系电话</label>
                            <input name="tel" type="text" id="tel" maxlength="50" size="20" autocomplete="off"
                                class="ui-input ui-input-170">
                            <label id="lbltel">
                            </label>
                        </div>
                    </div>
                    <div class="ui-form-item">
                        <div style=" float:left;" id="submitBtn" class="ui-button  ui-button-morange ">
                            <input id="regisub" type="button" class="ui-button-text" value="确  定" />
                        </div>
                        <div style=" float:left;"><a href="http://shop1143.etown.cn/v/about.aspx?id=2668">《易城商户注册协议》</a></div>          
                                            <br/>
                                          
                            <br/>
                        <span class="ui-form-confirm"><span class="loading-text fn-hide">正在提交信息</span></span>
                    </div>
                </div>
                </form>



                <div style="padding: 0; line-height: 25px; width: 350px; height: 355px; overflow-y: auto; margin: 10px auto 0 auto;">
                    <dl id="titellist">
                    </dl>
                </div>
            </div>
        </div>
</div>

<div id="regic" class="modal hide  in"  style="  position: absolute;height: 560px; width: 782px; left: 45%; border-top-left-radius: 12px; border-top-right-radius: 12px; border-bottom-right-radius: 12px; border-bottom-left-radius: 12px; z-index: 1999; display:none; " >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true" id="closec" style=" font-size:18px;">
                    ×</button>

            </div>

                <div class="grid-780 fn-clear" style=" height:240px; padding-top:80px;">

                    <h3 class="ui-form-title">
                        <span style="font-size: 18px;line-height: 18px;">你已成功提交商户注册信息，请等待开通确认通知。 <br>
继续填写以下信息将帮助我们加快审核。</span>
                    </h3>
<div class="ui-form-dashed">
                    </div>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                                企业服务介绍</label>
                                <textarea name="scenic_intro" cols="50" rows="5" class="mi-input" id="scenic_intro" ></textarea>
                            <label id="Label1">
                            </label>
                        </div>
                    </div>

                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                                官网网址</label>
                            <input name="domainname" type="text" id="domainname" size="40" maxlength="250" class="ui-input2" autocomplete="off">
                            <label id="Label2">
                            </label>
                        </div>
                    </div>

                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                                企业营业执照注册号</label>
                          <input name="com_code" type="text" id="com_code" size="20" maxlength="250" class="ui-input2" autocomplete="off">
                            <label id="Label3">
                            </label>
                        </div>
                    </div>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                                微信公众账号</label>
                          <input name="weixinname" type="text" id="weixinname" size="20" maxlength="250" class="ui-input2" autocomplete="off">
                            <label id="Label4">
                            </label>
                        </div>
                    </div>

                    <div class="ui-form-item">
                    <div style=" float:left;" id="Div1" class="ui-button  ui-button-morange ">
                            <input id="upregi" type="button" class="ui-button-text" value="完善注册信息" />
                        </div>
</div>
                    <div class="ui-form-dashed">
                    </div>
                </div>

</div>



<div id="regiagent" class="modal hide  in"  style="position: absolute;height: 500px; width: 650px; left: 45%; border-top-left-radius: 12px; border-top-right-radius: 12px; border-bottom-right-radius: 12px; border-bottom-left-radius: 12px; z-index: 901; display: none; " >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true" id="closeregiagent" style=" font-size:18px;">
                    ×</button>
            </div>
            <div id="Div2" style="padding: 0; line-height: 25px; width: 650px; height: 355px; overflow-y: auto; margin: 10px auto 0 auto;">
            <div id="Div3" style="height: 430px; width: 350px; left: 0; top: 30px; z-index: 99999; position: absolute; display: ">

                <h3 class="ui-form-title" style="padding-left:430px;">
                        <strong>分销商注册</strong></h3>

                <form name="Regi" method="post" action="" target="_parent">
                <div class="grid-780  fn-clear" style=" padding-top:15px;">
                    <h3 class="ui-form-title">
                        <strong>填写登录信息</strong><span class="explain">请填写您的账户登录名和密码</span></h3>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="payPwd" class="ui-label">
                                登陆账户</label>
                            <input name="RAgent_Email" type="text" id="RAgent_Email" maxlength="250" size="20" value=""
                                class="ui-input" autocomplete="off">
                            <label id="EmailVer">
                            </label>
                        </div>
                        <div class="ui-form-item">
                            <label for="payPwdConfirm" class="ui-label">
                                登录密码</label>
                            <input name="RAgent_passwords" type="password" id="RAgent_passwords" size="12" maxlength="50" class="ui-input"
                                data-explain="请输入登录密码">
                        </div>
                        <div class="ui-form-item">
                            <label for="payPwdConfirm" class="ui-label">
                                再次输入密码</label>
                            <input name="RAgent_qpasswords" type="password" id="RAgent_qpasswords" size="12" maxlength="50"
                                class="ui-input" data-explain="请再次输入登录密码。">
                        </div>
                    </div>
                    <div class="ui-form-dashed">
                    </div>
                    
                    <h3 class="ui-form-title">
                        <strong>联系人信息</strong><span class="explain"><span class="ft-orange">请准确填写联系人信息</span></span></h3>
                    <div class="ui-form-group">
                        <div class="ui-form-item">
                            <label for="realName" class="ui-label">
                                联系人姓名</label>
                            <input name="RAgent_Name" type="text" id="RAgent_Name" maxlength="250" size="20" class="ui-input"
                                autocomplete="off">
                        </div>


                         <div class="ui-form-item">
                            <label class="ui-label">
                                图形验证码
                            </label>
                            <input name="getcode" type="text" placeholder="图形验证码" id="getcode" size="10" class="i-text i-text-authcode ui-input sl-checkcode-input" />
                            <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码" />
                            <a href="javascript:;" id="validateCodetext">更换</a>
                        </div>
                        <div class="ui-form-item">
                            <label for="IDCardNo" class="ui-label">
                                联系人手机</label>
                            <input name="RAgent_Phone" type="text" id="RAgent_Phone" maxlength="50" size="20" autocomplete="off"
                                class="ui-input ui-input-170"><a  id="RAgent_getphonecode" style="text-decoration:underline; cursor:pointer;">获取短信验证码</a>
                            <label id="Label5">
                            </label>
                        </div>
                        <div class="ui-form-item">
                            <label for="IDCardNo" class="ui-label">
                                短信验证码</label>
                            <input name="RAgent_phonecode" type="text" id="RAgent_phonecode" maxlength="50" size="20" autocomplete="off"
                                class="ui-input ui-input-170">
                            <label id="Label6">
                            </label>
                        </div>
                    </div>
                    <div class="ui-form-item">
                        <div style=" float:left;" id="Div4" class="ui-button  ui-button-morange ">
                            <input id="RAgent_regisub" type="button" class="ui-button-text" value="确  定" />
                        </div>
        
                                            <br/>
                                          
                            <br/>
                        <span class="ui-form-confirm"><span class="loading-text fn-hide">正在提交信息</span></span>
                    </div>
                </div>
                </form>



                <div style="padding: 0; line-height: 25px; width: 350px; height: 355px; overflow-y: auto; margin: 10px auto 0 auto;">
                    <dl id="Dl1">
                    </dl>
                </div>
            </div>
        </div>
</div>

    <script type="text/javascript">
        var province = document.getElementById('province');
        var city = document.getElementById('city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>

    <div class="hide popover1 popover-goods js-popover-goods " id="popover-goods" style="right: 160px; top: 125px;">
      <div class="popover-inner">
        <h4 class="title clearfix"><span class="icon-weixin pull-left"></span>手机启动微信<br>扫一扫购买</h4>
        <div class="ui-goods-qrcode">
          <img id="proerweima" src="http://open.weixin.qq.com/qr/code/?username=zjktour" alt="二维码" class="qrcode-img">
        </div>
      </div>
    </div>


</align>

</body>

</html>
