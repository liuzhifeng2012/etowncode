<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master"
    CodeBehind="bangdingpos_add.aspx.cs" Inherits="ETS2.WebApp.UI.UserUI.bangdingpos_add" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            //获取商家绑定打印机信息
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var pos_id = $("#hid_pos_id").trimVal();

            if (pos_id != 0) {
                //获取pos信息
                $.post("/JsonFactory/AccountInfo.ashx?oper=posinfo", { pos_id: $("#hid_pos_id").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取信息出错，" + data.msg);
                    }
                    if (data.type == 100) {
                        SetCompanySelect(data.msg[0].Com_id);
                        $("#poscompany").val(data.msg[0].Com_id);
                        $("#posid").val(data.msg[0].Posid);

                        //测试posid，不允许修改
                        if (data.msg[0].Posid == "999999999") {
                            $("#button").hide();
                            $("#lblmsg").text("测试posid，不允许修改");
                        }

                        $("#Remark").val(data.msg[0].Remark);
                        $("#txt_md5key").val(data.msg[0].md5key);
                        SetProjcetidSelect(data.msg[0].Projectid, data.msg[0].Com_id);
                    }
                })
                
            } else {
                SetCompanySelect(0);
                SetProjcetidSelect(0, 0);
            }


            $("#button").click(function () {

                var posid = $("#posid").trimVal();
                var poscompany = $("#poscompany").trimVal();
                var Remark = $("#Remark").trimVal();
                var projectid = $("#projectid").trimVal();
                var md5key = $("#txt_md5key").trimVal();

                $.post("/JsonFactory/AccountInfo.ashx?oper=editbangpos", { userid: userid, comid: $("#poscompany").val(), projectid: projectid, posid: posid, poscompany: $("#poscompany").find("option:selected").text(), Remark: Remark, pos_id: pos_id, md5key: md5key }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("绑定POS信息出错");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("绑定POS成功", { buttons: [{ title: "确定", value: true}], submit: function (e, v, m, f) { if (v) { window.open("bangdingpos.aspx", target = "_self") } } });

                        return;
                    }
                })

            })
        })
        function SetCompanySelect(seledval) {
            $.post("/JsonFactory/AccountInfo.ashx?oper=GetAllCompany", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {

                }
                if (data.type == 100) {
                    if (data.totalcount == 0) {

                    } else {
                        $("#poscompany").empty();
                        var str = "";
                        for (var i = 0; i < data.msg.length; i++) {
                            if (data.msg[i].ID == seledval) {
                                str += '<option value="' + data.msg[i].ID + '" selected="selected">' + data.msg[i].ID + data.msg[i].Com_name + '</option>';
                            } else {
                                str += '<option value="' + data.msg[i].ID + '">' + data.msg[i].ID + data.msg[i].Com_name + '</option>';
                            }
                        }
                        $("#poscompany").append(str);
                    }
                }
            })
        }

        //指定项目
        function SetProjcetidSelect(pjectid, Com_id) {

            var comid_temp = $("#poscompany").val();
            if (Com_id != 0) {
                comid_temp = Com_id;
            }
            if (comid_temp != 0) {

                $.post("/JsonFactory/ProductHandler.ashx?oper=projectlist", { comid: comid_temp, projectstate: 1 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {

                    }
                    if (data.type == 100) {

                        $("#projectid").empty();
                        var str = "";
                        str = '<option value="0" selected="selected">不指定,通用验证</option>'
                        for (var i = 0; i < data.msg.length; i++) {
                            if (data.msg[i].Id == pjectid) {
                                str += '<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Projectname + '</option>';
                            } else {
                                str += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Projectname + '</option>';
                            }
                        }
                        $("#projectid").append(str);
                    }
                })
            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/PermissionUI/notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>
                <li class="on"><a href="/ui/userui/bangdingpos.aspx" onfocus="this.blur()" target="">
                    <span>Pos绑定</span></a></li>
            </ul>
        </div> 
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    Pos终端绑定</h3>
                <div>
                </div>
                <table class="grid">
                    <tr>
                        <td height="24" align="right">
                            POS ID：
                        </td>
                        <td>
                            <input name="posid" type="text" id="posid" size="24" />
                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            pos商家：
                        </td>
                        <td>
                            <%--<input name="poscompany" type="text" id="poscompany" size="24" />--%>
                            <select id="poscompany" onchange="SetProjcetidSelect(0,0);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            指定项目：
                        </td>
                        <td>
                            <%--<input name="poscompany" type="text" id="poscompany" size="24" />--%>
                            <select id="projectid">
                                <option value="0" selected>不指定，通用项目</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            备注：
                        </td>
                        <td>
                            <input name="Remark" type="text" id="Remark" size="50" />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td height="24" align="right">
                            md5秘钥：
                        </td>
                        <td>
                            <input type="text" id="txt_md5key" value="<%=md5key %>" size="50" readonly="readonly" />
                        </td>
                    </tr>
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确认绑定  " />
                            <label id="lblmsg">
                            </label>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_pos_id" value="<%=pos_id %>" />
    <input type="hidden" id="hid_md5key" value="<%=md5key %>" />
    <div class="data">
    </div>
</asp:Content>
