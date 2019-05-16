<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleManager.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.UI.PermissionUI.RoleManager" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            //动态添加管理组
            $.post("/jsonfactory/permissionhandler.ashx?oper=GetAllGroups", {}, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        $("#tdgroups").html("");

                        var groupstr = "";
                        for (var i = 0; i < data.totalCount; i++) {

                            groupstr += '<label><input type="checkbox" name="checkGrougId" value="' + data.msg[i].Groupid + '" />' + data.msg[i].Groupname + '</label>';

                        }
                        $("#tdgroups").html(groupstr);
                    }

                }
            });


            var groupid = $("#hid_groupid").trimVal();
            if (groupid != 0)//编辑管理组信息
            {
                $("#groupname").attr("readonly", "readonly");
                $.post("/JsonFactory/PermissionHandler.ashx?oper=getGroupById", { groupid: groupid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#groupname").val(data.msg.Groupname);
                        $("#groupinfo").val(data.msg.Groupinfo);

                        var groupidstr = data.msg.Groupids;
                        var items = groupidstr.split(/[,，]/g);
                        $.each(items, function (index, item) {
                            $("input[name='checkGrougId']").each(function () {
                                if ($(this).val() == item) {
                                    $(this).attr("checked", true);
                                }
                            });
                        });

                        var isviewchannel = data.msg.Isviewchannel;
                        $("input:radio[name='viewchannel'][value='" + isviewchannel + "']").attr("checked", "checked");

                        $("input:radio[name='CrmIsAccurateToPerson'][value='" + data.msg.CrmIsAccurateToPerson + "']").attr("checked", "checked");
                        $("input:radio[name='OrderIsAccurateToPerson'][value='" + data.msg.OrderIsAccurateToPerson + "']").attr("checked", "checked");

                        $("input:radio[name='iscanverify'][value='" + data.msg.Iscanverify + "']").attr("checked", "checked");
                        $("input:radio[name='iscanset_imprest'][value='" + data.msg.iscanset_imprest + "']").attr("checked", "checked");
                        $("input:radio[name='iscanset_order'][value='" + data.msg.iscanset_order + "']").attr("checked", "checked");
                        $("input:radio[name='validateservertype'][value='" + data.msg.validateservertype + "']").attr("checked", "checked");
                        $("input:radio[name='canviewpro'][value='" + data.msg.canviewpro + "']").attr("checked", "checked");
                    }
                });
            } else {
                $("#groupname").removeAttr("readonly");
            }

            $("#btnedit").click(function () {

                EditGroup("list"); //操作成功后回到列表页面

            })

            $("#groupname").blur(function () {
                EditGroup("self"); //操作成功后在本页面刷新
            })

            function EditGroup(linktarget) {
                var groupname = $("#groupname").trimVal();
                var groupinfo = $("#groupinfo").trimVal();

                var groupids = "";
                $("input:checkbox[name='checkGrougId']:checked").each(function (i, item) {
                    groupids += ($(this).val() + ",");
                })
                if (groupids == "") {
                    //                    $.prompt("选择管理组为空");
                    //                    return;
                }
                else {
                    groupids = groupids.substring(0, groupids.length - 1);
                }

                var isviewchannel = $("input:radio[name='viewchannel']:checked").trimVal();
                var CrmIsAccurateToPerson = $("input:radio[name='CrmIsAccurateToPerson']:checked").trimVal();
                var OrderIsAccurateToPerson = $("input:radio[name='OrderIsAccurateToPerson']:checked").trimVal();
                var iscanverify = $("input:radio[name='iscanverify']:checked").trimVal();

                var iscanset_imprest = $("input:radio[name='iscanset_imprest']:checked").trimVal();

                var iscanset_order = $("input:radio[name='iscanset_order']:checked").trimVal();
                var validateservertype = $("input:radio[name='validateservertype']:checked").trimVal();
                var canviewpro = $("input:radio[name='canviewpro']:checked").trimVal();

                if (groupname == "") {
                    $.prompt("管理组信息不可为空", { button: [{ title: "确定", value: true}], show: "slideDown", submit: function (e, v, m, f) { if (v == true) { } } });
                    return;
                }
                else {
                    $.post("/JsonFactory/PermissionHandler.ashx?oper=EditGroup", { groupid: groupid, groupname: groupname, groupinfo: groupinfo, groupids: groupids, isviewchannel: isviewchannel, CrmIsAccurateToPerson: CrmIsAccurateToPerson, iscanverify: iscanverify, iscanset_imprest: iscanset_imprest, OrderIsAccurateToPerson: OrderIsAccurateToPerson, iscanset_order: iscanset_order, validateservertype: validateservertype,canviewpro:canviewpro }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("编辑操作出现错误");
                            return;
                        }
                        if (data.type == 100) {
                            if (linktarget == "list") {
                                $.prompt("管理组编辑成功", { button: [{ title: "确定", value: true}], show: "slideDown", submit: function (e, v, m, f) { if (v == true) { window.open("rolelist.aspx", target = "_self") } } });
                                return;
                            }
                            else {
                                window.open("rolemanager.aspx?groupid=" + data.msg, target = "_self")
                                return;
                            }
                        }
                    });
                }
            }
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li class="on"><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>角色管理</span></a></li>
                <%--<li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>--%>
            </ul>
        </div>
    </div>
    <div id="setting-home">
        <div>
            <h3>
                管理组基本信息</h3>
            <table border="0">
                <tr>
                    <td style="width: 200px; height: 20px;">
                        管理组名称:
                    </td>
                    <td>
                        <input type="text" id="groupname" value="" size="50" />(管理组名称一旦设定，无法再次修改)
                    </td>
                </tr>
                <tr>
                    <td>
                        管理组描述:
                    </td>
                    <td>
                        <input type="text" id="groupinfo" value="" size="50" />
                    </td>
                </tr>
                <tr>
                    <td>
                        员工管理时显示的分组:
                    </td>
                    <td id="tdgroups">
                    </td>
                </tr>
                <tr>
                    <td>
                        员工管理时是否显示渠道信息:
                    </td>
                    <td>
                        <label>
                            <input type="radio" id="notview" name="viewchannel" value="false" checked="checked" />不显示
                        </label>
                        <label>
                            <input type="radio" id="view" name="viewchannel" value="true" />显 示
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        会员管理是否精确到渠道人:
                    </td>
                    <td>
                        <label>
                            <input type="radio" id="Radio1" name="CrmIsAccurateToPerson" value="false" checked="checked" />否
                        </label>
                        <label>
                            <input type="radio" id="Radio2" name="CrmIsAccurateToPerson" value="true" />是
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        订单管理是否精确到渠道人:
                    </td>
                    <td>
                        <label>
                            <input type="radio" id="Radio7" name="OrderIsAccurateToPerson" value="0" checked="checked" />否
                        </label>
                        <label>
                            <input type="radio" id="Radio8" name="OrderIsAccurateToPerson" value="1" />是
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        是否可以验电子码和验会员卡:
                    </td>
                    <td>
                        <label>
                            <input type="radio" id="Radio3" name="iscanverify" value="1" checked="checked" />可以
                        </label>
                        <label>
                            <input type="radio" id="Radio4" name="iscanverify" value="0" />不可以
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        是否可以设置会员/分销预付款:
                    </td>
                    <td>
                        <label>
                            <input type="radio" id="Radio5" name="iscanset_imprest" value="1" checked="checked" />可以
                        </label>
                        <label>
                            <input type="radio" id="Radio6" name="iscanset_imprest" value="0" />不可以
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        是否可以操作订单:
                    </td>
                    <td>
                        <label>
                            <input type="radio" id="Radio9" name="iscanset_order" value="1" checked="checked" />可以
                        </label>
                        <label>
                            <input type="radio" id="Radio10" name="iscanset_order" value="0" />不可以
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        验证电子票的产品服务类型:
                    </td>
                    <td>
                        <label>
                            <input type="radio" id="Radio11" name="validateservertype" value="0" checked="checked" />所有
                        </label>
                        <label>
                            <input type="radio" id="Radio12" name="validateservertype" value="10" />旅游大巴
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        看到产品范围:
                    </td>
                    <td>
                        <label>
                            <input type="radio" id="Radio13" name="canviewpro" value="0" />商户全部产品
                        </label>
                        <label>
                            <input type="radio" id="Radio14" name="canviewpro" value="1" checked="checked" />自己发布的产品
                        </label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" id="btnedit" value="提交" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <input type="hidden" id="hid_groupid" value="<%=groupid %>" />
</asp:Content>
