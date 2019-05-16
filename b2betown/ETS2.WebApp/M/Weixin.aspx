<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/M/MemberH5.Master" CodeBehind="Weixin.aspx.cs" Inherits="ETS2.WebApp.M.Weixin" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();
            var openid = $("#hid_openid").trimVal();
            var AccountId = $("#hid_AccountId").trimVal();
            $("#Wbt").hide();

            //已登录账户禁止修改
            if (AccountId != 0) {
                $("#Cardcode").attr("readOnly", true);
                $(".classlogin").hide();
                $("#confirmBtn").text("返回 会员优惠活动");

                //绑定手机(判断手机)
                var Password1 = $("#Password1").val();
                var Name = $("#Name").val();
                var Phone = $("#Phone").val();

                if (Phone == null || Name == null || Phone == "" || Name == "") {
                    $("#Phone").blur(function () {
                        $("#PhoneVer").html("");
                        var Phone = $("#Phone").val();
                        if (Phone != "") {
                            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getPhone", { Phone: Phone, comid: comid }, function (data) {
                                data = eval("(" + data + ")");
                                if (data.type == 100) {
                                    if (data.msg == "OK") {
                                        $("#displayPhone").show(); //绑定手机
                                        return;
                                    }
                                    else {
                                        //如果是已注册账户则，现实密码
                                        if (data.regitype == "old") {
                                            $("#Password1Ver").html(data.msg + ",请下面输入此手机账户的密码直接绑定!");
                                            $("#Password1Ver").css("color", "red");
                                            $(".classpass").show();
                                            $(".classname").hide();
                                            $(".classcard").hide();
                                            $("#confirmBtn").hide();
                                            $("#Wbt").show();
                                            $("#Wbt").text("提交绑定微信");

                                            return;
                                        }
                                    }
                                } else {
                                    $("#PhoneVer").html(data.msg);
                                    $("#PhoneVer").css("color", "red");
                                    $("#VPhone").val(0);
                                    return;
                                }
                            })
                        }
                    })

                    $("#PhoneBtn").click(function () {
                        var Phone = $("#Phone").val();
                        if (Phone != "") {
                            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=BtnPhone", { Phone: Phone, comid: comid, AccountId: AccountId }, function (Btndata) {
                                Btndata = eval("(" + Btndata + ")");
                                if (Btndata.type == 100) {
                                    if (Btndata.msg == "OK") {
                                        $("#displayPhone").hide();
                                        $("#PhoneBtn").attr("readOnly", true);
                                        return;
                                    }
                                }
                                else {
                                    $("#PhoneVer").val("请稍后再试！");
                                    $("#PhoneVer").css("color", "red");
                                    $("#VPhone").val(0);
                                    return;
                                }
                            })
                        }
                    })

                    //填写姓名
                    $("#Name").click(function () {
                        if (Name == "") {
                            $("#displayName").show();
                        }
                        else {
                            $("#Name").attr("readOnly", true);
                        }
                    })
                    $("#NameBtn").click(function () {
                        var Name = $("#Name").val();
                        if (Name != "") {
                            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=BtnName", { Name: Name, comid: comid, AccountId: AccountId }, function (Btndata) {
                                Btndata = eval("(" + Btndata + ")");
                                if (Btndata.type == 100) {
                                    if (Btndata.msg == "OK") {
                                        $("#Name").attr("readOnly", true);
                                        return;
                                    }
                                }
                                else {
                                    $("#NameVer").val("请稍后再试！");
                                    return;
                                }
                            })
                        }
                    })

                    //提交按钮
                    $("#Wbt").click("click", function () {
                        var Password1 = $("#Password1").val();
                        var Name = $("#Name").val();
                        var Phone = $("#Phone").val();

                        if (openid == "") {
                            if (Password1 == "") {
                                $("#Password1Ver").html("请填写密码");
                                $("#Password1Ver").css("color", "red");
                                return;
                            }
                        }

                        if (Phone == "") {
                            $("#PhoneVer").html("请填写手机");
                            $("#PhoneVer").css("color", "red");
                            return;
                        }

                        $(".loading-text").html("正在提交绑定信息，请稍后...")

                        if (openid != "") {
                            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Btnopenid", { openid: openid, comid: comid, Password1: Password1, Phone: Phone }, function (wdata) {
                                wdata = eval("(" + wdata + ")");

                                if (wdata.type == 100) {
                                    if (wdata.msg == "OK") {
                                        $(".loading-text").html(" ");
                                        location.reload();
                                        return;
                                    } else {
                                        $("#PhoneVer").html(wdata.msg);
                                        $("#PhoneVer").css("color", "red");
                                        return;
                                    }
                                } else {
                                    $("#PhoneVer").html(wdata.msg);
                                    $("#PhoneVer").css("color", "red");
                                    return;
                                }



                            })
                        }
                    })


                }
                else {
                    $("input").attr("readOnly", true)
                    $("input").css("background", "#cccccc");
                    $(".classlogin").hide();
                    $("#confirmBtn").text("返回 会员优惠活动");
                    //提交按钮
                    $("#confirmBtn").click("click", function () {
                        location.href = "/M/Default.aspx";
                    })
                }

            } else {

                //判断卡号
                $("#Cardcode").blur(function () {
                    $("#CardcodeVer").html(""); //离开后先清空备注
                    var Cardcode = $("#Cardcode").val();
                    //判断卡号不为空
                    if (Cardcode != "") {
                        $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getCard", { Card: Cardcode, comid: comid }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 100) {
                                if (data.msg != "OK") {
                                    $("#CardcodeVer").html(data.msg);
                                    $("#CardcodeVer").css("color", "red");
                                    $("#VCardcode").val(0);
                                    return;
                                } else {
                                    $("#VCardcode").val(1);
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
                                    if (openid != "" && openid != null) {
                                        $("#VPhone").val(1);
                                        $("#VLogin").val(0);
                                        $(".classpass").hide();
                                        $(".classcard").show();
                                        $(".classname").show();
                                        $("#Password1Ver").html("");
                                    } else {//如果没有微信号为注册电子账户
                                        $("#VPhone").val(1);
                                        $("#VLogin").val(0);
                                        $(".classpass").show();
                                        $(".classname").show();
                                        $(".classcard").show();
                                        $("#Password1Ver").html("");
                                    }
                                } else {
                                    //如果是已注册账户则，现实密码
                                    if (data.regitype == "old") {
                                        $("#Password1Ver").html(data.msg + ",请下面输入此手机账户的密码直接登录!");
                                        $("#Password1Ver").css("color", "red");
                                        $("#VPhone").val(1);
                                        $("#VLogin").val(1);
                                        $(".classpass").show();
                                        $(".classname").hide();
                                        $(".classcard").hide();
                                    } else {//手机号错误返回
                                        $("#PhoneVer").html(data.msg);
                                        $("#PhoneVer").css("color", "red");
                                        $("#VPhone").val(0);
                                        return;
                                    }
                                }
                            }
                            else {//非100返回
                                $("#PhoneVer").html(data.msg);
                                $("#PhoneVer").css("color", "red");
                                $("#VPhone").val(0);
                                return;
                            }
                        })
                    }
                })

                //提交按钮
                $("#confirmBtn").click("click", function () {

                    var Cardcode = $("#Cardcode").val();
                    var Email = $("#Email").val();
                    var Password1 = $("#Password1").val();
                    var Name = $("#Name").val();
                    var Phone = $("#Phone").val();
                    var VLogin = $("#VLogin").val();

                    if (openid == "") {
                        //$("#PhoneVer").html("传递信息错误，请返回重新提交！");
                        //$("#PhoneVer").css("color", "red");
                        //return;
                        if (Password1 == "") {
                            $("#Password1Ver").html("请填写密码");
                            $("#Password1Ver").css("color", "red");
                            return;
                        }
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

                    //如果是登陆判断密码，否则判断姓名
                    if (VLogin == 1) {
                        if (Password1 == "") {
                            $("#Password1Ver").html("请填写密码");
                            $("#Password1Ver").css("color", "red");
                            return;
                        }
                    } else {
                        if (Name == "") {
                            $("#NameVer").html("请填写姓名");
                            $("#NameVer").css("color", "red");
                            return;
                        }
                    }

                    $(".loading-text").html("正在提交开卡信息，请稍后...")

                    //创建订单
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=weixinregcard", { comid: comid, Cardcode: Cardcode, openid: openid, Name: Name, Phone: Phone, VLogin: VLogin, Password1: Password1 }, function (data) {
                        if (VLogin == 0) {
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
                                location.href = "/M/Default.aspx?openid=" + openid;
                                return;
                            }
                        } else {
                            if (data.toString() == "OK") {
                                location.href = "/M/Default.aspx";
                                return;
                            } else {
                                $("#PhoneVer").html(data);
                                $("#PhoneVer").css("color", "red");
                                return;
                            }

                        }
                    })
                })

            }
        })
        function goDefault(Openid) {
            window.location.href = 'Default.aspx';
        }

        //分享到朋友圈
        function weixinShareTimeline(title, desc, link, imgUrl) {
            WeixinJSBridge.invoke('shareTimeline', {
                "img_url": imgUrl,
                //"img_width":"640",
                //"img_height":"640",
                "link": link,
                "desc": desc,
                "title": title
            });
        }

        //发送给好友
        function weixinSendAppMessage(title, desc, link, imgUrl) {
            WeixinJSBridge.invoke('sendAppMessage', {
                //"appid":appId,
                "img_url": imgUrl,
                //"img_width":"640",
                //"img_height":"640",
                "link": link,
                "desc": desc,
                "title": title
            });
        }

        //分享到腾讯微博
        function weixinShareWeibo(title, link) {
            WeixinJSBridge.invoke('shareWeibo', {
                "content": title + link,
                "url": link
            });
        }

        //关注指定的微信号
        function weixinAddContact(name) {
//            WeixinJSBridge.invoke("addContact", { webtype: "1", username: name }, function (e) {
//                WeixinJSBridge.log(e.err_msg);
//                //e.err_msg:add_contact:added 已经添加
//                //e.err_msg:add_contact:cancel 取消添加
//                //e.err_msg:add_contact:ok 添加成功
//                if (e.err_msg == 'add_contact:added' || e.err_msg == 'add_contact:ok') {
//                    //关注成功，或者已经关注过
//                    alert("关注");
//                }
//            })
            WeixinJSBridge.invoke('profile', { 'username': name, 'scene': '57' }, function (e) {
                WeixinJSBridge.log(e.err_msg);
                alert(e.err_msg);
                if (e.err_msg == 'add_contact:added' || e.err_msg == 'add_contact:ok') {
                    //关注成功，或者已经关注过
                    alert("关注");
                }
            })
            // GET "https://api.weixin.qq.com/token?appid=wx1234hello" "&grant_type=authorization_code&code=4433222&redirect_Uri=" "http%3A//www.example.com/weixin_redirect"
           //URL: https://api.weixin.qq.com/token.format
        }



    </script>
