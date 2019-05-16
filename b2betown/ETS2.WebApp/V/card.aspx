<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/V/Member.Master" CodeBehind="card.aspx.cs"
    Inherits="ETS2.WebApp.V.card" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            //判断卡号
            $("#Cardcode").blur(function () {
                $("#CardcodeVer").html(""); //离开后先清空备注
                var Cardcode = $("#Cardcode").val();
                //判断卡号不为空
                if (Cardcode != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getCard", { Card: Cardcode, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#CardcodeVer").html("√");
                                $("#CardcodeVer").css("color", "green");
                                $("#VCardcode").val(1);

                            } else {
                                $("#CardcodeVer").html(data.msg);
                                $("#CardcodeVer").css("color", "red");
                                $("#VCardcode").val(0);
                                return;
                            }

                        }
                    })
                } else {
                    $("#VCardcode").val(1);
                }
            })

            //判断邮箱
            $("#Email").blur(function () {
                $("#EmailVer").html(""); //离开后先清空备注
                var Email = $("#Email").val();
                //判断邮箱不为空
                if (Email != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getEmail", { Email: Email, comid: comid }, function (data) {
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
                                $("#PhoneVer").html("√");
                                $("#PhoneVer").css("color", "green");
                                $("#VPhone").val(1);
                            } else {
                                $("#PhoneVer").html(data.msg);
                                $("#PhoneVer").css("color", "red");
                                $("#VPhone").val(0);
                                return;
                            }

                        }
                    })
                }
            })

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




            //推荐人判断有效性。可以为空
            $("#ChannelCard").blur(function () {
                $("#ChannelCardVer").html(""); //离开后先清空备注
                var ChannelCard = $("#ChannelCard").val();
                //推荐人可以为空
                if (ChannelCard != "") {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getChannelCard", { Card: ChannelCard, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#ChannelCardVer").html("√");
                                $("#ChannelCardVer").css("color", "green");
                                $("#VChannelCard").val(1);
                            } else {
                                $("#ChannelCardVer").html(data.msg);
                                $("#ChannelCardVer").css("color", "red");
                                $("#VChannelCard").val(0);
                                return;
                            }
                        }
                    })
                } else {
                    $("#VChannelCard").val(1);
                }
            })


            //提交按钮
            $("#btn-login").click("click", function () {
                var Cardcode = $("#Cardcode").val();
                var Email = $("#Email").val();
                var Password1 = $("#Password1").val();
                var QPassword1 = $("#QPassword1").val();
                var Name = $("#Name").val();
                var Phone = $("#Phone").val();
                var ChannelCard = $("#ChannelCard").val();
                var Sex = $('input:radio[name="Sex"]:checked').val();

//                if (Cardcode == "") {
//                    $("#CardcodeVer").html("请填写卡号");
//                    $("#CardcodeVer").css("color", "red");
//                    return;
//                }
                if ($("#VCardcode").val() == 0) {
                    $("#CardcodeVer").html("卡号有误");
                    $("#CardcodeVer").css("color", "red");
                    return;
                };

                //                if (Email == "") {
                //                    $("#EmailVer").html("请填电子邮件");
                //                    $("#EmailVer").css("color", "red");
                //                    return;
                //                }
                //                if ($("#VEmail").val() == 0) {
                //                    $("#EmailVer").html("电子邮箱有误");
                //                    $("#EmailVer").css("color", "red");
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

                if (Name == "") {
                    $("#NameVer").html("请填写姓名");
                    $("#NameVer").css("color", "red");
                    return;
                }

                if (Sex == "" || Sex == null) {
                    $("#NameVer").html("请选择性别");
                    $("#NameVer").css("color", "red");
                }


                if (Phone == "") {
                    $("#PhoneVer").html("请填写手机");
                    $("#PhoneVer").css("color", "red");
                    return;
                }
                if ($("#VPhone").val() == 0) {
                    $("#PhoneVer").html("手机号码有误");
                    $("#PhoneVer").css("color", "red");
                    return;
                };
                if ($("#VChannelCard").val() == 0) {
                    $("#ChannelCardVer").html("推荐人无效，如果没有推荐人可不填写");
                    $("#ChannelCardVer").css("color", "red");
                    return;
                };


                $(".loading-text").html("正在提交开卡信息，请稍后...")

                //创建订单
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=regcard", { comid: comid, Cardcode: Cardcode, Email: Email, Password1: Password1, Name: Name, Phone: Phone, ChannelCard: ChannelCard, Sex: Sex }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.msg == 0) {
                        $.prompt("注册账户错误，请重新提交");
                        return;
                    } else if (data.type == 1) {
                        $.prompt(data.msg, {
                            buttons: [{ title: "确定", value: true}],
                            submit: function (e, v, m, f) {
                                if (v == true) {
                                    location.reload();
                                }
                            }
                        });
                    }
                    else {
                        location.href = "/V/CardSuccess.aspx?Name=" + Name + "&Cardcode=" + Cardcode + "&Email=" + Email + "&Phone=" + Phone
                        return;
                    }
                })

            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div class="grid-780 grid-780-border fn-clear">
        <form class="ui-form ui-form-bg" name="regCompleteForm" method="post" action="#"
        id="J-complete-form" novalidate="novalidate" data-widget-cid="widget-0">
        <p class="ui-tiptext">
            开卡啦！
        </p>
        <div class="ui-form-dashed">
        </div>
        <div class="ui-form-group">
            <div class="ui-form-item">
                <label for="payPwd" class="ui-label">
                    输入卡号</label>
                <input autocomplete="off" class="ui-input" type="text" id="Cardcode" name="Cardcode"
                    data-error="    " seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2"
                    data-validator-set="widget-3" placeholder="卡号"><span id="CardcodeVer"></span>
                &nbsp; 如您没有卡号请不要填写。如果您已经开卡，请直接 <a href="Login.aspx">登录</a>
            </div>
        </div>
        <div class="ui-form-dashed">
        </div>
        <div class="ui-form-group">
            <div class="ui-form-item">
                <label for="payPwdConfirm" class="ui-label">
                    电子邮箱</label>
                <input autocomplete="off" class="ui-input2" type="text" id="Email" name="Email" data-error="    "
                    data-explain="。" seed="JCompleteForm-payPwdConfirm" smartracker="on" data-widget-cid="widget-4"
                    data-validator-set="widget-4" placeholder="电子邮箱"><span id="EmailVer"></span>
            </div>
            <div class="ui-form-item">
                <label for="payPwd" class="ui-label">
                    设置登录密码</label>
                <input autocomplete="off" class="ui-input" type="password" id="Password1" name="Password1"
                    data-error="    " seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2"
                    data-validator-set="widget-3" placeholder="密码"><span id="Password1Ver"></span>
                &nbsp;通过自设密码可在线查询优惠情况
            </div>
            <div class="ui-form-item">
                <label for="payPwd" class="ui-label">
                    再次确认密码</label>
                <input autocomplete="off" class="ui-input" type="password" id="QPassword1" name="QPassword1"
                    data-error="    " seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2"
                    data-validator-set="widget-3" placeholder="再次确认密码"><span id="QPassword1Ver"></span>
                &nbsp;</div>
            <div class="ui-form-item">
                <label for="payPwd" class="ui-label">
                    您的姓名</label>
                <input autocomplete="off" class="ui-input" type="text" id="Name" name="Name" data-error="    "
                    data-explain="" seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2"
                    data-validator-set="widget-3" placeholder="姓名">  &nbsp;&nbsp;
                <label> <input type="radio" name="Sex" value="男" > 先生 </label>&nbsp;&nbsp;
                 <label> <input type="radio" name="Sex" value="女" > 女士 </label>
                <span id="NameVer"></span>
            </div>
            <div class="ui-form-item">
                <label for="payPwdConfirm" class="ui-label">
                    手机号</label>
                <input autocomplete="off" class="ui-input" type="text" id="Phone" name="Phone" data-error="    "
                    data-explain="。" seed="JCompleteForm-payPwdConfirm" smartracker="on" data-widget-cid="widget-4"
                    data-validator-set="widget-4" placeholder="手机"><span id="PhoneVer"></span>
            </div>
        </div>
        <div class="ui-form-dashed">
        </div>
        <div class="ui-form-group">
            <div class="ui-form-item">
                <label for="payPwd" class="ui-label">
                    推荐人</label>
                <input autocomplete="off" class="ui-input" type="text" id="ChannelCard" name="ChannelCard"
                    data-error="    " seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2"
                    data-validator-set="widget-3" placeholder="推荐人卡号"><span id="ChannelCardVer"></span>
                &nbsp; 如果您不知道推荐人，可不必填写
            </div>
        </div>
        <div class="ui-form-dashed">
        </div>
        <div class="ui-form-item">
            <div id="submitBtn" class="ui-button ">
                <input name="btn-login" type="button" class="btn-login" id="btn-login" tabindex="4"
                    value="提交开卡信息" seed="B-login-button1">
            </div>
            <span class="ui-form-confirm"><span class="loading-text fn-hide"></span></span>
        </div>
        </form>
    </div>
    <input type="hidden" name="VCardcode" id="VCardcode" value="1">
    <input type="hidden" name="VEmail" id="VEmail" value="0">
    <input type="hidden" name="VQPassword1" id="VQPassword1" value="0">
    <input type="hidden" name="VPhone" id="VPhone" value="0">
    <input type="hidden" name="VChannelCard" id="VChannelCard" value="1">

    

</asp:Content>
