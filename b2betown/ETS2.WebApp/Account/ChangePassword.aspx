<%@ Page Title="更改密码" Language="C#" MasterPageFile="/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="ETS2.WebApp.Account.ChangePassword" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#button").click(function () {
                var oldpwd = $("#oldpwd").trimVal();
                var pwd1 = $("#pwd1").trimVal();
                var pwd2 = $("#pwd2").trimVal();
                var userid = $("#hid_userid").trimVal();

                var staffid = $("#hid_staffid").trimVal();
                if (staffid != "0") {
                    userid = staffid;
                }

                if (oldpwd == "" || pwd1 == "" || pwd2 == "") {
                    $.prompt("填写内容不可为空");
                    return;
                }
                if (pwd1 != pwd2) {
                    $.prompt("原密码与确认密码不相符");
                    return;
                }
                $.post("/JsonFactory/UserHandle.ashx?oper=changepwd", { userid: userid, oldpwd: oldpwd, pwd1: pwd1 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("修改密码失败" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("修改密码成功", {
                            buttons: [
                                { title: 'OK', value: true }

                                     ],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: callbackfunc
                        });
                    }
                })
            })
            function callbackfunc(e, v, m, f) {
                if (v == true)
                    location.href = "/default.aspx";
            }

        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/Account/ChangePassword.aspx" target="" title="">修改登陆密码</a></li>
                <li><a href="/ui/userui/bangdingprint.aspx" onfocus="this.blur()" target="">
                    小票打印机绑定</a></li>
                <li><a href="/ui/userui/bangdingpos.aspx" onfocus="this.blur()" target="">
                    Pos终端绑定</a></li>
                <li><a href="/ui/userui/notelist.aspx" onfocus="this.blur()" target="">短信设置</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    修改登陆密码</h3>
                <div>
                </div>
                <table class="grid">
                    <tr>
                        <td height="24" align="right">
                            原密码：                        </td>
                        <td>
                            <input name="oldpwd" type="password" id="oldpwd" size="12" />                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            密码：                        </td>
                        <td>
                            <input name="pwd1" type="password" id="pwd1" size="12" />                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            确认密码：                        </td>
                        <td>
                            <input name="pwd2" type="password" id="pwd2" size="12" />                        </td>
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
    <div class="data">
    </div>
    <input type="hidden" id="hid_staffid" value="<%=staffid %>" />
</asp:Content>
