<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master"
    CodeBehind="bangdingprint.aspx.cs" Inherits="ETS2.WebApp.UI.UserUI.bangdingprint" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            //获取商家绑定打印机信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompany", { comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取信息出错，" + data.msg);
                }
                if (data.type == 100) {
                    $("#Defaultprint").val(data.msg.B2bcompanyinfo.Defaultprint);
                    $("#hid_comextid").val(data.msg.B2bcompanyinfo.Id);
                }
            })
            $("#button1").click(function () {

                var Defaultprint = $("#Defaultprint").trimVal();
                var comextid = $("#hid_comextid").trimVal();
               
                if (Defaultprint == "") {
                    alert("请输入打印机名称");
                    return;
                }  
                $.post("/JsonFactory/AccountInfo.ashx?oper=editbangprint", { Defaultprint: Defaultprint, comextid: comextid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("绑定打印机信息出错");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("绑定打印机成功");
                        return;
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
                <%-- <li><a href="/Account/ChangePassword.aspx" target="this.blur()" title="">修改登陆密码</a></li>
                <li class="on"><a href="bangdingprint.aspx" onFocus="" target="">
                    小票打印机绑定</a></li>
                <li><a href="bangdingpos.aspx" onFocus="this.blur()" target="">Pos终端绑定</a></li>
                <li><a href="/ui/userui/notelist.aspx" onfocus="this.blur()" target="">短信设置</a></li>
                <li><a href="/Account/AccountManager.aspx" onfocus="this.blur()" target=""><span>账户管理</span></a></li>--%>
                <li class="on"><a href="/ui/userui/bangdingprint.aspx" onfocus="this.blur()" target="">
                    <span>绑定打印机</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    小票打印机绑定</h3>
                <table border="0" class="grid">
                    <tr>
                        <td height="80" align="center">
                            <p>
                                打印机名称&nbsp;
                                <input name="Defaultprint" type="text" id="Defaultprint" />
                                &nbsp;
                                <%if (iscanbindprint == 1)
                                  {//只有管理员,商户负责人（含实体卡）,商户负责人 , 景区负责人（含分销系统） 可以绑定打印机
                                %>
                                <input type="button" name="button" id="button1" value="  绑定打印机  " />
                                <%
                                    }
                                  else
                                  {%>
                                <input type="button" name="button" id="button1" value="  绑定打印机  " disabled="disabled" />
                                <%
                                    }%>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td height="80" align="left">
                            <p>
                                1、<a href="/soft/20120615133953197.rar" target="_blank">58小票打印机驱动程序（可根据您自己安装的打印机安装相应的驱动程序）</a>
                            </p>
                            <p>
                                1、<a href="/soft/80xprinterdriver.rar" target="_blank">80小票打印机驱动程序（可根据您自己安装的打印机安装相应的驱动程序）</a>
                            </p>
                            <p>
                                2、<a href="/soft/lodop.rar" target="_blank">页面打印插件</a>（当提示未安装插件或第一次使用验证请安装此打印插件，建议使用IE，或360浏览器验证）</p>
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
    </div>
    <input type="hidden" id="hid_comextid" value="" />
    <div class="data">
    </div>
</asp:Content>
