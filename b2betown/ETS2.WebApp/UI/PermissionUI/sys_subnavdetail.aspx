<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_subnavdetail.aspx.cs"
    Inherits="ETS2.WebApp.UI.PermissionUI.sys_subnavdetail" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var columnid = $("#hid_columnid").trimVal();
            var actionid = $("#hid_actionid").trimVal();

            var subnavid = $("#hid_sys_subnavid").trimVal();

            //            if (columnid == 0 && actionid == 0) {
            //                $("#lbl_columns").parent().hide();
            //                $("#sel_actionid").parent().hide();
            //            } 


            if (subnavid == "" || subnavid == 0) {

                //得到权限分栏表
                Getactioncloumns(columnid);
                //得到权限表
                GetActions(columnid, actionid);

            } else {//编辑右侧子导航
                $.post("/JsonFactory/PermissionHandler.ashx?oper=Getsyssubnav", { subnavid: subnavid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        $("#subnavname").val(data.msg.subnav_name);
                        $("#subnavurl").val(data.msg.subnav_url);
                        //得到权限分栏表
                        Getactioncloumns(data.msg.actioncolumnid);
                        //得到权限表
                        GetActions(data.msg.actioncolumnid, data.msg.actionid);

                    }
                })
            }

            $("#confirmpubb").click(function () {
                var subnavname = $("#subnavname").trimVal();
                var subnavurl = $("#subnavurl").trimVal();
                var columnid = $("input:radio[name='radiobutton']:checked").trimVal();
                var actionid = $("#sel_actionid").trimVal();
                var id = $("#hid_sys_subnavid").trimVal();
                $.post("/JsonFactory/PermissionHandler.ashx?oper=editsys_subnav", { id: id, actionid: actionid, columnid: columnid, subnavurl: subnavurl, subnavname: subnavname }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        alert("操作成功");
                        location.href = "sys_subnavlist.aspx?columnid=" + columnid + "&actionid=" + actionid;
                    }
                })
            })
        })

        function radiochange() {
            
                var ched = $("input:radio[name='radiobutton']:checked").val();

                GetActions(ched, 0);
             
         }
        function Getactioncloumns(columnid) {
             
            //获取权限分栏列表
            $.post("/JsonFactory/PermissionHandler.ashx?oper=Getallactioncolumns", {}, function (data) {
                data = eval("(" + data + ")");
                var columnid = $("#hid_columnid").trimVal();
                var actionid = $("#hid_actionid").trimVal();

                if (columnid == 0 && actionid == 0) {
                  
                    var str = '<label><input type="radio" name="radiobutton" value="0" checked="checked" onchange="radiochange()"    >全部</label>';

                    for (var i = 0; i < data.msg.length; i++) {
                        str += '<label><input type="radio" name="radiobutton" value="' + data.msg[i].Actioncolumnid + '" onchange="radiochange()" >' + data.msg[i].Actioncolumnname + '</label>';

                    }
                }
                else {
                     
                    var str = '<label><input type="radio" name="radiobutton" value="0" checked="checked" onchange="radiochange()"  disabled="disabled" >全部</label>';
                    if (columnid == 0) {
                        str = '<label><input type="radio" name="radiobutton" value="0" checked="checked" onchange="radiochange()"   >全部</label>';
                    }
                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i].Actioncolumnid == columnid) {
                            str += '<label><input type="radio" name="radiobutton" value="' + data.msg[i].Actioncolumnid + '" checked="checked" onchange="radiochange()">' + data.msg[i].Actioncolumnname + '</label>';
                        }
                        else {
                            str += '<label><input type="radio" name="radiobutton" value="' + data.msg[i].Actioncolumnid + '" onchange="radiochange()" disabled="disabled">' + data.msg[i].Actioncolumnname + '</label>';
                        }
                    }
                }

                $("#lbl_columns").after(str);
            })
        }
        function GetActions(columnid, actionid) {
            //获取权限列表
            $.post("/JsonFactory/PermissionHandler.ashx?oper=permissionlist", { columnid: columnid }, function (data) {
                data = eval("(" + data + ")");
                var columnid = $("#hid_columnid").trimVal();
                var actionid = $("#hid_actionid").trimVal();
                if (columnid == 0 && actionid == 0) {
                    var str = '<option value="0" class="mi-input" >未选择..</option>';
                   
                    for (var i = 0; i < data.msg.length; i++) {
                        
                            str += '<option value="' + data.msg[i].Actionid + '" class="mi-input"   >' + data.msg[i].Actionname + '</option>';
                         
                    }
                }
                else {
                    var str = '<option value="0" class="mi-input" disabled="disabled">未选择..</option>';
                    if (actionid == 0) {
                        str = '<option value="0" class="mi-input"   >未选择..</option>';
                    }
                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i].Actionid == actionid) {
                            str += '<option value="' + data.msg[i].Actionid + '" class="mi-input" selected="selected">' + data.msg[i].Actionname + '</option>';
                        }
                        else {
                            str += '<option value="' + data.msg[i].Actionid + '" class="mi-input"  disabled="disabled">' + data.msg[i].Actionname + '</option>';
                        }
                    }
                 }
               
                $("#sel_actionid").html(str);
            })
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
      <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li  class="on"><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>角色管理</span></a></li>
                <li ><a href="/ui/permissionui/sys_subnavdetail.aspx" onfocus="this.blur()" target=""><span>扩充子导航</span></a></li>
            </ul>
        </div>
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone"> 
            <div class="inner">
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        右侧子导航信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            子导航名称(25字内)</label>
                        <input   type="text" id="subnavname" size="25" class="mi-input"
                            style="width: 200px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            子导航相对路径</label>
                        <input  type="text" id="subnavurl" size="25" class="mi-input" style="width: 400px;"  placeholder="形如:/ui/pmui/a.aspx"/>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label" id="lbl_columns">
                            所属权限分栏</label>  
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            所属权限</label>
                        <select name="com_type" id="sel_actionid" class="ui-input">
                            
                        </select>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table border="0">
                    <tbody>
                        <tr>
                            <td width="600" height="80" align="center">
                                <input type="hidden" id="hid_sys_subnavid" value="<%=subnavid %>" />
                                 <input type="hidden" id="hid_actionid" value="<%=actionid %>" />
                                 <input type="hidden" id="hid_columnid" value="<%=columnid %>" />
                                <input type="button" name="confirmpub" class="mi-input" id="confirmpubb" value="  确认提交  " />
                                <input type="button" name="Submit" class="mi-input" value="返回上一步" onclick=" history.back()" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
