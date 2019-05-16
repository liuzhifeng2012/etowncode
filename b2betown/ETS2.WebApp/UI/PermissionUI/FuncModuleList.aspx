<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FuncModuleList.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PermissionUI.FuncModuleList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var rpage = $("#hid_rpage").trimVal();
            //获取权限列表
            SearchList(rpage, 16);

        })


        function SearchList(pageindex, pagesize) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/PermissionHandler.ashx?oper=permissionpagelist",
                data: { pageindex: pageindex, pagesize: pagesize },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询权限列表错误");
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

                    $("#hid_rpage").val(page);

                    return false;
                }
            });
        }

        function showsubnav(actionid, actionname,columnid) {
            SearchList1(1, 10, columnid, actionid);
            $("#hid_actionid").val(actionid);
            $("#span_actionname").text(actionname + "-右侧子导航管理");
            $("#href_subnavlist").bind("click", function(){
                location.href = 'sys_subnavlist.aspx?rpage='+$("#hid_rpage").trimVal()+'&actionid=' + actionid + "&columnid=" + columnid;
            });
            $("#rhshow").show();
        }
        function closesubnav() {
            $("#rhshow").hide();
        }
        function delfunc(actionid, actionname) {
            if (confirm("确认删除" + actionname + "吗")) {
                $.post("/JsonFactory/PermissionHandler.ashx?oper=delActionById", { actionid: actionid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("删除权限出错", { button: [{ title: "确定", value: true}], show: "slideDown", submit: function (e, v, m, f) { if (v == true) { window.open("FuncModuleList.aspx", target = "_self") } } });
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除权限成功", { button: [{ title: "确定", value: true}], show: "slideDown", submit: function (e, v, m, f) { if (v == true) { window.open("FuncModuleList.aspx", target = "_self") } } });
                    }
                });
            } else {

            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>角色管理</span></a></li>
                <li><a href="/ui/permissionui/sys_subnavdetail.aspx" onfocus="this.blur()" target=""><span>扩充子导航</span></a></li>
                <%--<li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>--%>
            </ul>
        </div>
        <%-- <div class="linetabs" style="color: rgb(51, 51, 51); font-family: 宋体; font-size: 12px;
            font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal;
            line-height: 22px; orphans: 2; text-align: start; text-indent: 0px; text-transform: none;
            white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto;
            -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); margin-top: 10px;
            padding-left: 15px;">
            <span style="margin: 0px; padding: 0px;">搜索：<input id="txtKeyword" class="txtinput1"
                maxlength="50" name="ctl00$ContentPlaceHolder1$txtKeyword" style="margin: 0px;
                padding: 0px; list-style: none; border: 1px solid rgb(204, 204, 204); height: 22px;
                line-height: 22px; width: 130px; color: rgb(204, 204, 204);" type="text" /><span
                    class="Apple-converted-space">&nbsp;</span>按<span class="Apple-converted-space">&nbsp;</span><select
                        id="drpCondition" class="txtinput2" name="ctl00$ContentPlaceHolder1$drpCondition"
                        style="border: 1px solid rgb(204, 204, 204); height: 22px; line-height: 22px;
                        width: 65px; color: rgb(204, 204, 204);">
                        <span class="Apple-converted-space"></span>
                        <option value="Name">按名称</option>
                        <span class="Apple-converted-space"></span>
                        <option value="Url">按路径</option>
                        <span class="Apple-converted-space"></span>
                    </select><span class="Apple-converted-space">&nbsp;</span>所属分栏：<span class="Apple-converted-space">&nbsp;</span><select
                        id="ddlApp" class="txtinput2" name="ctl00$ContentPlaceHolder1$ddlApp" style="border: 1px solid rgb(204, 204, 204);
                        height: 22px; line-height: 22px; width: 105px; color: rgb(204, 204, 204);">
                        <span class="Apple-converted-space"></span>
                        <option selected="selected" value="0">全部</option>
                        <span class="Apple-converted-space"></span>
                        <option value="10">微信管理</option>
                        <span class="Apple-converted-space"></span>
                        <option value="17">员工管理</option>
                        <span class="Apple-converted-space"></span>
                    </select><span class="Apple-converted-space">&nbsp;</span></span><input id="btnSearch"
                        class="searchButton" style="margin: 0px; padding: 0px; list-style: none;" type="button"
                        value="搜索" /></div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    权限列表</h3>
                <table width="780" border="0">
                    <tr >
                        <td width="51" style="font-size:14px;">
                            <p align="left">
                                权限ID</p>
                        </td>
                        <td width="157" style="font-size:14px;">
                            <p align="left">
                                权限名称
                            </p>
                        </td>
                        <td width="60" style="font-size:14px;">
                            <p align="left">
                                权限所属分栏
                            </p>
                        </td>
                        <td width="40" style="font-size:14px;">
                            <p align="left">
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
                    <tr >
                        <td width="51" style="font-size:14px;">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td width="157" style="font-size:14px;">
                            <p align="left">
                                ${ActionName} <a href="javascript:void(0)" style="font-size:12px;color:rgb(3, 110, 184);" onclick="showsubnav('${Id}','${ActionName}','${ActionColumnId}')">右侧子导航管理</a>
                            </p>
                        </td>
                        <td width="72" style="font-size:14px;">
                            <p align="left">
                                ${ActionColumnName}</p>
                        </td>
                         
                        <td width="40" style="font-size:14px;">
                            <p align="left">
                                <a href="FuncModuleManager.aspx?actioncolumnid=${ActionColumnId}" style="color: rgb(3, 110, 184); cursor: pointer;
                            text-decoration: none;">添加</a>&nbsp;|&nbsp; <a href="FuncModuleManager.aspx?actionid=${Id}&actioncolumnid=${ActionColumnId}"
                                style="color: rgb(3, 110, 184); cursor: pointer; text-decoration: none;">编辑</a>&nbsp;|&nbsp;<span
                                    class="Apple-converted-space">&nbsp;</span><a id="btnDelete" href="javascript:void(0)"
                                       style="color: rgb(3, 110, 184);
                                        cursor: pointer; text-decoration: none;" onclick="delfunc('${Id}', '${ActionName}')">删除</a> </p>
                        </td>
                      
                    </tr>
    </script>
    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        display: none; left: 10%; position: absolute; top: 20%;">
        <input type="hidden" id="hid_actionid" value="" />
         <input type="hidden" id="hid_seledsubnavlist" value="" />
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
                            <td width="40" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                                <p align="left">
                                    所属权限分栏
                                </p>
                            </td>
                            <td width="40" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                                <p align="left">
                                    所属权限
                                </p>
                            </td>
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
                    <input id="href_subnavlist" type="button" class="formButton" value="  编辑子导航库  " />
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
                         
                        <td width="40" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                            <p align="left">
                            ${actioncolumnname}
                               </p>
                        </td>
                       <td width="40" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                            <p align="left">
                            ${actionname}
                            </p>
                        </td> 
                          <td width="40" style="height: 30px;border-bottom: 1px solid #E0ECF8;padding-left: 3px;">
                            <p align="left">
                            {{if viewcode==0}}
                            <label><input type="checkbox" name="checkbox_name" onclick="chedsubnav(this,'${id}','${actionid}','${groupids}')" /> </label>
                            {{else}}
                            <label><input type="checkbox" name="checkbox_name" checked="checked" onclick="chedsubnav(this,'${id}','${actionid}','{groupids}')" /> </label>
                            {{/if}}
                            </p>
                        </td> 
                    </tr>
        </script>
        <script type="text/javascript">
            function chedsubnav(obj, subnavid, actionid,groupids) {
                var viewcode = 0;
                if ($(obj).is(':checked') == true) {
                    viewcode = 1;
                }
                $.post("/JsonFactory/PermissionHandler.ashx?oper=upsubnavviewcode", { viewcode: viewcode, subnavid: subnavid, actionid: actionid,groupids:groupids }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        alert("操作成功");
                        return;
                    }
                })
            }
            function SearchList1(pageindex, pagesize, columnid, actionid) {
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
          <!--返回页面页数-->
        <input type="hidden" id="hid_rpage" value="<%=rpage %>" />
</asp:Content>