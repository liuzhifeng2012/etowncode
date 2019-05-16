<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ETS2.WebApp.admin.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <meta charset="gb2312">
    <title>管理后台</title>

    <link charset="utf-8" rel="stylesheet" href="/Styles/business.css">

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
    <link rel="stylesheet" type="text/css" href="/Scripts/Impromptu.css" />
    <script type="text/javascript" src="/Scripts/common.js"></script>
<link charset="utf-8" rel="stylesheet" href="/Styles/reg.css" />

<script type="text/javascript">
    $(function () {
        //取消修改密码
        $("#t1").click(function () {
            $("#con4").hide();
            $("#con2").hide();
            $("#con3").show();
            $("#t1").removeClass("navcolor1 navcolor2");
            $("#t1").addClass("navcolor1");
            $("#t2").removeClass("navcolor1 navcolor2");
            $("#t2").addClass("navcolor2");
            $("#t3").removeClass("navcolor1 navcolor2");
            $("#t3").addClass("navcolor2");

        })
        $("#t2").click(function () {
            $("#con4").hide();
            $("#con3").hide();
            $("#con2").show();
            $("#t1").removeClass("navcolor1 navcolor2");
            $("#t1").addClass("navcolor2");
            $("#t2").removeClass("navcolor1 navcolor2");
            $("#t2").addClass("navcolor1");
            $("#t3").removeClass("navcolor1 navcolor2");
            $("#t3").addClass("navcolor2");
        })

        $("#t3").click(function () {
            $("#con4").show();
            $("#con3").hide();
            $("#con2").hide();
            $("#t1").removeClass("navcolor1 navcolor2");
            $("#t1").addClass("navcolor2");
            $("#t2").removeClass("navcolor1 navcolor2");
            $("#t2").addClass("navcolor2");
            $("#t3").removeClass("navcolor1 navcolor2");
            $("#t3").addClass("navcolor1");
        })

        $("#t1").mouseover(function () {
            $(this).attr("style", "BACKGROUND-COLOR: #4dcbf4");
        });
        $("#t1").mouseout(function () {
            $(this).attr("style", "BACKGROUND-COLOR: ");
        });
        $("#t2").mouseover(function () {
            $(this).attr("style", "BACKGROUND-COLOR: #4dcbf4");
        });
        $("#t2").mouseout(function () {
            $(this).attr("style", "BACKGROUND-COLOR: ");
        });

        $("#t3").mouseover(function () {
            $(this).attr("style", "BACKGROUND-COLOR: #4dcbf4");
        });
        $("#t3").mouseout(function () {
            $(this).attr("style", "BACKGROUND-COLOR: ");
        });

        $("#headkaihu").click(function () {
            $("html,body").animate({ scrollTop: $("#t2").offset().top }, 500)
            $("#con3").hide();
            $("#con2").show();
            $("#con4").hide();
            $("#t1").removeClass("navcolor1 navcolor2");
            $("#t1").addClass("navcolor2");
            $("#t2").removeClass("navcolor1 navcolor2");
            $("#t2").addClass("navcolor1");
            $("#t3").removeClass("navcolor1 navcolor2");
            $("#t3").addClass("navcolor2");
        });

        $("#agent").click(function () {
            $("html,body").animate({ scrollTop: $("#t3").offset().top }, 500)
            $("#con3").hide();
            $("#con2").hide();
            $("#con4").show();
            $("#t1").removeClass("navcolor1 navcolor2");
            $("#t1").addClass("navcolor2");
            $("#t2").removeClass("navcolor1 navcolor2");
            $("#t2").addClass("navcolor2");
            $("#t3").removeClass("navcolor1 navcolor2");
            $("#t3").addClass("navcolor1");
        });


        //确认发送
        $("#SubSend").click(function () {
            var Mobile = $("#Mobile").val();
            var Name = $("#Name").val();
            var Company = $("#Company").val();
            var Address = $("#Address").val();

            if (Name == '' || Name == '姓名') {
                $.prompt('请填写联系人姓名！');
                return;
            }
            if (Mobile == '' || Mobile == '手机') {
                $.prompt('请填写联系电话');
                return;
            }


            if (Company == '' || Company == '公司名称') {
                $.prompt('请填写公司名称！');
                return;
            }

            $.ajax({
                type: "Get",
                data: { Mobile: Mobile, Name: Name, Company: Company, Address: Address },
                url: "/JsonFactory/BusinessCustomersHandler.ashx?oper=contentsend",
                success: function (data) {
                    $.prompt("提交成功", {
                        buttons: [
                                 { title: '确定', value: true }
                                ],
                        opacity: 0.1,
                        focus: 0,
                        show: 'slideDown',
                        submit: function (e, v, m, f) {
                            if (v == true) {
                                $("#Mobile").val("");
                                $("#Name").val("");
                                $("#Company").val("");
                                $("#Address").val("");

                                window.location.reload();
                            }
                        }

                    });
                }
            });

        })


        $("#showGG").click(function () {
            $("#regi").show();
            $("#zhegaiceng").show();

        });

        $("#glume2").click(function () {
            $("#regi").show();
            $("#zhegaiceng").show();

        });

        $("#Button1").click(function () {
            $("#regi").hide();
            $("#zhegaiceng").hide();

        });

        $("#closec").click(function () {
            $("#regic").hide();

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
                $.prompt("登录账户不可为空");
                return;
            }

            if (passwords == "") {
                $.prompt("密码不可为空！");
                return;
            }
            if (qpasswords == "") {
                $.prompt("确认密码不可为空！");
                return;
            }
            if (passwords != qpasswords) {
                $.prompt("密码和确认密码不符！");
                return;
            }
            if (com_name == "") {

                $.prompt("单位名称不可为空！");
                return;
            }

            if (province == "" || province == "省份") {
                $.prompt("请选择所在省份");
                return;
            }
            if (city == "" || city == "城市") {
                $.prompt("请选择所属城市");
                return;
            }
            if (com_type == "") {
                $.prompt("请选择所属行业");
                return;
            }

            if (Contact == "") {
                $.prompt("联系人姓名不可为空！");
                return;
            }

            if (tel == "") {
                $.prompt("联系电话不可为空！");
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
                        $.prompt("账户注册提示:" + data.msg, { buttons: [{ title: "确定", value: true}], submit: function (e, v, m, f) { if (v == true) { } } });
                        return;
                    }
                    if (data.type == 100) {
                        $("#regi").hide();
                        $("#zhegaiceng").hide();
                        $("#regic").show();

//                        $.prompt("账户信息注册成功，请等待管理员处理", {
//                            buttons: [
//                                 { title: '确定', value: true }
//                                ],
//                            opacity: 0.1,
//                            focus: 0,
//                            show: 'slideDown',
//                            submit: function (e, v, m, f) {
//                                if (v == true)
//                                    $("#regi").hide();
//                                $("#zhegaiceng").hide();
//                                $("#regic").show();
//                            }
//                        });
                    }
                }
            });
        });

    })

