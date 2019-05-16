<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master"
    CodeBehind="AccountApprove.aspx.cs" Inherits="ETS2.WebApp.UI.UserUI.AccountApprove" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            //获取商家扩展信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompany", { comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取信息出错，" + data.msg);
                }
                if (data.type == 100) {
                    $("#com_code").val(data.msg.B2bcompanyinfo.Com_code);
                    $("#com_sitecode").val(data.msg.B2bcompanyinfo.Com_sitecode);
                    $("#com_license").val(data.msg.B2bcompanyinfo.Com_license);
                    $("#hid_comextid").val(data.msg.B2bcompanyinfo.Id);
                }
            })
            $("#button").click(function () {

                var com_code = $("#com_code").trimVal();
                var com_sitecode = $("#com_sitecode").trimVal();
                var com_license = $("#com_license").trimVal();
                var comextid = $("#hid_comextid").trimVal();

                if (com_code == "" || com_sitecode == "" || com_license == "") {
                    $.prompt("商家资质信息不可为空");
                    return;
                }

                $.post("/JsonFactory/AccountInfo.ashx?oper=editcomzizhi", { comextid: comextid, com_code: com_code, com_sitcode: com_sitecode, com_license: com_license }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("提交商家资质信息出错");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("提交商家资质信息成功");
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
                <li><a href="AccountInfo.aspx" target="" title="">商家基本信息</a></li>
                <li class="on"><a href="AccountApprove.aspx" onfocus="this.blur()" target="">
                    商家资质</a></li>
                <%--<li><a href="AccountAgreement.aspx" onfocus="this.blur()" target="">授权与协议</a></li>--%>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                   商家资质</h3>
                <div>
                </div>
                <table class="grid">
                    <tr>
                        <td height="24" align="right">
                            公司机构代码证：                        </td>
                        <td>
                            <label>
                                <input name="com_code" type="text" id="com_code" />
                            </label>                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            网站备案信息号：                        </td>
                        <td>
                            <input name="com_sitecode" type="text" id="com_sitecode" />                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            营业执照Flie：                        </td>
                        <td>
                        <input name="com_license" type="text" id="com_license" />                        </td>
                    </tr>
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="submit" name="button" id="button" value="  确认提交  " />                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
        <input type="hidden" id="hid_comextid" value="" />
    </div>
</asp:Content>
