<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChanelrebateApplyaccount.aspx.cs"
    Inherits="ETS2.WebApp.UI.VASUI.ChanelrebateAccount" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $.post("/JsonFactory/PermissionHandler.ashx?oper=getchanelrebateApplyaccount", { channelid: '<%=channelid %>' }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    $("#truename").val(data.msg.truename);
                    $("#newaccount").val(data.msg.alipayaccount);
                    $("#newphone").val(data.msg.alipayphone);
                    $("#hid_channelid").val(data.msg.channelid);
                    $("#hid_oldphone").val(data.msg.alipayphone);
                }
            })


            //图形验证码
            $("#validateCodetext").click(function () {
                $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })

            $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })

            //获取手机验证码
            $("#getphonecode").click(function () {
                var phone = $.trim($("#newphone").val());
                var oldphone = $("#hid_oldphone").trimVal();
                if (phone != "") {
                    if (phone == oldphone) {
                        alert("手机号码没有变化");
                        return;
                    }
                }

                if (phone == "") {
                    alert("请输入手机号码!");
                    return;
                }
                if (!checkMobile(phone)) {
                    alert("请正确输入手机号!");
                    return;
                }

                if ($.trim($("#getphonecode").html()) == "获取短信验证码") {
                    $("#getphonecode").attr("disabled", "disabled").css("background-color", "#f4f4f4");
                    _callSmsApi();

                }
            })

            $("#upaccount").click(function () {
                var channelid = $("#hid_channelid").trimVal();
                var truename = $("#truename").trimVal();
                if (phonecode == "") {
                    alert("请填写真实姓名");
                    return;
                }
                var account = $("#newaccount").trimVal();
                if (phonecode == "") {
                    alert("请填写支付宝账户");
                    return;
                }
                var newphone = $("#newphone").trimVal();
                if (newphone == "") {
                    alert("手机号不可为空");
                    return;
                }
                if (!checkMobile(newphone)) {
                    alert("手机号格式不正确");
                    return;
                }
                var phonecode = $("#phonecode").val();
                if (phonecode == "") {
                    alert("请填写短信验证码");
                    return;
                }

                //判断验证码输入是否正确
                $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms", { mobile:
$("#newphone").val(), smscode: $("#phonecode").val(), source: "通用验证码"
                }, function (data1) {
                    data1 = eval("(" + data1 + ")");
                    if (data1.type == 1) {
                        alert("短信验证码不相符");
                        return;
                    }
                    if (data1.type == 100) {

                        $.post("/JsonFactory/PermissionHandler.ashx?oper=Upchannelrebateaccount", { channelid: channelid, truename: truename, account: account, newphone: newphone, comid: $("#hid_comid").trimVal() }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                alert(data.msg);
                                return;
                            }
                            if (data.type == 100) {
                                alert("编辑成功");
                                return;
                            }
                        })

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
                $("#getphonecode").removeAttr("disabled").css("background-color", "#FFFFFF");
            }
        }

        function _callSmsApi() {
            var imgcode = $("#getcode").trimVal();
            if (imgcode == "") {
                alert("请输入图形验证码!");
                $("#getphonecode").removeAttr("disabled").css("background-color", "#FFFFFF");
                return;
            }

            var phone = $.trim($("#newphone").val());

            $.post("/JsonFactory/AgentHandler.ashx?oper=callvalidsms", { imgcode: imgcode, mobile: phone, comid: 0, source: "通用验证码" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    $("#getphonecode").removeAttr("disabled").css("background-color", "#FFFFFF");
                    return;
                }
                if (data.type == 100) {
                    $("#getphonecode").html("30秒后可再次发送短信");
                    window.setTimeout(_sendSmsCD, 1000);
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
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="ChanelrebateApplyaccount.aspx" onfocus="this.blur()">提现账户管理</a></li>
                <li><a href="ChanelrebateApplylist.aspx" target="" title="">提现申请列表</a></li>
                <li><a href="ChanelrebateApply.aspx" target="" title="">提现申请</a></li>
                <li><a href="Chanelrebatelist.aspx" target="" title="">返佣记录</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    提现账号</h3>
                <table class="grid">
                    <tr>
                        <td class="tdHead">
                            我的真实姓名:
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="truename" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            我的支付宝账号 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="newaccount" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            图形验证码 :
                        </td>
                        <td>
                            <input name="getcode" type="text" placeholder="图形验证码" id="getcode" size="10" />
                            <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码" />
                            <a href="javascript:;" id="validateCodetext">更换</a>
                        </td>
                    </tr>
                    <tr>
                        <td height="20" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                            我的支付宝手机号 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="newphone" value="" />
                            <a id="getphonecode" style="text-decoration: underline; cursor: pointer;">获取短信验证码</a>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            短信验证码:
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="phonecode" value="" />
                        </td>
                    </tr>
                </table>
                <table width="300px" class="grid">
                    <tr>
                        <td class="tdHead" colspan="2">
                            <input id="upaccount" type="button" value="    修改   " name="upaccount"> </input>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <!--记录原始手机-->
    <input type="hidden" value="" id="hid_oldphone" />
    <input type="hidden" value="<%=channelid %>" id="hid_channelid" />
</asp:Content>
