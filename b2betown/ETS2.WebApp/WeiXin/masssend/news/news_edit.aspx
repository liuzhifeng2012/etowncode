<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="news_edit.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.WeiXin.masssend.news.news_edit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var createuserid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            //            alert(jmz.GetLength('测试测试ceshiceshi'));
            //标题，封面图片，内容 不可为空
            $("#btnsave").click(function () {

                var thumb_media_id = $("#" + "<%=hid_thumb_media_id.ClientID %>").trimVal();
                if (thumb_media_id == "") {
                    alert("封面图片必须上传!");
                    return;
                }
                var txttitle = $("#txttitle").trimVal();
                if (txttitle == "") {
                    alert("标题不可为空!");
                    return;
                }
                else {
                    if (parseInt(GetLength(txttitle)) > 128) {
                        alert("标题字数不可超过64字");
                        return;
                    }
                }
                var thumb_url = $("#" + "<%=Image1.ClientID %>").attr("src");

            
                var txtauthor = $("#txtauthor").trimVal();
                if(txtauthor!="")
                {
                    if (parseInt(GetLength(txtauthor)) > 16) {
                        alert("作者字数不可超过8字");
                        return;
                    }
                }
                var txtdigest = $("#txtdigest").trimVal();
                if (txtdigest != "") {
                    if (parseInt(GetLength(txtdigest)) > 240) {
                        alert("摘要字数不可超过120字");
                        return;
                    }
                }
                var txtcontent = $("#txtcontent").val();
                if (txtcontent == "") {
                    alert("图文消息内容不可为空");
                    return;
                }
                var sel_show_cover_pic = $("#sel_show_cover_pic").val();
                var txtcontent_source_url = $("#txtcontent_source_url").val();

                $.post("/JsonFactory/WeiXinHandler.ashx?oper=uploadwxqunfa_news", { createuserid: createuserid, comid: comid, txttitle: txttitle, txtauthor: txtauthor, txtdigest: txtdigest, thumb_media_id: thumb_media_id, sel_show_cover_pic: sel_show_cover_pic, txtcontent: txtcontent, txtcontent_source_url: txtcontent_source_url, thumb_url: thumb_url, materialid: $("#hid_materialid").val() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        alert("添加图文消息成功");
                        window.open("news_list.aspx", target = "_self");
                    }
                })
            })

            $('#txtcontent').tinymce({
                // Location of TinyMCE script
                script_url: '/Scripts/tiny_mce/tiny_mce.js',
                width: '622',
                height: '320',
                // General options
                theme: "advanced",
                language: 'cn',
                plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,advlist",

                // Theme options
                theme_advanced_buttons1: "",
                //                theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,insertdate,image,preview",
                theme_advanced_buttons2: "",
                theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,ltr,rtl",
                // theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,|,nonbreaking,template,pagebreak",
                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left",
                theme_advanced_statusbar_location: "bottom",
                //theme_advanced_resizing: true,
                template_external_list_url: "lists/template_list.js",
                external_link_list_url: "lists/link_list.js",
                external_image_list_url: "lists/image_list.js",
                media_external_list_url: "lists/media_list.js",

                // Replace values for the template plugin
                template_replace_values: {
                    username: "Some User",
                    staffid: "991234"
                }
            });
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/weixin/masssend/send.aspx" onfocus="this.blur()"><span>新建群发消息</span></a></li>
                <li><a href="/weixin/masssend/list.aspx" onfocus="this.blur()">已发送</a></li>
                <li class="on"><a href="news_list.aspx" onfocus="this.blur()">图文信息管理</a> </li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div style="width: 600px; height: 40px; padding-left: 10px; border: 1px solid #FF4444;
                    display: none;" id="viewrenzheng">
                    <div style="padding: 12px 0 14px 0;">
                        <h5 class="text-overflow">
                            <a smartracker="on" seed="contentList-mainLinkbox" href="http://shop1143.etown.cn/v/about.aspx?id=2661"
                                target="_blank" style="font-size: 16px">该功能需将微信服务号进行认证后才能使用 ,点击查看如何进行认证... </a>
                        </h5>
                    </div>
                    <div class="content-list-des text-overflow">
                    </div>
                </div>
                <h3>
                </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        新建单图文消息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            封面</label>
                        <asp:Image ID="Image1" runat="server" ImageUrl="" /><br />
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="432px" />
                         <asp:Label ID="lb_info" runat="server" ForeColor="Red" Width="515px">图片只支持jpg格式</asp:Label><br />
                        <asp:Button ID="bt_upload" runat="server" Text="上传图片" OnClick="bt_upload_Click" />
                        <asp:HiddenField ID="hid_thumb_media_id" runat="server" />
                       
                    </div>
                    <div class="mi-form-item" style="display: none">
                        <label class="mi-label">
                            是否显示封面</label>
                        <select id="sel_show_cover_pic" class="mi-input">
                            <option value="1">是</option>
                            <option value="0">否</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            标题(64字以内)</label>
                        <input type="text" id="txttitle" value="" size="50" />* <span id="tdchildobj"></span>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            作者(8字以内)</label>
                        <input type="text" id="txtauthor" value="" size="50" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            摘要(120字以内)</label>
                        <input type="text" id="txtdigest" value="" size="50" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            内容</label>
                        <textarea style="width: 400px;" cols="200" rows="4" id="txtcontent" placeholder=""
                            class="mi-input"></textarea>*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            原文链接</label>
                        <input type="text" id="txtcontent_source_url" value="" size="50" />
                    </div>
                    <table border="0">
                        <tbody>
                            <tr>
                                <td width="600" height="80" align="center">
                                    <input type="hidden" id="hid_materialid" value="<%=materialid %>" />
                                    <input type="button" class="mi-input" value="  保存  " id="btnsave" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
</asp:Content>
