<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_subnavlist.aspx.cs"
    Inherits="ETS2.WebApp.UI.PermissionUI.sys_subnavlist" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var columnid = $("#hid_columnid").trimVal();
            var actionid = $("#hid_actionid").trimVal();
            //得到权限分栏表
            Getactioncloumns(columnid);
            //得到权限表
            GetActions(columnid, actionid);

            SearchList(1, 12, columnid, actionid);

        })
        function delsubnav(subnavid) {
            if (confirm("确认删除吗")) {
                $.post("/JsonFactory/PermissionHandler.ashx?oper=delsys_subnav", { subnavid: subnavid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        alert("操作成功");
                        location.reload();
                    }
                })
            }
        }
        function SearchList(pageindex, pagesize, columnid, actionid) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/PermissionHandler.ashx?oper=sys_subnavpagelist",
                data: { pageindex: pageindex, pagesize: pagesize, columnid: columnid, actionid: actionid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                            $.prompt("查询权限列表错误");
                        $("#tblist").empty();
                        $("#divPage").empty();
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("查询数据为空");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalcount, pagesize, pageindex, columnid, actionid);
                        }
                    }
                }
            })
        }

        //分页
        function setpage(newcount, newpagesize, curpage, columnid, actionid) {
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

                    SearchList(page, newpagesize, columnid, actionid);

                    return false;
                }
            });
        }

        function radiochange() {

            var ched = $("input:radio[name='radiobutton']:checked").val();

            GetActions(ched, 0);

            SearchList(1, 12, ched, 0);
        }
        function selchange() {
            var seled = $("#sel_actionid").val();
            var checked = $("input:radio[name='radiobutton']:checked").trimVal();
            

            SearchList(1, 12, checked, seled);
        }

        function Getactioncloumns(columnid) {

            //获取权限分栏列表
            $.post("/JsonFactory/PermissionHandler.ashx?oper=Getallactioncolumns", {}, function (data) {
                data = eval("(" + data + ")");
                var str = '<label><input type="radio" name="radiobutton" value="0" checked="checked" onchange="radiochange()"  disabled="disabled" >全部</label>';
                if (columnid == 0) {
                    str = '<label><input type="radio" name="radiobutton" value="0" checked="checked" onchange="radiochange()"   >全部</label>';
                }
                for (var i = 0; i < data.msg.length; i++) {
                    if (data.msg[i].Actioncolumnid == columnid) {
                        str += '<label><input type="radio" name="radiobutton" value="' + data.msg[i].Actioncolumnid + '" checked="checked" onchange="radiochange()">' + data.msg[i].Actioncolumnname + '</label>';
                    }
                    else {
                        str += '<label><input type="radio" name="radiobutton" value="' + data.msg[i].Actioncolumnid + '" onchange="radiochange()"   disabled="disabled" >' + data.msg[i].Actioncolumnname + '</label>';
                    }
                }
                $("#lbl_columns").after(str);
            })
        }
        function GetActions(columnid, actionid) {
            //获取权限列表
            $.post("/JsonFactory/PermissionHandler.ashx?oper=permissionlist", { columnid: columnid }, function (data) {
                data = eval("(" + data + ")");
                var str = '<option value="0" class="mi-input" disabled="disabled" >未选择..</option>';
                if(actionid==0)
                {
                    str = '<option value="0" class="mi-input"   >未选择..</option>';
                }
                for (var i = 0; i < data.msg.length; i++) {
                    if (data.msg[i].Actionid == actionid) {
                        str += '<option value="' + data.msg[i].Actionid + '" class="mi-input" selected="selected" >' + data.msg[i].Actionname + '</option>';
                    }
                    else {
                        str += '<option value="' + data.msg[i].Actionid + '" class="mi-input"   disabled="disabled" >' + data.msg[i].Actionname + '</option>';
                    }
                }
                $("#sel_actionid").html(str);
            })
        }
        function addsubnav() {
//            if (confirm("扩充操作需要技术添加，确定继续添加吗")) {
                var columnid = $("input:radio[name='radiobutton']:checked").trimVal();
                var actionid = $("#sel_actionid").val();
                location.href = "sys_subnavdetail.aspx?columnid=" + columnid + "&actionid=" + actionid;
//            }
        }


    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>角色管理</span></a></li>
                <li ><a href="/ui/permissionui/sys_subnavdetail.aspx" onfocus="this.blur()" target=""><span>扩充子导航</span></a></li> 
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    右侧子导航列表</h3>
                <div class="mi-form-item">
                    <label class="mi-label" id="lbl_columns">
                        所属权限分栏</label>
                </div>
                <div class="mi-form-item">
                    <label class="mi-label">
                        所属权限</label>
                    <select name="com_type" id="sel_actionid" class="ui-input" onchange="selchange()">
                    </select>
                </div>
                <table width="780" border="0" style="margin-left: 30px;">
                    <tr>
                        <td width="51">
                            <p align="left">
                                子导航ID</p>
                        </td>
                        <td width="157">
                            <p align="left">
                                子导航名称
                            </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                子导航url
                            </p>
                        </td>
                          <td width="40">
                            <p align="left">
                                所属权限
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                所属权限分栏
                            </p>
                        </td> 
                        <td width="40">
                            <p align="left">
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <a href="javascript:void(0)" class="a_anniu" style="margin: 20px 0 0 20px;" onclick="showsubnavlist()">
                    选择子导航</a> <a href="/ui/permissionui/FuncModuleList.aspx" class="a_anniu" style="margin: 20px 0 0 20px;">
                        返回</a> 
                   <a href="javascript:void(0)" class="a_anniu" onclick="addsubnav()" style="margin: 0 0 0 20px;">
                            手工扩充子导航</a> 
            </div>
        </div>
    </div>
    <div class="data">
    </div>
  
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td width="51">
                            <p align="left">
                                ${id}</p>
                        </td>
                        <td width="157">
                            <p align="left">
                                ${subnav_name}  
                            </p>
                        </td>
                        <td width="72">
                            <p align="left">
                                ${subnav_url}</p>
                        </td>
                         
                       
                       <td width="40">
                            <p align="left">
                            ${actionname}
                            </p>
                        </td> 
                         <td width="40">
                            <p align="left">
                            ${actioncolumnname}
                               </p>
                        </td>
                         <td width="40">
                            <p align="left">
                            <a  class="a_anniu"  href="sys_subnavdetail.aspx?subnavid=${id}&columnid=${actioncolumnid}&actionid=${actionid}">编辑</a>
                             <a  class="a_anniu"  href="javascript:void(0)" onclick="delsubnav('${id}')">删除</a>                   
                             
                               </p>
                        </td>
                    </tr>
    </script>
        <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        display: none; left: 10%; position: absolute; top: 20%;">
        <table width="800px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr>
                <td align="center" colspan="2">
                    <span style="font-size: 14px;" id="span_actionname"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   
                    <table width="780" border="0" style="margin-left: 30px;">
                        <tr>
                            <td width="51" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                                <p align="left">
                                    子导航ID</p>
                            </td>
                            <td width="157" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                                <p align="left">
                                    子导航名称
                                </p>
                            </td>
                            <td width="60" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                                <p align="left">
                                    子导航url
                                </p>
                            </td>
                        <%-- <td width="40" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                                <p align="left">
                                    所属权限分栏
                                </p>
                            </td>
                            <td width="40" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                                <p align="left">
                                    所属权限
                                </p>
                            </td> --%>
                            <td width="40" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                                <p align="left">
                                    操作
                                </p>
                            </td>
                        </tr>
                        <tbody id="Tbody1">
                        </tbody>
                    </table>
                    <div id="divPage1">
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                   
                    <input id="closebtn" type="button" class="formButton" value="  关 闭  " onclick="closesubnav()" />
                </td>
            </tr>
        </table> 
    </div>
    <script type="text/x-jquery-tmpl" id="Script1">   
                    <tr class="d_out"  >
                        <td width="25" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                            <p align="left">
                                ${id}</p>
                        </td>
                        <td width="55" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                            <p align="left">
                                ${subnav_name}  
                            </p>
                        </td>
                        <td width="72" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                            <p align="left">
                                ${subnav_url}</p>
                        </td>
                          <%--  <td width="40">
                            <p align="left">
                            ${actionname}
                            </p>
                        </td> 
                         <td width="40">
                            <p align="left">
                            ${actioncolumnname}
                               </p>
                        </td>--%>
                       
                          <td width="40" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                            <p align="left">
                           {{if isunderaction==0}}
                            <label><input type="checkbox" name="checkbox_name" 
