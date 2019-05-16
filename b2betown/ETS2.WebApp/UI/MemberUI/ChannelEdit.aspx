<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ChannelEdit.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.ChannelEdit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();
            //            $('input[name="radIssuetype"][value="1"]').attr("checked", true);
            var channelid = $("#hid_channelid").trimVal();

            var channeltype = $("#hid_channeltype").val();
            if (channelid != "0") {
                //判断渠道是否存在，存在显示详细；否则为空
                $.post("/JsonFactory/ChannelHandler.ashx?oper=getchanneldetailnew", { channelid: channelid, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取渠道信息出错");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == null) {
                            window.open("/ui/MemberUI/Channelstatistics.aspx");
                        }
                        else {
                            $('input[name="radIssuetype"][value=' + data.msg.Issuetype + ']').attr("checked", true);
                            //                        //根据得到的渠道类型得到渠道单位列表
                            //                        ShowUnitList(data.msg.Issuetype, data.msg.Companyid);

                            if (data.msg.Issuetype == "0") {
                                $("#lblissuetype").html("内部渠道");
                                $("#hid_channeltype").val("0");
                            } else {
                                $("#lblissuetype").html("外部渠道");
                                $("#hid_channeltype").val("1");
                            }

                            //根据渠道公司id得到渠道公司信息
                            $.post("/JsonFactory/ChannelHandler.ashx?oper=getchannelcompanyname", { comid: comid, channelcompanyid: data.msg.Companyid }, function (dataaa) {
                                dataaa = eval("(" + dataaa + ")");
                                if (dataaa.type == 1) {
                                    $("#selcompanyid").empty();
                                }
                                else {
                                    $("#selcompanyid").empty();
                                    $("#selcompanyid").append("<option value='" + data.msg.Companyid + "' selected='selected'>" + dataaa.msg + "</option>");
                                }
                            })

                            $("#txtname").val(data.msg.Name);
                            $("#txtmobile").val(data.msg.Mobile);
                            $("#txtcardcode").val(data.msg.Cardcode);
                            $("#txtchaddress").val(data.msg.Chaddress);
                            $("#txtchobjects").val(data.msg.ChObjects);
                            $("#txtrebateopen").val(data.msg.RebateOpen);
                            $("#txtrebateconsume").val(data.msg.RebateConsume);
                            $('input[name="radrebatelevel"][value=' + data.msg.RebateConsume2 + ']').attr("checked", true);

                            $("#opencardnum").val(data.msg.Opencardnum);
                            $("#firstdealnum").val(data.msg.Firstdealnum);
                            $("#summoney").val(data.msg.Summoney);
                            $("#hid_whetherdefaultchannel").val(data.msg.Whetherdefaultchannel);
                            $("#selrunstate").val(data.msg.Runstate);
                        }
                    }
                })

            } else {
                $("#hid_whetherdefaultchannel").val("0");
                ShowUnitList(channeltype, '');
            }

            //编辑渠道记录
            $("#aedit").click(function () {
                //                var Issuetype = $('input:radio[name="radIssuetype"]:checked').trimVal(); //  0 内部，   1 外部
                var Issuetype = $("#hid_channeltype").val();
                var companyid = $("#selcompanyid").val();
                if (companyid == null) {
                    $.prompt("渠道单位不可为空");
                    return;
                }
                var name = $("#txtname").trimVal();
                var mobile = $("#txtmobile").trimVal();
                var cardcode = $("#txtcardcode").trimVal();
                var chadress = $("#txtchaddress").trimVal();
                var chobjects = $("#txtchobjects").trimVal();
                var rebateopen = $("#txtrebateopen").trimVal();
                var rebateconsume = $("#txtrebateconsume").trimVal();
                var rebateleval = $('input:radio["name=radrebatelevel"]:checked').trimVal();
                if (Issuetype == "" || companyid == "" || name == "" || mobile == "" || chadress == "" || chobjects == "" || rebateopen == "" || rebateconsume == "" || rebateleval == "") {
                    $.prompt("请补充全部资料");
                    return;
                }
                else {
                    $.post("/JsonFactory/ChannelHandler.ashx?oper=editchannel", { runstate: $("#selrunstate").val(), whetherdefaultchannel: $("#hid_whetherdefaultchannel").val(), id: channelid, comid: $("#hid_comid").trimVal(), Issuetype: Issuetype, companyid: companyid, name: name, mobile: mobile, cardcode: cardcode, chadress: chadress, chobjects: chobjects, rebateopen: rebateopen, rebateconsume: rebateconsume, rebateleval: rebateleval, opencardnum: $("#opencardnum").trimVal(), firstdealnum: $("#firstdealnum").trimVal(), summoney: $("#summoney").trimVal() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("编辑渠道信息出错");
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("编辑渠道信息成功",
                                {
                                    buttons: [{ title: "确定", value: true}],
                                    show: "slideDown",
                                    submit: function (e, v, m, f) {
                                        if (v == true) {
                                            //  window.open("Channelstatistics.aspx", target = "_self");

                                            window.open("/ui/memberui/channelstatistics.aspx", target = "_self");
                                        }
                                    }
                                });
                            return;
                        }
                    });
                }
            });
            //            //给渠道类型绑定点击事件
            //            $('input[name="radIssuetype"]').bind("click", function () {
            //                var issuetype = $('input:radio[name="radIssuetype"]:checked').trimVal();
            //                ShowUnitList(issuetype);
            //            })


            function ShowUnitList(issuetype, companyid) {
                if (issuetype == "0") {
                    $("#lblissuetype").html("内部渠道");
                    $("#hid_channeltype").val("0");
                } else {
                    $("#lblissuetype").html("外部渠道");
                    $("#hid_channeltype").val("1");
                }

                $("#selcompanyid").empty();
                //添加外部或者内部渠道单位列表
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=GetChannelCompanyList2", {
                    channelcompanyid: $("#hid_channelcompanyid").trimVal(), comid: $("#hid_comid").trimVal(), issuetype: issuetype, companystate: "1", whetherdepartment: "0,1"
                }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        //                        $.prompt("获取渠道单位列表出现问题");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == null || data.msg == "") {

                        }
                        else {
                            for (var i = 0; i < data.msg.length; i++) {

                                if (data.msg[i].Id == companyid) {
                                    $("#selcompanyid").append("<option value='" + data.msg[i].Id + "' selected='selected'>" + data.msg[i].Companyname + "</option>");
                                }
                                else {
                                    $("#selcompanyid").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Companyname + "</option>");
                                }
                            }

                        }
                    }
                })
            }




            //            $("#cancel_rh").click(function () {

            //                $("#rhshow").hide();
            //            })

            //            $("#submit_rh").click(function () {

            //                var companyname = $("#companyname").trimVal();
            //                if (companyname == "") {
            //                    $.prompt("渠道单位名称不可为空");
            //                    return;
            //                }

            //                var Issuetype = $('input:radio[name="radIssuetype"]:checked').trimVal(); //  false 内部，   true 外部

            //                $.post("/JsonFactory/ChannelHandler.ashx?oper=editchannelcompany", { issuetype: Issuetype, comid: comid, companyname: companyname }, function (data) {
            //                    data = eval("(" + data + ")");
            //                    if (data.type == 1) {
            //                        $.prompt("添加新渠道单位出现错误");
            //                        return;
            //                    }
            //                    if (data.type == 100) {
            //                        $.prompt("添加新渠道单位成功", {
            //                            buttons: [{ title: '确定', value: true}],
            //                            opacity: 0.1,
            //                            focus: 0,
            //                            show: 'slideDown',
            //                            submit: callbackfunc
            //                        });
            //                    }
            //                })
            //                function callbackfunc(e, v, m, f) {
            //                    if (v == true)
            //                        window.location.reload();
            //                }

            //            })
        })
        //        //添加新渠道单位
        //        function referrer_ch() {
        //            $("#span_rh").text("添加新渠道公司");
        //            $("#rhshow").show();
        //        };
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%if (IsParentCompanyUser)
                  { %>
                <li id="addoutchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=out"
                    onfocus="this.blur()">合作单位</a></li>
                <li id="addoutchannelcompany"><a href="ChannelCompanyEdit.aspx?channeltype=out" onfocus="this.blur()">
                    添加合作单位</a></li>
                <li id="addinnerchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=inner"
                    onfocus="this.blur()">所属门店 </a></li>
                <li id="addinnerchannelcompany"><a href="ChannelCompanyEdit.aspx?channeltype=inner"
                    onfocus="this.blur()"><span>添加门店</span></a></li>
                <%}
                  else
                  { 
                %>
                <li id="addinnerchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=inner"
                    onfocus="this.blur()">所属门店 </a></li>
                <%
                    } %>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    添加实体卡发行渠道</h3>
                <h3>
                    &nbsp;</h3>
                <table class="grid">
                    <tr>
                        <td width="19%" class="tdHead">
                            渠道类型：
                        </td>
                        <td width="81%">
                            <%-- <input name="radIssuetype" type="radio" value="1" checked>
                            外部发行渠道
                            <input name="radIssuetype" type="radio" value="0">
                            内部发行渠道--%>
                            <label id="lblissuetype">
                            </label>
                            <input type="hidden" id="hid_channeltype" value="<%=channeltype %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            渠道单位名称：
                        </td>
                        <td width="81%">
                            <select id="selcompanyid">
                            </select>
                            <%-- <a href="javascript:void(0)" onclick='referrer_ch()'>添加渠道单位 </a>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            发行人姓名：
                        </td>
                        <td>
                            <input type="text" id="txtname" value="" size="20" readonly="readonly">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            发行人手机：
                        </td>
                        <td>
                            <input type="text" id="txtmobile" value="" size="20" readonly="readonly">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            发行人账户：
                        </td>
                        <td>
                            <input type="text" id="txtcardcode" value="0" />
                            （填写该渠道发行人实体卡卡号，登录渠道会员账户）
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            发行地点：
                        </td>
                        <td>
                            <input type="text" id="txtchaddress" value="北京" size="50">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            发行对象：
                        </td>
                        <td>
                            <input type="text" id="txtchobjects" value="客户" size="20">
                            （发卡人群描述）
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            返佣办法：
                        </td>
                        <td>
                            有效开卡奖励
                            <input type="text" id="txtrebateopen" value="0" size="10">
                            元 第一次成交奖励
                            <input type="text" id="txtrebateconsume" value="0" size="10">
                            元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            渠道级别：
                        </td>
                        <td>
                            <input type="radio" name="radrebatelevel" value="radiobutton" checked>
                            普通渠道
                            <%--   <input type="radio" name="radrebatelevel" value="radiobutton">
                            高级渠道 （高级渠道具有卡开用户管理功能）--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            开卡数量：
                        </td>
                        <td>
                            <input type="text" id="opencardnum" value="0" size="10" readonly="readonly">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            第一次交易数量：
                        </td>
                        <td>
                            <input type="text" id="firstdealnum" value="0" size="10" readonly="readonly">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            渠道人返佣金额：
                        </td>
                        <td>
                            <input type="text" id="summoney" value="0" size="10" readonly="readonly">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            运行状态：
                        </td>
                        <td>
                            <select id="selrunstate">
                                <option value="1">运行</option>
                                <option value="0">暂停</option>
                            </select>
                        </td>
                    </tr>
                </table>
                <p>
                    &nbsp;</p>
                <p>
                    &nbsp;</p>
                <p align="center">
                    <a href="javascript:void(0)" id="aedit" class="font_14"><strong>完成添加，确认提交</strong></a></p>
                <p>
                    &nbsp;</p>
                <h3>
                    &nbsp;</h3>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_channelid" value="<%=channelid %>" />
    <input type="hidden" id="hid_channelcompanyid" value="<%=channelcompanyid %>" />
    <input type="hidden" id="hid_whetherdefaultchannel" value="" />
    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="span_rh"></span>
                </td>
            </tr>
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                    渠道名:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" id="tdgroups">
                    <input type="text" tabindex="1" class="txt" size="36" name="companyname" id="companyname" />
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="submit_rh" id="submit_rh" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" value="  取  消  " />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
