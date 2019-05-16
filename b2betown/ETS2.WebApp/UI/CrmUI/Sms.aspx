<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Sms.aspx.cs"
    Inherits="ETS2.WebApp.UI.CrmUI.Sms" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //首先加载数据
            var comid = $("#hid_comid").trimVal();

            //确认发送
            $("#GoSmsSend").click(function () {
                var Smsphone = $("#Smsphone").trimVal();
                var Smstext = $("#Smstext").trimVal();

                if (Smsphone.length < 9) {
                    $.prompt('发送号码不能为空或不满足手机号码规范');
                    return;
                }

                if (Smstext == '') {
                    $.prompt('发送内容不能为空！');
                    return;
                }
                if (Smstext.length > 500) {
                    $.prompt('发送内容不能超过500字！');
                    return;
                }

                var len = Smstext.length;
                var ts = parseInt(len / 65);
                if (ts == 0) ts += 1;

                if (confirm("信息内容为" + len + "字，短信将按每人" + ts + "条发送！您确定发送吗？") == false) return false;


                $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=smssend", { comid: comid, Smsphone: Smsphone, Smstext: Smstext }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        if (data.msg > 0) {
                            $.prompt("短信发送成功", {
                                buttons: [
                                 { title: '确定', value: true }
                                ],
                                opacity: 0.1,
                                focus: 0,
                                show: 'slideDown',
                                submit: function (e, v, m, f) {
                                    if (v == true) {
                                        window.location.reload();
                                    }
                                }
                            });
                            return;
                        } else {
                            $.prompt("短信发送出错");
                            return;
                        }


                    } else {
                        $.prompt("短信发送出错");
                        return;
                    }
                })

            })

        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">会员列表</a></li>
                <li><a href="/ui/crmui/BusinessCustomersList.aspx?isactivate=0" onfocus="this.blur()">
                    未激活用户列表</a></li>
                <%
                    if (IsParentCompanyUser == true)
                    {
                %>
                <li><a href="/excel/importexcel.aspx" onfocus="this.blur()">导入历史会员信息</a></li>
                <li><a href="/excel/ObtainGZList.aspx" onfocus="this.blur()">导入已有微信粉丝</a></li>

                <%} %>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    发送短信
                </h3>
                <table class="grid">
                    <tr>
                        <td class="tdHead">
                            发送号码：
                        </td>
                        <td>
                            <input id="Smsphone" cols="60" name="Smsphone" placeholder="发送号码" value="<%=Phone%>"
                                readonly>
                            <%if (Phone == "")
                              {
                            %>
                            只能对已有会员进行发送短信，请在会员列表点击 “短信”
                            <%
                                }%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            发送短信内容：
                        </td>
                        <td>
                            <textarea id="Smstext" rows="8" cols="60" name="Smstext" placeholder="发送内容"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <table border="0">
                    <tr>
                        <td width="600" height="80" align="center">
                            <input type="button" name="GoSmsSend" id="GoSmsSend" value="  确认发送  " />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>