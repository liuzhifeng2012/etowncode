<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FuncModuleManager.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PermissionUI.FuncModuleManager" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var actionid = $("#hid_actionid").trimVal();
            var actioncolumnid = $("#hid_actioncolumnid").trimVal();
            if (actionid != 0)//编辑权限信息
            {
                $.post("/JsonFactory/PermissionHandler.ashx?oper=getActionById", { actionid: actionid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#txtactionname").val(data.msg.Actionname);
                        $("#txtactionurl").val(data.msg.Actionurl);

                        //                        $("#dropcolumnid").val(data.msg.Actioncolumnid);
                        LoadActionCloumn(data.msg.Actioncolumnid);
                        $('input:[name="radIsShow"][value=' + data.msg.Viewmode + ']').attr("checked", true);
                    }

                });
            } else {
                //                $("#dropcolumnid").val(actioncolumnid);
                LoadActionCloumn(actioncolumnid);
            }

            $("#btnsub").click(function () {
                var actionname = $("#txtactionname").trimVal();
                var actionurl = $("#txtactionurl").trimVal();
                var columnid = $("#dropcolumnid").val();
                var columnname = $("#dropcolumnid").text();
                var isshow = $("input:radio[name='radIsShow']:checked").trimVal();
                if (actionname == "") {
                    $.prompt("权限名称不可为空", { button: [{ title: "确定", value: true}], show: "slideDown", submit: function (e, v, m, f) { if (v == true) { } } });
                    return;
                }
                else {
                    $.post("/JsonFactory/PermissionHandler.ashx?oper=EditAction", { actionurl: actionurl, actionid: actionid, actionname: actionname, columnid: columnid, columnname: columnname, isshow: isshow }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("编辑操作出现错误");
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("编辑成功", { button: [{ title: "确定", value: true}], show: "slideDown", submit: function (e, v, m, f) { if (v == true) { window.open("FuncModuleList.aspx", target = "_self") } } });
                            return;
                        }
                    });
                }
            })
        })
        function LoadActionCloumn(columnid)
        {
          $.post("/JsonFactory/PermissionHandler.ashx?oper=Getallactioncolumns",{},function(data){
            data=eval("("+data+")");
            if(data.type==1){}
            if(data.type==100)
            {
               $("#dropcolumnid").html("");
               var str="";
               for(var i=0;i<data.msg.length;i++)
               {
                    if(data.msg[i].Actioncolumnid==columnid)
                    {
                     str+='<span class="Apple-converted-space"></span>'+
                           ' <option value="'+data.msg[i].Actioncolumnid+'" selected="selected">'+data.msg[i].Actioncolumnname+'</option>';
                    }
                    else
                    {
                     str+='<span class="Apple-converted-space"></span>'+
                           ' <option value="'+data.msg[i].Actioncolumnid+'">'+data.msg[i].Actioncolumnname+'</option>';
                    }
               }
                $("#dropcolumnid").html(str);
            }
          })
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>角色管理</span></a></li>
                <%-- <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>--%>
            </ul>
        </div>
    </div>
    <div id="setting-home">
        <div>
            <h3>
                基本信息</h3>
            <table border="0" cellpadding="0" cellspacing="0" class="tabuser_top highttop" width="100%">
                <tr>
                    <td class="tabuser_td">
                        名 称：
                    </td>
                    <td class="tabuser">
                        <input id="txtactionname" style="margin: 0px; padding: 0px; list-style: none;" type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="tabuser_td">
                        路 径：
                    </td>
                    <td class="tabuser">
                        <input id="txtactionurl" style="margin: 0px; padding: 0px; list-style: none; width: 500px;"
                            type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="tabuser_td">
                        权限分栏：
                    </td>
                    <td class="tabuser">
                        <select id="dropcolumnid" class="txtinput2" name="ctl00$ContentPlaceHolder1$dropAppId"
                            style="border: 1px solid rgb(204, 204, 204); height: 22px; line-height: 22px;
                            width: 130px; color: rgb(204, 204, 204);">
                            <%--  <span class="Apple-converted-space"></span>
                            <option value="1">高级管理权限</option>--%>
                            <span class="Apple-converted-space" ></span>
                            <option value="1"  >管理权限</option>
                            <%-- <span class="Apple-converted-space"></span>
                            <option value="2">验证管理</option>--%>
                            <span class="Apple-converted-space"></span>
                            <option value="3">销售管理</option>
                            <span class="Apple-converted-space"></span>
                            <option value="4">会员管理</option>
                            <span class="Apple-converted-space"></span>
                            <option value="5">微信管理</option>
                            <%--  <span class="Apple-converted-space"></span>
                            <option value="6">账户管理</option>--%>
                            <span class="Apple-converted-space"></span>
                            <option value="7">营销管理</option>
                            <span class="Apple-converted-space"></span>
                            <option value="8">商户管理</option>
                        </select>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td class="tabuser_td">
                        是否显示：
                    </td>
                    <td class="tabuser">
                        <input name="radIsShow" style="margin: 0px; padding: 0px; list-style: none;" type="radio"
                            value="true" />是
                        <input name="radIsShow" style="margin: 0px; padding: 0px; list-style: none;" type="radio"
                            value="false" checked />否
                    </td>
                </tr>
            </table>
            <input type="button" id="btnsub" value="提交" />
        </div>
    </div>
    <input type="hidden" id="hid_actionid" value="<%=actionid %>" />
    <input type="hidden" id="hid_actioncolumnid" value="<%=actioncolumnid %>" />
</asp:Content>
