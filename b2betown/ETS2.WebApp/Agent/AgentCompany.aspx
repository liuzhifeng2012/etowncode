<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"
    CodeBehind="AgentCompany.aspx.cs" Inherits="ETS2.WebApp.Agent.AgentCompany" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var proid = $("#hid_proid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    //                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=getagentinfo",
                    data: { agentid: agentid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#Company").text(data.msg.Company);
                            $("#name").val(data.msg.Contentname);
                            $("#tel").val(data.msg.Tel);
                            $("#phone").val(data.msg.Mobile);
                            $("#address").val(data.msg.Address);
                            $("#agentdomain").val(data.msg.Agent_domain);

                            $("#Span_company").html(data.msg.Company);
                            $("#hid_edit_agentid").val($("#hid_agentid").trimVal());

//                            //开通淘宝码商
//                            if (data.msg.istaobao == 1) {
//                                $("#lbl_istaobao").text("已开通");
//                            } else {
//                                $("#lbl_istaobao").text("未开通，如需开通请点击后方按钮");
//                                $("#btn_opentb").show();
//                            }

//                            $("#lbl_tb_syncurl").text(data.msg.tb_syncurl);

//                            if (data.msg.tb_isret_consumeresult == 1) {
//                                $("#lbl_tb_isret_consumeresult").text("返回");
//                            } else {
//                                $("#lbl_tb_isret_consumeresult").text("不返回");
//                            }

//                            $("#lbl_tb_seller_id").text(data.msg.tb_seller_id);

//                            $("#lbl_tb_seller_nick").text(data.msg.tb_seller_nick);

                        }
                    }
                })


            }

            $("#confirmButton").click(function () {
                var name = $("#name").trimVal();
                var tel = $("#tel").trimVal();
                var phone = $("#phone").trimVal();
                var address = $("#address").trimVal();
                var agentdomain = $("#agentdomain").trimVal();

                if (name == "") {
                    $.prompt("请填写姓名");
                    return;
                }
                if (phone == "") {
                    $.prompt("请填写手机号");
                    return;
                } else {
                    //                    if (!isMobel(phone)) {
                    //                        $.prompt("请正确填写手机号");
                    //                        return;
                    //                    }
                }

                //开通淘宝码商
                var istaobao = $("input:radio[name='istaobao']:checked").trimVal();
                var tb_syncurl = $("#tb_syncurl").trimVal();
                var tb_isret_consumeresult = $("input:radio[name='tb_isret_consumeresult']:checked").trimVal();
                var tb_seller_id = $("#tb_seller_id").trimVal();
                var tb_seller_nick = $("#tb_seller_nick").trimVal();

                $.post("/JsonFactory/AgentHandler.ashx?oper=AgentUp", { agentid: agentid, name: name, tel: tel, phone: phone, address: address, agentdomain: agentdomain }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("失败，请刷新后重新操作");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("修改成功");
                        return;
                    }
                })

            })

            //获取手机验证码
            $("#getphonecode").click(function () {

                if ($.trim($("#newphone").val()) == "") {
                    alert("请输入手机号码!");
                    return;
                }
                if (!checkMobile($("#newphone").val())) {
                    alert("请正确输入手机号!");
                    return;
                }

                if ($.trim($("#getphonecode").html()) == "获取短信验证码") {
                    $("#getphonecode").attr("disabled", "disabled").css("background-color", "#f4f4f4");
                    _callSmsApi();

                }
            })

            //提交手机更改
            $("#sub_rh").click(function () {
                var phonecode = $("#phonecode").val();
                if (phonecode == "") {
                    alert("请填写短信验证码");
                    return;
                }
                //判断验证码输入是否正确
                $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms", { mobile:

$("#newphone").val(), smscode: $("#phonecode").val(), source: "分销商商户管理验证码"
                },

                function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert("验证码不相符");
                        return;
                    }
                    if (data.type == 100) {
                        //更改分销商户联系手机号
                        $.post("/JsonFactory/AgentHandler.ashx?oper=AgentUpPhone", { agentid: $("#hid_agentid").trimVal(), newphone: $("#newphone").val() }, function (data1) {
                            data1 = eval("(" + data1 + ")");
                            if (data1.type == 1) {
                                //$.prompt("失败，请刷新后重新操作");
                                return;
                            }
                            if (data1.type == 100) {
                                //$.prompt("修改成功");
                                location.reload();
                                //return;
                            }
                        })
                    }
                })
            })
            //取消手机更改
            $("#cancel_rh").click(function () {
                $("#phonecode").val("");
                $("#div_showhangye").hide();

            })

            $("#btn_opentb").click(function () {
                window.open("Agent_OpenTaobao_Manage.aspx",target="_self");
            })
        })
        function newphone_set() {
            $("#div_showhangye").show();
        }
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
            $.post("/JsonFactory/AgentHandler.ashx?oper=callvalidsms", { mobile: $("#newphone").val(), comid: 0, source: "分销商商户管理验证码" }, function (data) {
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
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="AgentStaff.aspx">员工管理</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="AgentCompany.aspx">分销商信息</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;"  id="taobaoset">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Agent_OpenTaobao.aspx">淘宝店铺</a></div></div>
            </li>
         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right searchtool">
                 <span>&nbsp;</span>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
       
         
         </div></div>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    商户信息管理</h3>
                <table width="700px" class="grid">
                    <tr>
                        <td class="tdHead" style="width: 200px;">
                            公司名称 :
                        </td>
                        <td>
                            <h3 class="Company" id="Company">
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            绑定域名 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="agentdomain" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            联系人姓名 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="name" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            联系电话 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="tel" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            联系手机:
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="phone" value="" readonly="readonly" /><a
                                style="text-decoration: underline; cursor: pointer;" onclick="newphone_set()">更换新手机</a>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            地址 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="address" value="" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            是否开通淘宝 :
                        </td>
                        <td>
                            <label id="lbl_istaobao">
                            </label>
                            <input type="button" id="btn_opentb" value="申请开通" style="display: none;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            同步url地址 :
                        </td>
                        <td>
                            <label id="lbl_tb_syncurl">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            是否返回使用报告 :
                        </td>
                        <td>
                            <label id="lbl_tb_isret_consumeresult">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            淘宝卖家seller_id :
                        </td>
                        <td>
                            <label id="lbl_tb_seller_id">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            淘宝卖家用户名 :
                        </td>
                        <td>
                            <label id="lbl_tb_seller_nick">
                            </label>
                        </td>
                    </tr>--%>
                </table>
                <table width="300px" class="grid">
                    <tr>
                        <td class="tdHead">
                            <input id="confirmButton" type="button" value="    修改公司信息   " name="confirmButton"></input>
                        </td>
                    </tr>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <div id="div_showhangye" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span1">更换新手机
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    公司名称：<span id="Span_company"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    新手机号码 :
                    <input name="Input" class="dataNum dataIcon" id="newphone" value="" />
                    <a id="getphonecode" style="text-decoration: underline; cursor: pointer;">获取短信验证码</a>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    短信验证码 :
                    <input name="Input" class="dataNum dataIcon" id="phonecode" value="" />
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input type="hidden" id="hid_edit_agentid" value="0" />
                    <input id="sub_rh" type="button" class="formButton" value="  提  交  " />
                    <input id="cancel_rh" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>
