<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Agent.Master"
    CodeBehind="Regi.aspx.cs" Inherits="ETS2.WebApp.Agent.Regi" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
           //图形验证码
            $("#validateCodetext").click(function () {
                $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })
             $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })



            var comid = $("#hid_comid").trimVal();

            //账号名
            $("#Email").blur(function () {
                $("#EmailVer").html(""); //离开后先清空备注
                var Email = $("#Email").val();
                //判断邮箱不为空
                if (Email != "") {
                    $.post("/JsonFactory/AgentHandler.ashx?oper=getEmail", { Email: Email, comid: 106 }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#EmailVer").html("√");
                                $("#EmailVer").css("color", "green");
                                $("#VEmail").val(1);
                            } else {
                                $("#EmailVer").html(data.msg);
                                $("#EmailVer").css("color", "red");
                                $("#VEmail").val(0);
                                return;
                            }

                        }
                    })
                }
            })

//            //账号名
//            $("#Phone").blur(function () {
//                $("#PhoneVer").html(""); //离开后先清空备注
//                var Phone = $("#Phone").val();
//                //判断手机不为空
//                if (Phone != "") {
//                    $.post("/JsonFactory/AgentHandler.ashx?oper=getPhone", { Phone: Phone, comid: comid }, function (data) {
//                        data = eval("(" + data + ")");
//                        if (data.type == 100) {
//                            if (data.msg == "OK") {
//                                $("#PhoneVer").html("√");
//                                $("#PhoneVer").css("color", "green");
//                                $("#VPhone").val(1);
//                            } else {
//                                $("#PhoneVer").html(data.msg);
//                                $("#PhoneVer").css("color", "red");
//                                $("#VPhone").val(0);
//                                return;
//                            }