onclick="chedsubnav(this,'${id}','${actionid}','${actioncolumnid}','${viewcode}','${groupids}')" /> 
</label>
                            {{else}}
                            <label><input type="checkbox" name="checkbox_name" 
checked="checked" onclick="chedsubnav
(this,'${id}','${actionid}','${actioncolumnid}','${viewcode}','${groupids}')" /> </label>
                            {{/if}}
                            </p>
                        </td> 
                    </tr>
    </script>
    <script type="text/javascript"> 
        function showsubnavlist() {
            var seledactionid = $("#sel_actionid").val();
            var seledactionname = $("#sel_actionid").find("option:selected").text();
            var seledcolumnid = $("input:radio[name='radiobutton']:checked").trimVal();
          
            //显示子导航库
            SearchList1(1, 10, seledcolumnid, seledactionid);
      
            $("#span_actionname").text(seledactionname + "-右侧子导航选择");

            $("#rhshow").show();
        }
        function closesubnav() {
            $("#rhshow").hide();
            location.reload();

        }
        function chedsubnav(obj, subnavid, oldactionid,oldcolumnid,oldviewcode,isunderaction,groupids) {
            var checkd = 0;
            if ($(obj).is(':checked') == true) {
                checkd = 1;
            }

            var seledactionid = $("#sel_actionid").val();
            var seledcolumnid = $("input:radio[name='radiobutton']:checked").trimVal();

            $.post("/JsonFactory/PermissionHandler.ashx?oper=upsubnavdatabase", { oldviewcode: oldviewcode, subnavid: subnavid, oldactionid: oldactionid, oldcolumnid: oldcolumnid,oldgroupids:groupids, newviewcode: checkd,  newactionid: seledactionid, newcolumnid: seledcolumnid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    alert("操作成功");
                    return;
                }
            })

