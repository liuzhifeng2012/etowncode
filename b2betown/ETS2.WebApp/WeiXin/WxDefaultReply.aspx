<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WxDefaultReply.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.WxDefaultReply" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();


            //获取公司的微信公众平台信息
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxbasic", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取信息失败");
                    return;
                }
                if (data.type == 100) {

                    if (data.msg == null) {
                        $.prompt("请先补全第一步信息!", {
                            submit: function () {
                                location.href = "shangjiaset.aspx";
                            }
                        });

                    } else {
                        $("#hid_weixinbasicid").val(data.msg.Id);

                        $("#txtattentionautoreply").val(data.msg.Attentionautoreply);
                        $("#txtleavemsgautoreply").val(data.msg.Leavemsgautoreply);
                    }

                }

            });

            $("#button1").click(function () {
                var wxbasicid = $("#hid_weixinbasicid").trimVal();
                if (wxbasicid == 0) {
                    $.prompt("请先补全第一步信息!", {
                        submit: function () {
                            location.href = "shangjiaset.aspx";
                        }
                    });
                }
                var attentionreply = $("#txtattentionautoreply").trimVal();
                var leavemsgreply = $("#txtleavemsgautoreply").trimVal();
//                //对输入内容进行处理 去掉单，双引号，斜杠，反斜杠
//                attentionreply = attentionreply.replace(/\'/g, "").replace(/\"/g, "").replace(/\//g, "").replace(/\\/g, "");
//                leavemsgreply = leavemsgreply.replace(/\'/g, "").replace(/\"/g, "").replace(/\//g, "").replace(/\\/g, "");



                if (attentionreply == "" || leavemsgreply == "") {
                    $.prompt("请把信息补充完善");
                    return;
                }
                else {
                    $.post("/JsonFactory/WeiXinHandler.ashx?oper=editwxbasicreply", { wxbasicid: wxbasicid, attentionreply: attentionreply, leavemsgreply: leavemsgreply }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("编辑操作失败");
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("操作成功");
                        }
                    });
                }

            })


            $('#txtattentionautoreply').tinymce({
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
            $('#txtleavemsgautoreply').tinymce({
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
                <li><a href="/weixin/ShangJiaSet2.aspx" onfocus="this.blur()"><span>微信接口设置</span></a></li>
                <li class="on"><a href="/weixin/WxDefaultReply.aspx" onfocus="this.blur()"><span>微信默认设置</span></a></li>
                <li><a href="/weixin/menulist.aspx" onfocus="this.blur()"><span>自定义菜单管理</span></a></li>
                <li><a href="/MemberShipCard/MemberShipCardList.aspx" onfocus="this.blur()"><span>会员卡专区管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
            <h3></h3>
            <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
               <h2 class="p-title-area">微信端自动回复设置</h2>
               <div class="mi-form-item">
                    <label class="mi-label">第一次加关注后自动回复</label>
                   <textarea cols="60" rows="4" id="txtattentionautoreply" placeholder="第一次关注自动回复" class="mi-input"></textarea>
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">用户留言自动回复</label>
                    <textarea cols="60" rows="4" id="txtleavemsgautoreply" placeholder="微信用户留言没有匹配时自动回复" class="mi-input"></textarea>
               </div>

 <div class="mi-form-explain"></div>
</div>




                <table width="780" border="0" class="grid"  style=" margin-left:0px;">
                    <tr>
                        <td width="500" align="center">
                            <input type="button" id="button1" value="  确认  "  class="mi-input"/>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_weixinbasicid" value="0" />
</asp:Content>