</script>
</head>
   



<body role="document" >    
   <align="center">
<!--[if IE 6]>

  <style>
      .kie-bar {
          height: 24px;
          line-height: 1.8;
          font-weight:normal;
          text-align: center;
          border-bottom:1px solid #fce4b5;
          background-color:#FFFF9B;
          color:#e27839;
          position: relative;
          font-size: 14px;
          text-shadow: 0px 0px 1px #efefef;
          padding: 5px 0;
      }
      .kie-bar a {
          color:#08c;
          text-decoration: none;
      }
  </style>
  <![endif]-->
  
  

	
     <div class="header" coor="header">
<div class="grid-990 header-wrap fn-clear">
            <div id="et-img-logo" class="fn-left">
               <%=Logourl %>
			</div>
      
  <div id="nav"> 
  </div> 
</div>
      <div id="banner" class="slide-1" coor-rate="0.1" coor="default-banner" role="banner" data-banner="false">
        <div id="glume" >
          <ul class="Limg">
            <div id="banner2" class="slide-1" coor-rate="0.1" coor="default-banner" role="banner" data-banner="false">
              <div class="login-box">
                <iframe class="fn-hide" allowtransparency="true" src="/account/Login.aspx" width="286" height="367" id="loginIframe" frameborder="0" scrolling="no" coor="aside-login" style="display: block;"></iframe>
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

    </cite> 
              </div>
              <script language=javascript type="text/javascript">
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
	
    <div class="footer1" coor="footer">
         <div class="sitelink">
           <%=Copyright %>
          
         </div>
    </div>
	


	

</align>



<div id="regi" class="modal hide  in"  style="  position: absolute;height: 650px; width: 782px; left: 45%; border-top-left-radius: 12px; border-top-right-radius: 12px; border-bottom-right-radius: 12px; border-bottom-left-radius: 12px; z-index: 901; display: none; " >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true" id="Button1" style=" font-size:18px;">
                    ×</button>
            </div>
            <div id="contentinfo" style="padding: 0; line-height: 25px; width: 650px; height: 355px; overflow-y: auto; margin: 10px auto 0 auto;">
            <div id="showli" style="height: 430px; width: 350px; left: 0; top: 30px; z-index: 99999; position: absolute; display: ">



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

<div id="regic" class="modal hide  in"  style="  position: absolute;height: 260px; width: 782px; left: 45%; border-top-left-radius: 12px; border-top-right-radius: 12px; border-bottom-right-radius: 12px; border-bottom-left-radius: 12px; z-index: 1999; display:none; " >
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true" id="closec" style=" font-size:18px;">
                    ×</button>

            </div>

                <div class="grid-780 fn-clear" style=" height:240px; padding-top:80px;">

                    <h3 class="ui-form-title">
                        <span style="font-size: 18px;">你已成功提交注册，请等待开通确认通知。</span>
                        <br/><br/><br/>
                        <span style="font-size: 18px;">如希望加快开通速度，请电话联系 010-87550229 </span>
                    </h3>
                    <div class="ui-form-dashed">
                    </div>
                </div>

</div>

</align>
    <script type="text/javascript">
        var province = document.getElementById('province');
        var city = document.getElementById('city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</body>

</html>
