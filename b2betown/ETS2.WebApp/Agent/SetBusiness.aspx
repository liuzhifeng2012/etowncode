<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"
    CodeBehind="SetBusiness.aspx.cs" Inherits="ETS2.WebApp.Agent.SetBusiness" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () { 

            //图形验证码
            $("#validateCodetext").click(function () {
                $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })
            $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })


            var agentid = $("#hid_agentid").trimVal();
            var proid = $("#hid_proid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();

             
            $("#erweima").html("扫二维码访问<br><img src='/UI/PMUI/ETicket/showtcode.aspx?pno=http://shop" + comid + ".etown.cn/h5/order/'  '/>");

            /*获取短信验证码*/
            $("#send_sms_btn")
                .click(function (event) {
                    stopDefault(event);
                    sendSms();
                });

            $("#getPsw")
                .click(function (event) {
                    stopDefault(event);
                    checkForm();
                });


            $("#agentlogin").click(function () {
                $.ajax({
                    cache: false,
                    type: "POST",
                    timeout: 10000,
                    url: "/JsonFactory/RegisterUser.ashx?oper=agnetlogincom&ts=" + Math.random() + "&agentid=" + agentid,
                    dataType: "json",
                    success: function (data) {
                        if (parseInt(data.type) == 1) {
                            alert(data.msg);
                            return;
                        }
                        if (parseInt(data.type) == 100) {

                            if (navigator.userAgent.indexOf("Safari") > -1 && navigator.userAgent.indexOf("Chrome") < 1) {
                                //parent.location = '/Manage.aspx'
                                window.open('/Manage.aspx');
                                return true;

                            } else {
                                //parent.location.href = "/Manage.aspx";
                                window.open('/Manage.aspx');
                            }
                        }
                    },
                    error: function (request, status, error) {
                        alert('error');
                    },
                    complete: function () {
                        //alert('成功');
                    }
                })
            })


        })



        function checkForm() {
            if ($.trim($("input:text[name=new_tel]").val()) == "") {
                alert("请输入手机号码!");
                return;
            }
            if ($.trim($("input:text[name=sms_code]").val()) == "") {
                alert("请输入短信验证码!");
                return;
            }
            //判断验证码输入是否正确
            $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms_regi", { mobile: $("input:text[name=new_tel]").val(), smscode: $("input:text[name=sms_code]").val(), agentid: $("#hid_agentid").trimVal(), phonestate: $("#hid_phonestate").trimVal(), source: "通用验证码" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    //$("#get-form").submit();

                    alert("注册成功");
                    return;
                }
            })

        }

        function checkMobile(tel) {
            tel = $.trim(tel);
            if (tel.length != 11 || tel.substr(0, 1) != 1) {
                return false;
            }
            return true;
        }

        function sendSms() {
            if ($.trim($("input:text[name=new_tel]").val()) == "") {
                alert("请输入手机号码!");
                return;
            }
            if (!checkMobile($("input:text[name=new_tel]").val())) {
                alert("请正确输入手机号!");
                return;
            }

            if ($.trim($("#send_sms_btn").text()) == "获取短信验证码") {
                $("#send_sms_btn").attr("disabled", "disabled").css("background-color", "#f4f4f4");
                _callSmsApi();

            }
        }

        function _sendSmsCD() {
            var sec = parseInt($("#send_sms_btn").text());
            if (sec > 1) {
                $("#send_sms_btn").text((sec - 1) + "秒后可再次发送短信");
                window.setTimeout(_sendSmsCD, 1000);
            } else {
                $("#send_sms_btn").text("获取短信验证码");
                $("#send_sms_btn").removeAttr("disabled").css("background-color", "#FFFFFF");
            }
        }

        function _callSmsApi() {
            var imgcode = $("#getcode").trimVal();
            if (imgcode == "") {
                alert("请输入图形验证码!");
                $("#send_sms_btn").removeAttr("disabled").css("background-color", "#FFFFFF");
                return;
            }

            $.post("/JsonFactory/AgentHandler.ashx?oper=callvalidsms", { imgcode: imgcode, mobile: $("input:text[name=new_tel]").val(), comid: 0, source: "通用验证码" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    $("#send_sms_btn").removeAttr("disabled").css("background-color", "#FFFFFF");
                }
                if (data.type == 100) {
                    $("#send_sms_btn").text("30秒后可再次发送短信");
                    window.setTimeout(_sendSmsCD, 1000);
                }
            })

        }
        /*取消事件的默认动作*/
        function stopDefault(e) {

            if (e && e.preventDefault) {//如果是FF下执行这个

                e.preventDefault();

            } else {

                window.event.returnValue = false; //如果是IE下执行这个

            }

            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul class="composetab">
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_sel">
                        <div>
                            <a href="javascript:;">我的微店</a></div>
                    </div>
                </li>
            </ul>
            <div class="toolbg toolbgline toolheight nowrap" style="">
                <div class="right searchtool">
                    <span>&nbsp;</span>
                </div>
                <div class="nowrap left" unselectable="on" onselectstart="return false;">
                    <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
                </div>
            </div>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                </h3>
                <div style="float: right; position: fixed; top: 155px; right: 30px;">
                    <div style="" id="erweima">
                    
                    </div>
                </div>
                <%if (loginstate == 1)
                  { %>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <div class="mi-form-explain">
                    </div>
                    <div style="float: left; margin: 0px 10px 25px 25px;" id="Div3" class="ui-button  ui-button-morange ">
                        <input id="agentlogin" class="ui-button-text" value="快速登录我的微店" type="button" />
                    </div>
                    <p>
                        <br />
                        如果禁止了新弹出页面，请点击允许.</p>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <%}
                  else
                  { %>
                <div id="step1" class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        快速开通微店</h2>
                    <%if (phonestate == 0)
                      { %>
                    <div class="mi-form-item">
                        验证手机立即开通微店：
                        <br />
                    </div>
                    <%}
                      else if (phonestate == -1)
                      { %>
                    <div class="mi-form-item">
                        检测到您的手机 已经开通过微店，但此微店已经绑定分销账户：
                        <br />
                        商户名称：<%=comname %>
                        <br />
                        您可以进入 <a href="AgentCompany.aspx">分销商信息</a> 更换此账户的的绑定手机，用新重新注册一个微店。
                        <br />
                    </div>
                    <%}
                      else
                      { %>
                    <div class="mi-form-item">
                        检测到您的手机 已经开通过微店，验证手机立即绑定此商户：
                        <br />
                        商户名称：<%=comname %>
                        <br />
                    </div>
                    <%} %>
                    <%if (phonestate != -1)
                      { %>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            账户绑定手机</label>
                        <input id="new_tel" name="new_tel" value="<%=phone %>" class="mi-input" type="text"
                            disabled />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            图形验证码</label>
                        <input name="getcode" type="text" placeholder="图形验证码" id="getcode" size="10" class="i-text i-text-authcode ui-input sl-checkcode-input" />
                        <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码" />
                        <a href="javascript:;" id="validateCodetext">更换</a>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            验证码</label>
                        <input type="text" id="sms_code" name="sms_code" placeholder="短信验证码" class="mi-input" />
                        <button id="send_sms_btn" type="button" style="color: #333333; background-color: #dddddd;
                            border-color: #ccc; border-radius: 3px; box-shadow: 0px 0px 3px rgba(0, 0, 0, 0.2);
                            height: 30px; line-height: 30px; cursor: pointer;">
                            获取短信验证码</button>
                    </div>
                    <div style="float: left; margin: 10px 10px 10px 25px;" id="Div1" class="ui-button  ui-button-morange ">
                        <input id="getPsw" class="ui-button-text" value="下一步立即开通" type="button" />
                    </div>
                    <%} %>
                    <br />
                    <br />
                    <br />
                    <div class="mi-form-item">
                        注1：如果您 已经有微店账户，请把 分销账户和微店账户手机更改一致，然后再进行绑定
                        <br />
                        注2：新注册的微店账户，是按您分销账户信息进行注册的。请成功后登陆微店完善相关信息
                        <br />
                        注3：分销账户绑定已有的微店账户后，并不更改原微店账户信息，原微店账户密码不变
                        <br />
                        注4：注册微店时：如果出现 账户已经被注册,请查询
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <%} %>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_phonestate" type="hidden" value="<%=phonestate %>" />
</asp:Content>
