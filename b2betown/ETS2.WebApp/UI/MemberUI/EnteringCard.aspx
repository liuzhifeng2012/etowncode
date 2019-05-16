<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="EnteringCard.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.EnteringCard" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var issueid = $("#hid_issueid").val();
            if (issueid == "0") {
                $.prompt("发行参数不存在", { buttons: [{ title: "确定", value: true}],
                    opacity: 0.1,
                    focus: 0,
                    show: "slideDown",
                    submit: function (e, v, m, f) {
                        if (v == true) {
                            window.open("PublishList.aspx", target = "_self");
                        }
                    }
                });
            }
            else {
                $.post("/JsonFactory/CardHandler.ashx?oper=getCardByIssueId", { issueid: issueid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt(data.msg, {
                            buttons: [{ title: "确定", value: true}],
                            show: "slideDown",
                            submit: function (e, v, m, f) {
                                if (v == true) {
                                    location.reload();
                                }
                            }
                        });
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg.CardRule == 1)//后8位随机号码
                        {
                            $("#singeltb").show();
                            $("#sorttb").hide();

                            $("#numberbegin").val(data.msg.SingleCardNumber);
                        }
                        if (data.msg.CardRule == 2)//后8位顺序号码
                        {
                            $("#singeltb").show();
                            $("#sorttb").show();

                            $("#numberbegin").val(data.msg.CardRule_First);
                            $("#numberend").val(data.msg.CardRule_First);
                        }
                    }
                })
            }


            //单卡号录入
            $("#singleenter").click(function () {
                var cardnumber1 = $("#SingleCardNumber").trimVal();
                $.post("/JsonFactory/IssueHandler.ashx?oper=entercardnumber", { issueid: issueid, cardnumber: cardnumber1, comid: $("#hid_comid").val() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#SingleCardNumbererr").html(data.msg);
                        $("#SingleCardNumbererr").css("color", "red");
                        $("#SingleCardNumber").val("");
                        document.getElementById("SingleCardNumber").focus();
                        return;
                    }
                    if (data.type == 100) {
                        $("#SingleCardNumbererr").html("√");
                        $("#SingleCardNumbererr").css("color", "green");
                        $("#SingleCardNumber").val("");
                        document.getElementById("SingleCardNumber").focus();
                        return;
                    }
                })
            })
            //批量录入
            $("#batchenter").click(function () {
                var numberbegin = $("#numberbegin").trimVal();
                var numberend = $("#numberend").trimVal();
                if (numberbegin == "") {
                    $.prompt("起始卡号不能为空");
                    return;
                }

                if (numberend == "") {
                    $.prompt("结束卡号不能为空");
                    return;
                }
                if (numberbegin >= numberend) {
                    $.prompt("结束卡号必须大于开始卡号");
                    return;
                }

                $.post("/JsonFactory/IssueHandler.ashx?oper=batchentercardnumber", { issueid: issueid, numberbegin: numberbegin, numberend: numberend, comid: $("#hid_comid").val(), ignoreentered: false }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt(data.msg, {
                            buttons: [{ title: "确定", value: true}],
                            show: "slideDown",
                            submit: function (e, v, m, f) {
                                if (v == true) {

                                    $("#numberbegin").val("");
                                    $("#numberend").val("");
                                    //location.reload();
                                    document.getElementById("numberbegin").focus();
                                }
                            }
                        });
                        return;
                    }
                    if (data.type == 10) {//区间范围内含有已经录入的
                        if (confirm("区间范围内含有已经录入的，是否继续录入剩余的卡？")) {
                            $.post("/JsonFactory/IssueHandler.ashx?oper=batchentercardnumber", { issueid: issueid, numberbegin: numberbegin, numberend: numberend, comid: $("#hid_comid").val(), ignoreentered: true }, function (datad) {
                                datad = eval("(" + datad + ")");
                                if (datad.type == 1) {
                                    $.prompt(datad.msg, {
                                        buttons: [{ title: "确定", value: true}],
                                        show: "slideDown",
                                        submit: function (e, v, m, f) {
                                            if (v == true) {

                                                $("#numberbegin").val("");
                                                $("#numberend").val("");
                                                //location.reload();
                                                document.getElementById("numberbegin").focus();
                                            }
                                        }
                                    });
                                    return;
                                }
                                if (datad.type == 100) {
                                    $.prompt("卡号批量录入成功", {
                                        buttons: [{ title: "确定", value: true}],
                                        show: "slideDown",
                                        submit: function (e, v, m, f) {
                                            if (v == true) {

                                                $("#numberbegin").val("");
                                                $("#numberend").val("");
                                                //location.reload();
                                                document.getElementById("numberbegin").focus();
                                            }
                                        }
                                    });
                                    return;
                                }


                            })
                        } else {
                            alert("取消成功");
                            $("#numberbegin").val("");
                            $("#numberend").val("");
                            //location.reload();
                            document.getElementById("numberbegin").focus();
                        }
                    }
                    if (data.type == 100) {
                        $.prompt("卡号批量录入成功", {
                            buttons: [{ title: "确定", value: true}],
                            show: "slideDown",
                            submit: function (e, v, m, f) {
                                if (v == true) {

                                    $("#numberbegin").val("");
                                    $("#numberend").val("");
                                    //location.reload();
                                    document.getElementById("numberbegin").focus();
                                }
                            }
                        });
                        return;
                    }
                })
            })
        })
    </script>
    <style type="text/css">
        .style1
        {
            width: 91px;
        }
        .style2
        {
            width: 88px;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div class="grid-780 grid-780-border fn-clear">
        <div id="settings" class="view main">
            <div id="secondary-tabs" class="navsetting ">
                <ul>
                    <li><span>录入卡片</span></li>
                </ul>
            </div>
            <div id="setting-home" class="vis-zone">
                <div class="inner">
                    <form id="form1" action="">
                    <table class="grid" id="singeltb">
                        <tr>
                            <td class="style1">
                                <span class="STYLE3">卡号录入</span>
                            </td>
                            <td class="tdHead">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td height="60" class="style1">
                                随机会员卡<br>
                                单卡片录入
                            </td>
                            <td class="tdHead">
                                <input type="text" id="SingleCardNumber" name="SingleCardNumber" value="" />
                                <input type="button" id="singleenter" value="单个录入" />
                                <span id="SingleCardNumbererr"></span>
                            </td>
                        </tr>
                    </table>
                    <table class="grid" id="sorttb">
                        <tr>
                            <td height="60" class="style2">
                                顺序会员卡<br>
                                批量录入
                            </td>
                            <td class="tdHead">
                                自
                                <input type="text" id="numberbegin" name="numberbegin" value="" />
                                <span id="numberbeginerr"></span>
                                <br>
                                到
                                <input type="text" id="numberend" name="numberend" value="" />
                                <input type="button" id="batchenter" value="批量录入" />
                            </td>
                        </tr>
                    </table>
                    <input type="hidden" id="hid_issueid" value="<%=issueid %>" />
                    </form>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
