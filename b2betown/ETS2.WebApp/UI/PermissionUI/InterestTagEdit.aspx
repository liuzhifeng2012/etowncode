<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterestTagEdit.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PermissionUI.InterestTagEdit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var tagid = $("#hid_tagid").val();
            if (tagid != "0") {//编辑
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetTagById", { id: tagid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                    }
                    if (data.type == 100) {
                        $("#txttagname").val(data.msg.TagName);

                    }
                })
            }

            $("#Button1").click(function () {
                var id = $("#hid_tagid").val();
                var name = $("#txttagname").val();
                var tagtypeid = $("#hid_tagtypeid").val();
                if (name == "") {
                    $.prompt("标签名称不可为空");
                    return;
                }

                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=EditTag", { id: id, name: name, tagtypeid: tagtypeid, issystemadd: 1, comid: $("#hid_comid").trimVal() }, function (data) {
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
                                    location.href = "interesttaglist.aspx?tagtypeid=" + $("#hid_tagtypeid").trimVal();
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
                <li><a href="/ui/PermissionUI/interesttaglist.aspx" onfocus="this.blur()"><span>兴趣标签设置</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list mail-list1">
            <div class="inner">
                <h3>
                    <label>
                        兴趣标签添加</label>
                </h3>
                <table>
                    <tr>
                        <td>
                            标签名称:
                        </td>
                        <td>
                            <input id="txttagname" type="text" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input id="Button1" type="button" value="编 辑" />
                            <input type="hidden" id="hid_tagid" value="<%=tagid %>" />
                            <input type="hidden" id="hid_tagtypeid" value="<%=tagtypeid %>" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
