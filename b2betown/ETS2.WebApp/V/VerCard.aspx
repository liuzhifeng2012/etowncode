<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="VerCard.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.VerCard" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/print.js" type="text/javascript"></script>
    <script language="javascript" src="/Scripts/LodopFuncs.js"></script>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0"
        height="0">
        <param name="License" value="394101459411010811811255105117">
        <param name="LicenseA" value="741444553515055585560596856128">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0" pluginspage="install_lodop32.exe"></embed>
    </object>
    <script type="text/javascript">
        $(function () {

            //光标自动定位
            document.all.pno.focus();
            //捕捉回车
            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#selbtn").click();    //这里添加要处理的逻辑  
                }
            });


            $("#errmsg").hide();

            var comid = $("#hid_comid").trimVal();
            var hidpno = $("#hid_pno").trimVal();


            if (hidpno == "") {
                $("#tr1").show().siblings().hide();
                $("#alast").hide().siblings("#viewpro").hide();
            }
            else {

            }

            //确认查询
            $("#selbtn").click(function () {
                $("#errmsg").hide();
                $("#alast").hide().siblings("#viewpro").hide();



                $("#viewpro").show();
                $("#member").show();
                $("#Table2").hide();

                $("#tblist").html("");
                $("#Member_info").html("");
                $("#Tbody1").html("");
                //清空电子票使用张数
                //$("#usenum").find("option").remove();

                var pno = $("#pno").trimVal();
                if (pno == "") {
                    $("#errmsg").show().find("strong").text("卡号不可为空");
                    return;
                }
                else {
                    //判断卡号是否存在
                    $.post("/JsonFactory/CardHandler.ashx?oper=SearchCard", { pno: pno, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#errmsg").show().find("strong").text(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                //查询优惠劵优惠劵详细信息
                                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=VerActlist", { pno: pno, comid: comid }, function (data1) {
                                    data1 = eval("(" + data1 + ")");
                                    if (data1.type == 1) {
                                        $("#errmsg").show().find("strong").text(data1.msg);
                                        return;
                                    }
                                    if (data1.type == 100) {
                                        if (data1.totalCount == 0) {
                                            $("#errmsg").show().find("strong").text(pno + ":此账户没有可使用的活动！");
                                            return;

                                        } else {
                                            $("#pno").val("");
                                            $("#acctoutinfo").html("账户：" + pno);
                                            $("#ActList").tmpl(data1.msg).appendTo("#tblist");
                                        }
                                    }
                                })
                                //查询账户信息
                                $.post("/JsonFactory/CardHandler.ashx?oper=MemberCardPageList", { cardcode: pno, comid: comid, pageindex: 1, pagesize: 100, issueid: 0, channelid: 0, actid: 0, isopencard: 0 }, function (data2) {
                                    data2 = eval("(" + data2 + ")");
                                    if (data2.type == 1) {
                                        $("#errmsg").show().find("strong").text(data2.msg);
                                        return;
                                    }
                                    if (data2.type == 100) {
                                        if (data2.totalCount == 0) {
                                            //$("#errmsg").show().find("strong").text("此账户没有可使用的活动！");
                                        } else {
                                            $("#Memberinfo").tmpl(data2.msg).appendTo("#Member_info");
                                            $("#pno").val("");
                                        }
                                    }
                                })


                            } else {
                                $("#errmsg").show().find("strong").text(pno + ":" + data.msg);
                                return;
                            }
                        }
                    })
                }
            })

        })

        //使用预付款、积分
        function showKnter(Imprest, Integral, card) {
            $("#Kntershow").show();



            //取消
            $("#Kancel").click(function () {
                $("#Kntershow").hide();
                $("#KrderId").val("");
                $("#KrderM").val("");
                $("#Kales_admin").val("");
            })

            $("#Btnter").click(function () {
                $("#Btnter").attr("disabled", "disabled");
                var KrderId = $("#KrderId").val();
                var KrderM = $("#KrderM").val();
                var Kales_admin = $("#Kales_admin").val();
                var money = $('input:radio[name="money"]:checked').val();
                var comid = $("#hid_comid").trimVal();


                if (KrderId == "" || KrderId == null) {
                    //                    $("#KrderId_Span").html("请填写订单编号");
                    //                    $("#KrderId_Span").css('color', 'red');
                    //                    $("#Btnter").removeAttr("disabled", "disabled");
                    //                    return;
                    KrderId = 0;
                }
                if (KrderM == "" || KrderM == null) {
                    $("#KalesM_Span").html("请填写使用金额");
                    $("#KalesM_Span").css('color', 'red');
                    $("#Btnter").removeAttr("disabled", "disabled");
                    return;
                }
                if (Kales_admin == "" || Kales_admin == null) {
                    //                    $("#Kales_admin_Span").html("请填写服务专员");
                    //                    $("#Kales_admin_Span").css('color', 'red');
                    //                    $("#Btnter").removeAttr("disabled", "disabled");
                    //                    return;
                    Kales_admin = "";
                }
                if (money == "" || money == null) {
                    $("#money_Span").html("请选择使用项");
                    $("#money_Span").css('color', 'red');
                    $("#Btnter").removeAttr("disabled", "disabled");
                    return;
                }
                if (parseInt(money) == 0 && parseInt(KrderM) > parseInt(Imprest)) {
                    $("#money_Span").html("超过预付款范围");
                    $("#money_Span").css('color', 'red');
                    $("#Btnter").removeAttr("disabled", "disabled");
                    return;
                }
                if (parseInt(money) == 1 && parseInt(KrderM) > parseInt(Integral)) {
                    $("#money_Span").html("超过积分范围");
                    $("#money_Span").css('color', 'red');
                    $("#Btnter").removeAttr("disabled", "disabled");
                    return;
                }

                reg = /^[-+]?\d*$/;
                //                if (!reg.test(KrderId)) {
                //                    $("#KrderId_Span").html("订单号错误");
                //                    $("#KrderId_Span").css("color", "red");
                //                    $("#Btnter").removeAttr("disabled", "disabled");
                //                    return;
                //                }
                if (!reg.test(KrderM)) {
                    $("#KalesM_Span").html("使用金额错误");
                    $("#KalesM_Span").css("color", "red");
                    $("#Btnter").removeAttr("disabled", "disabled");
                    return;
                }

                $("#viewpro").hide();
                $("#member").hide();
                $("#Table2").show();

                //2013.10.28传值
                $.post("/JsonFactory/CardHandler.ashx?oper=CashCoupon", { comid: comid, KrderId: KrderId, KrderM: KrderM, Kales_admin: Kales_admin, money: money, Imprest: Imprest, Integral: Integral, card: card }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        if (msg == "预付款") {
                            $.prompt("预付款使用错误！");
                            $("#Btnter").removeAttr("disabled", "disabled");
                            return;
                        }
                        if (msg == "积分") {
                            $.prompt("积分使用错误！");
                            $("#Btnter").removeAttr("disabled", "disabled");
                            return;
                        }
                        $.prompt("操作数据出现错误");
                        $("#Btnter").removeAttr("disabled", "disabled");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == 0) {
                            $.prompt("参数传递出错，请重新操作");
                            $("#Btnter").removeAttr("disabled", "disabled");
                            return;
                        } else if (data.msg != 0) {


                            $("#Kntershow").hide();
                            $("#KrderId").val("");
                            $("#KrderM").val("");
                            $("#Kales_admin").val("");
                            //确认使用金额
                            if (money == 0) {
                                $("#Tbody1").html("");
                                $("#IIScript").tmpl().appendTo("#Tbody1");
                                $("#Imprest_span").html("预付款");
                                $("#Integral_span").html(KrderM);
                                IMPINT_Print('预付款使用验证单', '<%=account_info %>', '使用预付款', KrderM, '', '', card, '<%=DateTime.Now %>', '', '<%=printname %>', KrderId, '', '', '', Kales_admin)
                            }
                            if (money == 1) {
                                $("#Tbody1").html("");
                                $("#IIScript").tmpl().appendTo("#Tbody1");
                                $("#Imprest_span").html("积分");
                                $("#Integral_span").html(KrderM);
                                IMPINT_Print('积分使用验证单', '<%=account_info %>', '使用积分', KrderM, '', '', card, '<%=DateTime.Now %>', '', '<%=printname %>', KrderId, '', '', '', Kales_admin)
                            }

                            resets();
                            return;
                        }
                    }
                })
            })
        }
        //遍历input
        function resets() {
            var controls = document.getElementsByTagName('input');
            for (var i = 0; i < controls.length; i++) {
                if (controls[i].type == 'text') {
                    controls[i].value = '';
                }
            }

        }

        //立即使用
        function showYZ(conter, aid, actid, cardid) {

            if ($("#hid_comid").trimVal() != 101) {
                if (confirm("确认使用吗?")) {
                    var orderid = $("#OrderId").val();
                    var comid = $("#hid_comid").trimVal();
                    $("#errmsg").hide();
                    $("#tblist").html("");
                    
                    $("#viewpro").hide();
                    $("#member").hide();
                    $("#Table2").show();
                    //记录日志
                    $.post("/JsonFactory/CardHandler.ashx?oper=readuser", { Actid: actid, Cardid: cardid, OrderId: orderid, ServerName: "", Num_people: 0, Per_capita_money: 0, Member_return_money: 0, sales_admin: "", comid: comid, AccountName: "" }, function (datat) {
                        datat = eval("(" + datat + ")");
                        if (datat.type == 1) {
                            $.prompt("操作数据出现错误");
                            $("#enter").removeAttr("disabled", "disabled");
                            return;
                        }
                        if (datat.type == 100) {
                            if (datat.msg == 0) {
                                $.prompt("参数传递出错，请重新操作");
                                $("#enter").removeAttr("disabled", "disabled");
                                return;
                            } else if (datat.msg == "OK") {
                                $("#acttype").val("");
                                $("#show").hide();
                                $("#enter").removeAttr("disabled", "disabled");
                                resets();
                            }
                        }
                    })


                    $.post("/JsonFactory/CardHandler.ashx?oper=econfirmCard", { aid: aid, actid: actid, cardid: cardid, comid: $("#hid_comid").trimVal() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $("#errmsg").show().find("strong").text(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                //确认使用以后显示电子票使用情况
                                $("#tr1").show().siblings().hide();
                                $("#alast").show();

                                $("#Tbody1").html("");
                                $("#ProductItemEdit").tmpl(data.actinfo).appendTo("#Tbody1");
                                //电子票号清空
                                $("#pno2").val("");
                                $("#acctoutinfo").html("账户：" + data.idcard.toString() + " 姓名:" + data.name);
                                //打印电子凭证验证单，
                                ACT_Print('优惠活动验证单', '<%=account_info %>', data.actinfo.Title, data.actinfo.Money, '', data.agcardcode.toString(), data.idcard.toString(), '<%=DateTime.Now %>', data.actinfo.Actnum, '<%=printname %>', '0', '', '0', '0', '');
                            } else {
                                $("#errmsg").show().find("strong").text(data.msg);
                                $('#enter').show()
                                return;
                            }

                        }
                    })

                }
            } else {
                $("#show").show();
                $("#showtitile").html();
                $("#OrderId").html();
                $("#ServerName").html();
                $("#Num_people").html();
                $("#Per_capita_money").html();
                $("#Member_return_money").html();
                $("#sales_admin").html();
                $("#enter").html();
                $("#acttype").val(conter);



                //取消
                $("#cancel").click(function () {
                    $("#acttype").val("");
                    $("#show").hide();
                })

                $("#enter").click(function () {
                    $("#enter").attr("disabled", "disabled");
                    var orderid = $("#OrderId").val();
                    // var servername = $("#ServerName").val();
                    var num_people = $("#Num_people").val();
                    var per_capita_money = $("#Per_capita_money").val();
                    var menber_return_money = $("#Member_return_money").val();
                    var sales_admin = $("#sales_admin").val();
                    var province = $("#province").val();
                    //用户
                    var AccountName = $("#AccountName").val();

                    if (orderid == null || orderid == "") {
                        $("#VOrderId").html("请填写订单号");
                        $("#VOrderId").css("color", "red");
                        $("#enter").removeAttr("disabled", "disabled");
                        return;
                    }
                    else if (province == 0) {
                        $("#Vprovince").html("请选择服务项目");
                        $("#Vprovince").css("color", "red");
                        $("#enter").removeAttr("disabled", "disabled");
                        return;
                    }
                    else if (num_people == null || num_people == "") {
                        $("#VNum_people").html("请填写消费人数");
                        $("#VNum_people").css("color", "red");
                        $("#enter").removeAttr("disabled", "disabled");
                        return;
                    }
                    else if (per_capita_money == null || per_capita_money == "") {
                        $("#VPer_capita_money").html("请填写人均消费金额");
                        $("#VPer_capita_money").css("color", "red");
                        $("#enter").removeAttr("disabled", "disabled");
                        return;
                    }
                    else if (menber_return_money == null || menber_return_money == "") {
                        $("#VMember_return_money").html("请填写会员奖励返利");
                        $("#VMember_return_money").css("color", "red");
                        $("#enter").removeAttr("disabled", "disabled");
                        return;
                    }
                    else if (sales_admin == null || sales_admin == "") {
                        $("#Vsales_admin").html("请填写服务专员");
                        $("#Vsales_admin").css("color", "red");
                        $("#enter").removeAttr("disabled", "disabled");
                        return;
                    }
                    else {
                        reg = /^[-+]?\d*$/;
                        regm = /^((\d{1,3}(,\d{3})*)|(\d+))(\.\d{2})?$/;
                        regz = /^[\u0391-\uFFE5]+$/;
                        if (!reg.test(orderid)) {
                            $("#VOrderId").html("订单号错误");
                            $("#VOrderId").css("color", "red");
                            $("#enter").removeAttr("disabled", "disabled");
                            return;
                        }
                        else if (!reg.test(num_people)) {
                            $("#VNum_people").html("填写人数错误");
                            $("#VNum_people").css("color", "red");
                            $("#enter").removeAttr("disabled", "disabled");
                            return;
                        }
                        else if (!regm.test(per_capita_money)) {
                            $("#VPer_capita_money").html("人均消费金额格式错误");
                            $("#VPer_capita_money").css("color", "red");
                            $("#enter").removeAttr("disabled", "disabled");
                            return;
                        }
                        else if (!regm.test(menber_return_money)) {
                            $("#VMember_return_money").html("奖励返还金额格式错误");
                            $("#VMember_return_money").css("color", "red");
                            $("#enter").removeAttr("disabled", "disabled");
                            return;
                        }
                        else if (!regz.test(sales_admin)) {
                            $("#Vsales_admin").html("请填写中文");
                            $("#Vsales_admin").css("color", "red");
                            $("#enter").removeAttr("disabled", "disabled");
                            return;
                        }
                        else {
                            var comid = $("#hid_comid").trimVal();
                            var radio = GetRadioValue("radiobutton");
                            var servername = $("#province").val() + "," + $("#city").val() + "," + radio;

                            $("#viewpro").hide();
                            $("#member").hide();
                            $("#Table2").show();

                            $.post("/JsonFactory/CardHandler.ashx?oper=readuser", { Actid: actid, Cardid: cardid, OrderId: orderid, ServerName: servername, Num_people: num_people, Per_capita_money: per_capita_money, Member_return_money: menber_return_money, sales_admin: sales_admin, comid: comid, AccountName: AccountName }, function (datat) {
                                datat = eval("(" + datat + ")");
                                if (datat.type == 1) {
                                    $.prompt("操作数据出现错误");
                                    $("#enter").removeAttr("disabled", "disabled");
                                    return;
                                }
                                if (datat.type == 100) {
                                    if (datat.msg == 0) {
                                        $.prompt("参数传递出错，请重新操作");
                                        $("#enter").removeAttr("disabled", "disabled");
                                        return;
                                    } else if (datat.msg == "OK") {
                                        $("#acttype").val("");
                                        $("#show").hide();
                                        $("#enter").removeAttr("disabled", "disabled");
                                        resets();
                                    }
                                }
                            })

                            $("#errmsg").hide();
                            $("#tblist").html("");

                            //                            $('#enter').hide().after("<strong style='height: 36px;line-height: 35px;font-size: 16px;font-family: microsoft yahei;padding-left: 15px;padding-right: 15px;'>提交中...</strong>");
                            $.post("/JsonFactory/CardHandler.ashx?oper=econfirmCard", { aid: aid, actid: actid, cardid: cardid, comid: comid }, function (data) {
                                data = eval("(" + data + ")");
                                if (data.type == 1) {
                                    $("#errmsg").show().find("strong").text(data.msg);
                                    return;
                                }
                                if (data.type == 100) {
                                    if (data.msg == "OK") {
                                        $("#acttype").val("");
                                        $("#show").hide();
                                        $("#enter").removeAttr("disabled", "disabled");
                                        $("#OrderId").val("");
                                        $("#province").val("0");
                                        $("#city").val("0");
                                        $("#Num_people").val("");
                                        $("#Per_capita_money").val("");
                                        $("#Member_return_money").val("");
                                        $("#sales_admin").val("");



                                        //确认使用以后显示电子票使用情况
                                        $("#tr1").show().siblings().hide();
                                        $("#alast").show();

                                        $("#Tbody1").html("");
                                        $("#ProductItemEdit").tmpl(data.actinfo).appendTo("#Tbody1");
                                        //电子票号清空
                                        $("#pno2").val("");
                                        $("#acctoutinfo").html("账户：" + data.idcard.toString() + " 姓名:" + data.name);
                                        //打印电子凭证验证单，
                                        ACT_Print('优惠活动验证单', '<%=account_info %>', data.actinfo.Title, data.actinfo.Money, '', data.agcardcode.toString(), data.idcard.toString(), '<%=DateTime.Now %>', data.actinfo.Actnum, '<%=printname %>', orderid, servername, num_people, menber_return_money, sales_admin);
                                    } else {
                                        $("#errmsg").show().find("strong").text(data.msg);
                                        $('#enter').show()
                                        return;
                                    }

                                }
                            })

                        }
                    }
                })
            }
        }
    </script>
    <script language="JavaScript" type="text/javascript">
        //级联
        var city = [["云南", "海南", "河南", "山东", "广西", "内蒙古", "陕西", "湖南", "湖北", "福建", "四川", "华东", "东北", "新疆", "西藏", "江西", "贵州", "西北", "广深珠"],
                   ["东南亚", "港澳", "美洲", "韩国", "中东非", "南亚", "澳洲", "欧洲", "马尔代夫", "泰国", "日本", "北极"],
                   ["", "", "", ""]];
        function getCity() {
            var sltProvince = document.getElementById("province");
            var sltCity = document.getElementById("city");
            var provinceCity = city[sltProvince.selectedIndex - 1];
            sltCity.length = 1;
            for (var i = 0; i < provinceCity.length; i++) {
                sltCity[i + 1] = new Option(provinceCity[i], provinceCity[i]);
            }
        }
        //单选按钮
        function GetRadioValue(RadioName) {
            var obj;
            obj = document.getElementsByName(RadioName);
            if (obj != null) {
                var i;
                for (i = 0; i < obj.length; i++) {
                    if (obj[i].checked) {
                        return obj[i].value;
                    }
                }
            }
            return null;
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()"
                    target=""><span>会员卡验证</span></a></li>
                <li><a href="/v/Member_yufukuan_LogList.aspx" onfocus="this.blur()" target="">
                    <span>会员预付款验证明细</span></a></li>
                    <li><a href="/v/Member_jifen_LogList.aspx" onfocus="this.blur()" target="">
                    <span>会员积分验证明细</span></a></li>
                <li><a href="/ui/crmui/Member_Activity_LogList.aspx" onfocus="this.blur()" target="">
                    <span>活动验证明细</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    电脑验卡</h3>
                <table class="grid">
                    <tr id="tr1">
                        <td>
                            <label>
                                请输入卡号或手机号<font class="inputstyle">：</font>
                                <input name="pno" type="text" id="pno" maxlength="20" class="inputstyle" style="font-size: 20px;" />
                                &nbsp;
                            </label>
                            <label>
                                &nbsp;
                                <input type="button" id="selbtn" value="确认查询" style="width: 160px; height: 30px;" />
                            </label>
                            <div style="margin-top: 10px;">
                                <table id="member" width="780px" border="0" cellspacing="0" cellpadding="0">
                                    <tbody id="Member_info">
                                    </tbody>
                                </table>
                            </div>
                            <div style="margin-top: 10px;">
                            </div>
                            <table id="viewpro" width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tbody id="tblist">
                                </tbody>
                            </table>
                            <table id="Table2" width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tbody id="Tbody1">
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr2">
                        <td>
                            <span style="padding-top: 20px;">
                                <label>
                                    <font>会员卡号或手机号： </font>
                                    <input name="pno2" type="text" id="pno2" value="" maxlength="20" style="background-color: #EFEFEF;
                                        font-size: 20px;" class="inputstyle" readonly />
                                </label>
                                <label>
                                </label>
                                &nbsp;&nbsp;&nbsp;
                                <label>
                                    <input type="button" id="confirmbtn" value="  确认使用  " style="width: 160px; height: 25px;" />
                                </label>
                                <table id="Table1" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tbody id="ActTbody1">
                                    </tbody>
                                </table>
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <table bgcolor="#ff0000" width="760px;" class="grid" height="80px;" border="0" cellspacing="0"
        cellpadding="0" id="errmsg" style="display: none;">
        <tr>
            <td height="31">
                <strong></strong>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hid_pno" value="<%=pno %>" />
    <script type="text/x-jquery-tmpl" id="Memberinfo">   

                                  <tr>
                                    <td width="15%" height="20" align="center" bgcolor="#CCCCCC" >卡号：</td>
                                    <td width="8%" height="20" bgcolor="#CCCCCC"><p>姓名</p>                                    </td>
                                    <td width="12%" height="20" bgcolor="#CCCCCC">电话</td>
    <td width="12%" height="20" bgcolor="#CCCCCC">开卡日期</td>
                                <td width="10%" height="20" bgcolor="#CCCCCC">渠道</td>
                                <td width="19%" height="20" bgcolor="#CCCCCC">发行标题</td>
                                <td width="8%" bgcolor="#CCCCCC">预付款</td>
                                  <td width="8%" bgcolor="#CCCCCC">积分</td>
                                  <td  width="14%" bgcolor="#CCCCCC"></td>
  </tr>
  <tr>
    <td height="30" align="center" ><strong>${CardCode}</strong></td>
    <td >${Name}</td>
    <td>${Phone}</td>
    <td>${OpenSubDate}</td>
    <td>${ChannelName}</td>
    <td>${IssueTitle}</td>
    <td style="color:#ff6600"><strong>${Imprest}</strong></td>
    <td style="color:#ff6600"><strong>${Integral}</strong></td>
    <td><input type="button" id="Knter" value="使用" onclick="Javascript:showKnter('${Imprest}','${Integral}','${CardCode}')"  style="width:90px; height:25px;color:#5984bb" />
  </tr>
    </script>
    <%--{if(confirm('确认使用此劵?')){return confuse('${Aid}','${Id}','${Cardid}');}return false;}--%>
    <script type="text/x-jquery-tmpl" id="ActList">   
                                <tr >
                                    <td height="50" align="center" bgcolor="{{if Usestate=="已使用"}}#cccccc{{else}}#00CC33{{/if}}"><span class="STYLE4"><strong>${Title}</strong></span></td>
                                    <td height="50" bgcolor="{{if Usestate=="已使用"}}#cccccc {{else}}#00CC33{{/if}}"><p>金额：￥${fmoney(Money,2)}</p>                                    </td>
                                    <td bgcolor="{{if Usestate=="已使用"}}#cccccc{{else}}#00CC33{{/if}}">(有效期：${ChangeDateFormat(Actstar)} 至  ${ChangeDateFormat(Actend)})</td>
                                    <td width="10%" bgcolor="{{if Usestate=="已使用"}}#cccccc{{else}}#00CC33{{/if}}">
                                    {{if Usestate=="已使用"}}
                                     已使用
                                    {{else}}
                                     <input type="button" onclick="Javascript:showYZ(this,'${Aid}','${Id}','${Cardid}')" value=" 立即使用 " style="width: 120px; height: 25px;" />
                                    {{/if}}
                                    </td>
                                </tr>
                                 <tr >
                                    <td height="5" colspan="4" ></td>
                                </tr>
    </script>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                      <tr>
                                    <td height="48" colspan="2" align="center">
                                        <h2>
                                            <strong>${Title}</strong>使用活动确认成功！</h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="27" align="right" bgcolor="#00CC33">
                                        使用数量：
                                    </td>
                                    <td  height="27" bgcolor="#00CC33">
                                        <span class="STYLE9">${Actnum}</span> 张
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="right" bgcolor="#00CC33">
                                        抵扣金额：
                                    </td>
                                    <td height="30" bgcolor="#00CC33">
                                        ${fmoney(Money,2)} 元
                                    </td>
                                </tr>
                                
    </script>
    <script type="text/x-jquery-tmpl" id="IIScript">   
                        <tr>
                            <td height="48" align="center" bgcolor="#00CC33">
                                <h2>
                                    <strong>
                                        <span id="Imprest_span"></span>使用确认成功！
                                    </strong></h2>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="center" bgcolor="#00CC33">
                                <strong>抵扣金额：<span id="Integral_span"></span> 元</strong>
                            </td>
                        </tr>
                                
    </script>
    <%--<select name="ServerName" id="ServerName">
              <option value="国内">国内游</option>
              <option value="出境">出境游</option>
              <option value="周边">周边游</option>
              <option value="票务">票务</option>
            </select>--%>
    <div id="show" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 450px; height: 235px; display: none; left: 20%; position: absolute; top: 20%;
        line-height: 25px;">
        <dl style="padding-left: 10px; color: #5984bb">
            <dt><span style="padding-left: 10px; font-size: 18px; background: #5984bb; width: 100%;
                color: #fff" id="showtitile">请门市销售人员录入本次验卡消费对应的订单信息：</span></dt>
            <dd>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;订单编号：<input type="text" name="OrderId" id="OrderId" /><span
                    id="VOrderId"></span></dd>
            <dd>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;服务项目:
                <select name="province" id="province" onchange="getCity()">
                    <option value="0">请选择</option>
                    <option value="国内">国内</option>
                    <option value="出境">出境</option>
                    <option value="周边">周边</option>
                </select>
                <select id="city" name="city">
                    <option value="0">请选择</option>
                </select>
                <span id="Vprovince"></span>
                <div style="padding-left: 90px;">
                    <input type="radio" name="radiobutton" value="跟团旅游" checked />跟团旅游
                    <input type="radio" name="radiobutton" value="自由行" />自由行
                    <input type="radio" name="radiobutton" value="邮轮" />
                    邮轮
                    <input type="radio" name="radiobutton" value="独立成团" />独立成团
                    <input type="radio" name="radiobutton" value="自驾游" />自驾游
                </div>
            </dd>
            <dd>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;消费人数：<input type="text" name="Num_people" id="Num_people" />人<span
                    id="VNum_people"></span></dd>
            <dd>
                人均消费金额：<input type="text" name="Per_capita_money" id="Per_capita_money" />元<span
                    id="VPer_capita_money"></span></dd>
            <dd>
                会员奖励返还：<input type="text" name="Member_return_money" id="Member_return_money" />元<span
                    id="VMember_return_money">（仅用于会员下一次消费时现金抵扣）</span></dd>
            <dd>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;服务专员：<input type="text" name="sales_admin" id="sales_admin" /><span
                    id="Vsales_admin"></span></dd>
            <dd style="text-align: left; padding-left: 84px;">
                <input type="button" id="enter" value="确认提交" style="width: 90px; height: 25px; color: #5984bb" />
                <input name="cancel" id="cancel" type="button" value="  取  消  " style="width: 90px;
                    height: 25px; color: #5984bb" />
            </dd>
        </dl>
    </div>
    <div id="Kntershow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 310px; height: 160px; display: none; left: 20%; position: absolute;
        top: 20%; line-height: 25px;">
        <dl style="padding-left: 10px; color: #5984bb">
            <dt><span style="padding-left: 10px; font-size: 18px; background: #5984bb; width: 100%;
                color: #fff" id="Span1">使用预付款、积分：</span></dt>
            <dd>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;选择使用项：
                <input type="radio" name="money" value="0" />预付款
                <input type="radio" name="money" value="1" />积分 <span id="money_Span"></span>
            </dd>
            <dd>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;使用金额：<input type="text" name="KrderM" id="KrderM" /><span
                    id="KalesM_Span"></span></dd>
            <dd style="display: none;">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;订单编号：<input type="text" name="KrderId" id="KrderId" /><span
                    id="KrderId_Span"></span></dd>
            <dd style="display: none;">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;服务专员：<input type="text" name="Kales_admin" id="Kales_admin" /><span
                    id="Kales_admin_Span"></span></dd>
            <dd style="text-align: left; padding-left: 44px;">
                <input type="button" id="Btnter" value="确认提交" style="width: 70px; height: 25px; color: #5984bb" />
                <input name="cancel" id="Kancel" type="button" value="  取  消  " style="width: 70px;
                    height: 25px; color: #5984bb" />
            </dd>
        </dl>
    </div>
    <input type="hidden" id="AccountName" value="<%=AccountName %>" />
</asp:Content>
