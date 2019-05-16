<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ETS2.WebApp.UI.QunarUI._default"
    MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $.post("/JsonFactory/AccountInfo.ashx?oper=Getqunarbycomid", { comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    $("input:radio[name='isqunar'][value='" + data.msg.isqunar + "']").attr("checked", "checked");
                    $("#qunar_username").val(data.msg.qunar_username);
                    $("#qunar_pass").val(data.msg.qunar_pass);
                }
            })

            $("#button").click(function () {
                var isqunar = $("input:radio[name='isqunar']:checked").trimVal();
                var qunar_username = $("#qunar_username").trimVal();
                var qunar_pass = $("#qunar_pass").trimVal();
                if (isqunar == 1) {
                    if (qunar_username == "") {
                        alert("接口用户名不可为空");
                        return;

                    }
                    if (qunar_pass == "") {
                        alert("接口密码不可为空");
                        return;
                    }
                }
                $.post("/JsonFactory/AccountInfo.ashx?oper=Editqunarbycomid", { isqunar: isqunar, qunar_username: qunar_username, qunar_pass: qunar_pass, comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        alert("去哪接口设置完成");
                        return;
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
                   <li ><a href="/ui/pmui/order/orderlist.aspx" onfocus="this.blur()" target="">
                    订单列表</a></li>
               <li ><a href="/ui/pmui/order/ordercount.aspx" onfocus="this.blur()" target="">
                    订单统计</a></li>
                
                  <li  class="on"><a href="/ui/qunarui" onfocus="this.blur()" target="">
                    开通去哪儿</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        去哪接口设置</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            是否开通去哪</label>
                        <label>
                            <input type="radio" name="isqunar" value="1" />开通</label>
                        <label>
                            <input type="radio" name="isqunar" value="0" checked/>关闭</label>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            接口用户名</label>
                        <input type="text" id="qunar_username" value="" class="mi-input" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            接口密码</label>
                        <input type="password" id="qunar_pass" value="" class="mi-input" />*
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table border="0" width="780" class="grid">
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确认提交  " class="mi-input" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
