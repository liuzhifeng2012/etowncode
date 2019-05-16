<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master"
    CodeBehind="AccountAgreement.aspx.cs" Inherits="ETS2.WebApp.UI.UserUI.AccountAgreement" %>

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
                    $("#sale_Agreement").val(data.msg.B2bcompanyinfo.Sale_Agreement);
                    $("#agent_Agreement").val(data.msg.B2bcompanyinfo.Agent_Agreement);
                    $("#hid_comextid").val(data.msg.B2bcompanyinfo.Id);
                }
            })
            $("#button").click(function () {

                var sale_Agreement = $("#sale_Agreement").trimVal();
                var agent_Agreement = $("#agent_Agreement").trimVal();
                var comextid = $("#hid_comextid").trimVal();

                if (sale_Agreement == "" || agent_Agreement == "") {
                    $.prompt("商家授权和协议信息不可为空");
                    return;
                }

                $.post("/JsonFactory/AccountInfo.ashx?oper=editcomshouquan", { sale_Agreement: sale_Agreement, agent_Agreement: agent_Agreement, comextid: comextid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("提交商家授权和协议信息出错");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("提交商家授权和协议信息成功");
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
                <li><a href="AccountApprove.aspx" onfocus="this.blur()" target="">商家资质</a></li>
                <li class="on"><a href="AccountAgreement.aspx" onfocus="this.blur()" target="">
                    授权与协议</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                   授权与协议</h3>
                <div>
                </div>
                <table class="grid">
                    <tr>
                        <td height="24" align="right">
                            授权销售协议：                        </td>
                        <td>
                        <input name="sale_Agreement" type="text" id="sale_Agreement" size="30" />                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            与易城签署的代理协议：                        </td>
                        <td>
                            <input name="agent_Agreement" type="text" id="agent_Agreement" size="30" />                        </td>
                    </tr>
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确认提交  " />                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_comextid" value="" />
    <div class="data">
    </div>
</asp:Content>
