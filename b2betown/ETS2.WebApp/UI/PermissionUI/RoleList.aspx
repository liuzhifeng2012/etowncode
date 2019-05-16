<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.UI.PermissionUI.RoleList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //获取权限列表
            SearchList(1, 16);


            function SearchList(pageindex, pagesize) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/PermissionHandler.ashx?oper=grouppagelist",
                    data: { pageindex: pageindex, pagesize: pagesize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pagesize, pageindex);
                            }


                        }
                    }
                })


            }

            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchList(page, newpagesize);

                        return false;
                    }
                });
            }
        })
        function insnewgroup() {
            window.open("RoleManager.aspx", target = "_self");
        }
        function delgroup(groupid, groupname) {
            if (confirm("确认删除" + groupname + "吗？")) {
                $.post("/JsonFactory/PermissionHandler.ashx?oper=delGroupById", { groupid: groupid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert("删除" + groupname + "出错");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除" + groupname + "成功", { button: [{ title: "确定", value: true}], show: "slideDown", submit: function (e, v, m, f) { if (v == true) { window.open("RoleList.aspx", target = "_self") } } });
                    }
                });
            }
            else {

            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li class="on"><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>角色管理</span></a></li>
                <%-- <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>--%>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    管理组列表</h3>
                <br />
                <label onclick="insnewgroup()" style="float: right; color: Blue;">
                    添加管理组</label>
                <table width="780" border="0">
                    <tr>
                        <td width="51">
                            <p align="left">
                                管理组编号</p>
                        </td>
                        <td width="157">
                            <p align="left">
                                管理组名称
                            </p>
                        </td>
                        <td width="80">
                            <p align="center">
                                操作
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td >
                            <p align="left">
                                ${Groupid}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Groupname}
                            </p>
                        </td>
                       
                        <td >
                         <p align="center">
                         <a id="hlCreate2" href="MasterList.aspx?groupid=${Groupid}&groupname=${Groupname}" style="color: rgb(3, 110, 184);
                            cursor: pointer; text-decoration: none;">查看</a>&nbsp;|&nbsp; <a id="hlCreate2" href="MasterList.aspx" style="color: rgb(3, 110, 184);
                            cursor: pointer; text-decoration: none;">添加人员</a>&nbsp;|&nbsp;<span class="Apple-converted-space">&nbsp;</span><a
                                href="RoleManager.aspx?groupid=${Groupid}" style="color: rgb(3, 110, 184); cursor: pointer;
                                text-decoration: none;">编辑</a>&nbsp;|&nbsp;<span class="Apple-converted-space">&nbsp;</span><a
                                    href="RoleFunManager.aspx?groupid=${Groupid}" style="color: rgb(3, 110, 184); cursor: pointer;
                                    text-decoration: none;">分配权限</a>&nbsp;|&nbsp;<span class="Apple-converted-space">&nbsp;</span>  <a href="javascript:void(0)" style="color: rgb(3, 110, 184); cursor: pointer;text-decoration: none;" onclick="delgroup('${Groupid}','${Groupname}')">删 除</a>&nbsp;
                                        </p>
                        </td>
                      
                    </tr>
    </script>
</asp:Content>
