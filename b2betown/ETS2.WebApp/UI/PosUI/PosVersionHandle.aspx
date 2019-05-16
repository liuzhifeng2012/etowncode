<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/ui/etown.master" CodeBehind="PosVersionHandle.aspx.cs"
    Inherits="ETS2.WebApp.UI.PosUI.PosVersionHandle" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        $(function () {
            var posid = $("#hid_posid").trimVal();
            if (posid != "") {
                $("#opertype").val("修改pos版本更新");
                $("#posid").val(posid);
                $("#posid").attr("readonly", "readonly");
               
            } else {
                $("#opertype").val("添加pos版本更新");
                $("#posid").removeAttr("readonly");
            }

            $("#confirmpub").click(function () {
                var updateurl = $("#updateurl").trimVal();
                if (updateurl == "") {
                    $.prompt("更新文件路径需要填写");
                    return;
                }

                if (confirm("请确认更新文件路径正确并且路径下包含需要的文件")) {
                    $.ajax({
                        url: "/JsonFactory/PosVersionHandler.ashx?oper=updateposversion",
                        data: {posid:posid},
                        type: "post",
                        async: false,
                        success: function (data) {

                        }
                    })
                }
                else {
                    return;
                }

            })
        })  
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="body">
    <div id="settings" class="view main">
        <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="ProductList.aspx" onfocus="this.blur()" target=""><span>产品管理</span></a></li>
                <li class="on"><a href="ProductAdd.aspx" onfocus="this.blur()" target=""><span>
                    添加产品</span></a></li>
            </ul>
        </div>--%>
        <input type="hidden" id="hid_posid" value='<%=posid %>' />
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3 id="opertype">
                </h3>
                <table class="grid">
                    <tr>
                        <td width="13%" class="tdHead">
                            posid：
                        </td>
                        <td width="87%">
                            <input type="text" id="posid" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            更新文件所在路径：
                        </td>
                        <td>
                            <input type="text" id="updateurl" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            更新类型：
                        </td>
                        <td>
                            <select id="updatetype" runat="server" class="selector" style="min-width: 100px;">
                                <option value="1">不需更新</option>
                                <option value="2">exe更新</option>
                                <option value="3">xml更新</option>
                                <option value="4">exe和xml同时更新</option>
                            </select>
                        </td>
                    </tr>
                </table>
                <table width="780" border="0">
                    <tr>
                        <td width="699" height="80" align="center">
                            <a href="#" id="confirmpub">确 认</a>
                        </td>
                        <td width="59">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
