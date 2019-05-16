<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChanelrebateApply.aspx.cs"
    Inherits="ETS2.WebApp.UI.VASUI.ChanelrebateApply" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#upaccount").click(function () {
                //判断是否设置了提现账户
                $.post("/JsonFactory/PermissionHandler.ashx?oper=getchanelrebateApplyaccount", { channelid: '<%=channelid %>' }, function (data1) {
                    data1 = eval("(" + data1 + ")");
                    if (data1.type == 1) {
                        alert("请先设置提现账户");
                        location.href = "ChanelrebateApplyaccount.aspx";
                        return;
                    }
                    if (data1.type == 100) {
                        var applytype = "返佣提现";
                        var applydetail = $("#applydetail").trimVal();
                        var applymoney = $("#applymoney").trimVal();
                        if (parseFloat(applymoney) > 0) {
                            var rebatemoney = parseFloat('<%=rebatemoney %>');
                            if (rebatemoney < parseFloat(applymoney)) {
                                alert("提现金额不可大于账户余额");
                                return;
                            }
                        }
                        else {
                            alert("提现金额格式不正确");
                            return;
                        }

                        $.post("/JsonFactory/PermissionHandler.ashx?oper=channelapplyrebate", { comid: $("#hid_comid").trimVal(), channelid: '<%=channelid %>', applytype: applytype, applydetail: applydetail, applymoney: applymoney }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                alert(data.msg);
                                return;
                            }
                            if (data.type == 100) {
                                alert("申请提交成功");
                                location.href = "ChanelrebateApplylist.aspx";
                                return;
                            }
                        })
                    }
                })


            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone">
            <div id="secondary-tabs" class="navsetting ">
                <ul>
                    <li><a href="ChanelrebateApplyaccount.aspx" onfocus="this.blur()">提现账户管理</a></li>
                    <li><a href="ChanelrebateApplylist.aspx" target="" title="">提现申请列表</a></li>
                    <li class="on"><a href="ChanelrebateApply.aspx" target="" title="">提现申请</a></li>
                    <li><a href="Chanelrebatelist.aspx" target="" title="">返佣记录</a></li>
                </ul>
            </div>
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                        <td height="32">
                            可提现佣金总额：<span id="imprestinfo"><%=rebatemoney%></span>
                        </td>
                    </tr>
                </table>
                <table class="grid">
                    <tr>
                        <td class="tdHead" colspan="2">
                            提现类型:返佣提现
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            提现留言 :
                        </td>
                        <td>
                            <textarea cols="50" rows="2" class="mi-input" id="applydetail">佣金提现</textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            提现金额 :
                        </td>
                        <td>
                            <input type="text" id="applymoney" size="10" />元
                        </td>
                    </tr>
                </table>
                <table width="300px" class="grid">
                    <tr>
                        <td class="tdHead" colspan="2">
                            <input id="upaccount" type="button" value="    提交申请   " name="upaccount"> </input>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
