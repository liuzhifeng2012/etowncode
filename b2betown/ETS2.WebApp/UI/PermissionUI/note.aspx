<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="note.aspx.cs" Inherits="ETS2.WebApp.UI.WebForm3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();
            var note_id = $("#note_id").trimVal();

            if (note_id != 0) {
                $.post("/JsonFactory/AccountInfo.ashx?oper=noteinfo", { comid: comid, note_id: note_id }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取信息出错，" + data.msg);
                    }
                    if (data.type == 100) {
                        $("#key").attr("readOnly", true)
                        $("#key").css("background", "#cccccc");
                        $("#key").val(data.msg[0].Sms_key);
                        $("#content").val(data.msg[0].Remark);
                        $("#title").val(data.msg[0].Title);
                        $("input:radio[name='radiobutton'][value=" + data.msg[0].Openstate + "]").attr("checked", true);
                    }
                })
            }

            $("#key").blur(function () {
                var key = $("#key").val();
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=getkey", { key: key }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#Vkey").html("key已存在");
                        $("#Vkey").css("color", "red");
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            $("#Vkey").html("√");
                            $("#Vkey").css("color", "green");
                        } else {
                            $("#Vkey").html(data.msg);
                            $("#Vkey").css("color", "red");
                            return;
                        }
                    }
                })
            })

            $("#Enter").click(function () {
                var key = $("#key").val();
                var content = $("#content").val();
                var title = $("#title").val();
                var radio = $('input:radio[name="radiobutton"]:checked').val();

                if (key == null || key == "") {
                    $("#Vkey").html("请填写分类");
                    $("#Vkey").css("color", "red");
                    return;
                }
                if (content == null || content == "") {
                    $("#Vcontent").html("请填写短信内容");
                    $("#Vcontent").css("color", "red");
                    return;
                }
                if (radio == null) {
                    $("#Vradio").html("请选择是否发送");
                    $("#Vradio").css("color", "red");
                    return;
                }

                $.post("/JsonFactory/AccountInfo.ashx?oper=enterNote", { key: key, content: content, title: title, radio: radio, note_id: note_id }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("编辑短信信息出错");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("编辑短信成功");
                        window.location.href = "notelist.aspx"
                        return;
                    }
                })
            })
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                
                <li class="on"><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>
                
                <li><a href="/ui/userui/bangdingpos.aspx" onfocus="this.blur()" target="">
                    <span>Pos绑定</span></a></li>
            </ul>
        </div> 
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    修改添加短信</h3>
                <div>
                </div>
                <table class="grid">
                    <tr>
                        <td>
                            短信分类Key
                        </td>
                        <td>
                            <input id="key" type="text" />
                            <span id="Vkey"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            短信内容
                        </td>
                        <td>
                            <textarea id="content" style="width: 200px; height: 200px;" cols="20" rows="2"></textarea><br />
                            <span>
                                 商户名$comName$，手机$phone$，姓名$name$，卡号$card$，密码$password$，钱$money$，数量1$num$，<br />
                                 $old$，开始时间$starttime$，结束时间$endtime$，随机码$code$，自定义$customtext$，短信类型$key$，标题$title$，数量2$num1$
                            </span><br />

                            <span id="Vcontent" style="color: Red">*【微旅行】(中文输入"中括号【】")<br />
                                姓名用$name$代替,电话$phone$，卡号$card$，密码$pass$，钱$money$ 剩余积分$Xsurplus$ 剩余预付款$Ysurplus$<br />
                                *预订短信(姓名$name$，电话$phone$，预订标题$card$，预订数量$pass$，金额$money$) </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            短信说明(备注)
                        </td>
                        <td>
                            <input id="title" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            是否自动发送
                        </td>
                        <td>
                            <input type="radio" name="radiobutton" value="true" />是
                            <input type="radio" name="radiobutton" value="false" />否 <span id="Vradio"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="Enter" value="确认提交" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input id="note_id" type="hidden" value="<%=note_id %>" />
</asp:Content>