//                        }
//                    })
//                }
//            })

            //判断密码
            $("#QPassword1").blur(function () {
                $("#QPassword1Ver").html(""); //离开后先清空备注
                var QPassword1 = $("#QPassword1").val();
                var Password1 = $("#Password1").val();
                //Phone
                if (QPassword1 == "" || QPassword1 != Password1) {
                    $("#QPassword1Ver").html("再次填写密码错误");
                    $("#QPassword1Ver").css("color", "red");
                    return false;
                } else {
                    $("#VQPassword1").val(1);
                }
            })


            //提交按钮
            $("#btn-submit").click(function () {
                var Email = $("#Email").val();
                var Password1 = $("#Password1").val();
                var QPassword1 = $("#QPassword1").val();
                var Name = $("#Name").val();
                var Tel = "";
                var Phone = $("#Phone").val();
                var phonecode = $("#phonecode").val();

                var Company = $("#Email").val();
                var Address = "";
                var Sex = "";
                var agentsort = 0;
                var agentsourcesort = 0;

                if (Email == "") {
                    $("#EmailVer").html("请填账户");
                    $("#EmailVer").css("color", "red");
                    return;
                }


//                if ($("#VPhone").val() == 0) {
//                    $("#PhoneVer").html("手机有误");
//                    $("#PhoneVer").css("color", "red");
//                    return;
//                };
                if (Password1 == "") {
                    $("#Password1Ver").html("请填写密码");
                    $("#Password1Ver").css("color", "red");
                    return;
                }
                if (QPassword1 == "") {
                    $("#QPassword1Ver").html("再次填写密码错误");
                    $("#QPassword1Ver").css("color", "red");
                    return;
                }
                if ($("#VQPassword1").val() == 0) {
                    $("#QPassword1Ver").html("密码有误");
                    $("#QPassword1Ver").css("color", "red");
                    return;
                };
                //                if (agentsourcesort==0)
                //                {
                //                    $("#Span2").text("请选择分销类型");
                //                    $("#Span2").css("color", "red");
                //                    return;
                //                }
                if (Name == "") {
                    $("#NameVer").html("请填写姓名");
                    $("#NameVer").css("color", "red");
                    return;
                }

                if (Phone == "") {
                    $("#PhoneVer").html("请填写手机");
                    $("#PhoneVer").css("color", "red");
                    return;
                }
                
                if (phonecode == "") {
                    $("#Span3").html("请填写短信验证码");
                    $("#Span3").css("color", "red");
                    return;
                }
                 //判断验证码输入是否正确
            $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms", { mobile: $("#Phone").val(),smscode:$("#phonecode").val(),source:"分销注册验证码"},function(data11){
            data11=eval("("+data11+")");
            if(data11.type==1){
              alert("短信验证码不相符");
              return ;
            }
            if(data11.type==100){
           
           
//                if (Company == "") {
//                    $("#CompanyVer").html("请填写公司名称");
//                    $("#CompanyVer").css("color", "red");
//                    return;
//                }
//                if (Address == "") {
//                    $("#AddressVer").html("请填写地址");
//                    $("#AddressVer").css("color", "red");
//                    return;
//                }

                var com_province = "";
                var com_city = "";


//                if (com_province == "" || com_province == "省份") {
//                    $.prompt("请选择所在省份");
//                    return;
//                }
//                if (com_city == "" || com_city == "城市") {
//                    $.prompt("请选择所属城市");
//                    return;
//                }

                $("#loading").html("正在提交注册信息，请稍后...");

                //创建订单
                $.post("/JsonFactory/AgentHandler.ashx?oper=Agentregi", { agentsourcesort: agentsourcesort, comid: 106, Email: Email, Password1: Password1, Name: Name, Tel: Tel, Phone: Phone, Company: Company, Address: Address, agentsort: agentsort, com_province: "", com_city: "" }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("注册出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            alert(" 您已注册成功，请等待商家为您授权！");
                            location.href = "/Agent/Login.aspx?Email=" + Email
                            return;
                        }
                        else {
                            $.prompt("参数传递出错，请重新操作");
                            return;
                        }
                    }
                })


            }
            })
             

            })

            //获取手机验证码
            $("#getphonecode").click(function () { 
             var imgcode=$("#getcode").trimVal();
             if(imgcode=="")
             {
               alert("请输入图形验证码!");
               return;
             }
             //判断图形验证码是否正确
             $.post("/JsonFactory/RegisterUser.ashx?oper=verifyimgcode",{imgcode:imgcode},function(dd){
                dd=eval("("+dd+")");
                if(dd.type==1){
                    alert("图形验证码输入不正确");
                    $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                   return;
                }else{
                
                       if ($.trim($("#Phone").val()) == "") {
                            alert("请输入手机号码!");
                            return;
                        }
                        if (!checkMobile($("#Phone").val())) {
                            alert("请正确输入手机号!");
                            return;
                        }
          
                        if ($.trim($("#getphonecode").html()) == "获取短信验证码") {
                            $("#getphonecode").attr("disabled", "disabled").css("background-color", "#f4f4f4");
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
            var sec = parseInt($("#getphonecode").html());
            if (sec > 1) {
                $("#getphonecode").html((sec - 1) + "秒后可再次发送短信");
                window.setTimeout(_sendSmsCD, 1000);
            } else {
                $("#getphonecode").html("获取短信验证码");
                $("#getphonecode").removeAttr("disabled").css("background-color","#FFFFFF");
            }
        }

        function _callSmsApi() {
             var imgcode=$("#getcode").trimVal();
             if(imgcode=="")
             {
               alert("请输入图形验证码!");
               return;
             }
           
            $.post("/JsonFactory/AgentHandler.ashx?oper=callvalidsms", {imgcode:imgcode, mobile: $("#Phone").val(),comid:106,source:"分销注册验证码" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                }
                if (data.type == 100) { 
                $("#getphonecode").html("30秒后可再次发送短信");
                window.setTimeout(_sendSmsCD, 1000);
                }
            })

        }
    </script>
    <style type="text/css">
        .ui-name
        {
            border: 1px solid #C1C1C1;
            color: #343434;
            font-size: 14px;
            height: 25px;
            line-height: 25px;
            padding: 2px;
            vertical-align: middle;
            width: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div class="grid-780 grid-780-border fn-clear">
        <form class="ui-form ui-form-bg" name="regCompleteForm" method="post" action="#"
        id="J-complete-form" novalidate="novalidate" data-widget-cid="widget-0">
        <p class="ui-tiptext">
            注册分销账户！
        </p>
        <div class="ui-form-dashed">
        </div>
        <h3 class="ui-form-title">
            <strong>填写登录信息</strong><span class="explain">请填写您的 常用电子邮箱 做为登录账户名</span></h3>
        <div class="ui-form-group">
            <div class="ui-form-item">
                <label for="payPwd" class="ui-label">
                    账户名</label>
                <input autocomplete="off" class="ui-input" type="text" id="Email" name="Email" data-error="    "
                    seed="" smartracker="on" data-widget-cid="widget-2" data-validator-set="widget-3" />
                <span id="EmailVer"></span>
            </div>
            <div class="ui-form-item">
                <label for="payPwdConfirm" class="ui-label">
                    登录密码</label>
                <input autocomplete="off" class="ui-input" type="password" id="Password1" name="Password1"
                    data-error="    " data-explain="请再次输入登录密码。" seed="JCompleteForm-payPwdConfirm"
                    smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-4" />
                <span id="Password1Ver"></span>
            </div>
            <div class="ui-form-item">
                <label for="payPwdConfirm" class="ui-label">
                    再输入一次</label>
                <input autocomplete="off" class="ui-input" type="password" id="QPassword1" name="QPassword1"
                    data-error="    " data-explain="请再次输入登录密码。" seed="JCompleteForm-payPwdConfirm"
                    smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-4" />
                <span id="QPassword1Ver"></span>
            </div>
        </div>
        <div class="ui-form-dashed">
        </div>
        <h3 class="ui-form-title">
            <strong>分销信息</strong><span class="explain">请填写会员信息，已帮助我们为您提供会员专享服务</span></h3>
        <div class="ui-form-group">
            <div class="ui-form-item">
                <label for="payPwd" class="ui-label">
                    联系人姓名</label>
                <input autocomplete="off" class="ui-name" type="text" id="Name" name="Name" data-error="    "
                    data-explain="" seed="" smartracker="on" data-widget-cid="widget-2" data-validator-set="widget-3" />
                <span id="NameVer"></span>
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
                <label for="payPwdConfirm" class="ui-label">
                    联系人手机</label>
                <input autocomplete="off" class="ui-input" type="text" id="Phone" name="Phone" data-error="    "
                    data-explain="。" seed="" smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-4" />
                <a id="getphonecode" style="text-decoration: underline; cursor: pointer;">获取短信验证码</a>
                <span id="PhoneVer"></span>
            </div>
            <div class="ui-form-item">
                <label for="payPwdConfirm" class="ui-label">
                    短信验证码</label>
                <input autocomplete="off" class="ui-input" type="text" id="phonecode" data-error="    "
                    data-explain="。" seed="" smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-4" />
                <span id="Span3"></span>
            </div>
        </div>
        <div class="ui-form-dashed">
        </div>
        <div class="ui-form-item">
            <div id="submitBtn" class="ui-button  ">
                <input autocomplete="off" class="ui-input" type="hidden" id="Tel" name="Tel" data-error="    "
                    data-explain="。" seed="" smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-4" />
                <input name="按钮" type="button" class="btn-login" id="btn-submit" tabindex="4" value="提交注册信息"
                    seed="B-login-button1" />
            </div>
            <span class="ui-form-confirm"><span id="loading"></span></span>
        </div>
        </form>
    </div>
    <input type="hidden" name="VEmail" id="VEmail" value="0" />
    <input type="hidden" name="VPhone" id="VPhone" value="0" />
    <input type="hidden" name="VQPassword1" id="VQPassword1" value="0" />
</asp:Content>