//            if (checkd == 0) {
//                $.post("/JsonFactory/PermissionHandler.ashx?oper=upsubnavdatabase", { viewcode: oldviewcode, subnavid: subnavid, actionid: oldactionid, columnid: oldcolumnid, checkd: checkd }, function (data) {
//                    data = eval("(" + data + ")");
//                    if (data.type == 1) { }
//                    if (data.type == 100) {
//                        alert("操作成功");
//                        return;
//                    }
//                })
//            } else {
//                var seledactionid = $("#sel_actionid").val();
//                var seledcolumnid = $("input:radio[name='radiobutton']:checked").trimVal();

//                $.post("/JsonFactory/PermissionHandler.ashx?oper=upsubnavdatabase", { viewcode: oldviewcode, subnavid: subnavid, actionid: seledactionid, columnid: seledcolumnid, checkd: checkd }, function (data) {
//                    data = eval("(" + data + ")");
//                    if (data.type == 1) { }
//                    if (data.type == 100) {
//                        alert("操作成功");
//                        return;
//                    }
//                })
//            }
        }
        function SearchList1(pageindex, pagesize, columnid, actionid) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/PermissionHandler.ashx?oper=allsys_subnavpagelist",
                data: { pageindex: pageindex, pagesize: pagesize, columnid: columnid, actionid: actionid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                            $.prompt("查询权限列表错误");
                        $("#Tbody1").empty();
                        $("#divPage1").empty();
                        return;
                    }
                    if (data.type == 100) {
                        $("#Tbody1").empty();
                        $("#divPage1").empty();
                        if (data.totalCount == 0) {
                            $("#Tbody1").html("查询数据为空");
                        } else {
                            $("#Script1").tmpl(data.msg).appendTo("#Tbody1");
                            setpage1(data.totalcount, pagesize, pageindex, columnid, actionid);
                        }
                    }
                }
            })
        }

        //分页
        function setpage1(newcount, newpagesize, curpage, columnid, actionid) {
            $("#divPage1").paginate({
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

                    SearchList1(page, newpagesize, columnid, actionid);

                    return false;
                }
            });
        }
    </script>
    <input type="hidden" id="hid_actionid" value="<%=actionid %>" />
    <input type="hidden" id="hid_columnid" value="<%=columnid %>" />
</asp:Content>
