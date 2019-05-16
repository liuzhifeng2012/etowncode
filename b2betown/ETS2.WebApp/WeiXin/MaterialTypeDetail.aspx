<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="MaterialTypeDetail.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.MaterialTypeDetail" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var id = $("#hid_id").trimVal();
            if (id != "0")//编辑素材类型
            {
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=getmaterialtype", { id: id, comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("查询微信素材类型列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#typeid").val(data.msg.Id);
                        $("#typename").val(data.msg.Typename);
                        $("#seltypeclass").val(data.msg.Typeclass);

                        var isshow = 'true';
                        if (data.msg.Isshowpast == false) {
                            isshow = 'false';
                        }
                        $("#selisshowpast").val(isshow);
                    }
                })
            } else {
                $("#typeid").val(id);
            }

            $("#aedit").click(function () {
                var typename = $("#typename").trimVal();
                if (typename == "") {
                    $.prompt("类型名称不可为空");
                    return;
                }

                var typeclass = $("#seltypeclass").val();

                $.post("/JsonFactory/WeiXinHandler.ashx?oper=editmaterialtype", { isshowpast: $("#selisshowpast").val(), comid: $("#hid_comid").trimVal(), id: id, typename: typename, typeclass: typeclass }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("操作成功", { buttons: [{ title: "确 定", value: true}], show: "slideDown", submit: function (m, e, v, f) {
                            if (e == true) {
                                window.open("MaterialTypeList.aspx", target = "_self");
                            }
                        }
                        })
                    }
                })
            })

        })
      
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="materiallist.aspx" onfocus="this.blur()"><span>文章列表</span></a></li>
                <li class="on"><a href="MaterialTypeList.aspx" onfocus="this.blur()">添加文章类型</a></li>
                <li><a href="periodical.aspx" onfocus="this.blur()"><span>文章期号管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    编辑微信素材类型</h3>
                <h3>
                    &nbsp;</h3>
                <table class="grid">
                    <tr id="tr_parentmenu" style="display: none;">
                        <td width="19%" class="tdHead">
                            素材类型id：
                        </td>
                        <td width="81%">
                            <input type="text" id="typeid" value="" size="50" readonly />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            素材类型名称：
                        </td>
                        <td width="81%">
                            <input type="text" id="typename" value="" size="50" />*
                        </td>
                    </tr>
                    <tr id="tr_materialtypeid" style="display: none;">
                        <td class="tdHead">
                            素材类型分类：
                        </td>
                        <td>
                            <select id="seltypeclass">
                                <option value="detail">详 情</option>
                                <option value="book">预 订</option>
                            </select>
                        </td>
                    </tr>
                    <tr id="tr1">
                        <td class="tdHead">
                            是否显示往期：
                        </td>
                        <td>
                            <select id="selisshowpast">
                                <option value="true">显 示</option>
                                <option value="false">不显示</option>
                            </select>
                        </td>
                    </tr>
                </table>
                <p>
                    &nbsp;</p>
                <p>
                    &nbsp;</p>
                <p align="center">
                    <a href="javascript:void(0)" id="aedit" class="font_14"><strong>完成添加，确认提交</strong></a></p>
                <p>
                    &nbsp;</p>
                <h3>
                    &nbsp;</h3>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_id" value="<%=id %>" />
</asp:Content>
