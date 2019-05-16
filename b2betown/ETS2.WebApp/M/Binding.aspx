<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Binding.aspx.cs" Inherits="ETS2.WebApp.M.Binding" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>绑定实体卡</title>
    <meta charset="utf-8"></meta>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport"></meta>
    <meta content="telephone=no" name="format-detection"></meta>
    <link href="../Styles/weixin/StyleBinding.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        abbr, article, aside, audio, canvas, datalist, details, dialog, eventsource, figure, figcaption, footer, header, hgroup, mark, menu, meter, nav, output, progress, section, small, time, video, legend
        {
            display: block;
        }
    </style>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>

    <script src="/Scripts/common.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").val();
            var openid = $("#hid_openid").trimVal();

            if ($("#hid_f").val() == 6) {
                $("#Cardcode").val($("#hid_card").val());
                $("#Cardcode").attr("disabled", "disabled");
                $("#confirmBtn").css("display", "none");
                return;
            }
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
                                $("hid_ture").val(1);
                                $("#confirmBtn").remove("readOnly", false);
                                return;
                            }
                            else if (data.msg == "old") {
                                $("#CardcodeVer").html("此卡已使用");
                                $("#CardcodeVer").css("color", "red");
                                $("#Cardcode").val(" ");
                                $("hid_ture").val(0);
                                $("#confirmBtn").attr("readOnly", true);
                                return;
                            }
                            else {
                                $("#CardcodeVer").html("卡号有误");
                                $("#CardcodeVer").css("color", "red");
                                $("hid_ture").val(0);
                                $("#confirmBtn").attr("readOnly", true);
                                return;
                            }

                        }
                    })
                }
            })

            //提交按钮
            $("#confirmBtn").click("click", function () {
                var Cardcode = $("#Cardcode").val();
                var Phone = $("#hid_phone").val();

                if (Cardcode == "") {
                    $("#CardcodeVer").html("请填写实体卡号");
                    $("#CardcodeVer").css("color", "red");
                    return;
                }


                if (Phone == "") {
                    alert("请先完善个人信息");
                    location.href = "userinfo.aspx";
                    return;
                }

                if ($("hid_ture").val() == 0) {
                    $("#CardcodeVer").html("卡号错误");
                    $("#CardcodeVer").css("color", "red");
                    return;
                }

                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=upcard", { Cardcode: Cardcode, comid: comid, Phone: Phone }, function (wdata) {
                    wdata = eval("(" + wdata + ")");

                    if (wdata.type == 100) {
                        if (wdata.msg == "OK") {
                            location.href = "indexcard.aspx";
                            return;
                        }
                        else {
                            $("#CardcodeVer").html(wdata.msg);
                            $("#CardcodeVer").css("color", "red");
                            return;
                        }
                    } else {
                        $("#CardcodeVer").html("绑定卡号错误，请重新绑定");
                        $("#CardcodeVer").css("color", "red");
                        return;
                    }



                })

            })



        })
    </script>
</head>
<body class="" id="page_bind">
    <div id="loading" style="top: 150px; display: none;">
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

    <div id="mappContainer">
        <div style="height: 327px;" class="inner root">
            <p>
                填写信息将会员卡放到微信,从此不用携带卡片</p>
            <form>
            <fieldset>
                <div>
                    <input id="Cardcode" placeholder="请输入实体卡号" type="tel"/></div>
            </fieldset><span id="CardcodeVer"></span>
            <a href="#" class="sub" id="confirmBtn"> 提  交</a></form>
        </div>
    </div>

    <!--<div class="footFix" id="footReturn">
            <a href="indexcard.aspx"><span>返回会员卡首页</span></a>
    </div>-->
    <script>
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            WeixinJSBridge.call('hideToolbar');
        });
    </script>

    <input id="hid_card" type="hidden" value="<%=AccountCard %>" />
    <input id="hid_f" type="hidden" value="<%=fcard %>" />
    <input id="hid_phone" type="hidden" value="<%=Accountphone %>" />
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_comid" value="<%=comid %>" />
    <input type="hidden" id="hid_ture" value="0" />
</body>
</html>
