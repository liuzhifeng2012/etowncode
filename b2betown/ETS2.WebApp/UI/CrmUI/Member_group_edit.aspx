<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Member_group_edit.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.CrmUI.Member_group_edit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var groupid = $("#hid_groupid").val();
            if (groupid != "0") {//编辑
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetB2bGroupById", { id: groupid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                    }
                    if (data.type == 100) {
                        $("#txtgroupname").val(data.msg.Groupname);

                    }
                })
            }

            $("#Button1").click(function () {
                var id = $("#hid_groupid").val();
                var name = $("#txtgroupname").val();

                if (name == "") {
                    $.prompt("名称不可为空");
                    return;
                }

                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=EditB2bGroup", { comid: $("#hid_comid").val(), userid: $("#hid_userid").val(), id: id, name: name }, function (data) {
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
                                    location.href = "Member_group_list.aspx";
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
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/PermissionUI/Member_group_list.aspx" onfocus="this.blur()"><span>分组设置</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list mail-list1">
            <div class="inner">
                <h3>
                    <label>
                        分组添加</label>
                </h3>
                <table>
                    <tr>
                        <td>
                            分组名称:
                        </td>
                        <td>
                            <input id="txtgroupname" type="text" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input id="Button1" type="button" value="编 辑" />
                            <input type="hidden" id="hid_groupid" value="<%=groupid %>" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