<title>微旅行 无V不至</title> 
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="header">
		<span class="left btn_back" onclick="goDefault('<%=openid%>');"></span> 
		设置我的微旅行会员帐号
	</div> 
 
	<div class="info_wrapper"> 
		<form action="" id="loginForm" method="post"> 
            <p class="classlogin">无V不至 会员专享<%=errinfo%></p> 
			<p class="input_group"> 
				<span id="PhoneVer"></span>
				<input id="Phone" name="Phone" type="tel" value="<%=Accountphone %>" placeholder="输入您手机号" required> 
			</p>
            <p class="input_group" style=" display:none" id="displayPhone"> 
				<a href="javascript:;" class="btn_big btn_blue" id="PhoneBtn">确定</a>
			</p>
			<p class="input_group classname"> 
				<span id="NameVer"></span>
				<input id="Name" name="Name" type="text" value="<%=AccountName %>" placeholder="您的姓名" required> 
			</p> 
            <p class="input_group" style=" display:none" id="displayName"> 
				<a href="javascript:;" class="btn_big btn_blue" id="NameBtn">确定</a>
			</p>
            <p class="input_group classpass" style=" display:none;"> 
				<span id="Password1Ver"></span>
				<input id="Password1" name="Password1" type="password" value="" placeholder="请输入密码" required> 
			</p> 

            <p class="classlogin classcard">请输入微旅行会员卡号，没有请不要填写</p> 
            			<p class="input_group classcard"> 
				<span id="CardcodeVer"></span>
				<input id="Cardcode" name="Cardcode" type="number" value="<%=AccountCard %>" placeholder="请输入会员卡号" required /> 
			</p> 
            <% if (AccountId != 0)
               {%>
            <p class="input_group classname"> 
				<span id="Span1"><%="您的预付款："%> <%=Imprest%></span>
			</p> 
                        <p class="input_group classname"> 
				<span id="Span2"><%="您的积分："%> <%=Integral%></span>
			</p> 
            <%} %>



            <% if (Servercard != 0)
               { %>
            <p class="input_group classname"> 
				<span id="Servername"><%="您的旅游顾问：" + Servername%>(<%=Servermobile%>)</span>
			</p> 
			<%} %>
			
			<p><a href="javascript:;" class="btn_big btn_blue" id="confirmBtn">确定</a></p> 
            <p><a href="javascript:;" class="btn_big btn_blue" id="Wbt">确定</a></p> <span class="loading-text"></span>
            <%--<p><input class="btn_blue" type="button" value="关注" onclick="javascript:weixinAddContact('gh_7a29a360cac1')" /></p>--%>
            <p><a href="indexcard.aspx?openid=<%=openid %>&weixinpass=test" id="A1">--</a></p>
		</form> 
	</div> 
    
    <input type="hidden" id="hid_openid" value="<%=openid %>" />
    <input type="hidden" id="hid_AccountId" value="<%=AccountId %>" />
    <input type="hidden" name="VCardcode" id="VCardcode" value="0">
    <input type="hidden" name="VPhone" id="VPhone" value="0">
    <input type="hidden" name="VLogin" id="VLogin" value="0">
</asp:Content>