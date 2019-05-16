<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthorFocus.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.WeiXin.AuthorFocus" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            //根据公司id获得关注作者信息
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompany", { comid: $("#hid_comid").val() }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    $("#author").val(dat.msg.B2bcompanyinfo.Wxfocus_author);
                    $("#url").val(dat.msg.B2bcompanyinfo.Wxfocus_url);
                }
            });

            $("#button1").click(function () {
                var author = $("#author").val();
                var url = $("#url").val();
                if (author == "") {
                    alert("关注作者不可为空");
                    return;
                }
                if (url == "") {
                    alert("关注链接不可为空");
                    return;
                }

                $.post("/JsonFactory/AccountInfo.ashx?oper=editwxauthorfocus", { comid: $("#hid_comid").val(), author: author, url: url }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert("编辑失败");
                        return;
                    }
                    if (data.type == 100) {
                        alert("编辑成功");
                        return;
                    }
                })

            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="materiallist.aspx" onfocus="this.blur()"><span>文章列表</span></a></li>

                <li class="on"><a href="AuthorFocus.aspx" onfocus="this.blur()"><span>关注作者管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        关注作者管理</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            关注作者</label>
                        <input type="text" id="author" value="" class="mi-input" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            作者关注链接</label>
                        <input type="text" id="url" value="" class="mi-input" style="width: 420px;" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            请参考下面内容在微信公众账号（mp.weixin.qq.com）添加素材文章，之后将该文章链接url复制到上面作者关注链接框中</label>
                    </div>
                    <div class="mi-form-item">
                        <img id="img1" src="/Images/wxauthorfocus.png" />
                    </div>
                </div>
                <table border="0" width="780" class="grid">
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button1" value="  确认提交  " class="mi-input" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
