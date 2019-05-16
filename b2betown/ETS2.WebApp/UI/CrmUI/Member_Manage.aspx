<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Member_Manage.aspx.cs"
    Inherits="ETS2.WebApp.UI.CrmUI.Member_Manage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .pro1
        {
            width: 200px;
            float: left;
            padding-bottom: 2px;
            padding-left: 5px;
            border-bottom: inset 1px;
        }
        .pro2
        {
            width: 100px;
            float: left;
            padding-bottom: 2px;
            border-bottom: inset 1px;
        }
        .pro3
        {
            width: 200px;
            float: left;
            padding-bottom: 2px;
            border-bottom: inset 1px;
        }
        .pro4
        {
            width: 120px;
            float: left;
            padding-bottom: 2px;
            border-bottom: inset 1px;
        }
        .pro5
        {
            width: 100px;
            float: left;
            padding-bottom: 2px;
            border-bottom: inset 1px;
        }
    </style>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //判断是否可以设置预付款
            $.post("/JsonFactory/PermissionHandler.ashx?oper=getGroupByUserId", { userid: $("#hid_userid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    
                    if (data.msg.iscanset_imprest == 0) {
                       
                        $("#add_imprest").attr("disabled", "disabled");
                        $("#reduce_imprest").attr("disabled", "disabled");
                        $("#yufukuanjilu").attr("disabled", "disabled");
                    }
                    if (data.msg.iscanset_imprest == 1) {
                        $("#add_imprest").removeAttr("disabled");
                        $("#reduce_imprest").removeAttr("disabled");
                        $("#yufukuanjilu").removeAttr("disabled");
                    }
                }
            })

            //首先加载数据
            var hid_id = $("#hid_id").trimVal();
            var comid = $("#hid_comid").trimVal();
            var pageSize = 50; //每页显示条数

            var pageindex = 1;

            if (hid_id != 0) {
                $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=readuser", { id: hid_id, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#Name_span").html(data.msg.Name);
                        $("#Phone_span").html(data.msg.Phone);
                        $("#Email_span").html(data.msg.Email);
                        $("#Regidate_span").html(ChangeDateFormat(data.msg.Regidate));
                        $("#Sex_span").html(data.msg.Sex);
                        $("#Idcard_span").html(data.msg.Idcard);
                        $("#Servercard_span").html(data.msg.Servercard);
                        $("#Imprest_span").html(data.msg.Imprest);
                        $("#Integral_span").html(data.msg.Integral);
                        $("#span_totaldjf").html(data.msg.Dengjifen);

                        if (data.Namechannl != null) {
                            //$("#channelcard_span").html(data.Namechannl.Cardcode);
                            $("#channel_span").html(data.Namechannl.Name);
                            //                            if (data.Namechannl.Issuetype == 1) //(1外、0内、3网、4微)
                            //                            {    
                            //                               $("#channel_span").html("外部渠道会员");
                            //                            } else if (data.Namechannl.Issuetype == 2) {
                            //                                $("#channel_span").html("内部渠道会员");
                            //                            } else if (data.Namechannl.Issuetype == 3) {
                            //                                $("#channel_span").html("网站注册会员");
                            //                            } else if (data.Namechannl.Issuetype == 4) {
                            //                                $("#channel_span").html("微信注册会员");
                            //                            }


                        }
                        $("#WxNickname").html(data.msg.WxNickname);
                        $("#WxCity").html(data.msg.WxCity);
                        if (data.msg.WxSex == 0) {
                            $("#WxSex").html("未知");
                        } else if (data.msg.WxSex == 1) {
                            $("#WxSex").html("男");
                        } else {
                            $("#WxSex").html("女");
                        }


                    }
                })
                //读取等积分记录
                $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=readdengjifen", { id: hid_id, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg.length > 0) {
                            //填充列表数据
                            $.each(data.msg, function (i, item) {
                                $("#uldengjifen").append('<li class="cl" >'
                                        + '<div class="pro1 titlefont">'
                                           + item.admin
                                        + '</div>'
                                        + '<div class="pro2">'
                                            + item.money
                                        + '</div>'
                                        + '<div class="pro3 pricefont">'
                                             + jsonDateFormatKaler(item.subdate)
                                        + '</div>'
                                        + '<div class="pro5">'
                                           + item.orderid
                                        + '</div>'
                                        + '<div class="pro4">'
                                           + item.ordername
                                        + '</div>'
                                        + '</li>');
                            })
                        }
                    }
                })

                //读取预付款记录
                $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=readimprest", { id: hid_id, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg.length > 0) {
                            //填充列表数据
                            $.each(data.msg, function (i, item) {
                                $("#ulimprest").append('<li class="cl" >'
                                        + '<div class="pro1 titlefont">'
                                           + item.admin
                                        + '</div>'
                                        + '<div class="pro2">'
                                            + item.money
                                        + '</div>'
                                        + '<div class="pro3 pricefont">'
                                             + jsonDateFormatKaler(item.subdate)
                                        + '</div>'
                                        + '<div class="pro5">'
                                           + item.orderid
                                        + '</div>'
                                        + '<div class="pro4">'
                                           + item.ordername
                                        + '</div>'
                                        + '</li>');
                            })
                        }
                    }
                })
                //读取积分记录
                $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=readintegral", { id: hid_id, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg.length > 0) {
                            //填充列表数据
                            $.each(data.msg, function (i, item) {
                                $("#ulintegral").append('<li class="cl" >'
                                        + '<div class="pro1 titlefont">'
                                           + item.admin
                                        + '</div>'
                                        + '<div class="pro2">'
                                            + item.money
                                        + '</div>'
                                        + '<div class="pro3 pricefont">'
                                             + jsonDateFormatKaler(item.subdate)
                                        + '</div>'
                                        + '<div class="pro5">'
                                           + item.orderid
                                        + '</div>'
                                        + '<div class="pro4">'
                                           + item.ordername
                                        + '</div>'
                                        + '</li>');
                            })
                        }

                    }
                })


                //获取订单列表
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getuserorderlist",
                    data: { userid: $("#hid_id").val(), comid: comid, pageindex: 1, pagesize: 50, order_state: 0, ordertype: 1 },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                //setpage(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })

            }


            //分页
            function setpage(newcount, newpagesize, curpage, userid, comid, key, iswxfocus, ishasweixin) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchList(page, userid, comid, key, iswxfocus, ishasweixin);

                        return false;
                    }
                });
            }
            //修改会员休息
            $("#upmember").click(function () {
                if (hid_id != 0) {
                    $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=readuser", { id: hid_id, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("获取数据出现错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#Name").val(data.msg.Name);
                            $("#Phone").val(data.msg.Phone);
                            $("#Email").val(data.msg.Email);
                            $("input[name='Sex'][value='" + data.msg.Sex + "']").attr("checked", true)
                            $("#Idcard").val(data.msg.Idcard);
                            $("#Age").val(data.msg.Age);
                        }
                    })

                }
                $("#MemberInfo").show();
            })

            //初始会员密码
            $("#initial").click(function () {
                if (confirm('确认初始密码?')) {
                    if (hid_id != 0) {
                        $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=initialuser", { id: hid_id, comid: comid }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                $.prompt("操作数据出现错误");
                                return;
                            }
                            if (data.type == 100) {
                                if (data.msg == 0) {
                                    $.prompt("参数传递出错，请重新操作");
                                    return;
                                } else if (data.msg != 0) {
                                    $.prompt("初始会员密码成功");
                                    return;
                                }
                            }
                        })

                    }
                }
                else {
                    return false;
                }
            })



            //充现金卷
            $("#add_integral").click(function () {
                $("#Operating").show();
                $("#OperatingTitle").html("充现金卷");
                $("#OperatingName").html($("#Name_span").html() + $("#WxNickname").html());
                $("#OperatingType").html("本次充现金卷金额");
                $("#acttype").val("add_integral");

            })

            $("#reduce_integral_2").click(function () {
                $("#reduce_integral").click();
            })
            //减现金卷
            $("#reduce_integral").click(function () {
                $("#Operating").show();
                $("#OperatingTitle").html("减现金卷");
                $("#OperatingName").html($("#Name_span").html() + $("#WxNickname").html());
                $("#OperatingType").html("本次减现金卷金额");
                $("#acttype").val("reduce_integral");
            })


            //充预付款
            $("#add_imprest").click(function () {

                $("#Operating").show();
                $("#OperatingTitle").html("充预付款");
                $("#OperatingName").html($("#Name_span").html() + $("#WxNickname").html());
                $("#OperatingType").html("本次充预付款金额");
                $("#acttype").val("add_imprest");
            })


            //减预付款
            $("#reduce_imprest").click(function () {
                $("#Operating").show();
                $("#OperatingTitle").html("减预付款");
                $("#OperatingName").html($("#Name_span").html() + $("#WxNickname").html());
                $("#OperatingType").html("本次减预付款金额");
                $("#acttype").val("reduce_imprest");
            })

            $("#cancel").click(function () {
                $("#acttype").val("");
                $("#Operating").hide();
            })
            $("#cancelmember").click(function () {
                $("#MemberInfo").hide();
            })


            $("#submit").click(function () {
                var acttype = $("#acttype").val();
                var money = $("#money").val();
                var ordername = $("#ordername").val();

                if (money == 0 || money == "") {
                    $.prompt("请填写金额");
                    return;
                }
                if (acttype == "") {
                    $.prompt("出错，请重新操作");
                    return;
                }


                $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=writemoney", { id: hid_id, comid: comid, acttype: acttype, money: money, ordername: ordername, remark: ordername }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == 0) {
                            $.prompt("参数传递出错，请重新操作");
                            return;
                        } else if (data.msg != 0) {
                            $.prompt("录入成功");
                            $("#acttype").val("");
                            $("#ordername").val("");
                            $("#money").val(0);
                            $("#Operating").hide();
                            location.reload();
                            return;
                        }
                    }
                })


            })

            //修改推荐人和渠道
            $("#referrer_ch").click(function () {
                $("#rhshow").show();
            })
            $("#cancel_rh").click(function () {
                $("#rhshow").hide();
            })

            $("#submit_rh").click(function () {
                var channlcard = $("#channlcard").val();
                var oldcard = $("#oldcard").val();
                var Uptype = $('input:radio[name="Uptype"]:checked').val();
                if (channlcard == "") {
                    $.prompt("请填写渠道卡号");
                    return;
                }
                if (Uptype == "" || Uptype == null) {
                    $.prompt("请选择修改项");
                    return;
                }


                $.post("/JsonFactory/ChannelHandler.ashx?oper=upchannl", { id: hid_id, comid: comid, channlcard: channlcard, oldcard: oldcard, Uptype: Uptype }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            $.prompt("修改成功");
                            $("#rhshow").hide();
                            location.reload();
                            return;

                        }
                        else {
                            $.prompt("参数传递出错，请重新操作");
                            return;
                        }

                    }
                })
            })



            $("#subupmember").click(function () {
                var Name = $("#Name").val();
                var Sex = $('input:radio[name="Sex"]:checked').val();
                var Idcard = $("#Idcard").val();
                var Phone = $("#Phone").val();
                var Email = $("#Email").val();
                var Age = $("#Age").val();

                if (Name == "") {
                    $.prompt("请填写姓名");
                    return;
                }
                if (Phone == "") {
                    $.prompt("请填写手机");
                    return;
                }

                if (Sex == "") {
                    $.prompt("请选择性别");
                    return;
                }


                $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=upmember", { id: hid_id, comid: comid, Name: Name, Sex: Sex, Idcard: Idcard, Phone: Phone, Email: Email, Age: Age }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == 0) {
                            $.prompt("参数传递出错，请重新操作");
                            return;
                        } else if (data.msg != 0) {
                            $.prompt("修改成功");
                            $("#MemberInfo").hide();
                            location.reload();
                            return;
                        }
                    }
                })


            })

            $("#imprest_close").click(function () {
                $("#yufukuanjilu_info").hide();
            })
            $("#integral_close").click(function () {
                $("#jifenguanli_info").hide();
            })
            $("#dengjifen_close").click(function () {
                $("#dengjifen_info").hide();
            })



            $("#huiyuanjibenxinxi").click(function () {
                $("#huiyuanjibenxinxi_info").show();
                $("#huiyuanjibenxinxi").addClass("on");
                $("#xiaofeijilu").removeClass("on");
                $("#yufukuanjilu_info").hide();
                $("#jifenguanli_info").hide();
                $("#xiaofeijilu_info").hide();

            })

            $("#yufukuanjilu").click(function () {
                $("#yufukuanjilu_info").show();
                $("#xiaofeijilu").removeClass("on");
                $("#jifenguanli_info").hide();
                $("#xiaofeijilu_info").hide();
            })

            $("#jifenguanli").click(function () {

                $("#yufukuanjilu_info").hide();
                $("#jifenguanli_info").show();
                $("#yufukuanjilu").removeClass("on");
                $("#xiaofeijilu").removeClass("on");
                $("#xiaofeijilu_info").hide();
            })

            $("#xiaofeijilu").click(function () {
                $("#huiyuanjibenxinxi_info").hide();
                $("#yufukuanjilu_info").hide();
                $("#jifenguanli_info").hide();
                $("#xiaofeijilu_info").show();
                $("#huiyuanjibenxinxi").removeClass("on");
                $("#yufukuanjilu").removeClass("on");
                $("#jifenguanli").removeClass("on");
                $("#xiaofeijilu").addClass("on");
            })


        });

        function setxingqu() {
            var crmid = $("#hid_id").val();
            window.open("member_interest_manage.aspx?crmid=" + crmid + "&comurl=1", target = "_self");
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul id="daohang">
                <li><a href="BusinessCustomersList.aspx" onfocus="this.blur()">会员列表</a></li>
                <li id="huiyuanjibenxinxi" class="on"><a href="javascript:;" onfocus="this.blur()">会员基本信息管理</a></li>
                <li id="xiaofeijilu"><a href="javascript:;" onfocus="this.blur()">消费记录</a></li>
                <li><a href="javascript:;" onfocus="this.blur()" onclick="setxingqu()">兴趣设置</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                </h3>
                <div id="huiyuanjibenxinxi_info" class="edit-box J-commonSettingsModule" style="opacity: 1;
                    margin-top: 0px; position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        会员基本信息</h2>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            昵称(微信标注姓名)：</label>
                        <span id="WxNickname" style="font-size: 20px; color: #ff6633"></span>
                        <br />
                        <input type="button" name="initial" id="initial" value="初始会员密码" />
                        <input type="button" name="upmember" id="upmember" value="  修改会员信息  " />
                    </div>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            所属城市 ：</label>
                        <span id="WxCity" style="font-size: 20px; color: #ff6633"></span>
                    </div>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            性 别 ：</label>
                        <span id="WxSex" style="font-size: 20px; color: #ff6633"></span>
                    </div>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            客人姓名 ：</label>
                        <span id="Name_span" style="font-size: 20px; color: #ff6633"></span>
                    </div>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            手 机 ：</label>
                        <span id="Phone_span" style="font-size: 20px; color: #ff6633"></span>
                    </div>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            卡 号 ：</label>
                        <span id="Idcard_span" style="font-size: 20px; color: #ff6633"></span>
                    </div>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            注册时间 ：</label>
                        <span id="Regidate_span" style="font-size: 20px; color: #ff6633"></span>
                    </div>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            预 付 款 ：</label>
                        <span id="Imprest_span" style="font-size: 20px; color: #ff6633"></span>元
                        <br />
                        <input type="button" name="add_imprest" id="add_imprest" value="  充预付款  " />
                        <input type="button" name="reduce_imprest" id="reduce_imprest" value="  减预付款  " />
                        <input type="button" name="yufukuanjilu" id="yufukuanjilu" value="  预付款记录  " />
                    </div>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            积 分 ：</label>
                        <span id="Integral_span" style="font-size: 20px; color: #ff6633"></span>
                        <br />
                        <input type="button" name="add_integral" id="add_integral" value="  充积分  " />
                        <input type="button" name="reduce_integral" id="reduce_integral" value="  减积分  " />
                        <input type="button" name="jifenguanli" id="jifenguanli" value="  积分消费记录  " />
                    </div>
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            等积分(请设置好<a href="/ui/crmui/B2bCrm_LevelManage.aspx">会员级别</a>) ：</label>
                        <span id="span_totaldjf" style="font-size: 20px; color: #ff6633"></span>
                        <br />
                        <input type="button" id="add_dengjifen" value="  充等积分  " />
                        <input type="button" id="reduce_dengjifen" value="  减等积分  " />
                        <input type="button" id="dengjifenlog" value="  等积分变动记录  " />
                    </div>
                    <script type="text/javascript">
                        $(function () {
                            //充等积分
                            $("#add_dengjifen").click(function () {
                                $("#Operating").show();
                                $("#OperatingTitle").html("充等积分");
                                $("#OperatingName").html($("#Name_span").html() + $("#WxNickname").html());
                                $("#OperatingType").html("本次充等积分");
                                $("#acttype").val("add_dengjifen");
                            })
                            //减等积分
                            $("#reduce_dengjifen").click(function () {
                                $("#Operating").show();
                                $("#OperatingTitle").html("减等积分");
                                $("#OperatingName").html($("#Name_span").html() + $("#WxNickname").html());
                                $("#OperatingType").html("本次减等积分");
                                $("#acttype").val("reduce_dengjifen");
                            })
                            //等积分变动记录
                            $("#dengjifenlog").click(function () {
                               $("#yufukuanjilu_info").hide();
                                $("#jifenguanli_info").hide();
                                $("#dengjifen_info").show();
                                $("#yufukuanjilu").removeClass("on");
                                $("#xiaofeijilu").removeClass("on");
                                $("#xiaofeijilu_info").hide();
                            })

                           
                        })
                    </script>
                      
                    <div class="mi-form-item">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            渠 道 ：</label>
                        <span id="channel_span"></span><span id="ref_Span"></span><span id="channelcard_span">
                        </span>
                        <input type="button" name="referrer_ch" id="referrer_ch" value="修改渠道和推荐人" />
                    </div>
                    <div class="mi-form-item"  style="display: none;">
                        <label style="color: #4D4D4D; font-size: 14px; padding-right: 10px;">
                            渠 道 号 ：</label>
                        <span id="referrer_span"></span>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
                    width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;
                    z-index: 100;">
                    <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
                        style="padding: 5px;">
                        <tr>
                            <td height="42" colspan="2" bgcolor="#C1D9F3">
                                <span style="padding-left: 10px; font-size: 18px;" id="span_rh"></span>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                                新渠道卡号：
                            </td>
                            <td height="30" bgcolor="#E7F0FA" class="tdHead">
                                <input type="text" id="channlcard" name="channlcard" /><br />
                                <input type="radio" name="Uptype" value="1" />更换发行渠道
                                <input type="radio" name="Uptype" value="2" />更换服务号
                                <input type="radio" name="Uptype" value="3" />发行渠道，服务号都更换
                                <input id="oldcard" type="hidden" />
                            </td>
                        </tr>
                        <tr>
                            <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                                <input name="acttype" id="Hidden2" type="hidden" value="0" />
                                <input name="submit_rh" id="submit_rh" type="button" class="formButton" value="  确  定  " />
                                <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" value="  取  消  " />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Operating" style="background-color: #ffffff; border: 2px solid #5984bb;
                    margin: 0px auto; width: 400px; height: 190px; display: none; left: 20%; position: absolute;
                    top: 20%; z-index: 100;">
                    <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
                        style="padding: 5px;">
                        <tr>
                            <td height="42" colspan="2" bgcolor="#C1D9F3">
                                <span style="padding-left: 10px; font-size: 18px;" id="OperatingTitle"></span>
                            </td>
                        </tr>
                        <tr>
                            <td width="40%" height="35" align="right" bgcolor="#E7F0FA">
                                客户姓名:
                            </td>
                            <td width="60%" bgcolor="#E7F0FA">
                                <span id="OperatingName"></span>
                            </td>
                        </tr>
                        <tr>
                            <td height="38" align="right" bgcolor="#E7F0FA">
                                <span id="OperatingType"></span>:
                            </td>
                            <td bgcolor="#E7F0FA">
                                <input name="money" id="money" type="text" size="10" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td height="38" align="right" bgcolor="#E7F0FA">
                                <span id="">备注说明</span>:
                            </td>
                            <td bgcolor="#E7F0FA">
                                <input name="ordername" id="ordername" type="text" maxlength="50" size="20" />
                            </td>
                        </tr>
                        <tr>
                            <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                                <input name="acttype" id="acttype" type="hidden" value="0" />
                                <input name="submit" id="submit" type="button" class="formButton" value="  确  定  " />
                                <input name="cancel" id="cancel" type="button" class="formButton" value="  取  消  " />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="MemberInfo" style="background-color: #ffffff; border: 2px solid #5984bb;
                    margin: 0px auto; width: 500px; height: 260px; display: none; left: 20%; position: absolute;
                    top: 20%; z-index: 100;">
                    <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
                        style="padding: 5px;">
                        <tr>
                            <td height="42" colspan="2" bgcolor="#C1D9F3">
                                <span class="STYLE1">修改会员信息</span>
                            </td>
                        </tr>
                        <tr>
                            <td width="40%" height="30" align="right" bgcolor="#E7F0FA">
                                客户姓名:
                            </td>
                            <td width="60%" height="30" bgcolor="#E7F0FA">
                                <input name="Name" type="text" id="Name" size="10">
                                <label>
                                    <input type="radio" name="Sex" value="男">
                                    男</label>
                                <label>
                                    <input type="radio" name="Sex" value="女">
                                    女</label>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" bgcolor="#E7F0FA">
                                <span id="Span1"></span>卡号:
                            </td>
                            <td height="30" bgcolor="#E7F0FA">
                                <input name="Idcard" type="text" id="Idcard" readonly="readonly">
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                                手机:
                            </td>
                            <td height="30" bgcolor="#E7F0FA" class="tdHead">
                                <input name="Phone" type="text" id="Phone">
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                                邮箱:
                            </td>
                            <td height="30" bgcolor="#E7F0FA" class="tdHead">
                                <input name="Email" type="text" id="Email">
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                                年龄:
                            </td>
                            <td height="30" bgcolor="#E7F0FA" class="tdHead">
                                <label>
                                    <input name="Age" type="text" id="Age" size="4" maxlength="3">
                                    (数字格式)
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                                <input name="acttype" id="Hidden1" type="hidden" value="0" />
                                <input name="subupmember" id="subupmember" type="button" class="formButton" value="  确  定  " />
                                <input name="cancelmember" id="cancelmember" type="button" class="formButton" value="  取  消  " />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="yufukuanjilu_info" style="background-color: rgb(255, 255, 255); border: 2px solid rgb(89, 132, 187);
                    margin: 0px auto; width: 780px; height: 400px; left: 10%; position: absolute;
                    top: 20%; z-index: 100; display: none;">
                    <table width="780" cellpadding="10" cellspacing="1" class="grid">
                        <tr>
                            <td width="100%" height="26" align="center" bgcolor="#1E5494" style="color: #ffffff;
                                font-weight: bold;" class="tdHead">
                                <span style="float: left;">预付款记录</span><span style="float: right; cursor: pointer;"
                                    id="imprest_close">×</span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" bgcolor="#efefef">
                                <ul id="ulimprest">
                                    <li>
                                        <div class="pro1 titlefont">
                                            操作人
                                        </div>
                                        <div class="pro2">
                                            金额（元）
                                        </div>
                                        <div class="pro3 pricefont">
                                            时间
                                        </div>
                                        <div class="pro5">
                                            订单号</div>
                                        <div class="pro4">
                                            服务专员
                                        </div>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="jifenguanli_info" style="background-color: rgb(255, 255, 255); border: 2px solid rgb(89, 132, 187);
                    margin: 0px auto; width: 780px; height: 400px; left: 10%; position: absolute;
                    top: 20%; z-index: 100; display: none;">
                    <table width="780" cellpadding="10" cellspacing="1" class="grid">
                        <tr>
                            <td width="100%" align="center" bgcolor="#1E5494" style="color: #ffffff; font-weight: bold;">
                                <span style="float: left;">积分消费记录</span><span style="float: right; cursor: pointer;"
                                    id="integral_close">×</span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" bgcolor="#efefef">
                                <ul id="ulintegral">
                                    <li>
                                        <div class="pro1 titlefont">
                                            操作人
                                        </div>
                                        <div class="pro2">
                                            金额（元）
                                        </div>
                                        <div class="pro3 pricefont">
                                            时间
                                        </div>
                                        <div class="pro5">
                                            订单号</div>
                                        <div class="pro4">
                                            旅游专员</div>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </div>

                  <div id="dengjifen_info" style="background-color: rgb(255, 255, 255); border: 2px solid rgb(89, 132, 187);
                    margin: 0px auto; width: 780px; height: 400px; left: 10%; position: absolute;
                    top: 20%; z-index: 100; display: none;">
                    <table width="780" cellpadding="10" cellspacing="1" class="grid">
                        <tr>
                            <td width="100%" align="center" bgcolor="#1E5494" style="color: #ffffff; font-weight: bold;">
                                <span style="float: left;">等积分消费记录</span><span style="float: right; cursor: pointer;"
                                    id="dengjifen_close">×</span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" bgcolor="#efefef">
                                <ul id="uldengjifen">
                                    <li>
                                        <div class="pro1 titlefont">
                                            操作人
                                        </div>
                                        <div class="pro2">
                                            等积分
                                        </div>
                                        <div class="pro3 pricefont">
                                            时间
                                        </div>
                                        <div class="pro5">
                                            订单号</div>
                                        <div class="pro4">
                                            备注</div>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="xiaofeijilu_info" style="display: none;" class="mail-list">
                    <table width="780px" border="0">
                        <tr>
                            <td width="45px" height="30px">
                                <p align="left">
                                    ID
                                </p>
                            </td>
                            <td width="100px">
                                <p align="left">
                                    提交时间
                                </p>
                            </td>
                            <td width="147px">
                                <p align="left">
                                    产品名称
                                </p>
                            </td>
                            <td width="70px">
                                <p align="left">
                                </p>
                            </td>
                            <td width="100px">
                                <p align="left">
                                    购买人
                                </p>
                            </td>
                            <td width="45px">
                                <p align="center">
                                    应收
                                </p>
                            </td>
                            <td width="30px">
                                <p align="center">
                                    优惠
                                </p>
                            </td>
                            <td width="45px">
                                <p align="center">
                                    实收
                                </p>
                            </td>
                            <td width="50px">
                                <p align="center">
                                    备注
                                </p>
                            </td>
                            <td width="35px">
                                <p align="center">
                                    来源
                                </p>
                            </td>
                            <td width="60px">
                                <p align="center">
                                    渠道
                                </p>
                            </td>
                            <td width="180px">
                                <p align="center">
                                    状态
                                </p>
                            </td>
                        </tr>
                        <tbody id="tblist">
                        </tbody>
                    </table>
                    <div id="divPage">
                    </div>
                    <p>
                        &nbsp;
                    </p>
                </div>
            </div>
        </div>
        <div class="data">
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr class="fontcolor">
                        <td valign="top">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${jsonDateFormatKaler(U_subdate)}</p>
                        </td>
                        <td valign="top">
                            <p align="left">
                               ${Proname}(数量:${U_num})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                {{if Server_type!=1 }}${ChangeDateFormat(U_traveldate)}{{else}}订单号:${Id}{{/if}}
                                
                                </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${U_name}(${U_phone})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Server_type!=1}}${Pay_price}{{else}} ${Totalcount}{{/if}}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                                 {{if Order_state>1}} ${Integral1}{{/if}}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Server_type!=1}}${Pay_price}{{else}}{{if Order_state>1}} ${Paymoney} {{if Imprest1>=1}}+ ${Imprest1}{{/if}}{{/if}}{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                           ${Ticketinfo}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Agentid!="0"}} 分销 {{if Warrant_type=="1"}}(出){{else}}(验){{/if}} {{else}}{{if Openid==""}}网站{{else}}微信{{/if}}{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                        <p align="center">
                         ${Channelcompanyname}
                        </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                            {{if Order_state==1}}
                                {{if Server_type==3}}
                                    旅游优惠券
                                {{else}}
                                    {{if Server_type!=1 }}
                                        <input type="button" onclick="orderfin('${Id}')" style="color:#ff0000"  value="未处理"/>
                                    {{else}}
                                        {{if Order_type==1}}
                                             {{if Warrant_type==1}}
                                                 {{if Agentid=="0"}}
                                                     等待对方付款
                                                 {{else}}
                                                     分销购票失败
                                                 {{/if}}
                                             {{else}}
                                                 <input type="button" onclick="agentwarrant_ch('${Agent_company} 分销倒码确认','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}')"  value="分销倒码, 待确认"/>
                                             {{/if}}
                                        {{else}}
                                            充值等待支付
                                        {{/if}}
                                    {{/if}}
                                {{/if}}
                            {{/if}}

                            {{if Order_state==2}}
                                ${Order_state_str}
                                {{if Order_type==1}}
                                     <input type="button" onclick="sendticketsms('${Id}')" style="color:#ff0000"  value="立即发码"/>(可能码原不足)
                                {{else}}
                                     （充值失败，请手工为此会员充值）
                                {{/if}}
                                <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}')"  value="退票"/>
                            {{/if}}
                            {{if Order_state==4}}
                                
                                 {{if Warrant_type==1}}
                                     <input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                     <input type="button" onclick="referrer_ch('${U_name}(${U_phone})',' ${Proname}','${Order_eticket_code}')"  value="短信"/>
                                     <input type="button" onclick="referrer_ch('查看本订单支付详情','','${Pay_str}')"  value="支付"/>
                                     <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}')"  value="退票"/>
                                 {{else}}
                                     <a href="/Agent/DownExcel.aspx?comid=${Comid}&agentid=${Agentid}&orderid=${Id}&md5info=${Returnmd5}" style="font-size: 14px; font-weight: bold;color:#333933">下载电子码</a>
                                 {{/if}}
                            {{/if}}
                            {{if Order_state==6}}
                            已充值
                            {{/if}}
                            {{if Order_state==8}}
                            已消费
                            {{/if}}
                            {{if Order_state==16}}
                            订单退票
                            {{/if}}
                            {{if Order_state==17}}
                            申请退票中
                            {{/if}}
                            {{if Order_state==18}}
                            电子码已作废，退款处理中
                            {{/if}}
                            {{if Order_state==19}}
                            作废
                            {{/if}}
                            {{if Order_state==20}}
                            发码出错
                            {{/if}}
                            {{if Order_state==21}}
                            重新发码出错
                            {{/if}}
                            {{if Order_state==22}}
                            已处理
                            {{/if}}
                            </p>
                        </td>
                    </tr>
        </script>
    </div>
    </div>
    <p>
        &nbsp;</p>
    </div> </div> </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_id" value="<%=id %>" />
</asp:Content>
