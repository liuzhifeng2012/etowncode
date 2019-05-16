<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyIndustryEdit.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PermissionUI.CompanyIndustryEdit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var industryid = $("#hid_industryid").val();
            if (industryid != "0") {//编辑
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetIndustryById", { id: industryid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                    }
                    if (data.type == 100) {
                        $("#txtindustryname").val(data.msg.Industryname);
                        $("#txtindustryremark").val(data.msg.Remark);
                    }
                })
            }

            $("#Button1").click(function () {
                var id = $("#hid_industryid").val();
                var name = $("#txtindustryname").val();
                var remark = $("#txtindustryremark").val();
                if (name == "") {
                    $.prompt("行业名称不可为空");
                    return;
                }

                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=EditComIndustry", { id: id, name: name, remark: remark }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("编辑失败");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("编辑成功",
                        {
                            buttons: [
                                 { title: '确定', value: true }
                                ],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "interesttagtypelist.aspx";
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
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/PermissionUI/interesttagtypelist.aspx" onfocus="this.blur()"><span>
                    商户行业设置</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list mail-list1">
            <div class="inner">
                <h3>
                    <label>
                        公司行业添加</label>
                </h3>
                <table>
                    <tr>
                        <td>
                            行业名称:
                        </td>
                        <td>
                            <input id="txtindustryname" type="text" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            备注:
                        </td>
                        <td>
                            <textarea id="txtindustryremark" cols="20" rows="2"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input id="Button1" type="button" value="编 辑" />
                            <input type="hidden" id="hid_industryid" value="<%=industryid %>" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
