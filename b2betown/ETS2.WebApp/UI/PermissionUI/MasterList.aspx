<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MasterList.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.UI.PermissionUI.MasterList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //            var oper1 = $("#hid_oper1").trimVal();
            var groupid = $("#hid_groupid").trimVal();
            var groupname = $("#hid_groupname").trimVal();
            var childcomid = $("#hid_childcomid").trimVal();

            //动态添加管理组
            $.post("/jsonfactory/permissionhandler.ashx?oper=GetAllGroups", {}, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        //上边管理组类型
                        $("#h3groups").html("");

                        var h3groups = "管理组类型：";
                        for (var i = 0; i < data.totalCount; i++) {
                            if (groupid != data.msg[i].Groupid) {
                                h3groups += '<label><input name="radgroup" type="radio" value="' + data.msg[i].Groupid + '">' + data.msg[i].Groupname + '</label>';
                            } else {
                                h3groups += '<label><input name="radgroup" type="radio" value="' + data.msg[i].Groupid + '" checked>' + data.msg[i].Groupname + '</label>';

                            }
                        }
                        if (groupid != "0") {
                            h3groups += '<label><input name="radgroup" type="radio" value=""  >全部</label>';
                        } else {
                            h3groups += '<label><input name="radgroup" type="radio" value="" checked>全部</label>';
                        }

                        $("#h3groups").html(h3groups);

                        //下边重新分组
                        $("#tdgroups").html("");

                        var groupstr = "";
                        for (var i = 0; i < data.totalCount; i++) {
                            groupstr += '<label><input name="radgrouptype" type="radio" value="' + data.msg[i].Groupid + '">' + data.msg[i].Groupname + '</label>';
                        }
                        $("#tdgroups").html(groupstr);
                    }

                }
            });

            if (groupid == "") {//展示全部联系人

                $(".jqibuttons").hide();
                $("#divsearch").hide();

            }
            else {//显示特定管理组联系人

                $(".jqibuttons").show();
                $("#divsearch").show();
            }

            //            $("input:radio[name='radgroup'][value=" + groupid + "]").attr("checked", true);

            $('input[name="radgroup"]').live("click", function () {
                var issuetype = $('input:radio[name="radgroup"]:checked').trimVal();
                window.open("masterlist.aspx?groupid=" + issuetype, target = "_self");
            });

            //获取联系人列表
            SearchList(1, 10, '1');




            $("#cancel_rh").click(function () {
                $("input[type='checkbox'][name='Uptype']:checked").each(function () {
                    $(this).attr("checked", false);
                });
                $("#rhshow").hide();
            })

            $("#submit_rh").click(function () {

                var masterid = $("#hid_masterid").val();
                var mastername = $("#hid_mastername").val();
                //限制一个用户只是属于一种角色
                var groupid = $('input:radio[name="radgrouptype"]:checked').trimVal();
                EditMasterGroup(masterid, mastername, groupid);
                //                var grouparr = ""; //选中管理组数组字符串
                //                $("input[type='checkbox'][name='Uptype']:checked").each(function () {
                //                    grouparr += $(this).val() + ",";
                //                });
                //                if (grouparr == "") {
                //                    $.prompt("请选择管理组");
                //                    return;
                //                } else {
                //                    grouparr = grouparr.substr(0, grouparr.length - 1);

                //                    EditMasterGroup(masterid, mastername, grouparr);


                //                }

            })

            $("#Search").click(function () {
                 SearchList(1, 10, "1");
            })

        })
        function EditMasterGroup(masterid, mastername, grouparr) {
            $.post("/jsonfactory/permissionhandler.ashx?oper=editmastergroup", { masterid: masterid, mastername: mastername, grouparr: grouparr }, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {
                    $.prompt("操作成功", { buttons: [{ title: "确定", value: true}], show: "slideDown", submit: function (m, v, e, f)
                    { location.reload() }
                    });
                    return;

                }
            });
        }
        //重新分组
        function referrer_ch(masterid, mastername) {
            $("#span_rh").text("人员姓名：" + mastername);
            $("#hid_masterid").val(masterid);
            $("#hid_mastername").val(mastername);
            //根据人员id得到人员所在分组
            $.post("/JsonFactory/PermissionHandler.ashx?oper=GetGroupByUserId", { userid: masterid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    var groupid = data.msg.Groupid;

                    $("input:radio[name='radgrouptype'][value='" + groupid + "']").attr("checked", true);
                }
            })

            $("#rhshow").show();
        };
        //调整员工状态
        function adjustemploerstate(masterid) {
            var employerstate = $("#selemployerstate_" + masterid).val();
            $.post("/JsonFactory/AccountInfo.ashx?oper=adjustemploerstate", { masterid: masterid, employerstate: employerstate }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("调整员工状态出现问题");
                    return;
                }
                if (data.type == 100) {
                    $.prompt("调整员工状态成功", { buttons: [{ title: "确定", value: true}], submit: function (m, v, e, f) { if (v == true) { location.reload() } } });
                }
            })
        }
        function viewmaster(employstate) {
            SearchList(1, 10, employstate);
        }

        

        function SearchList(pageindex, pagesize, employstate) {
            var key = $("#key").val();
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/PermissionHandler.ashx?oper=masterpagelistbyemploystate",
                data: { employstate: employstate, pageindex: pageindex, pagesize: pagesize, groupid: $("#hid_groupid").trimVal(), childcomid: $("#hid_childcomid").trimVal(), key: key },
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
                            setpage(data.totalCount, pagesize, pageindex, employstate);
                        }


                    }
                }
            })


        }

        //分页
        function setpage(newcount, newpagesize, curpage, employstate) {
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

                    SearchList(page, newpagesize, employstate);

                    return false;
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%--  <li><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>管理组管理</span></a></li>--%>
                <li class="on"><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <%--  <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>--%>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ComFinance.aspx" onfocus="this.blur()" target=""><span>财务对账表</span></a></li>
                <li><a href="Withdraw_handle.aspx" onfocus="this.blur()" target=""><span>提现财务管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li ><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>产品类目</span></a></li>
                
                
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3 id="h3groups">
                    <%--  管理组类型：
                    <label>
                        <input name="radgroup" type="radio" value="1">
                        管理员</label>
                    <label>
                        <input name="radgroup" type="radio" value="2">
                        经理</label>
                    <label>
                        <input name="radgroup" type="radio" value="3">
                        员工</label>
                    <label>
                        <input name="radgroup" type="radio" value="" checked>
                        全部</label>--%>
                </h3>
                <br />
                <%--   <lable style="font-size: 15px; color: Red;">
                  <span style="color:Blue; font-size:12px;">
                    查询联系人 :
                </span>
                </lable>
                <br />
                <br />
                <div id="divsearch">
                    姓名<span class="Apple-converted-space">&nbsp;</span><input id="txtSearch" class="txtinput gray"
                        style="margin: 0px; padding: 0px 0px 0px 3px; list-style: none; color: gray;
                        width: 187px; height: 22px; line-height: 22px; background-color: rgb(236, 236, 237);
                        border: 1px solid rgb(221, 222, 222); background-position: 50% 100%; background-repeat: repeat no-repeat;"
                        type="text" /><span class="Apple-converted-space">&nbsp;</span><input id="search"
                            class="btnbg" style="margin: 0px; padding: 0px; list-style: none; background-color: rgb(243, 152, 0);
                            width: 65px; border-style: none; height: 24px; cursor: pointer; line-height: 24px;
                            background-position: initial initial; background-repeat: initial initial;" type="button"
                            value="搜索" />
                </div>
                <br />--%>
                <div style="text-align: center;">
                    <label>
                        请输入(账号、姓名、手机、商户名称)
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                     <label>
                        <input name="Search" type="button" id="Search" value="订单查询" style="width: 120px;
                            height: 26px;">
                    </label>
                </div>

                <br />
                <a href="#" onclick="viewmaster('0,1')">显示全部</a>&nbsp;&nbsp;<a href="#" onclick="viewmaster('0')">显示离职</a>&nbsp;&nbsp;<a
                    href="#" onclick="viewmaster('1')">显示在职</a>

                <table width="780" border="0">
                    <tr>
                        <td width="15">
                            <p align="left">
                                人员ID</p>
                        </td>
                        <td width="50">
                            <p align="left">
                                账 号
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                密 码
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                人员姓名
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                人员公司
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                所在渠道
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                手机
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                所在分组
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                管理
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
    <input type="hidden" id="hid_groupid" value="<%=groupid %>" />
    <input type="hidden" id="hid_groupname" value="<%=groupname%>" />
    <input type="hidden" id="hid_childcomid" value="<%=childcomid%>" />
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td width="15">
                             <p>${MasterId}</p>
                        </td>
                          <td width="50">
                            <p align="left">
                                ${Accounts}
                            </p>
                        </td>
                         <td width="50">
                            <p align="left">
                                ${PassWord}
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                ${MasterName}
                            </p>
                        </td>
                       
                        <td width="50">
                            <p align="left">
                                ${CompanyName}
                            </p>
                        </td>
                          <td width="50">
                            <p align="left">
                                ${ChannelCompanyName}
                            </p>
                        </td>
                      <td width="40">
                            <p align="left">
                                ${Tel}
                            </p>
                        </td>
                         <td width="50">
                            <p align="left">
                                ${GroupName}
                            </p>
                        </td>
                         <td width="50">
                            <p align="left">
                               <a href="javascript:void(0)" onclick='referrer_ch("${MasterId}","${MasterName}")'> 重新分组 </a>&nbsp;&nbsp;|
                              
                             <select id="selemployerstate_${MasterId}" onchange="adjustemploerstate('${MasterId}')">
                             {{if Employeestate==1}}
                              <option value="1" selected="selected">在职</option>
                              <option value="0">离职</option>
                             {{else}}
                              <option value="1">在职</option>
                              <option value="0" selected="selected">离职</option>
                             {{/if}}
                               
                            </select>
                            </p>
                        </td>
                    </tr>
    </script>
    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="span_rh"></span>
                </td>
            </tr>
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                    管理组类型:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" id="tdgroups">
                    <%-- <label>
                        <input type="checkbox" name="Uptype" value="1" />管理员</label>
                    <label>
                        <input type="checkbox" name="Uptype" value="2" />经理</label>
                    <label>
                        <input type="checkbox" name="Uptype" value="3" />员工</label>--%>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_masterid" value="" />
                    <input type="hidden" id="hid_mastername" value="" />
                    <input name="submit_rh" id="submit_rh" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" value="  取  消  " />
                </td>
            </tr>
        </table>
    </div>
    <%--<div class="jqibuttons" style="text-align: right; padding: 7px; border: 1px solid rgb(238, 238, 238);
        background-color: rgb(244, 244, 244); margin-top: 30px;">
        <button id="jqibutton1" name="jqi_0_button" style="background-image: url(/images/fyw_07.png);
            height: 27px; line-height: 27px; cursor: pointer; border: 0px none; color: rgb(51, 51, 51);
            font-size: 12px; margin-left: 10px; width: 64px; background-position: initial initial;
            background-repeat: no-repeat no-repeat;" value="true">
            提 交
        </button>
        <button id="jqibutton2" class="jqidefaultbutton" style="background-image: url(/images/fyw_10.png);
            height: 26px; line-height: 26px; cursor: pointer; border: 0px none; color: rgb(51, 51, 51);
            font-size: 12px; margin-left: 10px; width: 64px; background-position: initial initial;
            background-repeat: no-repeat no-repeat;" value="false">
            取消
        </button>
    </div>--%>
</asp:Content>
